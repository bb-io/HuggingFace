namespace Apps.HuggingFace.Models.Text.Responses;

public record GenerateEmbeddingResponse(IEnumerable<double> Embedding);