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

    [HttpPost]
    [Route("sync/callback")]
    public IActionResult Callback([FromBody] CallbackRequestModel callbackRequestModel)
    {
        var (projectId, prefix, fileName) = ParseRoom(callbackRequestModel.Room);
        _filesService.UploadFile(projectId, prefix, fileName, callbackRequestModel.Data.Monaco.Content);
        return Ok();
    }


    private static (string, string, string) ParseRoom(string room)
    {
        var (projectId, prefix, fileName) = room.Split(':') switch { var a => (a[0], a[1], a[2]) };
        return (projectId, prefix, fileName);
    }
}