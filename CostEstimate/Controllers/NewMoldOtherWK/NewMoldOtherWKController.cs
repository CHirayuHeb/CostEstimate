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

namespace CostEstimate.Controllers.NewMoldOtherWK
{
    public class NewMoldOtherWKController : Controller
    {
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public string path = @"\\thsweb\\CostEstimate\\";
        public string PgName = "CostEstimate";
        public NewMoldOtherWKController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
        public IActionResult Index(Class @class, string Docno)
        {
            @class._listAttachment = new List<ViewAttachment>();
            @class._ViewceMastModifyRequest = new ViewceMastModifyRequest();
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "4").ToList();


            List<string> _listSizeProduct = _MK._ViewceMastType.Where(x => x.mtType.Contains("SizeProduct") && x.mtProgram.Contains("MoldOtherWK")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList _TypeofSizeProduct = new SelectList(_listSizeProduct);
            ViewBag.TypeofSizeProduct = _TypeofSizeProduct;


            @class._ListViewceSubWorkingTimeRequestItem = new List<ViewceSubWorkingTimeRequestItem>();
            @class._ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "MoldOtherWK").OrderBy(x=>x.mpNo).ToList();
            for (int i = 0; i < @class._ListceMastProcess.Count(); i++)
            {
                @class._ListViewceSubWorkingTimeRequestItem.Add(new ViewceSubWorkingTimeRequestItem
                {
                    wriDocnomain = "",
                    wriRunno = i + 1,
                    wriPartName = "",
                    wriCusName​ = "",
                    wriCavityNo = "",
                    wriTypeCavity = "",
                    wriGroupName = @class._ListceMastProcess[i].mpGroupName.Trim(),
                    wriProcessName = @class._ListceMastProcess[i].mpProcessName.Trim(),
                    wriWT_Man = 0,
                    wriWT_Auto = 0,
                    wriEnable_WTMan = @class._ListceMastProcess[i].mpEnable_WTMan,
                    wriEnable_WTAuto = @class._ListceMastProcess[i].mpEnable_WTAuto,
                    wriTotal = 0,
                    wriRemain = 0,
                });
            }

            @class._ListGroupViewceSubWorkingTimeRequestItem = new List<GroupViewceSubWorkingTimeRequestItem>();
            @class._ListGroupViewceSubWorkingTimeRequestItem = @class._ListViewceSubWorkingTimeRequestItem.GroupBy(p => p.wriGroupName).Select(g => new GroupViewceSubWorkingTimeRequestItem
            {
                GroupName = g.Key.Trim(),
                WorkingTimeRequestItem = g.ToList()
            }
            ).ToList();



            return View(@class);
        }

        [Authorize("Checked")]
        [HttpPost]
        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            int v_step = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfStep : 0;
            string v_issue = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfCENo : "";
            List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
            partialUrl = Url.Action("SendMail", "NewMoldOtherWK", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });
            return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
        }
        public ActionResult SendMail(Class @class, int s_step, string s_issue, string mpNo)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
            var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365


            @class._ViewceHistoryApproved.htFrom = v_emailFrom;
            @class._ViewceHistoryApproved.htStatus = "Approve";
            return PartialView("SendMail", @class);
        }


    }
}