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
namespace CostEstimate.Controllers.MyRequestMoldOther
{
    public class MyRequestMoldOtherController : Controller
    {
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;

        public MyRequestMoldOtherController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            string user_id = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            @class._ListViewceMastMoldOtherRequest = new List<ViewceMastMoldOtherRequest>();
            @class._ListViewceMastWorkingTimeRequest = new List<ViewceMastWorkingTimeRequest>();
            @class._ListViewceMastWorkingTimeRequestDetail = new List<ViewceMastWorkingTimeRequestDetail>();
            @class._ListViewceMastMaterialRequest = new List<ViewceMastMaterialRequest>();
            @class._ListViewceMastToolGRRequest = new List<ViewceMastToolGRRequest>();
            @class._ListViewceMastInforSpacMoldRequest = new List<ViewceMastInforSpacMoldRequest>();

            @class._ListViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrEmpCodeRequest == user_id && x.mrStep != 8).ToList();

            @class._ListViewceMastWorkingTimeRequest = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrEmpCodeRequest == user_id && x.wrStep != 4).ToList();
            @class._ListViewceMastWorkingTimeRequestDetail = getceMastWorkingTimeRequestDetail(@class._ListViewceMastWorkingTimeRequest); //call get list 

            @class._ListViewceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrEmpCodeRequest == user_id && x.mrStep != 4).ToList();
            @class._ListViewceMastMaterialRequestDetail = getceMastMaterialRequestDetail(@class._ListViewceMastMaterialRequest);

            @class._ListViewceMastToolGRRequest = _MK._ViewceMastToolGRRequest.Where(x => x.trEmpCodeRequest == user_id && x.trStep != 4).ToList();
            @class._ListViewceMastToolGRRequestDetail = getceMastToolGRRequestDetail(@class._ListViewceMastToolGRRequest);

            @class._ListViewceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irEmpCodeRequest == user_id && x.irStep != 4).ToList();
            @class._ListViewceMastInforSpacMoldRequestDetail = getceMastInforSpacMoldRequestDetail(@class._ListViewceMastInforSpacMoldRequest);




            return View(@class);

        }

        public List<ViewceMastWorkingTimeRequestDetail> getceMastWorkingTimeRequestDetail(List<ViewceMastWorkingTimeRequest> _ListViewceMastWorkingTimeRequest)
        {
            List<ViewceMastWorkingTimeRequestDetail> _ListViewceMastWorkingTimeRequestDetail = new List<ViewceMastWorkingTimeRequestDetail>();

            for (int i = 0; i < _ListViewceMastWorkingTimeRequest.Count(); i++)
            {

                var _ceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == _ListViewceMastWorkingTimeRequest[i].wrDocumentNo).FirstOrDefault();

                _ListViewceMastWorkingTimeRequestDetail.Add(new ViewceMastWorkingTimeRequestDetail
                {
                    wrCustomerName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrCustomerName : "",
                    wrFunction = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrFunction : "",
                    wrModelName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrModelName : "",
                    wrRev = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrRevision : 0,
                    wrDocumentNo = _ListViewceMastWorkingTimeRequest[i].wrDocumentNo,
                    wrDocumentNoSub = _ListViewceMastWorkingTimeRequest[i].wrDocumentNoSub,
                    wrIssueDate = _ListViewceMastWorkingTimeRequest[i].wrIssueDate,
                    wrStep = _ListViewceMastWorkingTimeRequest[i].wrStep,
                    wrStatus = _ListViewceMastWorkingTimeRequest[i].wrStatus,
                    wrEmpCodeRequest = _ListViewceMastWorkingTimeRequest[i].wrEmpCodeRequest,
                    wrNameRequest = _ListViewceMastWorkingTimeRequest[i].wrNameRequest,
                    wrEmpCodeApprove = _ListViewceMastWorkingTimeRequest[i].wrEmpCodeApprove,
                    wrNameApprove = _ListViewceMastWorkingTimeRequest[i].wrNameApprove,
                    wrFlowNo = _ListViewceMastWorkingTimeRequest[i].wrFlowNo,
                });
            }
            return _ListViewceMastWorkingTimeRequestDetail;
        }

        public List<ViewceMastMaterialRequestDetail> getceMastMaterialRequestDetail(List<ViewceMastMaterialRequest> _ListView)
        {
            List<ViewceMastMaterialRequestDetail> _ListViewceMastMaterialRequestDetail = new List<ViewceMastMaterialRequestDetail>();

            for (int i = 0; i < _ListView.Count(); i++)
            {

                var _ceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == _ListView[i].mrDocumentNo).FirstOrDefault();

                _ListViewceMastMaterialRequestDetail.Add(new ViewceMastMaterialRequestDetail
                {
                    mrCustomerName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrCustomerName : "",
                    mrFunction = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrFunction : "",
                    mrModelName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrModelName : "",
                    mrRev = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrRevision : 0,
                    mrDocumentNo = _ListView[i].mrDocumentNo,
                    mrDocumentNoSub = _ListView[i].mrDocumentNoSub,
                    mrIssueDate = _ListView[i].mrIssueDate,
                    mrStep = _ListView[i].mrStep,
                    mrStatus = _ListView[i].mrStatus,
                    mrEmpCodeRequest = _ListView[i].mrEmpCodeRequest,
                    mrNameRequest = _ListView[i].mrNameRequest,
                    mrEmpCodeApprove = _ListView[i].mrEmpCodeApprove,
                    mrNameApprove = _ListView[i].mrNameApprove,
                    mrFlowNo = _ListView[i].mrFlowNo,
                });
            }
            return _ListViewceMastMaterialRequestDetail;
        }

        public List<ViewceMastToolGRRequestDetail> getceMastToolGRRequestDetail(List<ViewceMastToolGRRequest> _ListView)
        {
            List<ViewceMastToolGRRequestDetail> _ListViewceMastToolGRRequestDetail = new List<ViewceMastToolGRRequestDetail>();

            for (int i = 0; i < _ListView.Count(); i++)
            {

                var _ceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == _ListView[i].trDocumentNo).FirstOrDefault();

                _ListViewceMastToolGRRequestDetail.Add(new ViewceMastToolGRRequestDetail
                {
                    trCustomerName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrCustomerName : "",
                    trFunction = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrFunction : "",
                    trModelName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrModelName : "",
                    trRev = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrRevision : 0,
                    trDocumentNo = _ListView[i].trDocumentNo,
                    trDocumentNoSub = _ListView[i].trDocumentNoSub,
                    trIssueDate = _ListView[i].trIssueDate,
                    trStep = _ListView[i].trStep,
                    trStatus = _ListView[i].trStatus,
                    trEmpCodeRequest = _ListView[i].trEmpCodeRequest,
                    trNameRequest = _ListView[i].trNameRequest,
                    trEmpCodeApprove = _ListView[i].trEmpCodeApprove,
                    trNameApprove = _ListView[i].trNameApprove,
                    trFlowNo = _ListView[i].trFlowNo,
                });
            }
            return _ListViewceMastToolGRRequestDetail;
        }

        public List<ViewceMastInforSpacMoldRequestDetail> getceMastInforSpacMoldRequestDetail(List<ViewceMastInforSpacMoldRequest> _ListView)
        {
            List<ViewceMastInforSpacMoldRequestDetail> _ListViewceMastInforSpacMoldRequestDetail = new List<ViewceMastInforSpacMoldRequestDetail>();

            for (int i = 0; i < _ListView.Count(); i++)
            {

                var _ceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == _ListView[i].irDocumentNo).FirstOrDefault();

                _ListViewceMastInforSpacMoldRequestDetail.Add(new ViewceMastInforSpacMoldRequestDetail
                {
                    irCustomerName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrCustomerName : "",
                    irFunction = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrFunction : "",
                    irModelName = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrModelName : "",
                    irRev = _ceMastMoldOtherRequest != null ? _ceMastMoldOtherRequest.mrRevision : 0,
                    irDocumentNo = _ListView[i].irDocumentNo,
                    irDocumentNoSub = _ListView[i].irDocumentNoSub,
                    irIssueDate = _ListView[i].irIssueDate,
                    irStep = _ListView[i].irStep,
                    irStatus = _ListView[i].irStatus,
                    irEmpCodeRequest = _ListView[i].irEmpCodeRequest,
                    irNameRequest = _ListView[i].irNameRequest,
                    irEmpCodeApprove = _ListView[i].irEmpCodeApprove,
                    irNameApprove = _ListView[i].irNameApprove,
                    irFlowNo = _ListView[i].irFlowNo,
                });
            }
            return _ListViewceMastInforSpacMoldRequestDetail;
        }


    }
}