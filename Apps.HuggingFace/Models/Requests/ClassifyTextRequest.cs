using Apps.HuggingFace.DataSourceHandlers.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Requests;

public record ClassifyTextRequest
{
    [Display("Model")] 
    [DataSource(typeof(TextClassificationModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Text { get; init; }
    
    [Display("Use cache")] 
    public bool? UseCache { get; init; }
}