using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Image.Requests;

public record DetectObjectsWithLabelRequest : ImageFileWrapper
{
    [Display("Model")]
    [DataSource(typeof(ObjectDetectionModelDataSourceHandler))]
    public string ModelId { get; init; }

    public string Label { get; init; }
    
    [Display("Output image filename without extension")]
    public string? OutputImageFilename { get; init; }
}