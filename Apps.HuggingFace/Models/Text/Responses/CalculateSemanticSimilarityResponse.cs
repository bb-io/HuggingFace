using Blackbird.Applications.Sdk.Common;

namespace Apps.HuggingFace.Models.Text.Responses;

public record CalculateSemanticSimilarityResponse
{
    [Display("Similarity score")]
    public float SimilarityScore { get; init; }
}