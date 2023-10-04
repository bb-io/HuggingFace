using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.HuggingFace.Dtos;

public record TokenDto
{
    [Display("Entity group")]
    [JsonProperty("entity_group")]
    public string EntityGroup { get; init; }
    
    [Display("Word or phrase")]
    public string Word { get; init; }
    
    public float Score { get; init; }
}