#pragma checksum "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "46eb8440a9f97bc2717a8542899b1ca108d1871a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_MyRequest__DisplayDone), @"mvc.1.0.view", @"/Views/MyRequest/_DisplayDone.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/MyRequest/_DisplayDone.cshtml", typeof(AspNetCore.Views_MyRequest__DisplayDone))]
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
#line 2 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
using OTApproval.Models.Common;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"46eb8440a9f97bc2717a8542899b1ca108d1871a", @"/Views/MyRequest/_DisplayDone.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d14babe89c167d0913b6cbdafc36e203a7111ce5", @"/Views/_ViewImports.cshtml")]
    public class Views_MyRequest__DisplayDone : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<OTApproval.Models.MyRequest.MultiDocMast>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(82, 159, true);
            WriteLiteral("<div class=\"just-group\">\r\n    <div class=\"table-result wx-100 fs-16 bg-trans\">\r\n        <div class=\"container wx-100 d-flex flex-dir-col\">\r\n            <div>\r\n");
            EndContext();
#line 7 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                 if (Model.docList.Count > 0)
                {
                    foreach (var item in Model.docList)
                    {

#line default
#line hidden
            BeginContext(387, 213, true);
            WriteLiteral("                        <div class=\"docno\">\r\n                            <div class=\"px-3 py-3\">\r\n                                <div class=\"d-flex dropend\">\r\n                                    <span>เลขอ้างอิง ");
            EndContext();
            BeginContext(601, 22, false);
#line 14 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                Write(item.requestOT.mrNoReq);

#line default
#line hidden
            EndContext();
            BeginContext(623, 466, true);
            WriteLiteral(@"</span>
                                    <button type=""button"" data-bs-toggle=""dropdown"" class=""bd-0 bg-trans text-blue"" aria-expanded=""false"" style=""display: block;""><span class=""material-icon-symbols-outlined"">more_vert</span></button>
                                    <ul class=""w-fix-100p manage dropdown-menu"">
                                        <li class=""my-1 manage border-rad bd-0""><button class=""bd-0 bg-trans wx-100 text-left text-gray view""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1089, "\"", 1120, 1);
#line 17 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
WriteAttributeValue("", 1097, item.requestOT.mrNoReq, 1097, 23, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1121, 423, true);
            WriteLiteral(@" data-bs-toggle=""modal"" data-bs-target=""#modalViewDetail""><span class=""pe-1 text-blue material-icon-symbols-outlined"">info</span>รายละเอียด</button></li>
                                    </ul>
                                </div>


                                <div class=""card mb-3"">
                                    <div class=""card-body"" role=""button"" data-bs-toggle=""collapse"" data-bs-target=""#collapse");
            EndContext();
            BeginContext(1546, 22, false);
#line 23 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                                                                        Write(item.requestOT.mrNoReq);

#line default
#line hidden
            EndContext();
            BeginContext(1569, 23, true);
            WriteLiteral("\" aria-expanded=\"false\"");
            EndContext();
            BeginWriteAttribute("aria-controls", " aria-controls=\"", 1592, "\"", 1643, 2);
            WriteAttributeValue("", 1608, "collapseSub_", 1608, 12, true);
#line 23 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
WriteAttributeValue("", 1620, item.requestOT.mrNoReq, 1620, 23, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1644, 158, true);
            WriteLiteral(">\r\n                                        <div class=\"steps d-flex flex-wrap flex-sm-nowrap justify-content-between padding-top-2x py-3 padding-bottom-1x\">\r\n");
            EndContext();
#line 25 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                             foreach (var step in Model.mastFlow.Where(w => w.mfFlowNo == item.requestOT.mrFlow))
                                            {
                                                string[] capture = step.mfSubject.Split("_");
                                                string rejected = item.stepHistory.Where(w => w.htStep == step.mfStep.Value).FirstOrDefault() is null ? "" : item.stepHistory.Where(w => w.htStep == step.mfStep.Value).FirstOrDefault().htStatus;
                                                string hisFromName = item.stepHistory.Where(w => w.htStep == step.mfStep).FirstOrDefault() is null ? "" : item.stepHistory.Where(w => w.htStep == step.mfStep).FirstOrDefault().htFrom;

#line default
#line hidden
            BeginContext(2552, 52, true);
            WriteLiteral("                                                <div");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 2604, "\"", 2752, 2);
            WriteAttributeValue("", 2612, "step", 2612, 4, true);
#line 30 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
WriteAttributeValue(" ", 2616, item.requestOT.mrStep.Value >= step.mfStep.Value ? "completed" : rejected.StartsWith(GlobalVariable.StatusRejected) ? "rejected": "", 2617, 135, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2753, 176, true);
            WriteLiteral(">\r\n                                                    <div class=\"step-icon-wrap fs-12\">\r\n                                                        <div class=\"step-icon\"><span>");
            EndContext();
            BeginContext(2930, 11, false);
#line 32 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                                Write(step.mfStep);

#line default
#line hidden
            EndContext();
            BeginContext(2941, 208, true);
            WriteLiteral("</span></div>\r\n                                                    </div>\r\n                                                    <h4 class=\"step-title\">\r\n                                                        ");
            EndContext();
            BeginContext(3150, 10, false);
#line 35 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                   Write(capture[0]);

#line default
#line hidden
            EndContext();
            BeginContext(3160, 77, true);
            WriteLiteral("\r\n                                                        <div class=\"fs-10\">");
            EndContext();
            BeginContext(3239, 166, false);
#line 36 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                       Write((item.requestOT.mrStep.Value + 1) == step.mfStep.Value ? "โดย คุณ" + item.requestOT.mrNameApp : hisFromName != "" ? hisFromName : capture.Length > 1 ? capture[1] : "");

#line default
#line hidden
            EndContext();
            BeginContext(3406, 123, true);
            WriteLiteral("</div>\r\n                                                    </h4>\r\n                                                </div>\r\n");
            EndContext();
#line 39 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                            }

#line default
#line hidden
            BeginContext(3576, 245, true);
            WriteLiteral("                                        </div>\r\n                                    </div>\r\n\r\n                                    <div class=\"category\">\r\n                                        <div class=\"border-rad bg-white px-3 py-3 collapse\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 3821, "\"", 3859, 2);
            WriteAttributeValue("", 3826, "collapse", 3826, 8, true);
#line 44 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
WriteAttributeValue("", 3834, item.requestOT.mrNoReq, 3834, 25, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3860, 97, true);
            WriteLiteral(">\r\n                                            <div class=\"d-flex flex-dir-row flex-wrap pt-1\">\r\n");
            EndContext();
#line 46 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                 foreach (var worker in item.workerList.Where(w => w.drNoReq == item.requestOT.mrNoReq))
                                                {
                                                    string status = worker.drStatus is null ? "" : worker.drStatus;
                                                    if (!(status.StartsWith(GlobalVariable.StatusRejected)))
                                                    {

#line default
#line hidden
            BeginContext(4428, 221, true);
            WriteLiteral("                                                        <button class=\"btn btn-light d-flex flex-dir-row border-rad py-2 w-fix-260p\">\r\n                                                            <div class=\"worker-img\">\r\n");
            EndContext();
#line 53 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                 if (item.workerImages.Where(w => w.empcode == worker.drEmpCode) != null)
                                                                {
                                                                    

#line default
#line hidden
#line 55 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                     foreach (var image in item.workerImages.Where(w => w.empcode == worker.drEmpCode))
                                                                    {

#line default
#line hidden
            BeginContext(5079, 119, true);
            WriteLiteral("                                                                        <div class=\"img\"><img class=\"wx-100 border-rad\"");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 5198, "\"", 5216, 1);
#line 57 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
WriteAttributeValue("", 5204, image.image, 5204, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5217, 11, true);
            WriteLiteral(" /></div>\r\n");
            EndContext();
#line 58 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                    }

#line default
#line hidden
#line 58 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                     
                                                                }
                                                                else
                                                                {

#line default
#line hidden
            BeginContext(5503, 150, true);
            WriteLiteral("                                                                    <div class=\"img\"><span class=\"material-icon-symbols-outlined\">image</span></div>\r\n");
            EndContext();
#line 63 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                }

#line default
#line hidden
            BeginContext(5720, 308, true);
            WriteLiteral(@"                                                            </div>
                                                            <div class=""d-flex flex-dir-col fs-12 text-left h-fix-75p w-fix-130p hidden-txt worker-details"">
                                                                <div class=""px-2"">");
            EndContext();
            BeginContext(6029, 16, false);
#line 66 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                             Write(worker.drEmpCode);

#line default
#line hidden
            EndContext();
            BeginContext(6045, 90, true);
            WriteLiteral("</div>\r\n                                                                <div class=\"px-2\">");
            EndContext();
            BeginContext(6136, 16, false);
#line 67 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                             Write(worker.drPriName);

#line default
#line hidden
            EndContext();
            BeginContext(6152, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(6154, 13, false);
#line 67 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                                               Write(worker.drName);

#line default
#line hidden
            EndContext();
            BeginContext(6167, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(6169, 17, false);
#line 67 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                                                              Write(worker.drLastName);

#line default
#line hidden
            EndContext();
            BeginContext(6186, 90, true);
            WriteLiteral("</div>\r\n                                                                <div class=\"px-2\">");
            EndContext();
            BeginContext(6277, 16, false);
#line 68 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                             Write(worker.drJobCode);

#line default
#line hidden
            EndContext();
            BeginContext(6293, 143, true);
            WriteLiteral("</div>\r\n                                                            </div>\r\n                                                        </button>\r\n");
            EndContext();
#line 71 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                    }
                                                    else
                                                    {

#line default
#line hidden
            BeginContext(6604, 254, true);
            WriteLiteral("                                                        <button class=\"btn btn-light d-flex flex-dir-row border-rad bd-c-red text-gray py-2 w-fix-260p\">\r\n                                                            <div class=\"opacity-dot-7 worker-img\">\r\n");
            EndContext();
#line 76 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                 if (item.workerImages.Where(w => w.empcode == worker.drEmpCode) != null)
                                                                {
                                                                    

#line default
#line hidden
#line 78 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                     foreach (var image in item.workerImages.Where(w => w.empcode == worker.drEmpCode))
                                                                    {

#line default
#line hidden
            BeginContext(7288, 119, true);
            WriteLiteral("                                                                        <div class=\"img\"><img class=\"wx-100 border-rad\"");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 7407, "\"", 7425, 1);
#line 80 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
WriteAttributeValue("", 7413, image.image, 7413, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(7426, 11, true);
            WriteLiteral(" /></div>\r\n");
            EndContext();
#line 81 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                    }

#line default
#line hidden
#line 81 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                     
                                                                }
                                                                else
                                                                {

#line default
#line hidden
            BeginContext(7712, 150, true);
            WriteLiteral("                                                                    <div class=\"img\"><span class=\"material-icon-symbols-outlined\">image</span></div>\r\n");
            EndContext();
#line 86 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                }

#line default
#line hidden
            BeginContext(7929, 308, true);
            WriteLiteral(@"                                                            </div>
                                                            <div class=""d-flex flex-dir-col fs-12 text-left h-fix-75p hidden-txt w-fix-130p worker-details"">
                                                                <div class=""px-2"">");
            EndContext();
            BeginContext(8238, 16, false);
#line 89 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                             Write(worker.drEmpCode);

#line default
#line hidden
            EndContext();
            BeginContext(8254, 90, true);
            WriteLiteral("</div>\r\n                                                                <div class=\"px-2\">");
            EndContext();
            BeginContext(8345, 16, false);
#line 90 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                             Write(worker.drPriName);

#line default
#line hidden
            EndContext();
            BeginContext(8361, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(8363, 13, false);
#line 90 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                                               Write(worker.drName);

#line default
#line hidden
            EndContext();
            BeginContext(8376, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(8378, 17, false);
#line 90 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                                                              Write(worker.drLastName);

#line default
#line hidden
            EndContext();
            BeginContext(8395, 90, true);
            WriteLiteral("</div>\r\n                                                                <div class=\"px-2\">");
            EndContext();
            BeginContext(8486, 16, false);
#line 91 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                                             Write(worker.drJobCode);

#line default
#line hidden
            EndContext();
            BeginContext(8502, 145, true);
            WriteLiteral("</div>\r\n                                                            </div>\r\n\r\n                                                        </button>\r\n");
            EndContext();
#line 95 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                                                    }
                                                }

#line default
#line hidden
            BeginContext(8753, 252, true);
            WriteLiteral("                                            </div>\r\n                                        </div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n");
            EndContext();
#line 103 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                    }
                }
                else
                {

#line default
#line hidden
            BeginContext(9088, 114, true);
            WriteLiteral("                    <h6>สามารถติดตามเอกสารการขอทำงานล่วงเวลาได้ที่เมนู \"คำร้องของฉัน\" -> \"ระหว่างดำเนินการ\"</h6>\r\n");
            EndContext();
#line 108 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\MyRequest\_DisplayDone.cshtml"
                }

#line default
#line hidden
            BeginContext(9221, 56, true);
            WriteLiteral("            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<OTApproval.Models.MyRequest.MultiDocMast> Html { get; private set; }
    }
}
#pragma warning restore 1591
