#pragma checksum "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e3e6a6334c5504a76c33c5d23f8931644bc03a00"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
#line 2 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Home\Index.cshtml"
using CostEstimate.Models.Common;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e3e6a6334c5504a76c33c5d23f8931644bc03a00", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"03cd043d7b3157285994f12141f5133e7c84d925", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CostEstimate.Models.Common.Class>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/Home/siteHome.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/product.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("max-height:100px; max-width:100px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("center"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/mold.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/cost.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home | Thai Stanley";
    Layout = "~/Views/Shared/_MenuBar.cshtml";

#line default
#line hidden
#line 7 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Home\Index.cshtml"
  
    string _Permission = User.Claims.FirstOrDefault(s => s.Type == "Permission").Value;
    string _NickName = User.Claims.FirstOrDefault(s => s.Type == "NICKNAME")?.Value;
    string _Name = User.Claims.FirstOrDefault(s => s.Type == "Name")?.Value;
    string _SurName = User.Claims.FirstOrDefault(s => s.Type == "SurName")?.Value;
    string _Division = User.Claims.FirstOrDefault(s => s.Type == "Division")?.Value;
    string _Section = User.Claims.FirstOrDefault(s => s.Type == "Section")?.Value;
    string _admin = GlobalVariable.perAdmin.ToUpper();

#line default
#line hidden
            BeginContext(751, 56, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "677f7e9b8e2e4fa6925bc11c40c28d7a", async() => {
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
            BeginContext(807, 230, true);
            WriteLiteral("\r\n\r\n<script type=\"text/javascript\">document.onreadystatechange = () => { if (document.readyState === \"interactive\") { PositionY(\"Home\"); } }</script>\r\n<div class=\"home-content\">\r\n    <div id=\"DisplayContent\" class=\"content-box\">\r\n");
            EndContext();
            BeginContext(1082, 58, true);
            WriteLiteral("        <div style=\"margin:2%\" class=\"content-text\">\r\n\r\n\r\n");
            EndContext();
            BeginContext(1290, 38, true);
            WriteLiteral("            <div style=\"width:100%\">\r\n");
            EndContext();
            BeginContext(1441, 253, true);
            WriteLiteral("                <div class=\"Homecenterdiv\" id=\"divHomeProduct\" style=\"margin-inline: auto;\">\r\n                    <div class=\"row \">\r\n                        <div class=\"col-20\" style=\" display: flex;align-items: center; \">\r\n                            ");
            EndContext();
            BeginContext(1694, 92, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "d45f101637ca4593bd9d0a8bb91378dd", async() => {
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
            BeginContext(1786, 642, true);
            WriteLiteral(@"
                        </div>

                        <div class=""col-80"">
                            <div class=""grid-item fw-800"" style=""text-align:left;"">PRODUCT</div>
                            <div class=""grid-item fw-600"" style=""text-align:left;font-size:10px;"">1.COST ESTIMATE</div>
                        </div>

                    </div>


                </div>
                <div class=""Homecenterdiv"" id=""divHomeMold"" style=""margin-inline: auto;"" >
                    <div class=""row "">
                        <div class=""col-20"" style="" display: flex;align-items: center; "">
                            ");
            EndContext();
            BeginContext(2428, 89, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c771898412274ed1854d2363fd39c8a8", async() => {
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
            BeginContext(2517, 687, true);
            WriteLiteral(@"
                        </div>
                        <div class=""col-80"">
                            <div class=""grid-item fw-800"" style=""text-align:left;"">MOLD</div>
                            <div class=""grid-item fw-600 Btncenterdiv"" style=""text-align:left;"" onclick=""GoSideMenuNewRequest('New')"">1.SUB MAKER REQUEST SHEET DETAIL</div>
                            <div class=""grid-item fw-600 Btncenterdiv"" style=""text-align:left;""  onclick=""GoSideMenuNewRequest('NewMoldModify')"">2.MOLD MODIFY REQUEST SHEET DETAIL</div>
                        </div>

                    </div>



                </div>
                <div class=""centerdiv"" id=""divHomeCOstPlan""");
            EndContext();
            BeginWriteAttribute("style", " style=\"", 3204, "\"", 3309, 1);
#line 59 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Home\Index.cshtml"
WriteAttributeValue("", 3212, _Permission.ToUpper() !=_admin ? "margin-inline: auto;display: none;" : "margin-inline: auto;", 3212, 97, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3310, 207, true);
            WriteLiteral(" onclick=\"GoSideMenuNewRequest(\'MasterCost\')\">\r\n                    <div class=\"row \">\r\n                        <div class=\"col-20\" style=\" display: flex;align-items: center; \">\r\n                            ");
            EndContext();
            BeginContext(3517, 89, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "010209a10ccc4a0b8d111e256072820a", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
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
            BeginContext(3606, 830, true);
            WriteLiteral(@"
                        </div>
                        <div class=""col-80"">
                            <div class=""grid-item fw-800"" style=""text-align:left;"">COST PLANNING DIVISION</div>
                            <div class=""grid-item fw-600"" style=""text-align:left;font-size:10px;"">1.COST PLANNING</div>
                            <div class=""grid-item fw-600"" style=""text-align:left;font-size:10px;"">2.MODEL PLANNING</div>
                            <div class=""grid-item fw-600"" style=""text-align:left;font-size:10px;"">3.MASTER PROCESS</div>
                            <div class=""grid-item fw-600"" style=""text-align:left;font-size:10px;"">4.MASTER MODEL</div>
                        </div>

                    </div>


                </div>
            </div>


        </div>
    </div>
</div>

");
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
