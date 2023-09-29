using Apps.HuggingFace.Dtos;
using Apps.HuggingFace.Models.Requests;
using Apps.HuggingFace.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.HuggingFace;

[ActionList]
public class Actions : BaseInvocable
{
    private readonly IEnumerable<AuthenticationCredentialsProvider> _authenticationCredentialsProviders;

    public Actions(InvocationContext invocationContext) : base(invocationContext)
    {
        _authenticationCredentialsProviders = invocationContext.AuthenticationCredentialsProviders;
    }
    
    [Action("Summarize text", Description = "Summarize the text provided.")]
    public async Task<SummarizeTextResponse> SummarizeText([ActionParameter] SummarizeTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            parameters = new
            {
                min_length = input.MinTokens,
                max_length = input.MaxTokens,
                top_k = input.TopK,
                top_p = input.TopP,
                temperature = input.Temperature ?? 1.0,
                repetition_penalty = input.RepetitionPenalty
            },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<SummarizeTextResponse>>(request);
        return response.First();
    }

    [Action("Answer question", Description = "Answer the question given the context.")]
    public async Task<AnswerQuestionResponse> AnswerQuestion([ActionParameter] AnswerQuestionRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = new
            {
                question = input.Question,
                context = input.Context
            },
            options = new
            {
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<AnswerQuestionResponse>(request);
        return response;
    }
    
    [Action("Classify text", Description = "Perform text classification. Returning labels vary depending on model used.")]
    public async Task<LabelDto> ClassifyText([ActionParameter] ClassifyTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<IEnumerable<LabelDto>>>(request);
        return response.First().MaxBy(label => label.Score);
    }
    
    [Action("Translate text", Description = "Translate text from one language to another.")]
    public async Task<TranslateTextResponse> TranslateText([ActionParameter] TranslateTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<TranslateTextResponse>>(request);
        return response.First();
    }
}