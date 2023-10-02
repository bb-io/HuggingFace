using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Text.Requests;

public record CalculateSemanticSimilarityRequest
{
    [Display("Model")] 
    [DataSource(typeof(SemanticSimilarityModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    [Display("First text")]
    public string FirstText { get; init; }
    
    [Display("Second text")]
    public string SecondText { get; init; }
    
    [Display("Use cache")] 
    public bool? UseCache { get; init; }
}