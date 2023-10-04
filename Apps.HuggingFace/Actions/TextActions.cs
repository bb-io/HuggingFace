using System.Text;
using System.Text.RegularExpressions;
using Apps.HuggingFace.Dtos;
using Apps.HuggingFace.Models.Text.Requests;
using Apps.HuggingFace.Models.Text.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.HuggingFace.Actions;

[ActionList]
public class TextActions : BaseInvocable
{
    private readonly IEnumerable<AuthenticationCredentialsProvider> _authenticationCredentialsProviders;

    public TextActions(InvocationContext invocationContext) : base(invocationContext)
    {
        _authenticationCredentialsProviders = invocationContext.AuthenticationCredentialsProviders;
    }
    
    [Action("Summarize text", Description = "Summarize the text provided.")]
    public async Task<SummarizeTextResponse> SummarizeText([ActionParameter] SummarizeTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            parameters = new
            {
                min_length = input.MinTokens,
                max_length = input.MaxTokens,
                top_k = input.TopK,
                top_p = input.TopP,
                temperature = input.Temperature ?? 1.0,
                repetition_penalty = input.RepetitionPenalty
            },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<SummarizeTextResponse>>(request);
        return response.First();
    }

    [Action("Answer question", Description = "Answer the question given the context.")]
    public async Task<AnswerDto> AnswerQuestion([ActionParameter] AnswerQuestionRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = new
            {
                question = input.Question,
                context = input.Context
            },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<AnswerDto>(request);
        return response;
    }
    
