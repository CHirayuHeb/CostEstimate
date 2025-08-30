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

using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace CostEstimate.Controllers.NewMoldOtherMT
{
    public class NewMoldOtherMTController : Controller
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
        public NewMoldOtherMTController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "5").ToList();

            @class._ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();
            @class._ViewceMastMaterialRequest = new ViewceMastMaterialRequest();
            @class._ListceMastModel = new List<ViewceMastModel>();
            @class._ListViewceItemMaterialRequestPartName = new List<ViewceItemMaterialRequestPartName>();

            if (Docno != null)
            {
                @class._ListceMastModel = _MK._ViewceMastModel.Where(x => x.mmType == "MoldOtherMaterial").OrderBy(x => x.mmNo).ToList();

                @class._ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == Docno).FirstOrDefault();
                @class._ViewceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == Docno).FirstOrDefault();

                @class._ListViewceItemPartName = _MK._ViewceItemPartName.Where(x => x.ipDocumentNo == Docno).OrderBy(x => x.ipRunNo).ToList();
                for (int j = 0; j < @class._ListViewceItemPartName.Count(); j++)
                {
                    for (int i = 0; i < @class._ListceMastModel.Count(); i++)
                    {
                        var _ceMastMaterialRequest = _MK._ViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub
                                                                                          && x.mpNoProcess == @class._ListViewceItemPartName[j].ipRunNo
                                                                                          && x.mpItem.StartsWith(@class._ListceMastModel[i].mmModelName.ToString())
                                                                                          ).FirstOrDefault();
                        @class._ListViewceItemMaterialRequestPartName.Add(new ViewceItemMaterialRequestPartName
                        {
                            mpDocumentNoSub = @class._ViewceMastMaterialRequest.mrDocumentNoSub,
                            mpRunNo = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpRunNo : i + 1,
                            mpPartName = @class._ListViewceItemPartName[j].ipPartName,
                            mpCavityNo = @class._ListViewceItemPartName[j].ipCavityNo,
                            mpTypeCavity = @class._ListViewceItemPartName[j].ipTypeCavity,
                            mpNoProcess = @class._ListViewceItemPartName[j].ipRunNo,  // id master partname
                            mpNo = i + 1,
                            mpItem = @class._ListceMastModel[i].mmModelName,
                            mpPCS = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpPCS : 0,
                            mpAmount = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpAmount : 0,
                            mpTotal = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpTotal : 0,
                            mpIssueDate = @class._ViewceMastMoldOtherRequest != null ? @class._ViewceMastMoldOtherRequest.mrIssueDate : DateTime.Now.ToString("yyyy/MM/dd"),    //_ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpIssueDate : DateTime.Now.ToString("yyyy/MM/dd"),
                        });
                    }
                }

                @class._ListGroupViewceItemMaterialRequestPartName = new List<GroupViewceItemMaterialRequestPartName>();
                @class._ListGroupViewceItemMaterialRequestPartName = @class._ListViewceItemMaterialRequestPartName.GroupBy(x => new { x.mpPartName, x.mpCavityNo, x.mpTypeCavity, x.mpNoProcess })
                                                                        .Select(g => new GroupViewceItemMaterialRequestPartName
                                                                        {
                                                                            mpPartName = g.Key.mpPartName,
                                                                            mpCavityNo = g.Key.mpCavityNo,
                                                                            mpTypeCavity = g.Key.mpTypeCavity,
                                                                            mpNoProcess = g.Key.mpNoProcess,
                                                                            ItemMaterialRequestPartName = g.ToList()
                                                                        }).ToList();

                @class._listAttachment = _IT.Attachment.Where(x => x.fnNo == @class._ViewceMastMaterialRequest.mrDocumentNoSub).ToList();

            }

            return View(@class);
        }

        [Authorize("Checked")]
        [HttpPost]
        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            int v_step = @classs._ViewceMastMaterialRequest != null ? @classs._ViewceMastMaterialRequest.mrStep : 0;
            string v_issue = @classs._ViewceMastMaterialRequest != null ? @classs._ViewceMastMaterialRequest.mrEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastMaterialRequest != null ? @classs._ViewceMastMaterialRequest.mrDocumentNoSub : "";
            List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
            //_listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == v_DocNo).OrderBy(x => x.htStep).ToList();

            _listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == v_DocNo).OrderBy(x => x.htDate).ThenBy(x => x.htTime).ThenBy(x => x.htStep).ToList();
            if (_listHistory.Count() > 0)
            {
                for (int j = 0; j < _listHistory.Count(); j++)
                {
                    var v_htcc = _listHistory[j].htCC;
                    string v_CCemail = "";
                    if (v_htcc != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = v_htcc.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emEmpcode == i.Trim()).FirstOrDefault();
                                    v_CCemail += fromEmailCC.emName_M365.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }
                    }

                    _listHistory[j].htCC = v_CCemail;


                }
            }



            partialUrl = Url.Action("SendMail", "NewMoldOtherMT", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });

            //return Json(new { status = "hasHistory", listHistory = _listHistory, partial = partialUrl });

            return Json(new { status = _listHistory.Count() > 0 ? "hasHistory" : "empty", listHistory = _listHistory, partial = partialUrl });
        }
        public ActionResult SendMail(Class @class, int s_step, string s_issue, string mpNo)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
            var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365

            //To
            string v_empCodeTo = s_step == 3 ? _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == mpNo).Select(x => x.mrEmpCodeRequest).FirstOrDefault()
               : _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "5") != null
                    ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "5").Select(x => x.mfTo).FirstOrDefault()
                    : "";

            string v_listemailTo;

            v_empCodeTo = v_empCodeTo != null ? v_empCodeTo.Split(",").Count() > 0 ? v_empCodeTo.Split(",")[0] : v_empCodeTo : "";




            var v_nameTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empCodeTo.Trim()).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
            string v_empCodeCC = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "5") != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "4").Select(x => x.mfCC).FirstOrDefault() : "";


            string[] s_empCodeCC;// = v_empCodeCC.Split(",");
            string v_nameCC = "";

            if (!string.IsNullOrWhiteSpace(v_empCodeCC))
            {
                s_empCodeCC = v_empCodeCC.Split(",");
                for (int l = 0; l < s_empCodeCC.Count(); l++)
                {
                    var v_emailcc = _IT.rpEmails.Where(x => x.emEmpcode == s_empCodeCC[l].ToString()).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                    v_nameCC += v_emailcc + ",";
                    //_listNameTo.Add(v_emailcc);
                }
            }





            ViewBag.step = s_step;
            @class._ViewceHistoryApproved.htFrom = v_emailFrom;
            @class._ViewceHistoryApproved.htTo = v_nameTo;
            @class._ViewceHistoryApproved.htCC = v_nameCC;
            @class._ViewceHistoryApproved.htStatus = "Approve";
            return PartialView("SendMail", @class);
        }

        [HttpPost]
        public JsonResult chkSaveData(Class @class, List<IFormFile> files, string _ItemMaterialRequestPartName)
        {
            string config = "S";
            string msg = "Send Mail & Save File Already";
            string vStatus = "";
            string[] chkPermis;
            string[] chkSave;
            string[] chkSaveHistory;
            string[] chkSaveSendMail;
            int i_Step = 0;

            string[] vRunDoc;
            string[] vRunDocNo;

            try
            {


                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                i_Step = @class._ViewceMastMaterialRequest != null ? @class._ViewceMastMaterialRequest.mrStep : 0;
                if (@class._ViewceHistoryApproved.htTo != null || (@class._ViewceHistoryApproved.htTo == null && @class._ViewceHistoryApproved.htStatus == "Disapprove"))
                {
                    if (@class._ViewceHistoryApproved.htStatus == "Approve")
                    {
                        i_Step = i_Step + 1;
                        config = "S";

                        ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();
                        if (fromEmailTO == null)
                        {
                            config = "E";
                            msg = "Please Check your Email to , Email incorrect !!!";
                        }

                    }
                    else if (@class._ViewceHistoryApproved.htStatus == "Disapprove")
                    {
                        i_Step = 8;
                        config = "S";
                        string v_empissue = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub).Select(x => x.mrEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                        @class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).First();
                    }
                    else
                    {
                        config = "E";
                        msg = "Please input Status";
                    }
                }
                else
                {
                    config = "E";
                    msg = "Please input e-mail.";

                }
                if (config == "S")
                {
                    string vDoc = @class._ViewceMastMaterialRequest.mrDocumentNoSub;
                    if (_ItemMaterialRequestPartName != null)
                    {
                        @class._ListViewceItemMaterialRequestPartName = JsonConvert.DeserializeObject<List<ViewceItemMaterialRequestPartName>>(_ItemMaterialRequestPartName);
                    }

                    //save
                    chkSave = Save(@class, i_Step, files, "S");
                    if (chkSave[0] == "E")
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                    }

                    //save history
                    chkSaveHistory = SaveHistory(@class, i_Step, vDoc);
                    if (chkSave[0] == "E")
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                    }


                    //send mail
                    chkSaveSendMail = SendMailHistory(@class, i_Step, vDoc);
                    if (chkSaveSendMail[0] == "E")
                    {
                        config = chkSaveSendMail[0];
                        msg = chkSaveSendMail[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveSendMail[0];
                        msg = chkSaveSendMail[1];
                    }


                }
                else
                {
                    config = "E";
                    return Json(new { c1 = config, c2 = msg });
                }

            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
            }


            return Json(new { c1 = config, c2 = msg });
        }

        public string[] Save(Class @class, int vstep, List<IFormFile> files, string savetype)
        {
            string empissue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string UpdateBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            //User.Claims.FirstOrDefault(s => s.Type == "NICKNAME")?.Value;
            string v_msg = "";
            string v_status = "";

            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string vDocNo = @class._ViewceMastMaterialRequest.mrDocumentNoSub;
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "5").Select(x => x.mfSubject).First();
                    string empApprove = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() : @class._ViewceMastMaterialRequest.mrEmpCodeApprove;
                    string NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastMaterialRequest.mrNameApprove;

                    vstep = vstep == 9 ? vstep = 0 : vstep;
                    ViewceMastMaterialRequest _ceMastMaterialRequest = new ViewceMastMaterialRequest();
                    if (savetype == "S")
                    {
                        _ceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == vDocNo).FirstOrDefault();
                        _ceMastMaterialRequest.mrIssueDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                        _ceMastMaterialRequest.mrStep = vstep;
                        _ceMastMaterialRequest.mrStatus = _smStatus;
                        //_ceMastWorkingTimeRequest.wrEmpCodeRequest
                        //_ceMastWorkingTimeRequest.wrNameRequest
                        _ceMastMaterialRequest.mrEmpCodeApprove = empApprove;
                        _ceMastMaterialRequest.mrNameApprove = NickNameApprove;
                        _ceMastMaterialRequest.mrFlowNo = 5;
                        _MK._ViewceMastMaterialRequest.Update(_ceMastMaterialRequest);
                        _MK.SaveChanges();
                    }

                    var itemMT = _MK._ViewceItemMaterialRequestPartName.Where(p => p.mpDocumentNoSub == vDocNo).ToList();
                    if (itemMT.Count > 0)
                    {
                        _MK._ViewceItemMaterialRequestPartName.RemoveRange(itemMT);
                        _MK.SaveChanges();
                    }


                    //SqlBulk insert
                    var connection = (SqlConnection)_MK.Database.GetDbConnection();
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // ใช้ Transaction เดิมของ EF
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, (SqlTransaction)dbContextTransaction.GetDbTransaction()))
                    {
                        bulkCopy.DestinationTableName = "ceItemMaterialRequestPartName";
                        bulkCopy.BatchSize = 5000;
                        bulkCopy.BulkCopyTimeout = 0;

                        // --- สร้าง DataTable ---
                        DataTable dt = new DataTable();
                        dt.Columns.Add("mpDocumentNoSub", typeof(string));
                        dt.Columns.Add("mpRunNo", typeof(int));
                        dt.Columns.Add("mpPartName", typeof(string));
                        dt.Columns.Add("mpCavityNo", typeof(string));
                        dt.Columns.Add("mpTypeCavity", typeof(string));
                        dt.Columns.Add("mpNoProcess", typeof(string));
                        dt.Columns.Add("mpNo", typeof(string));
                        dt.Columns.Add("mpItem", typeof(string));
                        dt.Columns.Add("mpPCS", typeof(int));
                        dt.Columns.Add("mpAmount", typeof(decimal));
                        dt.Columns.Add("mpTotal", typeof(decimal));
                        dt.Columns.Add("mpIssueDate", typeof(DateTime));

                        foreach (var item in @class._ListViewceItemMaterialRequestPartName)
                        {
                            dt.Rows.Add(
                                item.mpDocumentNoSub,
                                item.mpRunNo,
                                item.mpPartName,
                                item.mpCavityNo,
                                item.mpTypeCavity,
                                item.mpNoProcess,
                                item.mpNo,
                                item.mpItem,
                                item.mpPCS,
                                item.mpAmount,
                                item.mpTotal,
                                item.mpIssueDate
                            );
                        }

                        // Mapping columns
                        bulkCopy.ColumnMappings.Add("mpDocumentNoSub", "mpDocumentNoSub");
                        bulkCopy.ColumnMappings.Add("mpRunNo", "mpRunNo");
                        bulkCopy.ColumnMappings.Add("mpPartName", "mpPartName");
                        bulkCopy.ColumnMappings.Add("mpCavityNo", "mpCavityNo");
                        bulkCopy.ColumnMappings.Add("mpTypeCavity", "mpTypeCavity");
                        bulkCopy.ColumnMappings.Add("mpNoProcess", "mpNoProcess");
                        bulkCopy.ColumnMappings.Add("mpNo", "mpNo");
                        bulkCopy.ColumnMappings.Add("mpItem", "mpItem");
                        bulkCopy.ColumnMappings.Add("mpPCS", "mpPCS");
                        bulkCopy.ColumnMappings.Add("mpAmount", "mpAmount");
                        bulkCopy.ColumnMappings.Add("mpTotal", "mpTotal");
                        bulkCopy.ColumnMappings.Add("mpIssueDate", "mpIssueDate");

                        // Bulk insert
                        bulkCopy.WriteToServer(dt);
                    }




                    _MK.SaveChanges();
                    dbContextTransaction.Commit();


                    string[] v_statusFile = savefile(@class, files, vDocNo);

                    v_status = v_statusFile[0];
                    v_msg = v_statusFile[1];
                }
                catch (Exception ex)
                {
                    try { dbContextTransaction.Rollback(); } catch { }
                    v_status = "E";
                    v_msg = "Error Save: " + ex.InnerException.Message;
                }
            }


            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }
        public string[] chkPermission(Class @class)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string _Permiss = User.Claims.FirstOrDefault(s => s.Type == "Permission")?.Value;
            string message_per = "";
            string status_per = "";
            var chkData = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub).FirstOrDefault();
            try
            {
                if (chkData != null)
                {
                    if (chkData.mrStep != 4)
                    {
                        //check operator //check create user
                        if (chkData.mrStep == 0 && _UserId == chkData.mrEmpCodeRequest)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (_UserId == chkData.mrEmpCodeApprove)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (chkData.mrStep == 4 && _Permiss.ToUpper() == "ADMIN")
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else
                        {
                            status_per = "P";
                            message_per = "You don't have permission to access";
                        }
                    }
                    else
                    {
                        status_per = "P";
                        message_per = "You don't have permission to access";
                    }

                }
                else
                {
                    status_per = "S";
                    message_per = "You have permission ";
                }

                string[] returnvar = { status_per, message_per };
                return returnvar;
            }
            catch (Exception ex)
            {
                string[] returnvar = { status_per, message_per };
                return returnvar;

            }
        }
        public string[] savefile(Class @class, List<IFormFile> file, string RunDoc)
        {
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string fileName = "";
            string v_error = "";
            string v_fnType = "";// v_type != "" ? v_type : @class._ViewsvsServiceRequest.srType;

            string v_status = "";
            string v_msg = "";
            int v_count = 0;
            try
            {
                if (file is null)
                {
                    v_status = "S";
                    v_msg = "Save & Send Mail Already";
                }
                else
                {
                    List<ViewAttachment> listInsert = new List<ViewAttachment>();
                    using (var dbContextTransaction = _IT.Database.BeginTransaction())
                    {
                        try
                        {
                            if (file is null)
                            {
                                //vStatus = "File not found";
                                v_status = "S";
                                v_msg = "Save & Send Mail Already";
                            }
                            else
                            {
                                for (int i = 0; i < file.Count; i++)
                                {
                                    string IssueDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    fileName = IssueDate + "-" + file[i].FileName;//System.IO.Path.GetExtension(file.FileName).ToLower();
                                    string filePath = path + fileName;
                                    var fileLocation = new FileInfo(filePath);
                                    //filePaths.Add(filePath);
                                    if (!Directory.Exists(filePath))
                                    {
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file[i].CopyTo(stream);
                                        }
                                    }
                                    ViewAttachment _viewAttachment = new ViewAttachment();
                                    _viewAttachment.fnNo = RunDoc; // @class._ViewceMastSubMakerRequest.smRevision;//vSno.ToString();
                                    _viewAttachment.fnPath = filePath;
                                    _viewAttachment.fnFilename = fileName;
                                    _viewAttachment.fnIssueBy = IssueBy;
                                    _viewAttachment.fnUpdateBy = IssueBy;
                                    _viewAttachment.fnType = v_fnType;
                                    _viewAttachment.fnProgram = PgName; //Program  name CostEstimate
                                                                        //_IT.Attachment.AddAsync(_viewAttachment);
                                                                        //_IT.SaveChanges();
                                                                        //vStatus = fileName;
                                    listInsert.Add(_viewAttachment);
                                }

                            }
                            _IT.Attachment.AddRange(listInsert); // Add หลายรายการ
                            _IT.SaveChanges(); // Save ทีเดียว
                            dbContextTransaction.Commit();
                            v_status = "S";
                            v_msg = "Save & Send Mail Already";
                        }
                        catch (Exception e)
                        {
                            v_status = "E";
                            v_error = e.Message;
                            v_msg = "Error Save file :" + e.Message;
                            //dbContextTransaction.Rollback();
                            try
                            {
                                dbContextTransaction.Rollback();
                            }
                            catch
                            {
                                // ignore ถ้า transaction ปิดไปแล้ว
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                v_status = "E";
                v_error = e.Message;
                v_msg = "Error Save file :" + e.Message;

            }
            v_count = v_count;
            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        public string[] SaveHistory(Class @class, int vstep, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";

            //test send mail
            string vEmpCodeCCemail = "";


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "4").Select(x => x.mfSubject).First();
                    vstep = vstep == 8 ? vstep = 0 : vstep;

                    if (@class._ViewceHistoryApproved.htCC != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                    vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }

                        ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                        _ViewceHistoryApproved.htDocNo = RunDoc;// getSrNo[0].ToString();
                        _ViewceHistoryApproved.htStep = vstep;
                        _ViewceHistoryApproved.htStatus = @class._ViewceHistoryApproved.htStatus;
                        _ViewceHistoryApproved.htFrom = @class._ViewceHistoryApproved.htFrom;
                        _ViewceHistoryApproved.htTo = @class._ViewceHistoryApproved.htTo;
                        _ViewceHistoryApproved.htCC = vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                        _ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                        _ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                        _ViewceHistoryApproved.htRemark = @class._ViewceHistoryApproved.htRemark;
                        _MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);
                        _MK.SaveChanges();
                    }

                    _MK.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    // dbContextTransaction.Rollback();
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    v_status = "E";
                    // v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    v_msg = "Error Save: " + ex.InnerException.Message;


                }
            }
            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        public string[] SendMailHistory(Class @class, int vstep, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";
            string vCCemail = "";
            try
            {
                RunDoc = @class._ViewceMastMaterialRequest.mrDocumentNo;

                string Empcode_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
                int vRevision = @class._ViewceMastMoldOtherRequest.mrRevision;
                string Name_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "NameE")?.Value; // HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NICKNAME)?.Value;
                string v_EmpCodeRequest = @class._ViewceMastMaterialRequest.mrEmpCodeRequest == null || @class._ViewceMastMaterialRequest.mrEmpCodeRequest == ""
                                                                                    ? Empcode_IssueBy + " : " + _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == Empcode_IssueBy).Select(x => x.NICKNAME).First()
                                                                                    : @class._ViewceMastMaterialRequest.mrEmpCodeRequest + " : " + @class._ViewceMastMaterialRequest.mrNameRequest;



                string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "5").Select(x => x.mfSubject).First();
                vstep = vstep == 8 ? vstep = 0 : vstep;

                var email = new MimeMessage();
                ViewrpEmail fromEmailFrom = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).FirstOrDefault();
                ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();

                MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);
                email.Subject = "Cost Estimate : Mold Other ==> Material   " + _smStatus; /*( " + _ViewlrBuiltDrawing.bdDocumentType + " ) " + _ViewlrHistoryApprove.htStatus*/;
                //email.From.Add(MailboxAddress.Parse(_ViewlrHistoryApprove.htFrom));
                email.From.Add(FromMailFrom);
                email.To.Add(FromMailTO);

                if (@class._ViewceHistoryApproved.htCC != null)
                {
                    ViewrpEmail fromEmailCC = new ViewrpEmail();
                    string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                    foreach (var i in splitCC)
                    {
                        if (i != " " & i != "")
                        {
                            var v_cc = "";
                            try
                            {
                                fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                MailboxAddress FromMailcc = new MailboxAddress(fromEmailCC.emName_M365, fromEmailCC.emEmail_M365);
                                email.Cc.Add(FromMailcc);
                                vCCemail += fromEmailCC.emEmail_M365.ToString() + ",";
                            }
                            catch (Exception e)
                            {
                                v_cc = e.Message;
                            }
                        }
                    }
                }
                var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther&subType=M";
                //var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther";// + getSrNo[0].ToString();
                var bodyBuilder = new BodyBuilder();
                //var image = bodyBuilder.LinkedResources.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\wwwroot\images\btn\OK.png");
                string vIssue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                string vIssueName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;
                string EmailBody = $"<div>" +
                    $"<B>Cost Estimate : Mold Other ==> Material </B> <br>" +
                    $"<B>Document No : </B> " + RunDoc + "<br>" +  //v_EmpCodeRequest
                    $"<B>Customer Name : </B> " + @class._ViewceMastMoldOtherRequest.mrCustomerName + "<br>" +
                    $"<B>Function : </B> " + @class._ViewceMastMoldOtherRequest.mrFunction + "<br>" +
                    $"<B>Revision No : </B> " + vRevision.ToString("D2") + " <br>" +
                    $"<B>Model Name : </B> " + @class._ViewceMastMoldOtherRequest.mrModelName + "<br>" +
                    $"<B>Request By : </B> " + v_EmpCodeRequest + "<br>" +
                    $"<B>Status : </B> " + _smStatus + "<br> " +
                    $"<B> หมายเหตุ : </B> " + @class._ViewceHistoryApproved.htRemark + "<br> " +
                    $"คลิ๊กลิงค์เพื่อเปิดเอกสาร <a href='" + varifyUrl + "'>More Detail" +
                    $"</a>" +
                    $"</div>";

                // bodyBuilder.Attachments.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\dev_rfc.log");

                bodyBuilder.HtmlBody = string.Format(EmailBody);
                email.Body = bodyBuilder.ToMessageBody();

                // send email
                var smtp1 = new SmtpClient();
                //smtp.Connect("mail.csloxinfo.com");
                smtp1.Connect("203.146.237.138");
                //smtp.Connect("10.200.128.12");
                smtp1.Send(email);
                smtp1.Disconnect(true);

                v_status = "S";
                v_msg = "File saved and email sent.!!!";
            }
            catch (Exception ex)
            {
                // dbContextTransaction.Rollback();

                v_status = "E";
                v_msg = "Error Save History: " + ex.Message;
            }



            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        public ActionResult DeteleDataFile(string id, string vname)
        {
            try
            {
                //var find = _IT.Attachment(X => X.)

                ViewAttachment find = _IT.Attachment.Where(x => x.fnNo == id && x.fnFilename == vname && x.fnProgram == "CostEstimate").FirstOrDefault();
                var delete = _IT.Attachment.Remove(find);


                _IT.SaveChanges();
            }
            catch
            {
                return Json(new { res = "error" });

            }
            return Json(new { res = "success" });


            //return Json(_IT.rpEmails.Where(p => p.emEmail.Contains(term) || p.emEmail_M365.Contains(term)).Select(p => p.emEmail_M365).ToList());

        }


        [Authorize("Checked")]
        [HttpPost]
        public JsonResult SaveDraft(Class @class, List<IFormFile> files, string _ItemMaterialRequestPartName)
        {

            string config = "S";
            string msg = "";

            string[] chkPermis;
            string[] chkSave;

            int i_Step = 0;
            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;
            try
            {
                i_Step = @class._ViewceMastMaterialRequest.mrStep;

                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                if (_ItemMaterialRequestPartName != null)
                {
                    @class._ListViewceItemMaterialRequestPartName = JsonConvert.DeserializeObject<List<ViewceItemMaterialRequestPartName>>(_ItemMaterialRequestPartName);
                }


                chkSave = Save(@class, i_Step, files, "D");
                if (chkSave[0] == "E")
                {
                    config = chkSave[0];
                    msg = chkSave[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                else
                {
                    config = chkSave[0];
                    msg = "Save Data success ";
                    // msg = chkSave[1];
                }

            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;

            }
            return Json(new { c1 = config, c2 = msg });
        }



    }
}