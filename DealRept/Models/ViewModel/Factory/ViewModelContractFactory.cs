using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DealRept.Models.ViewModel
{
    public class ViewModelContractFactory
    {
        public static  ContractViewModel Create(Contract contract,
            SelectList suppliers, SelectList branches)
        {
            return new ContractViewModel
            {
                Contract = contract,
                Suppliers=suppliers,
                Branches=branches
            };
        }

        public static ContractViewModel Edit(Contract contract, 
             List<CurrentDocumentVM> currentDocumentsToEdit,
            SelectList suppliers, SelectList branches, SelectList contractStatuses)
        {
            return new ContractViewModel
            {
                Contract = contract,
                ContractStatuses = contractStatuses,
                CurrentDocumentsToEdit = currentDocumentsToEdit,
                Branches=branches,
                Suppliers=suppliers
            };
        }

        public static ContractViewModel Delete(Contract contract, string errorMessage)
        {
            return new ContractViewModel
            {
                Contract = contract,
               //ErrorMessage=errorMessage
            };
        }

        public static ContractViewModel Details(Contract contract)
        {
            return new ContractViewModel
            {
                Contract = contract
            };  
        }
    }
}
