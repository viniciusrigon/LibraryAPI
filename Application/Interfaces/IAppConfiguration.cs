namespace Application.Interfaces;

public interface IAppConfiguration
{
    string BucketName { get; set; }
    string Region { get; set; }
    string CloudFrontDomainName { get; set; }
}