namespace DealRept.Models.ViewModel
{
    public class ModelForDirtyCheckModal
    {
        public string ID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int? RouteData { get; set; }
        public string Entity { get; set; }
    }
}
