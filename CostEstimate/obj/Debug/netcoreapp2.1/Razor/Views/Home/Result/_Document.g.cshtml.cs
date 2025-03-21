#pragma checksum "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "229b0a945de150f44e179747298592e84a6f98ba"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Result__Document), @"mvc.1.0.view", @"/Views/Home/Result/_Document.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Result/_Document.cshtml", typeof(AspNetCore.Views_Home_Result__Document))]
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
#line 3 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
using OTApproval.Models.Common;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"229b0a945de150f44e179747298592e84a6f98ba", @"/Views/Home/Result/_Document.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d14babe89c167d0913b6cbdafc36e203a7111ce5", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Result__Document : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<OTApproval.Models.Approval.MultiNewLate>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(83, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 5 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
 if (Model.docList != null)
{

#line default
#line hidden
            BeginContext(117, 44, true);
            WriteLiteral("    <div class=\"just-group\" id=\"Document\">\r\n");
            EndContext();
#line 8 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
         if (Model.docList.Count() > 0)
        {

#line default
#line hidden
            BeginContext(213, 580, true);
            WriteLiteral(@"            <div class=""table-result"">
                <table id=""table"" class=""table table-scroll table-bordered table-hover"">
                    <thead>
                        <tr>
                            <th>แผนก</th>
                            <th>ผู้ร้องขอ</th>
                            <th>Prodution Line</th>
                            <th>Model</th>
                            <th>จำนวน (คน)</th>
                            <th>จำนวน (ชั่วโมง)</th>
                            <th></th>
                        </tr>
                    </thead>
");
            EndContext();
#line 23 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                     foreach (string department in Model.docList.Select(s => s.requestOT.mrDeptReq).Distinct().OrderBy(o => o))
                    {
                        int countWorker = 0;
                        int sumOTMinute = 0;
                        string sumHour = "";

#line default
#line hidden
            BeginContext(1083, 109, true);
            WriteLiteral("                        <tbody class=\"lamp\" role=\"button\" data-bs-toggle=\"collapse\" data-bs-target=\"#collapse");
            EndContext();
            BeginContext(1194, 26, false);
#line 28 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                                                                                         Write(department.Substring(0, 4));

#line default
#line hidden
            EndContext();
            BeginContext(1221, 23, true);
            WriteLiteral("\" aria-expanded=\"false\"");
            EndContext();
            BeginWriteAttribute("aria-controls", " aria-controls=\"", 1244, "\"", 1301, 2);
            WriteAttributeValue("", 1260, "collapseSub_", 1260, 12, true);
#line 28 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
WriteAttributeValue("", 1272, department.Substring(0, 4), 1272, 29, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1302, 116, true);
            WriteLiteral(">\r\n                            <tr class=\"lamp\">\r\n                                <td colspan=\"4\" class=\"text-left\">");
            EndContext();
            BeginContext(1419, 10, false);
#line 30 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                                             Write(department);

#line default
#line hidden
            EndContext();
            BeginContext(1429, 7, true);
            WriteLiteral("</td>\r\n");
            EndContext();
#line 31 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                 foreach (var doc in Model.docList.Where(w => w.requestOT.mrDeptReq == department).ToList())
                                {
                                    int minuteST = (int.Parse(doc.requestOT.mrOTTimeSt.Split(":")[0]) * 60) + int.Parse(doc.requestOT.mrOTTimeSt.Split(":")[1]);
                                    int minuteED = (int.Parse(doc.requestOT.mrOTTimeEd.Split(":")[0]) * 60) + int.Parse(doc.requestOT.mrOTTimeEd.Split(":")[1]);
                                    countWorker = countWorker + doc.workerList.Where(w => w.drStatus.StartsWith(GlobalVariable.StatusFinishedST)).Count();
                                    sumOTMinute = ((minuteED - minuteST) * countWorker);
                                    string minuteString = sumOTMinute % 60 == 0 ? "" : (sumOTMinute % 60).ToString() + " นาที";
                                    sumHour = (sumOTMinute / 60).ToString() + " ชั่วโมง " + minuteString;
                                }

#line default
#line hidden
            BeginContext(2438, 53, true);
            WriteLiteral("                                <td class=\"lamp-sum\">");
            EndContext();
            BeginContext(2492, 11, false);
#line 40 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                                Write(countWorker);

#line default
#line hidden
            EndContext();
            BeginContext(2503, 60, true);
            WriteLiteral("</td>\r\n                                <td class=\"lamp-sum\">");
            EndContext();
            BeginContext(2564, 7, false);
#line 41 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                                Write(sumHour);

#line default
#line hidden
            EndContext();
            BeginContext(2571, 519, true);
            WriteLiteral(@"</td>
                                <td>
                                    <button type=""button"" class=""lamp-more"" role=""button"" data-bs-toggle=""collapse"" data-bs-target=""#collapseLE1"" aria-expanded=""false"" aria-controls=""collapseSub_le1"">
                                        <span class=""material-icon-symbols-outlined fs-16 cs-pt"">expand_more</span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
");
            EndContext();
            BeginContext(3092, 47, true);
            WriteLiteral("                        <tbody class=\"collapse\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 3139, "\"", 3181, 2);
            WriteAttributeValue("", 3144, "collapse", 3144, 8, true);
#line 50 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
WriteAttributeValue("", 3152, department.Substring(0, 4), 3152, 29, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3182, 3, true);
            WriteLiteral(">\r\n");
            EndContext();
#line 51 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                              int count = 1;

#line default
#line hidden
            BeginContext(3232, 28, true);
            WriteLiteral("                            ");
            EndContext();
#line 52 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                             foreach (string group in Model.docList.Where(w => w.requestOT.mrDeptReq == department).Select(s => s.requestOT.mrGrpReq).Distinct())
                            {

#line default
#line hidden
            BeginContext(3426, 121, true);
            WriteLiteral("                                <tr class=\"line\">\r\n                                    <td colspan=\"8\" class=\"text-left\">");
            EndContext();
            BeginContext(3548, 5, false);
#line 55 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                                                 Write(group);

#line default
#line hidden
            EndContext();
            BeginContext(3553, 46, true);
            WriteLiteral("</td>\r\n                                </tr>\r\n");
            EndContext();
#line 57 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                 foreach (var issueby in (Model.docList.Where(w => w.requestOT.mrDeptReq == department && w.requestOT.mrGrpReq == group).Select(s => new { s.requestOT.mrNameReq, s.requestOT.mrPositionReq, s.requestOT.mrEmpReq, s.requestOT.mrLastNameReq }).GroupBy(g => new { g.mrEmpReq, g.mrNameReq, g.mrPositionReq, g.mrLastNameReq }).Select(s => s.FirstOrDefault())))
                                {

#line default
#line hidden
            BeginContext(4021, 194, true);
            WriteLiteral("                                    <tr class=\"issue-by text-left\">\r\n                                        <td></td>\r\n                                        <td colspan=\"7\" class=\"text-left\">");
            EndContext();
            BeginContext(4217, 108, false);
#line 61 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                                                      Write(issueby.mrPositionReq + "(" + issueby.mrEmpReq + " " + issueby.mrNameReq + " " + issueby.mrLastNameReq + ")");

#line default
#line hidden
            EndContext();
            BeginContext(4326, 50, true);
            WriteLiteral("</td>\r\n                                    </tr>\r\n");
            EndContext();
#line 63 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"

                                    foreach (var doc in Model.docList.Where(w => w.requestOT.mrDeptReq == department && w.requestOT.mrGrpReq == group && w.requestOT.mrPositionReq == issueby.mrPositionReq && w.requestOT.mrEmpReq == issueby.mrEmpReq))
                                    {

#line default
#line hidden
            BeginContext(4668, 176, true);
            WriteLiteral("                                        <tr class=\"le1-detail-1 cs-pt\">\r\n                                            <td></td>\r\n                                            <td>");
            EndContext();
            BeginContext(4846, 7, false);
#line 68 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                            Write(count++);

#line default
#line hidden
            EndContext();
            BeginContext(4854, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(4910, 30, false);
#line 69 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                           Write(doc.requestOT.mrProductionLine);

#line default
#line hidden
            EndContext();
            BeginContext(4940, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(4996, 21, false);
#line 70 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                           Write(doc.requestOT.mrModel);

#line default
#line hidden
            EndContext();
            BeginContext(5017, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(5073, 89, false);
#line 71 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                           Write(doc.workerList.Where(w => w.drStatus.StartsWith(GlobalVariable.StatusFinishedST)).Count());

#line default
#line hidden
            EndContext();
            BeginContext(5162, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(5218, 22, false);
#line 72 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                           Write(doc.workerList.Count());

#line default
#line hidden
            EndContext();
            BeginContext(5240, 110, true);
            WriteLiteral(" </td>\r\n                                            <td></td>\r\n                                        </tr>\r\n");
            EndContext();
#line 75 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                    }
                                }

#line default
#line hidden
#line 76 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                                 

                            }

#line default
#line hidden
            BeginContext(5457, 34, true);
            WriteLiteral("                        </tbody>\r\n");
            EndContext();
#line 80 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
                    }

#line default
#line hidden
            BeginContext(5514, 46, true);
            WriteLiteral("                </table>\r\n            </div>\r\n");
            EndContext();
#line 83 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
        }

#line default
#line hidden
            BeginContext(5571, 12, true);
            WriteLiteral("    </div>\r\n");
            EndContext();
#line 85 "C:\Users\T0014210\Desktop\OTApproval\OTApproval\OTApproval\Views\Home\Result\_Document.cshtml"
}





#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<OTApproval.Models.Approval.MultiNewLate> Html { get; private set; }
    }
}
#pragma warning restore 1591
