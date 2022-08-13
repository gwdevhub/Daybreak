using Daybreak.Models.Builds;
using System;
using System.Extensions;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve
{
    public interface IIconCache
    {
        Task<Optional<Uri>> GetIconUri(Skill skill);
    }
}
