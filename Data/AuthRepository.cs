
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace dotnet_rpg.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public AuthRepository(DataContext dataContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dataContext = dataContext;

        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found!";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password!";
            }
            else
            {
                response.Data = CreateTokent(user);
                response.Message = "Login Successful!";
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (await UserExists(user.Username))
            {
                response.Success = true;
                response.Message = "User Exists Already!";
                return response;
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            response.Data = user.Id;
            response.Message = "User Registered Successfully!";
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dataContext.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        
        private string CreateTokent(User user)
        {
            List<Claim> claims=new List<Claim>(){
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username)
            };
             SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
             JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
             return tokenHandler.WriteToken(token);
        }
    }
}