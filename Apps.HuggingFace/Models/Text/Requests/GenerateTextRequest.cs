using Apps.HuggingFace.DataSourceHandlers.FloatParameters;
using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Text.Requests;

public record GenerateTextRequest
{
    [Display("Model")] 
    [DataSource(typeof(TextGenerationModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Prompt { get; init; }
    
    [Display("Return full text with original query")]
    public bool ReturnFullText { get; init; }

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