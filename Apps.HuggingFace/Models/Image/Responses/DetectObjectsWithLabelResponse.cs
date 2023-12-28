namespace Apps.HuggingFace.Models.Image.Responses;

public record DetectObjectsWithLabelResponse : ImageFileWrapper
{
    public bool ObjectsDetected { get; init; }
}