using Amazon.S3;
using Amazon.S3.Model;
using FileService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers;


[Route("buckets/{bucketName}/files")]
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

    [HttpPost]
    public async Task<IActionResult> UploadFileAsync(string bucketName, IFormFile file, string? prefix)
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
    
    [HttpGet("{fileId}")]
    public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string fileId)
    {
        fileId = Uri.UnescapeDataString(fileId);
        
        try
        {
            var bucketExists = await _s3Client.DoesS3BucketExist(bucketName);
            if (!bucketExists)
            {
                return NotFound($"Bucket {bucketName} does not exist.");
            }

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileId
            };

            try
            {
                var s3Object = await _s3Client.GetObjectAsync(request);
                return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
            }
            catch (AmazonS3Exception ex)
            {
                // В лог можно записать подробности об ошибке
                // logger.LogError($"Error getting object from S3. AWS Error Code: {ex.ErrorCode}. Message: {ex.Message}");
                return NotFound($"File with ID {fileId} not found in bucket {bucketName}.");
            }
        }
        catch (Exception ex)
        {
            // В лог можно записать подробности об ошибке
            // logger.LogError($"Unexpected error: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet]
    public IActionResult GetFiles(string bucketName, string? prefix)
    {
        return Ok(ListObjects(bucketName, prefix).ToListAsync());;
    }

    [HttpDelete("{fileId}")]
    public async Task<IActionResult> DeleteFile(string bucketName, string fileId)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            
            Key = fileId,
            BucketName = bucketName,

        };
        await _s3Client.DeleteObjectAsync(deleteRequest);
        return Ok();
    }
    
    private async IAsyncEnumerable<IEnumerable<string>> ListObjects(string bucketName, string? prefix)
    {
        var objectsV2Request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = prefix
        };
        ListObjectsV2Response listObjectsV2Response;
        do
        {
            listObjectsV2Response = await _s3Client.ListObjectsV2Async(objectsV2Request);
            objectsV2Request.ContinuationToken = listObjectsV2Response.NextContinuationToken;
            yield return listObjectsV2Response.S3Objects.Select(s => s.Key);
        } while (listObjectsV2Response.IsTruncated);
    }
}