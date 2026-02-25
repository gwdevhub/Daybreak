using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

public interface ITemplateParser<TTemplateContext> 
    : ITemplateParser
    where TTemplateContext : struct
{
    TTemplateContext Decode(DecodeContext context);
    IBuildEntry CreateBuildEntry(TTemplateContext templateContext);
}

public interface ITemplateParser
{
    bool CanDecode(TemplateHeader header);
    bool CanEncode(IBuildEntry buildEntry);
    string Encode(EncodeContext context);
}