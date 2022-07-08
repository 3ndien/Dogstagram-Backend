namespace Dogstagram.WebApi.Features.Identity
{
    using Dogstagram.WebApi.Data;
    using Dogstagram.WebApi.Data.Models;
    using Dogstagram.WebApi.Features.Identity.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class IdentityService : IIdentityService
    {
        private readonly DogstagramDbContext dbContext;
        private readonly RoleManager<UserRole> roleManager;
        private readonly UserManager<User> userManager;

        public IdentityService(
            DogstagramDbContext dbContext,
            RoleManager<UserRole> roleManager,
            UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public string GenerateJwtToken(string userId, string userName, IEnumerable<string> userRoles, string secret)
        {
            var responseModel = new LoginResponseModel();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                responseModel.Roles.Add(role);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }

        public async Task<User> GetUserByNameIgnoreFilters(string userName)
        {
            var result = await this.dbContext.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.UserName == userName);
            return result!;
        }
    }
}
