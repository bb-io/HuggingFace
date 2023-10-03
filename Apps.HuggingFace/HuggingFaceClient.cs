using System.Net;
using Apps.HuggingFace.Dtos;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.HuggingFace;

public class HuggingFaceClient : RestClient
{
    public HuggingFaceClient(ApiType apiType)
        : base(new RestClientOptions { ThrowOnAnyError = false, BaseUrl = GetBaseUrl(apiType), MaxTimeout = 300000 }) { }
    
    private static Uri GetBaseUrl(ApiType apiType) => apiType == ApiType.HubApi 
        ? new("https://huggingface.co/api") 
        : new("https://api-inference.huggingface.co");
    
    public async Task<T> ExecuteWithHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling(request);
        return DeserializeContent<T>(response.Content);
    }

    public async Task<RestResponse> ExecuteWithHandling(RestRequest request)
    {
        var response = await ExecuteAsync(request);
        
        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
        { 
            var error = DeserializeContent<ErrorDto>(response.Content);

            if (error.EstimatedTime is not null)
            {
                await Task.Delay(TimeSpan.FromSeconds(error.EstimatedTime.Value + error.EstimatedTime.Value * 0.05));
                response = await ExecuteAsync(request);
            }
        }
        
        if (response.IsSuccessful)
            return response;

        throw ConfigureErrorException(response.Content);
    }

    private Exception ConfigureErrorException(string responseContent)
    {
        var error = DeserializeContent<ErrorDto>(responseContent);
        
        if (error != null)
            return new(error.Error);

        return new(responseContent);
    }
    
    private static T DeserializeContent<T>(string content)
    {
        var deserializedContent = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            }
        );
        return deserializedContent;
    }
}

public enum ApiType
{
    HubApi,
    InferenceApi
}