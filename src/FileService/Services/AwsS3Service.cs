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

    public async Task<GetObjectResponse> GetObjectAsync(GetObjectRequest request) =>
        await _client.GetObjectAsync(request);

    public Task<ListObjectsV2Response> ListObjectsV2Async(ListObjectsV2Request request,
        CancellationToken cancellationToken = default) => _client.ListObjectsV2Async(request, cancellationToken);


    public Task<DeleteObjectResponse> DeleteObjectAsync(DeleteObjectRequest request, CancellationToken cancellationToken = default) =>
        _client.DeleteObjectAsync(request, cancellationToken);
}