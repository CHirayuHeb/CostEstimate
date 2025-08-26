using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.MyRequest;
using CostEstimate.Models.Table.HRMS;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MK;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CostEstimate.Controllers.MyRequest
{
    public class MyRequestController : Controller
    {
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public MyRequestController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
        public IActionResult Index(Class @class)
        {
            string EmpCode = User.Claims.FirstOrDefault(s => s.Type == "EmpCode").Value?.ToString();
            @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smEmpCodeRequest == EmpCode).OrderBy(x => x.smStep).ThenBy(x => x.smIssueDate).ToList();

            List<ViewceMastFlowApprove> _ViewceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "1").OrderBy(x => x.mfStep).Distinct().ToList();
            SelectList formStatus = new SelectList(_ViewceMastFlowApprove.Select(s => s.mfSubject).Distinct());
            ViewBag.vbformStatus = formStatus;

            if(@class._ViewSearchData != null)
            {
                if(@class._ViewSearchData.v_status != null)
                {
                    int smstep = _MK._ViewceMastFlowApprove.Where(x => x.mfSubject.Contains(@class._ViewSearchData.v_status)).Select(x => x.mfStep).FirstOrDefault();
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smStep == smstep).OrderBy(x => x.smStep).ThenBy(x => x.smIssueDate).ToList();

                }
            }

            //@classs._ListsvsServiceRequest = _IT.svsServiceRequest.Where(x => x.srRequestBy == EmpCode).OrderByDescending(x => x.srRequestDate).ToList();
            // @classs._ListsvsServiceRequest = _IT.svsServiceRequest.Where(x => x.srRequestBy == EmpCode).OrderByDescending(x => x.srRequestDate).ToList();

            //public ViewSearchMyReq _ViewSearchMyReq { get; set; }
            return View(@class);
        }

    }
}