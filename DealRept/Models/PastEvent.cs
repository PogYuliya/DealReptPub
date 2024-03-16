namespace DealRept.Models
{
    public class PastEvent:Event
    {
        /*Foreighn keys*/

        public int PastContractID { get; set; }

        /*Navigation properties*/

        public PastContract PastContract { get; set; }
    }
}
