namespace DealRept.Models
{
    public class CurrentDocument:Document
    {

        /*Foreign key*/
        public int CurrentContractID { get; set; }

        /*Navigation properties*/

        public CurrentContract CurrentContract { get; set; }

        
    }
}
