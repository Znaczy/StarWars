using System.Collections.Generic;

namespace StarWars.DAL.Entities
{
    public class CharacterEntity
    {
        public CharacterEntity()
        {
            CharacterEpisodes = new HashSet<CharacterEpisodeEntity>();
            CharacterFriends = new HashSet<CharacterFriendEntity>();
            FriendCharacters = new HashSet<CharacterFriendEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int? PlanetId { get; set; }
        public virtual PlanetEntity Planet { get; set; }

        public virtual IEnumerable<CharacterEpisodeEntity> CharacterEpisodes { get; set; }

        public virtual ICollection<CharacterFriendEntity> CharacterFriends { get; set; }
        public virtual ICollection<CharacterFriendEntity> FriendCharacters { get; set; }
    }
}