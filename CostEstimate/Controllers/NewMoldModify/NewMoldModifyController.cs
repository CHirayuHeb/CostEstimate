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
        public IActionResult Index(Class @class, string id)
        {

            try
            {
                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                @class._ViewOperaterCP = new ViewOperaterCP();
                @class._ViewceMastFlowApprove = new ViewceMastFlowApprove();

                List<string> _listTypeofCavity = new List<string>{
                                            "CAVITIES(R/L =1 Set) x 2MOLD",
                                            "CAVITIES(R/L =1 Set)",
                                            "CAVITIES",
                                            "CAVITY"};
                SelectList _TypeofCavity = new SelectList(_listTypeofCavity);
                ViewBag.TypeofCavity = _TypeofCavity;

                List<string> _listTypeofMODIFICATION = new List<string>{
                                            "CLAIM SUB MAKER",
                                            "MOLD MODIFICATION"};
                SelectList _TypeofMODIFICATION = new SelectList(_listTypeofMODIFICATION);
                ViewBag.TypeofMODIFICATION = _TypeofMODIFICATION;


                @class._ViewOperaterCP = new ViewOperaterCP();
                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.ToList();


                //@class._ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == "CE-S-25-05-008").FirstOrDefault();
               
            }
            catch (Exception ex)
            {
                ViewBag.test = ex.Message;
            }

            return View(@class);
        }


        [HttpPost]
        public PartialViewResult PrintMoldQUOTATION(string mpNo, Class @class)
        {
            try
            {


                if (mpNo != null)
                {
                    @class._ViewOperaterCP = new ViewOperaterCP();

                    string tbHistoryIssueName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryIssueEMPCODE = tbHistoryIssueName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryIssueName)).Select(x => x.emEmpcode).FirstOrDefault();

                    string tbHistoryCheckedGLName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedGLEMPCODE = tbHistoryCheckedGLName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedGLName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryCheckedName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedEMPCODE = tbHistoryCheckedName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryApproveByName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault();
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

                    @class._ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == mpNo).FirstOrDefault();

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
            {
             
                return Json(_MOLD._ViewLLLedger.Where(p => p.LGLegNo.Contains(term)).Select(p => p.LGLegNo + "" + p.LGTypeCode + "|" + p.LGCustomer + "|" + p.LGMoldNo + "/" + p.LGMoldName + "|" + "0").ToList());
                // return Json(_MOLD._ViewmtMaster_Mold_Control.Where(p => p.mcLedger_Number.Contains(term)).Select(p => p.mcLedger_Number + "|" + p.mcCUS + "|" + p.mcMoldname + "|" + (p.mcCavity != "" ? p.mcCavity : "0")).ToList());
            }
        }
    }
}