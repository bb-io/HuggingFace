using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.HuggingFace.Models.Audio;

public record AudioFileWrapper
{
    public FileReference Audio { get; init; }
}