using Amazon.S3;
using Amazon.S3.Model;
using FileService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers;

[Route("projects/{projectId}/files")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly string _projectsBucketName;

    private readonly AwsS3Service _s3Client;
    private readonly BucketsService _bucketsService;
    private readonly ProjectsService _projectsService;

    public ProjectsController(AwsS3Service s3Client, BucketsService bucketsService, IConfiguration config, ProjectsService projectsService)
    {
        _s3Client = s3Client;
        _bucketsService = bucketsService;
        _projectsService = projectsService;
        _projectsBucketName = config["ProjectsBucketName"] ?? string.Empty;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, string projectId, string? prefix)
    {
        // var accessToken = HttpContext.Request.Headers["Authorization"].ToString()[7..];
        // if (await _projectsService.IsCurrentUserHaveAccess(accessToken, Guid.Parse(projectId)))
        // {
        //     return Forbid();
        // }
        
        prefix = prefix is null ? projectId : $"{projectId}/{prefix.TrimEnd('/')}";
        var bucketExists = await _bucketsService.IsBucketExist(_projectsBucketName);
        if (!bucketExists) return NotFound($"Bucket {_projectsBucketName} does not exist.");
        var request = new PutObjectRequest()
        {
            BucketName = _projectsBucketName,
            Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
            InputStream = file.OpenReadStream()
        };
        request.Metadata.Add("Content-Type", file.ContentType);
        await _s3Client.PutObjectAsync(request);
        return Ok($"File {file.FileName} uploaded to S3 successfully!");
    }

    [HttpGet("getFile")]
    public async Task<IActionResult> GetFileByKeyAsync(string projectId, string fileId)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString()[7..];
        if (await _projectsService.IsCurrentUserHaveAccess(accessToken, Guid.Parse(projectId)))
        {
            return Forbid();
        }
        fileId = $"{projectId}/{Uri.UnescapeDataString(fileId)}";
        
        try
        {
            var bucketExists = await _s3Client.DoesS3BucketExist(_projectsBucketName);
            if (!bucketExists)
            {
                return NotFound($"Bucket {_projectsBucketName} does not exist.");
            }

            var request = new GetObjectRequest
            {
                BucketName = _projectsBucketName,
                Key = fileId
            };

            try
            {
                var s3Object = await _s3Client.GetObjectAsync(request);
                return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
            }
            catch (AmazonS3Exception ex)
            {
                return NotFound($"File with ID {fileId} not found in bucket {_projectsBucketName}.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles(string projectId, string? prefix)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString()[7..];
        if (await _projectsService.IsCurrentUserHaveAccess(accessToken, Guid.Parse(projectId)))
        {
            return Forbid();
        }
        
        prefix = prefix is null ? projectId : $"{projectId}/{prefix.TrimEnd('/')}";

        var temp =await ListObjects(projectId, prefix).ToListAsync();
        var result = temp.SelectMany(list => list);
        
        return Ok(result);
    }

    [HttpDelete("{fileId}")]
    public async Task<IActionResult> DeleteFile(string projectId, string fileId)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString()[7..];
        if (await _projectsService.IsCurrentUserHaveAccess(accessToken, Guid.Parse(projectId)))
        {
            return Forbid();
        }
        
        fileId = $"{projectId}/{Uri.UnescapeDataString(fileId)}";
        
        var deleteRequest = new DeleteObjectRequest
        {
            Key = fileId,
            BucketName = _projectsBucketName,
        };
        await _s3Client.DeleteObjectAsync(deleteRequest);
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteFolder(string projectId, string folderKey)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString()[7..];
        if (await _projectsService.IsCurrentUserHaveAccess(accessToken, Guid.Parse(projectId)))
        {
            return Forbid();
        }
        
        var listObjectsRequest = new ListObjectsV2Request
        {
            BucketName = _projectsBucketName,
            Prefix = folderKey
        };
        
        var listObjectsResponse = await _s3Client.ListObjectsV2Async(listObjectsRequest);

        foreach (var s3Object in listObjectsResponse.S3Objects)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _projectsBucketName,
                Key = s3Object.Key
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        return Ok();
    }

    private async IAsyncEnumerable<IEnumerable<string>> ListObjects(string projectId, string? prefix)
    {
        var objectsV2Request = new ListObjectsV2Request
        {
            BucketName = _projectsBucketName,
            Prefix = prefix
        };
        ListObjectsV2Response listObjectsV2Response;
        do
        {
            listObjectsV2Response = await _s3Client.ListObjectsV2Async(objectsV2Request);
            objectsV2Request.ContinuationToken = listObjectsV2Response.NextContinuationToken;
            yield return listObjectsV2Response.S3Objects.Select(s => s.Key.Replace($"{projectId}/", ""));
        } while (listObjectsV2Response.IsTruncated);
    }
}