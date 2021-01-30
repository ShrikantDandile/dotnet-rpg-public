using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
            
        }
         private static List<Character> knights=new List<Character>(){
            new Character(),
            new Character(){Id=1, Name="Sam"}
        };
         public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {             
          ServiceResponse<List<GetCharacterDTO>> serviceResponse=new ServiceResponse<List<GetCharacterDTO>>();
          Character character=_mapper.Map<Character>(newCharacter);
          character.Id=knights.Max(x => x.Id)+1;
          knights.Add(character); 
          serviceResponse.Data=(knights.Select(x => _mapper.Map<GetCharacterDTO>(x))).ToList();                          
          return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacter()
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse =new ServiceResponse<List<GetCharacterDTO>>();
            serviceResponse.Data=(knights.Select(x => _mapper.Map<GetCharacterDTO>(x))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse=new ServiceResponse<GetCharacterDTO>();
           serviceResponse.Data=_mapper.Map<GetCharacterDTO>(knights.FirstOrDefault(x => x.Id == id));
           return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse=new ServiceResponse<GetCharacterDTO>();
            try
            {
                 Character character=knights.FirstOrDefault(x => x.Id==updateCharacter.Id);
            character.Class=updateCharacter.Class;
            character.Defense=updateCharacter.Defense;
            character.HitPoints=updateCharacter.HitPoints;
            character.Intelligence=updateCharacter.Intelligence;
            character.Name=updateCharacter.Name;
            character.Strenghts=updateCharacter.Strenghts;
            serviceResponse.Data=_mapper.Map<GetCharacterDTO>(character);      
            }
            catch (Exception ex)
            {
               serviceResponse.Success=false;
               serviceResponse.Message=ex.Message;
            }
                             
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse=new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                 Character character=knights.First(x => x.Id==id);
            knights.Remove(character);
            serviceResponse.Data=(knights.Select(x => _mapper.Map<GetCharacterDTO>(x))).ToList();      
            }
            catch (Exception ex)
            {
               serviceResponse.Success=false;
               serviceResponse.Message=ex.Message;
            }                             
            return serviceResponse;
        }
    }
}
