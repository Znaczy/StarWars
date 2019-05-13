using System.Collections.Generic;

namespace StarWars.Models
{
    public class CharacterModel
    {
        public string Name { get; set; }
        public string Planet { get; set; }
        public List<string> Episodes { get; set; }
        public List<string> Friends { get; set; }
    }
}
