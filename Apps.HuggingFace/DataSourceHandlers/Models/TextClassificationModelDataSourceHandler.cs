﻿using Apps.HuggingFace.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.HuggingFace.DataSourceHandlers.Models;

public class TextClassificationModelDataSourceHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public TextClassificationModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var client = new HuggingFaceClient(ApiType.HubApi);
        var endpoint = $"/models?search={context.SearchString ?? ""}&sort=downloads&direction=-1&filter=text-classification&limit=30";
        var request = new HuggingFaceRequest(endpoint, Method.Get, InvocationContext.AuthenticationCredentialsProviders);
        var models = await client.ExecuteWithHandling<IEnumerable<ModelDto>>(request);
        return models.Where(model => model.PipelineTag == "text-classification")
            .ToDictionary(model => model.Id, model => model.Id);
    }
}