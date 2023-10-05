using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Image.Requests;

public record DetectObjectsRequest
{
    [Display("Model")]
    [DataSource(typeof(ObjectDetectionModelDataSourceHandler))]
    public string ModelId { get; init; }
    
    public File Image { get; init; }
    
    [Display("Output image filename without extension")]
    public string? OutputImageFilename { get; init; }
    
    [Display("Draw labels")]
    public bool? DrawLabels { get; init; }
}