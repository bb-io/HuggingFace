using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Image.Requests;

public record DetectObjectsRequest : ImageFileWrapper
{
    [Display("Model")]
    [DataSource(typeof(ObjectDetectionModelDataSourceHandler))]
    public string ModelId { get; init; }

    [Display("Output image filename without extension")]
    public string? OutputImageFilename { get; init; }
    
    [Display("Draw labels")]
    public bool? DrawLabels { get; init; }
}