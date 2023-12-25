using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;

namespace Apps.HuggingFace.Actions.Base;

public class BaseActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;
    protected readonly IEnumerable<AuthenticationCredentialsProvider> AuthenticationCredentialsProviders;

    protected BaseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
        AuthenticationCredentialsProviders = invocationContext.AuthenticationCredentialsProviders;
    }

    protected async Task<FileReference> ConvertToFileReference(byte[] bytes, string filename, string contentType)
    {
        using var stream = new MemoryStream(bytes);
        var file = await _fileManagementClient.UploadAsync(stream, contentType, filename);
        return file;
    }

    protected async Task<byte[]> ConvertToByteArray(FileReference file)
    {
        var fileStream = await _fileManagementClient.DownloadAsync(file);
        var fileBytes = await fileStream.GetByteData();
        return fileBytes;
    }
}