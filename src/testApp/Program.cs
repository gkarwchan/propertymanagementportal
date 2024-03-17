// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddUserSecrets(Assembly.GetExecutingAssembly(), true); 
 
IConfiguration config = builder.Build();
Console.WriteLine($"Build add: {config["blobConnection"]}");

var storageClient = new StorageClient(config["blobConnection"]);


Console.WriteLine("ddd");