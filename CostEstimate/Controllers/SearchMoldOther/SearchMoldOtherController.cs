using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CostEstimate.Models.Approval;
using CostEstimate.Models.Canvas;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.Table.LAMP;
//export excel
using ClosedXML.Excel;
using System.Data;
using System.IO;
using CostEstimate.Models.Table.MK;

using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CostEstimate.Controllers.SearchMoldOther
{
    public class SearchMoldOtherController : Controller
    {
        public string[] Header_FileExport = { "No.", "Reference No.", "Lot No.", "Customer Name", "Rev.No​ ", "Model Name", "Function", "Mold No. ", "Mold Name", "Cavity No.", "Development Stage", "Material Out/Date​", "Delivery/Date​", "Waiting Approve", "Status​ " };
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;

        public SearchMoldOtherController(LAMP lamp, HRMS hrms, IT it, MK mk, CacheSettingController cacheController, FunctionsController callfunction)
        {
            _LAMP = lamp;
            _HRMS = hrms;
            _IT = it;
            _MK = mk;
            _Cache = cacheController;
            _callFunc = callfunction;
        }


        [Authorize("Checked")]
        public IActionResult Index(Class @class)
        {
            List<ViewceMastMoldOtherRequest> _ListViewceMastMoldOtherRequest = new List<ViewceMastMoldOtherRequest>();
            @class._ListViewceMastMoldOtherRequest = new List<ViewceMastMoldOtherRequest>();
            @class._ListViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.OrderByDescending(x => x.mrDocmentNo).ToList();

            List<ViewceMastFlowApprove> _ViewceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "3").OrderBy(x => x.mfStep).Distinct().ToList();
            SelectList formStatus = new SelectList(_ViewceMastFlowApprove.Select(s => s.mfSubject).Distinct());
            ViewBag.vbformStatus = formStatus;


            if (@class._ListViewceMastMoldOtherRequest.Where(x => x.mrStep == 2).ToList().Count() > 0)
            {
                string status = UpdateStatusDoc(@class._ListViewceMastMoldOtherRequest.Where(x => x.mrStep == 2).ToList());
            }

            _ListViewceMastMoldOtherRequest = SearchList(@class._ListViewceMastMoldOtherRequest, @class);
            @class._ListViewceMastMoldOtherRequest = _ListViewceMastMoldOtherRequest;

            return View(@class);
        }


        public List<ViewceMastMoldOtherRequest> SearchList(List<ViewceMastMoldOtherRequest> _ViewceMastMoldOtherRequest, Class @class)
        {
            @class._ListViewceMastMoldOtherRequest.ForEach(item =>
            {
                if (item.mrIssueDate != null && item.mrIssueDate != "")
                {
                    var day = item.mrIssueDate.Substring(0, 2);
                    var month = item.mrIssueDate.Substring(3, 2);
                    var year = item.mrIssueDate.Substring(6, 4);

                    item.mrIssueDate = $"{year}/{month}/{day}";
                }
                else
                {
                    item.mrIssueDate = "";
                }


            });
            if (@class._ViewSearchData != null)
            {
                if (@class._ViewSearchData.v_DocumentNo != null && @class._ViewSearchData.v_DocumentNo != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo.ToUpper().Contains(@class._ViewSearchData.v_DocumentNo.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_CusName != null && @class._ViewSearchData.v_CusName != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrCustomerName.ToUpper().Contains(@class._ViewSearchData.v_CusName.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_Function != null && @class._ViewSearchData.v_Function != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrFunction.ToUpper().Contains(@class._ViewSearchData.v_Function.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_Revision != null && @class._ViewSearchData.v_Revision != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrRevision == int.Parse(@class._ViewSearchData.v_Revision)).ToList();
                }
                if (@class._ViewSearchData.v_ModelName != null && @class._ViewSearchData.v_ModelName != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrModelName.ToUpper().Contains(@class._ViewSearchData.v_ModelName.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_Event != null && @class._ViewSearchData.v_Event != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrEvent.ToUpper().Contains(@class._ViewSearchData.v_Event.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_MoldGo != null && @class._ViewSearchData.v_MoldGo != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrMoldGo == @class._ViewSearchData.v_MoldGo.ToUpper()).ToList();
                }
                if (@class._ViewSearchData.v_Try1 != null && @class._ViewSearchData.v_Try1 != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrTry1 == @class._ViewSearchData.v_Try1.ToUpper()).ToList();
                }
                if (@class._ViewSearchData.v_MoldMass != null && @class._ViewSearchData.v_MoldMass != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrMoldMass == @class._ViewSearchData.v_MoldMass.ToUpper()).ToList();
                }
                if (@class._ViewSearchData.v_Type != null && @class._ViewSearchData.v_Type != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrType.ToUpper().Contains(@class._ViewSearchData.v_Type.ToUpper())).ToList();
                }

                if (@class._ViewSearchData.v_DateIssueFrom != null && @class._ViewSearchData.v_DateIssueFrom != "")
                {

                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => !string.IsNullOrEmpty(x.mrIssueDate)
             && !string.IsNullOrEmpty(@class._ViewSearchData.v_DateIssueFrom)
             && DateTime.Parse(x.mrIssueDate) >= DateTime.Parse(@class._ViewSearchData.v_DateIssueFrom)).ToList();

                   // @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => DateTime.Parse(x.mrIssueDate) >= DateTime.Parse(@class._ViewSearchData.v_DateIssueFrom)).ToList();
                }
                if (@class._ViewSearchData.v_DateIssueTo != null && @class._ViewSearchData.v_DateIssueTo != "")
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => !string.IsNullOrEmpty(x.mrIssueDate)
&& !string.IsNullOrEmpty(@class._ViewSearchData.v_DateIssueTo)
&& DateTime.Parse(x.mrIssueDate) <= DateTime.Parse(@class._ViewSearchData.v_DateIssueTo)).ToList();

                    //@class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => DateTime.Parse(x.mrIssueDate) <= DateTime.Parse(@class._ViewSearchData.v_DateIssueTo)).ToList();
                }
                if (@class._ViewSearchData.v_status != null)
                {
                    @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrStatus.Contains(@class._ViewSearchData.v_status)).ToList();

                }
            }

            return @class._ListViewceMastMoldOtherRequest;
        }


        public string UpdateStatusDoc(List<ViewceMastMoldOtherRequest> _ViewceMastMoldOtherRequests)
        {
            string vstatus = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "3" && x.mfStep == 3).Select(x => x.mfSubject).FirstOrDefault();
            for (int i = 0; i < _ViewceMastMoldOtherRequests.Count(); i++)
            {
                string id = _ViewceMastMoldOtherRequests[i].mrDocmentNo;
                int vstepWK = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == id).Select(x => x.wrStep).FirstOrDefault();
                int vstepMT = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == id).Select(x => x.mrStep).FirstOrDefault();
                int vstepTGR = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == id).Select(x => x.trStep).FirstOrDefault();
                int vstepSP = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == id).Select(x => x.irStep).FirstOrDefault();
                if (vstepWK == 4 && vstepMT == 4 && vstepTGR == 4 && vstepSP == 4)
                {
                    var _ceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == id).FirstOrDefault();
                    _ceMastMoldOtherRequest.mrStep = 3;
                    _ceMastMoldOtherRequest.mrStatus = vstatus;
                    _MK._ViewceMastMoldOtherRequest.Update(_ceMastMoldOtherRequest);
                    _MK.SaveChanges();
                }
            }





            return "sucess";
        }



        [HttpPost]
        public IActionResult btnExportExcel(Class @class)
        {
            List<ViewceMastMoldOtherRequest> _ListViewceMastMoldOtherRequest = new List<ViewceMastMoldOtherRequest>();
            string slipMat = DateTime.Now.ToString("yyyyMMdd:HHmmss");
            string TempPath = Path.GetTempFileName();
            string fileName = "Export(" + slipMat + ").xlsx";

            @class._ListViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.OrderByDescending(x => x.mrDocmentNo).ToList();

            _ListViewceMastMoldOtherRequest = SearchList(@class._ListViewceMastMoldOtherRequest, @class);
            @class._ListViewceMastMoldOtherRequest = _ListViewceMastMoldOtherRequest;

            //@class._ListViewceMastMoldOtherRequest.ForEach(item =>
            //{
            //    if (item.mrIssueDate != null && item.mrIssueDate != "")
            //    {
            //        var day = item.mrIssueDate.Substring(0, 2);
            //        var month = item.mrIssueDate.Substring(3, 2);
            //        var year = item.mrIssueDate.Substring(6, 4);

            //        item.mrIssueDate = $"{year}/{month}/{day}";
            //    }

            //});



            //if (@class._ViewSearchData != null)
            //{
            //    if (@class._ViewSearchData.v_DocumentNo != null && @class._ViewSearchData.v_DocumentNo != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo.ToUpper().Contains(@class._ViewSearchData.v_DocumentNo.ToUpper())).ToList();
            //    }
            //    if (@class._ViewSearchData.v_CusName != null && @class._ViewSearchData.v_CusName != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrCustomerName.ToUpper().Contains(@class._ViewSearchData.v_CusName.ToUpper())).ToList();
            //    }
            //    if (@class._ViewSearchData.v_Function != null && @class._ViewSearchData.v_Function != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrFunction.ToUpper().Contains(@class._ViewSearchData.v_Function.ToUpper())).ToList();
            //    }
            //    if (@class._ViewSearchData.v_Revision != null && @class._ViewSearchData.v_Revision != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrRevision == int.Parse(@class._ViewSearchData.v_Revision)).ToList();
            //    }
            //    if (@class._ViewSearchData.v_ModelName != null && @class._ViewSearchData.v_ModelName != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrModelName.ToUpper().Contains(@class._ViewSearchData.v_ModelName.ToUpper())).ToList();
            //    }
            //    if (@class._ViewSearchData.v_Event != null && @class._ViewSearchData.v_Event != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrEvent.ToUpper().Contains(@class._ViewSearchData.v_Event.ToUpper())).ToList();
            //    }
            //    if (@class._ViewSearchData.v_MoldGo != null && @class._ViewSearchData.v_MoldGo != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrMoldGo == @class._ViewSearchData.v_MoldGo.ToUpper()).ToList();
            //    }
            //    if (@class._ViewSearchData.v_Try1 != null && @class._ViewSearchData.v_Try1 != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrTry1 == @class._ViewSearchData.v_Try1.ToUpper()).ToList();
            //    }
            //    if (@class._ViewSearchData.v_MoldMass != null && @class._ViewSearchData.v_MoldMass != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrMoldMass == @class._ViewSearchData.v_MoldMass.ToUpper()).ToList();
            //    }
            //    if (@class._ViewSearchData.v_Type != null && @class._ViewSearchData.v_Type != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrType.ToUpper().Contains(@class._ViewSearchData.v_Type.ToUpper())).ToList();
            //    }

            //    if (@class._ViewSearchData.v_DateIssueFrom != null && @class._ViewSearchData.v_DateIssueFrom != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => DateTime.Parse(x.mrIssueDate) >= DateTime.Parse(@class._ViewSearchData.v_DateIssueFrom)).ToList();
            //    }
            //    if (@class._ViewSearchData.v_DateIssueTo != null && @class._ViewSearchData.v_DateIssueTo != "")
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => DateTime.Parse(x.mrIssueDate) <= DateTime.Parse(@class._ViewSearchData.v_DateIssueTo)).ToList();
            //    }
            //    if (@class._ViewSearchData.v_status != null)
            //    {
            //        @class._ListViewceMastMoldOtherRequest = @class._ListViewceMastMoldOtherRequest.Where(x => x.mrStatus.Contains(@class._ViewSearchData.v_status)).ToList();

            //    }
            //}
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                int startRow = 1;

                // เขียนหัวตาราง (ชื่อ property)
                //var properties = typeof(ViewceMastModifyRequest).GetProperties();
                //for (int col = 0; col < properties.Length; col++)
                //{
                //    worksheet.Cells[startRow, col + 1].Value = properties[col].Name;
                //}

                // เขียนข้อมูลจาก list
                //for (int row = 0; row < @class._ListViewceMastModifyRequest.Count; row++)
                //{
                //    for (int col = 0; col < properties.Length; col++)
                //    {
                //        var value = properties[col].GetValue(@class._ListViewceMastModifyRequest[row]);
                //        worksheet.Cells[row + 2, col + 1].Value = value;
                //    }
                //}


                var properties = typeof(ViewceMastMoldOtherRequest).GetProperties();
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[startRow, col + 1].Value = properties[col].Name;
                }

                for (int row = 0; row < @class._ListViewceMastMoldOtherRequest.Count; row++)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(@class._ListViewceMastMoldOtherRequest[row]);
                        worksheet.Cells[row + 2, col + 1].Value = value;
                    }
                }



                // Auto fit column width
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Export เป็น stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // ส่งไฟล์ออก (ใช้ใน ASP.NET Core)
                // string fileName = $"Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

        }

    }
}