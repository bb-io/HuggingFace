using Apps.HuggingFace.DataSourceHandlers.Models.Audio;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Audio.Requests;

public record CreateTranscriptionRequest
{
    [Display("Model")]
    [DataSource(typeof(SpeechRecognitionDataSourceHandler))]
    public string ModelId { get; init; }
    
    public File File { get; init; }
}