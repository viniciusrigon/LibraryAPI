using Domain;
using Infrastructure;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Infrastructure.Configuration;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
        x=>x.MigrationsHistoryTable("_EfMigrations", configuration.GetSection("Schema").GetSection("<YourDataSchema>").Value)));



var appConfig = new AppConfiguration
{
    BucketName = configuration.GetSection("AWS").GetSection("Bucket").Value,
    Region = configuration.GetSection("AWS").GetSection("Region").Value,
    CloudFrontDomainName = configuration.GetSection("AWS").GetSection("CloudFrontDomainName").Value,
};

/************************************************
// * Dependency Injection
// ************************************************/
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AWSService>();
builder.Services.AddSingleton<AppConfiguration>(appConfig);
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
//builder.Services.Configure<AWSApiCredentials>(builder.Configuration);


/*************************************************
 * Enable CORS
 *************************************************/
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("_myAllowSpecificOrigins", builder => builder.WithOrigins("https://localhost:7227")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddAutoMapper(typeof(Program));

// builder.Host.ConfigureAppConfiguration(((_, configurationBuilder) =>
// {
//     configurationBuilder.AddAmazonSecretsManager("us-east-1", "arn:aws:secretsmanager:us-east-1:891377099603:secret:library-api-secrets-hmCHEy");
// }));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();