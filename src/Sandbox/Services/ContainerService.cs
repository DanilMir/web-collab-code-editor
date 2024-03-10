using System.Net.NetworkInformation;
using Docker.DotNet;
using Docker.DotNet.Models;
using ICSharpCode.SharpZipLib.GZip;
using Sandbox.Models;
using ICSharpCode.SharpZipLib.Tar;


namespace Sandbox.Services;

public class ContainerService : IContainerService
{
    private readonly DockerClient _dockerClient = new DockerClientConfiguration().CreateClient();
    private readonly string _absolutePath;

    public ContainerService(IConfiguration config)
    {
        _absolutePath = config["StoragePath"] ?? "-";
    }

    public async Task<ContainerRunResult> RunContainer(Guid projectId)
    {
        var dockerFilePath = $"{_absolutePath}projects/{projectId}/Dockerfile.tar.gz";
        var imageName = projectId.ToString();
        var imageTag = "latest";

        var existsContainerPort = await GetPort(imageName);

        if (existsContainerPort != null)
        {
            return new ContainerRunResult {Port = (int)existsContainerPort};
        }

        await CreateTarGZ(dockerFilePath, $"{_absolutePath}projects/{projectId}/");
        
        var port = GetAvailablePort(2000);
        
        await BuildImage(dockerFilePath, imageName, imageTag, projectId.ToString());
        var createdContainer =  await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = imageName,
            Name = imageName,
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "6901", new List<PortBinding> { new() { HostPort = port.ToString()} } }
                },
            },
            ExposedPorts = new Dictionary<string, EmptyStruct>
            {
                { "6901", new EmptyStruct() }
            }
        });
        
        await _dockerClient.Containers.StartContainerAsync(
            createdContainer.ID,
            new ContainerStartParameters()
        );
        
        return new ContainerRunResult {Port = port};
    }

    private async Task BuildImage(string dockerFilePath, string imageName, string imageTag, string projectId)
    {
        var parameters = new ImageBuildParameters
        {
            Dockerfile = Path.GetFileName($"{_absolutePath}projects/{projectId}/"),
            Tags = new[] { $"{imageName}:{imageTag}" },
        };


        await using (var fileStream = File.OpenRead(dockerFilePath))
        {
            var progressReporter = new Progress<JSONMessage>();
            progressReporter.ProgressChanged += (sender, message) =>
            {
                if (message.Error != null)
                {
                    Console.WriteLine(message.ErrorMessage);
                }
            };

            await _dockerClient.Images.BuildImageFromDockerfileAsync(parameters, fileStream, null, null,
                progressReporter);
        }
    }

    private async Task<int?> GetPort(string name)
    {
        var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true });
        var container = containers.FirstOrDefault(c => c.Names.Contains($"/{name}"));
        if (container == null)
        {
            return null;
        }

        var port = container.Ports.FirstOrDefault(item => item.PrivatePort == 6901).PublicPort;
        
        return port;
    }
    
    public async Task<List<ContainerListResponse>> GetContainersAsync()
    {
        IList<ContainerListResponse> containers =
            await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters());
        return containers.ToList();
    }

    private int GetAvailablePort(int startingPort)
    {
        var properties = IPGlobalProperties.GetIPGlobalProperties();

        //getting active connections
        var tcpConnectionPorts = properties.GetActiveTcpConnections()
            .Where(n => n.LocalEndPoint.Port >= startingPort)
            .Select(n => n.LocalEndPoint.Port);

        //getting active tcp listners - WCF service listening in tcp
        var tcpListenerPorts = properties.GetActiveTcpListeners()
            .Where(n => n.Port >= startingPort)
            .Select(n => n.Port);

        //getting active udp listeners
        var udpListenerPorts = properties.GetActiveUdpListeners()
            .Where(n => n.Port >= startingPort)
            .Select(n => n.Port);

        var port = Enumerable.Range(startingPort, ushort.MaxValue)
            .Where(i => !tcpConnectionPorts.Contains(i))
            .Where(i => !tcpListenerPorts.Contains(i))
            .Where(i => !udpListenerPorts.Contains(i))
            .FirstOrDefault();

        return port;
    }

    private async Task CreateTarGZ(string tgzFilename, string sourceDirectory)
    {
        await using (var fileStream = new FileStream(tgzFilename, FileMode.Create))
        await using (var gzipStream = new GZipOutputStream(fileStream))
        using (var tarArchive = TarArchive.CreateOutputTarArchive(gzipStream))
        {
            AddDirectoryFilesToTar(sourceDirectory, tarArchive, sourceDirectory);
        }
    }
    
    private void AddDirectoryFilesToTar(string sourceDirectory, TarArchive tarArchive, string rootDirectory)
    {
        var files = Directory.GetFiles(sourceDirectory);
        string[] directories = Directory.GetDirectories(sourceDirectory);
        
        foreach (string file in files)
        {
            string relativePath = file.Substring(rootDirectory.Length);
            if(relativePath == "Dockerfile.tar.gz")
                continue;
            TarEntry tarEntry = TarEntry.CreateEntryFromFile(file);
            tarEntry.Name = relativePath;
            tarArchive.WriteEntry(tarEntry, true);
        }

        // Рекурсивно добавляем файлы из подпапок
        foreach (string directory in directories)
        {
            AddDirectoryFilesToTar(directory, tarArchive, rootDirectory);
        }
    }
}