namespace DealRept.Services
{
    public interface ITooltipService
    {
        public string ToolTipPassRequirements { get; }
    }
    public class ToolTipService:ITooltipService
    {

        string ITooltipService.ToolTipPassRequirements => "<h4 class='lead'>Password must</h4>" +
        "<ul>" +
        "<li class='text-left'>Have one digit</li>" +
        "<li class='text-left'>Have one uppercase charachter</li>" +
        "<li class='text-left'>Have one lowercase charachter</li>" +
        "<li class='text-left'>Have one special charachter</li>" +
        "<li class='text-left'>Have eight charachters minimum</li>" +
        "<li class='text-left'>Not contain word password</li>" +
        "<li class='text-left'>Not coincide with email address</li>" +
        "</ul>";
    }
}
