namespace Apps.HuggingFace.Dtos;

public record ZeroShotClassificationResultDto(IEnumerable<string> Labels, IEnumerable<float> Scores);