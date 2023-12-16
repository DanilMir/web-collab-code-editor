namespace SyncService.Services;

public class FilesService
{
    private readonly HttpClient _client;

    public FilesService(HttpClient client)
    {
        _client = client;
    }

    public void UploadFile(string projectId, string? prefix, string fileName, byte[] fileContent)
    {
        var stream = new MemoryStream(fileContent);
        
        var request = new
        {
            File = new FormFile(stream, 0, fileContent.Length, fileName, fileName),
            ProjectId = projectId,
            Prefix = prefix ?? string.Empty
        };
        var content = JsonContent.Create(request);
        _client.PostAsync($"/projects/{projectId}/files", content);
    }
}