#pragma checksum "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Shared\_ptvViewOTDetail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dd4ef14b23c8d7ac0191f8de50f379e369b1c790"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ptvViewOTDetail), @"mvc.1.0.view", @"/Views/Shared/_ptvViewOTDetail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/_ptvViewOTDetail.cshtml", typeof(AspNetCore.Views_Shared__ptvViewOTDetail))]
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
#line 1 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\_ViewImports.cshtml"
using OTApproval;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dd4ef14b23c8d7ac0191f8de50f379e369b1c790", @"/Views/Shared/_ptvViewOTDetail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d14babe89c167d0913b6cbdafc36e203a7111ce5", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__ptvViewOTDetail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<OTApproval.Models.New.multiEditModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(46, 69, false);
#line 2 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Shared\_ptvViewOTDetail.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/OTViewDetail/_OTType.cshtml"));

#line default
#line hidden
            EndContext();
            BeginContext(115, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(118, 96, false);
#line 3 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Shared\_ptvViewOTDetail.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/OTViewDetail/_OTMyData.cshtml", Model.ViewMastRequestOT));

#line default
#line hidden
            EndContext();
            BeginContext(214, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(217, 93, false);
#line 4 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Shared\_ptvViewOTDetail.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/OTViewDetail/_OTForm.cshtml", Model.multiModelOTForm));

#line default
#line hidden
            EndContext();
            BeginContext(310, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(313, 102, false);
#line 5 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Shared\_ptvViewOTDetail.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/OTViewDetail/_OTAddWorker.cshtml", Model.multiModelCateWorker));

#line default
#line hidden
            EndContext();
            BeginContext(415, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(418, 98, false);
#line 6 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Shared\_ptvViewOTDetail.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/OTViewDetail/_OTEmailForm.cshtml", Model.multiOTEmailForm));

#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<OTApproval.Models.New.multiEditModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
