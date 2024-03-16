using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace DealRept.TagHelpers
{
    [HtmlTargetElement(Attributes="is-active-route")]
    public class ActiveClassTagHelper:AnchorTagHelper
    {
        public ActiveClassTagHelper(IHtmlGenerator generator):base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routeData = ViewContext.RouteData.Values;

            string currentController = routeData["controller"] as string;
            string currentAction = routeData["action"] as string;
            bool result = false;

            if (currentController.ToLower() == "account")
            {
                if (currentAction.ToLower() != "login")
                {
                    if (currentAction.ToLower().Contains("registration") || currentAction.ToLower() == "confirmemail")
                    {
                        currentAction = "register";
                    }

                    else if (currentAction.ToLower().Contains("password"))
                    {
                        currentAction = "login";
                    }
                    else if (currentAction.ToLower().Contains("access") ||
                        currentAction.ToLower() == "sendmessagetoadmins")
                    {
                        currentAction = "details";
                    }
                }
                result = string.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase);
            }
            else
            {

                if (string.IsNullOrWhiteSpace(Controller) && context.AllAttributes["class"].Value.ToString().Contains("auxiliaries"))
                {
                    Controller = currentController.ToLower() switch
                    {
                        "cities" => "Cities",
                        "banks" => "Banks",
                        _ => "",
                    };
                }
                if (!string.IsNullOrWhiteSpace(Controller))
                {
                    result = string.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase);
                }

            }
                    
            if (result)
            {
                string existingClasses = output.Attributes["class"].Value.ToString();
                if (output.Attributes["class"]!=null)
                {
                    output.Attributes.Remove(output.Attributes["class"]);
                }

                output.Attributes.Add("class", $"{existingClasses} active");
            }
        }
    }
}
