using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.Table.HRMS;
using CostEstimate.Models.Table.IT;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MK;
using MimeKit;
using MailKit.Net.Smtp;

namespace CostEstimate.Controllers.Account
{
    public class LoginController : Controller
    {
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private CacheSettingController _Cache;
        public string pgmName = "CostEstimateRequest";
        public LoginController(LAMP lamp, HRMS hrms, CacheSettingController cacheController, IT it, MK MK)
        {
            _LAMP = lamp;
            _HRMS = hrms;
            _IT = it;
            _Cache = cacheController;
            _MK = MK;
        }
        public IActionResult Index(string DocumentNo, string DocType, string subType)
        {
            //string domain = Environment.UserDomainName;
            //string user = Environment.UserName;
            //Console.WriteLine("Full User: " + domain + "\\" + user);
            string remember = User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            Class @class = new Class();
            if (DocumentNo != null)
                @class.param = DocumentNo;
            if (DocType != null)
                @class.paramtype = DocType;
            if (subType != null)
                @class.Moldtype = subType;
            //try
            //{
            //    subType = subType.Length > 2 ? subType.Substring(0, subType.Length - 2) : subType;
            //    @class.Moldtype = subType;
            //}
            //catch (Exception ex)
            //{

            //}



            if (remember != null)
            {
                return RedirectToAction("RememberMe", "Login", @class);
            }
            return View(@class);
        }

