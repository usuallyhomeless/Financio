using FinancioAPI.Models;
using FinancioAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinancioAPI.Entities
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Financiouser GetById(int id);
    }

    public class UserService : IUserService
    {        
        
        private readonly financioContext _context;
        public IConfiguration Configuration { get; }

        public UserService(financioContext context, IConfiguration _configuration)
        {            
            _context = context;
            Configuration = _configuration;
        }

        public Financiouser GetById(int id)
        {
            return (from x in this._context.Financiouser where x.Id == id select x).FirstOrDefault();
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = this._context.Financiouser.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            if (user == null) return null;            
            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(token);
        }                

        private string GenerateJwtToken(Financiouser user)
        {            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.Configuration.GetSection("JwtConfig").GetSection("Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.Now.AddMinutes(180),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
