using System.Net.Mime;
using System.Text;

namespace SyncService.Services;

public class FilesService
{
    private readonly HttpClient _client;

    public FilesService(HttpClient client)
    {
        _client = client;
    }

    public async Task UploadFile(string projectId, string? prefix, string fileName, byte[] fileContent)
    {
        using MultipartFormDataContent multipartContent = new();
        
        multipartContent.Add(new StreamContent(new MemoryStream(fileContent)), "file", fileName);
        multipartContent.Add(new StringContent(prefix ?? string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), "prefix");
        
        await _client.PostAsync($"/projects/{projectId}/files", multipartContent);
    }
}