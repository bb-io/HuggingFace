using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Image.Requests;

public record AnswerQuestionBasedOnImageRequest
{
    [Display("Model")] 
    [DataSource(typeof(VisualQuestionAnsweringDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public File Image { get; init; }
    
    public string Question { get; init; }
}