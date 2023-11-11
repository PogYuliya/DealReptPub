using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DealRept.Models.ViewModel
{
    public class UserViewModel: IValidatableObject
    {
        public User User { get; set; }
        public List<AssignedRoleViewModel> AllRolesWithAssignment { get; set; }
        = new List<AssignedRoleViewModel>();
        public int TimeZoneDifference { get; set; }

        public LockoutEndDate LockoutEndDate { get; set; }


        [Display(Name = "Important Information")]
        public string ErrorMessage { get; set; }

        public bool OldApproved { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!User.EmailConfirmed && User.IsApproved)
            {
                yield return new ValidationResult(
                    "To be approved User must confirm his (her) email.",
                    new[] { "User.IsApproved" });
            }

            if (!AllRolesWithAssignment.Any(r=>r.Assigned==true))
            {
                yield return new ValidationResult(
                    "User must have atleast one role.",
                    new[] { nameof(AllRolesWithAssignment) });
            }
            else
            {
                if ((AllRolesWithAssignment.Where(r => r.Assigned == true).Count() > 1)
                && (AllRolesWithAssignment.Any(r => r.RoleName.ToLower() == "justregistered" && r.Assigned == true)
                || AllRolesWithAssignment.Any(r => r.RoleName.ToLower() == "suspended" && r.Assigned == true)))
                {
                    yield return new ValidationResult(
                        "JustRegistered or Suspended User can't have another role.",
                        new[] { nameof(AllRolesWithAssignment) });
                }
                if (User.IsApproved == true&&AllRolesWithAssignment.Any(r => r.RoleName.ToLower() == "justregistered" && r.Assigned == true))
                {
                    yield return new ValidationResult(
                       "JustRegistered User can't be approved.",
                       new[] { nameof(AllRolesWithAssignment) });
                }
                if (User.IsApproved == false && AllRolesWithAssignment.Any(r => r.RoleName.ToLower() != "justregistered" && r.Assigned == true))
                {
                    yield return new ValidationResult(
                       "User with not approved account can't have active roles.",
                       new[] { nameof(AllRolesWithAssignment) });
                }
            }
        }
    }
}
