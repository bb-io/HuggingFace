using Newtonsoft.Json;

namespace Apps.HuggingFace.Models.Responses;

public record TranslateTextResponse([JsonProperty("translation_text")] string Translation);