using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Text.Requests;

public record AnswerQuestionWithTableRequest
{
    [Display("Model")]
    [DataSource(typeof(TableQuestionAnsweringModelDataSourceHandler))]
    public string ModelId { get; init; }
    
    public string Question { get; init; }
    
    [Display("Excel table")]
    public File ExcelTable { get; init; }
    
    [Display("Use cache")]
    public bool? UseCache { get; init; }
}