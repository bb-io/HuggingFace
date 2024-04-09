using Apps.HuggingFace.DataSourceHandlers.Models.Audio;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Audio.Requests;

public record CreateTranscriptionRequest : AudioFileWrapper
{
    [Display("Model")]
    [DataSource(typeof(SpeechRecognitionDataSourceHandler))]
    public string ModelId { get; init; }
}