using Application.Interfaces;

namespace Infrastructure.Storage;

public class AppConfiguration : IAppConfiguration
{
    public AppConfiguration()
    {
        BucketName = "";
        Region = "";
        CloudFrontDomainName = "";
    }

    public string BucketName { get; set; }
    public string Region { get; set; }
    public string CloudFrontDomainName { get; set; }      
}