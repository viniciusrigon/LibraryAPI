using Application.Interfaces;

namespace Infrastructure.Storage;

public class AppConfiguration : IAppConfiguration
{
    public AppConfiguration()
    {
        BucketName = "";
        Region = "";
        AwsAccessKey = "";
        AwsSecretAccessKey = "";
        AwsSessionToken = "";
        CloudFrontDomainName = "";
    }

    public string BucketName { get; set; }
    public string Region { get; set; }
    public string AwsAccessKey { get; set; }
    public string AwsSecretAccessKey { get; set; }
    public string AwsSessionToken { get; set; }

    public string CloudFrontDomainName { get; set; }      
}