namespace StarWars.DAL.Entities
{
    public class CharacterFriendEntity
    {
        public int CharacterId { get; set; }
        public virtual CharacterEntity Character { get; set; }

        public int FriendId { get; set; }
        public virtual CharacterEntity Friend { get; set; }
    }
}
