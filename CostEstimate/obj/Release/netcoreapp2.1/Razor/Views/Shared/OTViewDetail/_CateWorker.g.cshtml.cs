#pragma checksum "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "853e184e93eb11f59cb1e12d42402066215357db"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_OTViewDetail__CateWorker), @"mvc.1.0.view", @"/Views/Shared/OTViewDetail/_CateWorker.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/OTViewDetail/_CateWorker.cshtml", typeof(AspNetCore.Views_Shared_OTViewDetail__CateWorker))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"853e184e93eb11f59cb1e12d42402066215357db", @"/Views/Shared/OTViewDetail/_CateWorker.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"03cd043d7b3157285994f12141f5133e7c84d925", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_OTViewDetail__CateWorker : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CostEstimate.Models.New.multiModelCateWorker>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-center"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(53, 105, true);
            WriteLiteral("\r\n<div class=\"d-flex flex-dir-row border-rad py-2 w-fix-240p worker-box\">\r\n    <div class=\"worker-img\">\r\n");
            EndContext();
#line 5 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
         if (Model.image != null){

#line default
#line hidden
            BeginContext(194, 69, true);
            WriteLiteral("            <div class=\"px-2 py-2 img\"><img class=\"wx-100 border-rad\"");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 263, "\"", 281, 1);
#line 6 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
WriteAttributeValue("", 269, Model.image, 269, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(282, 11, true);
            WriteLiteral(" /></div>\r\n");
            EndContext();
#line 7 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
        }
        else{

#line default
#line hidden
            BeginContext(319, 104, true);
            WriteLiteral("            <div class=\"px-2 py-2 img\"><span class=\"material-icon-symbols-outlined\">image</span></div>\r\n");
            EndContext();
#line 10 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
        }

#line default
#line hidden
            BeginContext(434, 112, true);
            WriteLiteral("    </div>\r\n    <div class=\"d-flex flex-dir-col fs-12 worker-newot-details\">\r\n        <div class=\"px-2 empcode\">");
            EndContext();
            BeginContext(547, 32, false);
#line 13 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                             Write(Model.CategoryWorkerList.EmpCode);

#line default
#line hidden
            EndContext();
            BeginContext(579, 34, true);
            WriteLiteral("</div>\r\n        <div class=\"px-2\">");
            EndContext();
            BeginContext(614, 32, false);
#line 14 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                     Write(Model.CategoryWorkerList.PriName);

#line default
#line hidden
            EndContext();
            BeginContext(646, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(648, 29, false);
#line 14 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                                                       Write(Model.CategoryWorkerList.Name);

#line default
#line hidden
            EndContext();
            BeginContext(677, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(679, 32, false);
#line 14 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                                                                                      Write(Model.CategoryWorkerList.Surname);

#line default
#line hidden
            EndContext();
            BeginContext(711, 80, true);
            WriteLiteral("</div>\r\n        <select class=\"form-control py-1 px-1 h-fix-30p job\" disabled>\r\n");
            EndContext();
#line 16 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
             foreach (var item in Model.Jobs.Select(s => s.mjJobCode).OrderBy(o => o).Distinct())
            {
                string JobCode = Model.CategoryWorkerList.Job is null
                    ? ""
                    : Model.CategoryWorkerList.Job;

                if (item == JobCode)
                {

#line default
#line hidden
            BeginContext(1114, 20, true);
            WriteLiteral("                    ");
            EndContext();
            BeginContext(1134, 65, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "24af06fe7eb24d37844cd49f2514060a", async() => {
                BeginContext(1186, 4, false);
#line 24 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                                                                  Write(item);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginWriteTagHelperAttribute();
#line 24 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                                           WriteLiteral(item);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1199, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 25 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                }
                else
                {

#line default
#line hidden
            BeginContext(1261, 20, true);
            WriteLiteral("                    ");
            EndContext();
            BeginContext(1281, 56, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "72edf537b7b74b65a2ddfe44ccac4e6f", async() => {
                BeginContext(1324, 4, false);
#line 28 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                                                         Write(item);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginWriteTagHelperAttribute();
#line 28 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                                           WriteLiteral(item);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
#line 28 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\Shared\OTViewDetail\_CateWorker.cshtml"
                                                                            }

            }

#line default
#line hidden
            BeginContext(1357, 39, true);
            WriteLiteral("        </select>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CostEstimate.Models.New.multiModelCateWorker> Html { get; private set; }
    }
}
#pragma warning restore 1591
