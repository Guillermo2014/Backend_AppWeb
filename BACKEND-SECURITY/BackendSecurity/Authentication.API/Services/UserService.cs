using Authentication.API.Domain.Models;
using Authentication.API.Domain.Services;
using Authentication.API.Domain.Services.Communication;
using Authentication.API.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.API.Services
{
    public class UserService : IUserService
    {
        // TODO: Replace by Persistence Implementation
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Guillermo", LastName = "Rosales"
            , UserName = "aries_20_15@hotmail.com", Password = "1q2w3e4r"}
        };

        // Secret Settings
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest body)
        {
            var user = _users.SingleOrDefault(x =>
            x.UserName == body.Username && x.Password == body.Password);
            //Return null when user not found
            if (user == null)
            {
                return null;
            }
            var token = generateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
