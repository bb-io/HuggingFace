using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using File = Blackbird.Applications.Sdk.Common.Files.File;

using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Image.Requests;

public record DetectObjectsWithLabelRequest
{
    [Display("Model")]
    [DataSource(typeof(ObjectDetectionModelDataSourceHandler))]
    public string ModelId { get; init; }
    
    public File Image { get; init; }
    
    public string Label { get; init; }
    
    [Display("Output image filename without extension")]
    public string? OutputImageFilename { get; init; }
}