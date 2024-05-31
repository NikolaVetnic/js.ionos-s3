namespace IonosS3BlobStorageTest.Exceptions;
public class BlobAlreadyExistsException : Exception
{
    public BlobAlreadyExistsException(string blobName)
    {
        BlobName = blobName;
    }

    public string BlobName { get; }
}