    [Action("Answer question with table", Description = "Answer the question given the excel table with .xlsx extension " +
                                                        "where the answer could be found.")]
    public async Task<AnswerQuestionWithTableResponse> AnswerQuestionWithTable(
        [ActionParameter] AnswerQuestionWithTableRequest input)
    {
        var extenstion = Path.GetExtension(input.ExcelTable.Name);
        if (extenstion != ".xlsx")
            throw new Exception("Please provide excel table with .xlsx extension");

        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        var tableAsDictionary = ReadExcelFileToDictionary(input.ExcelTable.Bytes);
        request.AddJsonBody(new
        {
            inputs = new
            {
                query = input.Question,
                table = tableAsDictionary
            },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<AnswerQuestionWithTableResponse>(request);
        return response;
    }
    
    [Action("Classify text", Description = "Perform text classification. Returning labels vary depending on model used.")]
    public async Task<LabelDto> ClassifyText([ActionParameter] ClassifyTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<IEnumerable<LabelDto>>>(request);
        return response.First().MaxBy(label => label.Score);
    }
    
    [Action("Translate text", Description = "Translate text from one language to another.")]
    public async Task<TranslateTextResponse> TranslateText([ActionParameter] TranslateTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<TranslateTextResponse>>(request);
        return response.First();
    }
    
    [Action("Fill mask", Description = "Fills in a hole with a missing word. Use mask token to specify the place to " +
                                       "be filled. Mask token can differ depending on model used. There can be more than " +
                                       "one mask token in the text. Text with filled holes is returned.")]
    public async Task<FillMaskResponse> FillMask([ActionParameter] FillMaskRequest input)
    {
        var inferenceClient = new HuggingFaceClient(ApiType.InferenceApi);
        var inferenceRequest = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        inferenceRequest.AddJsonBody(new
        {
            inputs = input.Text,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var hubClient = new HuggingFaceClient(ApiType.HubApi);
        var getModelRequest = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Get, 
            _authenticationCredentialsProviders);
        var maskToken = (await hubClient.ExecuteWithHandling<MaskTokenDto>(getModelRequest)).MaskToken;
        
        var masks = Regex.Matches(input.Text, Regex.Escape(maskToken));

        if (masks.Count == 0)
            throw new Exception($"No {maskToken} tokens were found in the text.");
        
        if (masks.Count == 1)
        {
            var maskSequences = await inferenceClient.ExecuteWithHandling<IEnumerable<MaskSequenceDto>>(inferenceRequest);
            return new FillMaskResponse(maskSequences.MaxBy(sequence => sequence.Score).Sequence);
        }
        else
        {
            var maskSequences =
                (await inferenceClient.ExecuteWithHandling<IEnumerable<IEnumerable<MaskSequenceDto>>>(inferenceRequest))
                .ToArray();
            var resultText = new StringBuilder();
            var startIndex = 0;
            
            for (var i = 0; i < masks.Count; i++)
            {
                var holeIndex = masks[i].Index;
                var token = maskSequences[i].MaxBy(sequence => sequence.Score).Token;
                resultText.Append(input.Text.Substring(startIndex, holeIndex - startIndex));
                resultText.Append(token);
                startIndex += holeIndex - startIndex + maskToken.Length;
            }
            
            resultText.Append(input.Text.Substring(startIndex));
            return new FillMaskResponse(resultText.ToString());
        }
    }
    
    [Action("Calculate semantic similarity", Description = "Calculate semantic similarity between two texts.")]
    public async Task<CalculateSemanticSimilarityResponse> CalculateSemanticSimilarity(
        [ActionParameter] CalculateSemanticSimilarityRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            source_sentence = input.FirstText,
            sentences = new[] { input.SecondText },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<float>>(request);
        return new CalculateSemanticSimilarityResponse { SimilarityScore = response.First() };
    }
    
    [Action("Generate text", Description = "Generate text given a prompt.")]
    public async Task<GenerateTextResponse> GenerateText([ActionParameter] GenerateTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Prompt,
            parameters = new
            {
                top_k = input.TopK,
                top_p = input.TopP,
                temperature = input.Temperature ?? 1.0,
                repetition_penalty = input.RepetitionPenalty,
                return_full_text = input.ReturnFullText
            },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<GenerateTextResponse>>(request);
        return response.First();
    }
    
    [Action("Classify text according to candidate labels", Description = "Perform text classification according to " +
                                                                         "provided candidate labels.")]
    public async Task<LabelDto> ClassifyTextWithLabels([ActionParameter] ClassifyTextWithLabelsRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            parameters = new
            {
                candidate_labels = input.CandidateLabels
            },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<ZeroShotClassificationResultDto>(request);
        return new LabelDto(response.Labels.First(), response.Scores.First());
    }
    
    [Action("Chat", Description = "Perform conversational task. You can specify past user inputs and previously generated " +
                                  "responses. This lists should have the same lengths.")]
    public async Task<ChatResponse> Chat([ActionParameter] ChatRequest input)
    {
        if ((input.GeneratedResponses is null && input.PastUserInputs is not null)
            || (input.GeneratedResponses is not null && input.PastUserInputs is null))
            throw new Exception("Please provide both past user inputs and previously generated responses, or provide neither.");

        if (input.GeneratedResponses?.Count() != input.PastUserInputs?.Count())
            throw new Exception("Past user inputs and previously generated responses should be of the same length.");
        
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Prompt,
            generated_responses = input.GeneratedResponses,
            past_user_inputs = input.PastUserInputs,
            parameters = new
            {
                min_length = input.MinTokens,
                max_length = input.MaxTokens,
                top_k = input.TopK,
                top_p = input.TopP,
                temperature = input.Temperature ?? 1.0,
                repetition_penalty = input.RepetitionPenalty
            },
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<ChatResponse>(request);
        return response;
    }
    
    [Action("Classify tokens", Description = "Perform tokens classification. Usually used for keywords extraction or " +
                                             "grammatical sentence parsing.")]
    public async Task<ClassifyTokensResponse> ClassifyTokens([ActionParameter] ClassifyTokensRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<TokenDto>>(request);
        return new ClassifyTokensResponse(response);
    }

    [Action("Generate embedding", Description = "Generate text embedding. An embedding is a list of floating point " +
                                                "numbers that captures semantic information about the text that it " +
                                                "represents.")]
    public async Task<GenerateEmbeddingResponse> GenerateEmbedding([ActionParameter] GenerateEmbeddingRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.Text,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });

        try
        {
            var response = await client.ExecuteWithHandling<IEnumerable<double>>(request);
            return new GenerateEmbeddingResponse(response);
        }
        catch (JsonReaderException)
        {
            var response = await client.ExecuteWithHandling<IEnumerable<IEnumerable<IEnumerable<double>>>>(request);
            var flattenedEmbedding = response.First().SelectMany(e => e);
            return new GenerateEmbeddingResponse(flattenedEmbedding);
        }
    }
    
    private static Dictionary<string, List<string>> ReadExcelFileToDictionary(byte[] excelFile)
    {
        static string GetCellValue(WorkbookPart workbookPart, Cell cell)
        {
            var stringTablePart = workbookPart.SharedStringTablePart;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                var index = int.Parse(cell.InnerText);
                return stringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index).InnerText;
            }
        
            return cell.InnerText;
        }
        
        var result = new Dictionary<string, List<string>>();

        using (var stream = new MemoryStream(excelFile))
        using (var spreadsheetDocument = SpreadsheetDocument.Open(stream, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var worksheetPart = workbookPart.WorksheetParts.First();
            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            var headerRow = sheetData.Elements<Row>().FirstOrDefault();

            if (headerRow == null)
                throw new Exception("The Excel table does not have a header row.");

            foreach (var cell in headerRow.Elements<Cell>())
            {
                var columnHeader = GetCellValue(workbookPart, cell);

                if (!result.ContainsKey(columnHeader))
                    result[columnHeader] = new List<string>();
            }

            foreach (var row in sheetData.Elements<Row>().Skip(1)) // Skip the header row
            {
                var columnIndex = 0;

                foreach (var cell in row.Elements<Cell>())
                {
                    var columnHeader = GetCellValue(workbookPart, headerRow.Elements<Cell>().ElementAt(columnIndex));
                    var cellValue = GetCellValue(workbookPart, cell);

                    if (result.ContainsKey(columnHeader))
                        result[columnHeader].Add(cellValue);

                    columnIndex++;
                }
            }
        }

        return result;
    }
}