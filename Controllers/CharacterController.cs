using Microsoft.AspNetCore.Mvc;
using dotnet_rpg.Models;
using System.Collections.Generic;
using System.Linq;
using dotnet_rpg.Services.CharacterService;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;

        }
        

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            int userId=int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier ).Value);
            return Ok(await _characterService.GetAllCharacter(userId));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }
        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterDTO character)
        {
            return Ok(await _characterService.AddCharacter(character));
        }

         [HttpPut]
        public async Task<IActionResult> UpdateCharacter(UpdateCharacterDTO character)
        {
            var serviceResponse=await _characterService.UpdateCharacter(character);
            if (serviceResponse.Data==null)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCharacter(int id){
            var serviceResponse=await _characterService.DeleteCharacter(id);
             if (serviceResponse.Data==null)
            {
                return NotFound(serviceResponse);
            }
            return Ok(serviceResponse);
        }
    }
}