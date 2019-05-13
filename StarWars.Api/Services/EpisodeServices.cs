using StarWars.DAL.Entities;
using System.Threading.Tasks;

namespace StarWars.Api.Services
{
    public class EpisodeServices : IEpisodeServices
    {
        private readonly StarWarsDbContext _context;

        public EpisodeServices(StarWarsDbContext context)
        {
            _context = context;
        }

        public async Task<EpisodeEntity> AddEpisodeToDbAsync(EpisodeEntity episode)
        {
            await _context.Episodes.AddAsync(episode);
            await _context.SaveChangesAsync();

            return episode;
        }
    }
}
