using Apps.HuggingFace.DataSourceHandlers.Models.Image;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Image.Requests;

public record GenerateImageRequest
{
    [Display("Model")] 
    [DataSource(typeof(TextToImageModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    [Display("Image description")]
    public string ImageDescription { get; init; }
    
    [Display("Output image name without extension")]
    public string? OutputImageName { get; init; } 
    
    [Display("Use cache")] 
    public bool? UseCache { get; init; }
}