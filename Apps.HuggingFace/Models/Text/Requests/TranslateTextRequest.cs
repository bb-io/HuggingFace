using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Text.Requests;

public record TranslateTextRequest
{
    [Display("Model")] 
    [DataSource(typeof(TranslationModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Text { get; init; }
    
    [Display("Use cache")] 
    public bool? UseCache { get; init; }
}