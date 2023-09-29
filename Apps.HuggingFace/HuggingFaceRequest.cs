using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.HuggingFace;

public class HuggingFaceRequest : RestRequest
{
    public HuggingFaceRequest(string endpoint, Method method,
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
    {
        this.AddHeader("Authorization", authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
    }
}