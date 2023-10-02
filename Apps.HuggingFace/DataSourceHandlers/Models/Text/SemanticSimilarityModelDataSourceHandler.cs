using Apps.HuggingFace.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.HuggingFace.DataSourceHandlers.Models.Text;

public class SemanticSimilarityModelDataSourceHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public SemanticSimilarityModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var client = new HuggingFaceClient(ApiType.HubApi);
        var endpoint = $"/models?search={context.SearchString ?? ""}&sort=likes&direction=-1&filter=sentence-similarity&limit=30";
        var request = new HuggingFaceRequest(endpoint, Method.Get, InvocationContext.AuthenticationCredentialsProviders);
        var models = await client.ExecuteWithHandling<IEnumerable<ModelDto>>(request);
        return models.Where(model => model.PipelineTag == "sentence-similarity")
            .ToDictionary(model => model.Id, model => model.Id);
    }
}