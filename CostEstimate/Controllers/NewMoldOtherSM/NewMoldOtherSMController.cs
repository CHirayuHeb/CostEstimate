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


namespace CostEstimate.Controllers.NewMoldOtherSM
{
    public class NewMoldOtherSMController : Controller
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
        public NewMoldOtherSMController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "7").ToList();

            @class._ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();
            @class._ViewceMastInforSpacMoldRequest = new ViewceMastInforSpacMoldRequest();
            @class._ListceMastModel = new List<ViewceMastModel>();

            if (Docno != null)
            {
                @class._ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == Docno).FirstOrDefault();
                @class._ViewceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == Docno).FirstOrDefault();

            }
            return View(@class);
        }
    }
}