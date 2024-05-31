using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace IonosS3BlobStorageTest.ImplementationAlt;
public class IonosS3BlobStorageAlt : IBlobStorageAlt
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public IonosS3BlobStorageAlt(IonosS3Config config)
    {
        var s3Config = new AmazonS3Config
        {
            ServiceURL = config.ServiceUrl,
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(config.AccessKey, config.SecretKey, s3Config);
        _bucketName = config.BucketName;
    }

    public async Task AddAsync(string folder, string id, byte[] content)
    {
        using var stream = new MemoryStream(content);
        var request = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            Key = $"{folder}/{id}",
            BucketName = _bucketName
        };

        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(request);
    }

    public async Task<byte[]> FindAsync(string folder, string id)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = $"{folder}/{id}"
        };

        using var response = await _s3Client.GetObjectAsync(request);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public async IAsyncEnumerable<string> FindAllAsync(string folder, string? prefix = null)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = prefix != null ? $"{folder}/{prefix}" : folder
        };

        ListObjectsV2Response response;
        do
        {
            response = await _s3Client.ListObjectsV2Async(request);

            foreach (var obj in response.S3Objects)
            {
                yield return obj.Key;
            }

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated);
    }

    public async Task RemoveAsync(string folder, string id)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = $"{folder}/{id}"
        };

        await _s3Client.DeleteObjectAsync(request);
    }

    public async Task SaveAsync()
    {
        // This method might be unnecessary for S3 as changes are committed immediately.
        await Task.CompletedTask;
    }
}
