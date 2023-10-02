using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.HuggingFace.Models.Text.Responses;

public record ChatResponse
{
    [Display("Generated text")]
    [JsonProperty("generated_text")]
    public string GeneratedText { get; init; }
    
    public Conversation Conversation { get; init; }
}

public record Conversation
{
    [Display("Previously generated responses")]
    [JsonProperty("generated_responses")]
    public IEnumerable<string>? GeneratedResponses { get; init; }
    
    [Display("Past user inputs")]
    [JsonProperty("past_user_inputs")]
    public IEnumerable<string>? PastUserInputs { get; init; }
}