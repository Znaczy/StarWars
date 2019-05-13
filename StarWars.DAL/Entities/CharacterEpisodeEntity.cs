namespace StarWars.DAL.Entities
{
    public class CharacterEpisodeEntity
    {
        public int CharacterId { get; set; }
        public CharacterEntity Character { get; set; }

        public int EpisodeId { get; set; }
        public EpisodeEntity Episode { get; set; }
    }
}