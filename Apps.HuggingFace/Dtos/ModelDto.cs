﻿using Newtonsoft.Json;

namespace Apps.HuggingFace.Dtos;

public record ModelDto(string Id, [JsonProperty("pipeline_tag")] string PipelineTag);