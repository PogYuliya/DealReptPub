namespace DealRept.Models
{
    public class CurrentEvent:Event
    {
        /*Foreign key*/
        public int CurrentContractID { get; set; }

        /*Navigation properties*/

        public CurrentContract CurrentContract { get; set; }
    }
}
