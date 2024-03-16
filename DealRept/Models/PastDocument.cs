namespace DealRept.Models
{
    public class PastDocument:Document
    {
       /*Foreighn keys*/

        public int PastContractID { get; set; }

        /*Navigation properties*/

        public PastContract PastContract { get; set; }
    }
}
