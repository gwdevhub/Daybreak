using Daybreak.Models.Builds;

namespace Daybreak.Services.BuildTemplates
{
    public interface IBuildTemplateManager
    {
        Build DecodeTemplate(string template);
        string EncodeTemplate(Build build);
    }
}
