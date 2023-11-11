using Microsoft.AspNetCore.Mvc.Rendering;

namespace DealRept.Models.ViewModel
{
    public class ViewModelSupplierFactory
    {
        public static SupplierViewModel Create(Supplier supplier, SelectList specializations,
            SelectList cities, SelectList banks)
        {
            return new SupplierViewModel
            {
                Supplier = supplier,
                Specializations = specializations,
                Cities = cities,
                Banks = banks
            };
        }

        public static SupplierViewModel Edit(Supplier supplier,
            SelectList specializations, SelectList cities, SelectList banks)
        {
            return new SupplierViewModel
            {
                Supplier = supplier,
                Specializations = specializations,
                Banks = banks,
                Cities = cities
            };
        }

        public static SupplierViewModel Delete(Supplier supplier, string errorMessage)
        {
            return new SupplierViewModel
            {
                Supplier = supplier,
                ErrorMessage = errorMessage
            };
        }
    }
}
