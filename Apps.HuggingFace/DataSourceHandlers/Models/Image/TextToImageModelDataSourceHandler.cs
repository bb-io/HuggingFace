using Apps.HuggingFace.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.HuggingFace.DataSourceHandlers.Models.Image;

public class TextToImageModelDataSourceHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public TextToImageModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var client = new HuggingFaceClient(ApiType.HubApi);
        var endpoint = $"/models?search={context.SearchString ?? ""}&sort=likes&direction=-1&filter=text-to-image&limit=30";
        var request = new HuggingFaceRequest(endpoint, Method.Get, InvocationContext.AuthenticationCredentialsProviders);
        var models = await client.ExecuteWithHandling<IEnumerable<ModelDto>>(request);
        return models.Where(model => model.PipelineTag == "text-to-image")
            .ToDictionary(model => model.Id, model => model.Id);
    }
}