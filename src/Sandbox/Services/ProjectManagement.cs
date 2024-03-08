using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Sandbox.Models;

namespace Sandbox.Services;

public class ProjectManagement : IProjectManagement
{
    private readonly HttpClient _client;

    public ProjectManagement(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<Project> GetProject(string token, Guid id)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var response = await _client.GetAsync($"projects/{id}");
        response.EnsureSuccessStatusCode();
        await using var contentStream =
            await response.Content.ReadAsStreamAsync();
        var projectDto = await JsonSerializer.DeserializeAsync<ProjectDto>(contentStream);
        if (projectDto == null)
        {
            throw new Exception();
        }

        return new Project { ProgrammingLanguage = projectDto.ProgrammingLanguage };
    }
}