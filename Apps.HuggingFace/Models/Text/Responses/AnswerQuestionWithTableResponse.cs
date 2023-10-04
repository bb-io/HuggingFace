using Blackbird.Applications.Sdk.Common;

namespace Apps.HuggingFace.Models.Text.Responses;

public record AnswerQuestionWithTableResponse(string Answer)
{
    [Display("Cells referenced in answer")]
    public IEnumerable<string>? Cells { get; init; }

    [Display("Coordinates of cells referenced in answer")]
    public IEnumerable<IEnumerable<int>>? Coordinates { get; init; }
}