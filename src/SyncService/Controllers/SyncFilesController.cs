using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SyncService.Models;
using SyncService.Services;

namespace SyncService.Controllers;

[ApiController]
public class SyncFilesController : Controller
{
    private readonly FilesService _filesService;

    public SyncFilesController(FilesService filesService)
    {
        _filesService = filesService;
    }

    // [HttpPost]
    // [DisableRequestSizeLimit]
    // [Route("sync/callback")]
    // public async Task<IActionResult> Callback()
    // {
    //     string bodyAsString;
    //     Request.EnableBuffering();
    //     using (var streamReader = new StreamReader(Request.Body, Encoding.UTF8))
    //     {
    //         bodyAsString = await streamReader.ReadToEndAsync();
    //     }
    //
    //
    //     Console.WriteLine(bodyAsString);
    //     
    //     // Console.WriteLine($"test: {callbackRequestModel.Room}");
    //     // var (projectId, prefix, fileName) = ParseRoom(callbackRequestModel.Room);
    //     // await _filesService.UploadFile(projectId, prefix, fileName, Encoding.ASCII.GetBytes(callbackRequestModel.Data.Monaco.Content));
    //     return Ok();
    // }
    
    private static (string, string, string) ParseRoom(string room)
    {
        var (projectId, prefix, fileName) = room.Split(':') switch { var a => (a[0], a[1], a[2]) };
        return (projectId, prefix, fileName);
    }
}