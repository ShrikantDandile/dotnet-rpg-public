using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
         private static List<Character> knights=new List<Character>(){
            new Character(),
            new Character(){Id=1, Name="Sam"}
        };
        public async Task<List<Character>> AddCharacter(Character newCharacter)
        {
            knights.Add(newCharacter);
            return knights;
        }

        public async Task<List<Character>> GetAllCharacter()
        {
            return knights;
        }

        public async Task<Character> GetCharacterById(int id)
        {
           return knights.FirstOrDefault(x => x.Id == id);
        }
    }
}