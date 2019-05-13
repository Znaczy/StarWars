using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars.Models;

namespace StarWars.Services
{
    public interface ICharacterServices
    {
        Task<List<CharacterModel>> GetAllCharacters();
        Task<CharacterModel> GetCharacterAsync(int? id);
        Task<int> AddCharacterToDbAsync(CharacterModel model);
        Task<int> DeleteByIdAsync(int? id);
        Task<bool> IsCharacterInDatabaseAsync(string modelName);
        Task<int> UpdateEntityAsync(int id, CharacterModel model, bool isUpdate);
    }
}
