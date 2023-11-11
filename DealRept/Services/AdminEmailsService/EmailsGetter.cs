using DealRept.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Services.AdminEmailsService
{
    public class EmailsGetter:IEmailsGetter
    {
        private readonly IdentityContext _context;

        public EmailsGetter(IdentityContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetEmailAddressesAsync(string roleName)
        {
            string roleId = _context.Roles.FirstOrDefault(r => r.Name == roleName).Id;

            var emailAddresses = await (from user in _context.Users
                                     join userRole in _context.UserRoles.Where(ur => ur.RoleId == roleId)
                                     on user.Id equals userRole.UserId
                                     select user.Email).ToListAsync<string>();
            return emailAddresses;
        }

    }
}
