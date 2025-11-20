using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.MyRequest;
using CostEstimate.Models.New;
using CostEstimate.Models.Table.HRMS;
using CostEstimate.Models.Table.IT;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MOLD;
using CostEstimate.Models.Table.MK;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Http;
using System.IO;

using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Mail;

namespace CostEstimate.Controllers.NewMoldModify
{
    public class NewMoldModifyController : Controller
    {
        //username emppic ftpdb
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public string path = @"\\thsweb\\CostEstimate\\";
        public string PgName = "CostEstimate";
        public NewMoldModifyController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
        {
            _LAMP = lamp;
            _HRMS = hrms;
            _IT = it;
            _MK = mk;
            _MOLD = mold;
            _Cache = cacheController;
            _callFunc = callfunction;
        }

        [Authorize("Checked")]
        public IActionResult Index(Class @class, string id, string mfRevision)
        {

            try
            {
                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                @class._ViewOperaterCP = new ViewOperaterCP();
                @class._ViewceMastFlowApprove = new ViewceMastFlowApprove();


                List<string> _listTypeofCavity = _MK._ViewceMastType.Where(x => x.mtType.Contains("Cavity") && x.mtProgram.Contains("MoldModify")).OrderBy(x=>x.mtName).Select(x => x.mtName).ToList();

                //List<string> _listTypeofCavity = new List<string>{
                //                            "CAVITIES(R/L =1 Set) x 2MOLD",
                //                            "CAVITIES(R/L =1 Set)",
                //                            "CAVITIES",
                //                            "CAVITIES x 2 MOLD",
                //                            "CAVITY x 2 MOLD",
                //                            "CAVITY"};
                SelectList _TypeofCavity = new SelectList(_listTypeofCavity);
                ViewBag.TypeofCavity = _TypeofCavity;

                List<string> _listTypeofMODIFICATION = _MK._ViewceMastType.Where(x => x.mtType.Contains("Rate") && x.mtProgram.Contains("MoldModify")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
                //List<string> _listTypeofMODIFICATION = new List<string>{
                //                            "CLAIM SUB MAKER",
                //                            "MOLD MODIFICATION",
                //                            "SUB MAKER (CASE MODIFY)"};
                SelectList _TypeofMODIFICATION = new SelectList(_listTypeofMODIFICATION);
                ViewBag.TypeofMODIFICATION = _TypeofMODIFICATION;


                List<string> _listRequestBy = _MK._ViewceMastType.Where(x => x.mtType.Contains("RequestBy") && x.mtProgram.Contains("MoldModify")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
                //List<string> _listRequestBy = new List<string>{
                //                            "CUSTOMER", "DC", "LAMP","MOLD","NMD","PED"};
                SelectList _TypeofRequestBy = new SelectList(_listRequestBy);
                ViewBag.TypeofRequestBy = _TypeofRequestBy;
                @class._ViewOperaterCP = new ViewOperaterCP();

                @class._ViewceMastModifyRequest = new ViewceMastModifyRequest();


                //@class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "2").ToList();
                @class._listAttachment = new List<ViewAttachment>();
                @class._ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                @class._ListGroupViewceDetailSubMakerRequest = new List<GroupViewceDetailSubMakerRequest>();
                @class._ListViewceItemModifyRequest = new List<ViewceItemModifyRequest>();
                //@class._ListViewceItemModifyRequest = _MK._ViewceItemModifyRequest.ToList();

                @class._ListceMastSubMakerRequest = new List<ViewceMastSubMakerRequest>();


                //ViewBag.ModifyRequest =  _MK._ViewceItemModifyRequest.ToList();
                //id = "CE-M-25-06-001";
                if (id != null)
                {

                    @class._ViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == id).FirstOrDefault();
                    if (@class._ViewceMastModifyRequest == null)
                    {
                        return RedirectToAction("Index", "ErrorPage");
                        // return RedirectToAction("Index", "ErrorPageMold");
                        // return View(@class);
                    }
                    //@class._ViewceMastModifyRequest.mfIssueRate = _MK._ViewceHourChangeCategory.Where(x => x.hcIssue == @class._ViewceMastModifyRequest.mfIssueRate).OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcIssue)
                    //                                                .FirstOrDefault() is null ?
                    //                                                  _MK._ViewceHourChangeCategory.OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcYear + ":" + x.hcRev).FirstOrDefault()
                    //                                                : _MK._ViewceHourChangeCategory.Where(x => x.hcIssue == @class._ViewceMastModifyRequest.mfIssueRate).OrderByDescending(x => x.hcYear).Select(x => x.hcYear + ":" + x.hcRev).FirstOrDefault();
                    //string DocYear = _MK._ViewceHourChangeCategory.Where(x => x.hcIssue == @class._ViewceMastModifyRequest.mfIssueRate)
                    //                                                .OrderByDescending(x => x.hcYear).Select(x => x.hcYear.Substring(2, 2))
                    //                                                .FirstOrDefault() is null ?
                    //                                                                     _MK._ViewceHourChangeCategory.OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcYear.Substring(2, 2)).FirstOrDefault()
                    //                                                                    : _MK._ViewceHourChangeCategory.Where(x => x.hcIssue == @class._ViewceMastModifyRequest.mfIssueRate).OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcYear.Substring(2, 2)).FirstOrDefault();
                    //@class._ListMlistMonth = FunListMonth(DocYear).Select(m => new MlistMonth { Month = m }).ToList();

                    @class._ViewceMastModifyRequest.mfIssueRate = !string.IsNullOrEmpty(@class._ViewceMastModifyRequest.mfIssueRate) ? @class._ViewceMastModifyRequest.mfIssueRate :
                                                                 _MK._ViewceHourChangeCategory.OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcYear + "#" + x.hcRev).FirstOrDefault();


                    string DocYear = @class._ViewceMastModifyRequest.mfIssueRate.Split(':')[0].Substring(2, 2); //@class._ViewceMastModifyRequest.mfIssueRate;
                    @class._ListMlistMonth = FunListMonth(DocYear).Select(m => new MlistMonth { Month = m }).ToList();




                    @class._ListceDetailSubMakerRequest = _MK._ViewceDetailSubMakerRequest.Where(x => x.dsDocumentNo == id).ToList();
                    @class._ListViewceItemModifyRequest = _MK._ViewceItemModifyRequest.Where(x => x.imCENo == id).ToList();
                    if (mfRevision != null)
                    {


                        @class._ViewceMastModifyRequest.mfCENo = "";
                        @class._ViewceMastModifyRequest.mfRevision = String.Format("{0:D2}", int.Parse(@class._ViewceMastModifyRequest.mfRevision) + 1);
                        @class._ViewceMastModifyRequest.mfEmpCodeRequest = "";
                        @class._ViewceMastModifyRequest.mfNameRequest = "";
                        @class._ViewceMastModifyRequest.mfEmpCodeApprove = "";
                        @class._ViewceMastModifyRequest.mfNameApprove = "";
                        @class._ViewceMastModifyRequest.mfStep = 0;
                        @class._ViewceMastModifyRequest.mfStatus = "";

                        for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                        {
                            @class._ListceDetailSubMakerRequest[i].dsDocumentNo = "";
                            @class._ListceDetailSubMakerRequest[i].dsRevision = String.Format("{0:D2}", int.Parse(@class._ViewceMastModifyRequest.mfRevision) + 1);
                        }

                        for (int i = 0; i < @class._ListViewceItemModifyRequest.Count(); i++)
                        {
                            @class._ListViewceItemModifyRequest[i].imCENo = "";
                            // @class._ListceDetailSubMakerRequest[i].dsRevision = String.Format("{0:D2}", int.Parse(@class._ViewceMastSubMakerRequest.smRevision) + 1);
                        }
                    }
                    else
                    {
                        @class._listAttachment = _IT.Attachment.Where(x => x.fnNo == id).ToList();
                    }




                    //@class._ListViewceItemModifyRequest = _MK._ViewceItemModifyRequest.Where(x => x.imCENo == id).ToList();
                    @class._ListGroupViewceDetailSubMakerRequest = @class._ListceDetailSubMakerRequest.GroupBy(p => p.dsGroupName).Select(g => new GroupViewceDetailSubMakerRequest
                    {
                        GroupName = g.Key.Trim(),
                        DetailSubMakerRequest = g.ToList()
                    }).ToList();
                }
                else
                {
                    //DocYear = DocYear == null ? DateTime.Now.ToString("yyyy") : DocYear;
                    @class._ViewceMastModifyRequest.mfIssueRate = _MK._ViewceHourChangeCategory.OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcYear + "#" + x.hcRev).FirstOrDefault();
                    string DocYear = _MK._ViewceHourChangeCategory.OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcYear.Substring(2, 2)).FirstOrDefault() is null ? DateTime.Now.ToString("yyyy") : _MK._ViewceHourChangeCategory.OrderByDescending(x => x.hcYear).ThenByDescending(x => x.hcRev).Select(x => x.hcYear.Substring(2, 2)).FirstOrDefault();
                    @class._ListMlistMonth = FunListMonth(DocYear).Select(m => new MlistMonth { Month = m }).ToList();




                }


            }
            catch (Exception ex)
            {
                ViewBag.test = ex.Message;
            }

