using Microsoft.EntityFrameworkCore;
using StarWars.Api.Services;
using StarWars.DAL.Entities;
using StarWars.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Services
{
    public class CharacterServices : ICharacterServices
    {
        private readonly StarWarsDbContext _context;
        private readonly IEpisodeServices _episodeServices;
        private readonly IPlanetServices _planetServices;

        public CharacterServices(StarWarsDbContext context, IEpisodeServices episodeServices, IPlanetServices planetServices)
        {
            _context = context;
            _episodeServices = episodeServices;
            _planetServices = planetServices;
        }

        public async Task<List<CharacterModel>> GetAllCharacters()
        {
            var entities = await _context.Characters
                .Include(x => x.Planet)
                .Include(c => c.CharacterEpisodes)
                .ThenInclude(ce => ce.Episode)
                .Include(c => c.CharacterFriends)
                .ThenInclude(c => c.Friend)
                .ToListAsync();

            var models = new List<CharacterModel>();

            foreach (var e in entities)
            {
                var model = MapCharacterEntityToModel(e);
                models.Add(model);
            }

            return models;
        }

        public async Task<CharacterModel> GetCharacterAsync(int? id)
        {
            CharacterEntity entity = await GetEntityByIdAsync(id);
            var model = MapCharacterEntityToModel(entity);

            return model;
        }

        private async Task<CharacterEntity> GetEntityByIdAsync(int? id)
        {
            return await _context.Characters
                .Include(x => x.Planet)
                .Include(c => c.CharacterEpisodes)
                .ThenInclude(ce => ce.Episode)
                .Include(c => c.CharacterFriends)
                .ThenInclude(c => c.Friend)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        private CharacterModel MapCharacterEntityToModel(CharacterEntity e)
        {
            var model = new CharacterModel();

            model.Name = e.Name;

            if (e.Planet != null)
                model.Planet = e.Planet.Name;

            if (e.CharacterEpisodes.Count() > 0 || e.CharacterEpisodes != null)
                model.Episodes = AddEpisodesToModel(e.CharacterEpisodes);

            if (e.CharacterFriends != null || e.FriendCharacters != null)
                model.Friends = AddFriendsToModel(e.CharacterFriends);

            return model;
        }

        private List<string> AddFriendsToModel(IEnumerable<CharacterFriendEntity> characterFriends)
        {
            var result = new List<string>();

            foreach (var cf in characterFriends)
            {
                result.Add(cf.Friend.Name);
            }

            return result;
        }

        private List<string> AddEpisodesToModel(IEnumerable<CharacterEpisodeEntity> characterEpisodes)
        {
            var result = new List<string>();

            foreach (var ce in characterEpisodes)
            {
                result.Add(ce.Episode.Name);
            }

            return result;
        }

        public async Task<int> AddCharacterToDbAsync(CharacterModel model)
        {
            if (_context != null)
            {
                if (await _context.Characters.FirstOrDefaultAsync(e => e.Name == model.Name) == null)
                {
                    var entity = await MapCharacterModelToEntity(model);
                    await _context.Characters.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return entity.Id;
                }
            }

            return 0;
        }

        public async Task<bool> IsCharacterInDatabaseAsync(string modelName)
        {
            if (_context != null)
            {
                if (await _context.Characters.FirstOrDefaultAsync(e => e.Name == modelName) == null)
                {
                    return false;
                }
                else return true;
            }
            return false;
        }

        private async Task<CharacterEntity> MapCharacterModelToEntity(CharacterModel model, bool isUpdate = false, int id = 0)
        {
            var entity = isUpdate ? await GetEntityByIdAsync(id) : new CharacterEntity();

            entity.Name = model.Name;
            if (model.Planet != null)
                entity.Planet = await AddPlanetModelToEntity(model.Planet);

            if (model.Episodes.Count() > 0)
                entity.CharacterEpisodes = await AddCharacterEpisodesToEntity(model.Episodes, entity);

            if (model.Friends.Count() > 0)
                entity.CharacterFriends = await AddCharacterFriendsToEntity(model.Friends, entity);

            return entity;
        }

        private async Task<List<CharacterFriendEntity>> AddCharacterFriendsToEntity(List<string> friends, CharacterEntity entity)
        {
            var characterFriends = new List<CharacterFriendEntity>();

            foreach (var friendName in friends)
            {
                var friend = await ProcessFriend(friendName);

                if (friend != null)
                {
                    var cf = new CharacterFriendEntity()
                    {
                        Character = entity,
                        Friend = friend,
                        FriendId = friend.Id
                    };

                    characterFriends.Add(cf);
                }
            }

            return characterFriends;
        }

        private async Task<CharacterEntity> ProcessFriend(string friendName)
        {
            var entity = await _context.Characters.FirstOrDefaultAsync(x => x.Name == friendName);

            if (entity != null)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }

        private async Task<IEnumerable<CharacterEpisodeEntity>> AddCharacterEpisodesToEntity(List<string> episodes, CharacterEntity e)
        {
            if (_context.Episodes != null)
            {
                var characterEpisodes = new List<CharacterEpisodeEntity>();

                foreach (var epiName in episodes)
                {
                    var episode = await ProcessEpisode(epiName);
                    var ce = new CharacterEpisodeEntity()
                    {
                        Character = e,
                        Episode = episode
                    };
                    characterEpisodes.Add(ce);
                }

                return characterEpisodes;
            }

            return null;
        }

        private async Task<EpisodeEntity> ProcessEpisode(string epiName)
        {
            var entity = await _context.Episodes.FirstOrDefaultAsync(e => e.Name == epiName);

            if (entity != null)
            {
                return entity;
            }
            else
            {
                return await _episodeServices.AddEpisodeToDbAsync(MapEpisodeModelToEntity(epiName));
            }
        }

        private static EpisodeEntity MapEpisodeModelToEntity(string epiName)
        {
            return new EpisodeEntity()
            {
                Name = epiName
            };
        }

        private async Task<PlanetEntity> AddPlanetModelToEntity(string planet)
        {
            if (_context.Planets != null)
            {
                var entity = await _context.Planets.FirstOrDefaultAsync(e => e.Name == planet);

                if (entity != null)
                {
                    return entity;
                }
                else
                {
                    return await _planetServices.AddPlanetToDbAsync(MapPlanetModelToEntity(planet));
                }
            }

            return null;
        }

        private static PlanetEntity MapPlanetModelToEntity(string name)
        {
            return new PlanetEntity()
            {
                Name = name
            };
        }

        public async Task<int> UpdateEntityAsync(int id, CharacterModel model, bool isUpdate = true)
        {
            var entity = await MapCharacterModelToEntity(model, true, id);
            _context.Update(entity);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteByIdAsync(int? id)
        {
            int result = 0;

            if (_context != null)
            {
                var character = await GetEntityByIdAsync(id);

                if (character != null)
                {
                    _context.Characters.Remove(character);
                    result = await _context.SaveChangesAsync();
                }

                return result;
            }

            return result;
        }
    }
}
