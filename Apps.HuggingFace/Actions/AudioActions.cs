using Apps.HuggingFace.Dtos;
using Apps.HuggingFace.Models.Audio.Requests;
using Apps.HuggingFace.Models.Audio.Responses;
using Apps.HuggingFace.Models.Text.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.HuggingFace.Actions;

[ActionList]
public class AudioActions: BaseInvocable
{
    private readonly IEnumerable<AuthenticationCredentialsProvider> _authenticationCredentialsProviders;

    public AudioActions(InvocationContext invocationContext) : base(invocationContext)
    {
        _authenticationCredentialsProviders = invocationContext.AuthenticationCredentialsProviders;
    }
    
    [Action("Create transcription", Description = "Generates a transcription given an audio file.")]
    public async Task<CreateTranscriptionResponse> CreateTranscription([ActionParameter] CreateTranscriptionRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddFile("file", input.File.Bytes, input.File.Name);
        var response = await client.ExecuteWithHandling<CreateTranscriptionResponse>(request);
        return response;
    }
    
     [Action("Classify audio", Description = "Classify the audio provided. Possible labels are model specific.")]
        public async Task<LabelDto> ClassifyAudio([ActionParameter] ClassifyAudioRequest input)
        {
            var client = new HuggingFaceClient(ApiType.InferenceApi);
            var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
            request.AddFile("file", input.File.Bytes, input.File.Name);
            var response = await client.ExecuteWithHandling<IEnumerable<LabelDto>>(request);
            return response.MaxBy(label => label.Score);
        }
}