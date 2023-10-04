using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Models.Image.Responses;

public record GenerateImageResponse(File Image);