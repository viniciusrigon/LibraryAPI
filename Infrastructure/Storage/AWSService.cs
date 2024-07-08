using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Configuration;
using Infrastructure.File;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

public class AWSService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string BUCKET_NAME;
    
    private const string prefix = "covers";

    //public AWSService(AppConfiguration appConfiguration, IOptions<AWSApiCredentials> options)
    public AWSService(AppConfiguration appConfiguration)
    {
        //_s3Client = new AmazonS3Client(options.Value.AwsAccessKey, options.Value.AwsSecretAccessKey, RegionEndpoint.USEast1);
        _s3Client = new AmazonS3Client();
        BUCKET_NAME = appConfiguration.BucketName;
    }

    public Task<PutObjectResponse> UploadFile(FileDTO file)
    {
        var request = new PutObjectRequest()
        {
            BucketName = BUCKET_NAME,
            Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
            InputStream = file.Stream
        };
        request.Metadata.Add("Content-Type", file.ContentType);
        return _s3Client.PutObjectAsync(request);
    }
    
}