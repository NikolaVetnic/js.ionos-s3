﻿namespace IonosS3BlobStorageTest.Implementation;
public interface IBlobStorage
{
    void Add(string folder, string id, byte[] content);
    Task<byte[]> FindAsync(string folder, string id);
    Task<IAsyncEnumerable<string>> FindAllAsync(string folder, string? prefix = null);
    void Remove(string folder, string id);
    Task SaveAsync();
}
