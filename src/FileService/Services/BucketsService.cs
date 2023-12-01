using Amazon.S3;
using Amazon.S3.Util;

namespace FileService.Services;

public class BucketsService
{
    private readonly AwsS3Service _s3Client;
    
    public BucketsService(AwsS3Service s3Client)
    {
        _s3Client = s3Client;
    }

    // public async Task CreateBucket(string bucketName)
    // {
    //     var bucketExists = await _s3Client.DoesS3BucketExist(bucketName);
    //     if (bucketExists) throw new Exception($"Bucket {bucketName} already exists");
    //     await _s3Client.PutBucketAsync(bucketName);
    // }
    
    public async Task EnsureBucketIsCreated(string bucketName)
    {
        var bucketExists = await _s3Client.DoesS3BucketExist(bucketName);
        if (!bucketExists)
            await _s3Client.PutBucketAsync(bucketName);
    }

    public async Task<bool> IsBucketExist(string bucketName)
    {
        return await _s3Client.DoesS3BucketExist(bucketName);
    }
}