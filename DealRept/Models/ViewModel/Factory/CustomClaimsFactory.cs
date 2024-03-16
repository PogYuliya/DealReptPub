using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DealRept.Models.ViewModel.Factory
{
    public class CustomClaimsFactory:UserClaimsPrincipalFactory<User>
    {
        public CustomClaimsFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            ClaimsIdentity identity= await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("firstname", user.FirstName));
            identity.AddClaim(new Claim("lastname", user.LastName));
            identity.AddClaim(new Claim("middlename", user.MiddleName??"noMiddleName"));
            identity.AddClaim(new Claim("fullName", user.UserFullName));
            identity.AddClaim(new Claim("employeeNumber", user.EmployeeNumber.ToString()));
            identity.AddClaim(new Claim("isApproved", user.IsApproved.ToString()));
            identity.AddClaim(new Claim("registrationDate", user.RegistrationDateUTC.ToString()));


            var roles = await UserManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            return identity;
        }

    }
    
}
