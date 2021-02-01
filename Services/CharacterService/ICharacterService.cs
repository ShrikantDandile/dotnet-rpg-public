using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public interface ICharacterService
    {
         Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacter();
         Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id);
         Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter);
         Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter);
         Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id);
    }
}