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

namespace CostEstimate.Controllers.Search
{
    public class SearchController : Controller
    {

        public string[] Header_FileExport = { "No.", "Document No.", "Lot No.", "Customer Name", "Rev.No​ ", "Model Name", "Function", "Mold No. ", "Mold Name", "Cavity No.", "Development Stage", "Material Out/Date​", "Delivery/Date​", "Waiting Approve", "Status​ " };
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;

        public SearchController(LAMP lamp, HRMS hrms, IT it, MK mk, CacheSettingController cacheController, FunctionsController callfunction)
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
            List<ViewceMastFlowApprove> _ViewceMastFlowApprove = _MK._ViewceMastFlowApprove.OrderBy(x => x.mfStep).Distinct().ToList();
            SelectList formStatus = new SelectList(_ViewceMastFlowApprove.Select(s => s.mfSubject).Distinct());
            ViewBag.vbformStatus = formStatus;


            if (@class._ViewSearchData != null)
            {
                @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.OrderByDescending(x=>x.smIssueDate).ToList();
                if (@class._ViewSearchData.v_DocumentNo != null && @class._ViewSearchData.v_DocumentNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDocumentNo.Contains(@class._ViewSearchData.v_DocumentNo)).ToList();
                }
                if (@class._ViewSearchData.v_status != null)
                {
                    //int smstep = _MK._ViewceMastFlowApprove.Where(x => x.mfSubject.Contains(@class._ViewSearchData.v_status)).Select(x => x.mfStep).FirstOrDefault();
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smStatus.Contains(@class._ViewSearchData.v_status)).ToList();
                   // @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smStep == smstep).OrderBy(x => x.smStep).ThenBy(x => x.smIssueDate).ToList();

                }
                if (@class._ViewSearchData.v_LotNo != null && @class._ViewSearchData.v_LotNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smLotNo.Contains(@class._ViewSearchData.v_LotNo)).ToList();
                }
                if (@class._ViewSearchData.v_MoldNo != null && @class._ViewSearchData.v_MoldNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smMoldNo.Contains(@class._ViewSearchData.v_MoldNo)).ToList();
                }
                if (@class._ViewSearchData.v_CusName != null && @class._ViewSearchData.v_CusName != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCustomerName.Contains(@class._ViewSearchData.v_CusName)).ToList();
                }
                if (@class._ViewSearchData.v_ModelName != null && @class._ViewSearchData.v_ModelName != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smModelName.Contains(@class._ViewSearchData.v_ModelName)).ToList();
                }
                if (@class._ViewSearchData.v_CavityNo != null && @class._ViewSearchData.v_CavityNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCavityNo == int.Parse(@class._ViewSearchData.v_CavityNo)).ToList();
                }
                if (@class._ViewSearchData.v_Function != null && @class._ViewSearchData.v_Function != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smFunction.Contains(@class._ViewSearchData.v_Function)).ToList();
                }
                if (@class._ViewSearchData.v_DevelopmentStage != null && @class._ViewSearchData.v_DevelopmentStage != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDevelopmentStage.Contains(@class._ViewSearchData.v_DevelopmentStage)).ToList();
                }

                //Material
                if (@class._ViewSearchData.v_MaterialOutDateFrom != null && @class._ViewSearchData.v_MaterialOutDateFrom != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) >= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateFrom)).ToList();
                }
                if (@class._ViewSearchData.v_MaterialOutDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) <= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateTo)).ToList();
                }

                //DaliverryDate
                if (@class._ViewSearchData.v_DaliverryDateFrom != null && @class._ViewSearchData.v_DaliverryDateFrom != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) >= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateFrom)).ToList();
                }
                if (@class._ViewSearchData.v_DaliverryDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) <= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateTo)).ToList();
                }


            }

            return View(@class);
        }

        
        public IActionResult btnExportExcel(Class @class)
        {
            string slipMat = DateTime.Now.ToString("yyyyMMdd:HHmmss");
            string TempPath = Path.GetTempFileName();
            string fileName = "Export(" + slipMat + ").xlsx";

            @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.ToList();
            if (@class._ViewSearchData != null)
            {
                @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.ToList();
                if (@class._ViewSearchData.v_DocumentNo != null && @class._ViewSearchData.v_DocumentNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDocumentNo.Contains(@class._ViewSearchData.v_DocumentNo)).ToList();
                }
                if (@class._ViewSearchData.v_LotNo != null && @class._ViewSearchData.v_LotNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smLotNo.Contains(@class._ViewSearchData.v_LotNo)).ToList();
                }
                if (@class._ViewSearchData.v_MoldNo != null && @class._ViewSearchData.v_MoldNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smMoldNo.Contains(@class._ViewSearchData.v_MoldNo)).ToList();
                }
                if (@class._ViewSearchData.v_CusName != null && @class._ViewSearchData.v_CusName != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCustomerName.Contains(@class._ViewSearchData.v_CusName)).ToList();
                }
                if (@class._ViewSearchData.v_ModelName != null && @class._ViewSearchData.v_ModelName != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smModelName.Contains(@class._ViewSearchData.v_ModelName)).ToList();
                }
                if (@class._ViewSearchData.v_CavityNo != null && @class._ViewSearchData.v_CavityNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCavityNo == int.Parse(@class._ViewSearchData.v_CavityNo)).ToList();
                }
                if (@class._ViewSearchData.v_Function != null && @class._ViewSearchData.v_Function != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smFunction.Contains(@class._ViewSearchData.v_Function)).ToList();
                }
                if (@class._ViewSearchData.v_DevelopmentStage != null && @class._ViewSearchData.v_DevelopmentStage != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDevelopmentStage.Contains(@class._ViewSearchData.v_DevelopmentStage)).ToList();
                }

                //Material
                if (@class._ViewSearchData.v_MaterialOutDateFrom != null && @class._ViewSearchData.v_MaterialOutDateFrom != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) >= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateFrom)).ToList();
                }
                if (@class._ViewSearchData.v_MaterialOutDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) <= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateTo)).ToList();
                }

                //DaliverryDate
                if (@class._ViewSearchData.v_DaliverryDateFrom != null && @class._ViewSearchData.v_DaliverryDateFrom != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) >= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateFrom)).ToList();
                }
                if (@class._ViewSearchData.v_DaliverryDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) <= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateTo)).ToList();
                }


            }


            using (var package = new ExcelPackage())
            {
                var addWorkSheet = package.Workbook.Worksheets.Add("Sheet1");
                int onRow = 1;
                int mainCol = 1;


                foreach (string name in Header_FileExport)
                {
                    addWorkSheet.Cells[1, mainCol].Value = name;
                    mainCol++;
                }
                foreach (var item in @class._ListceMastSubMakerRequest)
                {
                    onRow++;
                    addWorkSheet.Cells[onRow, 1].Value = onRow - 1;
                    addWorkSheet.Cells[onRow, 2].Value = item.smDocumentNo;
                    addWorkSheet.Cells[onRow, 3].Value = item.smLotNo;
                    addWorkSheet.Cells[onRow, 4].Value = item.smCustomerName;
                    addWorkSheet.Cells[onRow, 5].Value = item.smRevision;
                    addWorkSheet.Cells[onRow, 6].Value = item.smModelName;
                    addWorkSheet.Cells[onRow, 7].Value = item.smFunction;
                    addWorkSheet.Cells[onRow, 8].Value = item.smMoldNo;
                    addWorkSheet.Cells[onRow, 9].Value = item.smMoldName;
                    addWorkSheet.Cells[onRow, 10].Value = item.smCavityNo;
                    addWorkSheet.Cells[onRow, 11].Value = item.smDevelopmentStage;
                    addWorkSheet.Cells[onRow, 12].Value = item.smMatOutDate + " : " + item.smMatOutTime;
                    addWorkSheet.Cells[onRow, 13].Value = item.smDeliveryDate + " : " + item.smDeliveryTime;
                    addWorkSheet.Cells[onRow, 14].Value = item.smEmpCodeApprove + " : " + item.smNameApprove;
                    addWorkSheet.Cells[onRow, 15].Value = item.smStatus;
                }


                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpPost]
        public JsonResult ExportExcel(Class @class)
        {
            string slipMat = DateTime.Now.ToString("yyyyMMdd");
            string TempPath = Path.GetTempFileName();
            string fileName = "Export (" + slipMat + ").xlsx";

            if (@class._ViewSearchData != null)
            {
                @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.ToList();
                if (@class._ViewSearchData.v_LotNo != null && @class._ViewSearchData.v_LotNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smLotNo.Contains(@class._ViewSearchData.v_LotNo)).ToList();
                }
                if (@class._ViewSearchData.v_MoldNo != null && @class._ViewSearchData.v_MoldNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smMoldNo.Contains(@class._ViewSearchData.v_MoldNo)).ToList();
                }
                if (@class._ViewSearchData.v_CusName != null && @class._ViewSearchData.v_CusName != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCustomerName.Contains(@class._ViewSearchData.v_CusName)).ToList();
                }
                if (@class._ViewSearchData.v_ModelName != null && @class._ViewSearchData.v_ModelName != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smModelName.Contains(@class._ViewSearchData.v_ModelName)).ToList();
                }
                if (@class._ViewSearchData.v_CavityNo != null && @class._ViewSearchData.v_CavityNo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCavityNo == int.Parse(@class._ViewSearchData.v_CavityNo)).ToList();
                }
                if (@class._ViewSearchData.v_Function != null && @class._ViewSearchData.v_Function != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smFunction.Contains(@class._ViewSearchData.v_Function)).ToList();
                }
                if (@class._ViewSearchData.v_DevelopmentStage != null && @class._ViewSearchData.v_DevelopmentStage != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDevelopmentStage.Contains(@class._ViewSearchData.v_DevelopmentStage)).ToList();
                }

                //Material
                if (@class._ViewSearchData.v_MaterialOutDateFrom != null && @class._ViewSearchData.v_MaterialOutDateFrom != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) >= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateFrom)).ToList();
                }
                if (@class._ViewSearchData.v_MaterialOutDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smMatOutDate) <= DateTime.Parse(@class._ViewSearchData.v_MaterialOutDateTo)).ToList();
                }

                //DaliverryDate
                if (@class._ViewSearchData.v_DaliverryDateFrom != null && @class._ViewSearchData.v_DaliverryDateFrom != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) >= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateFrom)).ToList();
                }
                if (@class._ViewSearchData.v_DaliverryDateTo != null && @class._ViewSearchData.v_MaterialOutDateTo != "")
                {
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smDeliveryDate) <= DateTime.Parse(@class._ViewSearchData.v_DaliverryDateTo)).ToList();
                }


            }


            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var addWorkSheet = package.Workbook.Worksheets.Add("Sheet1");
                    int onRow = 1;
                    int mainCol = 1;

                    foreach (string name in Header_FileExport)
                    {
                        addWorkSheet.Cells[1, mainCol].Value = name;
                        mainCol++;
                    }
                    //====== end set topic, head column name

                    //push value sine row 16
                    foreach (var item in @class._ListceMastSubMakerRequest)
                    {
                        onRow++;
                        addWorkSheet.Cells[onRow, 1].Value = onRow - 1;
                        addWorkSheet.Cells[onRow, 2].Value = item.smLotNo;
                        addWorkSheet.Cells[onRow, 3].Value = item.smCustomerName;
                        addWorkSheet.Cells[onRow, 4].Value = item.smRevision;
                        addWorkSheet.Cells[onRow, 5].Value = item.smModelName;
                        addWorkSheet.Cells[onRow, 6].Value = item.smFunction;
                        addWorkSheet.Cells[onRow, 7].Value = item.smMoldNo;
                        addWorkSheet.Cells[onRow, 8].Value = item.smMoldName;
                        addWorkSheet.Cells[onRow, 9].Value = item.smCavityNo;
                        addWorkSheet.Cells[onRow, 10].Value = item.smDevelopmentStage;
                        addWorkSheet.Cells[onRow, 11].Value = item.smMatOutDate + " : " + item.smMatOutTime;
                        addWorkSheet.Cells[onRow, 12].Value = item.smDeliveryDate + " : " + item.smDeliveryTime;
                        addWorkSheet.Cells[onRow, 13].Value = item.smEmpCodeApprove + " : " + item.smNameApprove;
                        addWorkSheet.Cells[onRow, 14].Value = item.smStatus;
                    }
                    //FileInfo fileTemp = new FileInfo(TempPath + fileName);
                    //package.SaveAs(fileTemp);
                    //byte[] fileByte = System.IO.File.ReadAllBytes(TempPath + fileName);
                    //File(fileByte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);


                    string config = "S";
                    string msg = $"Saved to:" + fileName; //"Export excel already";
                    return Json(new
                    {
                        c1 = config,
                        c2 = msg
                    });
                }
            }
            catch (Exception ex)
            {
                string config = "E";
                string msg = "Error : " + ex.Message;  //"Export excel already";
                return Json(new
                {
                    c1 = config,
                    c2 = msg
                });
            }


        }



    }
}