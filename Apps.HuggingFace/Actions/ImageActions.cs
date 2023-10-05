using Apps.HuggingFace.Dtos;
using Apps.HuggingFace.ImageHelpers;
using Apps.HuggingFace.Models.Image.Requests;
using Apps.HuggingFace.Models.Image.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.HuggingFace.Actions;

[ActionList]
public class ImageActions : BaseInvocable
{
    private readonly IEnumerable<AuthenticationCredentialsProvider> _authenticationCredentialsProviders;

    public ImageActions(InvocationContext invocationContext) : base(invocationContext)
    {
        _authenticationCredentialsProviders = invocationContext.AuthenticationCredentialsProviders;
    }
    
    [Action("Generate image", Description = "Generate image given text description of image.")]
    public async Task<GenerateImageResponse> GenerateImage([ActionParameter] GenerateImageRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = input.ImageDescription,
            options = new
            {
                use_cache = input.UseCache ?? true,
                wait_for_model = true
            }
        });
        
        var response = await client.ExecuteWithHandling(request);
        var extension = MimeTypes.GetMimeTypeExtensions(response.ContentType).Last();
        return new GenerateImageResponse(new File(response.RawBytes)
        {
            ContentType = response.ContentType,
            Name = $"{input.OutputImageName ?? input.ImageDescription}.{extension}"
        });
    }
    
    [Action("Classify image", Description = "Perform image classification. Possible labels are model specific.")]
    public async Task<LabelDto> ClassifyImage([ActionParameter] ClassifyImageRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddParameter(input.Image.ContentType, input.Image.Bytes, ParameterType.RequestBody);
        var response = await client.ExecuteWithHandling<IEnumerable<LabelDto>>(request);
        return response.MaxBy(label => label.Score);
    }
    
    [Action("Convert image to text", Description = "Given an image, generate its text description.")]
    public async Task<ConvertImageToTextResponse> ConvertImageToText([ActionParameter] ConvertImageToTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddParameter(input.Image.ContentType, input.Image.Bytes, ParameterType.RequestBody);
        var response = await client.ExecuteWithHandling<IEnumerable<ConvertImageToTextResponse>>(request);
        return response.First();
    }
    
    [Action("Answer question based on image", Description = "Answer the question based on the image.")]
    public async Task<AnswerDto> AnswerQuestionBasedOnImage([ActionParameter] AnswerQuestionBasedOnImageRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            inputs = new
            {
                question = input.Question, 
                image = input.Image.Bytes
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<AnswerDto>>(request);
        return response.MaxBy(answer => answer.Score);
    }
    
    [Action("Detect objects on image", Description = "Perform object detection.")]
    public async Task<DetectObjectsResponse> DetectObjects([ActionParameter] DetectObjectsRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddParameter(input.Image.ContentType, input.Image.Bytes, ParameterType.RequestBody);
        var detectedObjects = await client.ExecuteWithHandling<IEnumerable<DetectedObjectDto>>(request);
        var image = input.Image.DrawBoundingBoxes(detectedObjects, input.DrawLabels ?? false);
        var imageFilename =
            $"{input.OutputImageFilename ?? Path.GetFileNameWithoutExtension(input.Image.Name) + "_with_objects_detected"}" +
            $"{Path.GetExtension(input.Image.Name)}";
        
        return new DetectObjectsResponse
        {
            DetectedObjects = detectedObjects, 
            OutputImage = new File(image)
            {
                ContentType = input.Image.ContentType,
                Name = imageFilename
            }
        };
    }
    
    [Action("Detect objects on image with specified label", Description = "Detect objects on image that have the label " +
                                                                          "specified.")]
    public async Task<DetectObjectsWithLabelResponse> DetectObjectsWithLabel(
        [ActionParameter] DetectObjectsWithLabelRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, _authenticationCredentialsProviders);
        request.AddParameter(input.Image.ContentType, input.Image.Bytes, ParameterType.RequestBody);
        var detectedObjects = await client.ExecuteWithHandling<IEnumerable<DetectedObjectDto>>(request);
        var targetDetectedObjects = detectedObjects.Where(obj => obj.Label.Equals(input.Label, StringComparison.OrdinalIgnoreCase));
    
        if (!targetDetectedObjects.Any())
            return new DetectObjectsWithLabelResponse
            {
                ObjectsDetected = false,
                OutputImage = input.Image
            };
        
        var image = input.Image.DrawBoundingBoxes(targetDetectedObjects, false);
        var imageFilename = 
            $"{input.OutputImageFilename ?? Path.GetFileNameWithoutExtension(input.Image.Name) + "_with_objects_detected"}" +
            $"{Path.GetExtension(input.Image.Name)}";
        
        return new DetectObjectsWithLabelResponse
        {
            ObjectsDetected = true,
            OutputImage = new File(image)
            {
                ContentType = input.Image.ContentType,
                Name = imageFilename
            }
        };
    }
}