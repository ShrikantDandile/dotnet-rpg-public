
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService1:IWeaponService1
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public WeaponService1(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = context;

        }


        public async Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO newWeapons)
        {
           ServiceResponse<GetCharacterDTO> serviceResponse=new ServiceResponse<GetCharacterDTO>();
           try
           {
               Character character=await _dataContext.Characters
               .FirstOrDefaultAsync(c => c.Id== newWeapons.CharacterId && c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
               if (character==null)
               {
                   serviceResponse.Success=false;
                   serviceResponse.Message="Character Not Found!";
                   return serviceResponse;
               }
               Weapon weapon=new Weapon(){
                   CharacterId=newWeapons.CharacterId,
                   Name=newWeapons.Name,
                   Damage=newWeapons.Damage
               };
               await _dataContext.Weapons.AddAsync(weapon);
               await _dataContext.SaveChangesAsync();   
               serviceResponse.Data=_mapper.Map<GetCharacterDTO>(character);
               serviceResponse.Message="Weapon Added Successfully!";
           }
           catch (Exception ex)
           {
              serviceResponse.Message=ex.Message;
              serviceResponse.Success=false;
           }
           return serviceResponse;
        }
    }
    
}