            return View(@class);
        }

        public List<string> FunListMonth(string docYear1)
        {
            string docYear2 = (int.Parse(docYear1) + 1).ToString();
            List<string> _listMonth = new List<string>
                                    {
                                        "04/" + docYear1,
                                        "05/" + docYear1,
                                        "06/" + docYear1,
                                        "07/" + docYear1,
                                        "08/" + docYear1,
                                        "09/" + docYear1,
                                        "10/" + docYear1,
                                        "11/" + docYear1,
                                        "12/" + docYear1,
                                        "01/" + docYear2,
                                        "02/" + docYear2,
                                        "03/" + docYear2
                                    };

            // set to ViewBag if needed
            //ViewBag.listofMonth = new SelectList(_listMonth);

            return _listMonth;
        }



        [HttpPost]
        public PartialViewResult PrintMoldQUOTATION(string mpNo, Class @class)
        {
            try
            {


                if (mpNo != null)
                {
                    @class._ViewOperaterCP = new ViewOperaterCP();

                    string tbHistoryIssueName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryIssueEMPCODE = tbHistoryIssueName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryIssueName)).Select(x => x.emEmpcode).FirstOrDefault();

                    string tbHistoryCheckedGLName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedGLEMPCODE = tbHistoryCheckedGLName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedGLName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryCheckedName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedEMPCODE = tbHistoryCheckedName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryApproveByName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryApproveByEMPCODE = tbHistoryApproveByName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryApproveByName)).Select(x => x.emEmpcode).FirstOrDefault();



                    //string tbHistoryIssue3 = _IT.rpEmails.Where(u => u.emName_M365.Contains(_MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault())).Select(x => x.emEmpcode).FirstOrDefault();
                    //string tbHistoryIssue = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    //    _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();

                    //string tbHistoryChecked = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    //  _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();

                    //string tbHistoryApproveBy = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    // _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();


                    //ViewAccEMPLOYEE _ViewAccEMPLOYEEIssue = new ViewAccEMPLOYEE();
                    //ViewAccEMPLOYEE _ViewAccEMPLOYEEChecked = new ViewAccEMPLOYEE();
                    //ViewAccEMPLOYEE _ViewAccEMPLOYEEApproveBy = new ViewAccEMPLOYEE();

                    //_ViewAccEMPLOYEEIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssue).FirstOrDefault();
                    //_ViewAccEMPLOYEEChecked = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryChecked).FirstOrDefault();
                    //_ViewAccEMPLOYEEApproveBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveBy).FirstOrDefault();


                    //string tbFlowissue = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 2).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 2).Select(z => z.mfTo).FirstOrDefault();
                    //string tbFlowCheck1 = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 3).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 3).Select(z => z.mfTo).FirstOrDefault();
                    //string tbFlowCheck2 = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 4).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 4).Select(z => z.mfTo).FirstOrDefault();
                    //string tbFlowApprove = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 5).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 5).Select(z => z.mfTo).FirstOrDefault();


                    _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();

                    string vIssueBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssueEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssueEMPCODE).Select(x => x.NICKNAME).FirstOrDefault();
                    string vCheckByTL = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();
                    string vCheckByTM = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();
                    string vApproveBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();

                    @class._ViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == mpNo).FirstOrDefault();

                    @class._ViewOperaterCP.IssueBy = vIssueBy;
                    @class._ViewOperaterCP.CheckedByTL = vCheckByTL;
                    @class._ViewOperaterCP.CheckedByTM = vCheckByTM;
                    @class._ViewOperaterCP.ApproveBy = vApproveBy;

                    @class._ViewOperaterCP.empIssueBy = tbHistoryIssueEMPCODE;
                    @class._ViewOperaterCP.empCheckedByTL = tbHistoryCheckedGLEMPCODE;
                    @class._ViewOperaterCP.empCheckedByTM = tbHistoryCheckedEMPCODE;
                    @class._ViewOperaterCP.empApproveBy = tbHistoryApproveByEMPCODE;




                }


            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialDisplayMoldQuotation", @class);
            //return PartialView("_PartialDisplayQuotationTHSarabun", @class);

        }


        public ActionResult SearchMold_Ledger_Number(string term)
        {
            //Class @class = new Class();
            //@class._listViewLLLedger = _MOLD._ViewLLLedger.Where(p => p.LGLegNo.Contains(term)).ToList();

            return Json(
_MOLD._ViewLLLedger
    .Where(p => p.LGLegNo.Contains(term))
    .Select(p =>
        p.LGLegNo + "" +
        p.LGTypeCode + "|" +
        p.LGCustomer + "|" +
        p.LGMoldNo + "/" + p.LGMoldName + "|" +
        //p.LGMoldName + "/" + p.LGMoldNo + "|" +
        "0" + "|" +
        (p.LGEstimateDM ?? 0).ToString("0.##") + "|" +
        (p.LGCostResult ?? 0).ToString("0.##") + "|" +
        (p.LGMkPrice ?? 0).ToString("0.##") + "|" +
        (p.LGIcsName ?? "") + "|" +
        (p.LGPart ?? "-")
    )
    .ToList()
);
            //return Json(_MOLD._ViewLLLedger.Where(p => p.LGLegNo.Contains(term)).Select(p => p.LGLegNo + "" + p.LGTypeCode + "|" + p.LGCustomer + "|" + p.LGMoldName + "/" + p.LGMoldNo + "|" + "0"+"|"+p.LGEstimateDM +"|" + p.LGCostResult + "|" + p.LGMkPrice).ToList());
            //return Json(_MOLD._ViewLLLedger.Where(p => p.LGLegNo.Contains(term)).Select(p => p.LGLegNo + "" + p.LGTypeCode + "|" + p.LGCustomer + "|" + p.LGMoldName + "/" + p.LGMoldNo + "|" + "0").ToList());
            // return Json(_MOLD._ViewmtMaster_Mold_Control.Where(p => p.mcLedger_Number.Contains(term)).Select(p => p.mcLedger_Number + "|" + p.mcCUS + "|" + p.mcMoldname + "|" + (p.mcCavity != "" ? p.mcCavity : "0")).ToList());

        }

        public ActionResult SearchModelName(string term)
        {
            {
                // string a = _MK._ViewceMastModel.Where(p => p.mmModelName.Contains(term)).Where(x => x.mmType == "MoldModify").Select(p => p.mmModelName).First();

                //string a = _MK._ViewceMastCostModel.Where(p => p.mcModelName.Contains(term)).Select(p => p.mcModelName).First();
                //return Json(_MK._ViewceMastCostModel.Where(p => p.mcModelName.Contains(term)).Select(p => p.mcModelName).Distinct().ToList());
                return Json(_MK._ViewceMastModel.Where(p => p.mmModelName.Contains(term)).Where(x => x.mmType == "MoldModify").Select(p => p.mmModelName).Distinct().ToList());
            }
        }
        public PartialViewResult SearchbyModelName(string search, string issueRate, Class @class)
        {

            string msg = "";
            string mfCostType = "";
            int mfissueRate = issueRate != null ? int.Parse(issueRate.Split('#')[1]) : 0;
            try
            {
                var typeOrder = new List<string> { "RESULTS", "FORECAST", "PLAN" };

                mfCostType = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.Contains("Indirect") && typeOrder.Contains(x.heType))
                                                    .OrderBy(x => typeOrder.IndexOf(x.heType))
                                                    .Select(x => x.heType).ToList().Count() == 0 ? "" : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.Contains("Indirect") && typeOrder.Contains(x.heType))
                                                    .OrderBy(x => typeOrder.IndexOf(x.heType))
                                                    .Select(x => x.heType).First();




                // LABOUR = DESIGN =  Indirect +  Direct-DG ยกเว้น 3-D 
                double vLABOURIndirect = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.Contains("Indirect") && typeOrder.Contains(x.heType))
                                                    .OrderBy(x => typeOrder.IndexOf(x.heType))
                                                    .Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.Contains("Indirect") && typeOrder.Contains(x.heType))
                                                    .OrderBy(x => typeOrder.IndexOf(x.heType))
                                                    .Select(x => x.heAmount).First();
                double vLABOURDirectDG = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && (x.heProcessName.ToLower().Contains("direct") && x.heProcessName.Contains("DG")) && typeOrder.Contains(x.heType))
                                               .OrderBy(x => typeOrder.IndexOf(x.heType))
                                               .Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && (x.heProcessName.ToLower().Contains("direct") && x.heProcessName.Contains("DG")) && typeOrder.Contains(x.heType))
                                               .OrderBy(x => typeOrder.IndexOf(x.heType))
                                               .Select(x => x.heAmount).First();
                double vLABOURDirectPG = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && (x.heProcessName.ToLower().Contains("Direct") && x.heProcessName.Contains("PG")) && typeOrder.Contains(x.heType))
                                          .OrderBy(x => typeOrder.IndexOf(x.heType))
                                          .Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && (x.heProcessName.ToLower().Contains("Direct") && x.heProcessName.Contains("PG")) && typeOrder.Contains(x.heType))
                                                                                       .OrderBy(x => typeOrder.IndexOf(x.heType)).Select(x => x.heAmount).First();

                //Result  Forcast plan
                // ! LABOUR = DESIGN =  Indirect +   Direct-PG
                double vDESIGN = vLABOURIndirect + vLABOURDirectDG;
                double vNotDESIGN = vLABOURIndirect + vLABOURDirectPG;

                //DEPRECIATION (DP)
                //GM.
                double vDGM = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains("GM") && typeOrder.Contains(x.heType))
                                      .OrderBy(x => typeOrder.IndexOf(x.heType))
                                      .Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains("GM") && typeOrder.Contains(x.heType))
                                                                                   .OrderBy(x => typeOrder.IndexOf(x.heType)).Select(x => x.heAmount).First();


                double vDPOther = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains("Other") && typeOrder.Contains(x.heType))
                                       .OrderBy(x => typeOrder.IndexOf(x.heType))
                                       .Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains("Other") && typeOrder.Contains(x.heType))
                                                                                    .OrderBy(x => typeOrder.IndexOf(x.heType)).Select(x => x.heAmount).First();

                //MANUFACTURING  EXPENSES(ME)
                double vME = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains("M.E") && typeOrder.Contains(x.heType))
                                     .OrderBy(x => typeOrder.IndexOf(x.heType))
                                     .Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains("M.E") && typeOrder.Contains(x.heType))
                                                                                  .OrderBy(x => typeOrder.IndexOf(x.heType)).Select(x => x.heAmount).First();





                List<ViewceMastProcess> _ListceMastProcess = new List<ViewceMastProcess>();
                //string v_CostPlanningNo = _MK._ViewceMastCostModel.Where(x => x.mcModelName == search).OrderByDescending(x => x.mcCostPlanningNo).Select(x => x.mcCostPlanningNo).First();

                //List<ViewceCostPlanning> _ViewceCostPlanning = new List<ViewceCostPlanning>();
                // var v_ListceCostPlanning = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == v_CostPlanningNo).ToList();


                _ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "MoldModify").ToList();
                @class._ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "MoldModify").ToList();

                List<ViewceDetailSubMakerRequest> _ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                for (int i = 0; i < _ListceMastProcess.Count(); i++)
                {
                    try
                    {
                        //case  WEDM ,NCG ,NCGR


                        double vdsDP_Rate = 0;
                        if (_ListceMastProcess[i].mpGroupName.ToUpper().Trim().Contains("HW"))
                        {
                            vdsDP_Rate = vDPOther;
                        }
                        else if (_ListceMastProcess[i].mpGroupName.ToUpper().Trim().Contains("GM"))
                        {
                            vdsDP_Rate = vDGM + vDPOther;
                        }
                        else if ((
                                 _MK._ViewceHourChangeEntry
                                     .Where(
                                         x => x.heAmount != 0
                                              && x.heMonth == search
                                              && x.heRev == mfissueRate
                                              && x.heGroupMain.ToLower().Contains("DEPRECIATION")
                                              && x.heProcessName.ToLower().Contains(_ListceMastProcess[i].mpProcessName)
                                              && typeOrder.Contains(x.heType)
                                     )
                                     .OrderBy(x => typeOrder.IndexOf(x.heType))  // จัดลำดับตาม order ที่กำหนด
                                     .Select(x => x.heAmount)
                                     .ToList()
                                     .Count()  // นับจำนวนรายการ
                             ) > 0)
                        //_MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heProcessName.ToLower().Contains(_ListceMastProcess[i].mpProcessName) && typeOrder.Contains(x.heType))
                        //                                                        .OrderBy(x => typeOrder.IndexOf(x.heType)).Select(x => x.heAmount).First() != 0)


                        {
                            //NC GR.
                            if (_ListceMastProcess[i].mpProcessName.Contains("NC GR."))
                            {
                                vdsDP_Rate = _MK._ViewceHourChangeEntry.Where(x => x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains(_ListceMastProcess[i].mpProcessName) && x.heGroupMain.ToLower().Contains("DEPRECIATION") && x.heType == mfCostType)
                                                                                   .Select(x => x.heAmount).First();

                            }
                            else
                            {
                                vdsDP_Rate = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower().Contains(_ListceMastProcess[i].mpProcessName) && x.heGroupMain.ToLower().Contains("DEPRECIATION") && typeOrder.Contains(x.heType))
                                                                                   .OrderBy(x => typeOrder.IndexOf(x.heType)).Select(x => x.heAmount).First();

                            }

                            vdsDP_Rate = vdsDP_Rate + vDPOther;
                        }
                        else
                        {
                            vdsDP_Rate = 1;
                            //vdsDP_Rate = vdsDP_Rate + vDPOther;
                            string input = _ListceMastProcess[i].mpProcessName.Contains("W-E") ? "WEDM" : _ListceMastProcess[i].mpProcessName;
                            string firstPart = input;
                            if (input.Contains("-"))
                            {
                                // แทนที่ "-" ด้วย " " แล้วแยก
                                string replaced = input.Replace("-", " ");
                                string[] parts = replaced.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                firstPart = parts.Length > 0 ? parts[0] : "";
                            }
                            else
                            {
                                // ถ้าไม่มี "-" ก็แยกจากช่องว่างปกติ
                                string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                firstPart = parts.Length > 0 ? parts[0] : "";
                            }
                            //string textToSplit = input.Contains("-") ? input.Replace("-", " ") : input;
                            //string firstPart = textToSplit.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";

                            //NCG ,WEDM
                            if (firstPart.Contains("NCG") || firstPart.Contains("WEDM"))
                            {
                                vdsDP_Rate = _MK._ViewceHourChangeEntry.Where(x => x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower() == firstPart && x.heGroupMain.Contains("DEPRECIATION") && x.heType == mfCostType).Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower() == firstPart && x.heGroupMain.Contains("DEPRECIATION") && x.heType == mfCostType).Select(x => x.heAmount).First();
                            }
                            else
                            {
                                vdsDP_Rate = _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower() == firstPart && x.heGroupMain.Contains("DEPRECIATION") && typeOrder.Contains(x.heType))
                                  .OrderBy(x => typeOrder.IndexOf(x.heType))
                                  .Select(x => x.heAmount).ToList().Count() == 0 ? 0 : _MK._ViewceHourChangeEntry.Where(x => x.heAmount != 0 && x.heMonth == search && x.heRev == mfissueRate && x.heProcessName.ToLower() == firstPart && x.heGroupMain.Contains("DEPRECIATION") && typeOrder.Contains(x.heType))
                                                                               .OrderBy(x => typeOrder.IndexOf(x.heType)).Select(x => x.heAmount).First();

                            }



                            vdsDP_Rate = vdsDP_Rate + vDPOther;

                        }

                        //vdsDP_Rate = vdsDP_Rate + vDPOther;



                        //check case   _ListceMastProcess[i].mpProcessName = master




                        //check case ตัดช่องว่าง


                        //if _ListceMastProcess[i].mpGroupName.Trim() = "DESIGN" && _ListceMastProcess[i].mpProcessName.Trim() !="3-D"
                        //vLABOURIndirect + vLABOURDirectPG
                        //else  vLABOURIndirect + vLABOURDirectDG
                        _ListceDetailSubMakerRequest.Add(new ViewceDetailSubMakerRequest
                        {
                            dsDocumentNo = "",
                            dsRunNo = 0,
                            dsLotNo = "",
                            dsOrderNo = "",
                            dsRevision = "",
                            dsGroupName = _ListceMastProcess[i].mpGroupName.Trim(),
                            dsProcessName = _ListceMastProcess[i].mpProcessName.Trim(),

                            dsWT_Man = 0,
                            dsWT_Auto = 0,

                            dsEnable_WTAuto = _ListceMastProcess[i].mpEnable_WTAuto,//_ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTAuto).First(),  //true,
                            dsEnable_WTMan = _ListceMastProcess[i].mpEnable_WTMan,//_ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTMan).First(),





                            dsLabour_Rate = _ListceMastProcess[i].mpGroupName.ToUpper().Trim().Contains("DESIGN") && !_ListceMastProcess[i].mpProcessName.Trim().Contains("3-D") ? vDESIGN : vNotDESIGN,//v_ListceCostPlanning[i].cpLabour_Rate,
                            dsLabour_Cost = 0,

                            //dsDP_Rate = vDPOther +

                            dsDP_Rate = vdsDP_Rate,
                            dsDP_Cost = 0,

                            dsME_Rate = vME,//v_ListceCostPlanning[i].cpME_Rate,
                            dsME_Cost = 0,

                            dsTotalCost = 0,
                        });
                        //dsDocumentNo = "";

                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }



                //@class._ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                @class._ListGroupViewceDetailSubMakerRequest = new List<GroupViewceDetailSubMakerRequest>();
                @class._ListGroupViewceDetailSubMakerRequest = _ListceDetailSubMakerRequest.GroupBy(p => p.dsGroupName).Select(g => new GroupViewceDetailSubMakerRequest
                {
                    GroupName = g.Key.Trim(),
                    DetailSubMakerRequest = g.ToList()
                }
                ).ToList();

                //@class._GroupViewceDetailSubMakerRequest = @class._ListceDetailSubMakerRequest.GroupBy(p => p.cpGroupName).ToList();



                // @class._ListceDetailSubMakerRequest = _listceDetailSubMakerRequest;


            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            //ViewBag.mfCostType = mfCostType;
            ViewBag.mfCostType = mfCostType;
            return PartialView("_PartialNewMoldRequestProcess", @class);
        }
        [HttpPost]
        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            int v_step = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfStep : 0;
            string v_issue = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfCENo : "";
            List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
            partialUrl = Url.Action("SendMail", "NewMoldModify", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });
            try
            {
                if (@classs._ViewceMastModifyRequest != null)
                {
                    if (@classs._ViewceMastModifyRequest.mfCENo != "" && @classs._ViewceMastModifyRequest.mfCENo != null)
                    {
                        // htCostPlanningNo
                        String htDocNo = @classs._ViewceMastModifyRequest.mfCENo.ToString(); //htCostPlanningNo
                                                                                             //_listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htStep).ThenBy(x=>x.htDate).ThenBy(x=>x.htTime).ToList();
                        _listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htDate).ThenBy(x => x.htTime).ThenBy(x => x.htStep).ToList();
                        if (_listHistory.Count() > 0)
                        {
                            for (int j = 0; j < _listHistory.Count(); j++)
                            {
                                var v_htcc = _listHistory[j].htCC;
                                string v_CCemail = "";
                                if (v_htcc != null)
                                {
                                    ViewrpEmail fromEmailCC = new ViewrpEmail();
                                    string[] splitCC = v_htcc.Split(',');
                                    foreach (var i in splitCC)
                                    {
                                        if (i != " " & i != "")
                                        {
                                            var v_cc = "";
                                            try
                                            {
                                                fromEmailCC = _IT.rpEmails.Where(w => w.emEmpcode == i.Trim()).FirstOrDefault();
                                                v_CCemail += fromEmailCC.emName_M365.ToString() + ",";
                                            }
                                            catch (Exception e)
                                            {
                                                v_cc = e.Message;
                                            }
                                        }
                                    }
                                }

                                _listHistory[j].htCC = v_CCemail;


                            }

                        }


                        return Json(new { status = "hasHistory", listHistory = _listHistory, partial = partialUrl });
                    }
                }

            }
            catch (Exception e)
            {

            }

            //return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
            return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
        }
        public ActionResult SendMail(Class @class, int s_step, string s_issue, string mpNo)
        {
            ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;

            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
            var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
            if (mpNo != null && mpNo != "")
            {
                @class._ViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == mpNo).FirstOrDefault();
                s_issue = @class._ViewceMastModifyRequest.mfEmpCodeRequest;
            }

            //get emp operator
            var v_empstep = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "2") != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "2").Select(x => x.mfTo).FirstOrDefault() : "";
            if (v_empstep != null) //step 2-5
            {
                var v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empstep).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                @class._ViewceHistoryApproved.htTo = v_emailTo;
            }

            //step 6 Waiting Apporve By DM For ST Department ==> issue
            if (s_step == 5)
            {
                //var vdocNo = @class.
                var v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == s_issue).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                @class._ViewceHistoryApproved.htTo = v_emailTo;


                //string tbHistory2 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 1).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 1).Select(x => x.htFrom).FirstOrDefault();
                //string tbHistory2EMPCODE = tbHistory2 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory2)).Select(x => x.emEmpcode).FirstOrDefault();

                string tbHistory3 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 2).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 2).Select(x => x.htFrom).FirstOrDefault();
                string tbHistory3EMPCODE = tbHistory3 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory3)).Select(x => x.emEmpcode).FirstOrDefault();

                string tbHistory2 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault();
                string tbHistory2EMPCODE = tbHistory2 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory2)).Select(x => x.emEmpcode).FirstOrDefault();

                string tbHistory5 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault();
                string tbHistory5EMPCODE = tbHistory5 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory5)).Select(x => x.emEmpcode).FirstOrDefault();


                @class._ViewceHistoryApproved.htCC = tbHistory2 + "," + tbHistory3 + "," + tbHistory5 + ",";
                //@class._ViewceHistoryApproved.htCC = tbHistory3 + "," + tbHistory5 + ",";

            }


            //step 4 fix ST dept check

            @class._ViewceHistoryApproved.htFrom = v_emailFrom;
            @class._ViewceHistoryApproved.htStatus = "Approve";
            ViewBag.step = s_step;
            return PartialView("SendMail", @class);
        }


        public string[] chkPermission(Class @class)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string message_per = "";
            string status_per = "";
            var chkData = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == @class._ViewceMastModifyRequest.mfCENo).FirstOrDefault();
            try
            {
                if (chkData != null)
                {
                    //check operator //check create user
                    if (chkData.mfStep == 0 && _UserId == chkData.mfEmpCodeRequest)
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else if (_UserId == chkData.mfEmpCodeApprove)
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else
                    {
                        status_per = "P";
                        message_per = "You don't have permission to access";
                    }
                }
                else
                {
                    status_per = "S";
                    message_per = "You have permission ";
                }

                string[] returnvar = { status_per, message_per };
                return returnvar;
            }
            catch (Exception ex)
            {
                string[] returnvar = { status_per, message_per };
                return returnvar;

            }
        }

        public string[] RunDocNo(Class @class)
        {

            string v_msg = "";
            string v_rundoc = "";
            int i_rundoc = 0;

            string vIssue = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string vDocCode = "CE";
            string vDocSub = "M";
            string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
            string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);

            try
            {
                //check update revision or new
                if (@class._ViewceMastModifyRequest.mfCENo != null && @class._ViewceMastModifyRequest.mfCENo != "")
                {

                    v_msg = "Update";
                    v_rundoc = @class._ViewceMastModifyRequest.mfCENo;
                    // v_rundoc = "CE" + "S" + "-" + vYY + "-" + vMM + String.Format("{0:D3}", i_rundoc);
                }
                else
                {
                    //CE-S-25-03-001 10,3
                    i_rundoc = _MK._ViewceRunDocument.Where(x => x.rmDocCode == vDocCode && x.rmDocSub == vDocSub && x.rmYear == vYY && x.rmMonth == vMM).OrderByDescending(x => x.rmRunNo).Select(x => x.rmRunNo).FirstOrDefault();
                    v_msg = "New";
                    // i_rundoc = i_rundoc > 0 ? i_rundoc + 1 : 0;
                    if (i_rundoc > 0)
                    {
                        v_rundoc = "CE" + "-" + "M" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", i_rundoc + 1);
                    }
                    else
                    {
                        v_rundoc = "CE" + "-" + "M" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", 1);
                    }

                    //v_rundoc = "CE" + "S" + "-" + vYY + "-" + vMM + String.Format("{0:D3}", i_rundoc);

                }

            }
            catch (Exception ex)
            {
                v_msg = "Fail";
                v_rundoc = ex.Message;
            }


            string[] vRevision = { v_msg, v_rundoc };
            return vRevision;
        }

        [HttpPost]
        public JsonResult SaveForm_SendMail(Class @class, List<IFormFile> files)
        {

            string config = "S";
            string msg = "Send Mail & Save File Already";

            string vStatus = "";
            string[] chkPermis;
            string[] chkSave;
            string[] chkSaveHistory;
            int i_Step = 0;

            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;





            try
            {
                //chk permission
                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }



                i_Step = @class._ViewceMastModifyRequest != null ? @class._ViewceMastModifyRequest.mfStep : 0;

                //check history
                if (@class._ViewceHistoryApproved.htTo != null || (@class._ViewceHistoryApproved.htTo == null && @class._ViewceHistoryApproved.htStatus == "Disapprove"))
                {
                    if (@class._ViewceHistoryApproved.htStatus == "Approve")
                    {
                        i_Step = i_Step + 1;
                        config = "S";

                        ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();
                        if (fromEmailTO == null)
                        {
                            config = "E";
                            msg = "Please Check your Email to , Email incorrect !!!";
                        }

                    }
                    else if (@class._ViewceHistoryApproved.htStatus == "Disapprove")
                    {
                        i_Step = 8;
                        config = "S";
                        string v_empissue = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == @class._ViewceMastModifyRequest.mfCENo).Select(x => x.mfEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                        @class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).First();
                    }
                    else
                    {
                        config = "E";
                        msg = "Please input Status";
                    }
                }
                else
                {
                    config = "E";
                    msg = "Please input e-mail.";
                }




                if (config == "S")
                {
                    //check dm cs , DM Step 0-3  , ST Step 4-7
                    if (i_Step < 3)
                    {
                        //check emp positon
                        try
                        {
                            string v_POS_HCM_CODE = "";
                            string v_empcsup = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).FirstOrDefault();

                            if (i_Step == 1) //GL up
                            {
                                v_POS_HCM_CODE = _HRMS.AccPOSMAST.Where(x => x.POS_CODE == "TL").Select(x => x.POS_HCM_CODE).FirstOrDefault();
                                msg = "Please send approval to GL Up of Dept.!!!";
                            }
                            else if (i_Step == 2) //DM up
                            {
                                v_POS_HCM_CODE = _HRMS.AccPOSMAST.Where(x => x.POS_CODE == "DDM").Select(x => x.POS_HCM_CODE).FirstOrDefault();
                                msg = "Please send approval to DM Up of Dept.!!!";
                            }

                            ViewAccEMPLOYEE _ViewAccEMPLOYEE = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == v_empcsup).FirstOrDefault();
                            List<ViewAccPOSMAST> _ViewAccPOSMAST = _HRMS.AccPOSMAST.Where(x => int.Parse(x.POS_HCM_CODE) <= int.Parse(v_POS_HCM_CODE)).ToList();
                            string v_chk = _ViewAccPOSMAST.Where(x => x.POS_CODE == _ViewAccEMPLOYEE.POS_CODE).Select(x => x.POS_CODE).FirstOrDefault();

                            if (v_chk == null || v_chk == "")
                            {
                                config = "E";
                                // msg = msg;
                                return Json(new { c1 = config, c2 = msg });
                            }
                        }
                        catch (Exception ex)
                        {
                            config = "E";
                            msg = "Please check email send to !!!!";
                            return Json(new { c1 = config, c2 = msg });
                        }
                    }

                    //    //save 


                    //RunDoc()
                    //vRunDoc[0] = status  vRunDoc[1]= run doc number
                    // status New
                    // status Old
                    // status Update RunDocNo
                    vRunDoc = RunDocNo(@class);
                    //vRunDoc = RunDoc(@class);
                    //vRunDocNo = RunDocNo(@class);

                    if (vRunDoc[0] == "Fail")
                    {
                        config = "E";
                        msg = "Error Run Doc No : " + vRunDoc[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    //@class._ListViewceItemModifyRequest = JsonConvert.DeserializeObject<List<ViewceItemModifyRequest>>(_ceItemModifyRequest);

                    //sRunDoc = vRunDoc[1]; save , savehistory
                    chkSave = Save(@class, i_Step, vRunDoc[0], vRunDoc[1], files);
                    if (chkSave[0] == "E")
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                    }

                    //saveHistory
                    chkSaveHistory = saveHistory(@class, i_Step, vRunDoc[0], vRunDoc[1]);
                    if (chkSaveHistory[0] == "E")
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                    }




                    //vStatus = savefile(@class, files);

                    //return Json(new { c1 = config, c2 = msg, c3 = vRunDoc[1] });

                    string vlotNo = @class._ViewceMastModifyRequest.mfLotNo;
                    string vRefNo = @class._ViewceMastModifyRequest.mfRefNo;
                    string vRevno = @class._ViewceMastModifyRequest.mfRevision is null ? "00" : @class._ViewceMastModifyRequest.mfRevision;

                    return Json(new { c1 = config, c2 = msg, c3 = vRunDoc[1], c_lotNo = vlotNo, c_RefNo = vRefNo, c_RevNo = vRevno });

                }
                else
                {
                    config = "E";
                    msg = msg;
                    return Json(new { c1 = config, c2 = msg });
                }
            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
                return Json(new { c1 = config, c2 = msg });
            }





        }


        public string[] saveHistory(Class @class, int vstep, string status, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";
            string vCCemail = "";
            string vEmpCodeCCemail = "";
            string Empcode_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string vRevision = @class._ViewceMastModifyRequest.mfRevision != null && @class._ViewceMastModifyRequest.mfRevision != "" ? @class._ViewceMastModifyRequest.mfRevision : "00"; //String.Format("{0:D3}", RunDoc.Substring(11, 3));

            //string Empcode_IssueBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string Name_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "NameE")?.Value; // HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NICKNAME)?.Value;
            string v_EmpCodeRequest = @class._ViewceMastModifyRequest.mfEmpCodeRequest == null || @class._ViewceMastModifyRequest.mfEmpCodeRequest == ""
                                                                                ? Empcode_IssueBy + " : " + _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == Empcode_IssueBy).Select(x => x.NICKNAME).First()
                                                                                : @class._ViewceMastModifyRequest.mfEmpCodeRequest + " : " + @class._ViewceMastModifyRequest.mfNameRequest;




            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "2").Select(x => x.mfSubject).First();
                    vstep = vstep == 8 ? vstep = 0 : vstep;

                    if (@class._ViewceHistoryApproved.htCC != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                    vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }
                    }

                    //ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                    //_ViewceHistoryApproved.htDocNo = RunDoc;// getSrNo[0].ToString();
                    //_ViewceHistoryApproved.htStep = vstep;
                    //_ViewceHistoryApproved.htStatus = @class._ViewceHistoryApproved.htStatus;
                    //_ViewceHistoryApproved.htFrom = @class._ViewceHistoryApproved.htFrom;
                    //_ViewceHistoryApproved.htTo = @class._ViewceHistoryApproved.htTo;
                    //_ViewceHistoryApproved.htCC = vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                    //_ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                    //_ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                    //_ViewceHistoryApproved.htRemark = @class._ViewceHistoryApproved.htRemark;
                    //_MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);

                    //_MK.SaveChanges();



                    dbContextTransaction.Commit();

                    var email = new MimeMessage();
                    ViewrpEmail fromEmailFrom = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).FirstOrDefault();
                    ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();

                    MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                    MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);
                    email.Subject = "CostEstimate Mold Modify Request==> " + _smStatus; /*( " + _ViewlrBuiltDrawing.bdDocumentType + " ) " + _ViewlrHistoryApprove.htStatus*/;
                    //email.From.Add(MailboxAddress.Parse(_ViewlrHistoryApprove.htFrom));
                    email.From.Add(FromMailFrom);
                    email.To.Add(FromMailTO);

                    if (@class._ViewceHistoryApproved.htCC != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                    MailboxAddress FromMailcc = new MailboxAddress(fromEmailCC.emName_M365, fromEmailCC.emEmail_M365);
                                    email.Cc.Add(FromMailcc);
                                    vCCemail += fromEmailCC.emEmail_M365.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }
                    }
                    var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldModify";// + getSrNo[0].ToString();
                    var bodyBuilder = new BodyBuilder();
                    //var image = bodyBuilder.LinkedResources.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\wwwroot\images\btn\OK.png");
                    string vIssue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    string vIssueName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;
                    string EmailBody = $"<div>" +
                        $"<B>Cost Estimate : Mold Modify </B> <br>" +
                        $"<B>Document No : </B> " + RunDoc + "<br>" +  //v_EmpCodeRequest
                        $"<B>Reference No : </B> " + @class._ViewceMastModifyRequest.mfRefNo + "<br>" +
                        $"<B>Lot No : </B> " + @class._ViewceMastModifyRequest.mfLotNo + "<br>" +
                        $"<B>Revision No : </B> " + vRevision + "<br>" +
                        $"<B>Request By : </B> " + v_EmpCodeRequest + "<br>" +
                        // $"<B>Request By : </B> " + @class._ViewceMastSubMakerRequest.smEmpCodeRequest + " : " + @class._ViewceMastSubMakerRequest.smNameRequest + "<br>" +
                        //$"<B>Subject : </B>" + _smStatus + "<br>" +
                        $"<B>Status : </B> " + _smStatus + "<br> " +
                        $"<B> หมายเหตุ : </B> " + @class._ViewceHistoryApproved.htRemark + "<br> " +
                        $"คลิ๊กลิงค์เพื่อเปิดเอกสาร <a href='" + varifyUrl + "'>More Detail" +
                        //$"<img src = 'http://thsweb/MVCPublish/LR_Service_Request/images/btn/mail1.png' alt = 'HTML tutorial' style = 'width: 42px; height: 42px;'>" +
                        $"</a>" +
                        $"</div>";

                    // bodyBuilder.Attachments.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\dev_rfc.log");

                    bodyBuilder.HtmlBody = string.Format(EmailBody);
                    email.Body = bodyBuilder.ToMessageBody();

                    // send email
                    //var smtp1 = new SmtpClient();
                    ////smtp.Connect("mail.csloxinfo.com");
                    //smtp1.Connect("203.146.237.138");


                    ////smtp1.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    ////// connect แบบ SSL/TLS
                    ////smtp1.Connect("150.109.165.119", 465, SecureSocketOptions.SslOnConnect);

                    ////smtp1.Connect("150.109.165.119");
                    //smtp1.Send(email);
                    //smtp1.Disconnect(true);

                    var senderEmail = new MailAddress(fromEmailFrom.emEmail_M365, fromEmailFrom.emName_M365);
                    var receiverEmail = new MailAddress(fromEmailTO.emEmail_M365, fromEmailTO.emName_M365);


                    System.Net.Mime.ContentType mimeTypeS = new System.Net.Mime.ContentType("text/html");
                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(EmailBody, mimeTypeS);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.csloxinfo.com");
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    using (MailMessage mess = new MailMessage(senderEmail, receiverEmail))
                    {
                        mess.Subject = "CostEstimate Mold Modify Request==> " + _smStatus;
                        //add CC
                        if (@class._ViewceHistoryApproved.htCC != null)
                        {
                            ViewrpEmail fromEmailCC = new ViewrpEmail();
                            string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                            foreach (var i in splitCC)
                            {
                                if (i != " " & i != "")
                                {
                                    var v_cc = "";
                                    try
                                    {
                                        fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                        //MailboxAddress FromMailcc = new MailboxAddress(fromEmailCC.emName_M365, fromEmailCC.emEmail_M365);
                                        //email.Cc.Add(FromMailcc);
                                        //vCCemail += fromEmailCC.emEmail_M365.ToString() + ",";

                                        mess.CC.Add(fromEmailCC.emEmail_M365);
                                    }
                                    catch (Exception e)
                                    {
                                        v_cc = e.Message;
                                    }
                                }
                            }
                        }


                        mess.AlternateViews.Add(alternate);
                        smtp.Send(mess);
                    }



                    v_status = "S";
                    v_msg = "File saved and email sent.!!!";
                }
                catch (Exception ex)
                {
                   // dbContextTransaction.Rollback();
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    v_status = "E";
                    v_msg = "Error Save History: " + ex.Message;
                }
            }


            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }


        public string[] Save(Class @class, int vstep, string status, string RunDoc, List<IFormFile> files)
        {
            string empissue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string UpdateBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            //User.Claims.FirstOrDefault(s => s.Type == "NICKNAME")?.Value;
            string v_msg = "";
            string v_status = "";



            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string vDocCode = "CE";
                    string vDocSub = "M";
                    string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
                    string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);
                    string vRevision = @class._ViewceMastSubMakerRequest.smRevision != null && @class._ViewceMastSubMakerRequest.smRevision != "" ? @class._ViewceMastSubMakerRequest.smRevision : "00"; //String.Format("{0:D3}", RunDoc.Substring(11, 3));

                    string empIssue = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).Select(x => x.emEmpcode).First() :
                                        @class._ViewceMastSubMakerRequest.smEmpCodeRequest == null ? empissue : @class._ViewceMastSubMakerRequest.smEmpCodeRequest;
                    string NickNameIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.NICKNAME).First();
                    string DeptIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.DEPT_CODE).First();

                    string empApprove = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() : @class._ViewceMastSubMakerRequest.smEmpCodeApprove;
                    string NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastSubMakerRequest.smNameApprove;


                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "2").Select(x => x.mfSubject).First();

                    //case dis approve
                    vstep = vstep == 8 ? vstep = 0 : vstep;






                    // status New  // status Update
                    if (status == "New")
                    {
                        //save run doc //CE-S-25-03-001 10,3


                        ViewceRunDocument _ViewceRunDocument = new ViewceRunDocument();
                        _ViewceRunDocument.rmRunNo = int.Parse(RunDoc.Substring(11, 3));
                        _ViewceRunDocument.rmDocCode = vDocCode;
                        _ViewceRunDocument.rmDocSub = vDocSub;
                        _ViewceRunDocument.rmYear = vYY;
                        _ViewceRunDocument.rmMonth = vMM;
                        _ViewceRunDocument.rmIssueBy = IssueBy;
                        _ViewceRunDocument.rmIssueBy = UpdateBy;
                        _MK._ViewceRunDocument.AddAsync(_ViewceRunDocument);




                        ////add table MastModifyRequest
                        //var itemMaterail = _MK._ViewceItemModifyRequest.Where(p => p.imCENo == RunDoc).ToList();
                        //_MK._ViewceItemModifyRequest.RemoveRange(itemMaterail);
                        //_MK.SaveChanges();
                        //for (int i = 0; i < @class._ListViewceItemModifyRequest.Count(); i++)
                        //{
                        //    var _ViewceItemModifyRequest = new ViewceItemModifyRequest()
                        //    {
                        //        imCENo = RunDoc,
                        //        imItemNo = (i + 1),
                        //        imItemName = @class._ListViewceItemModifyRequest[i].imItemName,
                        //        imPCS = @class._ListViewceItemModifyRequest[i].imPCS,
                        //        imAmount = @class._ListViewceItemModifyRequest[i].imAmount,
                        //    };
                        //    _MK._ViewceItemModifyRequest.AddAsync(_ViewceItemModifyRequest);
                        //}



                        //table ceDetailSubMakerRequest

                        if (@class._ListceDetailSubMakerRequest.Count() > 0)
                        {
                            for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                            {
                                var _ViewceDetailSubMakerRequest = new ViewceDetailSubMakerRequest()
                                {
                                    dsDocumentNo = RunDoc,
                                    dsRunNo = i + 1,
                                    dsLotNo = @class._ViewceMastModifyRequest.mfLotNo,// @class._ViewceMastSubMakerRequest.smLotNo,
                                    dsOrderNo = @class._ViewceMastModifyRequest.mfRefNo,// @class._ViewceMastSubMakerRequest.smOrderNo, //wait 
                                    dsRevision = vRevision,
                                    dsGroupName = @class._ListceDetailSubMakerRequest[i].dsGroupName,
                                    dsProcessName = @class._ListceDetailSubMakerRequest[i].dsProcessName,
                                    dsWT_Man = @class._ListceDetailSubMakerRequest[i].dsWT_Man,
                                    dsWT_Auto = @class._ListceDetailSubMakerRequest[i].dsWT_Auto,
                                    dsEnable_WTAuto = @class._ListceDetailSubMakerRequest[i].dsEnable_WTAuto,
                                    dsEnable_WTMan = @class._ListceDetailSubMakerRequest[i].dsEnable_WTMan,
                                    dsLabour_Rate = @class._ListceDetailSubMakerRequest[i].dsLabour_Rate,
                                    dsLabour_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate / 1000, 2),//   @class._ListceDetailSubMakerRequest[i].dsLabour_Cost,
                                    dsDP_Rate = @class._ListceDetailSubMakerRequest[i].dsDP_Rate,
                                    dsDP_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsDP_Cost,
                                                                                                                                                                         //_ViewceDetailSubMakerRequest.dsDP_Cost = @class._ListceDetailSubMakerRequest[i].dsDP_Cost;
                                    dsME_Rate = @class._ListceDetailSubMakerRequest[i].dsME_Rate,
                                    dsME_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsME_Cost,
                                };
                                //s_dsWT_Man += _ViewceDetailSubMakerRequest.dsWT_Man;
                                //s_dsWT_Auto += _ViewceDetailSubMakerRequest.dsWT_Auto;
                                _MK._ViewceDetailSubMakerRequest.AddAsync(_ViewceDetailSubMakerRequest);
                            }

                        }

                        ViewceMastModifyRequest _ViewceMastModifyRequest = new ViewceMastModifyRequest();
                        _ViewceMastModifyRequest.mfCENo = RunDoc;
                        _ViewceMastModifyRequest.mfRevision = vRevision;//@class._ViewceMastModifyRequest.mfRevision;
                        _ViewceMastModifyRequest.mfRefNo = @class._ViewceMastModifyRequest.mfRefNo;
                        _ViewceMastModifyRequest.mfLotNo = @class._ViewceMastModifyRequest.mfLotNo;
                        _ViewceMastModifyRequest.mfIssueRate = @class._ViewceMastModifyRequest.mfIssueRate;
                        _ViewceMastModifyRequest.mfCostRate = @class._ViewceMastModifyRequest.mfCostRate;
                        _ViewceMastModifyRequest.mfCostType = @class._ViewceMastModifyRequest.mfCostType;
                        _ViewceMastModifyRequest.mfType = @class._ViewceMastModifyRequest.mfType;
                        _ViewceMastModifyRequest.mfESTCost = @class._ViewceMastModifyRequest.mfESTCost;
                        _ViewceMastModifyRequest.mfResultCost = @class._ViewceMastModifyRequest.mfResultCost;
                        _ViewceMastModifyRequest.mfMKPrice = @class._ViewceMastModifyRequest.mfMKPrice;
                        _ViewceMastModifyRequest.mfCustomerName = @class._ViewceMastModifyRequest.mfCustomerName;
                        _ViewceMastModifyRequest.mfMoldNoOrMoldName = @class._ViewceMastModifyRequest.mfMoldNoOrMoldName;
                        _ViewceMastModifyRequest.mfFunction = @class._ViewceMastModifyRequest.mfFunction;
                        _ViewceMastModifyRequest.mfModelName = @class._ViewceMastModifyRequest.mfModelName;
                        _ViewceMastModifyRequest.mfMoldMass = @class._ViewceMastModifyRequest.mfMoldMass;
                        _ViewceMastModifyRequest.mfCavityNo = @class._ViewceMastModifyRequest.mfCavityNo;
                        _ViewceMastModifyRequest.mfTypeCavity = @class._ViewceMastModifyRequest.mfTypeCavity;
                        _ViewceMastModifyRequest.mfLeadTime = @class._ViewceMastModifyRequest.mfLeadTime;
                        _ViewceMastModifyRequest.mfLabourCost = @class._ViewceMastModifyRequest.mfLabourCost;
                        _ViewceMastModifyRequest.mfDPCost = @class._ViewceMastModifyRequest.mfDPCost;
                        _ViewceMastModifyRequest.mfMECost = @class._ViewceMastModifyRequest.mfMECost;
                        _ViewceMastModifyRequest.mfCostUntilSH = @class._ViewceMastModifyRequest.mfCostUntilSH;
                        _ViewceMastModifyRequest.mfMatCost = @class._ViewceMastModifyRequest.mfMatCost;
                        _ViewceMastModifyRequest.mfRateCostUntilSH = @class._ViewceMastModifyRequest.mfRateCostUntilSH;
                        _ViewceMastModifyRequest.mfTotalESTCost = @class._ViewceMastModifyRequest.mfTotalESTCost;
                        _ViewceMastModifyRequest.mfRoundupotalESTCost = @class._ViewceMastModifyRequest.mfRoundupotalESTCost;
                        _ViewceMastModifyRequest.mfDetail = @class._ViewceMastModifyRequest.mfDetail;
                        _ViewceMastModifyRequest.mfProcess = @class._ViewceMastModifyRequest.mfProcess;
                        _ViewceMastModifyRequest.mfIssueDate = @class._ViewceMastModifyRequest.mfIssueDate;
                        _ViewceMastModifyRequest.mfIssueDept = @class._ViewceMastModifyRequest.mfIssueDept;
                        _ViewceMastModifyRequest.mfEmpCodeRequest = @class._ViewceMastModifyRequest.mfEmpCodeRequest != null && @class._ViewceMastModifyRequest.mfEmpCodeRequest != "" ? @class._ViewceMastModifyRequest.mfEmpCodeRequest : empIssue; // public string  // @class._ViewceMastModifyRequest.mfEmpCodeRequest;
                        _ViewceMastModifyRequest.mfNameRequest = @class._ViewceMastModifyRequest.mfNameRequest != null && @class._ViewceMastModifyRequest.mfNameRequest != "" ? @class._ViewceMastModifyRequest.mfNameRequest : NickNameIssue; ; //@class._ViewceMastModifyRequest.mfNameRequest;
                        _ViewceMastModifyRequest.mfEmpCodeApprove = empApprove; //@class._ViewceMastModifyRequest.mfEmpCodeApprove;
                        _ViewceMastModifyRequest.mfNameApprove = NickNameApprove;//@class._ViewceMastModifyRequest.mfNameApprove;
                        _ViewceMastModifyRequest.mfFlowNo = 2;// @class._ViewceMastModifyRequest.mfFlowNo;
                        _ViewceMastModifyRequest.mfStep = vstep;//@class._ViewceMastModifyRequest.mfStep;
                        _ViewceMastModifyRequest.mfStatus = _smStatus;//@class._ViewceMastModifyRequest.mfStatus;
                        _ViewceMastModifyRequest.mfMtTool = @class._ViewceMastModifyRequest.mfMtTool;
                        _ViewceMastModifyRequest.mfTotalMt = @class._ViewceMastModifyRequest.mfTotalMt;

                        _ViewceMastModifyRequest.mfIcsname = @class._ViewceMastModifyRequest.mfIcsname; //30/06/2025

                        _MK._ViewceMastModifyRequest.AddAsync(_ViewceMastModifyRequest);
                        _MK.SaveChanges();
                    }

                    // status Old
                    else if (status == "Update")
                    {
                        //add table MastModifyRequest
                        //var itemMaterail = _MK._ViewceItemModifyRequest.Where(p => p.imCENo == RunDoc).ToList();
                        //_MK._ViewceItemModifyRequest.RemoveRange(itemMaterail);
                        //_MK.SaveChanges();
                        //for (int i = 0; i < @class._ListViewceItemModifyRequest.Count(); i++)
                        //{
                        //    var _ViewceItemModifyRequest = new ViewceItemModifyRequest()
                        //    {
                        //        imCENo = RunDoc,
                        //        imItemNo = (i + 1),
                        //        imItemName = @class._ListViewceItemModifyRequest[i].imItemName,
                        //        imPCS = @class._ListViewceItemModifyRequest[i].imPCS,
                        //        imAmount = @class._ListViewceItemModifyRequest[i].imAmount,
                        //    };
                        //    _MK._ViewceItemModifyRequest.AddAsync(_ViewceItemModifyRequest);
                        //}


                        if (@class._ListceDetailSubMakerRequest.Count() > 0)
                        {
                            var products = _MK._ViewceDetailSubMakerRequest.Where(p => p.dsDocumentNo == RunDoc).ToList();
                            _MK._ViewceDetailSubMakerRequest.RemoveRange(products);
                            //_MK.SaveChangesAsync();
                            _MK.SaveChanges();
                            for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                            {
                                var _ViewceDetailSubMakerRequest = new ViewceDetailSubMakerRequest()
                                {
                                    dsDocumentNo = RunDoc,
                                    dsRunNo = i + 1,
                                    dsLotNo = @class._ViewceMastModifyRequest.mfLotNo,// @class._ViewceMastSubMakerRequest.smLotNo,
                                    dsOrderNo = @class._ViewceMastModifyRequest.mfRefNo,// @class._ViewceMastSubMakerRequest.smOrderNo, //wait 
                                    dsRevision = vRevision,
                                    dsGroupName = @class._ListceDetailSubMakerRequest[i].dsGroupName,
                                    dsProcessName = @class._ListceDetailSubMakerRequest[i].dsProcessName,
                                    dsWT_Man = @class._ListceDetailSubMakerRequest[i].dsWT_Man,
                                    dsWT_Auto = @class._ListceDetailSubMakerRequest[i].dsWT_Auto,
                                    dsLabour_Rate = @class._ListceDetailSubMakerRequest[i].dsLabour_Rate,
                                    dsLabour_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate / 1000, 2),//   @class._ListceDetailSubMakerRequest[i].dsLabour_Cost,
                                    dsDP_Rate = @class._ListceDetailSubMakerRequest[i].dsDP_Rate,
                                    dsDP_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsDP_Cost,
                                    dsEnable_WTAuto = @class._ListceDetailSubMakerRequest[i].dsEnable_WTAuto,
                                    dsEnable_WTMan = @class._ListceDetailSubMakerRequest[i].dsEnable_WTMan,
                                    //_ViewceDetailSubMakerRequest.dsDP_Cost = @class._ListceDetailSubMakerRequest[i].dsDP_Cost;
                                    dsME_Rate = @class._ListceDetailSubMakerRequest[i].dsME_Rate,
                                    dsME_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsME_Cost,
                                };
                                _MK._ViewceDetailSubMakerRequest.AddAsync(_ViewceDetailSubMakerRequest);
                            }
                            // _MK.SaveChanges();
                        }
                        ViewceMastModifyRequest _ViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == @class._ViewceMastModifyRequest.mfCENo).FirstOrDefault();
                        if (_ViewceMastModifyRequest != null)
                        {
                            _ViewceMastModifyRequest.mfCENo = RunDoc;
                            _ViewceMastModifyRequest.mfRevision = @class._ViewceMastModifyRequest.mfRevision;
                            _ViewceMastModifyRequest.mfRefNo = @class._ViewceMastModifyRequest.mfRefNo;
                            _ViewceMastModifyRequest.mfLotNo = @class._ViewceMastModifyRequest.mfLotNo;
                            _ViewceMastModifyRequest.mfIssueRate = @class._ViewceMastModifyRequest.mfIssueRate;
                            _ViewceMastModifyRequest.mfCostRate = @class._ViewceMastModifyRequest.mfCostRate;
                            _ViewceMastModifyRequest.mfCostType = @class._ViewceMastModifyRequest.mfCostType;
                            _ViewceMastModifyRequest.mfType = @class._ViewceMastModifyRequest.mfType;
                            _ViewceMastModifyRequest.mfESTCost = @class._ViewceMastModifyRequest.mfESTCost;
                            _ViewceMastModifyRequest.mfResultCost = @class._ViewceMastModifyRequest.mfResultCost;
                            _ViewceMastModifyRequest.mfMKPrice = @class._ViewceMastModifyRequest.mfMKPrice;
                            _ViewceMastModifyRequest.mfCustomerName = @class._ViewceMastModifyRequest.mfCustomerName;
                            _ViewceMastModifyRequest.mfMoldNoOrMoldName = @class._ViewceMastModifyRequest.mfMoldNoOrMoldName;
                            _ViewceMastModifyRequest.mfFunction = @class._ViewceMastModifyRequest.mfFunction;
                            _ViewceMastModifyRequest.mfModelName = @class._ViewceMastModifyRequest.mfModelName;
                            _ViewceMastModifyRequest.mfMoldMass = @class._ViewceMastModifyRequest.mfMoldMass;
                            _ViewceMastModifyRequest.mfCavityNo = @class._ViewceMastModifyRequest.mfCavityNo;
                            _ViewceMastModifyRequest.mfTypeCavity = @class._ViewceMastModifyRequest.mfTypeCavity;
                            _ViewceMastModifyRequest.mfLeadTime = @class._ViewceMastModifyRequest.mfLeadTime;
                            _ViewceMastModifyRequest.mfLabourCost = @class._ViewceMastModifyRequest.mfLabourCost;
                            _ViewceMastModifyRequest.mfDPCost = @class._ViewceMastModifyRequest.mfDPCost;
                            _ViewceMastModifyRequest.mfMECost = @class._ViewceMastModifyRequest.mfMECost;
                            _ViewceMastModifyRequest.mfCostUntilSH = @class._ViewceMastModifyRequest.mfCostUntilSH;
                            _ViewceMastModifyRequest.mfMatCost = @class._ViewceMastModifyRequest.mfMatCost;
                            _ViewceMastModifyRequest.mfRateCostUntilSH = @class._ViewceMastModifyRequest.mfRateCostUntilSH;
                            _ViewceMastModifyRequest.mfTotalESTCost = @class._ViewceMastModifyRequest.mfTotalESTCost;
                            _ViewceMastModifyRequest.mfRoundupotalESTCost = @class._ViewceMastModifyRequest.mfRoundupotalESTCost;
                            _ViewceMastModifyRequest.mfDetail = @class._ViewceMastModifyRequest.mfDetail;
                            _ViewceMastModifyRequest.mfProcess = @class._ViewceMastModifyRequest.mfProcess;
                            _ViewceMastModifyRequest.mfIssueDate = @class._ViewceMastModifyRequest.mfIssueDate;
                            _ViewceMastModifyRequest.mfIssueDept = @class._ViewceMastModifyRequest.mfIssueDept;
                            _ViewceMastModifyRequest.mfEmpCodeRequest = @class._ViewceMastModifyRequest.mfEmpCodeRequest != null && @class._ViewceMastModifyRequest.mfEmpCodeRequest != "" ? @class._ViewceMastModifyRequest.mfEmpCodeRequest : empIssue; // public string  // @class._ViewceMastModifyRequest.mfEmpCodeRequest;
                            _ViewceMastModifyRequest.mfNameRequest = @class._ViewceMastModifyRequest.mfNameRequest != null && @class._ViewceMastModifyRequest.mfNameRequest != "" ? @class._ViewceMastModifyRequest.mfNameRequest : NickNameIssue; ; //@class._ViewceMastModifyRequest.mfNameRequest;
                            _ViewceMastModifyRequest.mfEmpCodeApprove = empApprove; //@class._ViewceMastModifyRequest.mfEmpCodeApprove;
                            _ViewceMastModifyRequest.mfNameApprove = NickNameApprove;//@class._ViewceMastModifyRequest.mfNameApprove;
                            _ViewceMastModifyRequest.mfFlowNo = @class._ViewceMastModifyRequest.mfFlowNo;
                            _ViewceMastModifyRequest.mfStep = vstep;//@class._ViewceMastModifyRequest.mfStep;
                            _ViewceMastModifyRequest.mfStatus = _smStatus;//@class._ViewceMastModifyRequest.mfStatus;
                            _ViewceMastModifyRequest.mfMtTool = @class._ViewceMastModifyRequest.mfMtTool;
                            _ViewceMastModifyRequest.mfTotalMt = @class._ViewceMastModifyRequest.mfTotalMt;

                            _ViewceMastModifyRequest.mfIcsname = @class._ViewceMastModifyRequest.mfIcsname; //30/06/2025

                            _MK._ViewceMastModifyRequest.Update(_ViewceMastModifyRequest);
                        }

                        _MK.SaveChanges();


                    }


                    //v_status = "S";
                    //v_msg = "Save file & send Mail Already";

                    _MK.SaveChanges();
                    dbContextTransaction.Commit();

                    string[] v_statusFile = savefile(@class, files, RunDoc);

                    v_status = v_statusFile[0];
                    v_msg = v_statusFile[1];
                }
                catch (Exception ex)
                {
                   // dbContextTransaction.Rollback();
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    v_status = "E";
                    // v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    v_msg = "Error Save: " + ex.InnerException.Message;
                }
            }

            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }
        public string[] savefile(Class @class, List<IFormFile> file, string RunDoc)
        {
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string fileName = "";
            string v_error = "";
            string v_fnType = "";// v_type != "" ? v_type : @class._ViewsvsServiceRequest.srType;

            string v_status = "";
            string v_msg = "";
            int v_count = 0;
            try
            {
                if (file is null)
                {
                    v_status = "S";
                    v_msg = "Save & Send Mail Already";
                }
                else
                {
                    List<ViewAttachment> listInsert = new List<ViewAttachment>();
                    using (var dbContextTransaction = _IT.Database.BeginTransaction())
                    {
                        try
                        {
                            if (file is null)
                            {
                                //vStatus = "File not found";
                                v_status = "S";
                                v_msg = "Save & Send Mail Already";
                            }
                            else
                            {
                                for (int i = 0; i < file.Count; i++)
                                {
                                    string IssueDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    fileName = IssueDate + "-" + file[i].FileName;//System.IO.Path.GetExtension(file.FileName).ToLower();
                                    string filePath = path + fileName;
                                    var fileLocation = new FileInfo(filePath);
                                    //filePaths.Add(filePath);
                                    if (!Directory.Exists(filePath))
                                    {
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file[i].CopyTo(stream);
                                        }
                                    }
                                    ViewAttachment _viewAttachment = new ViewAttachment();
                                    _viewAttachment.fnNo = RunDoc; // @class._ViewceMastSubMakerRequest.smRevision;//vSno.ToString();
                                    _viewAttachment.fnPath = filePath;
                                    _viewAttachment.fnFilename = fileName;
                                    _viewAttachment.fnIssueBy = IssueBy;
                                    _viewAttachment.fnUpdateBy = IssueBy;
                                    _viewAttachment.fnType = v_fnType;
                                    _viewAttachment.fnProgram = PgName; //Program  name CostEstimate
                                                                        //_IT.Attachment.AddAsync(_viewAttachment);
                                                                        //_IT.SaveChanges();
                                                                        //vStatus = fileName;
                                    listInsert.Add(_viewAttachment);
                                }

                            }
                            _IT.Attachment.AddRange(listInsert); // Add หลายรายการ
                            _IT.SaveChanges(); // Save ทีเดียว
                            dbContextTransaction.Commit();
                            v_status = "S";
                            v_msg = "Save & Send Mail Already";
                        }
                        catch (Exception e)
                        {
                            v_status = "E";
                            v_error = e.Message;
                            v_msg = "Error Save file :" + e.Message;
                            //dbContextTransaction.Rollback();
                            try
                            {
                                dbContextTransaction.Rollback();
                            }
                            catch
                            {
                                // ignore ถ้า transaction ปิดไปแล้ว
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                v_status = "E";
                v_error = e.Message;
                v_msg = "Error Save file :" + e.Message;

            }
            v_count = v_count;
            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }



        //[HttpPost]
        //public IActionResult SaveDraft([FromForm] Class @class, List<IFormFile> files)

        [HttpPost]
        public JsonResult SaveDraft(Class @class, List<IFormFile> files, string _ceItemModifyRequest)
        //public JsonResult SaveDraft(Class @class, List<IFormFile> files, string _vceItemModifyRequest)//  List<ViewceItemModifyRequest> _listItemModify)
        // public JsonResult SaveDraft(Class @class, List<IFormFile> files, string _ceItemModifyRequest,List<ViewceItemModifyRequest> _listViewceItemModifyRequest)
        {

            string config = "S";
            string msg = "";

            string[] chkPermis;
            string[] chkSave;

            int i_Step = 0;
            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;



            try
            {




                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }

                i_Step = @class._ViewceMastModifyRequest != null ? @class._ViewceMastModifyRequest.mfStep : 0;


                //vStatus = savefile(@class, files);
                vRunDoc = RunDocNo(@class);
                if (vRunDoc[0] == "Fail")
                {
                    config = "E";
                    msg = "Error Run Doc No : " + vRunDoc[1];
                    return Json(new { c1 = config, c2 = msg });
                }

                if (_ceItemModifyRequest != null)
                {
                    @class._ListViewceItemModifyRequest = JsonConvert.DeserializeObject<List<ViewceItemModifyRequest>>(_ceItemModifyRequest);
                }

                chkSave = SaveData(@class, i_Step, vRunDoc[0], vRunDoc[1], files);
                if (chkSave[0] == "E")
                {
                    config = chkSave[0];
                    msg = chkSave[1];
                    return Json(new { c1 = config, c2 = msg, c3 = vRunDoc[1] });
                }
                else
                {
                    config = chkSave[0];
                    msg = chkSave[1];
                }


                string vlotNo = @class._ViewceMastModifyRequest.mfLotNo;
                string vRefNo = @class._ViewceMastModifyRequest.mfRefNo;
                string vRevno = @class._ViewceMastModifyRequest.mfRevision is null ? "00" : @class._ViewceMastModifyRequest.mfRevision;

                return Json(new { c1 = config, c2 = msg, c3 = vRunDoc[1], c_lotNo = vlotNo, c_RefNo = vRefNo, c_RevNo = vRevno });


            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
                return Json(new { c1 = config, c2 = msg });
            }

        }





        public ActionResult DeteleDataFile(string id, string vname)
        {
            try
            {
                //var find = _IT.Attachment(X => X.)

                ViewAttachment find = _IT.Attachment.Where(x => x.fnNo == id && x.fnFilename == vname && x.fnProgram == "CostEstimate").FirstOrDefault();
                var delete = _IT.Attachment.Remove(find);


                _IT.SaveChanges();
            }
            catch
            {
                return Json(new { res = "error" });

            }
            return Json(new { res = "success" });


            //return Json(_IT.rpEmails.Where(p => p.emEmail.Contains(term) || p.emEmail_M365.Contains(term)).Select(p => p.emEmail_M365).ToList());

        }


        [HttpPost]
        public JsonResult SaveCeItem(string _ceItemModifyRequest, string _runNo)
        {
            Class @class = new Class();
            string config = "S";
            string msg = "Saved Successfully !!!!";
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    // ลองตรวจสอบว่า Deserialize ได้มั้ย
                    // var items = JsonConvert.DeserializeObject<List<ViewceItemModifyRequest>>(_ceItemModifyRequest);
                    @class._ListViewceItemModifyRequest = JsonConvert.DeserializeObject<List<ViewceItemModifyRequest>>(_ceItemModifyRequest);
                    //add table MastModifyRequest
                    var itemMaterail = _MK._ViewceItemModifyRequest.Where(p => p.imCENo == _runNo).ToList();
                    _MK._ViewceItemModifyRequest.RemoveRange(itemMaterail);
                    _MK.SaveChanges();
                    for (int i = 0; i < @class._ListViewceItemModifyRequest.Count(); i++)
                    {
                        var _ViewceItemModifyRequest = new ViewceItemModifyRequest()
                        {
                            imCENo = _runNo,
                            imItemNo = (i + 1),
                            imItemName = @class._ListViewceItemModifyRequest[i].imItemName,
                            imPCS = @class._ListViewceItemModifyRequest[i].imPCS,
                            imAmount = @class._ListViewceItemModifyRequest[i].imAmount,
                        };
                        _MK._ViewceItemModifyRequest.AddAsync(_ViewceItemModifyRequest);
                    }

                    _MK.SaveChanges();
                    dbContextTransaction.Commit();

                    return Json(new { c1 = config, c2 = msg });
                }

                catch (Exception ex)
                {
                   // dbContextTransaction.Rollback();
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    //v_status = "E";
                    //// v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    //v_msg = "Error Save: " + ex.InnerException.Message;
                    msg = "Something is wrong !!!!! : " + ex.Message;
                    return Json(new { c1 = config, c2 = msg });
                }


            }
        }


        [HttpPost]
        public JsonResult chkSaveData(Class @class, List<IFormFile> files, string _ceItemModifyRequest)
        {
            string config = "S";
            string msg = "Send Mail & Save File Already";

            string vStatus = "";
            string[] chkPermis;
            string[] chkSave;
            string[] chkSaveHistory;
            int i_Step = 0;

            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;
            try
            {


                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }

                i_Step = @class._ViewceMastModifyRequest != null ? @class._ViewceMastModifyRequest.mfStep : 0;

                //check history
                if (@class._ViewceHistoryApproved.htTo != null || (@class._ViewceHistoryApproved.htTo == null && @class._ViewceHistoryApproved.htStatus == "Disapprove"))
                {
                    if (@class._ViewceHistoryApproved.htStatus == "Approve")
                    {
                        i_Step = i_Step + 1;
                        config = "S";

                        ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();
                        if (fromEmailTO == null)
                        {
                            config = "E";
                            msg = "Please Check your Email to , Email incorrect !!!";
                        }

                    }
                    else if (@class._ViewceHistoryApproved.htStatus == "Disapprove")
                    {
                        i_Step = 8;
                        config = "S";
                        string v_empissue = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == @class._ViewceMastModifyRequest.mfCENo).Select(x => x.mfEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                        @class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).First();
                    }
                    else
                    {
                        config = "E";
                        msg = "Please input Status";
                    }
                }
                else
                {
                    config = "E";
                    msg = "Please input e-mail.";
                }

                if (config == "S")
                {
                    //check dm cs , DM Step 0-3  , ST Step 4-7
                    if (i_Step < 2)
                    {
                        //check emp positon
                        try
                        {
                            string v_POS_HCM_CODE = "";
                            string v_empcsup = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).FirstOrDefault();

                            if (i_Step == 1) //GL up
                            {
                                v_POS_HCM_CODE = _HRMS.AccPOSMAST.Where(x => x.POS_CODE == "TL").Select(x => x.POS_HCM_CODE).FirstOrDefault();
                                msg = "Please send approval to GL Up of Dept.!!!";
                            }
                            //else if (i_Step == 2) //DM up
                            //{
                            //    v_POS_HCM_CODE = _HRMS.AccPOSMAST.Where(x => x.POS_CODE == "DDM").Select(x => x.POS_HCM_CODE).FirstOrDefault();
                            //    msg = "Please send approval to DM Up of Dept.!!!";
                            //}

                            ViewAccEMPLOYEE _ViewAccEMPLOYEE = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == v_empcsup).FirstOrDefault();
                            List<ViewAccPOSMAST> _ViewAccPOSMAST = _HRMS.AccPOSMAST.Where(x => int.Parse(x.POS_HCM_CODE) <= int.Parse(v_POS_HCM_CODE)).ToList();
                            string v_chk = _ViewAccPOSMAST.Where(x => x.POS_CODE == _ViewAccEMPLOYEE.POS_CODE).Select(x => x.POS_CODE).FirstOrDefault();

                            if (v_chk == null || v_chk == "")
                            {
                                config = "E";
                                // msg = msg;
                                return Json(new { c1 = config, c2 = msg });
                            }
                        }
                        catch (Exception ex)
                        {
                            config = "E";
                            msg = "Please check email send to !!!!";
                            return Json(new { c1 = config, c2 = msg });
                        }
                    }


                    vRunDoc = RunDocNo(@class);
                    if (vRunDoc[0] == "Fail")
                    {
                        config = "E";
                        msg = "Error Run Doc No : " + vRunDoc[1];
                        return Json(new { c1 = config, c2 = msg });
                    }

                    if (_ceItemModifyRequest != null)
                    {
                        @class._ListViewceItemModifyRequest = JsonConvert.DeserializeObject<List<ViewceItemModifyRequest>>(_ceItemModifyRequest);
                    }

                    //test send mail
                    string vEmpCodeCCemail = "";
                    int vstep = i_Step;
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "2").Select(x => x.mfSubject).First();
                    vstep = vstep == 8 ? vstep = 0 : vstep;

                    if (@class._ViewceHistoryApproved.htCC != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                    vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }
                    }

                    ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                    _ViewceHistoryApproved.htDocNo = vRunDoc[1];// getSrNo[0].ToString();
                    _ViewceHistoryApproved.htStep = vstep;
                    _ViewceHistoryApproved.htStatus = @class._ViewceHistoryApproved.htStatus;
                    _ViewceHistoryApproved.htFrom = @class._ViewceHistoryApproved.htFrom;
                    _ViewceHistoryApproved.htTo = @class._ViewceHistoryApproved.htTo;
                    _ViewceHistoryApproved.htCC = vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                    _ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                    _ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                    _ViewceHistoryApproved.htRemark = @class._ViewceHistoryApproved.htRemark;
                    _MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);

                    _MK.SaveChanges();



                    chkSave = SaveData(@class, i_Step, vRunDoc[0], vRunDoc[1], files);
                    if (chkSave[0] == "E")
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                    }

                    //test send mail
                    //string vEmpCodeCCemail = "";
                    //int vstep = i_Step;
                    //string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "2").Select(x => x.mfSubject).First();
                    //vstep = vstep == 8 ? vstep = 0 : vstep;

                    ////if (@class._ViewceHistoryApproved.htCC != null)
                    ////{
                    ////    ViewrpEmail fromEmailCC = new ViewrpEmail();
                    ////    string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                    ////    foreach (var i in splitCC)
                    ////    {
                    ////        if (i != " " & i != "")
                    ////        {
                    ////            var v_cc = "";
                    ////            try
                    ////            {
                    ////                fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                    ////                vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                    ////            }
                    ////            catch (Exception e)
                    ////            {
                    ////                v_cc = e.Message;
                    ////            }
                    ////        }
                    ////    }
                    ////}

                    //ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                    //_ViewceHistoryApproved.htDocNo = vRunDoc[1];// getSrNo[0].ToString();
                    //_ViewceHistoryApproved.htStep = 0;
                    //_ViewceHistoryApproved.htStatus = @class._ViewceHistoryApproved.htStatus;
                    //_ViewceHistoryApproved.htFrom = @class._ViewceHistoryApproved.htFrom;
                    //_ViewceHistoryApproved.htTo = @class._ViewceHistoryApproved.htTo;
                    //_ViewceHistoryApproved.htCC = "011998";//vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                    //_ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                    //_ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                    //_ViewceHistoryApproved.htRemark = "111111111111111111111111111111";
                    //_MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);

                    //_MK.SaveChanges();




                    //saveHistory
                    chkSaveHistory = saveHistory(@class, i_Step, vRunDoc[0], vRunDoc[1]);
                    if (chkSaveHistory[0] == "E")
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                    }


                    ////ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                    //_ViewceHistoryApproved.htDocNo = "222222";// getSrNo[0].ToString();
                    //_ViewceHistoryApproved.htStep = 0;
                    //_ViewceHistoryApproved.htStatus = "test";
                    //_ViewceHistoryApproved.htFrom = "test";
                    //_ViewceHistoryApproved.htTo = "test";
                    //_ViewceHistoryApproved.htCC = "015142";//@class._ViewceHistoryApproved.htCC;
                    //_ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                    //_ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                    //_ViewceHistoryApproved.htRemark = "test";
                    //_MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);
                    //_MK.SaveChanges();




                    string vlotNo = @class._ViewceMastModifyRequest.mfLotNo;
                    string vRefNo = @class._ViewceMastModifyRequest.mfRefNo;
                    string vRevno = @class._ViewceMastModifyRequest.mfRevision is null ? "00" : @class._ViewceMastModifyRequest.mfRevision;

                    return Json(new { c1 = config, c2 = msg, c3 = vRunDoc[1], c_lotNo = vlotNo, c_RefNo = vRefNo, c_RevNo = vRevno });
                }
                else
                {
                    config = "E";
                    msg = msg;
                    return Json(new { c1 = config, c2 = msg });
                }
            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
                return Json(new { c1 = config, c2 = msg });
            }

        }

        public string[] SaveData(Class @class, int vstep, string status, string RunDoc, List<IFormFile> files)
        {
            string empissue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string UpdateBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            //User.Claims.FirstOrDefault(s => s.Type == "NICKNAME")?.Value;
            string v_msg = "";
            string v_status = "";



            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string vDocCode = "CE";
                    string vDocSub = "M";
                    string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
                    string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);
                    string vRevision = @class._ViewceMastModifyRequest.mfRevision != null && @class._ViewceMastModifyRequest.mfRevision != "" ? @class._ViewceMastModifyRequest.mfRevision : "00"; //String.Format("{0:D3}", RunDoc.Substring(11, 3));

                    string empIssue = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).Select(x => x.emEmpcode).First() :
                                        @class._ViewceMastModifyRequest.mfEmpCodeRequest == null ? empissue : @class._ViewceMastModifyRequest.mfEmpCodeRequest;
                    string NickNameIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.NICKNAME).First();
                    string DeptIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.DEPT_CODE).First();

                    string empApprove = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() : @class._ViewceMastModifyRequest.mfEmpCodeApprove;
                    string NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastModifyRequest.mfNameApprove;


                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "2").Select(x => x.mfSubject).First();

                    //case dis approve
                    vstep = vstep == 8 ? vstep = 0 : vstep;






                    // status New  // status Update
                    if (status == "New")
                    {
                        //save run doc //CE-S-25-03-001 10,3


                        ViewceRunDocument _ViewceRunDocument = new ViewceRunDocument();
                        _ViewceRunDocument.rmRunNo = int.Parse(RunDoc.Substring(11, 3));
                        _ViewceRunDocument.rmDocCode = vDocCode;
                        _ViewceRunDocument.rmDocSub = vDocSub;
                        _ViewceRunDocument.rmYear = vYY;
                        _ViewceRunDocument.rmMonth = vMM;
                        _ViewceRunDocument.rmIssueBy = IssueBy;
                        _ViewceRunDocument.rmIssueBy = UpdateBy;
                        _MK._ViewceRunDocument.AddAsync(_ViewceRunDocument);




                        ////add table MastModifyRequest
                        //var itemMaterail = _MK._ViewceItemModifyRequest.Where(p => p.imCENo == RunDoc).ToList();
                        //_MK._ViewceItemModifyRequest.RemoveRange(itemMaterail);
                        //_MK.SaveChanges();
                        for (int i = 0; i < @class._ListViewceItemModifyRequest.Count(); i++)
                        {
                            var _ViewceItemModifyRequest = new ViewceItemModifyRequest()
                            {
                                imCENo = RunDoc,
                                imItemNo = (i + 1),
                                imItemName = @class._ListViewceItemModifyRequest[i].imItemName,
                                imPCS = @class._ListViewceItemModifyRequest[i].imPCS,
                                imAmount = @class._ListViewceItemModifyRequest[i].imAmount,
                            };
                            _MK._ViewceItemModifyRequest.AddAsync(_ViewceItemModifyRequest);
                        }



                        //table ceDetailSubMakerRequest

                        //if (@class._ListceDetailSubMakerRequest.Count() > 0)
                        //{
                        //    for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                        //    {
                        //        var _ViewceDetailSubMakerRequest = new ViewceDetailSubMakerRequest()
                        //        {
                        //            dsDocumentNo = RunDoc,
                        //            dsRunNo = i + 1,
                        //            dsLotNo = @class._ViewceMastModifyRequest.mfLotNo,// @class._ViewceMastSubMakerRequest.smLotNo,
                        //            dsOrderNo = @class._ViewceMastModifyRequest.mfRefNo,// @class._ViewceMastSubMakerRequest.smOrderNo, //wait 
                        //            dsRevision = vRevision,
                        //            dsGroupName = @class._ListceDetailSubMakerRequest[i].dsGroupName,
                        //            dsProcessName = @class._ListceDetailSubMakerRequest[i].dsProcessName,
                        //            dsWT_Man = @class._ListceDetailSubMakerRequest[i].dsWT_Man,
                        //            dsWT_Auto = @class._ListceDetailSubMakerRequest[i].dsWT_Auto,
                        //            dsEnable_WTAuto = @class._ListceDetailSubMakerRequest[i].dsEnable_WTAuto,
                        //            dsEnable_WTMan = @class._ListceDetailSubMakerRequest[i].dsEnable_WTMan,
                        //            dsLabour_Rate = @class._ListceDetailSubMakerRequest[i].dsLabour_Rate,
                        //            dsLabour_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate / 1000, 2),//   @class._ListceDetailSubMakerRequest[i].dsLabour_Cost,
                        //            dsDP_Rate = @class._ListceDetailSubMakerRequest[i].dsDP_Rate,
                        //            dsDP_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsDP_Cost,
                        //            //_ViewceDetailSubMakerRequest.dsDP_Cost = @class._ListceDetailSubMakerRequest[i].dsDP_Cost;
                        //            dsME_Rate = @class._ListceDetailSubMakerRequest[i].dsME_Rate,
                        //            dsME_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsME_Cost,
                        //        };
                        //        //s_dsWT_Man += _ViewceDetailSubMakerRequest.dsWT_Man;
                        //        //s_dsWT_Auto += _ViewceDetailSubMakerRequest.dsWT_Auto;
                        //        _MK._ViewceDetailSubMakerRequest.AddAsync(_ViewceDetailSubMakerRequest);
                        //    }

                        //}

                        ViewceMastModifyRequest _ViewceMastModifyRequest = new ViewceMastModifyRequest();
                        _ViewceMastModifyRequest.mfCENo = RunDoc;
                        _ViewceMastModifyRequest.mfRevision = vRevision;//@class._ViewceMastModifyRequest.mfRevision;
                        _ViewceMastModifyRequest.mfRefNo = @class._ViewceMastModifyRequest.mfRefNo;
                        _ViewceMastModifyRequest.mfLotNo = @class._ViewceMastModifyRequest.mfLotNo;
                        _ViewceMastModifyRequest.mfIssueRate = @class._ViewceMastModifyRequest.mfIssueRate;
                        _ViewceMastModifyRequest.mfCostRate = @class._ViewceMastModifyRequest.mfCostRate;
                        _ViewceMastModifyRequest.mfCostType = @class._ViewceMastModifyRequest.mfCostType;
                        _ViewceMastModifyRequest.mfType = @class._ViewceMastModifyRequest.mfType;
                        _ViewceMastModifyRequest.mfESTCost = @class._ViewceMastModifyRequest.mfESTCost;
                        _ViewceMastModifyRequest.mfResultCost = @class._ViewceMastModifyRequest.mfResultCost;
                        _ViewceMastModifyRequest.mfMKPrice = @class._ViewceMastModifyRequest.mfMKPrice;
                        _ViewceMastModifyRequest.mfCustomerName = @class._ViewceMastModifyRequest.mfCustomerName;
                        _ViewceMastModifyRequest.mfMoldNoOrMoldName = @class._ViewceMastModifyRequest.mfMoldNoOrMoldName;
                        _ViewceMastModifyRequest.mfFunction = @class._ViewceMastModifyRequest.mfFunction;
                        _ViewceMastModifyRequest.mfModelName = @class._ViewceMastModifyRequest.mfModelName;
                        _ViewceMastModifyRequest.mfMoldMass = @class._ViewceMastModifyRequest.mfMoldMass;
                        _ViewceMastModifyRequest.mfCavityNo = @class._ViewceMastModifyRequest.mfCavityNo;
                        _ViewceMastModifyRequest.mfTypeCavity = @class._ViewceMastModifyRequest.mfTypeCavity;
                        _ViewceMastModifyRequest.mfLeadTime = @class._ViewceMastModifyRequest.mfLeadTime;
                        _ViewceMastModifyRequest.mfLabourCost = @class._ViewceMastModifyRequest.mfLabourCost;
                        _ViewceMastModifyRequest.mfDPCost = @class._ViewceMastModifyRequest.mfDPCost;
                        _ViewceMastModifyRequest.mfMECost = @class._ViewceMastModifyRequest.mfMECost;
                        _ViewceMastModifyRequest.mfCostUntilSH = @class._ViewceMastModifyRequest.mfCostUntilSH;
                        _ViewceMastModifyRequest.mfMatCost = @class._ViewceMastModifyRequest.mfMatCost;
                        _ViewceMastModifyRequest.mfRateCostUntilSH = @class._ViewceMastModifyRequest.mfRateCostUntilSH;
                        _ViewceMastModifyRequest.mfTotalESTCost = @class._ViewceMastModifyRequest.mfTotalESTCost;
                        _ViewceMastModifyRequest.mfRoundupotalESTCost = @class._ViewceMastModifyRequest.mfRoundupotalESTCost;
                        _ViewceMastModifyRequest.mfDetail = @class._ViewceMastModifyRequest.mfDetail;
                        _ViewceMastModifyRequest.mfProcess = @class._ViewceMastModifyRequest.mfProcess;
                        _ViewceMastModifyRequest.mfIssueDate = @class._ViewceMastModifyRequest.mfIssueDate;
                        _ViewceMastModifyRequest.mfIssueDept = @class._ViewceMastModifyRequest.mfIssueDept;
                        _ViewceMastModifyRequest.mfEmpCodeRequest = @class._ViewceMastModifyRequest.mfEmpCodeRequest != null && @class._ViewceMastModifyRequest.mfEmpCodeRequest != "" ? @class._ViewceMastModifyRequest.mfEmpCodeRequest : empIssue; // public string  // @class._ViewceMastModifyRequest.mfEmpCodeRequest;
                        _ViewceMastModifyRequest.mfNameRequest = @class._ViewceMastModifyRequest.mfNameRequest != null && @class._ViewceMastModifyRequest.mfNameRequest != "" ? @class._ViewceMastModifyRequest.mfNameRequest : NickNameIssue; ; //@class._ViewceMastModifyRequest.mfNameRequest;
                        _ViewceMastModifyRequest.mfEmpCodeApprove = empApprove; //@class._ViewceMastModifyRequest.mfEmpCodeApprove;
                        _ViewceMastModifyRequest.mfNameApprove = NickNameApprove;//@class._ViewceMastModifyRequest.mfNameApprove;
                        _ViewceMastModifyRequest.mfFlowNo = 2;// @class._ViewceMastModifyRequest.mfFlowNo;
                        _ViewceMastModifyRequest.mfStep = vstep;//@class._ViewceMastModifyRequest.mfStep;
                        _ViewceMastModifyRequest.mfStatus = _smStatus;//@class._ViewceMastModifyRequest.mfStatus;
                        _ViewceMastModifyRequest.mfMtTool = @class._ViewceMastModifyRequest.mfMtTool;
                        _ViewceMastModifyRequest.mfTotalMt = @class._ViewceMastModifyRequest.mfTotalMt;

                        _ViewceMastModifyRequest.mfIcsname = @class._ViewceMastModifyRequest.mfIcsname; //30/06/2025
                        _MK._ViewceMastModifyRequest.AddAsync(_ViewceMastModifyRequest);
                        _MK.SaveChanges();
                    }

                    // status Old
                    else if (status == "Update")
                    {
                        //add table MastModifyRequest
                        var itemMaterail = _MK._ViewceItemModifyRequest.Where(p => p.imCENo == RunDoc).ToList();
                        if (itemMaterail.Count > 0)
                        {
                            _MK._ViewceItemModifyRequest.RemoveRange(itemMaterail);
                            _MK.SaveChanges();
                        }
                        for (int i = 0; i < @class._ListViewceItemModifyRequest.Count(); i++)
                        {
                            var _ViewceItemModifyRequest = new ViewceItemModifyRequest()
                            {
                                imCENo = RunDoc,
                                imItemNo = (i + 1),
                                imItemName = @class._ListViewceItemModifyRequest[i].imItemName,
                                imPCS = @class._ListViewceItemModifyRequest[i].imPCS,
                                imAmount = @class._ListViewceItemModifyRequest[i].imAmount,
                            };
                            _MK._ViewceItemModifyRequest.AddAsync(_ViewceItemModifyRequest);
                        }

                        //if (@class._ListceDetailSubMakerRequest.Count() > 0)
                        //{
                        //    var products = _MK._ViewceDetailSubMakerRequest.Where(p => p.dsDocumentNo == RunDoc).ToList();
                        //    if (products.Count > 0)
                        //    {
                        //        _MK._ViewceDetailSubMakerRequest.RemoveRange(products);
                        //        //_MK.SaveChangesAsync();
                        //        _MK.SaveChanges();
                        //    }
                        //    for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                        //    {
                        //        var _ViewceDetailSubMakerRequest = new ViewceDetailSubMakerRequest()
                        //        {
                        //            dsDocumentNo = RunDoc,
                        //            dsRunNo = i + 1,
                        //            dsLotNo = @class._ViewceMastModifyRequest.mfLotNo,// @class._ViewceMastSubMakerRequest.smLotNo,
                        //            dsOrderNo = @class._ViewceMastModifyRequest.mfRefNo,// @class._ViewceMastSubMakerRequest.smOrderNo, //wait 
                        //            dsRevision = vRevision,
                        //            dsGroupName = @class._ListceDetailSubMakerRequest[i].dsGroupName,
                        //            dsProcessName = @class._ListceDetailSubMakerRequest[i].dsProcessName,
                        //            dsWT_Man = @class._ListceDetailSubMakerRequest[i].dsWT_Man,
                        //            dsWT_Auto = @class._ListceDetailSubMakerRequest[i].dsWT_Auto,
                        //            dsLabour_Rate = @class._ListceDetailSubMakerRequest[i].dsLabour_Rate,
                        //            dsLabour_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate / 1000, 2),//   @class._ListceDetailSubMakerRequest[i].dsLabour_Cost,
                        //            dsDP_Rate = @class._ListceDetailSubMakerRequest[i].dsDP_Rate,
                        //            dsDP_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsDP_Cost,
                        //            dsEnable_WTAuto = @class._ListceDetailSubMakerRequest[i].dsEnable_WTAuto,
                        //            dsEnable_WTMan = @class._ListceDetailSubMakerRequest[i].dsEnable_WTMan,
                        //            //_ViewceDetailSubMakerRequest.dsDP_Cost = @class._ListceDetailSubMakerRequest[i].dsDP_Cost;
                        //            dsME_Rate = @class._ListceDetailSubMakerRequest[i].dsME_Rate,
                        //            dsME_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsME_Cost,
                        //        };
                        //        _MK._ViewceDetailSubMakerRequest.AddAsync(_ViewceDetailSubMakerRequest);
                        //    }
                        //    // _MK.SaveChanges();
                        //}
                        ViewceMastModifyRequest _ViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == @class._ViewceMastModifyRequest.mfCENo).FirstOrDefault();
                        if (_ViewceMastModifyRequest != null)
                        {
                            _ViewceMastModifyRequest.mfCENo = RunDoc;
                            _ViewceMastModifyRequest.mfRevision = @class._ViewceMastModifyRequest.mfRevision;
                            _ViewceMastModifyRequest.mfRefNo = @class._ViewceMastModifyRequest.mfRefNo;
                            _ViewceMastModifyRequest.mfLotNo = @class._ViewceMastModifyRequest.mfLotNo;
                            _ViewceMastModifyRequest.mfIssueRate = @class._ViewceMastModifyRequest.mfIssueRate;
                            _ViewceMastModifyRequest.mfCostRate = @class._ViewceMastModifyRequest.mfCostRate;
                            _ViewceMastModifyRequest.mfCostType = @class._ViewceMastModifyRequest.mfCostType;
                            _ViewceMastModifyRequest.mfType = @class._ViewceMastModifyRequest.mfType;
                            _ViewceMastModifyRequest.mfESTCost = @class._ViewceMastModifyRequest.mfESTCost;
                            _ViewceMastModifyRequest.mfResultCost = @class._ViewceMastModifyRequest.mfResultCost;
                            _ViewceMastModifyRequest.mfMKPrice = @class._ViewceMastModifyRequest.mfMKPrice;
                            _ViewceMastModifyRequest.mfCustomerName = @class._ViewceMastModifyRequest.mfCustomerName;
                            _ViewceMastModifyRequest.mfMoldNoOrMoldName = @class._ViewceMastModifyRequest.mfMoldNoOrMoldName;
                            _ViewceMastModifyRequest.mfFunction = @class._ViewceMastModifyRequest.mfFunction;
                            _ViewceMastModifyRequest.mfModelName = @class._ViewceMastModifyRequest.mfModelName;
                            _ViewceMastModifyRequest.mfMoldMass = @class._ViewceMastModifyRequest.mfMoldMass;
                            _ViewceMastModifyRequest.mfCavityNo = @class._ViewceMastModifyRequest.mfCavityNo;
                            _ViewceMastModifyRequest.mfTypeCavity = @class._ViewceMastModifyRequest.mfTypeCavity;
                            _ViewceMastModifyRequest.mfLeadTime = @class._ViewceMastModifyRequest.mfLeadTime;
                            _ViewceMastModifyRequest.mfLabourCost = @class._ViewceMastModifyRequest.mfLabourCost;
                            _ViewceMastModifyRequest.mfDPCost = @class._ViewceMastModifyRequest.mfDPCost;
                            _ViewceMastModifyRequest.mfMECost = @class._ViewceMastModifyRequest.mfMECost;
                            _ViewceMastModifyRequest.mfCostUntilSH = @class._ViewceMastModifyRequest.mfCostUntilSH;
                            _ViewceMastModifyRequest.mfMatCost = @class._ViewceMastModifyRequest.mfMatCost;
                            _ViewceMastModifyRequest.mfRateCostUntilSH = @class._ViewceMastModifyRequest.mfRateCostUntilSH;
                            _ViewceMastModifyRequest.mfTotalESTCost = @class._ViewceMastModifyRequest.mfTotalESTCost;
                            _ViewceMastModifyRequest.mfRoundupotalESTCost = @class._ViewceMastModifyRequest.mfRoundupotalESTCost;
                            _ViewceMastModifyRequest.mfDetail = @class._ViewceMastModifyRequest.mfDetail;
                            _ViewceMastModifyRequest.mfProcess = @class._ViewceMastModifyRequest.mfProcess;
                            _ViewceMastModifyRequest.mfIssueDate = @class._ViewceMastModifyRequest.mfIssueDate;
                            _ViewceMastModifyRequest.mfIssueDept = @class._ViewceMastModifyRequest.mfIssueDept;
                            _ViewceMastModifyRequest.mfEmpCodeRequest = @class._ViewceMastModifyRequest.mfEmpCodeRequest != null && @class._ViewceMastModifyRequest.mfEmpCodeRequest != "" ? @class._ViewceMastModifyRequest.mfEmpCodeRequest : empIssue; // public string  // @class._ViewceMastModifyRequest.mfEmpCodeRequest;
                            _ViewceMastModifyRequest.mfNameRequest = @class._ViewceMastModifyRequest.mfNameRequest != null && @class._ViewceMastModifyRequest.mfNameRequest != "" ? @class._ViewceMastModifyRequest.mfNameRequest : NickNameIssue; ; //@class._ViewceMastModifyRequest.mfNameRequest;
                            _ViewceMastModifyRequest.mfEmpCodeApprove = empApprove; //@class._ViewceMastModifyRequest.mfEmpCodeApprove;
                            _ViewceMastModifyRequest.mfNameApprove = NickNameApprove;//@class._ViewceMastModifyRequest.mfNameApprove;
                            _ViewceMastModifyRequest.mfFlowNo = @class._ViewceMastModifyRequest.mfFlowNo;
                            _ViewceMastModifyRequest.mfStep = vstep;//@class._ViewceMastModifyRequest.mfStep;
                            _ViewceMastModifyRequest.mfStatus = _smStatus;//@class._ViewceMastModifyRequest.mfStatus;
                            _ViewceMastModifyRequest.mfMtTool = @class._ViewceMastModifyRequest.mfMtTool;
                            _ViewceMastModifyRequest.mfTotalMt = @class._ViewceMastModifyRequest.mfTotalMt;

                            _ViewceMastModifyRequest.mfIcsname = @class._ViewceMastModifyRequest.mfIcsname; //30/06/2025
                            _MK._ViewceMastModifyRequest.Update(_ViewceMastModifyRequest);
                        }

                        _MK.SaveChanges();


                    }



                    //test send mail 
                    //string vEmpCodeCCemail = "";
                    ////string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "2").Select(x => x.mfSubject).First();
                    //vstep = vstep == 8 ? vstep = 0 : vstep;

                    //if (@class._ViewceHistoryApproved.htCC != null)
                    //{
                    //    ViewrpEmail fromEmailCC = new ViewrpEmail();
                    //    string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                    //    foreach (var i in splitCC)
                    //    {
                    //        if (i != " " & i != "")
                    //        {
                    //            var v_cc = "";
                    //            try
                    //            {
                    //                fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                    //                vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                    //            }
                    //            catch (Exception e)
                    //            {
                    //                v_cc = e.Message;
                    //            }
                    //        }
                    //    }
                    //}

                    //ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                    //_ViewceHistoryApproved.htDocNo = RunDoc;// getSrNo[0].ToString();
                    //_ViewceHistoryApproved.htStep = vstep;
                    //_ViewceHistoryApproved.htStatus = @class._ViewceHistoryApproved.htStatus;
                    //_ViewceHistoryApproved.htFrom = @class._ViewceHistoryApproved.htFrom;
                    //_ViewceHistoryApproved.htTo = @class._ViewceHistoryApproved.htTo;
                    //_ViewceHistoryApproved.htCC = vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                    //_ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                    //_ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                    //_ViewceHistoryApproved.htRemark = "111111111111111111111111111111";
                    //_MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);

                    //_MK.SaveChanges();

                    //var _vViewceHistoryApproved = new ViewceHistoryApproved()
                    //{
                    //    htDocNo = RunDoc,
                    //    htStep = vstep,
                    //    htStatus = @class._ViewceHistoryApproved.htStatus,
                    //    htFrom = @class._ViewceHistoryApproved.htFrom,
                    //    htTo = @class._ViewceHistoryApproved.htTo,
                    //    htCC = vEmpCodeCCemail,
                    //    htDate = DateTime.Now.ToString("yyyy/MM/dd"),
                    //    htTime = DateTime.Now.ToString("HH:mm:ss"),
                    //    htRemark = "222222222222",

                    //};
                    //_MK._ViewceHistoryApproved.AddAsync(_vViewceHistoryApproved);








                    //v_status = "S";
                    //v_msg = "Save file & send Mail Already";

                    _MK.SaveChanges();
                    dbContextTransaction.Commit();

                    string[] v_statusFile = savefile(@class, files, RunDoc);

                    v_status = v_statusFile[0];
                    v_msg = v_statusFile[1];
                }
                catch (Exception ex)
                {
                   // dbContextTransaction.Rollback();
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    v_status = "E";
                    // v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    v_msg = "Error Save: " + ex.InnerException.Message;
                }
            }


            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        [HttpPost]
        public JsonResult SaveCeDetailSub(Class @class, string _runNo, string _LotNo, string _RefNo, string _RevNo)
        {
            //Class @class = new Class();
            string config = "S";
            string msg = "Saved Successfully !!!!";
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    if (@class._ListceDetailSubMakerRequest.Count() > 0)
                    {
                        var products = _MK._ViewceDetailSubMakerRequest.Where(p => p.dsDocumentNo == _runNo).ToList();
                        if (products.Count > 0)
                        {
                            _MK._ViewceDetailSubMakerRequest.RemoveRange(products);
                            //_MK.SaveChangesAsync();
                            _MK.SaveChanges();
                        }

                        for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                        {
                            var _ViewceDetailSubMakerRequest = new ViewceDetailSubMakerRequest()
                            {
                                dsDocumentNo = _runNo,
                                dsRunNo = i + 1,
                                dsLotNo = _LotNo,//@class._ViewceMastModifyRequest.mfLotNo,// @class._ViewceMastSubMakerRequest.smLotNo,
                                dsOrderNo = _RefNo,//@class._ViewceMastModifyRequest.mfRefNo,// @class._ViewceMastSubMakerRequest.smOrderNo, //wait 
                                dsRevision = _RevNo,
                                dsGroupName = @class._ListceDetailSubMakerRequest[i].dsGroupName,
                                dsProcessName = @class._ListceDetailSubMakerRequest[i].dsProcessName,
                                dsWT_Man = @class._ListceDetailSubMakerRequest[i].dsWT_Man,
                                dsWT_Auto = @class._ListceDetailSubMakerRequest[i].dsWT_Auto,
                                dsLabour_Rate = @class._ListceDetailSubMakerRequest[i].dsLabour_Rate,
                                dsLabour_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate, 2),//Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate / 1000, 2),//   @class._ListceDetailSubMakerRequest[i].dsLabour_Cost,
                                dsDP_Rate = @class._ListceDetailSubMakerRequest[i].dsDP_Rate,
                                dsDP_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate, 2), //  Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsDP_Cost,
                                dsEnable_WTAuto = @class._ListceDetailSubMakerRequest[i].dsEnable_WTAuto,
                                dsEnable_WTMan = @class._ListceDetailSubMakerRequest[i].dsEnable_WTMan,
                                //_ViewceDetailSubMakerRequest.dsDP_Cost = @class._ListceDetailSubMakerRequest[i].dsDP_Cost;
                                dsME_Rate = @class._ListceDetailSubMakerRequest[i].dsME_Rate,
                                dsME_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate, 2)//Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsME_Cost,
                            };
                            _MK._ViewceDetailSubMakerRequest.AddAsync(_ViewceDetailSubMakerRequest);
                        }
                        // _MK.SaveChanges();
                    }
                    _MK.SaveChanges();
                    dbContextTransaction.Commit();

                    return Json(new { c1 = config, c2 = msg });
                }

                catch (Exception ex)
                {
                   // dbContextTransaction.Rollback();
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    //v_status = "E";
                    //// v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    //v_msg = "Error Save: " + ex.InnerException.Message;
                    msg = "Something is wrong !!!!! : " + ex.Message;
                    return Json(new { c1 = config, c2 = msg });
                }


            }
        }


    }
}