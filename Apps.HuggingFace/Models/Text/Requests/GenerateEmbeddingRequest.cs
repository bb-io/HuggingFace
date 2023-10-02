﻿using Apps.HuggingFace.DataSourceHandlers.Models.Text;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.HuggingFace.Models.Text.Requests;

public record GenerateEmbeddingRequest
{
    [Display("Model")] 
    [DataSource(typeof(EmbeddingModelDataSourceHandler))] 
    public string ModelId { get; init; }
    
    public string Text { get; init; }
    
    [Display("Use cache")] 
    public bool? UseCache { get; init; }
}