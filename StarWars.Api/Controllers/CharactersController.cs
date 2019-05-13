using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars.Models;
using StarWars.Services;

namespace StarWars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterServices _characterServices;

        public CharactersController(ICharacterServices characterServices)
        {
            _characterServices = characterServices;
        }

        // GET api/characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterModel>>> Get()
        {
            return await _characterServices.GetAllCharacters();
        }

        // GET api/characters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterModel>> Get(int id)
        {
            return await _characterServices.GetCharacterAsync(id);
        }

        // POST api/characters
        [HttpPost]
        public async Task<ActionResult<CharacterModel>> PostCharacter(CharacterModel model)
        {
            if (ModelState.IsValid)
            {
                if(!await _characterServices.IsCharacterInDatabaseAsync(model.Name))
                await _characterServices.AddCharacterToDbAsync(model);
            }

            return model;
        }

        // PUT api/characters/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CharacterModel>> Put(int id, CharacterModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _characterServices.IsCharacterInDatabaseAsync(model.Name);

                if (entity)
                {
                    var result = await _characterServices.UpdateEntityAsync(id, model, true);
                }
            }

            return model;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            await _characterServices.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
