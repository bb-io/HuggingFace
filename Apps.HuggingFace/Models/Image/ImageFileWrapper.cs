using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.HuggingFace.Models.Image;

public record ImageFileWrapper
{
    public FileReference Image { get; init; }
}