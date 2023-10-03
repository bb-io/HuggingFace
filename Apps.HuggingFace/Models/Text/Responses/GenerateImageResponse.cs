using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Text.Responses;

public record GenerateImageResponse(File Image);