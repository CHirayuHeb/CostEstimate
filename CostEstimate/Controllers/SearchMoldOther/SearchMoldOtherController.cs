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
            return View(@class);
        }
        public IActionResult btnExportExcel(Class @class)
        {
            string slipMat = DateTime.Now.ToString("yyyyMMdd:HHmmss");
            string TempPath = Path.GetTempFileName();
            string fileName = "Export(" + slipMat + ").xlsx";

            //@class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.ToList();
            @class._ListViewceMastModifyRequest = _MK._ViewceMastModifyRequest.OrderByDescending(x => x.mfCENo).ToList();

            if (@class._ViewSearchData != null)
            {
                if (@class._ViewSearchData.v_OrderNo != null && @class._ViewSearchData.v_OrderNo != "") //mfRefNo 
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfRefNo.ToUpper().Contains(@class._ViewSearchData.v_OrderNo.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_DocumentNo != null && @class._ViewSearchData.v_DocumentNo != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfCENo.ToUpper().Contains(@class._ViewSearchData.v_DocumentNo.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_status != null)
                {
                    //int smstep = _MK._ViewceMastFlowApprove.Where(x => x.mfSubject.Contains(@class._ViewSearchData.v_status)).Select(x => x.mfStep).FirstOrDefault();
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfStatus.Contains(@class._ViewSearchData.v_status)).ToList();
                    // @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smStep == smstep).OrderBy(x => x.smStep).ThenBy(x => x.smIssueDate).ToList();

                }
                if (@class._ViewSearchData.v_LotNo != null && @class._ViewSearchData.v_LotNo != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfLotNo.ToUpper().Contains(@class._ViewSearchData.v_LotNo.ToUpper())).ToList();
                }
                //if (@class._ViewSearchData.v_MoldNo != null && @class._ViewSearchData.v_MoldNo != "")
                //{
                //    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfMoldNoOrMoldName.ToUpper().Contains(@class._ViewSearchData.v_MoldNo.ToUpper())).ToList();
                //}
                if (@class._ViewSearchData.v_MoldMass != null && @class._ViewSearchData.v_MoldMass != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfMoldMass.ToUpper().Contains(@class._ViewSearchData.v_MoldMass.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_CusName != null && @class._ViewSearchData.v_CusName != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfCustomerName.ToUpper().Contains(@class._ViewSearchData.v_CusName.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_MoldName != null && @class._ViewSearchData.v_MoldName != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfMoldNoOrMoldName.ToUpper().Contains(@class._ViewSearchData.v_MoldName.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_ModelName != null && @class._ViewSearchData.v_ModelName != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfModelName.ToUpper().Contains(@class._ViewSearchData.v_ModelName.ToUpper())).ToList();
                }
                if (@class._ViewSearchData.v_CavityNo != null && @class._ViewSearchData.v_CavityNo != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfCavityNo == int.Parse(@class._ViewSearchData.v_CavityNo)).ToList();
                }
                if (@class._ViewSearchData.v_Function != null && @class._ViewSearchData.v_Function != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfFunction.ToUpper().Contains(@class._ViewSearchData.v_Function.ToUpper())).ToList();
                }
                //if (@class._ViewSearchData.v_DevelopmentStage != null && @class._ViewSearchData.v_DevelopmentStage != "")
                //{
                //    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDevelopmentStage.ToUpper().Contains(@class._ViewSearchData.v_DevelopmentStage.ToUpper())).ToList();
                //}

                ////Material
                //if (@class._ViewSearchData.v_MaterialOutDateFrom != null && @class._ViewSearchData.v_MaterialOutDateFrom != "")
                //{
                //    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) >= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateFrom)).ToList();
                //}
                //if (@class._ViewSearchData.v_MaterialOutDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                //{
                //    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) <= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateTo)).ToList();
                //}

                //DaliverryDate
                //if (@class._ViewSearchData.v_DaliverryDateFrom != null && @class._ViewSearchData.v_DaliverryDateFrom != "")
                //{
                //    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) >= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateFrom)).ToList();
                //}
                //if (@class._ViewSearchData.v_DaliverryDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                //{
                //    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) <= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateTo)).ToList();
                //}

                //date issue
                //date resuest
                if (@class._ViewSearchData.v_DateIssueFrom != null && @class._ViewSearchData.v_DateIssueFrom != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => DateTime.Parse(x.mfIssueDate) >= DateTime.Parse(@class._ViewSearchData.v_DateIssueFrom)).ToList();
                }
                if (@class._ViewSearchData.v_DateIssueTo != null && @class._ViewSearchData.v_DateIssueTo != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => DateTime.Parse(x.mfIssueDate) <= DateTime.Parse(@class._ViewSearchData.v_DateIssueTo)).ToList();
                }

                if (@class._ViewSearchData.v_TypeofCavity != null && @class._ViewSearchData.v_TypeofCavity != "")
                {
                    //@class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smTypeCavity.Contains(@class._ViewSearchData.v_TypeofCavity)).ToList();

                    //@class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smTypeCavity != null && x.smTypeCavity.IndexOf(@class._ViewSearchData.v_TypeofCavity, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfTypeCavity == @class._ViewSearchData.v_TypeofCavity).ToList();

                }

                if (@class._ViewSearchData.v_Revision != null && @class._ViewSearchData.v_Revision != "")
                {
                    @class._ListViewceMastModifyRequest = @class._ListViewceMastModifyRequest.Where(x => x.mfRevision.Contains(@class._ViewSearchData.v_Revision)).ToList();
                }
            }
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                int startRow = 1;

                // เขียนหัวตาราง (ชื่อ property)
                var properties = typeof(ViewceMastModifyRequest).GetProperties();
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[startRow, col + 1].Value = properties[col].Name;
                }

                // เขียนข้อมูลจาก list
                for (int row = 0; row < @class._ListViewceMastModifyRequest.Count; row++)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(@class._ListViewceMastModifyRequest[row]);
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