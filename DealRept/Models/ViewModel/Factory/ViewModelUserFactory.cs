using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Models.ViewModel
{
    public class ViewModelUserFactory
    {
        public static UserViewModel Details(User user, int timeZoneDifference)
        {
            return new UserViewModel
            {
                User = user,
                TimeZoneDifference = timeZoneDifference
            };
        }

        public static UserViewModel Edit(User user, List<AssignedRoleViewModel> allRolesWithAssinment, 
            LockoutEndDate lockOutEndDate,int timeZoneDifference, bool oldIsApproved)
        {
            return new UserViewModel
            {
                User = user,
                AllRolesWithAssignment = allRolesWithAssinment,
                LockoutEndDate = lockOutEndDate,
                TimeZoneDifference = timeZoneDifference,
                OldApproved=oldIsApproved
            };
        }

        public static UserViewModel Delete(User user, string errorMessage)
        {
            return new UserViewModel
            {
                User = user,
                ErrorMessage = errorMessage
            };
        }
    }
}
