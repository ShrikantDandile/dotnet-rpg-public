using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.User;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
       [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegister){
            ServiceResponse<int> response=await _authRepository.Register(
                new User{Username=userRegister.Username},userRegister.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }else{
                return Ok(response);
            }

        }

       [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO request){
            ServiceResponse<string> response=await _authRepository.Login(
                request.Username,
                request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }else{
                return Ok(response);
            }

        }
    }
}