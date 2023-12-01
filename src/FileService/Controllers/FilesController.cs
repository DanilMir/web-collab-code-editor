using Amazon.S3;
using Amazon.S3.Model;
using FileService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers;


[Route("files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly AwsS3Service _s3Client;
    private readonly BucketsService _bucketsService;

    public FilesController(AwsS3Service s3Client, BucketsService bucketsService)
    {
        _s3Client = s3Client;
        _bucketsService = bucketsService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
    {
        var bucketExists = await _bucketsService.IsBucketExist(bucketName);
        if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
        var request = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
            InputStream = file.OpenReadStream()
        };
        request.Metadata.Add("Content-Type", file.ContentType);
        await _s3Client.PutObjectAsync(request);
        return Ok($"File {prefix}/{file.FileName} uploaded to S3 successfully!");
    }

    [HttpGet("get-by-key")]
    public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
    {
        var bucketExists = await _s3Client.DoesS3BucketExist(bucketName);
        if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
        var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
        return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
    }
}