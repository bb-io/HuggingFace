using Newtonsoft.Json;

namespace Apps.HuggingFace.Dtos;

public record MaskTokenDto
{
    [JsonProperty("mask_token")]
    public string MaskToken { get; init; }
}