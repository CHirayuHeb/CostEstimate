#pragma checksum "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\MainPage\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2d1853ae5f9b2962a3be45703cf8bd1c73f721ef"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_MainPage_Index), @"mvc.1.0.view", @"/Views/MainPage/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/MainPage/Index.cshtml", typeof(AspNetCore.Views_MainPage_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\_ViewImports.cshtml"
using CostEstimate;

#line default
#line hidden
#line 2 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\MainPage\Index.cshtml"
using CostEstimate.Models.Common;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2d1853ae5f9b2962a3be45703cf8bd1c73f721ef", @"/Views/MainPage/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"03cd043d7b3157285994f12141f5133e7c84d925", @"/Views/_ViewImports.cshtml")]
    public class Views_MainPage_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CostEstimate.Models.Common.Class>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/Home/siteHome.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/budget (1).png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("max-height:100px; max-width:100px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("center"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/enviromentally-friendly.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\MainPage\Index.cshtml"
  
    ViewData["Title"] = "Home | Thai Stanley";
    Layout = "~/Views/Shared/_MenuBarMain.cshtml";

#line default
#line hidden
#line 7 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\MainPage\Index.cshtml"
  
    string _Permission = User.Claims.FirstOrDefault(s => s.Type == "Permission").Value;
    string _NickName = User.Claims.FirstOrDefault(s => s.Type == "NICKNAME")?.Value;
    string _Name = User.Claims.FirstOrDefault(s => s.Type == "Name")?.Value;
    string _SurName = User.Claims.FirstOrDefault(s => s.Type == "SurName")?.Value;
    string _Division = User.Claims.FirstOrDefault(s => s.Type == "Division")?.Value;
    string _Section = User.Claims.FirstOrDefault(s => s.Type == "Section")?.Value;
    string _admin = GlobalVariable.perAdmin.ToUpper();

#line default
#line hidden
            BeginContext(755, 27, true);
            WriteLiteral("<!DOCTYPE html>\r\n\r\n<html>\r\n");
            EndContext();
            BeginContext(782, 162, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c87635f8cd20448488995dbc8411cb07", async() => {
                BeginContext(788, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(794, 56, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "0e3a960837df4b8faf88c0966cb18114", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(850, 87, true);
                WriteLiteral("\r\n    <meta name=\"viewport\" content=\"width=device-width\" />\r\n    <title>Index</title>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(944, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(946, 2454, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3d71c6575ea547779d38c1db970e9045", async() => {
                BeginContext(952, 154, true);
                WriteLiteral("\r\n    <br />\r\n \r\n    <div class=\"row\" style=\"margin-left:10%\">\r\n        <div class=\"col-10\">\r\n\r\n        </div>\r\n        <div class=\"col-80\">\r\n          \r\n");
                EndContext();
                BeginContext(1215, 237, true);
                WriteLiteral("            <div class=\"Homecenterdiv\" id=\"divHomeProduct\" style=\"margin-inline: auto;\">\r\n                <div class=\"row \">\r\n                    <div class=\"col-20\" style=\" display: flex;align-items: center; \">\r\n                        ");
                EndContext();
                BeginContext(1452, 95, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "e8c55a803c1e4810953877094b539c1d", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1547, 597, true);
                WriteLiteral(@"
                    </div>

                    <div class=""col-80"">
                        <div class=""grid-item fw-800"" style=""text-align:left;"">PRODUCT</div>
                        <div class=""grid-item fw-600"" style=""text-align:left;font-size:10px;"">1.COST ESTIMATE</div>
                    </div>

                </div>


            </div>
            <div class=""Homecenterdiv"" id=""divHomeMold"" style=""margin-inline: auto;"">
                <div class=""row "">
                    <div class=""col-20"" style="" display: flex;align-items: center; "">
                        ");
                EndContext();
                BeginContext(2144, 108, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "cbae736f86374d1c81b3e61c111a93fd", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(2252, 537, true);
                WriteLiteral(@"
                    </div>
                    <div class=""col-80"">
                        <div class=""grid-item fw-800"" style=""text-align:left;font-size:15px"">MOLD REQUEST</div>
                        <div class=""grid-item fw-600 Btncenterdiv"" style=""text-align:left;"" onclick=""GoSideMenuNewRequest('New')"">1.SUB MAKER REQUEST SHEET DETAIL</div>
                        <div class=""grid-item fw-600 Btncenterdiv"" style=""text-align:left;"" onclick=""GoSideMenuNewRequest('NewMoldModify')"">2.MOLD MODIFY REQUEST SHEET DETAIL</div>
");
                EndContext();
                BeginContext(2946, 297, true);
                WriteLiteral(@"                        <div class=""grid-item fw-600 Btncenterdiv"" style=""text-align:left;cursor:not-allowed;"">3.OTHER REQUEST SHEET DETAIL</div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""col-10"">

        </div>
    </div>

");
                EndContext();
                BeginContext(3385, 8, true);
                WriteLiteral("\r\n\r\n\r\n\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3400, 11, true);
            WriteLiteral("\r\n</html>\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CostEstimate.Models.Common.Class> Html { get; private set; }
    }
}
#pragma warning restore 1591
