using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace ProjectManagement.Services;

public class FilesService
{
    private readonly HttpClient _client;

    public FilesService(HttpClient client)
    {
        _client = client;
    }

    public async Task UploadFile(string projectId, string? prefix, string fileName, byte[] fileContent, string token)
    {
        using MultipartFormDataContent multipartContent = new();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        multipartContent.Add(new StreamContent(new MemoryStream(fileContent)), "file", fileName);
        multipartContent.Add(new StringContent(prefix ?? string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), "prefix");
        
        await _client.PostAsync($"/projects/{projectId}/files", multipartContent);
    }

    public async Task DeleteFolder(string projectId, string token)
    {
        using MultipartFormDataContent multipartContent = new();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        
        await _client.DeleteAsync($"/projects/{projectId}/files");
    }
    
    public async Task<IEnumerable<string>> GetProjectDirectoryTree(string projectId)
    {
        var response = await _client.GetAsync($"/projects/{projectId}/files");
        var list = JsonConvert.DeserializeObject<IEnumerable<string>>(response.Content.ToString() ?? string.Empty);
        return list ?? Enumerable.Empty<string>();
    }

    
    //todo: check status code, and task wait all
    public async Task CreatePythonTemplateProject(string projectId, string token)
    {
        await UploadFile(projectId, "", "main.py", new byte[]{}, token);
    }
}