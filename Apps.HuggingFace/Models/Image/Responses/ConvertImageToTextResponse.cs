using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.HuggingFace.Models.Image.Responses;

public record ConvertImageToTextResponse
{
    [Display("Generated text")]
    [JsonProperty("generated_text")]
    public string GeneratedText { get; init; }
}