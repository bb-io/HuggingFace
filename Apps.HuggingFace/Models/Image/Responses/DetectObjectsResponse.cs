using Apps.HuggingFace.Dtos;
using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Image.Responses;

public record DetectObjectsResponse
{
    [Display("Detected objects")]
    public IEnumerable<DetectedObjectDto> DetectedObjects { get; init; }
    
    [Display("Output image")]
    public File OutputImage { get; init; }
}