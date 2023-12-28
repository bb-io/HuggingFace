using Apps.HuggingFace.Actions.Base;
using Apps.HuggingFace.Dtos;
using Apps.HuggingFace.ImageHelpers;
using Apps.HuggingFace.Models.Image;
using Apps.HuggingFace.Models.Image.Requests;
using Apps.HuggingFace.Models.Image.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.HuggingFace.Actions;

[ActionList]
public class ImageActions : BaseActions
{
    public ImageActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext, fileManagementClient)
    {
    }
    
    [Action("Generate image", Description = "Generate image given text description of image.")]
    public async Task<ImageFileWrapper> GenerateImage([ActionParameter] GenerateImageRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
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
        var fileReference = await ConvertToFileReference(response.RawBytes,
            filename: $"{input.OutputImageName ?? input.ImageDescription}.{extension}",
            contentType: response.ContentType);

        return new ImageFileWrapper { Image = fileReference };
    }
    
    [Action("Classify image", Description = "Perform image classification. Possible labels are model specific.")]
    public async Task<LabelDto> ClassifyImage([ActionParameter] ClassifyImageRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
        var imageBytes = await ConvertToByteArray(input.Image);
        request.AddParameter(input.Image.ContentType, imageBytes, ParameterType.RequestBody);
        var response = await client.ExecuteWithHandling<IEnumerable<LabelDto>>(request);
        return response.MaxBy(label => label.Score);
    }
    
    [Action("Convert image to text", Description = "Given an image, generate its text description.")]
    public async Task<ConvertImageToTextResponse> ConvertImageToText([ActionParameter] ConvertImageToTextRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
        var imageBytes = await ConvertToByteArray(input.Image);
        request.AddParameter(input.Image.ContentType, imageBytes, ParameterType.RequestBody);
        var response = await client.ExecuteWithHandling<IEnumerable<ConvertImageToTextResponse>>(request);
        return response.First();
    }
    
    [Action("Answer question based on image", Description = "Answer the question based on the image.")]
    public async Task<AnswerDto> AnswerQuestionBasedOnImage([ActionParameter] AnswerQuestionBasedOnImageRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
        var imageBytes = await ConvertToByteArray(input.Image);
        request.AddJsonBody(new
        {
            inputs = new
            {
                question = input.Question, 
                image = imageBytes
            }
        });
        
        var response = await client.ExecuteWithHandling<IEnumerable<AnswerDto>>(request);
        return response.MaxBy(answer => answer.Score);
    }
    
    [Action("Detect objects on image", Description = "Perform object detection.")]
    public async Task<DetectObjectsResponse> DetectObjects([ActionParameter] DetectObjectsRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
        var imageBytes = await ConvertToByteArray(input.Image);
        request.AddParameter(input.Image.ContentType, imageBytes, ParameterType.RequestBody);
        var detectedObjects = await client.ExecuteWithHandling<IEnumerable<DetectedObjectDto>>(request);
        var image = imageBytes.DrawBoundingBoxes(input.Image.ContentType, detectedObjects, input.DrawLabels ?? false);
        var imageFilename =
            $"{input.OutputImageFilename ?? Path.GetFileNameWithoutExtension(input.Image.Name) + "_with_objects_detected"}" +
            $"{Path.GetExtension(input.Image.Name)}";
        var imageFileReference = await ConvertToFileReference(image, imageFilename, input.Image.ContentType);
        
        return new DetectObjectsResponse
        {
            DetectedObjects = detectedObjects, 
            Image = imageFileReference
        };
    }
    
    [Action("Detect objects on image with specified label", Description = "Detect objects on image that have the label " +
                                                                          "specified.")]
    public async Task<DetectObjectsWithLabelResponse> DetectObjectsWithLabel(
        [ActionParameter] DetectObjectsWithLabelRequest input)
    {
        var client = new HuggingFaceClient(ApiType.InferenceApi);
        var request = new HuggingFaceRequest($"/models/{input.ModelId}", Method.Post, AuthenticationCredentialsProviders);
        var imageBytes = await ConvertToByteArray(input.Image);
        request.AddParameter(input.Image.ContentType, imageBytes, ParameterType.RequestBody);
        var detectedObjects = await client.ExecuteWithHandling<IEnumerable<DetectedObjectDto>>(request);
        var targetDetectedObjects = detectedObjects.Where(obj => obj.Label.Equals(input.Label, StringComparison.OrdinalIgnoreCase));
    
        if (!targetDetectedObjects.Any())
            return new DetectObjectsWithLabelResponse
            {
                ObjectsDetected = false,
                Image = input.Image
            };
        
        var image = imageBytes.DrawBoundingBoxes(input.Image.ContentType, targetDetectedObjects, false);
        var imageFilename = 
            $"{input.OutputImageFilename ?? Path.GetFileNameWithoutExtension(input.Image.Name) + "_with_objects_detected"}" +
            $"{Path.GetExtension(input.Image.Name)}";
        var imageFileReference = await ConvertToFileReference(image, imageFilename, input.Image.ContentType);
        
        return new DetectObjectsWithLabelResponse
        {
            ObjectsDetected = true,
            Image = imageFileReference
        };
    }
}