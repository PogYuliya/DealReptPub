using Microsoft.AspNetCore.Mvc.Rendering;

namespace DealRept.Models.ViewModel
{
    public class ViewModelBranchFactory
    {
        public static BranchViewModel Create(Branch branch, SelectList cities)
        {
            return new BranchViewModel
            {
                Branch = branch,
                Cities=cities
            };
        }

        public static BranchViewModel Edit(Branch branch, SelectList cities)
        {
            return new BranchViewModel
            {
                Branch = branch,
                Cities=cities
            };
        }

        public static BranchViewModel Delete(Branch branch, string errorMessage)
        {
            return new BranchViewModel
            {
                Branch = branch,
                ErrorMessage = errorMessage
            };
        }
    }
}
