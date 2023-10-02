using Newtonsoft.Json;

namespace Apps.HuggingFace.Dtos;

public record MaskSequenceDto(float Score, string Sequence)
{
    [JsonProperty("token_str")]
    public string Token { get; init; }
}