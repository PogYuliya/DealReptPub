using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealRept.Services.AdminEmailsService
{
    public interface IEmailsGetter
    {
        Task<List<string>> GetEmailAddressesAsync(string roleName);
    }
}
