#pragma checksum "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "292ea284d4f4ca62b848e2928df71c0f5775d7d1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_New__OTAddWorker), @"mvc.1.0.view", @"/Views/New/_OTAddWorker.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/New/_OTAddWorker.cshtml", typeof(AspNetCore.Views_New__OTAddWorker))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"292ea284d4f4ca62b848e2928df71c0f5775d7d1", @"/Views/New/_OTAddWorker.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"03cd043d7b3157285994f12141f5133e7c84d925", @"/Views/_ViewImports.cshtml")]
    public class Views_New__OTAddWorker : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<CostEstimate.Models.New.multiModelCateWorker>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
  string step = "4";

#line default
#line hidden
            BeginContext(89, 22, true);
            WriteLiteral("<div class=\"container\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 111, "\"", 137, 2);
            WriteAttributeValue("", 116, "OTContent_step", 116, 14, true);
#line 3 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
WriteAttributeValue("", 130, step, 130, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(138, 3, true);
            WriteLiteral(">\r\n");
            EndContext();
            BeginContext(172, 989, true);
            WriteLiteral(@"    <div class=""input-container flex-dir-col px-1"">
        <div class=""inputset"">
            <div class=""unit-text  fs-12"">
                <label>รหัสพนักงาน</label>
            </div>
            <div class=""form-check"">
                <div>
                    <input class=""form-control h-fix-30p my-1"" id=""txtEmpCode"" type=""text"" autocomplete=""off"" />
                    <div class=""z-1 autocomplete""></div>
                </div>
                <button class=""btn btn-light d-flex flex-wrap justify-center border-rad mx-2 my-1 add-worker"" type=""button"">
                    <span class=""fs-16 material-icon-symbols-outlined "">person_add</span>
                </button>
            </div>
        </div>
    </div>
    <div class=""worker-list py-2 ovf-auto-y"">
        <div class=""mx-0"">
            <div id=""divCountTitle""><span id=""spanPersonCount""></span></div>
            <div class=""d-flex flex-dir-row flex-wrap"" id=""WorkerContent"">
                
");
            EndContext();
#line 26 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
                 if (Model != null) { foreach (var EmpDetail in Model)
                    {
                    

#line default
#line hidden
            BeginContext(1277, 86, false);
#line 28 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
               Write(await Html.PartialAsync("~/Views/New/OTAddWorkerResult/_CateWorker.cshtml", EmpDetail));

#line default
#line hidden
            EndContext();
#line 28 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
                                                                                                           
                    }
                }

#line default
#line hidden
            BeginContext(1407, 60, true);
            WriteLiteral("            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n<div");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1467, "\"", 1485, 2);
            WriteAttributeValue("", 1472, "footer", 1472, 6, true);
#line 35 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
WriteAttributeValue("", 1478, step, 1478, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1486, 28, true);
            WriteLiteral(">\r\n    <button type=\"button\"");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 1514, "\"", 1565, 5);
            WriteAttributeValue("", 1522, "btn", 1522, 3, true);
            WriteAttributeValue(" ", 1525, "btn-secondary", 1526, 14, true);
            WriteAttributeValue(" ", 1539, "w-fix-100p", 1540, 11, true);
            WriteAttributeValue(" ", 1550, "previos", 1551, 8, true);
#line 36 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
WriteAttributeValue("", 1558, step, 1558, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1566, 111, true);
            WriteLiteral("><span class=\"material-icon-symbols-outlined\">arrow_back_ios</span>ย้อนกลับ</button>\r\n    <button type=\"button\"");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 1677, "\"", 1721, 5);
            WriteAttributeValue("", 1685, "btn", 1685, 3, true);
            WriteAttributeValue(" ", 1688, "btn-light", 1689, 10, true);
            WriteAttributeValue(" ", 1698, "w-fix-100p", 1699, 11, true);
            WriteAttributeValue(" ", 1709, "next", 1710, 5, true);
#line 37 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTAddWorker.cshtml"
WriteAttributeValue("", 1714, step, 1714, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1722, 27, true);
            WriteLiteral(">ถัดไป</button>\r\n</div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<CostEstimate.Models.New.multiModelCateWorker>> Html { get; private set; }
    }
}
#pragma warning restore 1591
