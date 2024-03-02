namespace Sandbox.Services;

public class StorageService : IStorageService
{
    private readonly string _absolutePath;

    public StorageService(IConfiguration config)
    {
        _absolutePath = config["StoragePath"] ?? "-";
    }

    public async Task SaveFile(byte[] content, string file)
    {
        var filePath = $"{_absolutePath}{file}";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        string directoryPath = Path.GetDirectoryName(filePath) ?? "";
        await EnsureCreatedDirectory(directoryPath);
        
        await using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await fs.WriteAsync(content, 0, content.Length);
        }
    }

    public async Task EnsureCreatedDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}