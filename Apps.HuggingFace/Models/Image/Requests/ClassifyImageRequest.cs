using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Image.Requests;

public record ClassifyImageRequest
{
    [Display("Model")] 
    [DataSource(typeof(ImageClassificationModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public File Image { get; init; }
}