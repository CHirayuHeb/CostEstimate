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
using System.Globalization;


using PagedList;
//using System.Globalization;

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
        //public IActionResult Index(Class @class, int? page)
        public IActionResult Index(Class @class)
        {
            try
            {
                //for new his sum 09/07/2025 
                @class._ViewSearchHisSum = new ViewSearchHisSum();
                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                var _listViewceMastSubHistorySum = _MK._ViewceMastSubHistorySum.ToList();

                //List<string> _listMastSubMarker1 = _MK._ViewceMastSubMakerRequest.Select(x => x.smLotNo + "|" + x.smMoldName + "|" + x.smModelName).Distinct().ToList();



                List<string> _listMastSubMarker = _MK._ViewceMastSubMakerRequest.Select(x => x.smLotNo + "|"
                +
                 (_MK._ViewceMastSubMakerRequest.Where(e => e.smLotNo == x.smLotNo).FirstOrDefault() != null
                ? _MK._ViewceMastSubMakerRequest.Where(e => e.smLotNo == x.smLotNo).Select(r => r.smMoldName).FirstOrDefault() : ""
                )
                //x.smMoldName 
                + "|" + x.smModelName
                + "|" +
                (_listViewceMastSubHistorySum.Where(s => s.shLotNo == x.smLotNo).FirstOrDefault() != null
                ? _listViewceMastSubHistorySum.Where(s => s.shLotNo == x.smLotNo).Select(f => f.shStatus).FirstOrDefault() == true ? "OK" : "DRAFT"
                : "NEW"
                )
                ).Distinct().ToList();





                SelectList _listofMastSubMarker = new SelectList(_listMastSubMarker);
                ViewBag.listMastSubMarker = _listofMastSubMarker;



                List<string> _listTypeofCavity = _MK._ViewceMastType.Where(x => x.mtType.Contains("Cavity") && x.mtProgram.Contains("SubMaker")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
                SelectList _TypeofCavity = new SelectList(_listTypeofCavity);
                ViewBag.TypeofCavity = _TypeofCavity;

                //int pageSize = 10; // จำนวนรายการที่จะแสดงต่อหน้า
                //int pageNumber = (page ?? 1); // หน้าเริ่มต้นคือหน้า 1 ถ้าไม่มีการระบุ

                List<ViewceMastFlowApprove> _ViewceMastFlowApprove = _MK._ViewceMastFlowApprove.OrderBy(x => x.mfStep).Distinct().ToList();
                SelectList formStatus = new SelectList(_ViewceMastFlowApprove.Select(s => s.mfSubject).Distinct());
                ViewBag.vbformStatus = formStatus;

                //<List>ViewceMastSubMakerRequest

                @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.OrderByDescending(x => x.smDocumentNo).ToList();

                @class._ListceMastSubMakerRequest.ForEach(item =>
                {
                    var day = item.smIssueDate.Substring(0, 2);
                    var month = item.smIssueDate.Substring(3, 2);
                    var year = item.smIssueDate.Substring(6, 4);

                    item.smIssueDate = year + "/" + month + "/" + day;
                });


                //            @class._ListceMastSubMakerRequest =  _MK._ViewceMastSubMakerRequest
                //.OrderByDescending(x => DateTime.ParseExact(x.smIssueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                //.ToList();
                if (@class._ViewSearchData != null)
                {
                    if (@class._ViewSearchData.v_OrderNo != null && @class._ViewSearchData.v_OrderNo != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smOrderNo.ToUpper().Contains(@class._ViewSearchData.v_OrderNo.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_DocumentNo != null && @class._ViewSearchData.v_DocumentNo != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDocumentNo.ToUpper().Contains(@class._ViewSearchData.v_DocumentNo.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_status != null)
                    {
                        //int smstep = _MK._ViewceMastFlowApprove.Where(x => x.mfSubject.Contains(@class._ViewSearchData.v_status)).Select(x => x.mfStep).FirstOrDefault();
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smStatus.Contains(@class._ViewSearchData.v_status)).ToList();
                        // @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smStep == smstep).OrderBy(x => x.smStep).ThenBy(x => x.smIssueDate).ToList();

                    }
                    if (@class._ViewSearchData.v_LotNo != null && @class._ViewSearchData.v_LotNo != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smLotNo.ToUpper().Contains(@class._ViewSearchData.v_LotNo.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_MoldNo != null && @class._ViewSearchData.v_MoldNo != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smMoldNo.ToUpper().Contains(@class._ViewSearchData.v_MoldNo.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_MoldMass != null && @class._ViewSearchData.v_MoldMass != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smMoldNo.ToUpper().Contains(@class._ViewSearchData.v_MoldMass.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_CusName != null && @class._ViewSearchData.v_CusName != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCustomerName.ToUpper().Contains(@class._ViewSearchData.v_CusName.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_MoldName != null && @class._ViewSearchData.v_MoldName != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smMoldName.ToUpper().Contains(@class._ViewSearchData.v_MoldName.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_ModelName != null && @class._ViewSearchData.v_ModelName != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smModelName.ToUpper().Contains(@class._ViewSearchData.v_ModelName.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_CavityNo != null && @class._ViewSearchData.v_CavityNo != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCavityNo.Contains(@class._ViewSearchData.v_CavityNo)).ToList();
                    }
                    if (@class._ViewSearchData.v_Function != null && @class._ViewSearchData.v_Function != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smFunction.ToUpper().Contains(@class._ViewSearchData.v_Function.ToUpper())).ToList();
                    }
                    if (@class._ViewSearchData.v_DevelopmentStage != null && @class._ViewSearchData.v_DevelopmentStage != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smDevelopmentStage.ToUpper().Contains(@class._ViewSearchData.v_DevelopmentStage.ToUpper())).ToList();
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

                    //date issue
                    //date resuest
                    //var format = "yyyy/MM/dd";
                    //var culture = CultureInfo.InvariantCulture;
                    if (@class._ViewSearchData.v_DateIssueFrom != null && @class._ViewSearchData.v_DateIssueFrom != "")
                    {
                        // @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x =>DateTime.ParseExact(x.smIssueDate, format, culture) >=DateTime.ParseExact(@class._ViewSearchData.v_DateIssueFrom, format, culture)).ToList();

                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smIssueDate) >= DateTime.Parse(@class._ViewSearchData.v_DateIssueFrom)).ToList();
                    }
                    if (@class._ViewSearchData.v_DateIssueTo != null && @class._ViewSearchData.v_DateIssueTo != "")
                    {
                        //@class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.ParseExact(x.smIssueDate, format, culture) <= DateTime.ParseExact(@class._ViewSearchData.v_DateIssueTo, format, culture)).ToList();

                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => DateTime.Parse(x.smIssueDate) <= DateTime.Parse(@class._ViewSearchData.v_DateIssueTo)).ToList();
                    }

                    if (@class._ViewSearchData.v_TypeofCavity != null && @class._ViewSearchData.v_TypeofCavity != "")
                    {
                        //@class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smTypeCavity.Contains(@class._ViewSearchData.v_TypeofCavity)).ToList();

                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smTypeCavity != null && x.smTypeCavity.IndexOf(@class._ViewSearchData.v_TypeofCavity, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }

                    if (@class._ViewSearchData.v_Revision != null && @class._ViewSearchData.v_Revision != "")
                    {
                        @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smRevision.Contains(@class._ViewSearchData.v_Revision)).ToList();
                    }
                }
                // ViewBag.Listcount = @class._ListceMastSubMakerRequest.Count();
                // var pagedList = @class._ListceMastSubMakerRequest
                //.Skip((pageNumber - 1) * pageSize) // ข้ามหน้าที่แล้ว
                //.Take(pageSize) // เลือกข้อมูลในหน้า
                //.ToList();

                // // คำนวณจำนวนหน้าทั้งหมด
                // ViewBag.CurrentPage = pageNumber;
                // //ViewBag.TotalPages = Math.Ceiling((double)@class._ListceMastSubMakerRequest.Count() / pageSize);
                // ViewBag.TotalPages = (int)Math.Ceiling((double)@class._ListceMastSubMakerRequest.Count() / pageSize);
                // // คำนวณหน้าใกล้เคียงที่จะแสดง (เช่น 3 หน้า)
                // int startPage = Math.Max(1, pageNumber - 1); // คำนวณหน้าเริ่มต้น (แสดงหน้า 1 ถึง 3)
                // int endPage = Math.Min(ViewBag.TotalPages, pageNumber + 1); // คำนวณหน้าสิ้นสุด



                // ViewBag.StartPage = startPage;
                // ViewBag.EndPage = endPage;

                // @class._ListceMastSubMakerRequest = pagedList; // ส่งข้อมูลที่แบ่งหน้าแล้วไปยัง View
            }
            catch (Exception ex)
            {
                string a = ex.Message;
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
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCavityNo.Contains(@class._ViewSearchData.v_CavityNo)).ToList();
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
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                int startRow = 1;

                // เขียนหัวตาราง (ชื่อ property)
                var properties = typeof(ViewceMastSubMakerRequest).GetProperties();
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[startRow, col + 1].Value = properties[col].Name;
                }

                // เขียนข้อมูลจาก list
                for (int row = 0; row < @class._ListceMastSubMakerRequest.Count; row++)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(@class._ListceMastSubMakerRequest[row]);
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

            //using (var package = new ExcelPackage())
            //{
            //    var addWorkSheet = package.Workbook.Worksheets.Add("Sheet1");
            //    int onRow = 1;
            //    int mainCol = 1;


            //    foreach (string name in Header_FileExport)
            //    {
            //        addWorkSheet.Cells[1, mainCol].Value = name;
            //        mainCol++;
            //    }
            //    foreach (var item in @class._ListceMastSubMakerRequest)
            //    {
            //        onRow++;
            //        addWorkSheet.Cells[onRow, 1].Value = onRow - 1;
            //        addWorkSheet.Cells[onRow, 2].Value = item.smDocumentNo;
            //        addWorkSheet.Cells[onRow, 3].Value = item.smLotNo;
            //        addWorkSheet.Cells[onRow, 4].Value = item.smCustomerName;
            //        addWorkSheet.Cells[onRow, 5].Value = item.smRevision;
            //        addWorkSheet.Cells[onRow, 6].Value = item.smModelName;
            //        addWorkSheet.Cells[onRow, 7].Value = item.smFunction;
            //        addWorkSheet.Cells[onRow, 8].Value = item.smMoldNo;
            //        addWorkSheet.Cells[onRow, 9].Value = item.smMoldName;
            //        addWorkSheet.Cells[onRow, 10].Value = item.smCavityNo;
            //        addWorkSheet.Cells[onRow, 11].Value = item.smDevelopmentStage;
            //        addWorkSheet.Cells[onRow, 12].Value = item.smMatOutDate + " : " + item.smMatOutTime;
            //        addWorkSheet.Cells[onRow, 13].Value = item.smDeliveryDate + " : " + item.smDeliveryTime;
            //        addWorkSheet.Cells[onRow, 14].Value = item.smEmpCodeApprove + " : " + item.smNameApprove;
            //        addWorkSheet.Cells[onRow, 15].Value = item.smStatus;
            //    }


            //    var stream = new MemoryStream();
            //    package.SaveAs(stream);
            //    stream.Position = 0;

            //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            //}
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
                    @class._ListceMastSubMakerRequest = @class._ListceMastSubMakerRequest.Where(x => x.smCavityNo.Contains(@class._ViewSearchData.v_CavityNo)).ToList();
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

                    //int vRow = 1;
                    //int columnCount = typeof(ViewceMastSubMakerRequest).GetProperties().Length;



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