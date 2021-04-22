using Daybreak.Models.Builds;
using System.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve
{
    public interface IIconRetriever
    {
        Task<Optional<Stream>> GetIcon(Skill skill);
    }
}
