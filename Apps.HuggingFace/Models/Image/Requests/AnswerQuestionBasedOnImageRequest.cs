using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Image.Requests;

public record AnswerQuestionBasedOnImageRequest : ImageFileWrapper
{
    [Display("Model")] 
    [DataSource(typeof(VisualQuestionAnsweringDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Question { get; init; }
}