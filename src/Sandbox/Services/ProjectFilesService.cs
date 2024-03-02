using System.Net.Http.Headers;
using System.Text.Json;
using Sandbox.Models;

namespace Sandbox.Services;

public class ProjectFilesService : IProjectFilesService
{
    private readonly HttpClient _client;
    private readonly IStorageService _storage;
    private const string Path = "";
    
    public ProjectFilesService(HttpClient client, IStorageService storage)
    {
        _client = client;
        _storage = storage;
    }
    
    private async Task<IEnumerable<string>> GetFilesPaths(Guid projectId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        var response = await _client.GetAsync($"projects/{projectId}/files");
        await using var contentStream =
            await response.Content.ReadAsStreamAsync();
        var projectPaths = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(contentStream);
        
        if (projectPaths == null)
        {
            throw new Exception("Paths is incorrect");
        }
        
        return projectPaths;
    }

    private async Task<byte[]> DownloadFile(Guid projectId, string file, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        var response = await _client.GetAsync($"projects/{projectId}/files/getFile/?fileId={file}");
        await using var contentStream =
            await response.Content.ReadAsStreamAsync();
        
        byte[] bytes = new byte[contentStream.Length];
        contentStream.Read(bytes, 0, bytes.Length);
        
        return bytes;
    }
    
    public async Task DownloadProject(Guid projectId, string token)
    {
        var files = await GetFilesPaths(projectId, token);
        foreach (var file in files)
        {
            var content = await DownloadFile(projectId, file, token);
            await _storage.SaveFile(content, $"projects/{projectId}/{file}");
        }
    }
}