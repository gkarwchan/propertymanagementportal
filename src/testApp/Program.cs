// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddUserSecrets(Assembly.GetExecutingAssembly(), true); 
 
IConfiguration config = builder.Build();
Console.WriteLine($"Build add: {config["blobConnection"]}");

var blobUtil = new BlobUtil(config["blobConnection"]);

await blobUtil.UploadFile("d:\\tmp\\Pink Card.pdf", "3");

await blobUtil.UploadFile("d:\\tmp\\Pink Card.pdf", "4");

await blobUtil.UploadStream("d:\\tmp\\Record of Services.pdf", "3");

await blobUtil.UploadStream("d:\\tmp\\Record of Services.pdf", "4");
Console.WriteLine("ddd");