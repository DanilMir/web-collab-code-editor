using System.Net.Http.Headers;

namespace FileService.Services;

public class ProjectsService
{
    private readonly HttpClient _client;

    public ProjectsService(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<bool> IsCurrentUserHaveAccess(string token, Guid id)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        var response = await _client.GetAsync($"projects/{id}");
        return response.IsSuccessStatusCode;
    }
}