namespace Sandbox.Services;

public interface IStorageService
{
    Task SaveFile(byte[] content, string file);
    Task EnsureCreatedDirectory(string path);
}