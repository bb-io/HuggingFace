using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Image.Requests;

public record ClassifyImageRequest : ImageFileWrapper
{
    [Display("Model")] 
    [DataSource(typeof(ImageClassificationModelDataSourceHandler))] 
    public string ModelId { get; init; }
}