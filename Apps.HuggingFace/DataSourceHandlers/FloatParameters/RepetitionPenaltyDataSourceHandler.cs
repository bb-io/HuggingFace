using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.HuggingFace.DataSourceHandlers.FloatParameters;

public class RepetitionPenaltyDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public RepetitionPenaltyDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var temperatures = FloatArrayExtensions.GenerateFormattedFloatArray(0.0f, 100.0f, 0.1f)
            .Where(temperature => context.SearchString == null || temperature.Contains(context.SearchString))
            .ToDictionary(temperature => temperature, temperature => temperature);

        return temperatures;
    }
}