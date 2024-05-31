namespace IonosS3BlobStorageTest.ImplementationAlt;
public interface IBlobStorageAlt
{
    Task AddAsync(string folder, string id, byte[] content);
    Task<byte[]> FindAsync(string folder, string id);
    IAsyncEnumerable<string> FindAllAsync(string folder, string? prefix = null);
    Task RemoveAsync(string folder, string id);
    Task SaveAsync();
}