using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Airline.AppData.Model;
using System.Security.Claims;

namespace Airline.AppLogic.Service
{
    public class AppClaimIdentityFactory : ClaimsIdentityFactory<AppUser, Guid>
    {
        public override async Task<ClaimsIdentity> CreateAsync(UserManager<AppUser, Guid> manager, AppUser user, string authenticationType)
        {
            var claim = await base.CreateAsync(manager, user, authenticationType);

            claim.AddClaim(new Claim("FirstName", user.FirstName, ClaimValueTypes.String));
            claim.AddClaim(new Claim("UserId", user.Id.ToString(), ClaimValueTypes.String));
            claim.AddClaim(new Claim(ClaimTypes.Surname, user.SecondName, ClaimValueTypes.String));

            return claim;
        }
    }
}
