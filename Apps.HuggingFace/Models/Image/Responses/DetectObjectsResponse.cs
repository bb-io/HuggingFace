using Apps.HuggingFace.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.HuggingFace.Models.Image.Responses;

public record DetectObjectsResponse : ImageFileWrapper
{
    [Display("Detected objects")]
    public IEnumerable<DetectedObjectDto> DetectedObjects { get; init; }
}