using StarWars.DAL.Entities;
using System.Threading.Tasks;

namespace StarWars.Api.Services
{
    public interface IPlanetServices
    {
        Task<PlanetEntity> AddPlanetToDbAsync(PlanetEntity planet);
    }
}