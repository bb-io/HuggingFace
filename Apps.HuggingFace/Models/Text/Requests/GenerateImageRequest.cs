using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Text.Requests;

public record GenerateImageRequest
{
    [Display("Model")] 
    [DataSource(typeof(TextToImageModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string ImageDescription { get; init; }
    
    [Display("Output image name without extension")]
    public string? OutputImageName { get; init; } 
    
    [Display("Use cache")] 
    public bool? UseCache { get; init; }
}