using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public interface ICharacterService
    {
         Task<List<Character>> GetAllCharacter();
         Task<Character> GetCharacterById(int id);
         Task<List<Character>> AddCharacter(Character newCharacter);
    }
}