using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServerWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerWeb.Service
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileService(
            UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
        }

        private async Task<List<Claim>> GetClaimFromUserAsync(ApplicationUser user)
        {
            var claim = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claim.Add(new Claim(JwtClaimTypes.Role, role));
            }

            if (!string.IsNullOrWhiteSpace(user.RealName))
            {
                claim.Add(new Claim("RealName", user.RealName));
            }
            if (!string.IsNullOrWhiteSpace(user.LoginName))
            {
                claim.Add(new Claim("LoginName", user.LoginName));
            }

            return claim;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Value == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            var claim = await GetClaimFromUserAsync(user);
            context.IssuedClaims = claim;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;

            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Value == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IsActive = user != null;
        }
    }
}
