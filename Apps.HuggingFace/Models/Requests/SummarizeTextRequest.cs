using Apps.HuggingFace.DataSourceHandlers.FloatParameters;
using Apps.HuggingFace.DataSourceHandlers.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Requests;

public record SummarizeTextRequest
{
    [Display("Model")] 
    [DataSource(typeof(SummarizationModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Text { get; init; }
    
    [Display("Minimum length in tokens of output summary")]
    public int? MinTokens { get; init; }
    
    [Display("Maximum length in tokens of output summary")]
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