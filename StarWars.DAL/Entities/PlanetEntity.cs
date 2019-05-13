using System.Collections.Generic;

namespace StarWars.DAL.Entities
{
    public class PlanetEntity
    {
        public PlanetEntity()
        {
            Characters = new HashSet<CharacterEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<CharacterEntity> Characters { get; set; }
    }
}
