#pragma checksum "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\Account\AccessDenied.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6ff140e281f07263f1b3025a2f3accc29ebea93e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_AccessDenied), @"mvc.1.0.view", @"/Views/Account/AccessDenied.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\_ViewImports.cshtml"
using System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\_ViewImports.cshtml"
using DealRept.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\_ViewImports.cshtml"
using DealRept.Models.ViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\_ViewImports.cshtml"
using DealRept.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\_ViewImports.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6ff140e281f07263f1b3025a2f3accc29ebea93e", @"/Views/Account/AccessDenied.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"47b6e4332ccc8d22d3fe9828086b5379ca3b76c4", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_AccessDenied : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("px-1 pb-1"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "D:\Documents\Visual Studio 2019\Education\Practice\CopyForPubRepositoryDealRept\DealRept\DealRept\Views\Account\AccessDenied.cshtml"
  
    ViewBag.Title = "Access denied";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("Header", async() => {
                WriteLiteral("\r\n\r\n    <div class=\"orangeLink\">\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6ff140e281f07263f1b3025a2f3accc29ebea93e5487", async() => {
                    WriteLiteral("<i class=\"fi-xtluxl-sign-in-thin\"></i> Login details");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(" <span class=\"text-nowrap lead align-middle\">&sol; Access</span>\r\n    </div>\r\n");
            }
            );
            WriteLiteral(@"
<section id=""accessDenied"">
    <div class=""container"">
        <div class=""row"">
            <div class=""col-md-6 mx-auto my-5 px-1"">
                <div class=""card p-5 card-body bg-primary scaleBlueButton"">
                    <div class=""card-title text-center"">
                        <h2 class=""text-white display-4"">Access denied</h2>
                    </div>
                    <div class=""card-text lead text-white"">
                            You do not have access to this resource.
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>




");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
