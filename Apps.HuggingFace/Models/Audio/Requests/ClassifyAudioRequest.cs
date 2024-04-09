using Apps.HuggingFace.DataSourceHandlers.Models.Audio;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Audio.Requests;

public record ClassifyAudioRequest : AudioFileWrapper
{
    [Display("Model")]
    [DataSource(typeof(AudioClassificationModelDataSourceHandler))]
    public string ModelId { get; init; }
}