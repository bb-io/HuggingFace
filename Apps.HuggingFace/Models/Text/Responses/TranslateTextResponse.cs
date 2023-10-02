using Newtonsoft.Json;

namespace Apps.HuggingFace.Models.Text.Responses;

public record TranslateTextResponse
{
    [JsonProperty("translation_text")] 
    public string Translation { get; init; }
}