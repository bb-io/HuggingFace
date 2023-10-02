using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Text.Requests;

public record AnswerQuestionRequest
{
    [Display("Model")] 
    [DataSource(typeof(QuestionAnsweringModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Question { get; init; }
    
    public string Context { get; init; }
}