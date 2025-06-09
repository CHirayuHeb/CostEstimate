using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CostEstimate.Models.Approval;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.New;
using CostEstimate.Models.Table.HRMS;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MK;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CostEstimate.Controllers.ApprovalMold
{
    public class ApprovalMoldController : Controller
    {
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public ApprovalMoldController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smEmpCodeApprove == EmpCode && x.smStep != 7).OrderBy(x => x.smStep).ThenBy(x => x.smIssueDate).ToList();

            @class._ListViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfEmpCodeApprove == EmpCode && x.mfStep != 6).OrderBy(x => x.mfStep).ThenBy(x => x.mfIssueDate).ToList();

           // List<ViewceMastFlowApprove> _ViewceMastFlowApprove = _MK._ViewceMastFlowApprove.OrderBy(x => x.mfStep).Distinct().ToList();
            List<ViewceMastFlowApprove> _ViewceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "2").OrderBy(x => x.mfStep).Distinct().ToList();
            SelectList formStatus = new SelectList(_ViewceMastFlowApprove.Select(s => s.mfSubject).Distinct());
            ViewBag.vbformStatus = formStatus;

            if (@class._ViewSearchData != null)
            {
                if (@class._ViewSearchData.v_status != null)
                {
                    //int smstep = _MK._ViewceMastFlowApprove.Where(x => x.mfSubject.Contains(@class._ViewSearchData.v_status)).Select(x => x.mfStep).FirstOrDefault();
                    //@class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smStep == smstep).OrderBy(x => x.smStep).ThenBy(x => x.smIssueDate).ToList();

                    int vsmstep = _MK._ViewceMastFlowApprove.Where(x => x.mfSubject.Contains(@class._ViewSearchData.v_status) && x.mfFlowNo == "2").Select(x => x.mfStep).FirstOrDefault();
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfStep == vsmstep && x.mfFlowNo == 2).OrderBy(x => x.mfStep).ThenBy(x => x.mfIssueDate).ToList();


                }
            }

            return View(@class);
        }
    }
}