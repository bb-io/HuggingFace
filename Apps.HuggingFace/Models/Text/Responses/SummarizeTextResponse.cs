using Newtonsoft.Json;

namespace Apps.HuggingFace.Models.Text.Responses;

public record SummarizeTextResponse
{
    [JsonProperty("summary_text")] 
    public string Summary { get; init; }
}