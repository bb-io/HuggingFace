using Apps.HuggingFace.Actions.Base;
using Apps.HuggingFace.Dtos;
using Apps.HuggingFace.Models.Audio.Requests;
using Apps.HuggingFace.Models.Audio.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.HuggingFace.Actions;

[ActionList]
public class AudioActions : BaseActions
{
    public AudioActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext, fileManagementClient)
    {
    }
    
    [Action("Create transcription", Description = "Generates a transcription given an audio file.")]
    public async Task<CreateTranscriptionResponse> CreateTranscription([ActionParameter] CreateTranscriptionRequest input)
    {
        var file = await ConvertToByteArray(input.Audio);
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
        request.AddFile("file", file, input.Audio.Name);
        var response = await client.ExecuteWithHandling<CreateTranscriptionResponse>(request);
        return response;
    }
    
     [Action("Classify audio", Description = "Classify the audio provided. Possible labels are model specific.")]
        public async Task<LabelDto> ClassifyAudio([ActionParameter] ClassifyAudioRequest input)
        {
            var file = await ConvertToByteArray(input.Audio);
            var client = new HuggingFaceClient(ApiType.InferenceApi);
            var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
            request.AddFile("file", file, input.Audio.Name);
            var response = await client.ExecuteWithHandling<IEnumerable<LabelDto>>(request);
            return response.MaxBy(label => label.Score);
        }
}