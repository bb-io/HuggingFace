using Blackbird.Applications.Sdk.Common;

namespace Apps.HuggingFace;

public class HuggingFaceApplication : IApplication
{
    public string Name
    {
        get => "Hugging Face";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}