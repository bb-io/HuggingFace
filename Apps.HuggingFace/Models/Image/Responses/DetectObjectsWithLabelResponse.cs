using File = Blackbird.Applications.Sdk.Common.Files.File;

using Blackbird.Applications.Sdk.Common;

namespace Apps.HuggingFace.Models.Image.Responses;

public record DetectObjectsWithLabelResponse
{
    public bool ObjectsDetected { get; init; }
    
    [Display("Output image")]
    public File OutputImage { get; init; }
}