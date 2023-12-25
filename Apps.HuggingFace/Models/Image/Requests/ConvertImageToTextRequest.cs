using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Image.Requests;

public record ConvertImageToTextRequest : ImageFileWrapper
{
    [Display("Model")]
    [DataSource(typeof(ImageToTextDataSourceHandler))]
    public string ModelId { get; init; }
}