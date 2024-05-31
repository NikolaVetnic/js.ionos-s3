namespace IonosS3BlobStorageTest.Exceptions;
public class ApplicationError
{
    public ApplicationError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }
}