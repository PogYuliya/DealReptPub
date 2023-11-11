namespace DealRept.Models.ViewModel
{
    public class ViewModelBankFactory
    {
        public static BankViewModel Delete(Bank bank, string errorMessage)
        {
            return new BankViewModel
            {
                Bank = bank,
                ErrorMessage = errorMessage
            };
        }
    }
}
