using StarWars.DAL.Entities;
using System.Threading.Tasks;

namespace StarWars.Api.Services
{
    public class PlanetServices : IPlanetServices
    {
        private readonly StarWarsDbContext _context;

        public PlanetServices(StarWarsDbContext context)
        {
            _context = context;
        }

        public async Task<PlanetEntity> AddPlanetToDbAsync(PlanetEntity planet)
        {
            await _context.Planets.AddAsync(planet);
            await _context.SaveChangesAsync();

            return planet;
        }

    }
}
