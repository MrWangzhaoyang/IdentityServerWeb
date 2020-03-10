using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityServerWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerWeb.Data
{
    public class ApplicationDbContext
: IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>,
ApplicationUserRole, IdentityUserLogin<int>,
IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
