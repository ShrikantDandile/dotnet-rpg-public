using System.Threading.Tasks;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService1 _weaponservice;
        public WeaponController(IWeaponService1 weaponservice)
        {
            _weaponservice = weaponservice;
        }
        [HttpPost]
        public async Task<IActionResult> AddWeapon(AddWeaponDTO newWeapon)
        {
            return Ok(await _weaponservice.AddWeapon(newWeapon));
        }
    }
}