using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.HuggingFace.Models.Text.Responses;

public record GenerateTextResponse
{
    [Display("Generated text")]
    [JsonProperty("generated_text")]
    public string GeneratedText { get; init; }
}