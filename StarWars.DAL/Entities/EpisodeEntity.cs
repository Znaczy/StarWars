using System.Collections.Generic;

namespace StarWars.DAL.Entities
{
    public class EpisodeEntity
    {
        public EpisodeEntity()
        {
            CharacterEpisodes = new HashSet<CharacterEpisodeEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<CharacterEpisodeEntity> CharacterEpisodes { get; set; }
    }
}
