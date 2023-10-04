using Apps.HuggingFace.Dtos;

namespace Apps.HuggingFace.Models.Text.Responses;

public record ClassifyTokensResponse(IEnumerable<TokenDto> Result);