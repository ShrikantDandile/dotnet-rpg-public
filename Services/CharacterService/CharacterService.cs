using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharacterService(IMapper mapper, DataContext dataContext, IHttpContextAccessor  httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor=httpContextAccessor;
        }   

        int GetUserId()=> int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User=await _dataContext.Users.FirstOrDefaultAsync(x => x.Id==GetUserId()); 
            await _dataContext.Characters.AddAsync(character);
            await _dataContext.SaveChangesAsync();
            serviceResponse.Data = (_dataContext.Characters.Where(x => x.User.Id ==GetUserId()).Select(x => _mapper.Map<GetCharacterDTO>(x))).ToList();
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacter()
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            List<Character> dbCharacters=await _dataContext.Characters.Where(x => x.User.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = (dbCharacters.Select(x => _mapper.Map<GetCharacterDTO>(x))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            Character dbcharacter=await _dataContext.Characters.FirstOrDefaultAsync(x => x.Id==id && x.User.Id==GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbcharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                Character character = await _dataContext.Characters.FirstOrDefaultAsync(x => x.Id == updateCharacter.Id);
                character.Class = updateCharacter.Class;
                character.Defense = updateCharacter.Defense;
                character.HitPoints = updateCharacter.HitPoints;
                character.Intelligence = updateCharacter.Intelligence;
                character.Name = updateCharacter.Name;
                character.Strenghts = updateCharacter.Strenghts;
                _dataContext.Characters.Update(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                Character character = await _dataContext.Characters.FirstOrDefaultAsync(x => x.Id == id && x.User.Id == GetUserId());
              if(character != null){
                _dataContext.Characters.Remove(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Message="Character Deleted Successfully!";
                    serviceResponse.Data = (_dataContext.Characters.Where(x => x.User.Id ==GetUserId()).Select(x => _mapper.Map<GetCharacterDTO>(x))).ToList();
                }else{
serviceResponse.Success=false;
serviceResponse.Message="Character not found!";
                              }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
