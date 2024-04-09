using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.HuggingFace;

public class HuggingFaceApplication : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get =>
        [
            ApplicationCategory.ArtificialIntelligence, ApplicationCategory.MachineTranslationAndMtqe,
            ApplicationCategory.Multimedia
        ];
        set { }
    }

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