        [HttpPost]
        public async Task<IActionResult> Autherize(Class @class)
        {
            try
            {
                ViewLogin login = new ViewLogin();
                string sUsername = @class._ViewLoginPgm.UserId.Trim();
                string sPassword = @class._ViewLoginPgm.Password.Trim();

                ViewAccEMPLOYEE accData = new ViewAccEMPLOYEE();

                //ViewLoginPgm _ViewLoginPgm = new ViewLoginPgm();
                ViewLoginPgm _ViewLoginPgm = _MK._ViewLoginPgm.FirstOrDefault(x => x.UserId == sUsername && x.Password == sPassword && x.Program == "CostEstimateRequest");
                if (_ViewLoginPgm != null)
                {
                    accData = _HRMS.AccEMPLOYEE.FirstOrDefault(x => x.EMP_CODE == _ViewLoginPgm.Empcode);
                    if (accData != null)
                    {
                        string[] stat = await Task.Run(() => SetClaim(accData, _ViewLoginPgm.Permission, sUsername, sPassword));
                        if (stat[0] == "Ok")
                        {
                            if (@class.param != null)
                            {
                                if (@class.paramtype == "MoldModify")
                                {
                                    var _ceMast = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == @class.param).FirstOrDefault();
                                    if (_ceMast != null)
                                    {
                                        return RedirectToAction("Index", "NewMoldModify", new { id = @class.param });
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "ErrorPage");
                                    }

                                }
                                else if (@class.paramtype == "SubMarker")
                                {
                                    var _ceMast = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == @class.param).FirstOrDefault();
                                    if (_ceMast != null)
                                    {
                                        return RedirectToAction("Index", "New", new { smDocumentNo = @class.param });
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "ErrorPage");
                                    }


                                   
                                }
                                else if (@class.paramtype == "MoldOther") //MoldOther
                                {
                                    if (@class.Moldtype != null)
                                    {
                                        if (@class.Moldtype == "W") //working time
                                        {
                                            var _ceMast = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == @class.param).FirstOrDefault();
                                            if (_ceMast != null)
                                            {
                                                return RedirectToAction("Index", "NewMoldOtherWK", new { Docno = @class.param });

                                            }
                                            else
                                            {
                                                return RedirectToAction("Index", "ErrorPage");
                                            }

                                        }
                                        else if (@class.Moldtype == "M") //Material
                                        {
                                            var _ceMast = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == @class.param).FirstOrDefault();
                                            if (_ceMast != null)
                                            {
                                                return RedirectToAction("Index", "NewMoldOtherMT", new { Docno = @class.param });

                                            }
                                            else
                                            {
                                                return RedirectToAction("Index", "ErrorPage");
                                            }

                                        }
                                        else if (@class.Moldtype == "T") //Tool GR
                                        {
                                            var _ceMast = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == @class.param).FirstOrDefault();
                                            if (_ceMast != null)
                                            {
                                                return RedirectToAction("Index", "NewMoldOtherTGR", new { Docno = @class.param });

                                            }
                                            else
                                            {
                                                return RedirectToAction("Index", "ErrorPage");
                                            }

                                        }
                                        else if(@class.Moldtype == "I") //Information Spac
                                        {
                                            var _ceMast = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == @class.param).FirstOrDefault();
                                            if(_ceMast != null)
                                            {
                                                return RedirectToAction("Index", "NewMoldOtherSM", new { Docno = @class.param });
                                            }
                                            else
                                            {
                                                return RedirectToAction("Index", "ErrorPage");
                                            }

                                        }
                                        else
                                        {
                                            return RedirectToAction("Index", "ErrorPage");
                                        }

                                    }
                                    return RedirectToAction("Index", "NewMoldOther", new { id = @class.param });


                                }
                                else // other
                                {
                                    return RedirectToAction("Index", "New", new { smDocumentNo = @class.param });
                                }

                                //return RedirectToAction("Index", "New", new { smDocumentNo = @class.param });
                            }
                            else
                            {

                                return RedirectToAction("Index", "MainPage");
                                // return RedirectToAction("Index", "Home");
                            }


                        }
                        else
                        {
                            @class._Error = new Error();
                            @class._Error.validation = stat[1];
                            return View("Index", @class);
                        }
                    }
                    else
                    {
                        @class._Error = new Error();
                        @class._Error.validation = "Username or Password invalid Or Username  must have Email M365";
                        return View("Index", @class);
                    }

                }
                else
                {
                    @class._Error = new Error();
                    @class._Error.validation = "Username or Password invalid Or Username  must have Email M365";
                    return View("Index", @class);
                }
            }
            catch (Exception ex)
            {
                @class._Error = new Error();
                @class._Error.validation = ex.Message;
                return View("Index", @class);
            }


        }

        public async Task<string[]> SetClaim(ViewAccEMPLOYEE accdata, string vAccess, string vusername, string vpassword)
        {
            try
            {
                ViewAccEMPLOYEE acc = new ViewAccEMPLOYEE();
                string Email = "";


                _Cache.clearCacheAccEmployee();
                acc = await Task.Run(() => _Cache.cacheAccEmployee().FirstOrDefault(s => s.EMP_CODE == accdata.EMP_CODE));
                ViewrpEmail Emails = _Cache.cacheEmail().Where(w => w.emEmail_M365 != null && w.emEmpcode.Trim() == acc.EMP_CODE.Trim()).FirstOrDefault();
                if (Emails is null)
                {
                    Email = GlobalVariable.AdminEmail;
                }
                else
                {
                    Email = _Cache.cacheEmail().Where(w => w.emEmpcode.Trim() == acc.EMP_CODE.Trim()).FirstOrDefault().emEmail_M365.Trim();
                }


                acc.DIVI_CODE = await Task.Run(() => _Cache.cacheAccDIVIMast().Where(w => w.DIVI_CODE == acc.DIVI_CODE).FirstOrDefault().DIVI_NAME);
                acc.DEPT_CODE = await Task.Run(() => _Cache.cacheDEPTMast().Where(w => w.DEPT_CODE == acc.DEPT_CODE).FirstOrDefault().DEPT_NAME);
                acc.SEC_CODE = await Task.Run(() => _Cache.cacheSECMast().Where(w => w.SEC_CODE == acc.SEC_CODE).FirstOrDefault().SEC_NAME);
                acc.GRP_CODE = await Task.Run(() => _Cache.cacheGRPMast().Where(w => w.GRP_CODE == acc.GRP_CODE).FirstOrDefault().GRP_NAME);

                acc.LAST_TNAME = acc.LAST_TNAME is null ? "" : acc.LAST_TNAME;
                acc.NICKNAME = acc.EMP_ENAME.Substring(0, 1) + (acc.LAST_ENAME == "" ? "" : acc.LAST_ENAME.Substring(0, 1))?.ToString();



                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Country, "CostEstimate"));
                claims.Add(new Claim(ClaimTypes.Name, acc.EMP_CODE.ToString()));
                claims.Add(new Claim(ClaimTypes.Actor, acc.EMP_TNAME + " " + acc.LAST_TNAME));
                claims.Add(new Claim("UserId", acc.EMP_CODE.ToString()));

                claims.Add(new Claim("Username", vusername?.ToString()));
                claims.Add(new Claim("Password", vpassword?.ToString()));

                claims.Add(new Claim("EmpCode", acc.EMP_CODE?.ToString()));
                claims.Add(new Claim("Permission", vAccess?.ToString()));
                // claims.Add(new Claim(ClaimTypes.Role, login.Permission?.ToString()));
                claims.Add(new Claim("Division", acc.DIVI_CODE.ToUpper()));
                claims.Add(new Claim("Department", acc.DEPT_CODE.ToUpper()));
                claims.Add(new Claim("DeptCode", acc.DEPT_CODE.ToUpper()));
                claims.Add(new Claim("Section", acc.SEC_CODE.ToUpper()));
                claims.Add(new Claim("Group", acc.GRP_CODE.ToUpper()));
                claims.Add(new Claim("Unit", acc.UNT_CODE.ToUpper()));
                claims.Add(new Claim("Position", acc.POS_CODE.ToUpper()));
                claims.Add(new Claim("ProgramName", GlobalVariable.ProgramName));
                claims.Add(new Claim("PriName", acc.PRI_THAI));
                claims.Add(new Claim("Name", acc.EMP_TNAME));
                claims.Add(new Claim("SurName", acc.LAST_TNAME));
                claims.Add(new Claim("NICKNAME", acc.NICKNAME.ToUpper()));
                claims.Add(new Claim("Email", Email));

                ClaimsIdentity identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , principal, new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        AllowRefresh = true, // ✅ อนุญาตให้ session ถูกยืดอายุ
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    }); //true is remember login

                //            await HttpContext.SignInAsync(
                //CookieAuthenticationDefaults.AuthenticationScheme,
                //new ClaimsPrincipal(identity),
                //new AuthenticationProperties
                //{
                //    IsPersistent = true,
                //    AllowRefresh = true, // ✅ อนุญาตให้ session ถูกยืดอายุ
                //    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                //});



                string[] stat = { "Ok" };
                return stat;
            }
            catch (Exception ex)
            {
                string[] stat = { "Ng", ex.Message };
                return stat;
            }
        }

        public string sendMail(Class @class)
        {
            try
            {
                var email = new MimeMessage();

                ViewrpEmail fromEmailFrom = _IT.rpEmails.Where(w => w.emEmpcode == @class._ViewLogin.emailFrom).FirstOrDefault();
                ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emEmpcode == @class._ViewLogin.emailTo).FirstOrDefault();

                MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                //MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);

                MailboxAddress FromMailTO = new MailboxAddress("test", "OG_THS000141@stanley-electric.com");

                email.Subject = "test mail";//*( " + _ViewlrBuiltDrawing.bdDocumentType + " ) " + _ViewlrHistoryApprove.htStatus*/;
                                            //email.From.Add(MailboxAddress.Parse(_ViewlrHistoryApprove.htFrom));
                email.From.Add(FromMailFrom);
                email.To.Add(FromMailTO);
                var bodyBuilder = new BodyBuilder();
                string EmailBody = "test mail";
                bodyBuilder.HtmlBody = string.Format(EmailBody);
                email.Body = bodyBuilder.ToMessageBody();

                var smtp = new SmtpClient();
                //smtp.Connect("mail.csloxinfo.com");
                smtp.Connect("203.146.237.138");
                //smtp.Connect("10.200.128.12");s
                smtp.Send(email);
                smtp.Disconnect(true);
                return "";
            }
            catch (Exception ex)
            {
                return "fail";
            }



        }


        public async Task<IActionResult> RememberMe(Class @class)
        {
            try
            {
                ViewLogin login = new ViewLogin();
                string sUsername = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;
                string sPassword = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Password")?.Value;
                // login = _LAMP.Login.FirstOrDefault(s => s.UserId == sUsername && s.Password == sPassword && s.Program == GlobalVariable.ProgramName);
                ViewLoginPgm _ViewLoginPgm = _MK._ViewLoginPgm.FirstOrDefault(x => x.UserId == sUsername && x.Password == sPassword && x.Program == "CostEstimateRequest");
                // string[] stat = await Task.Run(() => SetClaim(login));
                ViewAccEMPLOYEE accData = new ViewAccEMPLOYEE();
                accData = _HRMS.AccEMPLOYEE.FirstOrDefault(x => x.EMP_CODE == _ViewLoginPgm.Empcode);
                string[] stat = await Task.Run(() => SetClaim(accData, _ViewLoginPgm.Permission, sUsername, sPassword));
                if (stat[0] == "Ok")
                {
                    if (@class.param != null)
                    {
                        if (@class.paramtype == "MoldModify")
                        {
                            return RedirectToAction("Index", "NewMoldModify", new { id = @class.param });
                        }
                        else if (@class.paramtype == "SubMarker")
                        {
                            return RedirectToAction("Index", "New", new { smDocumentNo = @class.param });
                        }
                        else if (@class.paramtype == "MoldOther") //MoldOther
                        {
                            if (@class.Moldtype != null)
                            {
                                if (@class.Moldtype == "W") //working time
                                {
                                    return RedirectToAction("Index", "NewMoldOtherWK", new { Docno = @class.param });
                                }
                                else if (@class.Moldtype == "M") //Material
                                {
                                    return RedirectToAction("Index", "NewMoldOtherMT", new { Docno = @class.param });
                                }
                                else if (@class.Moldtype == "T") //Tool GR
                                {
                                    return RedirectToAction("Index", "NewMoldOtherTGR", new { Docno = @class.param });
                                }
                                else //if(@class.Moldtype == "I") //Information Spac
                                {
                                    return RedirectToAction("Index", "NewMoldOther", new { Docno = @class.param });
                                }

                            }
                            return RedirectToAction("Index", "NewMoldOther", new { id = @class.param });


                        }
                        else // other
                        {
                            return RedirectToAction("Index", "New", new { smDocumentNo = @class.param });
                        }

                        // return RedirectToAction("Index", "New", new { smDocumentNo = @class.param });
                    }
                    else
                    {
                        return RedirectToAction("Index", "MainPage");
                    }


                }

                await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Logout");
            }
        }

        public IActionResult Logout()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData.Clear();
            return RedirectToAction("Index", "Login");
        }

        public async Task<string[]> SetClaim(ViewLogin login)
        {
            try
            {
                ViewAccEMPLOYEE acc = new ViewAccEMPLOYEE();
                string Email = "";

                if (login.UserId != GlobalVariable.AdminUserName && login.Password != GlobalVariable.AdminPassword)
                {
                    _Cache.clearCacheAccEmployee();
                    acc = await Task.Run(() => _Cache.cacheAccEmployee().FirstOrDefault(s => s.EMP_CODE == login.Empcode));
                    ViewrpEmail Emails = _Cache.cacheEmail().Where(w => w.emEmail_M365 != null && w.emEmpcode.Trim() == acc.EMP_CODE.Trim()).FirstOrDefault();
                    if (Emails is null)
                    {
                        Email = "";
                    }
                    else
                    {
                        Email = _Cache.cacheEmail().Where(w => w.emEmpcode.Trim() == acc.EMP_CODE.Trim()).FirstOrDefault().emEmail_M365.Trim();
                    }
                }
                else
                {
                    acc.EMP_CODE = GlobalVariable.AdminEmpCode;
                    acc.NICKNAME = GlobalVariable.AdminPermission;
                    acc.EMP_TNAME = GlobalVariable.AdminPermission;
                    acc.EMP_ENAME = GlobalVariable.AdminPermission;
                    acc.DIVI_CODE = GlobalVariable.AdminDivision;
                    acc.DEPT_CODE = GlobalVariable.AdminDepartment;
                    acc.SEC_CODE = GlobalVariable.AdminSection;
                    acc.GRP_CODE = GlobalVariable.AdminGroup;
                    acc.UNT_CODE = GlobalVariable.AdminUnit;
                    acc.POS_CODE = GlobalVariable.AdminPosition;
                    acc.PRI_THAI = "";
                    acc.LAST_ENAME = "";
                    Email = GlobalVariable.AdminEmail;
                }

                acc.DIVI_CODE = await Task.Run(() => _Cache.cacheAccDIVIMast().Where(w => w.DIVI_CODE == acc.DIVI_CODE).FirstOrDefault().DIVI_NAME);
                acc.DEPT_CODE = await Task.Run(() => _Cache.cacheDEPTMast().Where(w => w.DEPT_CODE == acc.DEPT_CODE).FirstOrDefault().DEPT_NAME);
                acc.SEC_CODE = await Task.Run(() => _Cache.cacheSECMast().Where(w => w.SEC_CODE == acc.SEC_CODE).FirstOrDefault().SEC_NAME);
                acc.GRP_CODE = await Task.Run(() => _Cache.cacheGRPMast().Where(w => w.GRP_CODE == acc.GRP_CODE).FirstOrDefault().GRP_NAME);

                acc.LAST_TNAME = acc.LAST_TNAME is null ? "" : acc.LAST_TNAME;
                acc.NICKNAME = acc.EMP_ENAME.Substring(0, 1) + (acc.LAST_ENAME == "" ? "" : acc.LAST_ENAME.Substring(0, 1))?.ToString();



                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim("UserId", login.UserId.ToString()));
                claims.Add(new Claim("Password", login.Password.ToString()));
                claims.Add(new Claim("EmpCode", acc.EMP_CODE?.ToString()));
                claims.Add(new Claim("Permission", login.Permission?.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, login.Permission?.ToString()));
                claims.Add(new Claim("Division", acc.DIVI_CODE.ToUpper()));
                claims.Add(new Claim("Department", acc.DEPT_CODE.ToUpper()));
                claims.Add(new Claim("DeptCode", acc.DEPT_CODE.ToUpper()));
                claims.Add(new Claim("Section", acc.SEC_CODE.ToUpper()));
                claims.Add(new Claim("Group", acc.GRP_CODE.ToUpper()));
                claims.Add(new Claim("Unit", acc.UNT_CODE.ToUpper()));
                claims.Add(new Claim("Position", acc.POS_CODE.ToUpper()));
                claims.Add(new Claim("ProgramName", GlobalVariable.ProgramName));
                claims.Add(new Claim("PriName", acc.PRI_THAI));
                claims.Add(new Claim("Name", acc.EMP_TNAME));
                claims.Add(new Claim("SurName", acc.LAST_TNAME));
                claims.Add(new Claim("NICKNAME", acc.NICKNAME.ToUpper()));
                claims.Add(new Claim("Email", Email));
                ClaimsIdentity identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , principal, new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    }); //true is remember login

                string[] stat = { "Ok" };
                return stat;
            }
            catch (Exception ex)
            {
                string[] stat = { "Ng", ex.Message };
                return stat;
            }
        }

        public string FirstCharToUpper(string Text)
        {
            if (Text == null)
                return null;

            if (Text.Length > 0)
                return char.ToUpper(Text[0]) + Text.Substring(1);

            return Text.ToUpper();
        }

        public IActionResult SignOut()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _Cache.clearCacheAccEmployee();
            TempData.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}