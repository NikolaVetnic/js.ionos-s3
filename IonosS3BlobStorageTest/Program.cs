using IonosS3BlobStorageTest.Implementation;
using IonosS3BlobStorageTest.ImplementationAlt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IonosS3BlobStorageTest;
internal class Program
{
    private const string BlobFolder = "example-folder";
    private const string BlobId = "example-id";
    private static readonly byte[] BlobContent = [1, 2, 3, 4, 5];

    private static async Task Main()
    {
        await TestBlobStorageImplementation();
        await TestBlobStorageImplementationAlt();
    }

    private static async Task TestBlobStorageImplementation()
    {
        // Setup configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.override.json", optional: true, reloadOnChange: true)
            .Build();

        // Bind configuration
        var ionosConfig = configuration.GetSection("IonosS3Config").Get<IonosS3Config>()!;

        // Setup dependency injection
        var serviceCollection = new ServiceCollection()
            .AddLogging(configure => configure.AddConsole())
            .AddSingleton(ionosConfig)
            .AddSingleton<IBlobStorage, IonosS3BlobStorage>()
            .BuildServiceProvider();

        var blobStorage = serviceCollection.GetService<IBlobStorage>()!;

        // Add a file
        blobStorage.Add(BlobFolder, BlobId, BlobContent);

        // Save changes (upload files)
        await blobStorage.SaveAsync();

        // Find a file
        var retrievedContent = await blobStorage.FindAsync(BlobFolder, BlobId);
        Console.WriteLine($"Retrieved Content: {BitConverter.ToString(retrievedContent)}");

        // List all files
        var allBlobs = await blobStorage.FindAllAsync(BlobFolder);
        await foreach (var item in allBlobs)
        {
            Console.WriteLine(item);
        }

        // Remove a file
        blobStorage.Remove(BlobFolder, BlobId);

        // Save changes (delete files)
        await blobStorage.SaveAsync();
    }

    // ReSharper disable once UnusedMember.Local
    private static async Task TestBlobStorageImplementationAlt()
    {
        // Setup configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.override.json", optional: true, reloadOnChange: true)
            .Build();

        // Bind configuration
        var ionosConfig = configuration.GetSection("IonosS3Config").Get<IonosS3Config>()!;

        // Setup dependency injection
        var serviceCollection = new ServiceCollection()
            .AddSingleton(ionosConfig)
            .AddSingleton<IBlobStorageAlt, IonosS3BlobStorageAlt>()
            .BuildServiceProvider();

        var blobStorage = serviceCollection.GetService<IBlobStorageAlt>()!;

        // Add a file
        await blobStorage.AddAsync(BlobFolder, BlobId, BlobContent);

        // Find a file
        var retrievedContent = await blobStorage.FindAsync(BlobFolder, BlobId);
        Console.WriteLine($"Retrieved Content: {BitConverter.ToString(retrievedContent)}");

        // List all files
        await foreach (var item in blobStorage.FindAllAsync(BlobFolder))
        {
            Console.WriteLine(item);
        }

        // Remove a file
        await blobStorage.RemoveAsync(BlobFolder, BlobId);
    }
}