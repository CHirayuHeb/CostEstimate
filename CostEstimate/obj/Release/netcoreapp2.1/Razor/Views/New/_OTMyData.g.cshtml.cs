#pragma checksum "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "000458ef65f3fcf0dd4b02aeabe1e965f29f0a83"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_New__OTMyData), @"mvc.1.0.view", @"/Views/New/_OTMyData.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/New/_OTMyData.cshtml", typeof(AspNetCore.Views_New__OTMyData))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"000458ef65f3fcf0dd4b02aeabe1e965f29f0a83", @"/Views/New/_OTMyData.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"03cd043d7b3157285994f12141f5133e7c84d925", @"/Views/_ViewImports.cshtml")]
    public class Views_New__OTMyData : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CostEstimate.Models.Table.LAMP.ViewMastRequestOT>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("OTType"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
  string step = "2";

#line default
#line hidden
            BeginContext(80, 24, true);
            WriteLiteral("\r\n<div class=\"container\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 104, "\"", 130, 2);
            WriteAttributeValue("", 109, "OTContent_step", 109, 14, true);
#line 4 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
WriteAttributeValue("", 123, step, 123, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(131, 3, true);
            WriteLiteral(">\r\n");
            EndContext();
#line 5 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
     if (Model != null)
    {

#line default
#line hidden
            BeginContext(166, 43, true);
            WriteLiteral("        <div class=\"dp-none\">\r\n            ");
            EndContext();
            BeginContext(209, 54, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c6be612bfbb047a0a85fe8ea72d5a72a", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
#line 8 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrOTType);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(263, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(277, 41, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3dad1328ea264596b44376fbe2c01921", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 9 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrNoReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(318, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(332, 46, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "2e0f5ffc29cf4758a80c57c1a9d5d8d6", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 10 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrPriNameReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(378, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(392, 42, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "26cb8d38437b4916b76f8e281c166831", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 11 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrEmpReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(434, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(448, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "d02f17c8f0ff48c5ba7143c5b1155447", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 12 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrNameReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(491, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(505, 47, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "a8a80a7c4d55479d814d9bcecc1e308c", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 13 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrLastNameReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(552, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(566, 47, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "e46ea1fb952547b6bf21d4d549ccb2c6", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 14 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrPositionReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(613, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(627, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "68e5d9b8de1c4586b8da2617a95408de", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 15 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrDeptReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(670, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(684, 42, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "bde5595d800c4e4d95eb5aff358477b3", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 16 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrSecReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(726, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(740, 42, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "5ccb24ec681d4c6bac44919a2dbd19ee", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 17 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrGrpReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(782, 14, true);
            WriteLiteral("\r\n            ");
            EndContext();
            BeginContext(796, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "98bb09295ad544d0a8c91ab4ba0d2bc9", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 18 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.mrUnitReq);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(839, 284, true);
            WriteLiteral(@"
        </div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>วันที่ขอ OT</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(1124, 15, false);
#line 26 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrDateReq);

#line default
#line hidden
            EndContext();
            BeginContext(1139, 333, true);
            WriteLiteral(@"</label>
                </div>
            </div>
        </div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>ลำดับที่</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(1473, 13, false);
#line 36 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrNoReq);

#line default
#line hidden
            EndContext();
            BeginContext(1486, 373, true);
            WriteLiteral(@"</label>
                </div>
            </div>
        </div>
        <div class=""clear-b""></div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>รหัสพนักงาน</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(1860, 14, false);
#line 47 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrEmpReq);

#line default
#line hidden
            EndContext();
            BeginContext(1874, 3, true);
            WriteLiteral(" : ");
            EndContext();
            BeginContext(1878, 15, false);
#line 47 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                                        Write(Model.mrNameReq);

#line default
#line hidden
            EndContext();
            BeginContext(1893, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(1895, 19, false);
#line 47 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                                                         Write(Model.mrLastNameReq);

#line default
#line hidden
            EndContext();
            BeginContext(1914, 332, true);
            WriteLiteral(@"</label>
                </div>
            </div>
        </div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>ตำแหน่ง</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(2247, 19, false);
#line 57 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrPositionReq);

#line default
#line hidden
            EndContext();
            BeginContext(2266, 329, true);
            WriteLiteral(@"</label>
                </div>
            </div>
        </div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>ฝ่าย</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(2596, 15, false);
#line 67 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrDeptReq);

#line default
#line hidden
            EndContext();
            BeginContext(2611, 329, true);
            WriteLiteral(@"</label>
                </div>
            </div>
        </div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>แผนก</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(2941, 14, false);
#line 77 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrSecReq);

#line default
#line hidden
            EndContext();
            BeginContext(2955, 330, true);
            WriteLiteral(@"</label>
                </div>
            </div>
        </div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>กลุ่ม</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(3286, 14, false);
#line 87 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrGrpReq);

#line default
#line hidden
            EndContext();
            BeginContext(3300, 330, true);
            WriteLiteral(@"</label>
                </div>
            </div>
        </div>
        <div class=""input-container fl"">
            <div class=""inputset"">
                <div class=""unit-text fs-16"">
                    <label>หน่วย</label>
                </div>
                <div class=""form-check"">
                    <label>");
            EndContext();
            BeginContext(3631, 15, false);
#line 97 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
                      Write(Model.mrUnitReq);

#line default
#line hidden
            EndContext();
            BeginContext(3646, 107, true);
            WriteLiteral("</label>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <div class=\"clear-b\"></div>\r\n");
            EndContext();
#line 102 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"

    }

#line default
#line hidden
            BeginContext(3762, 12, true);
            WriteLiteral("</div>\r\n<div");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 3774, "\"", 3792, 2);
            WriteAttributeValue("", 3779, "footer", 3779, 6, true);
#line 105 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
WriteAttributeValue("", 3785, step, 3785, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3793, 28, true);
            WriteLiteral(">\r\n    <button type=\"button\"");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 3821, "\"", 3872, 5);
            WriteAttributeValue("", 3829, "btn", 3829, 3, true);
            WriteAttributeValue(" ", 3832, "btn-secondary", 3833, 14, true);
            WriteAttributeValue(" ", 3846, "w-fix-100p", 3847, 11, true);
            WriteAttributeValue(" ", 3857, "previos", 3858, 8, true);
#line 106 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
WriteAttributeValue("", 3865, step, 3865, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3873, 111, true);
            WriteLiteral("><span class=\"material-icon-symbols-outlined\">arrow_back_ios</span>ย้อนกลับ</button>\r\n    <button type=\"button\"");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 3984, "\"", 4028, 5);
            WriteAttributeValue("", 3992, "btn", 3992, 3, true);
            WriteAttributeValue(" ", 3995, "btn-light", 3996, 10, true);
            WriteAttributeValue(" ", 4005, "w-fix-100p", 4006, 11, true);
            WriteAttributeValue(" ", 4016, "next", 4017, 5, true);
#line 107 "C:\Users\t0015142\OneDrive - STANLEY ELECTRIC CO., LTD\Desktop\Work\MVC Project\CostEstimate\CostEstimate\Views\New\_OTMyData.cshtml"
WriteAttributeValue("", 4021, step, 4021, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(4029, 98, true);
            WriteLiteral(">ถัดไป<span class=\"material-icon-symbols-outlined\">arrow_forward_ios</span></button>\r\n</div>\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CostEstimate.Models.Table.LAMP.ViewMastRequestOT> Html { get; private set; }
    }
}
#pragma warning restore 1591
