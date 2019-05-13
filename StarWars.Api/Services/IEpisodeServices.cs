using StarWars.DAL.Entities;
using System.Threading.Tasks;

namespace StarWars.Api.Services
{
    public interface IEpisodeServices
    {
        Task<EpisodeEntity> AddEpisodeToDbAsync(EpisodeEntity episode);
    }
}