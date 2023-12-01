using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace FileService.Services;

public class AwsS3Service
{
    private readonly IAmazonS3 _client;

    public AwsS3Service(IConfiguration config)
    {
        var configsS3 = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(config["AWS:Region"] ?? String.Empty),
            ServiceURL = config["AWS:ServiceURL"] ?? String.Empty,
            ForcePathStyle = true
        };
        _client = new AmazonS3Client(
            config["AWS:AccessKey"] ?? String.Empty,
            config["AWS:SecretKey"] ?? String.Empty,
            configsS3);
    }

    public async Task<bool> DoesS3BucketExist(string bucketName) =>
        await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName);

    public async Task PutBucketAsync(string bucketName) => await _client.PutBucketAsync(bucketName);

    public async Task PutObjectAsync(PutObjectRequest request) => await _client.PutObjectAsync(request);

    public async Task<GetObjectResponse> GetObjectAsync(string bucketName, string key) =>
        await _client.GetObjectAsync(bucketName, key);
}