using Newtonsoft.Json;

namespace Apps.HuggingFace.Models.Responses;

public record SummarizeTextResponse([JsonProperty("summary_text")] string Summary);