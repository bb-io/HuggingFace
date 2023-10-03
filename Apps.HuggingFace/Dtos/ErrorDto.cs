using Newtonsoft.Json;

namespace Apps.HuggingFace.Dtos;

public record ErrorDto(string Error)
{
    [JsonProperty("estimated_time")]
    public double? EstimatedTime { get; init; }
}