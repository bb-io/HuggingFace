using Apps.HuggingFace.DataSourceHandlers.FloatParameters;
using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Text.Requests;

public record ChatRequest
{
    [Display("Model")] 
    [DataSource(typeof(ChatModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Prompt { get; init; }
    
    [Display("Previously generated responses")]
    public IEnumerable<string>? GeneratedResponses { get; init; }
    
    [Display("Past user inputs")]
    public IEnumerable<string>? PastUserInputs { get; init; }

    [Display("Minimum length in tokens of output text")]
    public int? MinTokens { get; init; }
    
    [Display("Maximum length in tokens of output text")]
    public int? MaxTokens { get; init; }
    
    [DataSource(typeof(TemperatureDataSourceHandler))]
    public float? Temperature { get; init; }
    
    [Display("Repetition penalty")]
    [DataSource(typeof(RepetitionPenaltyDataSourceHandler))]
    public float? RepetitionPenalty { get; init; }
    
    [Display("Top K")]
    public int? TopK { get; init; }
    
    [Display("Top P")]
    public float? TopP { get; init; }
    
    [Display("Use cache")]
    public bool? UseCache { get; init; }
}