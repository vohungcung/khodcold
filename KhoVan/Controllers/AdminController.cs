using ApiOrder.Models;
using CaptchaDotNet2.Security.Cryptography;
using Global;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace MvcApplication5.Controllers
{
    public class AdminController : Controller
    {
        public enum Dimensions
        {
            Width,
            Height
        }
        //phan trang
        private ArrayList GetPage(int nTotal, int PAGE_SHOW_COUNT, int PAGE_SIZE, int CurrentPage)
        {
            ArrayList results = new ArrayList();
            int haft = PAGE_SHOW_COUNT / 2;
            int n = Convert.ToInt32(Math.Ceiling((nTotal * 1.0) / PAGE_SIZE));

            for (int i = 1; i <= n; i++)
            {
                if (i >= CurrentPage - haft && i <= CurrentPage + haft)
                {
                    results.Add(i);
                }
            }
            if (results.Count < PAGE_SHOW_COUNT)
            {
                if (CurrentPage > n / 2)
                {
                    int i = Commons.ConvertToInt(results[0]) - 1;
                    while (i >= 1 && results.Count < PAGE_SHOW_COUNT)
                    {
                        results.Add(i);
                        i--;
                    }
                }
                else
                {
                    int i = Commons.ConvertToInt(results[results.Count - 1]) + 1;
                    while (i <= n && results.Count < PAGE_SHOW_COUNT)
                    {
                        results.Add(i);
                        i++;
                    }
                }
            }
            results.Sort();

            return results;
        }

        private void LoadInfo()
        {
            ViewBag.VUserName = GlobalVariables.UserName;
            ViewBag.UserID = GlobalVariables.UserID;
            if (GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/logout");
            }
        }
        //home
        public ActionResult Index()
        {
            ViewBag.Message = "Chào mừng bạn đến với hệ thống quản trị";

            LoadInfo();
            return View();
        }
        //login
        public ActionResult Login()
        {
            //string html = Xuat(1, "", false);
            //System.IO.File.WriteAllText("d:\\abc.html", html);
            //Gom();
            return View();
        }
        public string Xuat(int cap, string dk, bool over)
        {
            string html = "";
            if (over == false)
            {
                html += "<ul>" + Environment.NewLine;
            }
            string subhtml = "";
            string cot = "c" + cap;
            string ssql = "select " + cot + " from ttt1 where 1=1 " + dk + " group by " + cot;
            ssql += " order by min(tt) ";
            DataTable dt = Commons.GetData(ssql);
            foreach (DataRow item in dt.Rows)
            {
                if (item[0].ToString() != "")
                {
                    html += "<li>" + Environment.NewLine; ;
                    if (cap < 6)
                    {
                        subhtml = Xuat(cap + 1, dk + " and " + cot + " = N'" + Fix(item[0].ToString()) + "'", false);
                        html += "<p>" + item[0] + "</p>";
                        html += subhtml + Environment.NewLine;
                    }
                    else
                    {
                        html += item[0];
                    }

                    html += "</li>" + Environment.NewLine;
                }
                else
                {
                    if (cap < 6)
                    {
                        subhtml = Xuat(cap + 1, dk + " and " + cot + " = N'" + Fix(item[0].ToString()) + "'", true);
                        html += subhtml + Environment.NewLine;
                    }
                    else
                    {
                        html += item[0];
                    }
                }

            }
            if (over == false)
            {
                html += "</ul>";
            }

            return html;
        }
        public void Gom()
        {
            int l = 0;
            string ssql = "select c1,c2,c3,c4,c5,c6,c7,c8 from ttt order by tt ";
            DataTable dt = Commons.GetData(ssql);
            //level1
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                if (r[l].ToString() == "" && i > 0)
                {

                    dt.Rows[i][l] = dt.Rows[i - 1][l];

                }
            }

            //leveln
            for (int c = 1; c < 8; c++)
            {
                l = c;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r[l].ToString() == "" && i > 0)
                    {
                        bool flag = true;
                        for (int j = l - 1; j >= 0; j--)
                        {
                            if (r[j].ToString() != dt.Rows[i - 1][j].ToString())
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            string prev = dt.Rows[i - 1][l].ToString();
                            if (prev != "")
                            {
                                dt.Rows[i][l] = prev;
                            }
                        }
                    }
                }
            }

            Export d = new Export();
            d.ExportExcel(Response, "aaaa", dt);

        }
        //view user
        public ActionResult ViewUsers()
        {
            if (Global.GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();


            return View();
        }
        public ActionResult GetPhoto()
        {

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 12;
            ViewBag.ClientID = Commons.ConvertToString(Request.QueryString["id"]);
            ViewBag.TypeID = Commons.ConvertToString(Request.QueryString["type"]).Replace("'", "");
            DataTable dt = new DataTable();
            dt.Columns.Add("ThumbImage");
            dt.Columns.Add("CreateDate", DateTime.Now.GetType());
            dt.Columns.Add("i", CurrentPage.GetType());
            string[] files = System.IO.Directory.GetFiles(Server.MapPath("~/upload"));
            int row = 0;
            foreach (string file in files)
            {
                if (file.IndexOf(".txt") < 0)
                {
                    DataRow r = dt.NewRow();
                    string file1 = file.Substring(file.LastIndexOf("\\") + 1);
                    string path = "~/upload/" + file1.Substring(0, file1.LastIndexOf(".")) + file1.Substring(file1.LastIndexOf("."));
                    r[0] = path;
                    System.IO.FileInfo f1 = new System.IO.FileInfo(file);
                    r[1] = f1.CreationTime;
                    if (file1.IndexOf(".db", StringComparison.OrdinalIgnoreCase) < 0)
                    {

                        dt.Rows.Add(r);

                    }
                }
            }
            dt.DefaultView.Sort = "CreateDate desc";
            string sWrite = "";
            foreach (DataRowView item in dt.DefaultView)
            {
                sWrite += "insert into Photos(ThumbImage,CreateDate)values(N'" + Commons.Fix(item["ThumbImage"].ToString()) + "',getdate());";

            }
            Commons.ExecuteNoneQuery(sWrite);
            return View();
        }
        //photo management
        private void LoadPhoto()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 12;
            ViewBag.ClientID = Commons.ConvertToString(Request.QueryString["id"]);
            ViewBag.TypeID = Commons.ConvertToString(Request.QueryString["type"]).Replace("'", "");
            int row = 0;
            int nTotal = 0;
            string sSQL = "select * from Photos ";
            if (keyword != "")
            {
                sSQL += " where ThumbImage like N'%" + Commons.Fix(keyword) + "%' ";
            }

            sSQL += " order by id desc";
            DataTable dt = Commons.GetData(sSQL, CurrentPage, PAGE_SIZE, ref nTotal);
            dt.Columns.Add("i", CurrentPage.GetType());


            foreach (DataRow item in dt.Rows)
            {
                item["i"] = row;
                row++;
            }

            ViewBag.Data = dt;

            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());


            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/PhotoManagement?key=" + keyword + "&page=" + e + "&id=" + Commons.ConvertToString(Request.QueryString["id"]) + "&type=" + Commons.ConvertToString(Request.QueryString["type"]).Replace("'", "");
                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                }
            }
            ViewBag.Paging = dp;
        }
        /// <summary>
        //bo anh
        /// </summary>
        /// <returns></returns>
        public ActionResult PhotoManagement()
        {
            LoadPhoto();
            return View();
        }
        [HttpPost]
        public ActionResult PhotoManagement(HttpPostedFileBase FileUpload)
        {
            if (FileUpload != null)
            {
                string filename = Commons.ConvertToURLName(FileUpload.FileName.Substring(0, FileUpload.FileName.IndexOf(".")));
                string ext = FileUpload.FileName.Substring(FileUpload.FileName.LastIndexOf("."));
                int i = 1;
                string f = "~/upload/" + filename + ext;

                while (System.IO.File.Exists(Server.MapPath(f)))
                {
                    f = "~/upload/" + filename + i + ext;
                    i++;
                }

                FileUpload.SaveAs(Server.MapPath(f));
                string imageFile = Server.MapPath(f);
                ResizeImage(imageFile);
                string sWrite = "insert into Photos(ThumbImage,CreateDate)values(N'" + Commons.Fix(f) + "',getdate())";
                //Commons.WriteToLog(sWrite);


                Commons.ExecuteNoneQuery(sWrite);
            }

            LoadPhoto();

            return View();
        }
        private void ResizeImage(string imageFile)
        {
            string Path = imageFile.Substring(0, imageFile.LastIndexOf("\\"));
            string filename = imageFile.Substring(imageFile.LastIndexOf("\\") + 1);

            Bitmap photo = new Bitmap(imageFile);
            Bitmap thumbPhoto = ResizeImage(photo, 250, Dimensions.Width);
            string thumb = Path + "\\resize";
            if (System.IO.Directory.Exists(thumb) == false)
            {
                System.IO.Directory.CreateDirectory(thumb);
            }

            thumbPhoto.Save(thumb + "\\" + filename);
            photo.Dispose();
            thumbPhoto.Dispose();
            photo = new Bitmap(thumb + "\\" + filename);
            photo.Save(imageFile);
            photo.Dispose();

            System.IO.File.Delete(thumb + "\\" + filename);

        }
        public static Bitmap ResizeImage(Bitmap bitmapImage, int newSize, Dimensions dimension)
        {
            int sourceWidth = bitmapImage.Width;
            int sourceHeight = bitmapImage.Height;
            float ratio = 1;

            if (dimension == Dimensions.Width)
            {
                ratio = ((float)newSize / (float)sourceWidth);
            }
            else
            {
                ratio = ((float)newSize / (float)sourceHeight);
            }

            Bitmap resultBitmap;
            Size resultSize = new Size((int)(sourceWidth * ratio), (int)(sourceHeight * ratio));

            resultBitmap = new Bitmap(resultSize.Width, resultSize.Height, PixelFormat.Format16bppRgb555);
            System.Drawing.Graphics graphic = Graphics.FromImage(resultBitmap);

            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.High;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect = new Rectangle(0, 0, resultSize.Width, resultSize.Height);
            graphic.DrawImage(bitmapImage, rect, 0, 0, bitmapImage.Width, bitmapImage.Height, GraphicsUnit.Pixel);

            return resultBitmap;
        }
        //xoa tai khoan
        [HttpPost]
        public ActionResult DeleteUser(int aid)//xoa user
        {
            if (Global.GlobalVariables.UserID == 0 || Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Chưa đăng nhập. hoặc không có quyền", success = false });
            }

            if (Global.GlobalVariables.UserID == aid)
            {
                return Json(new { errorMsg = "Bạn không thể xóa chính mình", success = false });
            }

            Exception ex = null;
            string[] l = { "@AdminID" };
            object[] lv = { aid };
            DbType[] ts = { DbType.Int32 };


            bool result = Commons.ExecuteNoneQueryP("SP_DeleteAdmin", l, lv, ts, ref ex);

            if (result)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không thể xóa", success = false });
            }
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (LoginAdmin(username, password))
            {
                Response.Redirect("~/admin");
            }
            else
            {
                ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng.";
            }

            return View();
        }
        public bool LoginAdmin(string UserName, string Password)
        {

            string ssql = "select adminid,username,divisionid from admins where username like N'" + Commons.Fix(UserName) + "' and Password=N'" + Commons.Fix(Encryptor.Encrypt(Password)) + "'";

            DataTable dt = Commons.GetDataFromOtherDataBase(ssql, Commons.WUSERConnectionString);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                GlobalVariables.UserID = Commons.ConvertToInt(r[0]);
                GlobalVariables.UserName = Commons.ConvertToString(r[1]);
                GlobalVariables.DivisionID = r[2].ToString();
                GlobalVariables.CN = r[2].ToString();//xac dinh ben mien nao de dinh database
                GlobalVariables.FullName = null;

                SetLoginTime(Commons.ConvertToInt(r[0]));
                return true;
            }
            return false;
        }
        public void SetLoginTime(int U)
        {
            string sSQL = "exec SP_UpdateLogin " + U.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            GlobalVariables.TimeLogin = dt.Rows[0][0].ToString();

        }
        public ActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string password, string repassword)
        {
            if (password != repassword)
            {
                return Json(new { errorMsg = "Mật khẩu và mật khẩu nhập lại không khớp", success = false });
            }

            string sWrite = "Update Admins set password='" + Commons.Fix(Encryptor.Encrypt(password)) + "' ";
            sWrite += " where AdminID=" + Global.GlobalVariables.UserID.ToString();
            if (password.Trim() == "")
            {
                ViewBag.MessageError = "Bạn chưa nhập mật khẩu.";
                ViewBag.MessageSuccess = "";
                return View();
            }
            if (repassword.Trim() != password.Trim())
            {
                ViewBag.MessageError = "Mật khẩu nhập lại không khớp.";
                ViewBag.MessageSuccess = "";
                return View();
            }
            bool result = Commons.ExecuteNoneQuery(sWrite);
            if (result)
            {
                ViewBag.MessageSuccess = "Cập nhật thành công.";
                ViewBag.MessageError = "";
            }
            else
            {
                ViewBag.MessageError = "Có lỗi trong quá trình cập nhật.";
                ViewBag.MessageSuccess = "";
            }
            return View();
        }


        [HttpPost]
        public ActionResult ChangePasswordA(string password, string repassword)
        {
            if (Global.GlobalVariables.UserID == 0 || Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Không có quyền", success = false });
            }

            if (password != repassword)
            {
                return Json(new { errorMsg = "Mật khẩu và mật khẩu nhập lại không khớp", success = false });
            }

            int AdminID = Commons.ConvertToInt(Request.QueryString["id"]);
            string[] l = { "@AdminID", "@Password" };
            object[] lv = { AdminID, Encryptor.Encrypt(password) };
            DbType[] ts = { DbType.Int32, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_ChangePassword", l, lv, ts, ref ex);

            if (r)
            {
                return Json(new { msg = "Đổi mật khẩu thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult ChangeMyPassword(string OldPassword, string MyPassword, string MyRePassword)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Chưa đăng nhập", success = false });
            }

            if (CheckOldPassword(OldPassword) == false)
            {
                return Json(new { errorMsg = "Mật khẩu cũ không đúng", success = false });

            }

            if (MyPassword != MyRePassword)
            {
                return Json(new { errorMsg = "Mật khẩu và mật khẩu nhập lại không khớp", success = false });
            }

            string[] l = { "@AdminID", "@Password" };
            object[] lv = { Global.GlobalVariables.UserID, Encryptor.Encrypt(MyPassword) };
            DbType[] ts = { DbType.Int32, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_ChangePassword", l, lv, ts, ref ex);

            if (r)
            {
                return Json(new { msg = "Đổi mật khẩu thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public ActionResult Logout()
        {

            GlobalVariables.UserID = 0;
            GlobalVariables.UserName = "";
            GlobalVariables.DivisionID = "";
            GlobalVariables.FullName = null;
            GlobalVariables.CN = "";
            Response.Redirect("~/admin/login");

            return View();
        }
        public ActionResult NotPermit()
        {

            LoadInfo();
            return View();
        }



        [HttpPost]
        public ActionResult Get_Users()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);


            string sSQL = "exec GetAdmin N'" + Commons.Fix(keyword) + "'";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            AdminID = Commons.ConvertToInt(p["AdminID"]),
                            FullName = Commons.ConvertToString(p["FullName"]),
                            Email = Commons.ConvertToString(p["Email"]),
                            UserName = Commons.ConvertToString(p["UserName"]),
                            Phone = Commons.ConvertToString(p["Phone"]),
                            IsAdmin = Commons.ConvertToBool(p["IsAdmin"]),
                            DivisionID = Commons.ConvertToString(p["DivisionID"]),
                            Z = (Commons.ConvertToBool(p["IsAdmin"]) == true ? "IsAdmin" : "")

                        };
            return Json(query);
        }



        [HttpPost]
        public ActionResult Update_User(string FullName, string DivisionID, string Phone, string Z)
        {
            int AdminID = Commons.ConvertToInt(Request.QueryString["id"]);

            string[] l = { "@AdminID", "@FullName", "@Phone", "@IsAdmin", "@DivisionID" };
            object[] lv = { AdminID, FullName, Phone, (Z == "IsAdmin" ? true : false), DivisionID };
            DbType[] ts = { DbType.Int32, DbType.String, DbType.String, DbType.Boolean, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_UpdateAdmin", l, lv, ts, ref ex);

            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult Add_User(string DivisionID, string FullName, string Email, string PassWord, string RePassword, string Phone, string UserName, string Z)
        {
            if (RePassword != PassWord)
            {
                return Json(new { errorMsg = "Mật khẩu xác nhận lại không đúng", success = false });

            }

            if (Commons.UserIsExists(UserName))
            {
                return Json(new { errorMsg = "User này đã có rồi", success = false });

            }


            Exception ex = null;


            string[] l = { "@FullName", "@Phone", "@UserName", "@PassWord", "@Email", "@IsAdmin", "@DivisionID" };
            object[] lv = { FullName, Phone, UserName, Encryptor.Encrypt(PassWord), Email, (Z == "IsAdmin" ? true : false), DivisionID };
            DbType[] ts = { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.Boolean, DbType.String };

            Commons.ExecuteNoneQueryP("sp_insertadmin", l, lv, ts, ref ex);
            if (ex == null)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }







        private string MDY(string v)
        {
            if (v.Trim() == "")
            {
                return "";
            }

            string[] d = v.Split('.');
            return d[2] + "/" + d[1] + "/" + d[0];
        }





        public ActionResult Setting()
        {
            LoadInfo();




            return View();
        }



        public ActionResult SetPermit()
        {
            if (Global.GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string sSQL = "select S.ScreenID,S.ScreenName,Isnull(P.AdminID,0) CoQuyen from Screens S ";
            sSQL += " left join Permits P on S.ScreenID=P.ScreenID ";
            sSQL += " and P.AdminID=" + Commons.ConvertToInt(Request.QueryString["id"]).ToString();
            sSQL += " order by S.ScreenName ";
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.Data = dt;
            int AdminID = Commons.ConvertToInt(Request.QueryString["id"]);
            DataTable db = Commons.GetData("select FullName from Admins where AdminID=" + AdminID.ToString());
            if (db.Rows.Count == 0)
            {
                Response.Redirect("~/admin/logout");
                return View();
            }

            ViewBag.Data = dt;
            ViewBag.FullName = db.Rows[0][0].ToString();
            return View();
        }
        [HttpPost]
        public ActionResult AddOrRemovePermit(string ScreenID, int AdminID, int CoQuyen)
        {
            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không phải admin nên không có quyền này", success = false });
            }

            string sWrite = "";
            if (CoQuyen == 0)
            {
                sWrite = "delete Permits where ScreenID='" + Commons.Fix(ScreenID) + "' and AdminID=" + AdminID.ToString();
            }
            else
            {
                sWrite = "insert into Permits(ScreenID,AdminID) values('" + Commons.Fix(ScreenID) + "' , " + AdminID.ToString() + ")";
            }

            sWrite += ";";


            Commons.ExecuteNoneQuery(sWrite);
            return Json(new { msg = "Cập nhật thành công", success = true });
        }
        public ActionResult tatmocanhbao(int mo)
        {

            string sWrite = "";
            if (mo == 0)
            {
                sWrite = "delete Permits where ScreenID='canhbaopickhang' and AdminID=" + GlobalVariables.UserID.ToString();
            }
            else
            {
                sWrite = "insert into Permits(ScreenID,AdminID) values('canhbaopickhang' , " + GlobalVariables.UserID.ToString() + ")";
            }

            sWrite += ";";


            Commons.ExecuteNoneQuery(sWrite);
            return Json(new { msg = "Cập nhật thành công", success = true });
        }
        public ActionResult SetGroup()
        {
            if (Global.GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();

            int AdminID = Commons.ConvertToInt(Request.QueryString["id"]);
            DataTable db = Commons.GetData("select FullName,NhomVT from Admins where AdminID=" + AdminID.ToString());
            if (db.Rows.Count == 0)
            {
                ViewBag.Data = new DataTable();
                Response.Redirect("~/admin/logout");
                return View();
            }
            ViewBag.FullName = db.Rows[0][0].ToString();
            string NhomVT = Commons.ConvertToString(db.Rows[0][1]);

            DataTable dt = new DataTable();
            dt.Columns.Add("ScreenID");
            dt.Columns.Add("ScreenName");
            bool b = true;
            dt.Columns.Add("CoQuyen", b.GetType());

            string sSQL = "select ItemGroupID,ItemGroupName from ItemGroups order by ItemGroupName ";

            DataTable ItemGroups = Commons.GetData(sSQL);
            foreach (DataRow item in ItemGroups.Rows)
            {
                DataRow r = dt.NewRow();
                r[0] = item[0];
                r[1] = item[1];
                if (NhomVT.IndexOf(item[0].ToString()) >= 0)
                {
                    r[2] = true;
                }
                else
                {
                    r[2] = false;
                }

                dt.Rows.Add(r);
            }

            ViewBag.Data = dt;


            return View();
        }
        [HttpPost]
        public ActionResult AddOrRemovePermitGroup(string ScreenID, int AdminID, int CoQuyen)
        {
            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không phải admin nên không có quyền này", success = false });
            }
            DataTable db = Commons.GetData("select NhomVT from Admins where AdminID=" + AdminID.ToString());
            string NhomVT = Commons.ConvertToString(db.Rows[0][0]);
            string[] data = NhomVT.Split(',');
            string th = "";
            foreach (string item in data)
            {
                if (item.ToLower() == ScreenID.ToLower())
                {
                    if (CoQuyen == 0)
                    {
                    }
                    else
                    {
                        if (th == "")
                        {
                            th = item;
                        }
                        else
                        {
                            th += "," + item;
                        }

                    }
                }
                else
                {

                    if (th == "")
                    {
                        th = item;
                    }
                    else
                    {
                        th += "," + item;
                    }
                }

            }
            string sWrite = "update Admins set NhomVT=N'" + th + "' where AdminID=" + AdminID.ToString("0");


            sWrite += ";";


            Commons.ExecuteNoneQuery(sWrite);
            return Json(new { msg = "Cập nhật thành công", success = true });
        }
        public string Protocol
        {
            get
            {
                if (Request.Url.ToString().ToLower().IndexOf("https") >= 0)
                {
                    return "https://";
                }

                return "http://";
            }
        }
        public string HttpGetCurrentDomain
        {
            get
            {
                string url = Request.Url.ToString().ToLower();
                int l = "https://".Length + 1;

                int p = url.IndexOf("/", l);
                string sResult = url.Substring(0, p);
                return sResult;
            }
        }

        private bool CheckOldPassword(string PasswordCheck)
        {
            string sSQL = "select Password from Admins where AdminID=" + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {

                return false;
            }
            string OldPassword = dt.Rows[0][0].ToString();
            return Encryptor.Encrypt(PasswordCheck) == OldPassword;

        }
        //MC


        private bool IsImage(string file)
        {
            return Commons.IsImage(file);
        }
        //danh sach thu muc

        /// <returns></returns>
        public ActionResult MultiFileUpload()
        {

            return View();
        }
        [HttpPost]
        public ActionResult MultiFileUpload(HttpPostedFileBase[] FileUpload, string Title)
        {
            int DirID = Commons.ConvertToInt(Request.QueryString["id"]);
            if (DirID == 0)
            {
                ViewBag.Message = "Bạn chưa chọn thư mục";
                return View();
            }
            if (Commons.ConvertToString(Title) == "")
            {
                ViewBag.Message = "Bạn chưa nhập tiêu đề";
                return View();
            }
            if (FileUpload.Length == 0)
            {
                ViewBag.Message = "Bạn chưa chọn file";
                return View();
            }

            foreach (HttpPostedFileBase item in FileUpload)
            {
                SaveOneFile(item, Title, DirID);

            }

            return View();
        }
        private void SaveOneFile(HttpPostedFileBase FileUpload, string Title, int DirID)
        {


            if (FileUpload != null)
            {
                string filename = Commons.ConvertToURLName(FileUpload.FileName.Substring(0, FileUpload.FileName.IndexOf(".")));
                string ext = FileUpload.FileName.Substring(FileUpload.FileName.LastIndexOf("."));
                int i = 1;
                string f = "~/files/" + filename + ext;

                while (System.IO.File.Exists(Server.MapPath(f)))
                {
                    f = "~/files/" + filename + i + ext;
                    i++;
                }

                FileUpload.SaveAs(Server.MapPath(f));
                string imageFile = Server.MapPath(f);
                if (IsImage(imageFile))
                {
                    CreateThumb(imageFile);
                }

                string sWrite = "";

                sWrite += ";exec [SP_InsertDoc] " + DirID.ToString();
                sWrite += " ,N'" + Commons.Fix(Title) + "'";
                sWrite += " ,N'" + Commons.Fix(f) + "'";
                sWrite += " ," + Global.GlobalVariables.UserID.ToString("0");
                Commons.ExecuteNoneQuery(sWrite);


            }

        }


        public void CreateThumb(string imageFile)
        {
            if (System.IO.File.Exists(imageFile) == false)
            {
                return;
            }

            try
            {
                string Path = imageFile.Substring(0, imageFile.LastIndexOf("\\"));
                string filename = imageFile.Substring(imageFile.LastIndexOf("\\") + 1);

                Bitmap photo = new Bitmap(imageFile);
                Bitmap thumbPhoto = ResizeImage(photo, 250, Dimensions.Width);
                string thumb = Path + "\\Thumbs";
                if (System.IO.Directory.Exists(thumb) == false)
                {
                    System.IO.Directory.CreateDirectory(thumb);
                }

                thumbPhoto.Save(thumb + "\\" + filename);
                photo.Dispose();
                thumbPhoto.Dispose();

            }
            catch
            {

            }


        }

        public ActionResult ViewItemVolumes()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("ViewItemVolumes") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "exec SP_GetCountOfItemVolume N'" + Commons.Fix(keyword) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewItemVolumes?key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewItemVolumes?key=" + keyword + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewItemVolumes?key=" + keyword + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }

        [HttpPost]
        public ActionResult Get_ItemVolumes()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;

            string sSQL = "exec SP_GetItemVolumes N'" + Commons.Fix(keyword) + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ItemID = p["ItemID"].ToString(),
                            ItemName = Commons.ConvertToString(p["ItemName"]),
                            UnitID = Commons.ConvertToString(p["UnitID"]),
                            ItemGroupID = Commons.ConvertToString(p["ItemGroupID"]),
                            Length = Commons.ConvertToDecimal(p["Length"]),
                            Width = Commons.ConvertToDecimal(p["Width"]),
                            Height = Commons.ConvertToDecimal(p["Height"]),
                            Box = Commons.ConvertToInt(p["Box"]),
                            Cm3 = Commons.ConvertToDecimal(p["Cm3"]),
                            BarCode = Commons.ConvertToString(p["BarCode"]),
                            Status = (Commons.ConvertToBool(p["Used"]) ? "" : "Khóa")
                        };
            return Json(query);
        }



        [HttpPost]
        public ActionResult Update_ItemVolume(string ItemGroupID, string ItemName, decimal Length, decimal Width, decimal Height, int Box, decimal Cm3, string BarCode, string UnitID)
        {
            BarCode = BarCode.ToUpper();

            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewItemVolumes") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);


            string[] l = { "@ItemID", "@ItemName", "@Length", "@Width", "@Height", "@Box", "@Cm3", "@BarCode", "@UnitID", "@ItemGroupID" };
            object[] lv = { ItemID, ItemName, Length, Width, Height, Box, Cm3, BarCode, UnitID, ItemGroupID };
            DbType[] ts = { DbType.String, DbType.String, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Int32, DbType.Decimal, DbType.String, DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_UpdateItemVolume", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult Add_ItemVolume(string ItemGroupID, string ItemID, string ItemName, decimal Length, decimal Width, decimal Height, int Box, decimal Cm3, string BarCode, string UnitID)
        {
            BarCode = BarCode.ToUpper();

            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewItemVolumes") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@ItemID", "@ItemName", "@Length", "@Width", "@Height", "@Box", "@Cm3", "@BarCode", "@UnitID", "@ItemGroupID" };
            object[] lv = { ItemID, ItemName, Length, Width, Height, Box, Cm3, BarCode, UnitID, ItemGroupID };
            DbType[] ts = { DbType.String, DbType.String, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Int32, DbType.Decimal, DbType.String, DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_InsertItemVolume", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        //xoa doc
        [HttpPost]
        public ActionResult DeleteItemVolume(string ItemID)//xoa doc
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewItemVolumes") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            decimal FileID = Commons.ConvertToDecimal(Request.QueryString["id"]);
            Exception ex = null;
            string sWrite = "delete ItemVolumes where ItemID = N'" + Commons.Fix(ItemID) + "'";
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không thể xóa", success = false });
            }
        }

        public ActionResult ViewLocations()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("ViewLocations") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string DivisionID = Commons.ConvertToString(Request.QueryString["type"]);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "exec SP_GetCountOfLocation N'" + Commons.Fix(DivisionID) + "',N'" + Commons.Fix(Keyword) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 40;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewLocations?key=" + Keyword + "&type=" + DivisionID + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewLocations?key=" + Keyword + "&type=" + DivisionID + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewLocations?key=" + Keyword + "&type=" + DivisionID + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);

                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }

        [HttpPost]
        public ActionResult Get_Locations()
        {
            string DivisionID = Commons.ConvertToString(Request.QueryString["type"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 40;

            string sSQL = "exec SP_GetLocations N'" + Commons.Fix(DivisionID) + "',N'" + Commons.Fix(Keyword) + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Location = p["Location"].ToString(),
                            Volume = Convert.ToDecimal(p["Volume"]),
                            UuTien = Convert.ToInt32(p["UuTien"]),
                            VolumeUsed = Convert.ToDecimal(p["VolumeUsed"]),
                            MinVolume = Convert.ToDecimal(p["MinVolume"]),
                            Temp = (Commons.ConvertToBool(p["Temp"]) ? 1 : 0),
                            Odd = (Commons.ConvertToBool(p["Odd"]) ? 1 : 0),
                            NotAuto = (Commons.ConvertToBool(p["NotAuto"]) ? 1 : 0),
                            Rack = (Commons.ConvertToBool(p["Rack"]) ? 1 : 0),
                            Status = (Commons.ConvertToBool(p["Temp"]) ? "Tạm - " : "bình thường - ") + (Commons.ConvertToBool(p["Odd"]) ? "Kệ lẻ" : "Kệ chẳn"),
                            LockedForIn = (Commons.ConvertToBool(p["LockedForIn"]) ? 1 : 0),
                            LockedForOut = (Commons.ConvertToBool(p["LockedForOut"]) ? 1 : 0),

                        };
            return Json(query);
        }



        [HttpPost]
        public ActionResult Update_Location(string Location, decimal Volume, decimal MinVolume, int Temp, int Odd, int LockedForIn, int LockedForOut, int NotAuto, int UuTien, int Rack)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewLocations") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string DivisionID = Commons.ConvertToString(Request.QueryString["type"]);

            string[] l = { "@DivisionID", "@Location", "@Volume", "@MinVolume", "@Temp", "@Odd", "@LockedForIn", "@LockedForOut", "@UuTien", "@NotAuto", "@Rack" };
            object[] lv = { DivisionID, Location.ToUpper(), Volume, MinVolume, Temp == 1, Odd == 1, LockedForIn == 1, LockedForOut == 1, UuTien, NotAuto == 1, Rack == 1 };
            DbType[] ts = { DbType.String, DbType.String, DbType.Decimal, DbType.Decimal, DbType.Boolean, DbType.Boolean, DbType.Boolean, DbType.Boolean, DbType.Int32, DbType.Boolean, DbType.Boolean };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_UpdateLocation", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult Add_Location(string Location, decimal Volume, decimal MinVolume, int Temp, int Odd,
            int NotAuto, int UuTien, int Rack)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewLocations") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string DivisionID = Commons.ConvertToString(Request.QueryString["type"]);

            string[] l = { "@DivisionID", "@Location", "@Volume", "@MinVolume", "@Temp", "@Odd", "@UuTien", "@NotAuto", "@Rack" };
            object[] lv = { DivisionID, Location.ToUpper(), Volume, MinVolume, Temp == 1, Odd == 1, UuTien, NotAuto == 1, Rack == 1 };
            DbType[] ts = { DbType.String, DbType.String, DbType.Decimal, DbType.Decimal, DbType.Boolean, DbType.Boolean, DbType.Int32, DbType.Boolean, DbType.Boolean };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_InsertLocation", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        //xoa doc
        [HttpPost]
        public ActionResult DeleteLocation(string DivisionID, string Location)//xoa doc
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewLocations") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }



            decimal FileID = Commons.ConvertToDecimal(Request.QueryString["id"]);

            string sWrite = "exec [SP_DeleteLocation] N'" + Commons.Fix(DivisionID) + "',";
            sWrite += "N'" + Commons.Fix(Location) + "' ";
            DataTable dt = Commons.GetData(sWrite);
            int Result = Commons.ConvertToInt(dt.Rows[0][0]);
            string M = Commons.ConvertToString(dt.Rows[0][1]);
            if (Result == 1)
            {
                return Json(new { msg = M, success = true });
            }
            else
            {
                return Json(new { errorMsg = M, success = false });
            }
        }




        public ActionResult ViewDivisions()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();

            return View();
        }

        [HttpPost]
        public ActionResult Get_Divisions()
        {
            string sSQL = "exec SP_GetDivisions ";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            DivisionID = Commons.ConvertToString(p["DivisionID"]),
                            DivisionName = Commons.ConvertToString(p["DivisionName"]),
                            Address = Commons.ConvertToString(p["Address"]),
                            Phone = Commons.ConvertToString(p["Phone"]),
                            Site = Commons.ConvertToString(p["Site"]),
                            ConnectionString = Commons.ConvertToString(p["ConnectionString"])

                        };
            return Json(query);
        }



        [HttpPost]
        public ActionResult Update_Division(string DivisionID, string DivisionName, string Address, string Site, string Phone)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@DivisionID", "@DivisionName", "@Address", "@Site", "@Phone" };
            object[] lv = { DivisionID, DivisionName, Address, Site, Phone };
            DbType[] ts = { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_UpdateDivision", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult Add_Division(string DivisionID, string DivisionName, string Address, string Site, string Phone)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@DivisionID", "@DivisionName", "@Address", "@Site", "@Phone" };
            object[] lv = { DivisionID, DivisionName, Address, Site, Phone };
            DbType[] ts = { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_InsertDivision", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        //xoa doc
        [HttpPost]
        public ActionResult DeleteDivision(string DivisionID)//xoa doc
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            DataTable dt = Commons.GetData("exec SP_CheckDivisionIsUsed N'" + Commons.Fix(DivisionID) + "'");
            if (Convert.ToInt32(dt.Rows[0][0]) == 1)
            {
                return Json(new { errorMsg = "Đơn vị này đã được sử dụng rồi. Bạn không thể xóa ", success = false });
            }
            Exception ex = null;
            string sWrite = "exec [SP_DeleteDivision] N'" + Commons.Fix(DivisionID) + "'";
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không thể xóa " + ex.Message, success = false });
            }
        }




        public ActionResult ViewItemGroups()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("ViewItemGroups") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "exec SP_GetItemGroupCount N'" + Commons.Fix(keyword) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewItemGroups?key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }

        [HttpPost]
        public ActionResult Get_ItemGroups()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;

            string sSQL = "exec SP_GetItemGroups N'" + Commons.Fix(keyword) + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ItemGroupID = p["ItemGroupID"],
                            ItemGroupName = p["ItemGroupName"]
                        };
            return Json(query);
        }



        [HttpPost]
        public ActionResult Update_ItemGroup(string ItemGroupID, string ItemGroupName)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewItemGroups") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@ItemGroupID", "@ItemGroupName" };
            object[] lv = { ItemGroupID, ItemGroupName };
            DbType[] ts = { DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_UpdateItemGroup", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult Add_ItemGroup(string ItemGroupID, string ItemGroupName)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewItemGroups") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@ItemGroupID", "@ItemGroupName" };
            object[] lv = { ItemGroupID, ItemGroupName };
            DbType[] ts = { DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_UpdateItemGroup", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        //xoa doc
        [HttpPost]
        public ActionResult DeleteItemGroup(string ItemGroupID)//xoa 
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewItemGroups") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            Exception ex = null;
            string sWrite = "delete ItemGroups where ItemGroupID = N'" + Commons.Fix(ItemGroupID) + "'";
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không thể xóa", success = false });
            }
        }

        public string NewPallet()
        {
            string sSQL = "exec [GetPalletID] '" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            string PalletID = dt.Rows[0][0].ToString();
            return PalletID;
        }
        public string OnlyNewPallet()
        {
            string sSQL = "exec [GetNewPalletID] '" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            string PalletID = dt.Rows[0][0].ToString();
            return PalletID;
        }
        public ActionResult InputW()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("InputW") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string sSQL = "";
            string PalletID = Commons.ConvertToString(Request.QueryString["id"]);
            DataTable dt = new DataTable();
            if (PalletID == "")
            {

                PalletID = NewPallet();
                GlobalVariables.PalletID = PalletID;
                Response.Redirect("~/admin/InputW?id=" + PalletID);

            }
            sSQL = "select OrderNo,CreateDate,isnull(Location,'') Location,Active,OutBound,Finish,Description from Pallets where PalletID='" + Commons.Fix(PalletID) + "'";
            sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.OrderNo = Commons.ConvertToInt(dt.Rows[0]["OrderNo"]).ToString("N0");
                ViewBag.diengiai = Commons.ConvertToString(dt.Rows[0]["Description"]);
                ViewBag.CreateDate = Commons.ConvertToDateTime(dt.Rows[0]["CreateDate"]).ToString("dd/MM/yyyy");
                string Location = dt.Rows[0]["Location"].ToString();
                if (Commons.ConvertToBool(dt.Rows[0]["Finish"]))
                {
                    ViewBag.finished = 1;
                }
                else
                {
                    ViewBag.finished = 0;
                }

                if (Location != "")
                {
                    ViewBag.Location = Location;
                    ViewBag.exists = true;
                }

                else
                {
                    ViewBag.Location = "Chưa có";
                    ViewBag.exists = false;
                }
                ViewBag.OutBound = Commons.ConvertToString(dt.Rows[0]["OutBound"]);
                ViewBag.active = Commons.ConvertToBool(dt.Rows[0]["Active"]);

            }

            List<CC> ds = new List<CC>();
            sSQL = "exec [SP_GetPalletDetail] '" + Commons.Fix(PalletID) + "','" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            dt = Commons.GetData(sSQL);
            int TotalQuantity = 0;
            foreach (DataRow item in dt.Rows)
            {
                CC i = new CC();
                i.i = item["ItemID"].ToString();
                i.d = item["ItemName"].ToString();
                i.q = Convert.ToInt32(item["Quantity"]);
                i.u = Commons.ConvertToString(item["UnitID"]);
                i.lsx = Commons.ConvertToString(item["LSX"]);
                i.outbound = Commons.ConvertToString(item["OutBound"]);
                i.kp = Commons.ConvertToBool(item["IsOff"]);
                if (i.kp)
                {
                    i.status = "Kém phẩm";
                }
                TotalQuantity += i.q;
                ds.Add(i);
            }


            ViewBag.TotalQuantity = TotalQuantity.ToString("N0");
            ViewBag.data = ds;

            ViewBag.PalletID = PalletID;
            sSQL = "select BarCode from BarCodeFromPallets ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and PalletID='" + Commons.Fix(PalletID) + "'";
            string b = "";
            dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                b = b + item[0].ToString() + ",";
            }
            b = b.Trim(',');
            ViewBag.barcode = b;
            return View();
        }

        public CC GetFromItem(string ItemID)
        {
            CC result = new CC();
            result.i = "";
            string sSQL = "select ItemID,ItemName,UnitID from ItemVolumes where ItemID=N'" + Commons.Fix(ItemID) + "'";
            sSQL += " and Used=1 ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {

                result.i = Commons.ConvertToString(dt.Rows[0]["ItemID"]);
                result.d = Commons.ConvertToString(dt.Rows[0]["ItemName"]);
                result.u = Commons.ConvertToString(dt.Rows[0]["UnitID"]);
            }
            return result;
        }

        public string GetBarCodeFromItem(string ItemID)
        {
            string sResult = "";
            string sSQL = "select BarCode from ItemVolumes where ItemID=N'" + Commons.Fix(ItemID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                sResult = dt.Rows[0][0].ToString();
            }

            return sResult;
        }

        public CC GetFrom18(string BarCode)
        {
            BarCode = BarCode.ToUpper();

            CC result = new CC();
            result.i = "";
            string sSQL = "select top 1 ItemID,ItemName,UnitID from ItemVolumes where BarCode=N'" + Commons.Fix(BarCode) + "'";
            sSQL += " and Used=1 ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {

                result.i = Commons.ConvertToString(dt.Rows[0]["ItemID"]);
                result.d = Commons.ConvertToString(dt.Rows[0]["ItemName"]);
                result.u = Commons.ConvertToString(dt.Rows[0]["UnitID"]);
            }
            return result;
        }


        public List<CC> GetFrom1811(string BarCode)
        {
            BarCode = BarCode.ToUpper();

            List<CC> result = new List<CC>();
            string sSQL = "select ItemID,ItemName,UnitID from ItemVolumes ";
            sSQL += " where left(right(BarCode,11),5) = N'" + Commons.Fix(BarCode.Substring(7, 5)) + "'";
            sSQL += " and SUBSTRING(barcode,14,2)='" + BarCode.Substring(13, 2) + "' ";
            sSQL += " and SUBSTRING(barcode,16,2)='" + BarCode.Substring(15, 2) + "' ";
            sSQL += " and Used=1 ";
            DataTable dt = Commons.GetData(sSQL);
            int Q = int.Parse(BarCode.Substring(BarCode.Length - 1, 1));

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    CC i = new CC();
                    i.i = Commons.ConvertToString(item["ItemID"]);
                    i.d = Commons.ConvertToString(item["ItemName"]);
                    i.u = Commons.ConvertToString(item["UnitID"]);
                    i.q = Q;
                    result.Add(i);
                }

            }
            return result;
        }

        public CC GetFrom27(string BarCode)
        {
            BarCode = BarCode.ToUpper();

            CC result = new CC();
            result.i = "";
            string sSQL = "select ItemID,ItemName,UnitID from ItemVolumes ";
            sSQL += " where left(BarCode,12)+left(right(BarCode,5),4) like N'" + Commons.Fix(BarCode.Substring(0, 16)) + "%'";
            sSQL += " and Used=1 ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {

                result.i = Commons.ConvertToString(dt.Rows[0]["ItemID"]);
                result.d = Commons.ConvertToString(dt.Rows[0]["ItemName"]);
                result.u = Commons.ConvertToString(dt.Rows[0]["UnitID"]);
                result.lsx = BarCode.Substring(16, 6);
                if (Commons.ConvertToString(Session["barcode"]).IndexOf(BarCode) < 0)
                {
                    Session["barcode"] = Commons.ConvertToString(Session["barcode"]) + "," + BarCode;
                }
            }

            return result;
        }
        public List<CC> GetFromHappyBitis(string BarCode)
        {
            //mau moi
            BarCode = BarCode.ToUpper();

            try
            {
                if (BarCode.Substring(14, 1) == "H")
                {
                    return GetFromHappyBitisMauMoi(BarCode);
                }
                else
                {
                    return GetFromHappyBitisMauCu(BarCode);
                }
            }
            catch (Exception ex)
            {


            }
            return new List<CC>();
        }
        public List<CC> GetFromHappyBitisMauMoi(string BarCode)
        {
            BarCode = BarCode.ToUpper();

            List<CC> result = new List<CC>();
            string Tem = BarCode.Substring(0, 14);
            string Loai = BarCode.Substring(14, 2);
            string SL = BarCode.Substring(23, 1);
            int Quantity = Commons.ConvertToInt(SL);
            string LSX = BarCode.Substring(15, 6);
            string sSQL = "select ItemID,ItemName,UnitID from ItemVolumes ";
            sSQL += " where left(BarCode,12)+left(right(BarCode,5),4) like N'" + Commons.Fix(Tem) + "%'";
            sSQL += " and Used=1 ";
            sSQL += " order by ItemID ";
            DataTable dt = Commons.GetData(sSQL);


            if (Loai == "HM")
            {
                if (dt.Rows.Count != 6)
                {
                    return new List<CC>();

                }
            }
            else
            {
                if (dt.Rows.Count != 5)
                {
                    return new List<CC>();
                }
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CC r = new CC();
                r.i = Commons.ConvertToString(dt.Rows[i]["ItemID"]);
                r.d = Commons.ConvertToString(dt.Rows[i]["ItemName"]);
                r.u = Commons.ConvertToString(dt.Rows[i]["UnitID"]);
                r.q = Quantity;
                r.lsx = LSX;
                result.Add(r);
            }


            return result;
        }

        public List<CC> GetFromHappyBitisMauCu(string BarCode)
        {
            BarCode = BarCode.ToUpper();

            List<CC> result = new List<CC>();
            string Tem = BarCode.Substring(0, 16);
            Tem = Tem.Substring(0, Tem.Length - 1);
            Tem = Tem.Remove(12, 1);

            string Loai = BarCode.Substring(16, 2);
            string SL = BarCode.Substring(23, 1);
            int Quantity = Commons.ConvertToInt(SL);
            string LSX = BarCode.Substring(18, 4);
            string sSQL = "select ItemID,ItemName,UnitID from ItemVolumes ";
            sSQL += " where left(BarCode,12)+left(right(BarCode,5),4) like N'" + Commons.Fix(Tem) + "%'";
            sSQL += " and Used=1 ";
            sSQL += " order by ItemID ";
            DataTable dt = Commons.GetData(sSQL);


            if (Loai == "HM")
            {
                if (dt.Rows.Count != 6)
                {
                    return new List<CC>();

                }
            }
            else
            {
                if (dt.Rows.Count != 5)
                {
                    return new List<CC>();
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CC r = new CC();
                r.i = Commons.ConvertToString(dt.Rows[i]["ItemID"]);
                r.d = Commons.ConvertToString(dt.Rows[i]["ItemName"]);
                r.u = Commons.ConvertToString(dt.Rows[i]["UnitID"]);
                r.q = Quantity;
                r.lsx = LSX;
                result.Add(r);
            }


            return result;
        }
        public ActionResult ExportPallet()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);
            int Sum = Commons.ConvertToInt(Request.QueryString["sum"]);
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            try
            {
                string[] l = from.Split('/');
                fromdate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = to.Split('/');
                todate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }
            string sSQL = "exec SP_ExportPallet ";
            sSQL += "N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",'" + fromdate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + todate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + Fix(key) + "'";
            sSQL += "," + Sum;
            sSQL += "," + GlobalVariables.UserID.ToString();

            DataTable dt = Commons.GetData(sSQL);

            Export d = new Export();
            if (Sum == 0)
            {
                d.ToExcel(Response, dt, "ExportPalletDetail");
            }
            else
            {
                d.ToExcel(Response, dt, "ExportPallet");
            }

            return View();
        }

        public int GetQuantity(string BarCode, string ItemID)
        {
            BarCode = BarCode.ToUpper();


            string QC = BarCode.Substring(22, 2);
            string sSQL = "select Q,RQ,RS from [Styles] where [Style]='" + Commons.Fix(QC) + "'";
            DataTable dt = Commons.GetData(sSQL);
            //System.IO.File.WriteAllText("d:\\a1.sql", sSQL);
            int Q = 0;
            if (dt.Rows.Count > 0)
            {
                string[] RD = dt.Rows[0]["RQ"].ToString().Split(':');
                string[] RS = dt.Rows[0]["RS"].ToString().Split(':');
                string SizeID = ItemID.Substring(12);

                Q = Convert.ToInt32(dt.Rows[0][0]);
                if (Q == 0)
                {
                    for (int i = 0; i < RS.Length; i++)
                    {
                        if (RS[i] == SizeID)
                        {
                            Q = int.Parse(RD[i]);
                            break;
                        }
                    }
                }
            }
            return Q;

        }
        [HttpPost]
        public ActionResult SaveOutBound(string PalletID, string OutBound)
        {
            Exception ex = null;
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            string sWrite = "update pallets set OutBound='" + Commons.Fix(OutBound) + "' where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and PalletID ='" + Commons.Fix(PalletID) + "'";
            sWrite += ";exec [SP_InsertOutBound] N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + Commons.Fix(PalletID) + "','" + Commons.Fix(OutBound) + "'";
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                return Json(new { msg = "Thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        private bool CheckBarCodeIsExists(string PalletID, string BarCode)
        {
            BarCode = BarCode.ToUpper();

            string sSQL = "select BarCode from BarCodeFromPallets where BarCode='" + Commons.Fix(BarCode) + "' and PalletID = N'" + Commons.Fix(PalletID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult UpdatePalletDescription(string PalletID, string diengiai)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery("update pallets set Description=N'" + Fix(diengiai) + "' where PalletID=N'" + Fix(PalletID) + "'", ref ex);
            if (b == false)
                return Json(new { errorMsg = ex.Message, success = false });

            return Json(new { msg = "Cập nhật thành công", success = true });
        }
        [HttpPost]
        public ActionResult PostW(string BarCode, string PalletID, int Quantity)
        {
            try
            {
                BarCode = BarCode.ToUpper();

                string sWrite = "";

                if (PalletUsed(PalletID))
                {
                    return Json(new { errorMsg = "Pallet này đã có xuất bạn không được thêm", success = false });
                }

                bool mustload = false;

                string LSX = "";
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }

                if (Global.Commons.CheckPermit("InputW") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }

                if (BarCode == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập barcode hoặc mã hàng", success = false });
                }

                if (BarCode.Length == 27 && (BarCode.Substring(16, 1) == "H" || BarCode.Substring(14, 1) == "H"))
                {
                    if (CheckBarCodeIsExists(PalletID, BarCode))
                    {
                        return Json(new { errorMsg = "Barcode này đã quét rồi", success = false });
                    }

                    List<CC> b = GetFromHappyBitis(BarCode);
                    if (b.Count > 0)
                    {

                        sWrite = "";
                        string listitemid = "";
                        foreach (CC item in b)
                        {
                            sWrite += "exec [SP_InsertPalletDetail] ";
                            sWrite += " N'" + Commons.Fix(Commons.ConvertToString(PalletID)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.i)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.d)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.u)) + "'";
                            sWrite += " ," + item.q.ToString("0");
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.outbound)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.lsx)) + "'";

                            sWrite += ";";
                            listitemid += ", " + item.i;
                        }
                        listitemid = listitemid.Trim(',');
                        listitemid = listitemid.Trim();

                        Exception ex = null;
                        bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        if (result == true)
                        {
                            string sW = "exec SP_InsertBarCodeFromPallet   ";
                            sW += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                            sW += ",'" + Commons.Fix(PalletID) + "'";
                            sW += ",'" + Commons.Fix(BarCode) + "'";
                            sW += ",'" + Commons.Fix(b[0].i) + "'";
                            sW += ",'" + Commons.Fix(b[0].lsx) + "'";

                            Commons.ExecuteNoneQuery(sW);

                            AddPalletLog(PalletID, "[" + GlobalVariables.UserName + "] thêm hàng happy " + listitemid + " sl mỗi mã:" + b[0].q.ToString("0"));

                            return Json(new { msg = "Thành công", success = true, mustload = true });
                        }

                        else
                        {
                            return Json(new { errorMsg = ex.Message, success = false });
                        }
                    }

                    else
                    {
                        return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                    }
                }




                CC r = new CC();
                if (BarCode.Length == 18)
                {
                    if (BarCode.Substring(BarCode.Length - 1, 1) == "0")
                    {
                        r = GetFrom18(BarCode);
                        r.q = Quantity;
                    }
                    else
                    {
                        List<CC> sL = GetFrom1811(BarCode);
                        if (sL.Count == 0)
                        {
                            return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                        }

                        sWrite = "";
                        foreach (CC item in sL)
                        {
                            sWrite += "exec [SP_InsertPalletDetail] ";
                            sWrite += " N'" + Commons.Fix(Commons.ConvertToString(PalletID)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.i)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.d)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.u)) + "'";
                            sWrite += " ," + item.q.ToString("0");
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.outbound)) + "'";
                            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(item.lsx)) + "'";

                            sWrite += ";";
                        }




                        Exception ex = null;
                        bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        if (result == true)
                        {


                            AddPalletLog(PalletID, "[" + GlobalVariables.UserName + "] thêm hàng tem 18 " + sL[0].i + " sl:" + sL[0].q.ToString("0"));

                            return Json(new { msg = "Thành công", success = true, mustload = true });
                        }

                        else
                        {
                            return Json(new { errorMsg = ex.Message, success = false });
                        }
                    }

                }
                else if (BarCode.Length == 27)
                {
                    if (CheckBarCodeIsExists(PalletID, BarCode))
                    {
                        return Json(new { errorMsg = "Barcode này đã quét rồi", success = false });

                    }


                    try
                    {
                        r = GetFrom27(BarCode);
                        if (r.i == "" || r.i == null)
                        {
                            return Json(new { errorMsg = "Không tìm thấy thùng hàng này", success = false });
                        }

                        LSX = r.lsx;
                    }
                    catch
                    {

                        return Json(new { errorMsg = "loi so luong", success = false });
                    }
                    r.q = GetQuantity(BarCode, r.i);

                    string sW = "exec SP_InsertBarCodeFromPallet   ";
                    sW += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sW += ",'" + Commons.Fix(PalletID) + "'";
                    sW += ",'" + Commons.Fix(BarCode) + "'";
                    sW += ",'" + Commons.Fix(r.i) + "'";
                    sW += ",'" + Commons.Fix(r.lsx) + "';";
                    sW += " exec SP_UpdateItemBox   ";
                    sW += " '" + Commons.Fix(r.i) + "'";
                    sW += "," + Commons.ConvertToInt(r.q).ToString("0") + ";";

                    Commons.ExecuteNoneQuery(sW);

                    mustload = true;
                }
                else if (BarCode.Length == 14)
                {
                    r = GetFromItem(BarCode);
                    r.q = Quantity;
                }
                else
                {
                    r.i = "";
                    r.q = 1;
                }
                r.outbound = "";

                if ((r.i != "" || r.i == null) && r.q > 0)
                {
                    sWrite = "exec [SP_InsertPalletDetail] ";
                    sWrite += " N'" + Commons.Fix(Commons.ConvertToString(PalletID)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(r.i)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(r.d)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(r.u)) + "'";
                    sWrite += " ," + r.q.ToString("0");
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(r.outbound)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(r.lsx)) + "'";

                    sWrite += ";";


                    Exception ex = null;
                    bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    if (result == true)
                    {
                        AddPalletLog(PalletID, "[" + GlobalVariables.UserName + "] thêm hàng " + r.i + " sl:" + r.q.ToString("0"));

                        return Json(new { msg = r.d, success = true, mustload = true });
                    }

                    else
                    {
                        return Json(new { errorMsg = ex.Message, success = false });
                    }
                }
                else
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });

                }


            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = "Tem không hợp lệ " + ex.Message, success = false });
            }



        }
        [HttpPost]
        public ActionResult DeleteItemW(string ItemID, string PalletID, string LSX)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (PalletUsed(PalletID))
            {
                return Json(new { errorMsg = "Pallet này đã có xuất bạn không được xóa", success = false });
            }

            if (PalletFinished(PalletID))
            {
                return Json(new { errorMsg = "Pallet này đã xác nhận vị trí rồi bạn không được xóa", success = false });
            }

            string sWrite = "exec [SP_DeletePalletDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " ,'" + Commons.Fix(PalletID) + "' ,'" + Commons.Fix(ItemID) + "','" + Commons.Fix(LSX) + "'; ";
            string Location = GetLocationFromPallet(PalletID);
            sWrite += "exec SP_UpdateVolumeUsed '" + Commons.Fix(Location) + "','" + Commons.Fix(GlobalVariables.DivisionID) + "';";
            Exception ex = null;
            string ssql = "select Quantity from PalletDetail ";
            ssql += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            ssql += " and PalletID='" + Commons.Fix(PalletID) + "' ";
            ssql += " and ItemID='" + Commons.Fix(ItemID) + "' ";
            ssql += " and LSX='" + Commons.Fix(LSX) + "' ";

            DataTable dt = Commons.GetData(ssql);
            int Quantiy = 0;
            if (dt.Rows.Count > 0)
            {
                Quantiy = Convert.ToInt32(dt.Rows[0][0]);
            }

            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                AddPalletLog(PalletID, "[" + GlobalVariables.UserName + "] xóa hàng " + ItemID + " sl: " + Quantiy.ToString());
                return Json(new { msg = "Xóa thành công", success = true });

            }
            else
            {
                return Json(new { errorMsg = "Không tìm thấy hàng này", success = false });
            }
        }
        //xac nhan kem pham
        [HttpPost]
        public ActionResult XNKP(string ItemID, string PalletID, string OutBound)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            string sWrite = "exec [SP_XNKP] N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " ,'" + Commons.Fix(PalletID) + "' ,'" + Commons.Fix(OutBound) + "','" + Commons.Fix(ItemID) + "'; ";
            Exception ex = null;
            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không tìm thấy hàng này", success = false });
            }
        }
        [HttpPost]
        public ActionResult DeleteLSX(string LSX, string PalletID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            string sWrite = "exec SP_DeleteLSX N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " ,'" + Commons.Fix(PalletID) + "','" + Commons.Fix(LSX) + "' ";
            Exception ex = null;
            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                AddPalletLog(PalletID, GlobalVariables.UserName + " xóa lsx " + LSX + " ");
                return Json(new { msg = "Xóa thành công", success = true });
            }

            else
            {
                return Json(new { errorMsg = "Không thể xóa " + ex.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult DeleteOutBound(string OutBound, string PalletID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            string sWrite = "exec SP_DeleteOutBound N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " ,'" + Commons.Fix(PalletID) + "','" + Commons.Fix(OutBound) + "' ";
            Exception ex = null;
            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                AddPalletLog(PalletID, GlobalVariables.UserName + " xóa outbound " + OutBound + " ");
                return Json(new { msg = "Xóa thành công", success = true });

            }
            else
            {
                return Json(new { errorMsg = "Không thể xóa " + ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult DeletePallet(string PalletID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (PalletUsed(PalletID))
            {
                return Json(new { errorMsg = "Pallet này đã có xuất bạn không được xóa", success = false });
            }

            if (PalletFinished(PalletID))
            {
                return Json(new { errorMsg = "Pallet này đã xác nhận vị trí rồi bạn không được xóa", success = false });
            }

            string sWrite = "delete PalletDetail where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and PalletID='" + Commons.Fix(PalletID) + "' ;";
            Exception ex = null;
            sWrite += "delete Pallets where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and PalletID='" + Commons.Fix(PalletID) + "' ;";
            sWrite += ";delete BarCodeFromPallets where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and PalletID='" + Commons.Fix(PalletID) + "' ;";
            string Location = GetLocationFromPallet(PalletID);

            sWrite += "exec SP_UpdateVolumeUsed '" + Commons.Fix(Location) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";


            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public string GetLocationFromPallet(string PalletID)
        {
            string sSQL = "select Location from Pallets where PalletID='" + Commons.Fix(PalletID) + "'";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Commons.ConvertToString(dt.Rows[0][0]);
            }
            return "";
        }
        public DataTable GetLocationFromXH(string VoucherID)
        {
            string sSQL = "select Location from XH where VoucherID='" + Commons.Fix(VoucherID) + "'";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);

            return dt;
        }
        public ActionResult PrintPallet()
        {


            return View();
        }
        public ActionResult ViewPallets()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("viewpallets") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string date = Commons.ConvertToString(Request.QueryString["date"]);
            string dateto = Commons.ConvertToString(Request.QueryString["dateto"]);
            DateTime d = DateTime.Now;
            DateTime dto = DateTime.Now;

            try
            {
                string[] l = date.Split('/');
                d = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = dateto.Split('/');
                dto = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }
            ViewBag.dd = d.ToString("yyyy.MM.dd");
            ViewBag.dd1 = dto.ToString("yyyy.MM.dd");

            string sSQL = "exec SP_GetPalletCount ";
            sSQL += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ", N'" + Commons.Fix(keyword) + "'";
            sSQL += ",'" + d.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + dto.ToString("yyyy.MM.dd") + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");

            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int nSum = Convert.ToInt32(dt.Rows[0][1]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/viewpallets?date=" + d.ToString("dd/MM/yyyy") + "&dateto=" + dto.ToString("dd/MM/yyyy") + "&key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();


                    rd[0] = "/admin/viewpallets?date=" + d.ToString("dd/MM/yyyy") + "&dateto=" + dto.ToString("dd/MM/yyyy") + "&key=" + keyword + "&page=" + 1;
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/viewpallets?date=" + d.ToString("dd/MM/yyyy") + "&dateto=" + dto.ToString("dd/MM/yyyy") + "&&key=" + keyword + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");

                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);

                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");
            return View();
        }
        [HttpPost]
        public ActionResult CheckSSNK()
        {
            string ssql = "exec SP_SSNKAccess " + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(ssql);
            DataRow r = dt.Rows[0];
            if (Commons.ConvertToInt(r[0]) == 1)
            {
                return Json(new { msg = "Thành công", success = true });
            }

            return Json(new { errorMsg = r[1], success = false });


        }

        [HttpPost]
        public ActionResult Get_Pallets()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;
            string date = Commons.ConvertToString(Request.QueryString["date"]);
            string dateto = Commons.ConvertToString(Request.QueryString["dateto"]);
            DateTime d = DateTime.Now;
            DateTime dto = DateTime.Now;

            try
            {
                string[] l = date.Split('/');
                d = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = dateto.Split('/');
                dto = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }
            string sSQL = "exec SP_GetPallets ";
            sSQL += "N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ", N'" + Commons.Fix(keyword) + "'";
            sSQL += "," + CurrentPage.ToString("0");
            sSQL += "," + PAGE_SIZE.ToString("0");
            sSQL += ",'" + d.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + dto.ToString("yyyy.MM.dd") + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            PalletID = p["PalletID"],
                            CreateDate = Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy  HH:mm"),
                            UserName = Commons.ConvertToString(p["UserName"]),
                            Location = Commons.ConvertToString(p["Location"]),
                            OrderNo = p["OrderNo"],
                            Description = p["Description"],
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            Status = (Commons.ConvertToBool(p["Active"]) == false ? "Đang dở dang" : "Đã quét xong"),
                            ActiveStatus = (Commons.ConvertToBool(p["Active"]) == false ? "<span style='color:red'>Đang dở dang</span>" : "<span style='color:green'>Đã quét xong</span>"),
                            Status1 = (Commons.ConvertToBool(p["Finish"]) == false ? "Đang chờ" : "Đã xác nhận"),
                            ConfirmStatus = (Commons.ConvertToBool(p["Finish"]) == false ? "<span style='color:red'>Đang chờ</span>" : "<span style='color:green'>Đã xác nhận</span>"),
                            FullName = p["FullName"],
                            PalletInfo = PalletInfo(p["PalletID"].ToString(), Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy  HH:mm"), Commons.ConvertToString(p["UserName"]), Commons.ConvertToString(p["Location"]), p["OrderNo"].ToString(), Commons.ConvertToInt(p["Quantity"]).ToString("N0"), (Commons.ConvertToBool(p["Active"]) == false ? "<span style='color:red'>Đang dở dang</span>" : "<span style='color:green'>Đã quét xong</span>"), (Commons.ConvertToBool(p["Finish"]) == false ? "<span style='color:red'>Đang chờ</span>" : "<span style='color:green'>Đã xác nhận</span>"), Commons.ConvertToString(p["FullName"]))


                        };
            return Json(query);
        }
        public string PalletInfo(string PalletID, string CreateDate,
            string UserName, string Location, string OrderNo, string Quantity,
            string ActiveStatus, string ConfirmStatus
            , string FullName)
        {
            string sResult = "";
            sResult += "<p>Mã pallet:" + PalletID + "</p>";
            sResult += "<p>Ngày tạo:" + CreateDate + "</p>";
            sResult += "<p>Thứ tự:" + OrderNo + "</p>";
            sResult += "<p>Số lượng:" + Quantity + "</p>";
            sResult += "<p>Trạng thái:" + ActiveStatus + "</p>";
            sResult += "<p>Xác nhận sau cùng:" + ConfirmStatus + "</p>";
            sResult += "<p>Người tạo:" + FullName + "</p>";
            return sResult;
        }
        public ActionResult PrintRange()
        {
            int From = Commons.ConvertToInt(Request.QueryString["from"]);
            int To = Commons.ConvertToInt(Request.QueryString["to"]);
            string d = Commons.ConvertToString(Request.QueryString["d"]);
            string d1 = Commons.ConvertToString(Request.QueryString["d1"]);

            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            string sSQL = "exec SP_PrintRange N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += "," + From.ToString("0") + " , " + To.ToString("0");
            sSQL += ",N'" + Commons.Fix(Keyword) + "'";
            sSQL += ",'" + Commons.Fix(d) + "'";
            sSQL += ",'" + Commons.Fix(d1) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            sSQL += ";";

            DataTable dt = Commons.GetData(sSQL);
            dt.Columns.Add("r", To.GetType());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["r"] = i;
            }
            ViewBag.data = dt.Rows;
            ViewBag.n = dt.Rows.Count;

            sSQL = "select top 1 RC from Settings ";
            dt = Commons.GetData(sSQL);
            ViewBag.rc = dt.Rows[0][0];
            return View();
        }
        public bool WasSet(string PalletID)
        {
            string sSQL = "select isnull(Location,'') Location from Pallets where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' and PalletID='" + Commons.Fix(PalletID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows[0][0].ToString() != "";
        }
        [HttpPost]
        public ActionResult CaculatePos(string PalletID)
        {
            try
            {
                if (WasSet(PalletID))
                {
                    return Json(new { errorMsg = "Pallet này đã đặt vị trí rồi", success = false });
                }
                string sSQL = "exec SP_GetPalletSum '" + Commons.Fix(PalletID) + "' ,'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    decimal M3 = Convert.ToDecimal(dt.Rows[0]["CM3"]) / 1000000;
                    sSQL = "exec SP_GetAvaliable N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sSQL += " ," + Commons.DecimalToSQL(M3);
                    sSQL += ",'" + Commons.Fix(PalletID) + "' ";
                    dt = Commons.GetData(sSQL);
                    if (dt.Rows.Count > 0)
                    {
                        string Location = dt.Rows[0]["Location"].ToString();
                        string sWrite = "exec SP_SetLocation ";
                        sWrite += " N'" + Commons.Fix(PalletID) + "'";
                        sWrite += ",N'" + Commons.Fix(Location) + "' ";
                        sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                        sWrite += "," + Commons.DecimalToSQL(M3);
                        sWrite += ";";
                        Exception ex = null;
                        bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        if (result == true)
                        {
                            return Json(new { msg = "Chọn thành công", success = true });
                        }
                        else
                        {
                            return Json(new { errorMsg = "Có lỗi " + ex.Message, success = false });
                        }
                    }
                }
            }
            catch (Exception ext)
            {
                return Json(new { errorMsg = ext.Message, success = false });


            }

            return Json(new { errorMsg = "Không tìm được vị trí trống. Vui lòng tự xác định bằng tay", success = false });

        }
        [HttpPost]
        public ActionResult FinishPallet(string PalletID)
        {


            //if (OutBound == "")
            //{
            //    return Json(new { errorMsg = "Bạn chưa cập nhật số chứng từ", success = false });

            //}

            Exception ex = null;
            string sWrite = "update Pallets set Active=1,OutBound='' where PalletID= '" + Commons.Fix(PalletID) + "' and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);

            if (result == true)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Có lỗi " + ex.Message, success = false });
            }
        }

        public bool ApplyPos(string PalletID)
        {
            try
            {
                if (WasSet(PalletID))
                {
                    return false;
                }
                string sSQL = "exec SP_GetPalletSum '" + Commons.Fix(PalletID) + "' ,'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    decimal M3 = Convert.ToDecimal(dt.Rows[0]["CM3"]) / 1000000;
                    sSQL = "exec SP_GetAvaliable N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sSQL += " ," + Commons.DecimalToSQL(M3);
                    sSQL += ",'" + Commons.Fix(PalletID) + "' ";
                    dt = Commons.GetData(sSQL);
                    if (dt.Rows.Count > 0)
                    {
                        string Location = dt.Rows[0]["Location"].ToString();
                        string sWrite = "exec SP_SetLocation ";
                        sWrite += " N'" + Commons.Fix(PalletID) + "'";
                        sWrite += ",N'" + Commons.Fix(Location) + "' ";
                        sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                        sWrite += "," + Commons.DecimalToSQL(M3);
                        sWrite += ";";
                        Exception ex = null;
                        bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        if (result == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;


            }

            return false;

        }
        [HttpPost]
        public ActionResult ApplyPos()
        {
            try
            {
                string sSQL = "select PalletID from Pallets where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and Active=1 and isnull(Location,'') ='' ";
                DataTable dt = Commons.GetData(sSQL);
                foreach (DataRow item in dt.Rows)
                {
                    ApplyPos(item[0].ToString());
                }
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            catch (Exception ex1)
            {

                return Json(new { errorMsg = "Có lỗi " + ex1.Message, success = false });

            }

        }

        public ActionResult MoveW()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("InputW") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string sSQL = "";
            string PalletID = Commons.ConvertToString(Request.QueryString["id"]);
            DataTable dt = new DataTable();
            if (PalletID == "")
            {

                Response.Redirect("~/admin");

            }
            sSQL = "select OrderNo,CreateDate,isnull(Location,'') Location from Pallets where PalletID='" + Commons.Fix(PalletID) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.OrderNo = Commons.ConvertToInt(dt.Rows[0]["OrderNo"]).ToString("N0");
                ViewBag.CreateDate = Commons.ConvertToDateTime(dt.Rows[0]["CreateDate"]).ToString("dd/MM/yyyy");
                string Location = dt.Rows[0]["Location"].ToString();
                if (Location != "")
                {
                    ViewBag.Location = Location;
                    ViewBag.exists = true;
                }

                else
                {
                    ViewBag.Location = "Chưa có";
                    ViewBag.exists = false;
                }


            }

            List<CC> ds = new List<CC>();
            sSQL = "exec [SP_GetPalletDetail] '" + Commons.Fix(PalletID) + "','" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            dt = Commons.GetData(sSQL);
            int TotalQuantity = 0;
            foreach (DataRow item in dt.Rows)
            {
                CC i = new CC();
                i.i = item["ItemID"].ToString();
                i.d = item["ItemName"].ToString();
                i.q = Convert.ToInt32(item["Quantity"]);
                i.u = Commons.ConvertToString(item["UnitID"]);
                TotalQuantity += i.q;
                ds.Add(i);
            }
            ViewBag.TotalQuantity = TotalQuantity.ToString("N0");
            ViewBag.data = ds;
            ViewBag.PalletID = PalletID;
            return View();
        }

        private bool AllowSet(string Location)
        {
            return true;
            string sSQL = "select  Location from Locations ";
            sSQL += " where Location='" + Commons.Fix(Location) + "' and (isnull(Odd,0) = 0 ";
            sSQL += " and dbo.SP_IsEmptyLocation(Location,DivisionID) = 1  or isnull(Odd,0)=1 )";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;


        }
        [HttpPost]
        public ActionResult MoveW(string PalletID, string Location)
        {
            if (Location == "")
            {
                return Json(new { errorMsg = "Bạn chưa nhập vị trí", success = false });
            }
            if (CheckLocationExists(Location) == false)
            {
                return Json(new { errorMsg = "Vị trí này không tồn tại", success = false });
            }
            if (CheckPalletExists(PalletID) == false)
            {
                return Json(new { errorMsg = "Pallet này không tồn tại", success = false });
            }
            if (LockedForIn(Location))
            {
                return Json(new { errorMsg = "Vị trí này bị cấm nhập. vui lòng chọn vị trí khác ", success = false });
            }
            if (AllowSet(Location) == false)
            {
                return Json(new { errorMsg = "Bạn không được phép chuyển vào vị trí này", success = false });

            }
            if (PalletFinished(PalletID) == true)
            {
                return Json(new { errorMsg = "Pallet này đã được xác nhận rồi bạn không được phép chuyển", success = false });

            }
            string sSQL = "exec SP_GetPalletSum '" + Commons.Fix(PalletID) + "' ,'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                decimal M3 = Convert.ToDecimal(dt.Rows[0]["CM3"]) / 1000000;
                string OldLocation = dt.Rows[0]["Location"].ToString();
                if (OldLocation == Location)
                {
                    return Json(new { errorMsg = "Vị trí mới phải khác vị trí cũ", success = false });
                }
                string sWrite = "exec SP_MovePallet ";
                sWrite += " N'" + Commons.Fix(PalletID) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",N'" + Commons.Fix(Location) + "' ";
                sWrite += "," + Commons.DecimalToSQL(M3);
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                sWrite += ";";
                Exception ex = null;
                bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (result == true)
                {
                    return Json(new { msg = "Chuyển thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = "Có lỗi " + ex.Message, success = false });
                }
            }

            return Json(new { errorMsg = "Không tồn tại vị trí này", success = false });
        }
        [HttpPost]
        public ActionResult UpdatePalletHandMade(string PalletID, string Location)
        {
            if (Location == "")
            {
                return Json(new { errorMsg = "Bạn chưa nhập vị trí", success = false });
            }
            if (CheckLocationExists(Location) == false)
            {
                return Json(new { errorMsg = "Vị trí này không tồn tại", success = false });
            }
            if (LockedForIn(Location))
            {
                return Json(new { errorMsg = "Vị trí này bị cấm nhập. vui lòng chọn vị trí khác ", success = false });
            }
            if (CheckPalletExists(PalletID) == false)
            {
                return Json(new { errorMsg = "Pallet này không tồn tại", success = false });
            }
            try
            {
                if (WasSet(PalletID))
                {
                    return Json(new { errorMsg = "Pallet này đã đặt vị trí rồi", success = false });
                }
                if (AllowSet(Location) == false)
                {
                    return Json(new { errorMsg = "Bạn không được phép chuyển vào vị trí này", success = false });

                }

                string sSQL = "exec SP_GetPalletSum '" + Commons.Fix(PalletID) + "' ,'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    decimal M3 = Convert.ToDecimal(dt.Rows[0]["CM3"]) / 1000000;

                    string sWrite = "exec SP_SetLocation ";
                    sWrite += " N'" + Commons.Fix(PalletID) + "'";
                    sWrite += ",N'" + Commons.Fix(Location) + "' ";
                    sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += "," + Commons.DecimalToSQL(M3);
                    sWrite += ";";
                    Exception ex = null;
                    bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    if (result == true)
                    {
                        return Json(new { msg = "Chọn thành công", success = true });
                    }
                    else
                    {
                        return Json(new { errorMsg = "Có lỗi " + ex.Message, success = false });
                    }
                }
            }
            catch (Exception ext)
            {
                return Json(new { errorMsg = ext.Message, success = false });


            }

            return Json(new { errorMsg = "Không tìm được vị trí ", success = false });
        }

        public bool CheckPalletExists(string PalletID)
        {
            string sSQL = "select PalletID from Pallets ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and PalletID='" + Commons.Fix(PalletID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool PalletUsed(string PalletID)
        {
            string sSQL = "select PalletID from PalletDetailX ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and PalletID='" + Commons.Fix(PalletID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool PalletFinished(string PalletID)
        {
            string sSQL = "select PalletID from Pallets  ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and PalletID='" + Commons.Fix(PalletID) + "' and Finish=1 ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool LocationUsed(string Location)
        {
            string sSQL = "select PalletID from Pallets ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and Location='" + Commons.Fix(Location) + "'";
            sSQL += " union all select '' PalletID from BalanceAll ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and Location='" + Commons.Fix(Location) + "'";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult CheckPostPalletExists(string PalletID)
        {
            string sSQL = "select PalletID from Pallets ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and PalletID='" + Commons.Fix(PalletID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Json(new { msg = "Thành công", success = true });
            }

            return Json(new { errorMsg = "Không tồn tại pallet này", success = false });

        }
        [HttpPost]
        public ActionResult CheckVoucherExists(string VoucherID)
        {
            string sSQL = "select VoucherID from W ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID='" + Commons.Fix(VoucherID) + "'";

            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Không tồn tại chứng từ này", success = false });
            }

            sSQL = "select VoucherID from XH ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID='" + Commons.Fix(VoucherID) + "'";

            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Chứng từ này chưa định vị trí", success = false });
            }

            return Json(new { msg = "Thành công", success = true });



        }
        public bool CheckPalletLocationExists(string PalletID, string Location)
        {
            string sSQL = "select PalletID from Pallets ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and PalletID='" + Commons.Fix(PalletID) + "'";
            sSQL += " and Location='" + Commons.Fix(Location) + "'";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool CheckLocationExists(string Location)
        {
            string sSQL = "select Location from Locations ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location='" + Commons.Fix(Location) + "'";
            sSQL += " and (isnull(LockedForIn,0)=0 or isnull(LockedForOut,0)=0)  ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool CheckLocationSource(string Location)//check nguon vi tri xuat
        {
            string sSQL = "select Location from Locations ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location='" + Commons.Fix(Location) + "'";
            sSQL += " and  isnull(LockedForOut,0)=0  ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool CheckLocationDest(string Location)//check dich vi tri nhap
        {
            string sSQL = "select Location from Locations ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location='" + Commons.Fix(Location) + "'";
            sSQL += " and  isnull(LockedForIn,0)=0  ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool CheckMoveVoucherExists(string VoucherID)
        {
            string sSQL = "select VoucherID from MoveItems ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and VoucherID='" + Commons.Fix(VoucherID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool LockedForIn(string Location)
        {
            string sSQL = "select LockedForIn from Locations ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and Location='" + Commons.Fix(Location) + "' and isnull(LockedForIn,0)=1";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        private string GetNewKey()
        {
            string sResult = "";
            string sSQL = "exec GetNewKey N'" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            sResult = dt.Rows[0][0].ToString();
            return sResult;
        }
        [HttpPost]
        public ActionResult CheckPostLocationExists(string LocationIDFrom, string LocationIDTo)
        {
            bool xuat = true;
            bool nhap = true;

            xuat = CheckLocationSource(LocationIDFrom);
            nhap = CheckLocationDest(LocationIDTo);


            if (xuat == false)
            {
                return Json(new { errorMsg = "Không tìm thấy vị trí xuất " + LocationIDFrom + " hoặc vị trí này đã bị khóa", success = false });
            }

            if (nhap == false)
            {
                return Json(new { errorMsg = "Không tìm thấy vị trí nhập " + LocationIDTo + " hoặc vị trí này đã bị khóa", success = false });
            }

            string VoucherID = GetNewKey();


            return Json(new { msg = "Thành công", success = true, VoucherID = VoucherID });



        }
        [HttpPost]
        public ActionResult CheckPostLocationToExists(string LocationIDTo)
        {
            bool kq = true;

            string sSQL = " ";

            sSQL = "select Location from Locations ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and Location='" + Commons.Fix(LocationIDTo) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                kq = false;
            }

            if (kq == false)
            {
                return Json(new { errorMsg = "Không tìm thấy vị trí " + LocationIDTo, success = false });
            }

            string VoucherID = GetNewKey();


            return Json(new { msg = "Thành công", success = true, VoucherID = VoucherID });



        }

        [HttpPost]
        public ActionResult CheckSLocation(string Location)
        {
            bool kq = true;

            string sSQL = "select Location from Locations ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location='" + Commons.Fix(Location) + "'";
            DataTable dt = Commons.GetData(sSQL);

            if (dt.Rows.Count == 0)
            {
                kq = false;
            }

            if (kq == false)
            {
                return Json(new { errorMsg = "Không tìm thấy vị trí " + Location, success = false });
            }

            return Json(new { msg = "Thành công", success = true });



        }
        [HttpPost]
        public ActionResult CheckPostLocationBalance(string LocationID)
        {
            string sSQL = "select Location,sum(Quantity) Quantity from ViewPallets ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location='" + Commons.Fix(LocationID) + "'";
            sSQL += " group by Location ";
            sSQL += " having sum(Quantity)>0 ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {

                return Json(new { msg = "Thành công", success = true });
            }



            return Json(new { errorMsg = "Không có hàng để duy", success = false });

        }
        public ActionResult NK()
        {
            W w = new W();
            ViewBag.VoucherDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.ObjectID = "";
            ViewBag.ObjectName = "";
            ViewBag.WareHouseID = "";

            if (GlobalVariables.WA != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(GlobalVariables.WA);
                }
                catch
                {

                }

            }
            int i = 0;

            try
            {
                foreach (WD item in w.WD)
                {
                    WD n = new WD();
                    n.ItemID = item.ItemID;
                    n.ItemName = item.ItemName;
                    n.UnitID = item.UnitID;
                    n.Quantity = item.Quantity;

                    i++;
                }
            }
            catch
            {


            }

            ViewBag.data = w.WD;
            ViewBag.VoucherDate = w.VoucherDate;
            ViewBag.ObjectID = w.ObjectID;
            ViewBag.ObjectName = w.ObjectName;
            ViewBag.WareHouseID = w.IW;
            ViewBag.palletlist = w.Pallets;


            return View();
        }
        [HttpPost]
        public ActionResult AddNK(string ItemID, int Quantity, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.WA;
            }
            else
            {
                token = GlobalVariables.WE;
            }

            W w = new W();
            Exception eee = null;
            if (ItemID.Length == 12)
            {
                bool b = AddFromPallet(ItemID, ref eee, AddNew);
                return Json(new { msg = "Cập nhật thành công", success = true });

            }
            if (ItemID == "")
            {
                return Json(new { errorMsg = "Không tìm thấy mã này", success = false });
            }

            string sSQL = "select ItemID,ItemName,UnitID from ItemVolumes where ItemID='" + Commons.Fix(ItemID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string ItemName = "";
            string UnitID = "";
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ItemName = r["ItemName"].ToString();
                UnitID = r["UnitID"].ToString();

                if (token != "")
                {
                    try
                    {
                        w = JsonConvert.DeserializeObject<W>(token);
                    }
                    catch
                    {
                    }

                }

                bool hasexists = false;
                foreach (WD item in w.WD)
                {
                    if (ItemID == item.ItemID)
                    {
                        item.Quantity += Quantity;
                        hasexists = true;
                        break;
                    }
                }
                if (hasexists == false)
                {
                    WD d = new WD();
                    d.ItemID = ItemID;
                    d.ItemName = ItemName;
                    d.UnitID = UnitID;
                    d.Quantity = Quantity;
                    w.WD = AddToList(w.WD, d);


                }
                if (AddNew == 1)
                {
                    GlobalVariables.WA = JsonConvert.SerializeObject(w);
                }
                else
                {
                    GlobalVariables.WE = JsonConvert.SerializeObject(w);
                }

                return Json(new { msg = "Cập nhật thành công", success = true });

            }
            else
            {
                return Json(new { errorMsg = "Không tìm thấy mã này", success = false });
            }

        }

        [HttpPost]
        public ActionResult DeleteNK(string ItemID, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.WA;
            }
            else
            {
                token = GlobalVariables.WE;
            }

            try
            {
                if (token != "")
                {
                    W w = new W();
                    w = JsonConvert.DeserializeObject<W>(token);
                    int i = 0;
                    foreach (WD item in w.WD)
                    {
                        if (item.ItemID == ItemID)
                        {
                            w.WD = RemoveList(w.WD, i);
                            break;
                        }
                        i++;
                    }

                    if (AddNew == 1)
                    {
                        GlobalVariables.WA = JsonConvert.SerializeObject(w);
                    }
                    else
                    {
                        GlobalVariables.WE = JsonConvert.SerializeObject(w);
                    }

                    return Json(new { msg = "Xóa thành công", success = true });

                }

            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

            return Json(new { errorMsg = "Không tìm thấy mã này", success = false });
        }

        [HttpPost]
        public ActionResult SaveTT(string VoucherDate, string IW, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.WA;
            }
            else
            {
                token = GlobalVariables.WE;
            }

            W w = new W();


            if (token != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(token);
                    w.VoucherDate = VoucherDate;
                    w.IW = IW;
                }
                catch
                {


                }

            }
            if (AddNew == 1)
            {
                GlobalVariables.WA = JsonConvert.SerializeObject(w);
            }
            else
            {
                GlobalVariables.WE = JsonConvert.SerializeObject(w);
            }

            return Json(new { msg = "Đã lưu", success = true });
        }

        [HttpPost]
        public ActionResult AddW(string VoucherDate, string WareHouseID)
        {

            //check data
            string[] l = VoucherDate.Split('/');
            if (l.Length != 3)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[0]) < 1 || Commons.ConvertToInt(l[0]) > 31)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[1]) < 1 || Commons.ConvertToInt(l[1]) > 12)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[2]) < 2000)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            DateTime d = DateTime.Now;
            try
            {
                d = new DateTime(Commons.ConvertToInt(l[2]), Commons.ConvertToInt(l[1]), Commons.ConvertToInt(l[0]));

            }
            catch
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });

            }
            if (WareHouseID.Trim() == "")
            {
                return Json(new { errorMsg = "Bạn chưa nhập mã kho", success = false });
            }

            string sSQL = "exec [SP_GetWK] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string VoucherID = dt.Rows[0][0].ToString();

            string sWrite = "exec [SP_InsertW] ";
            sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += ",'" + Commons.Fix(VoucherID) + "' ";
            sWrite += ",'" + Commons.Fix(WareHouseID) + "' ";
            sWrite += ",'' ";
            sWrite += ",'" + d.ToString("MM/dd/yyyy") + "'";
            sWrite += ",'GN'";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ";";
            int i = 1;
            Exception ex = null;
            if (GlobalVariables.WA != "")
            {

                W w = JsonConvert.DeserializeObject<W>(GlobalVariables.WA);
                if (w.WD.Length == 0)
                {
                    return Json(new { errorMsg = "Chưa có dữ liệu chi tiết", success = false });
                }

                foreach (WD item in w.WD)
                {
                    sWrite += "exec [SP_InsertWD] ";
                    sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sWrite += ",'" + VoucherID + "_" + i.ToString("00") + "'";
                    sWrite += ",'" + VoucherID + "'";
                    sWrite += "," + i.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.ItemID) + "'";
                    sWrite += "," + item.Quantity.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.UnitID) + "'";
                    sWrite += ",1";
                    sWrite += ",N'" + Commons.Fix(item.ItemName) + "'";
                    sWrite += ";";
                    i++;
                }
                foreach (string item in w.Pallets)
                {
                    sWrite += "exec [SP_InsertWLD] N'" + GlobalVariables.DivisionID + "'";
                    sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                    sWrite += ",N'" + Commons.Fix(item) + "';";
                }
                bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (result)
                {
                    GlobalVariables.WA = "";
                    return Json(new { msg = "Thêm thành công", v = VoucherID, success = true });
                }

                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            else
            {
                return Json(new { errorMsg = "Chưa có dữ liệu chi tiết", success = false });

            }

        }
        public ActionResult SK()
        {
            string sSQL = "";
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            if (VoucherID == "")
            {
                Response.Redirect("~/admin");
            }

            ViewBag.VoucherID = VoucherID;

            W w = new W();
            if (GlobalVariables.WE != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(GlobalVariables.WE);
                }
                catch
                {
                }
            }
            DataTable dt = new DataTable();
            if (VoucherID != w.VoucherID)
            {
                w.VoucherID = VoucherID;
                sSQL = "select  VoucherDate, ObjectID, ObjectName, Description, SAP,IW ";
                sSQL += " from W where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += " and VoucherID = '" + Commons.Fix(VoucherID) + "' and TransactionTypeID='GN' ";
                dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    w.ObjectID = ViewBag.ObjectID = r["ObjectID"];
                    w.ObjectName = ViewBag.ObjectName = r["ObjectName"];
                    w.IW = ViewBag.WareHouseID = r["IW"];
                    w.Description = ViewBag.Description = r["Description"];
                    w.SAP = ViewBag.SAP = r["SAP"];
                    w.VoucherDate = ViewBag.VoucherDate = Convert.ToDateTime(r["VoucherDate"]).ToString("dd/MM/yyyy");
                    sSQL = "select  TransactionID, ItemID, Quantity, UnitID, TransactionX, Note ";
                    sSQL += " from WD where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    sSQL += " and VoucherID = '" + Commons.Fix(VoucherID) + "' order by OrderNo ";
                    dt = Commons.GetData(sSQL);
                    EnumerableRowCollection<WD> query = from p in dt.AsEnumerable()
                                                        select new WD
                                                        {
                                                            TransactionID = p["TransactionID"].ToString(),
                                                            ItemID = p["ItemID"].ToString(),
                                                            ItemName = p["Note"].ToString(),
                                                            Quantity = Convert.ToInt32(p["Quantity"]),
                                                            UnitID = p["UnitID"].ToString()
                                                        };


                    ViewBag.data = query;
                    w.WD = query.ToArray();



                    //danh cho load pallet
                    sSQL = "select PalletID from WLD where VoucherID='" + Commons.Fix(VoucherID) + "' and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    dt = Commons.GetData(sSQL);
                    string[] pallets = new string[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow item in dt.Rows)
                    {
                        pallets[i] = item["PalletID"].ToString();
                        i++;
                    }
                    ViewBag.palletlist = pallets;
                    w.Pallets = pallets;
                    GlobalVariables.WE = JsonConvert.SerializeObject(w);

                }
                else
                {
                    Response.Redirect("~/admin");
                }
            }
            else
            {
                ViewBag.VoucherDate = w.VoucherDate;
                ViewBag.ObjectID = w.ObjectID;
                ViewBag.ObjectName = w.ObjectName;
                ViewBag.WareHouseID = w.IW;
                ViewBag.data = w.WD;
                ViewBag.palletlist = w.Pallets;

            }


            return View();

        }
        public ActionResult RemovePalletFromList(string PalletID, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.WA;
            }
            else
            {
                token = GlobalVariables.WE;
            }

            W w = new W();
            if (token != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(token);
                    foreach (string item in w.Pallets)
                    {
                        if (item == PalletID)
                        {
                            string[] p = new string[w.Pallets.Length - 1];
                            int i = 0;
                            foreach (string item1 in w.Pallets)
                            {
                                if (item1 != PalletID)
                                {
                                    p[i++] = item1;
                                }
                            }
                            w.Pallets = p;
                            break;
                        }
                    }
                }
                catch
                {
                }

                if (AddNew == 1)
                {
                    GlobalVariables.WA = JsonConvert.SerializeObject(w);
                }
                else
                {
                    GlobalVariables.WE = JsonConvert.SerializeObject(w);
                }
            }

            return Json(new { msg = "Xóa thành công", success = true });

        }

        public ActionResult ResetW()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("InputW") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string sSQL = "";
            string PalletID = Commons.ConvertToString(Request.QueryString["id"]);
            DataTable dt = new DataTable();
            if (PalletID == "")
            {

                Response.Redirect("~/admin");

            }
            sSQL = "select OrderNo,CreateDate,isnull(Location,'') Location from Pallets where PalletID='" + Commons.Fix(PalletID) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.OrderNo = Commons.ConvertToInt(dt.Rows[0]["OrderNo"]).ToString("N0");
                ViewBag.CreateDate = Commons.ConvertToDateTime(dt.Rows[0]["CreateDate"]).ToString("dd/MM/yyyy");
                string Location = dt.Rows[0]["Location"].ToString();
                if (Location != "")
                {
                    ViewBag.Location = Location;
                    ViewBag.exists = true;
                }

                else
                {
                    ViewBag.Location = "Chưa có";
                    ViewBag.exists = false;
                }


            }

            List<CC> ds = new List<CC>();
            sSQL = "exec [SP_GetPalletDetail] '" + Commons.Fix(PalletID) + "','" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            dt = Commons.GetData(sSQL);
            int TotalQuantity = 0;
            foreach (DataRow item in dt.Rows)
            {
                CC i = new CC();
                i.i = item["ItemID"].ToString();
                i.d = item["ItemName"].ToString();
                i.q = Convert.ToInt32(item["Quantity"]);
                i.u = Commons.ConvertToString(item["UnitID"]);
                TotalQuantity += i.q;
                ds.Add(i);
            }
            ViewBag.TotalQuantity = TotalQuantity.ToString("N0");
            ViewBag.data = ds;
            ViewBag.PalletID = PalletID;
            return View();
        }
        public ActionResult ViewNK()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-30).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            string sSQL = "exec SP_GetCountNK N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',1";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/viewnk?from=" + from.ToString() + "&to=" + to.ToString() + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }
        [HttpPost]
        public ActionResult Get_NK()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-30).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            string sSQL = "exec SP_GetNK N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",1";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            DivisionID = p["DivisionID"],
                            VoucherDate = Convert.ToDateTime(p["VoucherDate"]).ToString("dd/MM/yyyy"),
                            VoucherID = Commons.ConvertToString(p["VoucherID"]),
                            TransactionTypeID = p["TransactionTypeID"],
                            ObjectID = p["ObjectID"],
                            ObjectName = p["ObjectName"],
                            Description = p["Description"],
                            SAP = p["SAP"],
                            IW = p["IW"],
                            OW = p["OW"],


                        };
            return Json(query);
        }

        [HttpPost]
        public ActionResult DeleteVoucherNK(string VoucherID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            string sWrite = "exec SP_DeleteNK N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " ,'" + Commons.Fix(VoucherID) + "' ;";
            Exception ex = null;
            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult DeleteNKD(string ItemID, int AddNew)
        {
            W w = new W();

            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.WA;
            }
            else
            {
                token = GlobalVariables.WE;
            }

            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (token != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(token);

                }
                catch
                {

                }
            }
            int i = 0;
            bool f = false;
            foreach (WD item in w.WD)
            {
                if (item.ItemID == ItemID)
                {
                    f = true;

                    break;
                }
                i++;
            }

            if (f == true)
            {
                w.WD = RemoveList(w.WD, i);
            }

            if (AddNew == 1)
            {
                GlobalVariables.WA = JsonConvert.SerializeObject(w);
            }
            else
            {
                GlobalVariables.WE = JsonConvert.SerializeObject(w);
            }

            return Json(new { msg = "Xóa thành công", success = true });


        }
        [HttpPost]
        public ActionResult RestoreVoucher()
        {
            GlobalVariables.WE = "";
            GlobalVariables.WA = "";
            return Json(new { msg = "Phục hồi thành công", success = true });
        }
        [HttpPost]
        public ActionResult RestoreVoucherX()
        {
            GlobalVariables.XE = "";
            GlobalVariables.XA = "";
            return Json(new { msg = "Phục hồi thành công", success = true });
        }
        [HttpPost]
        public ActionResult UpdateW(string VoucherID, string VoucherDate, string WareHouseID)
        {
            W w = new W();
            //check data
            string[] l = VoucherDate.Split('/');
            if (l.Length != 3)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[0]) < 1 || Commons.ConvertToInt(l[0]) > 31)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[1]) < 1 || Commons.ConvertToInt(l[1]) > 12)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[2]) < 2000)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            DateTime d = DateTime.Now;
            try
            {
                d = new DateTime(Commons.ConvertToInt(l[2]), Commons.ConvertToInt(l[1]), Commons.ConvertToInt(l[0]));

            }
            catch
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });

            }

            if (WareHouseID.Trim() == "")
            {
                return Json(new { errorMsg = "Bạn chưa nhập mã kho", success = false });
            }

            if (GlobalVariables.WE != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(GlobalVariables.WE);
                }
                catch
                {
                }
            }
            string sWrite = "exec [SP_UpdateW] ";
            sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += ",'" + Commons.Fix(VoucherID) + "' ";
            sWrite += ",'" + Commons.Fix(WareHouseID) + "' ";
            sWrite += ",'' ";
            sWrite += ",'" + d.ToString("MM/dd/yyyy") + "'";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ";";
            sWrite += " delete WD where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and VoucherID = '" + Commons.Fix(VoucherID) + "'; ";
            sWrite += " delete WLD where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and VoucherID = '" + Commons.Fix(VoucherID) + "'; ";
            int i = 1;
            Exception ex = null;

            if (w.WD.Length > 0)
            {


                foreach (WD item in w.WD)
                {
                    sWrite += "exec [SP_InsertWD] ";
                    sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sWrite += ",'" + VoucherID + "_" + i.ToString("00") + "'";
                    sWrite += ",'" + VoucherID + "'";
                    sWrite += "," + i.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.ItemID) + "'";
                    sWrite += "," + item.Quantity.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.UnitID) + "'";
                    sWrite += ",1";
                    sWrite += ",N'" + Commons.Fix(item.ItemName) + "'";
                    sWrite += ";";
                    i++;
                }

                foreach (string item in w.Pallets)
                {
                    sWrite += "exec [SP_InsertWLD] N'" + GlobalVariables.DivisionID + "'";
                    sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                    sWrite += ",N'" + Commons.Fix(item) + "';";
                }

                bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (result)
                {
                    GlobalVariables.WE = "";
                    return Json(new { msg = "Cập nhật thành công", v = VoucherID, success = true });
                }

                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            else
            {
                return Json(new { errorMsg = "Chưa có dữ liệu chi tiết", success = false });

            }

        }

        public bool AddFromPallet(string PalletID, ref Exception ex, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.WA;
            }
            else
            {
                token = GlobalVariables.WE;
            }

            W w = new W();
            if (token != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(token);
                }
                catch
                {
                    w = new W();
                }

            }
            try
            {

                foreach (string item in w.Pallets)
                {
                    if (item == PalletID)
                    {
                        return false;
                    }
                }

                string sSQL = "select PalletID,ItemID,ItemName,Quantity,UnitID from PalletDetail ";
                sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += " and PalletID='" + Commons.Fix(PalletID) + "' order by OrderNo ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                foreach (DataRow r in dt.Rows)
                {
                    string ItemID = r["ItemID"].ToString();
                    int Quantity = Convert.ToInt32(r["Quantity"]);

                    bool hasexists = false;
                    foreach (WD item in w.WD)
                    {

                        if (r["ItemID"].ToString() == item.ItemID)
                        {
                            item.Quantity += Quantity;
                            hasexists = true;
                        }


                    }
                    if (hasexists == false)
                    {
                        WD m = new WD();
                        m.ItemID = r["ItemID"].ToString();
                        m.ItemName = r["ItemName"].ToString();
                        m.UnitID = r["UnitID"].ToString();
                        m.Quantity = Convert.ToInt32(r["Quantity"]);
                        w.WD = AddToList(w.WD, m);
                    }

                }
                string[] pp = new string[w.Pallets.Length + 1];
                int j = 0;
                foreach (string item in w.Pallets)
                {
                    pp[j++] = item;
                }
                pp[pp.Length - 1] = PalletID;
                w.Pallets = pp;

                if (AddNew == 1)
                {
                    GlobalVariables.WA = JsonConvert.SerializeObject(w);
                }
                else
                {
                    GlobalVariables.WE = JsonConvert.SerializeObject(w);
                }
            }
            catch (Exception exx)
            {
                ex = exx;
                return false;
            }

            return true;
        }


        public ActionResult PrintLocation()
        {
            int j = 0;
            string DivisionID = Commons.ConvertToString(Request.QueryString["id"]);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            string sSQL = "select Location from Locations where DivisionID='" + Commons.Fix(DivisionID) + "' and location like N'%" + Commons.Fix(Keyword) + "%' order by Location ";
            DataTable dt = Commons.GetData(sSQL);
            dt.Columns.Add("r", j.GetType());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["r"] = i;
            }
            ViewBag.data = dt.Rows;
            ViewBag.n = dt.Rows.Count;
            return View();
        }
        //xac thuc vi tri pallet
        public ActionResult XCVT()
        {
            return View();
        }
        public bool IsExistsRelative(string PalletID, string Location, string DivisionID)
        {
            string sSQL = "select PalletID,Location from Pallets ";
            sSQL += " where DivisionID='" + Commons.Fix(DivisionID) + "'";
            sSQL += " and Location = '" + Commons.Fix(Location) + "' ";
            sSQL += " and PalletID = '" + Commons.Fix(PalletID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult XNVT(string PalletID, string Location)
        {
            if (IsExistsRelative(PalletID, Location, GlobalVariables.DivisionID) == false)
            {
                return Json(new { errorMsg = "Pallet và vị trí này không khớp", success = false });

            }
            string sSQL = "select PalletID,Location from Pallets ";
            sSQL += " where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and Location = '" + Commons.Fix(Location) + "' ";
            sSQL += " and PalletID = '" + Commons.Fix(PalletID) + "' ";
            sSQL += " and isnull(Active,0) = 1 and isnull(Finish,0) = 0 ";

            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                string sWrite = "exec ConfirmNK  ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(PalletID) + "' ";
                Exception ex = null;
                bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (r == true)
                {
                    return Json(new { msg = "Xác nhận thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });

                }
            }
            return Json(new { errorMsg = "Pallet này đã được xác nhận rồi ", success = false });

        }

        [HttpPost]
        public ActionResult ConfirmAll(string sList)
        {
            string sWrite = "";
            List<CH> ar = new List<CH>();
            sList = sList.Trim();
            string[] lines = sList.Split('\n');



            foreach (string PalletID in lines)
            {
                sWrite += "exec ConfirmNK  ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(PalletID) + "';";

            }



            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r == true)
            {
                return Json(new { msg = "Xác nhận thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

        }
        public WD[] AddToList(WD[] list, WD item)
        {
            WD[] ar = new WD[list.Length + 1];
            for (int i = 0; i < list.Length; i++)
            {
                ar[i] = list[i];
            }
            ar[list.Length] = item;
            return ar;

        }
        public WD[] RemoveList(WD[] list, int pos)
        {
            WD[] ar = new WD[list.Length - 1];
            int j = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (i != pos)
                {
                    ar[j++] = list[i];
                }
            }
            return ar;

        }

        //xuat kho
        public ActionResult XK()
        {
            W w = new W();
            ViewBag.VoucherDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.ObjectID = "";
            ViewBag.ObjectName = "";
            ViewBag.WareHouseID = "";
            if (Global.Commons.CheckPermit("xk") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            if (GlobalVariables.XA != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(GlobalVariables.XA);
                }
                catch
                {

                }

            }
            int i = 0;

            try
            {
                foreach (WD item in w.WD)
                {
                    WD n = new WD();
                    n.ItemID = item.ItemID;
                    n.ItemName = item.ItemName;
                    n.UnitID = item.UnitID;
                    n.Quantity = item.Quantity;

                    i++;
                }
            }
            catch
            {


            }

            ViewBag.data = w.WD;
            ViewBag.VoucherDate = w.VoucherDate;
            ViewBag.ObjectID = w.ObjectID;
            ViewBag.ObjectName = w.ObjectName;
            ViewBag.WareHouseID = w.IW;
            ViewBag.palletlist = w.Pallets;



            return View();
        }


        [HttpPost]
        public ActionResult AddXK(string ItemID, int Quantity, string OutBound, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.XA;
            }
            else
            {
                token = GlobalVariables.XE;
            }

            W w = new W();

            if (ItemID == "")
            {
                return Json(new { errorMsg = "Không tìm thấy mã này", success = false });
            }

            string sSQL = "select ItemID,ItemName,UnitID from ItemVolumes where ItemID='" + Commons.Fix(ItemID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string ItemName = "";
            string UnitID = "";
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ItemName = r["ItemName"].ToString();
                UnitID = r["UnitID"].ToString();

                if (token != "")
                {
                    try
                    {
                        w = JsonConvert.DeserializeObject<W>(token);
                    }
                    catch
                    {
                    }

                }

                bool hasexists = false;
                foreach (WD item in w.WD)
                {
                    if (ItemID == item.ItemID && OutBound == item.OB)
                    {
                        item.Quantity += Quantity;
                        hasexists = true;
                        break;
                    }
                }
                if (hasexists == false)
                {
                    WD d = new WD();
                    d.ItemID = ItemID;
                    d.ItemName = ItemName;
                    d.UnitID = UnitID;
                    d.Quantity = Quantity;
                    d.OB = OutBound;
                    w.WD = AddToList(w.WD, d);

                }
                if (AddNew == 1)
                {
                    GlobalVariables.XA = JsonConvert.SerializeObject(w);
                }
                else
                {
                    GlobalVariables.XE = JsonConvert.SerializeObject(w);
                }

                return Json(new { msg = "Cập nhật thành công", success = true });

            }
            else
            {
                return Json(new { errorMsg = "Không tìm thấy mã này", success = false });
            }

        }

        [HttpPost]
        public ActionResult DeleteXK(string ItemID, string OutBound, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.XA;
            }
            else
            {
                token = GlobalVariables.XE;
            }

            try
            {
                if (token != "")
                {
                    W w = new W();
                    w = JsonConvert.DeserializeObject<W>(token);
                    int i = 0;
                    foreach (WD item in w.WD)
                    {
                        if (item.ItemID == ItemID && item.OB == OutBound)
                        {
                            w.WD = RemoveList(w.WD, i);
                            break;
                        }
                        i++;
                    }

                    if (AddNew == 1)
                    {
                        GlobalVariables.XA = JsonConvert.SerializeObject(w);
                    }
                    else
                    {
                        GlobalVariables.XE = JsonConvert.SerializeObject(w);
                    }

                    return Json(new { msg = "Xóa thành công", success = true });

                }

            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

            return Json(new { errorMsg = "Không tìm thấy mã này", success = false });
        }

        [HttpPost]
        public ActionResult SaveTTXK(string VoucherDate, int AddNew)
        {
            string token = "";
            if (AddNew == 1)
            {
                token = GlobalVariables.XA;
            }
            else
            {
                token = GlobalVariables.XE;
            }

            W w = new W();


            if (token != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(token);
                    w.VoucherDate = VoucherDate;
                    w.OW = "";
                }
                catch
                {


                }

            }
            if (AddNew == 1)
            {
                GlobalVariables.XA = JsonConvert.SerializeObject(w);
            }
            else
            {
                GlobalVariables.XE = JsonConvert.SerializeObject(w);
            }

            return Json(new { msg = "Đã lưu", success = true });
        }

        [HttpPost]
        public ActionResult AddX(string VoucherDate)
        {

            //check data
            string[] l = VoucherDate.Split('/');
            if (l.Length != 3)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[0]) < 1 || Commons.ConvertToInt(l[0]) > 31)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[1]) < 1 || Commons.ConvertToInt(l[1]) > 12)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[2]) < 2000)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            DateTime d = DateTime.Now;
            try
            {
                d = new DateTime(Commons.ConvertToInt(l[2]), Commons.ConvertToInt(l[1]), Commons.ConvertToInt(l[0]));

            }
            catch
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });

            }

            string sSQL = "exec [SP_GetWK] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string VoucherID = dt.Rows[0][0].ToString();

            string sWrite = "exec [SP_InsertW] ";
            sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += ",'" + Commons.Fix(VoucherID) + "' ";
            sWrite += ",'' ";
            sWrite += ",'' ";
            sWrite += ",'" + d.ToString("MM/dd/yyyy") + "'";
            sWrite += ",'GC'";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ";";
            int i = 1;
            Exception ex = null;
            if (GlobalVariables.XA != "")
            {

                W w = JsonConvert.DeserializeObject<W>(GlobalVariables.XA);
                if (w.WD.Length == 0)
                {
                    return Json(new { errorMsg = "Chưa có dữ liệu chi tiết", success = false });
                }

                foreach (WD item in w.WD)
                {
                    sWrite += "exec [SP_InsertWD] ";
                    sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sWrite += ",'" + VoucherID + "_" + i.ToString("00") + "'";
                    sWrite += ",'" + VoucherID + "'";
                    sWrite += "," + i.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.ItemID) + "'";
                    sWrite += "," + item.Quantity.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.UnitID) + "'";
                    sWrite += ",-1";
                    sWrite += ",N'" + Commons.Fix(item.ItemName) + "'";
                    sWrite += ",N'" + Commons.Fix(item.OB) + "'";
                    sWrite += ";";
                    i++;
                }

                bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (result)
                {
                    GlobalVariables.WA = "";
                    GlobalVariables.XA = "";
                    return Json(new { msg = "Thêm thành công", v = VoucherID, success = true });
                }

                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            else
            {
                return Json(new { errorMsg = "Chưa có dữ liệu chi tiết", success = false });

            }

        }

        public ActionResult SX()
        {
            string sSQL = "";
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            if (VoucherID == "")
            {
                Response.Redirect("~/admin");
            }

            ViewBag.VoucherID = VoucherID;
            ViewBag.xn = (IsXN(VoucherID) ? 1 : 0);

            W w = new W();
            if (GlobalVariables.XE != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(GlobalVariables.XE);
                }
                catch
                {
                }
            }
            DataTable dt = new DataTable();
            //if (VoucherID != w.VoucherID)
            //{
            w.VoucherID = VoucherID;
            sSQL = "select  VoucherDate, ObjectID, ObjectName, Description, SAP,OW,CheckBalance,isnull(PrintCount,0) PrintCount ";
            sSQL += ",OBCount from W where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID = '" + Commons.Fix(VoucherID) + "' and TransactionTypeID='GC' ";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                w.ObjectID = ViewBag.ObjectID = r["ObjectID"];
                w.ObjectName = ViewBag.ObjectName = r["ObjectName"];
                w.OW = ViewBag.WareHouseID = r["OW"];
                if (Commons.ConvertToInt(r["PrintCount"]) > 0)
                {
                    ViewBag.PrintCount = " (Đã in " + r["PrintCount"] + " lần)";
                }

                ViewBag.OBCount = " (Có " + r["OBCount"] + " đầu 8 )";

                if (Commons.ConvertToBool(r["CheckBalance"]) == false)
                {
                    ViewBag.Status = " (<span style='color:red'> Không kiểm tra tồn</span> )";
                }
                else
                {
                    ViewBag.Status = "";
                }

                w.Description = ViewBag.Description = r["Description"];
                w.SAP = ViewBag.SAP = r["SAP"];
                w.VoucherDate = ViewBag.VoucherDate = Convert.ToDateTime(r["VoucherDate"]).ToString("dd/MM/yyyy");
                sSQL = "exec SP_GetDetailWD1 N'" + Commons.Fix(VoucherID) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                dt = Commons.GetData(sSQL);
                EnumerableRowCollection<WD> query = from p in dt.AsEnumerable()
                                                    select new WD
                                                    {
                                                        TransactionID = "",
                                                        ItemID = Commons.ConvertToString(p["ItemID"]),
                                                        ItemName = Commons.ConvertToString(p["ItemName"]),
                                                        Quantity = Commons.ConvertToInt(p["Quantity"]),
                                                        Q = Commons.ConvertToInt(p["Q"]),
                                                        UnitID = Commons.ConvertToString(p["UnitID"]),
                                                        Location = Commons.ConvertToString(p["Location"])
                                                    };


                ViewBag.data = query;
                w.WD = query.ToArray();




                GlobalVariables.XE = JsonConvert.SerializeObject(w);

            }
            else
            {
                Response.Redirect("~/admin");
            }

            return View();

        }
        public bool HasPalletToOutPut(string sVoucherID)
        {
            string sSQL = "select PalletID from PalletDetailX ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and VoucherID=N'" + Commons.Fix(sVoucherID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult UpdateX(string VoucherID, string VoucherDate)
        {
            if (HasPalletToOutPut(VoucherID))
            {
                return Json(new { errorMsg = "Phiếu này đã được ấn định vị trí lấy hàng rồi . Bạn không được xóa sửa", success = false });
            }

            W w = new W();
            //check data
            string[] l = VoucherDate.Split('/');
            if (l.Length != 3)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[0]) < 1 || Commons.ConvertToInt(l[0]) > 31)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[1]) < 1 || Commons.ConvertToInt(l[1]) > 12)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            if (Commons.ConvertToInt(l[2]) < 2000)
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
            }

            DateTime d = DateTime.Now;
            try
            {
                d = new DateTime(Commons.ConvertToInt(l[2]), Commons.ConvertToInt(l[1]), Commons.ConvertToInt(l[0]));

            }
            catch
            {
                return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });

            }


            if (GlobalVariables.XE != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(GlobalVariables.XE);
                }
                catch
                {
                }
            }
            string sWrite = "exec [SP_UpdateW] ";
            sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += ",'" + Commons.Fix(VoucherID) + "' ";
            sWrite += ",'' ";
            sWrite += ",'' ";
            sWrite += ",'" + d.ToString("MM/dd/yyyy") + "'";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ";";
            sWrite += " delete WD where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and VoucherID = '" + Commons.Fix(VoucherID) + "'; ";
            sWrite += " delete WLD where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and VoucherID = '" + Commons.Fix(VoucherID) + "'; ";
            int i = 1;
            Exception ex = null;

            if (w.WD.Length > 0)
            {


                foreach (WD item in w.WD)
                {
                    sWrite += "exec [SP_InsertWD] ";
                    sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sWrite += ",'" + VoucherID + "_" + i.ToString("00") + "'";
                    sWrite += ",'" + VoucherID + "'";
                    sWrite += "," + i.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.ItemID) + "'";
                    sWrite += "," + item.Quantity.ToString("0");
                    sWrite += ",N'" + Commons.Fix(item.UnitID) + "'";
                    sWrite += ",-1";
                    sWrite += ",N'" + Commons.Fix(item.ItemName) + "'";
                    sWrite += ";";
                    i++;
                }

                foreach (string item in w.Pallets)
                {
                    sWrite += "exec [SP_InsertWLD] N'" + GlobalVariables.DivisionID + "'";
                    sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                    sWrite += ",N'" + Commons.Fix(item) + "';";
                }

                bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (result)
                {
                    GlobalVariables.XE = "";
                    return Json(new { msg = "Cập nhật thành công", v = VoucherID, success = true });
                }

                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            else
            {
                return Json(new { errorMsg = "Chưa có dữ liệu chi tiết", success = false });

            }

        }

        public ActionResult ViewXK()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("ViewXK") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec SP_GetCountNK N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(keyword) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Commons.ConvertToInt(dt.Rows[0][0]);
            int nSum = Commons.ConvertToInt(dt.Rows[0][1]);
            int nSum1 = Commons.ConvertToInt(dt.Rows[0][2]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/viewxk?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/viewxk?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/viewxk?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");
            ViewBag.Sum1 = nSum1.ToString("N0");
            ////outbound chua co vi tri
            //sSQL = "select count(OB) C,Sum(Quantity) Q ,sum(TotalAmount) T from OBChuaCoVT where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'";
            //DataTable db = Commons.GetData(sSQL);
            //int n = Convert.ToInt32(db.Rows[0][0]);
            //int Q = Commons.ConvertToInt(db.Rows[0][1]);
            //decimal TotalAmount = 0;
            //TotalAmount = Commons.ConvertToDecimal(db.Rows[0][2]);
            //if (n > 0)
            //{
            //    string content = "<p><a style='color:red' href='javascript:treovt()'>Có <strong style='color:#9c2323'>" + n.ToString("N0") + "</strong> ";
            //    content += "outbound lấy hàng với tổng số lượng  <strong style='color:#9c2323'>" + Q.ToString("N0") + "</strong> ";
            //    content += " và tổng số tiền <strong style='color:#9c2323'>" + TotalAmount.ToString("N0") + " VNĐ</strong>";
            //    content += " chưa bấm lấy hàng để xác định vị trí </a></p>";

            //    ViewBag.TreoVT = content;
            //}
            //else
            //{
            //    ViewBag.TreoVT = "";
            //}

            ////outbound treo
            //sSQL = "select count(OB) C,Sum(Quantity) Q,sum(TotalAmount) T from OBTreo where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'";
            //db = Commons.GetData(sSQL);
            //n = Convert.ToInt32(db.Rows[0][0]);
            //Q = Commons.ConvertToInt(db.Rows[0][1]);
            //TotalAmount = Commons.ConvertToDecimal(db.Rows[0][2]);
            //if (n > 0)
            //{
            //    string content = "<p><a style='color:red' href='javascript:treo()'>Có <strong style='color:#9c2323'>" + n.ToString("N0") + "</strong> ";
            //    content += "outbound lấy hàng với tổng số lượng  <strong style='color:#9c2323'>" + Q.ToString("N0") + "</strong> ";
            //    content += " và tổng số tiền <strong style='color:#9c2323'>" + TotalAmount.ToString("N0") + " VNĐ</strong>";
            //    content += " đang treo, có vị trí nhưng chưa xác nhận lấy hàng </a></p>";

            //    ViewBag.Treo = content;
            //}
            //else
            //{
            //    ViewBag.Treo = "";
            //}

            return View();
        }
        [HttpPost]
        public ActionResult GetXKNote()
        {
            try
            {
                string content = "";//outbound chua co vi tri
                string sSQL = "select count(OB) C,Sum(Quantity) Q ,sum(TotalAmount) T from OBChuaCoVT where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'";
                DataTable db = Commons.GetData(sSQL);
                int n = Convert.ToInt32(db.Rows[0][0]);
                int Q = Commons.ConvertToInt(db.Rows[0][1]);
                decimal TotalAmount = 0;
                TotalAmount = Commons.ConvertToDecimal(db.Rows[0][2]);
                if (n > 0)
                {
                    content += "<p><a style='color:red;font-style:italic' href='javascript:treovt()'>Có <strong style='color:#9c2323'>" + n.ToString("N0") + "</strong> ";
                    content += "outbound lấy hàng với tổng số lượng  <strong style='color:#9c2323'>" + Q.ToString("N0") + "</strong> ";
                    content += " và tổng số tiền <strong style='color:#9c2323'>" + TotalAmount.ToString("N0") + " VNĐ</strong>";
                    content += " chưa bấm lấy hàng để xác định vị trí </a></p>";


                }


                //outbound treo
                sSQL = "select count(OB) C,Sum(Quantity) Q,sum(TotalAmount) T from OBTreo where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'";
                db = Commons.GetData(sSQL);
                n = Convert.ToInt32(db.Rows[0][0]);
                Q = Commons.ConvertToInt(db.Rows[0][1]);
                TotalAmount = Commons.ConvertToDecimal(db.Rows[0][2]);
                if (n > 0)
                {
                    content += "<p><a style='color:red;font-style:italic' href='javascript:treo()'>Có <strong style='color:#9c2323'>" + n.ToString("N0") + "</strong> ";
                    content += "outbound lấy hàng với tổng số lượng  <strong style='color:#9c2323'>" + Q.ToString("N0") + "</strong> ";
                    content += " và tổng số tiền <strong style='color:#9c2323'>" + TotalAmount.ToString("N0") + " VNĐ</strong>";
                    content += " đang treo, có vị trí nhưng chưa xác nhận lấy hàng </a></p>";

                }

                //outbound treo chua in
                sSQL = "select count(OB) C,Sum(Quantity) Q,sum(TotalAmount) T from OBTreoChuaIn where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'";
                db = Commons.GetData(sSQL);
                n = Convert.ToInt32(db.Rows[0][0]);
                Q = Commons.ConvertToInt(db.Rows[0][1]);
                TotalAmount = Commons.ConvertToDecimal(db.Rows[0][2]);
                if (n > 0)
                {
                    content += "<p><a style='color:red;font-style:italic' href='javascript:treochuain()'>Có <strong style='color:#9c2323'>" + n.ToString("N0") + "</strong> ";
                    content += "outbound lấy hàng với tổng số lượng  <strong style='color:#9c2323'>" + Q.ToString("N0") + "</strong> ";
                    content += " và tổng số tiền <strong style='color:#9c2323'>" + TotalAmount.ToString("N0") + " VNĐ</strong>";
                    content += " đang treo, có vị trí nhưng chưa in </a></p>";

                }
                if (Commons.CheckPermit("obchuaracong"))
                {
                    sSQL = "select count(OB) C,Sum(TotalQuantityCT) Q,sum(TotalAmount) T from OBList where XN=N'Đã xác nhận' and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' and OB not in(select OB from OBLoaiTru) and OB not in(select OB from PRCD)  and convert(nvarchar(20),CreateDate,102)>='2019.11.01'";
                    db = Commons.GetData(sSQL);
                    n = Convert.ToInt32(db.Rows[0][0]);
                    Q = Commons.ConvertToInt(db.Rows[0][1]);
                    TotalAmount = Commons.ConvertToDecimal(db.Rows[0][2]);

                    if (n > 0)
                    {
                        content += "<p><a style='color:red;font-style:italic' href='javascript:chuaracong()'>Có <strong style='color:#9c2323'>" + n.ToString("N0") + "</strong> ";
                        content += "outbound lấy hàng với tổng số lượng  <strong style='color:#9c2323'>" + Q.ToString("N0") + "</strong> ";
                        content += " và tổng số tiền <strong style='color:#9c2323'>" + TotalAmount.ToString("N0") + " VNĐ</strong>";
                        content += " đã xác nhận lấy hàng nhưng chưa làm phiếu ra cổng </a></p>";

                    }
                }



                return Json(new { msg = content, success = true });
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });


            }



        }
        public ActionResult Treo()
        {
            string ssql = "";
            ssql += " exec GETVTTReo N'" + Fix(GlobalVariables.DivisionID) + "'  ";
            int Total = 0;
            DataTable dt = Commons.GetData(ssql);
            foreach (DataRow item in dt.Rows)
            {
                Total += Commons.ConvertToInt(item["Quantity"]);
            }
            ViewBag.data = dt.Rows;
            ViewBag.Total = Total.ToString("N0");
            return View();

        }
        public ActionResult ChuaRaCong()
        {

            string sSQL = "select OB,TotalQuantityCT,TotalAmount from OBList where XN=N'Đã xác nhận' and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' and OB not in(select OB from OBLoaiTru) and OB not in(select OB from PRCD)  and convert(nvarchar(20),CreateDate,102)>='2019.11.01' order by CreateDate,OB";
            DataTable dt = Commons.GetData(sSQL);

            ViewBag.data = dt.Rows;
            return View();

        }
        public ActionResult ExportChuaRaCong()
        {

            string sSQL = "exec [ExportOBChuaRaCong]  N'" + Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Controllers.Export();
            d.ExportExcel(Response, "ExportChuaRaCong", dt);
            return View();

        }
        public ActionResult TreoChuaIn()
        {
            string ssql = "";
            ssql += " exec GETVTTreoChuaIn N'" + Fix(GlobalVariables.DivisionID) + "'  ";
            int Total = 0;
            DataTable dt = Commons.GetData(ssql);
            foreach (DataRow item in dt.Rows)
            {
                Total += Commons.ConvertToInt(item["Quantity"]);
            }
            ViewBag.data = dt.Rows;
            ViewBag.Total = Total.ToString("N0");
            return View();

        }
        public ActionResult ExportTreo()
        {
            string ssql = "";
            ssql += " exec GETVTTReoForExcel N'" + Fix(GlobalVariables.DivisionID) + "'  ";
            DataTable dt = Commons.GetData(ssql);
            Export d = new Export();
            d.ExportExcel(Response, "Treo", dt);
            return View();
        }
        public ActionResult TreoVT()
        {
            string ssql = "select OB,VoucherID,Quantity  from OBChuaCoVT ";
            ssql += " where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' order by VoucherID ";
            int Total = 0;
            DataTable dt = Commons.GetData(ssql);
            foreach (DataRow item in dt.Rows)
            {
                Total += Commons.ConvertToInt(item["Quantity"]);
            }
            ViewBag.data = dt.Rows;
            ViewBag.Total = Total.ToString("N0");
            return View();

        }
        [HttpPost]
        public ActionResult Get_XK()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetNK N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(keyword) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);


            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            VoucherDate = Convert.ToDateTime(p["VoucherDate"]).ToString("dd/MM/yyyy HH:mm"),
                            VoucherID = Commons.ConvertToString(p["VoucherID"]),
                            DT = "<div style='max-height:200px;overflow:auto'>" + Commons.ConvertToString(p["OBHTML"]) + "<br/>" + Commons.ConvertToString(p["OBHTML2"]) + "</div>",
                            VT = (p["CLH"].ToString().IndexOf("Đã xác định") >= 0 ? "<span style='color:green'>" + p["CLH"].ToString() + "</span>" : p["CLH"].ToString()),
                            FullName = p["FullName"],
                            OB = Commons.ConvertToInt(p["OB"]).ToString("N0"),
                            Status = (p["Confirmed"].ToString().IndexOf("Đã xác nhận") >= 0 ? "<span style='color:green'>" + p["Confirmed"] + "</span>" : p["Confirmed"].ToString()),
                            PrintCount = p["PrintCount"],
                            Locked = (Commons.ConvertToBool(p["Locked"]) ? "Đã khóa" : "Chưa khóa"),
                            DH = (Commons.ConvertToBool(p["DH"]) ? "<span style='color:blue;font-weight:bold'>Đủ hàng</span>" : (Commons.ConvertToInt(p["DaDi"]) == 0 ? "<span style='color:red;font-weight:bold'>Thiếu hàng </span>" + GetHangThieu(p["VoucherID"].ToString(), p["Confirmed"].ToString()) : "<span style='color:blue;font-weight:bold'>Thiếu hàng và đã di " + Commons.ConvertToInt(p["DaDi"]) + " </span>")) +
                            (p["PrintCount"].ToString() == "0" ? "<p style='color:red;font-size:10px;font-style:italic;display:block;margin:3px;border:1px solid #ddd;border-radius:10px'>Chưa in</p>" : "<p style='color:green;font-size:10px;font-style:italic;display:block;margin:3px;border:1px solid #ddd;border-radius:10px'>Đã in " + p["PrintCount"] + " lần</p>")
                            ,
                            CheckBalance = (Commons.ConvertToBool(p["CheckBalance"]) ? "Kiểm kho" : "Không kiểm kho")
                        };
            return Json(query);
        }
        public string GetHangThieu(string VoucherID, string XN)
        {
            if (XN == "Chưa xác nhận lấy hàng")
            {
                return "<div style='max-height:150px;overflow:auto'>Chưa quét</div>";
            }

            string ssql = "exec SP_GetHangThieu N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            ssql += ",'" + Commons.Fix(VoucherID) + "'";
            DataTable dt = Commons.GetData(ssql);
            string sResult = "<div style='max-height:150px;overflow:auto'>";
            foreach (DataRow item in dt.Rows)
            {
                sResult += item["ItemID"] + ": " + Commons.ConvertToInt(item["Quantity"]).ToString("N0") + " / " + Commons.ConvertToInt(item["ReceiveQuantity"]).ToString("N0") + "<br/>";
            }
            sResult += "</div>";
            return sResult;
        }
        [HttpPost]
        public ActionResult DeleteVoucherXK(string VoucherID, string OB = "")
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Commons.CheckPermit("ViewXK") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });

            }
            string Message = "";
            bool b = DeleteWareHouseOutPut(VoucherID, ref Message, OB);

            if (b == true)
            {
                return Json(new { msg = Message, success = true });
            }
            else
            {
                return Json(new { errorMsg = Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult CheckPrintCount(string VoucherID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Commons.CheckPermit("ViewXK") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });

            }
            DataTable dt = Commons.GetData("select PrintCount from W where VoucherID=N'" + Fix(VoucherID) + "' and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'");

            if (dt.Rows.Count > 0)
            {

                return Json(new { msg = "Thành công", PrintCount = Commons.ConvertToInt(dt.Rows[0][0]), success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không tồn tại phiếu này", PrintCount = 0, success = false });
            }
        }
        [HttpPost]
        public ActionResult GetPickingListFromOB(string OB)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            DataTable dt = Commons.GetData("select top 1 WD.VoucherID,W.PrintCount from WD inner join W on WD.DivisionID=W.DivisionID and WD.VoucherID=W.VoucherID where WD.OB=N'" + Fix(OB) + "' and WD.DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'");

            if (dt.Rows.Count > 0)
            {

                return Json(new { msg = "Thành công", VoucherID = dt.Rows[0][0], PrintCount = dt.Rows[0][1], success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không tồn tại phiếu này", VoucherID = "", success = false });
            }
        }

        [HttpPost]
        public ActionResult DeleteVoucherXKForAdmin(string VoucherID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });

            }
            string Message = "";
            bool b = DeleteWareHouseOutPutForAdmin(VoucherID, ref Message);

            if (b == true)
            {
                return Json(new { msg = Message, success = true });
            }
            else
            {
                return Json(new { errorMsg = Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult xacnhantuquantri(string OB)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });

            }
            Exception ex = null;
            string Message = "Thành công";
            string sWrite = "update wd set ReceiveQuantity=Quantity where OB='" + Fix(OB) + "' and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += "exec SP_XNOB '" + Commons.Fix(OB) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);

            if (b == true)
            {
                return Json(new { msg = Message, success = true });
            }
            else
            {
                return Json(new { errorMsg = Message, success = false });
            }
        }
        public bool DeleteWareHouseOutPut(string VoucherID, ref string Message, string OB = "")
        {
            if (IsLocked(VoucherID))
            {
                Message = "Phiếu này đã bị khóa. Bạn không thể xóa";
                return false;
            }
            if (IsXN(VoucherID))// && GlobalVariables.IsAdmin == false)
            {
                Message = "Phiếu này đã xác nhận lấy hàng. Bạn không thể xóa. Vui lòng liên hệ quản trị";
                return false;
            }
            DataTable dt = GetLocationFromXH(VoucherID);

            string sWrite = "exec SP_DeleteNK N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " ,'" + Commons.Fix(VoucherID) + "','" + Fix(OB) + "' ;";

            foreach (DataRow item in dt.Rows)
            {
                sWrite += "exec [SP_UpdateVolumeUsed] '" + Commons.Fix(item["Location"].ToString()) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";
            }
            Exception ex = null;
            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                Message = "Xóa thành công";
                return true;
            }
            else
            {
                Message = ex.Message;
                return false;
            }

        }
        public bool DeleteWareHouseOutPutForAdmin(string VoucherID, ref string Message)
        {
            if (GlobalVariables.IsAdmin == false)
            {
                Message = "Bạn không phải admin nên không có quyền này";
                return false;
            }
            if (IsLocked(VoucherID))
            {
                Message = "Phiếu này đã bị khóa. Bạn không thể xóa";
                return false;
            }

            DataTable dt = GetLocationFromXH(VoucherID);

            string sWrite = "exec SP_DeleteNK N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " ,'" + Commons.Fix(VoucherID) + "','' ;";

            foreach (DataRow item in dt.Rows)
            {
                sWrite += "exec [SP_UpdateVolumeUsed] '" + Commons.Fix(item["Location"].ToString()) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";
            }
            Exception ex = null;
            bool flag = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (flag == true)
            {
                Message = "Xóa thành công";
                return true;
            }
            else
            {
                Message = ex.Message;
                return false;
            }

        }
        public string ListGift()
        {
            string sSQL = "select ItemID  from IES ";
            DataTable dt = Commons.GetData(sSQL);
            string sL = "' '";
            foreach (DataRow item in dt.Rows)
            {
                sL += ",N'" + item[0].ToString() + "'";
            }

            return sL;
        }


        //neu co ton tai hang nao khong lay duoc thi khong cho lay
        [HttpPost]
        public ActionResult GetProductsForVoucher1(string VoucherID)
        {

            try
            {
                PalletAndItem[] list = new PalletAndItem[0];
                ArrayList ar = new ArrayList();
                ArrayList ll = new ArrayList();

                string sSQL = "select ItemID,sum(Quantity) Quantity  from WD ";
                sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += " and VoucherID ='" + Commons.Fix(VoucherID) + "' ";

                sSQL += " and ItemID not in (" + ListGift() + ")";
                sSQL += " group by ItemID ";
                DataTable dt = Commons.GetData(sSQL);
                DataTable dt1 = Commons.GetData("select CheckBalance from W where VoucherID=N'" + Commons.Fix(VoucherID) + "' and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'");
                if (dt1.Rows.Count == 0)
                {
                    return Json(new { errorMsg = "Phiếu này không tồn tại hoặc đã bị xóa rồi", success = false });
                }

                bool CheckBalance = Commons.ConvertToBool(dt1.Rows[0][0]);
                string sWrite = "";
                string ssql = "exec [SP_LayHangVoucherV4] N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + Commons.Fix(VoucherID) + "'," + GlobalVariables.UserID.ToString("0") + ";";
              

                DataTable dd = Commons.GetData(ssql);
                string m = "";
                string obthieu = "";
                if (dd.Rows.Count > 0)
                {
                    foreach (DataRow item in dd.Rows)
                    {
                        if (item["ItemID"].ToString().Length > 20)//Có người đang chạy hệ thống xác định vị trí lấy hàng. Vui lòng thử lại sau vài giây
                        {
                            return Json(new { errorMsg = item["ItemID"], success = false });
                        }
                        if (obthieu.IndexOf(item["OB"].ToString()) < 0)
                        {
                            obthieu = obthieu + item["OB"].ToString() + "<br/>";
                        }

                        m += item["ItemID"].ToString() + " của đầu 8 " + item["OB"].ToString() + " thiếu hàng " + Commons.ConvertToInt(item["DL"]).ToString("N0") + "/ " + Commons.ConvertToInt(item["YC"]).ToString("N0") + "\n";
                    }

                    ssql = "select OB from WD where VoucherID=N'" + Fix(VoucherID) + "' and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' group by OB ";
                    DataTable ob = Commons.GetData(ssql);
                    string obdu = "";
                    foreach (DataRow item in ob.Rows)
                    {
                        if (obthieu.IndexOf(item[0].ToString()) < 0)
                        {
                            if (obdu.IndexOf(item[0].ToString()) < 0)
                            {
                                obdu = obdu + item[0].ToString() + "<br/>";
                            }
                        }
                    }

                    return Json(new { errorMsg = m, thieu = obthieu, du = obdu, success = false });

                }

                sWrite += "update W set DV=N'Đã xác định' where VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
                sWrite += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "';\n";
                sWrite += "exec SP_TinhLaiTheTichCuaPhieu N'" + Fix(VoucherID) + "',N'" + Fix(GlobalVariables.DivisionID) + "'\n;";
                Exception ex = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (b == false)
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }

                GlobalVariables.WA = "";
                GlobalVariables.WE = "";
                GlobalVariables.XA = "";
                GlobalVariables.XE = "";
                return Json(new { msg = "Thành công", success = true });



            }
            catch (Exception ex11)
            {
                ResetLocation(VoucherID);
                return Json(new { errorMsg = ex11.Message, success = false });


            }


        }
        public void ResetLocation(string VoucherID)
        {
            string sWrite = "exec [SP_ResetLocation] N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
            Commons.ExecuteNoneQuery(sWrite);


        }
        private void SetVirtualData(DataTable dt)
        {
            string ssql = "select Location from VTCH where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable db = Commons.GetData(ssql);
            foreach (DataRow item in dt.Rows)
            {
                db.DefaultView.RowFilter = "Location='" + item["Location"].ToString() + "'";
                if (db.DefaultView.Count > 0)
                {
                    item["Quantity"] = 999999;
                }
            }
        }
        private bool GetProductDetail(string ItemID, int Quantity, ref PalletAndItem[] list, bool checkbalance = true)
        {
            int n = Quantity;
            string sSQL = "exec GetBalance N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",'" + Commons.Fix(ItemID) + "'";

            bool finish = false;
            DataTable dt = Commons.GetData(sSQL);


            System.Collections.ArrayList ar = new ArrayList();
            foreach (DataRow item in dt.Rows)
            {
                if (n <= 0)
                {
                    break;
                }

                int q = Convert.ToInt32(item["Quantity"]);
                if (q >= n)
                {
                    PalletAndItem c = new PalletAndItem();
                    c.ItemID = ItemID;
                    c.PalletID = "";
                    c.Quantity = n;
                    c.Location = item["Location"].ToString();
                    c.LSX = "";
                    ar.Add(c);
                    finish = true;
                    n = 0;
                    break;
                }
                else
                {
                    PalletAndItem c = new PalletAndItem();
                    c.ItemID = ItemID;
                    c.PalletID = "";
                    c.Quantity = q;
                    c.Location = item["Location"].ToString();
                    c.LSX = "";
                    ar.Add(c);
                    n -= q;

                }
            }




            PalletAndItem[] l = new PalletAndItem[ar.Count];
            int i = 0;
            foreach (PalletAndItem item in ar)
            {
                l[i++] = item;
            }
            list = l;
            if (finish == false)
            {
                return false;
            }

            return true;
        }
        public void SaveT()
        {

        }

        [HttpPost]
        public ActionResult SaveDataLocationToprint(string L, string DivisionID)
        {
            int i = 0;
            L = L.Trim();
            DataTable dt = new DataTable();
            string sSQL = "declare @T as table (  Location nvarchar(20) )" + Environment.NewLine;


            foreach (string item in L.Split('\n'))
            {
                sSQL += "insert into @T values('" + Commons.Fix(item) + "')" + Environment.NewLine;

            }
            sSQL += " select t.Location,0 r  " + Environment.NewLine;
            sSQL += " from @T t inner join Locations l on t.location=l.location ";
            sSQL += " and l.divisionid='" + Commons.Fix(DivisionID) + "' " + Environment.NewLine;
            i = 0;
            dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                item["r"] = i++;
            }
            if (dt.Rows.Count != L.Split('\n').Length)
            {
                return Json(new { errorMsg = "Lỗi vị trí không hợp lệ", success = false });

            }

            Session["dt"] = dt;

            return Json(new { msg = "Thành công", success = true });

        }

        [HttpPost]
        public ActionResult SaveDataLocationToExport(string L, string DivisionID)
        {
            int i = 0;
            L = L.Trim();
            DataTable dt = new DataTable();

            string sSQL = "declare @T as table (  Location nvarchar(20) )" + Environment.NewLine;


            foreach (string item in L.Split('\n'))
            {
                sSQL += "insert into @T values(N'" + Commons.Fix(item) + "')" + Environment.NewLine;

            }
            sSQL += " select t.Location [Vị trí],sum(p.Quantity) [Số lượng],l.Volume [Sức chứa],l.VolumeUsed [Đã sử dụng],l.Volume-l.VolumeUsed [Còn lại] " + Environment.NewLine;
            sSQL += " from @T t inner join Locations l on t.location=l.location and l.divisionid='" + Commons.Fix(DivisionID) + "' " + Environment.NewLine;
            sSQL += " left join BalanceAll p on l.location=p.location and l.divisionid=p.divisionid " + Environment.NewLine;
            sSQL += " group by t.location,l.volume,l.volumeused ";
            //System.IO.File.WriteAllText("d:\\a.sql", sSQL);

            dt = Commons.GetData(sSQL);
            dt.Columns[0].ColumnName = "Vị trí";
            dt.Columns[1].ColumnName = "Số lượng";
            dt.Columns[2].ColumnName = "Sức chứa";
            dt.Columns[3].ColumnName = "Đã sử dụng";
            dt.Columns[4].ColumnName = "Còn lại";

            dt.Columns[0].Caption = "Vị trí";
            dt.Columns[1].Caption = "Số lượng";
            dt.Columns[2].Caption = "Sức chứa";
            dt.Columns[3].Caption = "Đã sử dụng";
            dt.Columns[4].Caption = "Còn lại";

            if (dt.Rows.Count != L.Split('\n').Length)
            {
                return Json(new { errorMsg = "Lỗi vị trí không hợp lệ", success = false });

            }
            Session["dtl"] = dt;

            return Json(new { msg = "Thành công", success = true });

        }

        [HttpPost]
        public ActionResult SaveDataLocationToExportAll(string DivisionID, string Keyword)
        {
            DataTable dt = new DataTable();

            string sSQL = "";
            sSQL += "exec  LocationToExportAll N'" + Commons.Fix(DivisionID) + "'";
            sSQL += ",N'" + Commons.Fix(Keyword) + "'";
            dt = Commons.GetData(sSQL);


            Session["dtl"] = dt;

            return Json(new { msg = "Thành công", success = true });

        }

        public ActionResult PrintL()
        {
            DataTable dt = new DataTable();
            int i = 0;
            dt.Columns.Add("Location");
            dt.Columns.Add("r", i.GetType());

            if (Session["dt"] != null)
            {
                try
                {
                    dt = (DataTable)Session["dt"];
                }
                catch
                {

                }
            }

            ViewBag.data = dt.Rows;

            return View();
        }

        public ActionResult ExportL()
        {
            DataTable dt = new DataTable();
            int i = 0;

            if (Session["dtl"] != null)
            {
                try
                {
                    dt = (DataTable)Session["dtl"];
                }
                catch
                {

                }
            }

            Export d = new Export();
            d.ToExcel(Response, dt, "location");
            return View();
        }
        public ActionResult PrintOutPut()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "select * from W where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "' and TransactionTypeID='GC' ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ViewBag.VoucherDate = Commons.ConvertToDateTime(r["VoucherDate"]).ToString("dd/MM/yyyy");
                ViewBag.WareHouseID = r["OW"];


            }
            sSQL = "select OB from WD where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " group by OB ";
            dt = Commons.GetData(sSQL);
            ViewBag.C = dt.Rows.Count;
            ViewBag.L = dt.Rows;

            sSQL = "exec SP_GetDetailWD2 N'" + Commons.Fix(VoucherID) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            dt = Commons.GetData(sSQL);
            SetPrintCount(VoucherID);
            ViewBag.data = dt.Rows;
            return View();
        }
        public void SetPrintCount(string VoucherID)
        {
            Commons.ExecuteNoneQuery("exec SP_UpdatePrintCount N'" + Fix(VoucherID) + "',N'" + Fix(GlobalVariables.DivisionID) + "'");

        }
        public ActionResult PrintOutPut1()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "select * from W where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "' and TransactionTypeID='GC' ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ViewBag.VoucherDate = Commons.ConvertToDateTime(r["VoucherDate"]).ToString("dd/MM/yyyy");
                ViewBag.WareHouseID = r["OW"];


            }
            sSQL = "select OB from WD where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "'  ";
            sSQL += " group by OB ";
            dt = Commons.GetData(sSQL);
            ViewBag.C = dt.Rows.Count;
            ViewBag.L = dt.Rows;

            sSQL = "exec SP_GetDetailWDLe N'" + Commons.Fix(VoucherID) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;
            SetPrintCount(VoucherID);
            return View();
        }
        public ActionResult PrintOutPut2()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "select * from W where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "' and TransactionTypeID='GC' ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ViewBag.VoucherDate = Commons.ConvertToDateTime(r["VoucherDate"]).ToString("dd/MM/yyyy");
                ViewBag.WareHouseID = r["OW"];


            }
            sSQL = "select OB from WD where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " group by OB ";
            dt = Commons.GetData(sSQL);
            ViewBag.C = dt.Rows.Count;
            ViewBag.L = dt.Rows;

            sSQL = "exec SP_GetDetailWDChan N'" + Commons.Fix(VoucherID) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;
            SetPrintCount(VoucherID);
            return View();
        }
        //xac thuc vi tri pallet
        public ActionResult XTVTLH()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XNVTLH(string PalletID, string Location, string VoucherID, string ItemID)
        {
            if (IsExistsRelative(PalletID, Location, GlobalVariables.DivisionID) == false)
            {
                return Json(new { errorMsg = "Pallet và vị trí này không khớp", success = false });

            }
            string sSQL = "select PalletID,Location from PalletDetailX ";
            sSQL += " where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and Location = '" + Commons.Fix(Location) + "' ";
            sSQL += " and PalletID = '" + Commons.Fix(PalletID) + "' ";
            sSQL += " and ItemID = '" + Commons.Fix(ItemID) + "' ";
            sSQL += " and isnull(Finish,0) = 0 ";

            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                string sWrite = "update PalletDetailX set Finish=1  ";
                sWrite += " where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += " and Location = '" + Commons.Fix(Location) + "' ";
                sWrite += " and PalletID = '" + Commons.Fix(PalletID) + "' ";
                sWrite += " and ItemID = '" + Commons.Fix(ItemID) + "' ";
                sWrite += " and isnull(Finish,0) = 0 ";
                Exception ex = null;
                bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (r == true)
                {
                    return Json(new { msg = "Cập nhật thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });

                }
            }
            return Json(new { errorMsg = "Hàng này đã được xác nhận trước rồi ", success = false });

        }
        public bool IsExistsRelativeLH(string ItemID, string PalletID, string Location, string VoucherID, string DivisionID)
        {
            string sSQL = "select PalletID,Location from PalletDetailX ";
            sSQL += " where DivisionID = N'" + Commons.Fix(DivisionID) + "'";
            sSQL += " and Location = N'" + Commons.Fix(Location) + "' ";
            sSQL += " and PalletID = N'" + Commons.Fix(PalletID) + "' ";
            sSQL += " and VoucherID = N'" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and ItemID = N'" + Commons.Fix(ItemID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public ActionResult ExportItemToExcel()
        {
            string sSQL = "exec [SP_ExportItemVolumes] N'" + Commons.Fix(Commons.ConvertToString(Request.QueryString["key"])) + "'";
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Export();
            d.ToExcel(Response, dt, "ItemList");
            return View();
        }

        private bool CheckTran(string PalletSourceID, string ItemID, string LSX, int Quantity)
        {
            string sSQL = "select dbo.FN_CheckItemBeforeTran (N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",'" + Commons.Fix(PalletSourceID) + "'";
            sSQL += ",'" + Commons.Fix(LSX) + "'";
            sSQL += ",'" + Commons.Fix(ItemID) + "'";
            sSQL += "," + Quantity.ToString("0");
            sSQL += ")";
            DataTable dt = Commons.GetData(sSQL);
            return Commons.ConvertToInt(dt.Rows[0][0]) == 1;

        }


        public ActionResult ChangeQuantity()
        {
            string PalletID = Commons.ConvertToString(Request.QueryString["type"]);
            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);
            string LSX = Commons.ConvertToString(Request.QueryString["lsx"]);

            string sSQL = "Select Quantity from PalletDetail ";
            sSQL += " where PalletID='" + Commons.Fix(PalletID) + "'";
            sSQL += " and ItemID='" + Commons.Fix(ItemID) + "'";
            sSQL += " and LSX='" + Commons.Fix(LSX) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Quantity = Commons.ConvertToInt(dt.Rows[0][0]).ToString("0");
            }

            return View();
        }
        [HttpPost]
        public ActionResult ChangeQuantity(int Quantity)
        {
            string PalletID = Commons.ConvertToString(Request.QueryString["type"]);
            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);
            string LSX = Commons.ConvertToString(Request.QueryString["lsx"]);

            string sWrite = "update PalletDetail set Quantity=" + Quantity.ToString("0");
            sWrite += " where PalletID='" + Commons.Fix(PalletID) + "'";
            sWrite += " and ItemID='" + Commons.Fix(ItemID) + "'";
            sWrite += " and LSX='" + Commons.Fix(LSX) + "'";
            bool b = Commons.ExecuteNoneQuery(sWrite);
            if (b)
            {
                ViewBag.MessageSuccess = "TC";
            }
            else
            {
                ViewBag.MessageSuccess = "TB";
            }
            return View();
        }

        public string GetNewVoucher()
        {
            string sSQL = "exec [SP_GetWK] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string VoucherID = dt.Rows[0][0].ToString();
            return VoucherID;
        }

        private void AddPalletLog(string PalletID, string NewContent)
        {
            string sSQL = "select LogContent from Pallets where PalletID=N'" + Commons.Fix(PalletID) + "' ";
            sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string OldContent = "";
            if (dt.Rows.Count > 0)
            {
                OldContent = Commons.ConvertToString(dt.Rows[0][0]);
                string Content = OldContent + "<p>" + NewContent + " : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt") + "</p>";
                string sWrite = "Update Pallets set LogContent = N'" + Commons.Fix(Content) + "' where PalletID = '" + Commons.Fix(PalletID) + "' ";
                sWrite += " and DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                Commons.ExecuteNoneQuery(sWrite);
            }


        }
        public ActionResult ViewPalletLog()
        {
            string PalletID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "select LogContent from Pallets where PalletID='" + Commons.Fix(PalletID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.content = Commons.ConvertToString(dt.Rows[0][0]);
            }
            else
            {
                ViewBag.content = "";
            }

            return View();
        }
        [HttpPost]
        public ActionResult Get_PalletDetail()
        {
            string PalletID = Commons.ConvertToString(Request.QueryString["id"]);

            string sSQL = "select Row_Number() over(order by D.OrderNo) TT,D.ItemID,I.ItemName,I.UnitID,D.Quantity,D.LSX from PalletDetail D inner join ItemVolumes I ";
            sSQL += " on D.ItemID=I.ItemID inner join Pallets M on M.PalletID=D.PalletID and M.DivisionID=D.DivisionID ";
            sSQL += " where M.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and D.PalletID='" + Commons.Fix(PalletID) + "' order by D.OrderNo";
            DataTable dt = Commons.GetData(sSQL);
            int Total = 0;
            foreach (DataRow item in dt.Rows)
            {
                Total += Convert.ToInt32(item["Quantity"]);
            }

            DataRow r = dt.NewRow();
            r["ItemID"] = "";
            r["ItemName"] = "<strong>Tổng cộng</strong>";
            r["UnitID"] = "";
            r["Quantity"] = Total;
            r["LSX"] = "";
            dt.Rows.Add(r);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            TT = p["TT"],
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = (p["ItemName"].ToString() == "<strong>Tổng cộng</strong>" ? "<strong>" + Commons.ConvertToInt(p["Quantity"]).ToString("N0") + "</strong>" : Commons.ConvertToInt(p["Quantity"]).ToString("N0")),
                            UnitID = p["UnitID"],
                            LSX = p["LSX"]
                        };
            return Json(query);

        }
        [HttpPost]
        public ActionResult Get_RemainDetail()
        {
            string LocationID = Commons.ConvertToString(Request.QueryString["id"]);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int cpage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string sSQL = "exec Get_RemainDetail N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",'" + Commons.Fix(LocationID) + "'";
            sSQL += ",'" + Commons.Fix(Keyword) + "'";
            sSQL += "," + cpage;




            DataTable dt = Commons.GetData(sSQL);



            int Total = 0;
            foreach (DataRow item in dt.Rows)
            {
                Total += Convert.ToInt32(item["Quantity"]);
            }

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            UnitID = p["UnitID"],
                            LSX = p["LSX"]
                        };
            return Json(query);

        }

        [HttpPost]
        public ActionResult Get_RemainDetailSum()
        {
            string LocationID = Commons.ConvertToString(Request.QueryString["id"]);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int cpage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string sSQL = "exec Get_RemainDetailCount N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",'" + Commons.Fix(LocationID) + "'";
            sSQL += ",'" + Commons.Fix(Keyword) + "'";
            sSQL += "," + cpage;




            DataTable dt = Commons.GetData(sSQL);
            return Json(new
            {
                msg = "Thành công",
                Count = Commons.ConvertToInt(dt.Rows[0][0]).ToString("N0")
                ,
                SL = Commons.ConvertToInt(dt.Rows[0][1]).ToString("N0"),
                success = true
            });



        }
        [HttpPost]
        public ActionResult Get_PalletTotalQuantity()
        {
            string PalletID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "select sum(D.Quantity) S from PalletDetail D inner join ItemVolumes I ";
            sSQL += " on D.ItemID=I.ItemID inner join Pallets M on M.PalletID=D.PalletID and M.DivisionID=D.DivisionID ";
            sSQL += " where M.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and D.PalletID='" + Commons.Fix(PalletID) + "' order by D.OrderNo";
            DataTable dt = Commons.GetData(sSQL);
            int Total = 0;
            Total = Commons.ConvertToInt(dt.Rows[0][0]);
            return Json(new { total = Total.ToString("N0") });

        }
        public ActionResult SSNK()
        {


            return View();
        }
        [HttpPost]
        public ActionResult GetOutBoundList(string Content)
        {
            if (Content == "")
            {
                return Json(new { msg = "", success = true });
            }

            GetAndPostController d = new GetAndPostController();
            Content = Content.Trim();
            Session["ssnk"] = Content;
            string l = "";
            foreach (string item in Content.Split('\n'))
            {
                bool b = d.DownloadOutBound(item);
                if (b == false)
                {
                    l += item + ", ";
                }
            }
            if (l != "")
            {
                return Json(new { errorMsg = "Outbound " + l + " không tồn tại", success = false });
            }

            return Json(new { msg = "Post thành công", success = true });

        }
        [HttpPost]
        public ActionResult Get_SS()
        {
            string PalletID = Commons.ConvertToString(Request.QueryString["id"]);
            string list = Commons.ConvertToString(Session["ssnk"]);
            string sOutBound = "";
            foreach (string item in list.Split('\n'))
            {
                sOutBound += ",'" + Commons.Fix(item) + "'";

            }
            sOutBound = sOutBound.Trim(',');

            string sSQL = "select  o.OutBound, o.ItemID,i.ItemName, o.Quantity, o.STO, i.UnitID";
            sSQL += " from OutBound o inner join ItemVolumes i on o.ItemID=i.ItemID ";
            sSQL += "where o.OutBound in(" + sOutBound + ")";

            DataTable dt = Commons.GetData(sSQL);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OutBound = p["OutBound"],
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = (p["ItemName"].ToString() == "<strong>Tổng cộng</strong>" ? "<strong>" + Commons.ConvertToInt(p["Quantity"]).ToString("N0") + "</strong>" : Commons.ConvertToInt(p["Quantity"]).ToString("N0")),
                            UnitID = p["UnitID"],
                            sTO = p["STO"]
                        };
            return Json(query);

        }
        //pallet so sanh
        public ActionResult GoToPallet()
        {
            if (Session["ssnk"] == null)
            {
                Response.Redirect("~/admin/ssnk");
            }

            string v1 = Commons.ConvertToString(Request.QueryString["fromdate"]);
            string v2 = Commons.ConvertToString(Request.QueryString["todate"]);
            string fromdate = DateTime.Now.ToString("dd/MM/yyyy"), todate = DateTime.Now.ToString("dd/MM/yyyy");
            string fromdate1 = DateTime.Now.ToString("yyyy.MM.dd"), todate1 = DateTime.Now.ToString("yyyy.MM.dd");
            string[] l1 = v1.Split('.');
            if (l1.Length == 3)
            {
                v1 = l1[2] + "/" + l1[1] + "/" + l1[0];
                fromdate1 = Commons.ConvertToString(Request.QueryString["fromdate"]);
            }

            else
            {
                v1 = fromdate;
            }

            string[] l2 = v2.Split('.');
            if (l2.Length == 3)
            {
                todate1 = Commons.ConvertToString(Request.QueryString["todate"]);
                v2 = l2[2] + "/" + l2[1] + "/" + l2[0];
            }

            else
            {
                v2 = todate;
            }

            ViewBag.fromdate = v1;
            ViewBag.todate = v2;

            DataTable dt = new DataTable();
            string sSQL = "select PalletID from Pallets ";
            sSQL += " where convert(nvarchar(20),CreateDate,102) between '" + Commons.Fix(fromdate1) + "'";
            sSQL += " and '" + Commons.Fix(todate1) + "' and Active=1";
            dt = Commons.GetData(sSQL);

            string chose = Commons.ConvertToString(Session["chose"]);
            DataTable dt1 = dt.Clone();
            foreach (string item in chose.Split(','))
            {
                DataRow r = dt1.NewRow();
                r[0] = item;
                dt1.Rows.Add(r);
            }
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                foreach (string item in chose.Split(','))
                {
                    if (dt.Rows[i][0].ToString() == item)
                    {
                        dt.Rows.RemoveAt(i);
                        break;
                    }
                }

            }
            ViewBag.data = dt.Rows;
            ViewBag.data1 = dt1.Rows;
            return View();
        }
        [HttpPost]
        public ActionResult MoveOne(string PalletID)
        {

            string chose = Commons.ConvertToString(Session["chose"]);
            System.Collections.ArrayList ar = new ArrayList();
            foreach (string item in chose.Split(','))
            {
                if (ar.IndexOf(item) < 0)
                {
                    ar.Add(item);
                }
            }
            foreach (string item in PalletID.Split(','))
            {
                if (ar.IndexOf(item) < 0)
                {
                    ar.Add(item);
                }
            }
            string tk = "";
            foreach (string item in ar)
            {
                tk += item + ",";
            }
            tk = tk.Trim(',');
            Session["chose"] = tk;
            return Json(new { msg = "Chuyển thành công", success = true });

        }
        [HttpPost]
        public ActionResult RemoveOne(string PalletID)
        {

            string chose = Commons.ConvertToString(Session["chose"]);
            System.Collections.ArrayList ar = new ArrayList();
            foreach (string item in chose.Split(','))
            {
                if (ar.IndexOf(item) < 0)
                {
                    ar.Add(item);
                }
            }
            foreach (string item in PalletID.Split(','))
            {
                int p = ar.IndexOf(item);
                if (p >= 0)
                {
                    ar.RemoveAt(p);
                }
            }
            string tk = "";
            foreach (string item in ar)
            {
                tk += item + ",";
            }
            tk = tk.Trim(',');
            Session["chose"] = tk;
            return Json(new { msg = "Xóa thành công", success = true });

        }

        public ActionResult Compare()
        {

            string list = "";

            int i = 0;
            foreach (string item in Commons.ConvertToString(Session["ssnk"]).Split('\n'))
            {
                if (i > 0)
                {
                    list += ",";
                }

                list += "'" + Commons.Fix(item) + "'";
                i++;
            }
            string sSQL = "select Row_Number() over (order by o.ItemID , o.OutBound) TT ";
            sSQL += ",o.OutBound,o.ItemID,i.ItemName,o.Quantity, isnull(Post1,0) Post1, isnull(Post,0) Post,o.Quantity CL from OutBound o inner join ItemVolumes i ";
            sSQL += " on o.ItemID=i.ItemID ";
            sSQL += "where o.OutBound in (" + list + ") order by o.OutBound , o.ItemID ";
            DataTable dt = Commons.GetData(sSQL);
            string chose = Commons.ConvertToString(Session["chose"]);
            list = "";
            i = 0;
            foreach (string item in chose.Split(','))
            {
                if (i > 0)
                {
                    list += ",";
                }

                list += "'" + Commons.Fix(item) + "'";
                i++;
            }
            sSQL = "select ItemID,sum(Quantity) Quantity,'' ItemName,0 colam from PalletDetail ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            sSQL += " and PalletID in(" + list + ") ";
            sSQL += " group by ItemID ";
            DataTable dt1 = Commons.GetData(sSQL);
            foreach (DataRow post in dt1.Rows)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["ItemID"].ToString() == post["ItemID"].ToString())
                    {
                        post["colam"] = 1;
                        if (Convert.ToInt32(post["Quantity"]) > Convert.ToInt32(item["Post1"]))
                        {
                            item["Post"] = item["Post1"];
                            item["CL"] = Convert.ToInt32(item["Quantity"]) - Convert.ToInt32(item["Post"]);
                            post["Quantity"] = Convert.ToInt32(post["Quantity"]) - Convert.ToInt32(item["Post1"]);
                        }
                        else
                        {
                            item["Post"] = post["Quantity"];
                            item["CL"] = Convert.ToInt32(item["Quantity"]) - Convert.ToInt32(item["Post"]);
                            post["Quantity"] = 0;
                            break;
                        }

                    }

                }
            }
            foreach (DataRow post in dt1.Rows)
            {
                foreach (DataRow item in dt.Rows)
                {

                    if (item["ItemID"].ToString() == post["ItemID"].ToString() && Convert.ToInt32(item["Post1"]) == 0)
                    {
                        post["colam"] = 1;
                        if (Convert.ToInt32(post["Quantity"]) > Convert.ToInt32(item["Quantity"]))
                        {
                            item["Post"] = item["Quantity"];
                            item["CL"] = 0;
                            post["Quantity"] = Convert.ToInt32(post["Quantity"]) - Convert.ToInt32(item["Quantity"]);

                        }
                        else
                        {
                            item["post"] = post["Quantity"];
                            item["CL"] = Convert.ToInt32(item["Quantity"]) - Convert.ToInt32(item["post"]);
                            post["Quantity"] = 0;
                            break;
                        }
                    }

                }
            }

            for (int j = dt1.Rows.Count - 1; j >= 0; j--)
            {
                if (Convert.ToInt32(dt1.Rows[j]["Quantity"]) <= 0)
                {
                    dt1.Rows.RemoveAt(j);
                }
            }
            ViewBag.dt1 = dt1.Rows;
            ViewBag.data = dt.Rows;
            return View();
        }
        public ActionResult ExportPalletLocation()
        {
            string sSQL = "exec [SP_ExportPalletHasLocation] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);

            Export d = new Export();
            d.ToExcel(Response, dt, "location");
            return View();
        }
        public ActionResult ExportPalletLocation1()
        {
            string sSQL = "exec [SP_ExportPalletHasLocation1] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);

            Export d = new Export();
            d.ToExcel(Response, dt, "location");
            return View();
        }
        [HttpPost]
        public ActionResult PostOutBound(ApiOrder.Models.OutBoundClass[] list)
        {
            string sWrite = "";
            foreach (OutBoundClass item in list)
            {

                if (item.Quantity >= 0)
                {
                    sWrite += "update outbound set post1=case when " + item.Quantity.ToString() + "<=Quantity then " + item.Quantity.ToString("0") + " else Quantity end ";
                    sWrite += " where itemid='" + Commons.Fix(item.ItemID) + "' ";
                    sWrite += " and outbound='" + Commons.Fix(item.OutBound) + "' ;";
                }

            }
            Commons.ExecuteNoneQuery(sWrite);

            return Json(new { msg = "post thành công", success = true });


        }
        public ActionResult ExportDataSS()
        {
            string list = "";

            int i = 0;
            foreach (string item in Commons.ConvertToString(Session["ssnk"]).Split('\n'))
            {
                if (i > 0)
                {
                    list += ",";
                }

                list += "'" + Commons.Fix(item) + "'";
                i++;
            }
            string sSQL = "select Row_Number() over (order by o.ItemID , o.OutBound) TT ";
            sSQL += ",o.OutBound,o.ItemID,i.ItemName,o.Quantity, isnull(Post1,0) Post1, isnull(Post,0) Post,o.Quantity CL from OutBound o inner join ItemVolumes i ";
            sSQL += " on o.ItemID=i.ItemID ";
            sSQL += "where o.OutBound in (" + list + ") order by o.ItemID , o.OutBound ";
            DataTable dt = Commons.GetData(sSQL);
            string chose = Commons.ConvertToString(Session["chose"]);
            list = "";
            i = 0;
            foreach (string item in chose.Split(','))
            {
                if (i > 0)
                {
                    list += ",";
                }

                list += "'" + Commons.Fix(item) + "'";
                i++;
            }
            sSQL = "select ItemID,sum(Quantity) Quantity,'' ItemName from PalletDetail ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            sSQL += " and PalletID in(" + list + ") ";
            sSQL += " group by ItemID ";
            DataTable dt1 = Commons.GetData(sSQL);
            foreach (DataRow post in dt1.Rows)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["ItemID"].ToString() == post["ItemID"].ToString())
                    {
                        if (Convert.ToInt32(post["Quantity"]) > Convert.ToInt32(item["Post1"]))
                        {
                            item["Post"] = item["Post1"];
                            item["CL"] = Convert.ToInt32(item["Quantity"]) - Convert.ToInt32(item["Post"]);
                            post["Quantity"] = Convert.ToInt32(post["Quantity"]) - Convert.ToInt32(item["Post1"]);
                        }
                        else
                        {
                            item["Post"] = post["Quantity"];
                            item["CL"] = Convert.ToInt32(item["Quantity"]) - Convert.ToInt32(item["Post"]);
                            post["Quantity"] = 0;
                            break;
                        }
                    }

                }
            }
            foreach (DataRow post in dt1.Rows)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["ItemID"].ToString() == post["ItemID"].ToString() && Convert.ToInt32(item["Post1"]) == 0)
                    {
                        if (Convert.ToInt32(post["Quantity"]) > Convert.ToInt32(item["Quantity"]))
                        {
                            item["Post"] = item["Quantity"];
                            item["CL"] = 0;
                            post["Quantity"] = Convert.ToInt32(post["Quantity"]) - Convert.ToInt32(item["Quantity"]);

                        }
                        else
                        {
                            item["post"] = post["Quantity"];
                            item["CL"] = Convert.ToInt32(item["Quantity"]) - Convert.ToInt32(item["post"]);
                            post["Quantity"] = 0;
                            break;
                        }
                    }

                }
            }

            Export d = new Export();
            d.ToExcel(Response, dt, "ketquass");
            return View();
        }
        public ActionResult DiHang()
        {
            return View();
        }

        public ActionResult DiHang1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPalletForXK(string ItemID, int Quantity)
        {
            string sSQL = "exec SP_GetItemFromXK ";
            sSQL += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",N'" + Commons.Fix(ItemID) + "'";
            sSQL += "," + Quantity.ToString("0");
            sSQL += ";";
            DataTable dt = Commons.GetData(sSQL);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            PalletID = p["PalletID"],
                            Title = p["Title"]

                        };
            return Json(query);
        }
        public string CheckOBList(Dau8[] list)
        {
            string sResult = "";
            int hc = 0, hn = 0, chtt = 0, tgpp = 0;
            string ob = "";
            DataTable MCHC = Commons.GetData("select * from MCHC");
            DataTable ies = Commons.GetData("select * from IEs");
            string HCList = "";
            string HNList = "";
            foreach (Dau8 item in list)
            {
                string OB = item.VBELN;
                ies.DefaultView.RowFilter = "ItemID='" + item.MATNR + "'";
                if (ies.DefaultView.Count > 0)
                {
                    continue;
                }

                if (OB.Substring(0, 2) == "88")
                {
                    chtt++;
                }
                else
                {
                    tgpp++;
                    if (ob != item.VBELN)
                    {
                        ob = item.VBELN;
                        hc = 0;
                        hn = 0;
                    }
                    MCHC.DefaultView.RowFilter = "MC='" + item.MATKL + "'";
                    if (MCHC.DefaultView.Count > 0)
                    {
                        hc++;

                        if (HCList.IndexOf(MCHC.DefaultView[0]["MC"].ToString()) < 0)
                        {
                            if (HCList != "")
                            {
                                HCList += "," + MCHC.DefaultView[0]["MC"].ToString();
                            }
                            else
                            {
                                HCList = MCHC.DefaultView[0]["MC"].ToString();
                            }
                        }
                    }

                    else
                    {
                        hn++;
                        if (HNList.IndexOf(item.MATKL) < 0)
                        {
                            if (HNList != "")
                            {
                                HNList += "," + item.MATKL;
                            }
                            else
                            {
                                HNList = item.MATKL;
                            }
                        }
                    }

                    if (hc > 0 && hn > 0)
                    {
                        sResult = "MC hàng chậm và hàng nóng phải tách riêng ra \n MC chậm: " + HCList;
                        sResult += " \n MC nóng: " + HNList;
                    }

                }
                if (chtt > 0 && tgpp > 0)
                {
                    sResult = "Phải tách riêng cửa hàng tiếp thị ra";


                }

            }

            return sResult;
        }
        public void AddSTO(ref System.Collections.ArrayList ar, STO e)
        {
            foreach (STO item in ar)
            {
                if (item.VoucherID == e.VoucherID && item.OB == e.OB)
                {
                    if (item.WH.IndexOf(e.WH) < 0)
                    {
                        if (item.WH != "")
                        {
                            item.WH += "," + e.WH;
                        }
                        else
                        {
                            item.WH = e.WH;
                        }
                    }
                    if (item.ST.IndexOf(e.ST) < 0)
                    {
                        if (item.ST != "")
                        {
                            item.ST += "," + e.ST;
                        }
                        else
                        {
                            item.ST = e.ST;
                        }
                    }
                    return;
                }
            }

            ar.Add(e);
        }
        public void SaveSTO(System.Collections.ArrayList ar)
        {
            string sWrite = "";
            foreach (STO item in ar)
            {
                sWrite += "update wd set WH='" + Commons.Fix(item.WH) + "'";
                sWrite += ",STO='" + Commons.Fix(item.ST) + "' ";
                sWrite += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += " and VoucherID='" + Commons.Fix(item.VoucherID) + "' ";
                sWrite += " and OB='" + Commons.Fix(item.OB) + "'; ";

            }
            Commons.ExecuteNoneQuery(sWrite);

        }
        [HttpPost]
        public ActionResult NhanHang(string OutBound, string VoucherDate, int CheckBalance, string loai = "")
        {
            try
            {
                System.Collections.ArrayList STO = new ArrayList();

                if (OutBound.Trim() == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập đầu 8", success = false });

                }

                string reffff = "";
                if (CheckAlreadyOutBound(OutBound, ref reffff))
                {
                    return Json(new { errorMsg = "Có đầu 8 đã nhận rồi bao gồm " + reffff.Trim(','), success = false });
                }
                OutBound = OutBound.Replace(" ", "");
                OutBound = OutBound.Trim();

                //check data
                string[] l = VoucherDate.Split('/');
                if (l.Length != 3)
                {
                    return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
                }

                if (Commons.ConvertToInt(l[0]) < 1 || Commons.ConvertToInt(l[0]) > 31)
                {
                    return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
                }

                if (Commons.ConvertToInt(l[1]) < 1 || Commons.ConvertToInt(l[1]) > 12)
                {
                    return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
                }

                if (Commons.ConvertToInt(l[2]) < 2000)
                {
                    return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });
                }

                DateTime d = DateTime.Now;
                try
                {
                    d = new DateTime(Commons.ConvertToInt(l[2]), Commons.ConvertToInt(l[1]), Commons.ConvertToInt(l[0]));

                }
                catch
                {
                    return Json(new { errorMsg = "Ngày phiếu không hợp lệ", success = false });

                }


                string TableName = "user" + GlobalVariables.UserID.ToString("0");
                DataTable detail = new DataTable();
                int q = 0;
                decimal money = 0;
                detail.Columns.Add("TransactionID");
                detail.Columns.Add("VoucherID");
                detail.Columns.Add("ItemID");
                detail.Columns.Add("OrderNo", q.GetType());
                detail.Columns.Add("OB");
                detail.Columns.Add("Note");
                detail.Columns.Add("District");
                detail.Columns.Add("KindOf");
                detail.Columns.Add("ContactNo");
                detail.Columns.Add("ContactDes");
                detail.Columns.Add("UnitID");
                detail.Columns.Add("Quantity", q.GetType());
                detail.Columns.Add("CustomerName");
                detail.Columns.Add("Address");
                detail.Columns.Add("Price", money.GetType());
                detail.Columns.Add("Discount");
                detail.Columns.Add("PrepaingDate");

                detail.TableName = "user" + GlobalVariables.UserID.ToString("0");
                string tablescript = "select TransactionID,VoucherID,ItemID,OrderNo,OB,Note,District";
                tablescript += ",KindOf,ContactNo,ContactDes,UnitID,Quantity,CustomerName,Address";
                tablescript += ",Price,Discount,PrepaingDate into " + TableName + " from WD where 1=2 ";
                Commons.ExecuteNoneQuery("drop table " + TableName);
                Commons.ExecuteNoneQuery(tablescript);

                string sWrite = "";
                Exception ex = null;
                if (GlobalVariables.ForOne == false || loai == "ao")
                {
                    string[] ob = OutBound.Split('\n');
                    string ss = "";
                    for (int j = 0; j < ob.Length; j++)
                    {
                        if (j > 0)
                        {
                            ss += ",";
                        }

                        ss += ob[j];

                    }
                    GetAndPostController gp = new GetAndPostController();
                    Dau8[] ds = gp.GetOutBound(OutBound, 2);
                    if (ds.Length == 0)
                    {
                        return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
                    }
                    if (loai == "ao")
                    {
                        //do nothing
                    }
                    else
                    {
                        string s = CheckOBList(ds);
                        if (s != "")
                        {
                            return Json(new { errorMsg = s, success = false });

                        }
                    }
                    if (CheckAlreadyOutBound(OutBound, ref reffff))
                    {
                        return Json(new { errorMsg = "Có đầu 8 đã nhận rồi bao gồm " + reffff.Trim(','), success = false });
                    }

                    string sSQL = "exec [SP_GetWK] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    DataTable dt = Commons.GetData(sSQL);
                    string VoucherID = dt.Rows[0][0].ToString();
                    sWrite += "exec [SP_InsertW] ";
                    sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += ",'" + Commons.Fix(VoucherID) + "' ";
                    sWrite += ",'' ";
                    sWrite += ",'' ";
                    sWrite += ",'" + d.ToString("MM/dd/yyyy") + "'";
                    sWrite += ",'GC'";
                    sWrite += ",N''";
                    sWrite += ",N''";
                    sWrite += ",N''";
                    sWrite += "," + GlobalVariables.UserID.ToString("0");
                    sWrite += ",N'" + Commons.Fix(ss) + "'";//outbound luu dang cach nhau dau phay
                    sWrite += "," + CheckBalance.ToString();
                    sWrite += ";";
                    int i = 1;


                    //Commons.ExecuteNoneQuery(sWrite);

                    foreach (Dau8 item in ds)
                    {
                        //sWrite += "exec [SP_InsertWD] ";
                        //sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                        //sWrite += ",'" + VoucherID + "_" + i.ToString("00") + "'";
                        //sWrite += ",'" + VoucherID + "'";
                        //sWrite += "," + i.ToString("0");
                        //sWrite += ",N'" + Commons.Fix(item.MATNR) + "'";
                        //sWrite += "," + int.Parse(item.LFIMG).ToString("0");
                        //sWrite += ",N'" + Commons.Fix(item.MEINS) + "'";
                        //sWrite += ",-1";
                        //sWrite += ",N''";
                        //sWrite += ",N'" + Commons.Fix(item.VBELN) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.KBETR) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.DISC) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.ERDAT) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.VGBEL) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.LGOBE) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.NAME) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.ADDRESS) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.REGION) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.VTEXT) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.HOPDONG) + "'";
                        //sWrite += ",N'" + Commons.Fix(item.DIENGIAI) + "'";

                        //sWrite += ";";

                        DataRow r = detail.NewRow();
                        r["TransactionID"] = VoucherID + "_" + i.ToString("00");
                        r["VoucherID"] = VoucherID;
                        r["OrderNo"] = i;
                        r["ItemID"] = item.MATNR;
                        r["OB"] = item.VBELN;
                        r["Note"] = item.DIENGIAI;
                        r["District"] = item.REGION;
                        r["KindOf"] = item.VTEXT;
                        r["ContactNo"] = item.HOPDONG;
                        r["ContactDes"] = item.DIENGIAI;
                        r["UnitID"] = item.MEINS;
                        r["Quantity"] = int.Parse(item.LFIMG);
                        r["CustomerName"] = item.NAME;
                        r["Address"] = item.ADDRESS;
                        r["Discount"] = item.DISC;
                        r["Price"] = item.KBETR;
                        r["PrepaingDate"] = item.WADAT;
                        detail.Rows.Add(r);

                        STO e = new STO();
                        e.OB = item.VBELN;
                        e.ST = item.VGBEL;
                        e.WH = item.LGOBE;
                        e.VoucherID = VoucherID;
                        AddSTO(ref STO, e);
                        i++;
                        //if (i % 500 == 0)
                        //{
                        //    bool b11 = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        //    sWrite = "";
                        //    if (b11 == false)
                        //    {
                        //        return Json(new { errorMsg = ex.Message, success = false });
                        //    }
                        //}
                    }
                    //sWrite += " delete AllowList where OutBound in(select OB from WD where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and VoucherID = N'" + Commons.Fix(VoucherID) + "');";

                    using (SqlConnection destinationConnection = new SqlConnection(Commons.ConnectionString))
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                    {
                        destinationConnection.Open();
                        bulkCopy.DestinationTableName = TableName;
                        bulkCopy.WriteToServer(detail);
                        destinationConnection.Close();

                    }

                    string sWriteData = "insert into WD ( DivisionID, TransactionID, VoucherID, OrderNo, ItemID, Quantity, UnitID, TransactionX, Note, OB, ReceiveQuantity, Price, Discount, PrepaingDate, STO, WH, CustomerName, Address, District, KindOf, ContactNo, ContactDes) ";
                    sWriteData += "select N'" + Commons.Fix(GlobalVariables.DivisionID) + "' DivisionID, max(TransactionID), VoucherID, max(OrderNo), ItemID, sum(Quantity), max(UnitID), -1, max(Note), OB, 0, max(Price), max(Discount), min(PrepaingDate), '' STO,'' WH, max(CustomerName), max(Address), max(District), max(KindOf), max(ContactNo), max(ContactDes) ";
                    sWriteData += " from " + TableName + " where VoucherID='" + VoucherID + "'";
                    sWriteData += " group by    VoucherID,ItemID,OB ;";

                    sWrite += sWriteData;
                    sWrite += "exec SP_UpdateOBHTML '" + VoucherID + "' ,N'" + Fix(GlobalVariables.DivisionID) + "';";

                    bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    if (result)
                    {

                        SaveSTO(STO);
                        GlobalVariables.WA = "";
                        return Json(new { msg = "Thêm thành công", v = VoucherID, success = true });
                    }

                    else
                    {
                        return Json(new { errorMsg = ex.Message, success = false });
                    }
                }
                else //1 dau 8 1 phieu
                {
                    string[] ob = OutBound.Split('\n');
                    string ss = "";
                    for (int j = 0; j < ob.Length; j++)
                    {
                        if (j > 0)
                        {
                            ss += ",";
                        }

                        ss += ob[j];

                    }
                    int sd = 0;
                    foreach (string oo in ob)
                    {
                        GetAndPostController gp = new GetAndPostController();
                        Dau8[] ds = gp.GetOutBoundForOne(oo, 2);
                        if (ds.Length == 0)
                        {
                            return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
                        }

                        string s = CheckOBList(ds);
                        if (s != "")
                        {
                            return Json(new { errorMsg = s, success = false });

                        }
                        string sSQL = "exec [SP_GetWK] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                        DataTable dt = Commons.GetData(sSQL);
                        string VoucherID = dt.Rows[0][0].ToString();
                        sWrite += "exec [SP_InsertW] ";
                        sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                        sWrite += ",'" + Commons.Fix(VoucherID) + "' ";
                        sWrite += ",'' ";
                        sWrite += ",'' ";
                        sWrite += ",'" + d.ToString("MM/dd/yyyy") + "'";
                        sWrite += ",'GC'";
                        sWrite += ",N''";
                        sWrite += ",N''";
                        sWrite += ",N''";
                        sWrite += "," + GlobalVariables.UserID.ToString("0");
                        sWrite += ",N'" + Commons.Fix(oo) + "'";//outbound luu dang cach nhau dau phay
                        sWrite += "," + CheckBalance.ToString();
                        sWrite += ";";
                        int i = 1;




                        foreach (Dau8 item in ds)
                        {
                            sWrite += "exec [SP_InsertWD] ";
                            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                            sWrite += ",'" + VoucherID + "_" + i.ToString("00") + "'";
                            sWrite += ",'" + VoucherID + "'";
                            sWrite += "," + i.ToString("0");
                            sWrite += ",N'" + Commons.Fix(item.MATNR) + "'";
                            sWrite += "," + int.Parse(item.LFIMG).ToString("0");
                            sWrite += ",N'" + Commons.Fix(item.MEINS) + "'";
                            sWrite += ",-1";
                            sWrite += ",N''";
                            sWrite += ",N'" + Commons.Fix(item.VBELN) + "'";
                            sWrite += ",N'" + Commons.Fix(item.KBETR) + "'";
                            sWrite += ",N'" + Commons.Fix(item.DISC) + "'";
                            sWrite += ",N'" + Commons.Fix(item.ERDAT) + "'";
                            sWrite += ",N'" + Commons.Fix(item.VGBEL) + "'";
                            sWrite += ",N'" + Commons.Fix(item.LGOBE) + "'";
                            sWrite += ",N'" + Commons.Fix(item.NAME) + "'";
                            sWrite += ",N'" + Commons.Fix(item.ADDRESS) + "'";
                            sWrite += ",N'" + Commons.Fix(item.REGION) + "'";
                            sWrite += ",N'" + Commons.Fix(item.VTEXT) + "'";
                            sWrite += ",N'" + Commons.Fix(item.HOPDONG) + "'";
                            sWrite += ",N'" + Commons.Fix(item.DIENGIAI) + "'";

                            sWrite += ";";

                            STO e = new STO();
                            e.OB = item.VBELN;
                            e.ST = item.VGBEL;
                            e.WH = item.LGOBE;
                            e.VoucherID = VoucherID;
                            AddSTO(ref STO, e);

                            i++;
                        }
                        sd++;
                        sWrite += "exec SP_UpdateOBHTML '" + VoucherID + "' ,N'" + Fix(GlobalVariables.DivisionID) + "';";

                        //sWrite += " delete AllowList where OutBound in(select OB from WD where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and VoucherID = N'" + Commons.Fix(VoucherID) + "');";
                        if (sd % 7 == 0)
                        {
                            bool b11 = Commons.ExecuteNoneQuery(sWrite, ref ex);
                            sWrite = "";
                            if (b11 == false)
                            {
                                return Json(new { errorMsg = ex.Message, success = false });
                            }
                        }
                    }
                    if (sWrite.Trim() != "")
                    {
                        bool b11 = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        sWrite = "";
                        if (b11 == false)
                        {
                            return Json(new { errorMsg = ex.Message, success = false });
                        }

                    }
                    //bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    //if (result)
                    //{
                    GlobalVariables.WA = "";
                    SaveSTO(STO);
                    return Json(new { msg = "Thêm thành công", v = "", success = true });
                    //}

                    //else
                    //{
                    //    return Json(new { errorMsg = ex.Message, success = false });
                    //}
                }
            }
            catch (Exception x)
            {

                return Json(new { errorMsg = x.Message, success = false });

            }






        }
        public ActionResult SXX()
        {
            return View();
        }
        public int GetSLYC(string VoucherID, string ItemID)
        {
            DataTable dt = new DataTable();
            //kiem tra ma hang 
            string sSQL = "select sum(Quantity) Quantity from WD where ItemID='" + Commons.Fix(ItemID) + "' ";
            sSQL += " and VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Commons.ConvertToInt(dt.Rows[0][0]);
            }

            return 0;
        }
        public int GetSLDL(string VoucherID, string ItemID)
        {
            DataTable dt = new DataTable();
            //kiem tra ma hang 
            string sSQL = "select sum(Quantity) Quantity from XH where ItemID='" + Commons.Fix(ItemID) + "' ";
            sSQL += " and VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Commons.ConvertToInt(dt.Rows[0][0]);
            }

            return 0;
        }
        public int GetSLDL(string VoucherID, string ItemID, string Location)
        {
            DataTable dt = new DataTable();
            //kiem tra ma hang 
            string sSQL = "select sum(Quantity) Quantity from XH where ItemID='" + Commons.Fix(ItemID) + "' ";
            sSQL += " and VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and Location='" + Commons.Fix(Location) + "' ";

            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Commons.ConvertToInt(dt.Rows[0][0]);
            }

            return 0;
        }
        [HttpPost]
        public ActionResult ThayDoi(string ItemID, string VoucherID, int Quantity, string NewLocation, string OldLocation)
        {
            if (Commons.CheckPermit("ql") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            if (IsLocked(VoucherID))
            {
                return Json(new { errorMsg = "Phiếu này đã bị khóa bạn không thể cập nhật", success = false });
            }

            if (Quantity < 0)
            {
                return Json(new { errorMsg = "Số lượng không hợp lệ", success = false });
            }

            string sSQL = "";
            DataTable dt = new DataTable();
            //kiem tra ma hang 
            sSQL = "select ItemID from ItemVolumes where ItemID='" + Commons.Fix(ItemID) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Mã hàng này không đúng", success = false });
            }

            bool CheckBalance = true;
            sSQL = "select VoucherID,CheckBalance from W where VoucherID = N'" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu này không đúng", success = false });
            }

            CheckBalance = Commons.ConvertToBool(dt.Rows[0][1]);

            if (CheckLocationExists(NewLocation) == false)
            {
                return Json(new { errorMsg = "Vị trí này không tồn tại", success = false });
            }

            int SLYC = GetSLYC(VoucherID, ItemID);
            int SLDL = GetSLDL(VoucherID, ItemID);

            if (SLYC < SLDL - GetSLDL(VoucherID, ItemID, OldLocation) + Quantity)
            {
                return Json(new { errorMsg = "Bạn không được lấy dư hàng " + ItemID + " . Số lượng yêu cầu chỉ có " + SLYC.ToString("N0"), success = false });

            }




            try
            {


                string sWrite = "exec SP_DeleteXH N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(OldLocation) + "'";
                sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "';";
                sWrite += "exec SP_UpdateVolumeUsed N'" + Commons.Fix(OldLocation) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";


                sSQL = "select sum(Quantity) Quantity, Location ";
                sSQL += " from [BalanceAll] where ItemID=N'" + Commons.Fix(ItemID) + "' ";
                sSQL += " And DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += " And Location = N'" + Commons.Fix(NewLocation) + "' ";
                sSQL += " group by Location ";
                sSQL += " having sum(Quantity) >0 ";
                dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return Json(new { errorMsg = "Không đủ số lượng", success = false });
                }

                if (Commons.ConvertToInt(dt.Rows[0]["Quantity"]) < Quantity)
                {
                    return Json(new { errorMsg = "Không đủ số lượng", success = false });

                }

                sWrite += "exec SP_InsertXH N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                sWrite += ",N'" + Commons.Fix(NewLocation) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + Quantity.ToString("0");
                if (CheckBalance)
                {
                    sWrite += ",1";
                }
                else
                {
                    sWrite += ",0";
                }

                sWrite += ";";



                if (OldLocation != "")
                {
                    sWrite += "exec [SP_UpdateVolumeUsed] '" + Commons.Fix(OldLocation) + "','" + Commons.Fix(GlobalVariables.DivisionID) + "';";
                }

                sWrite += "exec [SP_UpdateVolumeUsed] '" + Commons.Fix(NewLocation) + "','" + Commons.Fix(GlobalVariables.DivisionID) + "';";


                Exception eee = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref eee);
                if (b == true)
                {
                    return Json(new { msg = "Đổi thành công", v = VoucherID, success = true });
                }
                else
                {
                    return Json(new { errorMsg = eee.Message, success = false });
                }
            }
            catch (Exception te)
            {
                return Json(new { errorMsg = te.Message, success = false });


            }

            return Json(new { errorMsg = "Lỗi", success = false });


        }
        public ActionResult ChiaHang()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("chiahang") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            return View();
        }
        public ActionResult ChiaHang1()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("chiahang") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            if (IsHasLocation(VoucherID) == false)
            {
                Response.Redirect("~/admin");
            }

            string sSQL = "select VoucherDate from W where VoucherID='" + Commons.Fix(VoucherID) + "' and DivisionID = '" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.VoucherDate = Commons.ConvertToDateTime(dt.Rows[0][0]).ToString("dd/MM/yyyy");
            }
            sSQL = "select OB from WD where VoucherID='" + Commons.Fix(VoucherID) + "' and DivisionID = N'" + Commons.ConvertToString(GlobalVariables.DivisionID) + "' ";
            sSQL += " group by OB order by OB ";
            dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;
            ViewBag.IsLocked = IsLocked(VoucherID);
            return View();
        }
        public int GetQuantityFromLocation(string ItemID, string LSX, string Location)
        {
            string sSQL = "select sum(Quantity) Quantity from BalanceAll where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location=N'" + Commons.Fix(Location) + "' and ItemID=N'" + Commons.Fix(ItemID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Commons.ConvertToInt(dt.Rows[0][0]);
            }

            return 0;
        }
        //cho lay dang khong chuan

        public List<CC> SetLSXFromLocation(CC I, string Location)
        {
            int q = I.q;
            List<CC> results = new List<CC>();

            string sSQL = "exec getlsx N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + Commons.Fix(I.i) + "','" + Commons.Fix(Location) + "'";
            sSQL += ",'" + Commons.Fix(I.lsx) + "'";

            DataTable dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                int Quantity = Commons.ConvertToInt(item["Quantity"]);
                string LSX = Commons.ConvertToString(item["LSX"]);

                if (Quantity >= I.q)
                {
                    CC r = new CC();
                    r.i = I.i;
                    r.lsx = LSX;
                    r.q = I.q;
                    I.q = 0;
                    results.Add(r);
                    break;
                }
                else if (Quantity > 0)
                {
                    CC r = new CC();
                    r.i = I.i;
                    r.lsx = LSX;
                    r.q = Quantity;
                    I.q = I.q - Quantity;
                    results.Add(r);

                }
            }
            if (I.q > 0)
            {
                return new List<CC>();
            }

            return results;
        }

        public int GetQuantityFromLocationNotNeedLSX(string ItemID, string Location)
        {
            string sSQL = "select sum(Quantity) Quantity from [BalanceAll] where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location=N'" + Commons.Fix(Location) + "' and ItemID=N'" + Commons.Fix(ItemID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Commons.ConvertToInt(dt.Rows[0][0]);
            }

            return 0;
        }

        [HttpPost]
        public ActionResult DiHang(string LSX, string ItemID, int Quantity, string LocationID, string NewLocationID)
        {
            LocationID = LocationID.ToUpper();
            NewLocationID = NewLocationID.ToUpper();
            try
            {
                if (CheckLocationExists(LocationID) == false)
                {
                    return Json(new { errorMsg = "Không tồn tại vị trí nguồn này", success = false });

                }



                if (CheckLocationExists(NewLocationID) == false)
                {
                    return Json(new { errorMsg = "Không tồn tại vị trí đích này", success = false });

                }

            }
            catch (Exception loi)
            {

                return Json(new { errorMsg = loi.Message, success = false });

            }
            int remain = GetQuantityFromLocation(ItemID, LSX, LocationID);
            int remain1 = GetQuantityFromLocationNotNeedLSX(ItemID, LocationID);
            if (remain > remain1)
            {
                remain = remain1;
            }

            if (remain < Quantity)
            {
                return Json(new { errorMsg = "Số lượng mã này không đủ. Hiện chỉ còn " + remain.ToString("N0"), success = false });

            }

            string sWrite = "";

            sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += ",'" + Commons.Fix(NewLocationID) + "'";
            sWrite += ",1";
            sWrite += ",'" + Commons.Fix(ItemID) + "'";
            sWrite += ",'" + Commons.Fix(LSX) + "'";
            sWrite += "," + Quantity.ToString("0");
            sWrite += ";";
            sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += ",'" + Commons.Fix(LocationID) + "'";
            sWrite += ",-1";
            sWrite += ",'" + Commons.Fix(ItemID) + "'";
            sWrite += ",'" + Commons.Fix(LSX) + "'";
            sWrite += "," + Quantity.ToString("0");
            sWrite += ";";
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                //ghi nhat ky
                sWrite = "exec SP_TranItemLog ";
                sWrite += " N'" + Commons.Fix(LocationID) + "'";
                sWrite += ",N'" + Commons.Fix(NewLocationID) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                Commons.ExecuteNoneQuery(sWrite);
                return Json(new { msg = "Cập nhật thành công", success = true });
            }

            return Json(new { errorMsg = ex.Message, success = false });

        }
        public CC GetItemFromBarCode(string BarCode)
        {
            BarCode = BarCode.ToUpper();

            CC r = new CC();
            if (BarCode.Length == 18)
            {
                r = GetFrom18(BarCode);
                r.q = 1;

            }
            else if (BarCode.Length == 27)
            {

                r = GetFrom27(BarCode);
                r.q = GetQuantity(BarCode, r.i);
            }
            else if (BarCode.Length == 14)
            {
                r = GetFromItem(BarCode);
                r.q = 1;

            }
            else
            {
                r.i = "";
                r.q = 1;
            }
            return r;
        }
        [HttpPost]
        public ActionResult TimHangTuViTri(string BarCode, string LocationID)
        {
            BarCode = BarCode.ToUpper();

            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (BarCode == "")
            {
                return Json(new { errorMsg = "Bạn chưa nhập barcode hoặc mã hàng", success = false });
            }

            CC r = GetItemFromBarCode(BarCode);

            if (r.i != "")
            {
                string sSQL = "select Row_Number() over(order by D.ItemID,D.LSX) TT,D.ItemID";

                sSQL += ",I.ItemName,I.UnitID,sum(D.Quantity) Quantity,D.Location,D.LSX";
                sSQL += " from ViewPallets D inner join ItemVolumes I on D.ItemID = I.ItemID ";
                sSQL += " where D.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sSQL += " and D.Location='" + Commons.Fix(LocationID) + "' ";
                sSQL += " group by D.ItemID,I.ItemName,I.UnitID,D.Location,D.LSX ";
                sSQL += " order by D.ItemID,D.LSX ";
                DataTable dt = Commons.GetData(sSQL);
                int p = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (r.lsx == "" || r.lsx == null)
                    {
                        if (dt.Rows[i]["ItemID"].ToString() == r.i)
                        {
                            return Json(new { msg = "Thành công", pos = i, success = true });

                        }

                    }

                    else
                    {

                        if (dt.Rows[i]["ItemID"].ToString() == r.i || dt.Rows[i]["LSX"].ToString() == r.lsx)
                        {
                            return Json(new { msg = "Thành công", pos = i, success = true });

                        }
                    }
                }

            }

            return Json(new { errorMsg = "Không tìm thấy", success = false });

        }
        [HttpPost]
        public ActionResult CheckOutBoundForItem(string BarCode, string VoucherID)
        {
            BarCode = BarCode.ToUpper();

            string sWrite = "";
            string sSQL = "";
            if (IsLocked(VoucherID))
            {
                return Json(new { errorMsg = "Phiếu này đã bị khóa rồi. Bạn không thể xóa,sửa", success = false });
            }

            List<CC> sL = new List<CC>();
            bool bc = false;

            if (BarCode.Length == 18 && BarCode.Substring(BarCode.Length - 1, 1) != "0")//neu tem dây 18
            {
                sL = GetFrom1811(BarCode);
                bc = true;
            }
            //neu la tem happy
            if (BarCode.Length == 27 && (BarCode.Substring(16, 1) == "H" || BarCode.Substring(14, 1) == "H"))
            {

                sL = GetFromHappyBitis(BarCode);
                bc = true;
            }

            if (bc == true)
            {
                if (sL.Count == 0)
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }

                CC f = sL[0];
                string lid = "";
                foreach (CC item in sL)
                {
                    if (lid == "")
                    {
                        lid = "N'" + Commons.Fix(item.i) + "'";
                    }
                    else
                    {
                        lid += ",N'" + Commons.Fix(item.i) + "'";
                    }
                }
                //lay ra dau outbound 
                sSQL = "select top 1 D.OB,D.ItemID,I.ItemName,sum(D.Quantity) Quantity,sum(isnull(D.ReceiveQuantity,0)) ReceiveQuantity from WD D ";
                sSQL += " inner join ItemVolumes I on D.ItemID=I.ItemID ";
                sSQL += " where D.ItemID='" + Commons.Fix(f.i) + "' ";
                sSQL += " And D.VoucherID='" + Commons.Fix(VoucherID) + "' ";
                sSQL += " And D.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sSQL += " group by D.OB,D.ItemID,I.ItemName  ";
                sSQL += " having sum(D.Quantity-isnull(D.ReceiveQuantity,0))>0 ";
                sSQL += " order by sum(D.Quantity-isnull(D.ReceiveQuantity,0)) desc ";
                DataTable db = Commons.GetData(sSQL);
                if (db.Rows.Count == 0)
                {
                    return Json(new { errorMsg = "Không tìm thấy", success = false });
                }

                int c = Commons.ConvertToInt(db.Rows[0][0]);
                if (c != sL.Count)
                {
                    return Json(new { errorMsg = "Đầu outbound này không có đủ danh mục của " + f.i.Substring(0, 9), success = false });
                }

                int q = Convert.ToInt32(db.Rows[0]["Quantity"]);
                int r = Convert.ToInt32(db.Rows[0]["ReceiveQuantity"]);


                if (q < r + f.q)
                {
                    return Json(new { errorMsg = "Dư hàng " + f.i, success = false });

                }

                string o = db.Rows[0]["OB"].ToString();
                //kiem tra co du ma theo day khong
                sSQL = "select count(ItemID) c from WD  ";
                sSQL += " where ItemID in(" + lid + ") ";
                sSQL += " And VoucherID='" + Commons.Fix(VoucherID) + "' ";
                sSQL += " And OB='" + Commons.Fix(o) + "' ";
                sSQL += " And DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                db = Commons.GetData(sSQL);

                //toi buoc nay la co du danh muc
                sWrite = "update WD set ReceiveQuantity=isnull(ReceiveQuantity,0) + " + f.q;
                sWrite += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += " and ItemID in(" + lid + ")  and OB = N'" + Commons.Fix(o) + "' ";
                sWrite += " and VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
                Exception ex11 = null;
                bool b1 = Commons.ExecuteNoneQuery(sWrite, ref ex11);
                if (b1 == false)
                {
                    return Json(new { errorMsg = ex11.Message, success = false });
                }

                db = GetDataOutBound(VoucherID);
                for (int t = db.Rows.Count - 1; t >= 0; t--)
                {
                    if (db.Rows[t]["OB"].ToString() != o)
                    {
                        db.Rows.RemoveAt(t);
                    }
                }
                var query2 = from p in db.AsEnumerable()
                             select new
                             {
                                 OB = p["OB"],
                                 ItemID = p["ItemID"],
                                 ItemName = p["ItemName"],
                                 Quantity = Convert.ToInt32(p["Quantity"]).ToString("N0"),
                                 ReceiveQuantity = (Convert.ToInt32(p["ReceiveQuantity"])).ToString("N0")

                             };



                db = GetDataOutBoundFinished(VoucherID);
                var query3 = from p in db.AsEnumerable()
                             select new
                             {
                                 OB = p["OB"]
                             };
                return Json(new { success = true, msg = "Thành công", itemid = f.i, ob = o, list = query2, list1 = query3 });

            }



            CC i = GetItemFromBarCode(BarCode);

            if (i.i == "" || i.i == null)
            {
                return Json(new { errorMsg = "Không tìm thấy", success = false });
            }

            sSQL = "select top 1 D.OB,D.ItemID,I.ItemName,sum(D.Quantity) Quantity,sum(isnull(D.ReceiveQuantity,0)) ReceiveQuantity from WD D ";
            sSQL = "select top 1 D.OB,D.ItemID,I.ItemName,sum(D.Quantity) Quantity,sum(isnull(D.ReceiveQuantity,0)) ReceiveQuantity from WD D ";
            sSQL += " inner join ItemVolumes I on D.ItemID=I.ItemID ";
            sSQL += " where D.ItemID='" + Commons.Fix(i.i) + "' and D.ItemID not in(" + ListGift() + ")";
            sSQL += " And D.VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " And D.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " group by D.OB,D.ItemID,I.ItemName  ";
            sSQL += " having sum(D.Quantity-isnull(D.ReceiveQuantity,0))>0 ";
            sSQL += " order by D.OB ";

            if (BarCode.Length >= 27)
            {
                sSQL = "select top 1 D.OB,D.ItemID,I.ItemName,sum(D.Quantity) Quantity,sum(isnull(D.ReceiveQuantity,0)) ReceiveQuantity from WD D ";
                sSQL += " inner join ItemVolumes I on D.ItemID=I.ItemID ";
                sSQL += " where D.ItemID='" + Commons.Fix(i.i) + "' and ItemID not in (" + ListGift() + ")";
                sSQL += " And D.VoucherID='" + Commons.Fix(VoucherID) + "' ";
                sSQL += " And D.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sSQL += " group by D.OB,D.ItemID,I.ItemName  ";
                sSQL += " having sum(D.Quantity-isnull(D.ReceiveQuantity,0))>0 ";
                sSQL += " order by sum(D.Quantity-isnull(D.ReceiveQuantity,0)) desc ";
            }

            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Không tìm thấy hoặc dư hàng", success = false });
            }

            string OB = dt.Rows[0]["OB"].ToString();
            int Quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]);
            int ReceiveQuantity = Convert.ToInt32(dt.Rows[0]["ReceiveQuantity"]);

            string ItemID = dt.Rows[0]["ItemID"].ToString();

            if (Quantity < ReceiveQuantity + i.q)
            {
                return Json(new { errorMsg = "Dư hàng " + i.i, success = false });

            }
            sWrite = "update WD set ReceiveQuantity=isnull(ReceiveQuantity,0) + " + i.q;
            sWrite += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += " and ItemID = N'" + Commons.Fix(i.i) + "' and OB = N'" + Commons.Fix(OB) + "' ";
            sWrite += " and VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

            dt = GetDataOutBound(VoucherID);
            for (int t = dt.Rows.Count - 1; t >= 0; t--)
            {
                if (dt.Rows[t]["OB"].ToString() != OB)
                {
                    dt.Rows.RemoveAt(t);
                }
            }
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OB = p["OB"],
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = Convert.ToInt32(p["Quantity"]).ToString("N0"),
                            ReceiveQuantity = (Convert.ToInt32(p["ReceiveQuantity"])).ToString("N0")

                        };



            dt = GetDataOutBoundFinished(VoucherID);


            var query1 = from p in dt.AsEnumerable()
                         select new
                         {
                             OB = p["OB"]


                         };
            return Json(new { success = true, msg = "Thành công", itemid = ItemID, ob = OB, list = query, list1 = query1 });

        }
        public DataTable GetDataOutBound(string VoucherID)
        {
            string sSQL = "select D.OB,D.ItemID,I.ItemName,D.Quantity,isnull(D.ReceiveQuantity,0) ReceiveQuantity ";
            sSQL += " from WD D inner join ItemVolumes I on D.ItemID=I.ItemID ";
            sSQL += " where VoucherID=N'" + Commons.Fix(VoucherID) + "'";
            sSQL += " and D.ItemID not in(" + ListGift() + ")";
            sSQL += " and DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " order by D.OB,D.ItemID ";
            DataTable dt = Commons.GetData(sSQL);
            return dt;


        }
        public DataTable GetDataOutBoundFinished(string VoucherID)
        {
            string sSQL = "select OB from WD where VoucherID=N'" + Commons.Fix(VoucherID) + "'";
            sSQL += " and ItemID not in(" + ListGift() + ")";
            sSQL += " and DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " group by OB ";
            sSQL += "  having sum(Quantity-isnull(ReceiveQuantity,0) )=0 ";
            DataTable dt = Commons.GetData(sSQL);

            return dt;


        }
        [HttpPost]
        public ActionResult GetStatusOutBoundForItem(string VoucherID)
        {
            DataTable dt = GetDataOutBoundFinished(VoucherID);

            var query1 = from p in dt.AsEnumerable()
                         select new
                         {
                             OB = p["OB"]
                         };

            dt = GetDataOutBound(VoucherID);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OB = p["OB"],
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = Convert.ToInt32(p["Quantity"]).ToString("N0"),
                            ReceiveQuantity = Convert.ToInt32(p["ReceiveQuantity"]).ToString("N0")


                        };

            return Json(new { success = true, msg = "Thành công", list = query, list1 = query1 });

        }
        [HttpPost]
        public ActionResult Get_RemainCombo()
        {
            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);

            string sSQL = "select sum(b.Quantity) Quantity,b.Location,b.Location Title,l.Odd from BalanceAll b inner join locations l on b.divisionid=l.divisionid And b.location=l.location ";
            sSQL += " where b.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and b.ItemID='" + Commons.Fix(ItemID) + "'";
            sSQL += " group by b.Location,l.Odd ";

            sSQL += " having sum(b.Quantity)>0 ";
            sSQL += " order by l.Odd desc,b.Location ";
            DataTable dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                item[2] = item[2] + " - " + (Commons.ConvertToBool(item["Odd"]) ? "Lẻ" : "Chẳn") + " : " + Commons.ConvertToInt(item[0]).ToString("N0");
            }
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            Location = p["Location"],
                            Title = p["Title"],

                        };
            return Json(query);

        }
        public bool CheckAlreadyOutBound(string OutBound, ref string list1)
        {

            string sL = "";
            string[] list = OutBound.Split('\n');
            foreach (string item in list)
            {
                if (item.Trim() == "")
                {
                    continue;
                }

                if (sL != "")
                {
                    sL += ",'" + Commons.Fix(item.Trim()) + "'";
                }
                else
                {
                    sL += "'" + Commons.Fix(item.Trim()) + "'";
                }
            }
            string sSQL = "select OB,VoucherID from WD where OB in (" + sL + ") and OB not in (select OutBound from AllowList) group by OB,VoucherID";
            DataTable dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                list1 += "(đầu 8: " + item[0] + " - phiếu lấy hàng: " + item[1] + ") ,";
                //list1 += item[1]+ "<br/>";
            }


            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult ResetXK(string VoucherID, string LocationID, string ItemID)
        {
            if (IsLocked(VoucherID))
            {
                return Json(new { errorMsg = "Phiếu này đã bị khóa bạn không thể cập nhật", success = false });
            }

            string sWrite = "exec SP_DeleteXH N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(LocationID) + "'";
            sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
            sWrite += ",N'" + Commons.Fix(ItemID) + "';";
            sWrite += "exec SP_UpdateVolumeUsed N'" + Commons.Fix(LocationID) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

            return Json(new { msg = "Thành công", success = true });

        }
        [HttpPost]
        public ActionResult ResetAll(string VoucherID)
        {
            if (IsLocked(VoucherID))
            {
                return Json(new { errorMsg = "Phiếu này đã bị khóa bạn không thể cập nhật", success = false });
            }

            if (IsXN(VoucherID) && GlobalVariables.IsAdmin == false)
            {

                return Json(new { errorMsg = "Phiếu này đã xác nhận lấy hàng. Bạn không thể khởi tạo lại. Vui lòng liên hệ quản trị", success = false });

            }

            string sSQL = "select distinct Location from XH where VoucherID ='" + Commons.Fix(VoucherID) + "' and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string sWrite = "";
            sWrite = "exec SP_ResetLocation N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",'" + Commons.Fix(VoucherID) + "'";
            foreach (DataRow item in dt.Rows)
            {
                string LocationID = item[0].ToString();
                sWrite += "exec SP_UpdateVolumeUsed N'" + Commons.Fix(LocationID) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

            }
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

            return Json(new { msg = "Thành công", success = true });

        }
        public ActionResult InDau8ThieuHang()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);


            DataTable dt = Commons.GetData("exec SP_GetOutBoundDetailNotScan '" + Commons.Fix(VoucherID) + "', N'" + Commons.Fix(GlobalVariables.DivisionID) + "'");

            ViewBag.data = dt.Rows;
            int ReceiveQuantity = 0;
            int Quantity = 0;
            int CL = 0;
            foreach (DataRow item in dt.Rows)
            {
                ReceiveQuantity += Commons.ConvertToInt(item["ReceiveQuantity"]);
                Quantity += Commons.ConvertToInt(item["Quantity"]);
            }
            CL = Quantity - ReceiveQuantity;

            ViewBag.ReceiveQuantity = ReceiveQuantity.ToString("N0");
            ViewBag.Quantity = Quantity.ToString("N0");
            ViewBag.CL = CL.ToString("N0");

            return View();
        }
        public ActionResult InDau8()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string OB = Commons.ConvertToString(Request.QueryString["ob"]);
            string sSQL = "select OB,sum(Quantity) Quantity,sum(isnull(ReceiveQuantity,0))  ReceiveQuantity";
            sSQL += ",PrepaingDate,STO,max(WH) WH,CustomerName,Address,District,KindOf,ContactNo,ContactDes";
            sSQL += ",sum(dbo.CongBB(DivisionID,ItemID,Quantity)) SLBB,sum(dbo.CongBB(DivisionID,ItemID,ReceiveQuantity)) SLBBN";
            sSQL += " from WD where VoucherID='" + Commons.ConvertToString(VoucherID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            if (OB != "")
            {
                sSQL += " and OB='" + Commons.Fix(OB) + "' ";
            }

            sSQL += " group by OB,PrepaingDate,STO,CustomerName";
            sSQL += " ,Address,District,KindOf,ContactNo,ContactDes ";
            sSQL += " order by OB  ";
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;



            sSQL = "select DivisionName ,Phone from Divisions where DivisionID='" + Commons.Fix(Global.GlobalVariables.DivisionID) + "' ";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                string p = "<p>Công Ty SX HTD BÌNH TIÊN</p>";
                p += "<p>ĐƠN VỊ: " + Commons.ConvertToString(dt.Rows[0][0]) + "</p>";
                p += "<p>ĐIỆN THOẠI: " + Commons.ConvertToString(dt.Rows[0][1]) + "</p>";
                ViewBag.Division = p;
            }


            //tong hop
            string OBList = "";

            sSQL = "select OB from WD where VoucherID='" + Commons.ConvertToString(VoucherID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " group by OB ";
            sSQL += " order by OB  ";
            dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                OBList += item["OB"].ToString() + ".";
            }
            OBList = OBList.Trim('.');
            ViewBag.OBList = OBList;
            return View();
        }

        public ActionResult InDau81()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string OB = Commons.ConvertToString(Request.QueryString["ob"]);
            string sSQL = "select OB,sum(Quantity) Quantity,sum(isnull(ReceiveQuantity,0))  ReceiveQuantity";
            sSQL += ",PrepaingDate,STO,max(WH) WH ,CustomerName,Address,District,KindOf,ContactNo,ContactDes";
            sSQL += " from WD where VoucherID='" + Commons.ConvertToString(VoucherID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";

            sSQL += " group by OB,PrepaingDate,STO,CustomerName,Address,District,KindOf,ContactNo,ContactDes ";
            sSQL += " order by OB  ";
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;



            sSQL = "select DivisionName ,Phone from Divisions where DivisionID='" + Commons.Fix(Global.GlobalVariables.DivisionID) + "' ";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                string p = "<p>Công Ty SX HTD BÌNH TIÊN</p>";
                p += "<p>ĐƠN VỊ: " + Commons.ConvertToString(dt.Rows[0][0]) + "</p>";
                p += "<p>ĐIỆN THOẠI: " + Commons.ConvertToString(dt.Rows[0][1]) + "</p>";
                ViewBag.Division = p;
            }


            //tong hop
            string OBList = "";

            sSQL = "select OB from WD where VoucherID='" + Commons.ConvertToString(VoucherID) + "' ";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " group by OB ";
            sSQL += " order by OB  ";
            dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                OBList += item["OB"].ToString() + ".";
            }
            OBList = OBList.Trim('.');
            ViewBag.OBList = OBList;
            return View();
        }
        [HttpPost]
        public ActionResult GetFromLocation(string VoucherID, string LocationID)
        {
            LocationID = LocationID.ToUpper();
            if (CheckLocationExists(LocationID) == false)
            {
                return Json(new { errorMsg = "Không tồn tại vị trí này", success = false });

            }
            string sSQL = sSQL = "select Location from XH where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID ='" + Commons.Fix(VoucherID) + "' ";
            DataTable db = Commons.GetData(sSQL);
            if (db.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Phiếu này đã có vị trí rồi. Bạn không được phép", success = false });
            }

            sSQL = "select ItemID,sum(Quantity) Quantity  from WD ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID ='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and ItemID not in (" + ListGift() + ")";
            sSQL += " group by ItemID ";
            DataTable dt = Commons.GetData(sSQL);
            string sMessage = "";
            //kiem tra co du hang khong
            foreach (DataRow item in dt.Rows)
            {
                int Remain = GetQuantityFromLocationNotNeedLSX(item["ItemID"].ToString(), LocationID);
                int Quantity = Commons.ConvertToInt(item["Quantity"]);

                if (Remain < Quantity)
                {
                    sMessage += "Không tồn đủ " + item["ItemID"].ToString() + " để lấy . Hiện chỉ còn " + Remain.ToString("N0") + "\n";
                }
            }
            if (sMessage != "")
            {
                return Json(new { errorMsg = sMessage, success = false });
            }

            string sWrite = "delete XH where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sWrite += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "';";

            sSQL = "select ItemID,sum(Quantity) Quantity,OB  from WD ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID ='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and ItemID not in (" + ListGift() + ")";
            sSQL += " group by ItemID,OB ";
            dt = Commons.GetData(sSQL);

            foreach (DataRow item in dt.Rows)
            {

                sWrite += "exec SP_InsertXH N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                sWrite += ",N'" + Commons.Fix(LocationID) + "'";
                sWrite += ",N'" + Commons.Fix(item["ItemID"].ToString()) + "'";
                sWrite += "," + Commons.ConvertToInt(item["Quantity"]).ToString("0");
                sWrite += ",1";
                sWrite += ",N'" + Commons.Fix(item["OB"].ToString()) + "'";
                sWrite += ";";
                sWrite += ";\n";


            }
            sWrite += "exec SP_UpdateVolumeUsed '" + Commons.Fix(LocationID) + "', ";
            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "';\n";

            Exception ex = null;
            bool t = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (t == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

            return Json(new { msg = "Thành công", success = true });

        }



        public int ReadyToConfirm(string OB, string DivisionID = "")
        {
            if (DivisionID == "")
            {
                DivisionID = GlobalVariables.DivisionID;
            }

            string sSQL = "select dbo.ReadyToConfirmOB (";
            sSQL += "'" + Commons.Fix(OB) + "' ";
            sSQL += ",N'" + Commons.Fix(DivisionID) + "')";

            DataTable dt = Commons.GetData(sSQL);//lay tong hang da xac nhan
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public bool CheckValidDataForConfirm(string VoucherID)
        {
            string sSQL = "select sum(ReceiveQuantity) t1, sum(Quantity) t2 from WD ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and VoucherID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            int ReceiveQuantity = 0, Quantity = 0;
            int ReceiveQuantity1 = 0, Quantity1 = 0;

            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ReceiveQuantity = Commons.ConvertToInt(dt.Rows[0][0]);
                Quantity = Commons.ConvertToInt(dt.Rows[0][1]);

            }
            sSQL = "select sum(ReceiveQuantity) t1, sum(Quantity) t2 from XH ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and VoucherID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ReceiveQuantity1 = Commons.ConvertToInt(dt.Rows[0][0]);
                Quantity1 = Commons.ConvertToInt(dt.Rows[0][1]);
            }

            return true;
        }
        public bool ConfirmXH(string OB, ref string sMessage, string DivisionID = "")
        {
            if (DivisionID == "")
            {
                DivisionID = GlobalVariables.DivisionID;
            }

            //string sSQL = "select top 1 ItemID,Confirmed from XH ";
            //sSQL += " where DivisionID = N'" + Commons.Fix(DivisionID) + "' ";
            //sSQL += " and OB = N'" + Commons.Fix(OB) + "' ";
            //DataTable dt = Commons.GetData(sSQL);
            //if (dt.Rows.Count == 0)
            //{
            //    sMessage = "Phiếu này chưa ấn định vị trí";
            //    return false;
            //}
            //bool Confirmed = Commons.ConvertToBool(dt.Rows[0]["Confirmed"]);
            //if (Confirmed)
            //{
            //    sMessage = "Phiếu này đã xác nhận rồi";
            //    return false;

            //}
            int r = ReadyToConfirm(OB, DivisionID);
            if (r == 0)
            {
                sMessage = "Phiếu này chưa có số lượng quét hoặc số lượng quét chưa đủ";
                return false;
            }
            if (r == 2)
            {
                sMessage = "Phiếu này chưa ấn định vị trí cho tất cả hàng";
                return false;
            }
            if (r == 3)
            {
                sMessage = "Phiếu này số lượng yêu cầu và số lượng trên vị trí không khớp";
                return false;
            }
            if (r == 4)
            {
                sMessage = "Phiếu này chưa có nhập bao thùng";
                return false;
            }
            if (r == 5)
            {
                sMessage = "Phiếu này có nhập bao thùng nhưng chưa khớp số lượng hàng hoặc túi xốp thực tế quét";
                return false;
            }

            string sWrite = "exec SP_XNOB '" + Commons.Fix(OB) + "',N'" + Commons.Fix(DivisionID) + "' ";

            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                UnConfirmXH(OB, DivisionID);
                sMessage = ex.Message;
                return false;
            }
            sMessage = "Xác nhận thành công";
            return true;
        }
        public void UnConfirmXH(string OB, string DivisionID = "")
        {
            if (DivisionID == "")
            {
                DivisionID = GlobalVariables.DivisionID;
            }

            string sWrite = "update XH set Confirmed=0,ConfirmQuantity=Quantity ";
            sWrite += " where OB='" + Commons.Fix(OB) + "' and DivisionID='" + Fix(DivisionID) + "'";
            Commons.ExecuteNoneQuery(sWrite);

        }
        public ActionResult ConfirmXK(string OB)
        {
            string sMessage = "";
            bool b = ConfirmXH(OB, ref sMessage);
            if (b == false)
            {
                return Json(new { errorMsg = sMessage, success = false });
            }
            string www = "exec SP_UpdateOB N'" + Commons.ConvertToString(OB) + "' , N'" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'";
            Commons.ExecuteNoneQuery(www);

            return Json(new { msg = sMessage, success = true });

        }
        public ActionResult LapLai()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Get_LapLai()
        {
            string sSQL = "select * from AllowList order by OutBound ";
            DataTable dt = Commons.GetData(sSQL);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OutBound = p["OutBound"],

                        };
            return Json(query);
        }


        [HttpPost]
        public ActionResult Add_LapLai(string OutBound)
        {
            OutBound = OutBound.Trim();
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("laplai") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@OutBound", };
            object[] lv = { OutBound };
            DbType[] ts = { DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_InsertAllowList", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult DeleteAllowOutBound(string OutBound)//xoa doc
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("laplai") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            Exception ex = null;
            string sWrite = "delete AllowList where OutBound = N'" + Commons.Fix(OutBound) + "'";
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }

            return Json(new { errorMsg = "Không thể xóa", success = false });

        }


        public ActionResult NhanHangForAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NhanHangForAdmin(string VoucherID)
        {
            string OutBound = "";
            if (GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không phải admin", success = false });
            }

            string ssql = "select OB from WD where ";
            ssql += " DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            ssql += " and VoucherID='" + Commons.Fix(VoucherID) + "'";
            ssql += " group by OB ";
            DataTable dt = Commons.GetData(ssql);
            foreach (DataRow item in dt.Rows)
            {
                OutBound += Commons.ConvertToString(item[0]) + "\n";

            }
            OutBound = OutBound.Trim('\n');
            OutBound = OutBound.Trim();
            if (OutBound == "")
            {
                return Json(new { errorMsg = "Bạn chưa nhập đầu 8", success = false });

            }

            OutBound = OutBound.Replace(" ", "");
            OutBound = OutBound.Trim();


            string[] ob = OutBound.Split('\n');
            string ss = "";
            for (int j = 0; j < ob.Length; j++)
            {
                if (j > 0)
                {
                    ss += ",";
                }

                ss += ob[j];

            }
            GetAndPostController gp = new GetAndPostController();
            Dau8[] ds = gp.GetOutBound(OutBound);
            if (ds.Length == 0)
            {
                return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
            }

            string sWrite = " ";

            Exception ex = null;




            foreach (Dau8 item in ds)
            {
                sWrite += "exec [SP_UWD] ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",'" + VoucherID + "'";
                sWrite += ",N'" + Commons.Fix(item.MATNR) + "'";//ma hang
                sWrite += ",N'" + Commons.Fix(item.VBELN) + "'";//dau 8

                sWrite += ",N'" + Commons.Fix(item.KBETR) + "'";//don gia

                sWrite += ",N'" + Commons.Fix(item.DISC) + "'";//chiet khau
                sWrite += ",N'" + Commons.Fix(item.ERDAT) + "'";//ngay soan hang
                sWrite += ",N'" + Commons.Fix(item.VGBEL) + "'";//sto
                sWrite += ",N'" + Commons.Fix(item.LGOBE) + "'";//kho
                sWrite += ",N'" + Commons.Fix(item.NAME) + "'";//ten khach
                sWrite += ",N'" + Commons.Fix(item.ADDRESS) + "'";//dia chi
                sWrite += ",N'" + Commons.Fix(item.REGION) + "'";//quan huyen
                sWrite += ",N'" + Commons.Fix(item.VTEXT) + "'";//loai hinh kd
                sWrite += ",N'" + Commons.Fix(item.HOPDONG) + "'";//hop dong
                sWrite += ",N'" + Commons.Fix(item.DIENGIAI) + "'";//dien giai

                sWrite += ";";

            }
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                GlobalVariables.WA = "";
                return Json(new { msg = "Cập nhật thành công", success = true });
            }

            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public ActionResult UpdateScanner()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login?f=1");
            }

            if (Global.Commons.CheckPermit("updatescanner") == false)
            {
                Response.Redirect("~/admin/notpermit?f=1");
            }

            return View();
        }
        [HttpPost]
        public ActionResult CheckOutBoundExists(string OB)
        {
            string sSQL = "select OB from OBOut ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and OB='" + Commons.Fix(OB) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Đã tồn tại outbound này", success = false });

            }
            sSQL = "select OB from OBList ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and OB='" + Commons.Fix(OB) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Đã tồn tại outbound này", success = false });

            }
            return Json(new { msg = "Thành công", success = true });

        }
        [HttpPost]
        public ActionResult CheckOutBoundForOutExists(string OB)
        {
            string sSQL = "select OB from OBOut ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and OB='" + Commons.Fix(OB) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Đã tồn tại outbound này", success = false });

            }
            sSQL = "select OB from OBList ";
            sSQL += "where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and OB='" + Commons.Fix(OB) + "'";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Đã tồn tại outbound này", success = false });

            }
            return Json(new { msg = "Thành công", success = true });

        }
        public DataTable GetTX()
        {
            string sSQL = "select ItemID from IES where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt;

        }
        public string Key
        {
            get
            {
                return Commons.APIHostKey;
            }
        }
        [HttpPost]
        public ActionResult UpdateScanner(string OutBound)
        {
            DataTable IES = GetTX();
            GetAndPostController gp = new GetAndPostController();
            Dau8[] ds = gp.GetOutBound(OutBound);
            if (ds.Length == 0)
            {
                return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
            }

            int TotalQuantity = 0;
            decimal TotalAmount = 0;
            string itemlist = "";
            foreach (Dau8 item in ds)
            {
                itemlist = itemlist + "N'" + Commons.Fix(item.MATNR) + "',";
            }
            itemlist = itemlist.Trim(',');
            DataTable dbb = Commons.GetData("select ItemID,Cm3 from ItemVolumes where ItemID in(" + itemlist + ")");
            decimal TotalCM3 = 0;
            int TangPham = 0;
            foreach (Dau8 item in ds)
            {
                dbb.DefaultView.RowFilter = "ItemID='" + item.MATNR.Replace("'", "") + "'";
                IES.DefaultView.RowFilter = "ItemID='" + item.MATNR.Replace("'", "") + "'";
                int Q = Commons.ConvertToInt(item.LFIMG.Replace(",", ""));
                TotalQuantity += Q;
                int TT = Commons.ConvertToInt(item.LFIMG.Replace(",", "")) * Commons.ConvertToInt(item.KBETR.Replace(",", ""));
                decimal ck = Commons.ConvertToDecimal(item.DISC.Replace(",", "")) * TT / 100;

                TotalAmount += Math.Round((TT - ck), 0);
                if (dbb.DefaultView.Count > 0)
                {
                    decimal CM3 = Commons.ConvertToDecimal(dbb.DefaultView[0]["CM3"]);
                    TotalCM3 += CM3 * Q;
                }
                if (IES.DefaultView.Count > 0)
                {
                    TangPham += Q;
                }
            }
            TotalCM3 = TotalCM3 / 1000000;

            OBList m = new OBList();
            string address = Commons.ConvertToString(ds[0].ADDRESS);
            //if (address.Trim() == "")
            //address = GetAddress(ds[0].KUNNR);
            //if (address == "")
            //{
            //    address = Commons.ConvertToString(ds[0].ADDRESS);
            //}
            string dau42 = "";
            dau42 = ds[0].VGBEL;
            if (dau42.IndexOf("42") == 0 && GlobalVariables.thongtintudonhang == true)
            {
                try
                {
                    string url = Commons.MAP + "/vc/GetInfoFromSTO?id=" + dau42;
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Headers.Clear();

                    httpWebRequest.Headers.Add("APIKey", Key);
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string[] result = streamReader.ReadToEnd().Split('|');
                        if (result.Length == 3)
                        {
                            m.Address = result[0];
                            m.CustomerName = result[2];
                            m.CustomerID = result[1];
                        }
                        else
                        {
                            m.Address = address;
                            m.CustomerName = ds[0].NAME;
                            m.CustomerID = ds[0].KUNNR;
                        }
                    }
                }
                catch (Exception ex)
                {
                    m.Address = address;
                    m.CustomerName = ds[0].NAME;
                    m.CustomerID = ds[0].KUNNR;

                }


            }
            else
            {
                m.Address = address;
                m.CustomerName = ds[0].NAME;
                m.CustomerID = ds[0].KUNNR;
            }

            m.Bag = 0;
            m.Box = 0;

            m.PlanDate = ds[0].WADAT;
            m.EmployeeName = "";
            m.ScannerID = GlobalVariables.FullName;
            m.OB = OutBound;
            m.TotalQuantity = TotalQuantity;
            m.TotalAmount = TotalAmount;
            m.M3 = TotalCM3;
            m.OB = OutBound;
            m.TotalTX = TangPham;
            Session["ob"] = m;

            return Json(new { msg = "Thành công", success = true });



        }
        public string GetAddress(string CustomerID)
        {
            string ssql = "select Address from Customers where dbo.FixCustomer(CustomerID) = dbo.FixCustomer('" + Fix(CustomerID) + "') ";
            DataTable dt = Commons.GetData(ssql);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                return r[0].ToString();
            }
            return "";
        }
        [HttpPost]
        public ActionResult UpdateScannerFromSAP(string OB)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("updatescanner") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (IsOBLocked(OB))
            {
                return Json(new { errorMsg = "Outbound này đã bị khoá", success = false });
            }

            DataTable IES = GetTX();
            GetAndPostController gp = new GetAndPostController();
            Dau8[] ds = gp.GetOutBound(OB);
            if (ds.Length == 0)
            {
                return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
            }

            int TotalQuantity = 0;
            decimal TotalAmount = 0;
            string itemlist = "";
            foreach (Dau8 item in ds)
            {
                itemlist = itemlist + "N'" + Commons.Fix(item.MATNR) + "',";
            }
            itemlist = itemlist.Trim(',');
            DataTable dbb = Commons.GetData("select ItemID,Cm3 from ItemVolumes where ItemID in(" + itemlist + ")");
            decimal TotalCM3 = 0;
            int TangPham = 0;
            foreach (Dau8 item in ds)
            {
                dbb.DefaultView.RowFilter = "ItemID='" + item.MATNR.Replace("'", "") + "'";
                IES.DefaultView.RowFilter = "ItemID='" + item.MATNR.Replace("'", "") + "'";
                int Q = Commons.ConvertToInt(item.LFIMG.Replace(",", ""));
                TotalQuantity += Q;
                int TT = Commons.ConvertToInt(item.LFIMG.Replace(",", "")) * Commons.ConvertToInt(item.KBETR.Replace(",", ""));
                decimal ck = Commons.ConvertToDecimal(item.DISC.Replace(",", "")) * TT / 100;

                TotalAmount += Math.Round((TT - ck), 0);
                if (dbb.DefaultView.Count > 0)
                {
                    decimal CM3 = Commons.ConvertToDecimal(dbb.DefaultView[0]["CM3"]);
                    TotalCM3 += CM3 * Q;
                }
                if (IES.DefaultView.Count > 0)
                {
                    TangPham += Q;
                }
            }
            TotalCM3 = TotalCM3 / 1000000;


            string sWrite = " Update OBList ";
            sWrite += " set TotalQuantity=" + TotalQuantity.ToString("0");
            sWrite += " , TotalAmount=" + TotalAmount.ToString("0");
            sWrite += " , M3=" + Commons.DecimalToSQL(TotalCM3);
            sWrite += " , BX=" + TangPham.ToString("0");
            sWrite += ",CustomerName=N'" + Commons.Fix(ds[0].NAME) + "'";
            sWrite += ",CustomerID=N'" + Commons.Fix(ds[0].KUNNR) + "'";
            sWrite += ",PlanDate=N'" + Commons.Fix(ds[0].WADAT) + "'";
            sWrite += ",Address=N'" + Commons.Fix(ds[0].ADDRESS) + "'";
            sWrite += " where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and OB='" + Commons.Fix(OB) + "'";
            sWrite += ";exec SP_UpdateOBDSum N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(OB) + "'";
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                return Json(new { msg = "Thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        public void LoadUpdateScanner()
        {
            DataTable tttt = new DataTable();
            ViewBag.data = tttt.Rows;
            string OB = Commons.ConvertToString(Request.QueryString["id"]);
            if (OB != "")
            {
                string sSQL = "select  OB, TotalQuantity, TotalAmount, Bag, Box,BX, ScannerID, EmployeeName";
                sSQL += ", CustomerName, Address, M3,CustomerID,PlanDate,isnull(Locked,0) Locked,Note ";
                sSQL += " from OBList where OB = N'" + Commons.Fix(OB) + "'";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    ViewBag.TotalQuantity = Commons.ConvertToInt(r["TotalQuantity"]).ToString("N0");
                    ViewBag.TotalAmount = Commons.ConvertToInt(r["TotalAmount"]).ToString("N0");
                    ViewBag.ScannerID = Commons.ConvertToString(r["ScannerID"]);
                    ViewBag.EmployeeName = Commons.ConvertToString(r["EmployeeName"]);
                    ViewBag.Bag = Commons.ConvertToInt(r["Bag"]);
                    ViewBag.Box = Commons.ConvertToInt(r["Box"]);
                    ViewBag.BX = Commons.ConvertToInt(r["BX"]);
                    ViewBag.CustomerName = Commons.ConvertToString(r["CustomerName"]);
                    ViewBag.CustomerID = Commons.ConvertToString(r["CustomerID"]);
                    ViewBag.PlanDate = Commons.ConvertToString(r["PlanDate"]);
                    ViewBag.Address = Commons.ConvertToString(r["Address"]);
                    ViewBag.M3 = Commons.ConvertToDecimal(r["M3"]).ToString("N6");
                    ViewBag.OB = OB;
                    ViewBag.Locked = (Commons.ConvertToBool(r["Locked"]) ? "1" : "0");
                    ViewBag.Note = r["Note"];

                }
                int q1 = 0, q2 = 0, q3 = 0, q4 = 0, tx1 = 0, tx2 = 0;

                sSQL = "select TT,Q1,Q2,Q3 from OBD where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += " and OutBound = N'" + Commons.Fix(OB) + "' and Style='Thung'";
                sSQL += " Order by TT ";
                dt = Commons.GetData(sSQL);
                ViewBag.data = dt.Rows;
                foreach (DataRow item in dt.Rows)
                {
                    q1 += Commons.ConvertToInt(item["Q1"]);
                    q2 += Commons.ConvertToInt(item["Q2"]) * Commons.ConvertToInt(item["Q1"]);
                    tx1 += Commons.ConvertToInt(item["Q3"]);

                }
                sSQL = "select TT,Q1,Q2,Q3 from OBD where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += " and OutBound = N'" + Commons.Fix(OB) + "' and Style='Bao'";
                sSQL += " Order by TT ";
                dt = Commons.GetData(sSQL);
                ViewBag.data1 = dt.Rows;
                foreach (DataRow item in dt.Rows)
                {
                    q3 += Commons.ConvertToInt(item["Q1"]);
                    q4 += Commons.ConvertToInt(item["Q2"]) * Commons.ConvertToInt(item["Q1"]);
                    tx2 += Commons.ConvertToInt(item["Q3"]);

                }

                ViewBag.Q1 = q1;
                ViewBag.Q2 = q2;
                ViewBag.Q3 = q3;
                ViewBag.Q4 = q4;
                ViewBag.TX1 = tx1;
                ViewBag.TX2 = tx2;


            }
            else if (Session["ob"] != null)
            {
                OBList m = (OBList)(Session["ob"]);
                ViewBag.OB = m.OB;

                ViewBag.TotalQuantity = m.TotalQuantity.ToString("N0");
                ViewBag.TotalAmount = m.TotalAmount.ToString("N0");
                ViewBag.ScannerID = m.ScannerID;
                ViewBag.BX = m.TotalTX;
                ViewBag.EmployeeName = m.EmployeeName;
                ViewBag.Bag = m.Bag;
                ViewBag.Box = m.Box;
                ViewBag.CustomerName = m.CustomerName;
                ViewBag.CustomerID = m.CustomerID;
                ViewBag.PlanDate = m.PlanDate;
                ViewBag.Address = m.Address;
                ViewBag.M3 = m.M3.ToString("N6");
                DataTable db = new DataTable();
                int v = 0;
                db.Columns.Add("TT", v.GetType());
                db.Columns.Add("Q1", v.GetType());
                db.Columns.Add("Q2", v.GetType());
                db.Columns.Add("Q3", v.GetType());
                DataRow r = db.NewRow();
                r[0] = 1;
                r[1] = 1;
                r[2] = 0;
                r[3] = 0;
                db.Rows.Add(r);

                ViewBag.data = db.Rows;
                ViewBag.data1 = db.Rows;
            }
        }

        public ActionResult UpdateScanner1()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login?f=1");
            }

            if (Global.Commons.CheckPermit("updatescanner") == false)
            {
                Response.Redirect("~/admin/notpermit?f=1");
            }

            LoadUpdateScanner();

            return View();
        }

        [HttpPost]
        public ActionResult UpdateScanner1(List<OBD> LItem, OBList M)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });

            }
            if (Commons.CheckPermit("updatescanner") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền", success = false });

            }
            if (IsOBLocked(M.OB))
            {
                return Json(new { errorMsg = "Outbound này đã bị khoá rồi", success = false });

            }
            if (LItem.Count == 0)
            {
                return Json(new { errorMsg = "Bạn chưa nhập chi tiết", success = false });
            }
            int SB = 0;
            int ST = 0;

            int TotalQuantity = 0;
            int TotalQ3 = 0;
            foreach (OBD item in LItem)
            {
                TotalQuantity += (item.Q2 * item.Q1) + item.Q3;
                TotalQ3 += item.Q3;
            }
            //if (TotalQuantity != M.TotalQuantity)
            //    return Json(new { errorMsg = "Dữ liệu về tổng số lượng không khớp với đầu 8 đã lấy " + TotalQuantity.ToString("N0") + "#" + M.TotalQuantity.ToString("N0"), success = false });

            //if (TotalQ3 != M.TotalTX)
            //    return Json(new { errorMsg = "Dữ liệu về tổng số lượng túi xốp không khớp với đầu 8 đã lấy " + TotalQ3.ToString("N0") + "#" + M.TotalTX.ToString("N0"), success = false });

            string sWrite = "exec SP_InsertScanner ";
            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(M.OB) + "'";
            sWrite += ",N'" + Commons.Fix(M.CustomerName) + "'";
            sWrite += ",N'" + Commons.Fix(M.Address) + "'";
            sWrite += "," + Commons.DecimalToSQL(M.TotalQuantity);
            sWrite += "," + Commons.DecimalToSQL(M.TotalAmount);
            sWrite += ",N'" + Commons.Fix(M.ScannerID) + "'";
            sWrite += ",N'" + Commons.Fix(M.EmployeeName) + "'";
            sWrite += "," + Commons.DecimalToSQL(M.M3);
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ",N'" + Commons.Fix(M.CustomerID) + "'";
            sWrite += ",N'" + Commons.Fix(M.PlanDate) + "'";
            sWrite += ",N'" + Commons.Fix(M.Note) + "'";
            sWrite += ";";
            sWrite += "update OBList set BX=" + M.TotalTX.ToString("0");
            sWrite += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += " and OB='" + Commons.Fix(M.OB) + "' ;";

            sWrite += "delete OBD where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += " and OutBound='" + Commons.Fix(M.OB) + "' ;";
            foreach (OBD item in LItem)
            {
                sWrite += "exec SP_InsertOBD ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(M.OB) + "'";
                sWrite += "," + item.TT.ToString("0");
                sWrite += ",N'" + Commons.Fix(item.Style) + "'";
                sWrite += "," + item.Q1.ToString("0");
                sWrite += "," + Commons.DecimalToSQL(item.Q2);
                sWrite += "," + Commons.DecimalToSQL(item.Q3);
                sWrite += ";";
                if (item.Style == "Thung")
                {
                    ST++;
                }

                if (item.Style == "Bao")
                {
                    SB++;
                }
            }
            sWrite += "exec SP_UpdateOBDSum N'" + Commons.Fix(GlobalVariables.DivisionID) + "',N'" + Commons.Fix(M.OB) + "';";
            Exception ex = null;
            GetAndPostController d = new GetAndPostController();
            bool b = d.TranToSAP(M.OB, SB, ST);

            if (b == false)
            {
                return Json(new { errorMsg = "Không kết nối SAP được ", success = false });
            }

            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {

                return Json(new { msg = "Cập nhật thành công", success = true });

            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }


        public ActionResult ViewScanners()
        {

            int F = Commons.ConvertToInt(Request.QueryString["f"]);
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login?f=1");
            }

            if (Global.Commons.CheckPermit("viewscanner") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec [SP_GetCountScanner] N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int nSum = Commons.ConvertToInt(dt.Rows[0][1]);
            int nSum1 = Commons.ConvertToInt(dt.Rows[0][2]);
            int nSP = Commons.ConvertToInt(dt.Rows[0][3]);
            int nBX = Commons.ConvertToInt(dt.Rows[0][4]);
            decimal nTotalAmount = Commons.ConvertToDecimal(dt.Rows[0][5]);


            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {

                            r[0] = "/admin/ViewScanners?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewScanners?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewScanners?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");
            ViewBag.Sum1 = nSum1.ToString("N0");
            ViewBag.Sum3 = nSP.ToString("N0");
            ViewBag.Sum4 = nBX.ToString("N0");
            ViewBag.Sum5 = nTotalAmount.ToString("N0");

            return View();
        }
        [HttpPost]
        public ActionResult LoadMessageFromScanner()
        {
            string sSQL = "exec SP_OBNote N'" + Fix(GlobalVariables.DivisionID) + "', " + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            return Json(new { content = dt.Rows[0][0], success = true });

        }

        [HttpPost]
        public ActionResult Get_OB()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetOB N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            CreateDate = Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy HH:mm"),
                            LastModifyDate = Convert.ToDateTime(p["LastModifyDate"]).ToString("dd/MM/yyyy HH:mm"),
                            OB1 = "<span " + (Commons.ConvertToBool(p["IsOk"]) == false ? "style='color:red'" : "") + ">" + Commons.ConvertToString(p["OB"]) + "</span>",
                            OB = Commons.ConvertToString(p["OB"]),
                            Bag = "<span style='color:blue;font-weight:bold'>" + Commons.ConvertToInt(p["Bag"]).ToString("N0") + "</span>",
                            Box = "<span style='color:blue;font-weight:bold'>" + Commons.ConvertToInt(p["Box"]).ToString("N0") + "</span>",
                            TotalQuantity = "<span style='color:red;font-weight:bold'>" + Commons.ConvertToInt(p["TotalQuantity"]).ToString("N0") + "</span>",
                            TotalQuantity1 = "<span style='color:red;font-weight:bold'>" + (Commons.ConvertToInt(p["TotalQuantity"]) - Commons.ConvertToInt(p["BX"])).ToString("N0") + "</span>",
                            TotalQuantity2 = "<span style='color:red;font-weight:bold'>" + Commons.ConvertToInt(p["BX"]).ToString("N0") + "</span>",
                            TotalAmount = Commons.ConvertToInt(p["TotalAmount"]).ToString("N0"),
                            EmployeeName = Commons.ConvertToString(p["EmployeeName"]),
                            CustomerName = "<p>" + Commons.ConvertToString(p["CustomerID"]) + "<p>" + Commons.ConvertToString(p["CustomerName"]) + "</p> <p>" + Commons.ConvertToString(p["Address"]) + "</p><p style='color:" + (Commons.ConvertToString(p["XN"]).IndexOf("Chưa") >= 0 ? "red" : "blue") + "'>" + Commons.ConvertToString(p["XN"]) + (Commons.ConvertToBool(p["Locked"]) ? "(Khoá)" : "") + "</p><p style='color:" + (Commons.ConvertToBool(p["CPLH"]) ? "green" : "#C5161D") + "'>" + (Commons.ConvertToBool(p["CPLH"]) ? "Có phiếu lấy hàng" : "Không có phiếu lấy hàng")
                            + "</p>",
                            Address = Commons.ConvertToString(p["Address"]),
                            Locked = (Commons.ConvertToBool(p["Locked"]) ? 1 : 0),
                            ScannerID = Commons.ConvertToString(p["ScannerID"]),
                            XN = Commons.ConvertToString(p["XN"]),
                            Note = p["Note"]
                        };
            return Json(query);
        }

        public ActionResult ES()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_ExportOB N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Export();
            d.ToExcel(Response, dt, "ob");
            return View();
        }
        public ActionResult ESOut()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_ExportOBForOut N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Export();
            d.ToExcel(Response, dt, "ob");
            return View();
        }

        [HttpPost]
        public ActionResult DeleteOB(string OB)
        {
            try
            {
                string sWrite = "exec SP_OBDelete '" + Fix(OB) + "'";
                sWrite += ",N'" + Fix(GlobalVariables.DivisionID) + "'";
                sWrite += "," + GlobalVariables.UserID.ToString("0");

                DataTable dt = Commons.GetData(sWrite);
                DataRow r = dt.Rows[0];

                if (Convert.ToInt32(r[0]) == 1)
                {
                    return Json(new { msg = r[1].ToString(), success = true });
                }

                else
                {
                    return Json(new { errorMsg = r[1].ToString(), success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

        }

        public ActionResult SetUp()
        {
            string sSQL = "select top 1 RC,TTT,ForOne,OpenForOut,XuatThang,AllowNotFull,GoogleAPIKey,thongtintudonhang from Settings ";
            DataTable dt = Commons.GetData(sSQL);
            DataRow r = dt.Rows[0];
            ViewBag.rc = r["RC"];
            ViewBag.ForOne = (Commons.ConvertToBool(r["ForOne"]) ? "1" : "0");
            ViewBag.OpenForOut = (Commons.ConvertToBool(r["OpenForOut"]) ? "1" : "0");
            ViewBag.XuatThang = (Commons.ConvertToBool(r["XuatThang"]) ? "1" : "0");
            ViewBag.thongtintudonhang = (Commons.ConvertToBool(r["thongtintudonhang"]) ? "1" : "0");
            ViewBag.GoogleAPIKey = Commons.ConvertToString(r["GoogleAPIKey"]);
            ViewBag.AllowNotFull = (Commons.ConvertToBool(r["AllowNotFull"]) ? "1" : "0");
            ViewBag.TTT = Commons.ConvertToDecimal(r["TTT"]).ToString("0.000000");
            sSQL = "select top 1 T1,T2 from Processes ";
            dt = Commons.GetData(sSQL);
            r = dt.Rows[0];
            if (Commons.ConvertToInt(r[0]) == Commons.ConvertToInt(r[1]))
            {
                ViewBag.ready = 1;
            }
            else
            {
                ViewBag.ready = 0;
            }

            sSQL = "select top 1 InventoryLockedDate from KKLocks ";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                r = dt.Rows[0];
                ViewBag.LockedDate = r[0];
            }

            return View();
        }
        [HttpPost]
        public ActionResult DownloadMC()
        {
            bool result = false;
            try
            {
                string ssql = "select  T1,T2 from Processes ";
                DataTable dt = Commons.GetData(ssql);


                string s = Commons.ConvertToInt(dt.Rows[0][0]).ToString("N0");
                string c = Commons.ConvertToInt(dt.Rows[0][1]).ToString("N0");
                if (s != c)
                {
                    return Json(new { errorMsg = "Đang có tiến trình khác đang chạy", success = false });

                }
                Commons.ExecuteNoneQuery("update Processes set T1=0,T2=1");

                ssql = "select  MaNhom, DienGiaiMaNhom from Article group by MaNhom, DienGiaiMaNhom ";
                dt = Commons.GetDataFromOtherDataBase(ssql);
                string sWrite = "";


                foreach (DataRow item in dt.Rows)
                {
                    sWrite += "exec [SP_InsertMC] N'" + Commons.Fix(item["MaNhom"].ToString()) + "',N'" + Commons.Fix(item["DienGiaiMaNhom"].ToString()) + "';";
                }

                Exception ex = null;
                //if (colam)
                //{
                result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (result == false)
                {
                    return Json(new { errorMsg = ex.Message, success = false });

                }
                //}


                ssql = "select  MaBC,MaHH, MaNhom,TenHH,DVT from Article ";
                dt = Commons.GetDataFromOtherDataBase(ssql);



                sWrite = "";
                int i = 0;
                foreach (DataRow item in dt.Rows)
                {

                    sWrite += "exec [SP_InsertMCItem] N'" + Commons.Fix(item["MaHH"].ToString()) + "',N'" + Commons.Fix(item["MaNhom"].ToString()) + "',N'" + Commons.Fix(item["MaBC"].ToString()) + "';";

                    sWrite += "exec [SP_InsertItemVolume] N'" + Commons.Fix(item["MaHH"].ToString()) + "',N'" + Commons.Fix(item["TenHH"].ToString()) + "',0,0,0,0,0,N'" + Commons.Fix(item["MaBC"].ToString()) + "',N'" + Commons.Fix(item["DVT"].ToString()) + "',N'" + Commons.Fix(item["MaNhom"].ToString()) + "';";

                    i = i + 1;
                    if (i % 300 == 0 && i > 0)
                    {
                        Commons.ExecuteNoneQuery("update Processes set T1=" + i.ToString("0") + ",T2=" + dt.Rows.Count.ToString("0"));

                        ex = null;

                        result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        if (result == false)
                        {
                            return Json(new { errorMsg = ex.Message, success = false });

                        }
                        sWrite = "";


                    }
                }

                if (sWrite != "")
                {
                    Commons.ExecuteNoneQuery("update Processes set T1=" + dt.Rows.Count.ToString("0") + ",T2=" + dt.Rows.Count.ToString("0"));

                    ex = null;
                    result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    if (result == false)
                    {
                        return Json(new { errorMsg = ex.Message, success = false });

                    }
                }

            }
            catch (Exception ee)
            {

                return Json(new { errorMsg = ee.Message, success = false });

            }


            return Json(new { msg = "Cập nhật thành công", success = true });

        }

        [HttpPost]
        public ActionResult DownloadKH()
        {
            bool result = false;
            try
            {
                string ssql = "select  T1,T2 from Processes ";
                DataTable dt = Commons.GetData(ssql);


                string s = Commons.ConvertToInt(dt.Rows[0][0]).ToString("N0");
                string c = Commons.ConvertToInt(dt.Rows[0][1]).ToString("N0");
                if (s != c)
                {
                    return Json(new { errorMsg = "Đang có tiến trình khác đang chạy", success = false });

                }
                Commons.ExecuteNoneQuery("update Processes set T1=0,T2=1");

                ssql = "select  CustomerID,CustomerName,Address,CustomerLine,CustomerLineName";
                ssql += " from Customers where isnull(IsMarketing,0)=0 ";
                dt = Commons.GetDataFromOtherDataBase(ssql);
                string sWrite = "";
                sWrite = "";
                int i = 0;
                foreach (DataRow item in dt.Rows)
                {

                    sWrite += "exec SP_InsertCustomer ";
                    sWrite += " N'" + Commons.Fix(Commons.ConvertToString(item["CustomerID"])) + "'";
                    sWrite += ",N'" + Commons.Fix(Commons.ConvertToString(item["CustomerName"])) + "'";
                    sWrite += ",N'" + Commons.Fix(Commons.ConvertToString(item["Address"])) + "'";
                    sWrite += ",N'" + Commons.Fix(Commons.ConvertToString(item["CustomerLine"])) + "'";
                    sWrite += ",N'" + Commons.Fix(Commons.ConvertToString(item["CustomerLineName"])) + "'";
                    sWrite += ";";
                    i = i + 1;
                    if (i % 300 == 0 && i > 0)
                    {
                        Commons.ExecuteNoneQuery("update Processes set T1=" + i.ToString("0") + ",T2=" + dt.Rows.Count.ToString("0"));

                        Exception ex = null;

                        result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        if (result == false)
                        {
                            return Json(new { errorMsg = ex.Message, success = false });

                        }
                        sWrite = "";


                    }
                }

                if (sWrite != "")
                {
                    Commons.ExecuteNoneQuery("update Processes set T1=" + dt.Rows.Count.ToString("0") + ",T2=" + dt.Rows.Count.ToString("0"));

                    Exception ex = null;
                    result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    if (result == false)
                    {
                        return Json(new { errorMsg = ex.Message, success = false });

                    }
                }

            }
            catch (Exception ee)
            {

                return Json(new { errorMsg = ee.Message, success = false });

            }


            return Json(new { msg = "Cập nhật thành công", success = true });

        }
        public bool ImportOB(string list)
        {

            GetAndPostController d = new GetAndPostController();
            Dau8[] v = d.GetOutBoundNotSave(list);
            List<CH> ar = new List<CH>();
            foreach (Dau8 item in v)
            {
                string ItemID = item.MATNR;
                bool co = false;
                foreach (CH j in ar)
                {
                    if (j.ItemID == ItemID)
                    {
                        co = true;
                        j.Quantity += Commons.ConvertToInt(item.LFIMG);
                        break;
                    }
                }
                if (co == false)
                {
                    CH i = new CH();
                    i.CM3 = 0;
                    i.ItemID = ItemID;
                    i.Quantity1 = 0;
                    i.Quantity = Commons.ConvertToInt(item.LFIMG);
                    ar.Add(i);
                }
            }
            ImportReport(ar);
            GlobalVariables.ChamHang = ar;

            return true;
        }
        public void ImportReport(List<CH> list)
        {
            DataTable dt = Commons.GetData("select AdminID, ItemID, Quantity, Quantity1 from Reports where 1=2 ");
            foreach (CH item in list)
            {
                DataRow r = dt.NewRow();
                r["AdminID"] = GlobalVariables.UserID;
                r["ItemID"] = item.ItemID;
                r["Quantity"] = item.Quantity;
                r["Quantity1"] = 0;
                dt.Rows.Add(r);
            }
            Commons.ExecuteNoneQuery("delete Reports where AdminID=" + GlobalVariables.UserID.ToString("0"));

            using (SqlConnection destinationConnection = new SqlConnection(Commons.ConnectionString))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
            {
                destinationConnection.Open();
                bulkCopy.DestinationTableName = "Reports";
                bulkCopy.WriteToServer(dt);
                destinationConnection.Close();

            }

        }
        [HttpPost]
        public ActionResult add_List(string sList)
        {

            List<CH> ar = new List<CH>();
            sList = sList.Trim();
            string[] lines = sList.Split('\n');
            foreach (string item in lines)
            {
                string[] ll = item.Split('\t');
                if (ll.Length == 1)
                {
                    if (ImportOB(sList))
                    {

                        return Json(new { msg = "Thành công", success = true });

                    }
                    else
                    {
                        return Json(new { errorMsg = "Dữ liệu không hợp lệ", success = false });
                    }
                }

                if (ll.Length > 1)
                {
                    CH r = new CH();
                    r.ItemID = ll[0].Trim();
                    r.Quantity = Commons.ConvertToInt(ll[1]);

                    foreach (CH mm in ar)
                    {
                        if (mm.ItemID == r.ItemID)
                        {
                            return Json(new { errorMsg = "Mã " + r.ItemID + " có nhiều dòng lặp lại", success = false });

                        }
                    }
                    ar.Add(r);
                }
                else
                {
                    return Json(new { errorMsg = "Dữ liệu không hợp lệ", success = false });

                }

            }
            ImportReport(ar);
            GlobalVariables.ChamHang = ar;

            return Json(new { msg = "Thành công", success = true });

        }
        [HttpPost]
        public ActionResult GetCountCC()
        {
            string ssql = "select  T1,T2 from Processes ";
            DataTable dt = Commons.GetData(ssql);


            string s = Commons.ConvertToInt(dt.Rows[0][0]).ToString("N0");
            string c = Commons.ConvertToInt(dt.Rows[0][1]).ToString("N0");
            int ready = 0;
            if (s == c)
            {
                ready = 1;
            }
            else
            {
                ready = 0;
            }

            if (ready == 1)
            {
                return Json(new { msg = "Đã lấy: " + s + " / " + c, ss = ready, success = true });
            }
            else
            {
                return Json(new { msg = "Đang lấy: " + s + " / " + c, ss = ready, success = true });
            }
        }
        [HttpPost]
        public ActionResult LockVoucher(string VoucherID)
        {
            if (VoucherID == "")
            {
                return Json(new { errorMsg = "Phiếu không hợp lệ", success = false });
            }

            string sSQL = "select top 1 ItemID from WD where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and isnull(ReceiveQuantity,0)<>Quantity and ItemID not in(" + ListGift() + ") ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Phiếu này chưa quét đủ hàng", success = false });

            }
            sSQL = "select * from XH where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu này chưa có vị trí", success = false });

            }
            sSQL = "select * from XH where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and isnull(Confirmed,0)=0";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Phiếu này chưa xác nhận đủ mã. Vui lòng xác nhận lại", success = false });

            }
            string sWrite = "update W set Locked=1 where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and VoucherID = N'" + Commons.Fix(VoucherID) + "'";
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

            return Json(new { msg = "Khóa thành công", success = true });

        }
        private bool IsHasLocation(string VoucherID)
        {
            string sSQL = "select top 1 ItemID from XH where DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult DiHangThieu(string VoucherID)
        {
            string sSQL = "";
            if (IsLocked(VoucherID))
            {
                return Json(new { errorMsg = "Phiếu đã bị khóa không thể xử lý", success = false });
            }

            if (IsMoved(VoucherID))
            {
                return Json(new { errorMsg = "Phiếu đã di rồi", success = false });
            }

            if (IsHasLocation(VoucherID) == false)
            {
                return Json(new { errorMsg = "Phiếu này chưa ấn định vị trí", success = false });
            }

            sSQL = "select ItemID ";
            sSQL += " from XH where VoucherID = N'" + Commons.Fix(VoucherID) + "'";
            sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += "  and Confirmed=1  ";

            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu này chưa thực hiện xác nhận lấy hàng", success = false });
            }

            sSQL = "exec SP_GetOutBoundDetailNotScan N'" + VoucherID + "'";
            sSQL += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu lấy hàng này đã chia đủ hàng. Không có hàng để di", success = false });
            }

            string sWrite = "";
            foreach (DataRow item in dt.Rows)
            {
                string ItemID = item["ItemID"].ToString();
                int Quantity = Commons.ConvertToInt(item["Quantity"]);
                int ReceiveQuantity = Commons.ConvertToInt(item["ReceiveQuantity"]);
                if (Quantity > ReceiveQuantity)
                {
                    sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += ",'CHOXULY'";
                    sWrite += ",1";
                    sWrite += ",'" + Commons.Fix(ItemID) + "'";
                    sWrite += ",''";
                    sWrite += "," + (Quantity - ReceiveQuantity).ToString("0");
                    sWrite += ";";
                    sWrite += "exec SP_InsertWDResult N'" + Commons.Fix(VoucherID) + "' ";
                    sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                    sWrite += ",'" + Commons.Fix(ItemID) + "'";
                    sWrite += "," + (Quantity - ReceiveQuantity).ToString("0");
                    sWrite += "," + GlobalVariables.UserID.ToString("0");
                    sWrite += ";";
                }

            }
            sSQL = "select ItemID,Quantity,isnull(ConfirmQuantity,0)  ReceiveQuantity,Location ";
            sSQL += " from XH where VoucherID = N'" + Commons.Fix(VoucherID) + "'";
            sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += "  and Quantity<>isnull(ConfirmQuantity,0)  ";

            dt = Commons.GetData(sSQL);

            foreach (DataRow item in dt.Rows)
            {
                string ItemID = item["ItemID"].ToString();
                int Quantity = Commons.ConvertToInt(item["Quantity"]);
                int ReceiveQuantity = Commons.ConvertToInt(item["ReceiveQuantity"]);
                if (Quantity > ReceiveQuantity)
                {
                    sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += ",'" + Commons.Fix(item["Location"].ToString()) + "'";
                    sWrite += ",-1";
                    sWrite += ",'" + Commons.Fix(ItemID) + "'";
                    sWrite += ",''";
                    sWrite += "," + (Quantity - ReceiveQuantity).ToString("0");
                    sWrite += ";";
                }

            }

            sWrite += "update W set Locked=1 where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and VoucherID = N'" + Commons.Fix(VoucherID) + "';";
            sWrite = "delete WDResults Where VoucherID=N'" + Commons.Fix(VoucherID) + "' and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "';" + sWrite;
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

            return Json(new { msg = "Di hàng thành công", success = true });

        }
        public bool IsLocked(string VoucherID)
        {
            string sSQL = "select top 1 Locked from W where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID = N'" + Commons.Fix(VoucherID) + "' and Locked = 1  ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool IsXN(string VoucherID)
        {
            string sSQL = "select ItemID from XH where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID = N'" + Commons.Fix(VoucherID) + "' and Confirmed = 1  ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool IsMoved(string VoucherID)
        {
            string sSQL = "select top 1 ItemID from WDResults where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID = N'" + Commons.Fix(VoucherID) + "'  ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }




        public ActionResult ViewIES()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();

            return View();
        }

        [HttpPost]
        public ActionResult Get_IES()
        {
            string sSQL = "select E.ItemID,I.ItemName from IES E inner join ItemVolumes I on E.ItemID=I.ItemID ";
            sSQL += " where E.DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' order by 1 ";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ItemID = p["ItemID"].ToString(),
                            ItemName = p["ItemName"].ToString()

                        };
            return Json(query);
        }





        [HttpPost]
        public ActionResult Add_IE(string ItemID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@DivisionID", "@ItemID" };
            object[] lv = { GlobalVariables.DivisionID, ItemID };
            DbType[] ts = { DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_InsertIE", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        //xoa doc
        [HttpPost]
        public ActionResult DeleteIE(string ItemID)//xoa doc
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            Exception ex = null;
            string sWrite = "Delete IES where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and ItemID = N'" + Commons.Fix(ItemID) + "' ";
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không thể xóa " + ex.Message, success = false });
            }
        }

        //phan di hang moi
        public ActionResult PhieuDiHang()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }
            else
            if (Global.Commons.CheckPermit("dihang") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }
            string sSQL = "select Description,CreateDate from MoveItems where ";
            sSQL += " VoucherID='" + Commons.Fix(Commons.ConvertToString(Request.QueryString["id"])) + "'";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.dd = Commons.ConvertToString(dt.Rows[0]["Description"]);
                ViewBag.n = Commons.ConvertToDateTime(dt.Rows[0]["CreateDate"]).ToString("dd/MM/yyyy HH:mm");


            }

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            string LocationFrom = Commons.ConvertToString(Request.QueryString["from"]);

            string id = Commons.ConvertToString(Request.QueryString["id"]);
            string LocationTo = Commons.ConvertToString(Request.QueryString["to"]);

            int cpage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            sSQL = "exec Get_RemainDetailCount N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",'" + Commons.Fix(LocationFrom) + "'";
            sSQL += ",'" + Commons.Fix(keyword) + "'";
            sSQL += "," + cpage;
            // System.IO.File.WriteAllText("d:\\aaa.sql", sSQL);
            dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int nSL = Convert.ToInt32(dt.Rows[0][1]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/PhieuDiHang?key=" + keyword + "&page=" + e + "&from=" + LocationFrom + "&to=" + LocationTo + "&id=" + id;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/PhieuDiHang?key=" + keyword + "&page=1&from=" + LocationFrom + "&to=" + LocationTo + "&id=" + id;

                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/PhieuDiHang?key=" + keyword + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0") + "&from=" + LocationFrom + "&to=" + LocationTo + "&id=" + id;
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.SL = nSL.ToString("N0");

            return View();
        }


        //phan di hang moi
        public ActionResult ChiXemPhieuDiHang()
        {
            string sSQL = "select Description,CreateDate,CreateBy from MoveItems where ";
            sSQL += " VoucherID='" + Commons.Fix(Commons.ConvertToString(Request.QueryString["id"])) + "'";
            sSQL += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.allowedit = 0;

            if (dt.Rows.Count > 0)
            {
                ViewBag.dd = Commons.ConvertToString(dt.Rows[0]["Description"]);
                ViewBag.n = Commons.ConvertToDateTime(dt.Rows[0]["CreateDate"]).ToString("dd/MM/yyyy HH:mm");
                if (Commons.ConvertToInt(dt.Rows[0]["CreateBy"]) == GlobalVariables.UserID || Commons.CheckPermit("ql"))
                {
                    ViewBag.allowedit = 1;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChiXemPhieuDiHang(string VoucherID, string Description)
        {
            string sWrite = "update MoveItems set Description =N'" + Fix(Description) + "' ";
            sWrite += " where VoucherID=N'" + Fix(VoucherID) + "' and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' ";

            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

        }
        public ActionResult ChuyenHang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult get_moveitems()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);

            string sSQL = "select D.ItemID,I.ItemName,I.UnitID,D.Quantity,D.LSX";
            sSQL += " from MoveItemDetail D inner join MoveItems M on D.VoucherID = M.VoucherID ";
            sSQL += " and D.DivisionID=M.DivisionID ";
            sSQL += " inner join ItemVolumes I on I.ItemID=D.ItemID ";
            sSQL += " where M.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and M.VoucherID='" + Commons.Fix(VoucherID) + "'";
            sSQL += " order by D.ItemID,D.LSX ";
            DataTable dt = Commons.GetData(sSQL);
            int Total = 0;
            foreach (DataRow item in dt.Rows)
            {
                Total += Convert.ToInt32(item["Quantity"]);
            }

            DataRow r = dt.NewRow();
            r["ItemID"] = "";
            r["ItemName"] = "<strong>Tổng cộng</strong>";
            r["UnitID"] = "";
            r["Quantity"] = Total;
            r["LSX"] = "";
            dt.Rows.Add(r);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = (p["ItemName"].ToString() == "<strong>Tổng cộng</strong>" ? "<strong>" + Commons.ConvertToInt(p["Quantity"]).ToString("N0") + "</strong>" : Commons.ConvertToInt(p["Quantity"]).ToString("N0")),
                            UnitID = p["UnitID"],
                            LSX = p["LSX"]
                        };
            return Json(query);

        }
        [HttpPost]
        public ActionResult Add_ListDetail(List<Item> LItem, string VoucherID, string LocationFrom, string LocationTo, string Description)
        {
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Tài khoản không hợp lệ. Vui lòng đăng nhập lại", success = false });
                }
                string llll = "";
                if (LocationFrom == LocationTo)
                {
                    return Json(new { errorMsg = "Vị trí nguồn và đích phải khác nhau ", success = false });
                }
                if (Description == null)
                {
                    Description = "";
                }

                if (LItem.Count > 0)
                {
                    for (int i = LItem.Count - 1; i >= 0; i--)
                    {
                        if (LItem[i].ItemID == "" || LItem[i].ItemID == null)
                        {
                            LItem.RemoveAt(i);
                            break;
                        }

                    }
                }

                LocationFrom = LocationFrom.ToUpper();
                LocationTo = LocationTo.ToUpper();
                if (GlobalVariables.GHXuatThang)
                {
                    if (LocationFrom == "XUATTHANG" || LocationTo == "XUATTHANG")
                    {
                        if (Commons.CheckPermit("xuatthang") == false)
                        {
                            return Json(new { errorMsg = "Bạn chưa được phân quyền sử dụng vị trí XUATTHANG", success = false });
                        }
                    }
                }

                try
                {
                    if (CheckLocationSource(LocationFrom) == false)
                    {
                        return Json(new { errorMsg = "Không tồn tại vị trí nguồn này hoặc vị trí này đã bị khóa", success = false });

                    }



                    if (CheckLocationDest(LocationTo) == false)
                    {
                        return Json(new { errorMsg = "Không tồn tại vị trí đích này hoặc vị trí này đã bị khóa", success = false });

                    }

                }
                catch (Exception loi)
                {

                    return Json(new { errorMsg = loi.Message, success = false });

                }
                string sWrite = "";

                foreach (Item i in LItem)
                {
                    llll += i.ItemID + ",";
                    string ssql = "exec [vanchuyenconhatky] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    ssql += ",N'" + Commons.Fix(VoucherID) + "'";
                    ssql += ",N'" + Commons.Fix(Description) + "'";
                    ssql += ",'" + Commons.Fix(LocationFrom) + "'";
                    ssql += ",'" + Commons.Fix(LocationTo) + "'";
                    ssql += ",'" + Commons.Fix(i.ItemID) + "'";
                    ssql += "," + i.Quantity.ToString("0");
                    ssql += "," + GlobalVariables.UserID.ToString();
                    ssql += ",0;";

                    DataTable dt = Commons.GetData(ssql);

                    if (Commons.ConvertToInt(dt.Rows[0][0]) == 0)
                    {
                        return Json(new { errorMsg = dt.Rows[0][1], success = false });
                    }


                }
                sWrite += "exec SP_UpdateVolumeUsed ";
                sWrite += " N'" + Commons.Fix(LocationFrom) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";
                sWrite += "exec SP_UpdateVolumeUsed ";
                sWrite += " N'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

                Exception ex = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (b == true)
                {
                    return Json(new { msg = "Cập nhật thành công", ItemList = llll.Trim(','), success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });

                }

            }
            catch (Exception eee)
            {
                return Json(new { errorMsg = eee.Message, success = false });

            }



        }
        [HttpPost]
        public ActionResult Remove_ListDetail(List<Item> LItem, string VoucherID, string LocationFrom, string LocationTo, string Description)
        {
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Tài khoản không hợp lệ. Vui lòng đăng nhập lại", success = false });
                }
                string llll = "";
                if (LocationFrom == LocationTo)
                {
                    return Json(new { errorMsg = "Vị trí nguồn và đích phải khác nhau ", success = false });
                }
                if (Description == null)
                {
                    Description = "";
                }

                if (LItem.Count > 0)
                {
                    for (int i = LItem.Count - 1; i >= 0; i--)
                    {
                        if (LItem[i].ItemID == "" || LItem[i].ItemID == null)
                        {
                            LItem.RemoveAt(i);
                            break;
                        }

                    }
                }

                LocationFrom = LocationFrom.ToUpper();
                LocationTo = LocationTo.ToUpper();
                if (GlobalVariables.GHXuatThang)
                {
                    if (LocationFrom == "XUATTHANG" || LocationTo == "XUATTHANG")
                    {
                        if (Commons.CheckPermit("xuatthang") == false)
                        {
                            return Json(new { errorMsg = "Bạn chưa được phân quyền sử dụng vị trí XUATTHANG", success = false });
                        }
                    }
                }

                try
                {
                    if (CheckLocationExists(LocationFrom) == false)
                    {
                        return Json(new { errorMsg = "Không tồn tại vị trí nguồn này hoặc vị trí này đã bị khóa", success = false });

                    }



                    if (CheckLocationExists(LocationTo) == false)
                    {
                        return Json(new { errorMsg = "Không tồn tại vị trí đích này hoặc vị trí này đã bị khóa", success = false });

                    }

                }
                catch (Exception loi)
                {

                    return Json(new { errorMsg = loi.Message, success = false });

                }
                string sWrite = "";


                foreach (Item i in LItem)
                {
                    string ssql = "exec [vanchuyenconhatky] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    ssql += ",N'" + Commons.Fix(VoucherID) + "'";
                    ssql += ",N'" + Commons.Fix(Description) + "'";
                    ssql += ",'" + Commons.Fix(LocationFrom) + "'";
                    ssql += ",'" + Commons.Fix(LocationTo) + "'";
                    ssql += ",'" + Commons.Fix(i.ItemID) + "'";
                    ssql += "," + i.Quantity.ToString("0");
                    ssql += "," + GlobalVariables.UserID.ToString();
                    ssql += ",1;";//remove

                    DataTable dt = Commons.GetData(ssql);

                    if (Commons.ConvertToInt(dt.Rows[0][0]) == 0)
                    {
                        return Json(new { errorMsg = dt.Rows[0][1], success = false });
                    }


                    llll += i.ItemID + ",";

                }
                sWrite += "exec SP_UpdateVolumeUsed ";
                sWrite += " N'" + Commons.Fix(LocationFrom) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";
                sWrite += "exec SP_UpdateVolumeUsed ";
                sWrite += " N'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

                Exception ex = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (b == true)
                {
                    return Json(new { msg = "Cập nhật thành công", ItemList = llll.Trim(','), success = true });
                }

                return Json(new { errorMsg = ex.Message, success = false });
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = ee.Message, success = false });

            }


        }
        [HttpPost]
        public ActionResult QuetDiHang(string BarCode, string VoucherID, string LocationFrom, string LocationTo, int Quantity, string Description)
        {
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Tài khoản không hợp lệ. Vui lòng đăng nhập lại", success = false });
                }

                if (VoucherID == "")
                {
                    VoucherID = GetNewKey();
                }

                string llll = "";
                BarCode = BarCode.ToUpper();
                if (LocationFrom == LocationTo)
                {
                    return Json(new { errorMsg = "Vị trí nguồn và đích phải khác nhau ", success = false });
                }
                if (GlobalVariables.GHXuatThang)
                {
                    if (LocationFrom == "XUATTHANG" || LocationTo == "XUATTHANG")
                    {
                        if (Commons.CheckPermit("xuatthang") == false)
                        {
                            return Json(new { errorMsg = "Bạn chưa được phân quyền sử dụng vị trí XUATTHANG", success = false });
                        }
                    }
                }

                if (CheckLocationDest(LocationTo) == false)
                {
                    return Json(new { errorMsg = "Không tồn tại vị trí đích này hoặc vị trí này đã bị khóa", success = false });

                }
                if (Description == null)
                {
                    Description = "";
                }

                List<CC> b = new List<CC>();
                string sWrite = "";
                if (BarCode == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập barcode hoặc mã hàng", success = false });
                }

                if (BarCode.Length == 27 && (BarCode.Substring(16, 1) == "H" || BarCode.Substring(14, 1) == "H"))
                {
                    b = GetFromHappyBitis(BarCode);
                }
                else
                if (BarCode.Length == 18 && BarCode.Substring(BarCode.Length - 1, 1) != "0")
                {    //neu tem dây 18            
                    b = GetFrom1811(BarCode);
                    foreach (CC item in b)
                    {
                        CC d = item;
                        if (item.lsx == "" || item.lsx == null)
                        {

                        }
                    }
                }
                else
                {

                    CC a = GetItemFromBarCode(BarCode);
                    if (BarCode.Length == 14 || BarCode.Length == 18)
                    {
                        a.q = Quantity;
                    }

                    if (a.lsx == "" || a.lsx == null)
                    {

                    }
                    if (a.i != null && a.i != "")
                    {
                        b.Add(a);
                    }


                }

                if (LocationFrom != "TUNGOAI")
                {
                    if (CheckLocationSource(LocationFrom) == false)
                    {
                        return Json(new { errorMsg = "Không tồn tại vị trí nguồn này hoặc vị trí này đã bị khóa", success = false });
                    }
                }


                if (b.Count == 0)
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }

                foreach (CC i in b)
                {
                    llll += i.i + ",";


                    string ssql = "exec [vanchuyenconhatky] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    ssql += ",N'" + Commons.Fix(VoucherID) + "'";
                    ssql += ",N'" + Commons.Fix(Description) + "'";
                    ssql += ",'" + Commons.Fix(LocationFrom) + "'";
                    ssql += ",'" + Commons.Fix(LocationTo) + "'";
                    ssql += ",'" + Commons.Fix(i.i) + "'";
                    ssql += "," + i.q.ToString("0");
                    ssql += "," + GlobalVariables.UserID.ToString();
                    ssql += ",0;";

                    DataTable dt = Commons.GetData(ssql);

                    if (Commons.ConvertToInt(dt.Rows[0][0]) == 0)
                    {
                        return Json(new { errorMsg = dt.Rows[0][1], success = false });
                    }



                }
                if (LocationFrom == "TUNGOAI")
                {

                }
                else
                {
                    sWrite += "exec SP_UpdateVolumeUsed ";
                    sWrite += " N'" + Commons.Fix(LocationFrom) + "'";
                    sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

                }
                sWrite += "exec SP_UpdateVolumeUsed ";
                sWrite += " N'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

                Exception ex = null;
                bool v = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (v == true)
                {
                    return Json(new { msg = "Cập nhật thành công", VoucherID = VoucherID, ItemList = llll.Trim(','), success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = ee.Message, success = false });

            }

        }

        public ActionResult XemCacPhieuDiHang()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("xemdihang") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string fl = Commons.ConvertToString(Request.QueryString["fl"]);
            string tl = Commons.ConvertToString(Request.QueryString["tl"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            try
            {
                string[] l = from.Split('/');
                fromdate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = to.Split('/');
                todate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }

            string sSQL = "exec SP_GetMoveItemCount N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ", N'" + Commons.Fix(fl) + "'";
            sSQL += ", N'" + Commons.Fix(tl) + "'";
            sSQL += ",'" + fromdate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + todate.ToString("yyyy.MM.dd") + "'";

            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            decimal nSum = Convert.ToDecimal(dt.Rows[0][1]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {
                    string l = "";
                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            l = "/admin/XemCacPhieuDiHang?from=" + fromdate.ToString("dd/MM/yyyy");
                            l += "&to=" + todate.ToString("dd/MM/yyyy");
                            l += "&fl=" + fl;
                            l += "&tl=" + tl;
                            l += "&page=" + e;

                            r[0] = l;
                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    l = "/admin/XemCacPhieuDiHang?from=" + fromdate.ToString("dd/MM/yyyy");
                    l += "&to=" + todate.ToString("dd/MM/yyyy");
                    l += "&fl=" + fl;
                    l += "&tl=" + tl;
                    l += "&page=" + 1;

                    rd[0] = l;
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    l = "/admin/XemCacPhieuDiHang?from=" + fromdate.ToString("dd/MM/yyyy");
                    l += "&to=" + todate.ToString("dd/MM/yyyy");
                    l += "&fl=" + fl;
                    l += "&tl=" + tl;
                    l += "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");

                    rc[0] = l;
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");




            return View();
        }
        [HttpPost]
        public ActionResult LayPhieuDiHang()
        {
            string fl = Commons.ConvertToString(Request.QueryString["fl"]);
            string tl = Commons.ConvertToString(Request.QueryString["tl"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            try
            {
                string[] l = from.Split('/');
                fromdate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = to.Split('/');
                todate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }
            string sSQL = "exec SP_GetMoveItem ";
            sSQL += "N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ", N'" + Commons.Fix(fl) + "'";
            sSQL += ", N'" + Commons.Fix(tl) + "'";
            sSQL += "," + CurrentPage.ToString("0");
            sSQL += "," + PAGE_SIZE.ToString("0");
            sSQL += ",'" + fromdate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + todate.ToString("yyyy.MM.dd") + "'";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            VoucherID = p["VoucherID"],
                            CreateDate = Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy HH:mm"),
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            FullName = p["FullName"],
                            Description = p["Description"],
                            FromLocation = p["FromLocation"],
                            ToLocation = p["ToLocation"],
                            TranItemInfo = TranItemInfo(p["VoucherID"].ToString(), Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy HH:mm"), Commons.ConvertToInt(p["Quantity"]).ToString("N0"), p["FullName"].ToString(), p["Description"].ToString(), p["FromLocation"].ToString(), p["ToLocation"].ToString())

                        };
            return Json(query);
        }
        public string TranItemInfo(string VoucherID, string CreateDate, string Quantity, string FullName
            , string Description, string FromLocation, string ToLocation)
        {
            string sResult = "";
            sResult += "<p> Số phiếu: " + VoucherID + "</p>";
            sResult += "<p> Ngày phiếu: " + CreateDate + "</p>";
            sResult += "<p> Từ vị trí: " + FromLocation + "</p>";
            sResult += "<p> Đến vị trí: " + ToLocation + "</p>";
            sResult += "<p> Số lượng: " + Quantity + "</p>";
            sResult += "<p> Diễn giải: " + Description + "</p>";
            sResult += "<p> Người tạo: " + FullName + "</p>";

            return sResult;
        }
        public ActionResult ExportMoveItem()
        {
            string fl = Commons.ConvertToString(Request.QueryString["fl"]);
            string tl = Commons.ConvertToString(Request.QueryString["tl"]);
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            try
            {
                string[] l = from.Split('/');
                fromdate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = to.Split('/');
                todate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }
            string sSQL = "exec SP_ExportMoveItem ";
            sSQL += "N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ", N'" + Commons.Fix(fl) + "'";
            sSQL += ", N'" + Commons.Fix(tl) + "'";
            sSQL += ",'" + fromdate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + todate.ToString("yyyy.MM.dd") + "'";
            DataTable dt = Commons.GetData(sSQL);

            Export d = new Export();

            d.ToExcel(Response, dt, "moveitem");
            return View();
        }
        public ActionResult ExportMoveItemDetail()
        {
            string fl = Commons.ConvertToString(Request.QueryString["fl"]);
            string tl = Commons.ConvertToString(Request.QueryString["tl"]);
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            try
            {
                string[] l = from.Split('/');
                fromdate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = to.Split('/');
                todate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }
            string sSQL = "exec SP_ExportMoveItemDetail ";
            sSQL += "N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ", N'" + Commons.Fix(fl) + "'";
            sSQL += ", N'" + Commons.Fix(tl) + "'";
            sSQL += ",'" + fromdate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + todate.ToString("yyyy.MM.dd") + "'";
            DataTable dt = Commons.GetData(sSQL);

            Export d = new Export();

            d.ToExcel(Response, dt, "moveitemdetail");
            return View();
        }
        [HttpPost]
        public ActionResult CheckTimeLogin()
        {
            if (GlobalVariables.TimeLogin == "")
            {
                SetLoginTime(GlobalVariables.UserID);
                return Json(new { msg = "ok", success = true });
            }

            else
            {
                string sSQL = "select TimeLogin from LoginInfo where AdminID = " + GlobalVariables.UserID.ToString("0");
                sSQL += " and TimeLogin>'" + Commons.Fix(GlobalVariables.TimeLogin) + "'";
                DataTable dt = Commons.GetData(sSQL);

                if (dt.Rows.Count > 0)
                {
                    return Json(new { errorMsg = "not ok", success = false });
                }
            }
            if (Session["clogin"] == null)
            {
                GlobalVariables.UserID = GlobalVariables.UserID;
                GlobalVariables.CN = GlobalVariables.CN;
                GlobalVariables.DivisionID = GlobalVariables.DivisionID;
                GlobalVariables.UserName = GlobalVariables.UserName;
                Session["clogin"] = 1;
            }
            return Json(new { msg = "ok", success = true });


        }
        public ActionResult BaoCaoChamHang()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string sSQL = "exec SP_GetCountChamHang N'" + Commons.Fix(GlobalVariables.DivisionID) + "',N'" + Commons.Fix(keyword) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/BaocaoChamHang?key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }

        [HttpPost]
        public ActionResult Get_ChamHang()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "exec SP_GetChamHang '" + Commons.Fix(GlobalVariables.DivisionID) + "',N'" + Commons.Fix(keyword) + "' ";
            sSQL += "," + CurrentPage.ToString("0") + ",20";

            DataTable dt = Commons.GetData(sSQL);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Location = p["Location"],
                            Volume = p["Volume"],
                            VolumeUsed = p["VolumeUsed"],
                            Quantity = p["Quantity"]

                        };
            return Json(query);

        }


        public ActionResult XuatChamHang()
        {
            string ssql = "exec SP_XuatChamHang N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(ssql);
            Export d = new Export();
            d.ToExcel(Response, dt, "baocaochamhang");
            return View();
        }
        public ActionResult KeCham()
        {
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "select count(Location) C from VTCH  ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location like N'" + Commons.Fix(Keyword) + "%' ";
            sSQL += " order by 1 ";

            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/KeCham?key=" + Keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }
        [HttpPost]
        public ActionResult LayKeCham()
        {
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int from = (CurrentPage - 1) * PAGE_SIZE + 1;
            int to = CurrentPage * PAGE_SIZE;
            string sSQL = "select V.* from (select top 100 percent Row_Number() over(order by Location) Position, Location from VTCH  ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and Location like N'" + Commons.Fix(Keyword) + "%' ";

            sSQL += " order by Location ";
            sSQL += ") V where V.Position between " + from.ToString("0") + " and " + to.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Location = p["Location"].ToString(),

                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult ThemKeCham(string Location)
        {
            Location = Location.ToUpper();
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("kexetchamhang") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string sWrite = "insert into VTCH (DivisionID,Location)";
            sWrite += "values(N'" + Commons.Fix(GlobalVariables.DivisionID) + "',N'" + Commons.Fix(Location) + "')";
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult XoaKeCham(string Location)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("kexetchamhang") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string sWrite = "Delete VTCH ";
            sWrite += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' and Location=N'" + Commons.Fix(Location) + "' ";
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult UPRC(int RC, decimal TTT, int ForOne, int OpenForOut, int XuatThang, int AllowNotFull
            , string LockedDate, string GoogleAPIKey, int thongtintudonhang


            )
        {
            string sWrite = "Update Settings ";
            sWrite += "set RC=" + RC.ToString("0");
            sWrite += ", TTT=" + Commons.DecimalToSQL(TTT);
            sWrite += ", GoogleAPIKey=N'" + Commons.Fix(GoogleAPIKey) + "'";
            sWrite += ", ForOne=" + ForOne;
            sWrite += ", OpenForOut=" + OpenForOut;
            sWrite += ", XuatThang=" + XuatThang;
            sWrite += ", AllowNotFull=" + AllowNotFull;
            sWrite += ", thongtintudonhang=" + thongtintudonhang;
            sWrite += ";";

            sWrite += " if not exists(select DivisionID from KKLocks where DivisionID='" + Fix(GlobalVariables.DivisionID) + "')";
            sWrite += "insert into KKLocks(DivisionID,InventoryLockedDate) values(";
            sWrite += "N'" + Fix(GlobalVariables.DivisionID) + "','" + Fix(LockedDate) + "'";
            sWrite += ")";
            sWrite += " else ";
            sWrite += " update  KKLocks set InventoryLockedDate='" + Fix(LockedDate) + "' ";
            sWrite += " where DivisionID='" + Fix(GlobalVariables.DivisionID) + "'";

            Exception ex = null;
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }

            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        public ActionResult BaoCao()
        {
            return View();
        }
        public ActionResult AddMItem()
        {
            return View();
        }
        //the tich bang cm3
        //KTT: kich thuoc thung
        //KTSP : kich thuoc san pham
        //SLSP: so luong san pham
        public int ChuanHoaSLDeDuThung(decimal KTT, decimal KTSP, int SLSP)
        {
            int result = 0;
            int slthung = 1;
            int sp_per_box = 1;
            if (KTSP == 0)
            {
                KTSP = 700;
            }

            sp_per_box = (int)Math.Floor(KTT / KTSP);
            if (SLSP <= sp_per_box)
            {
                result = sp_per_box;
            }
            else
            {
                slthung = (int)Math.Ceiling(SLSP * 1.0 / sp_per_box);
                result = slthung * sp_per_box;
            }
            return result;
        }
        //the tich bang cm3
        //KTT: kich thuoc thung
        //KTSP : kich thuoc san pham
        //SLSP: so luong san pham
        public int QuyRaThung(decimal KTT, decimal KTSP, int SLSP)
        {
            if (KTSP == 0)
            {
                KTSP = 700;

            }

            int sp_per_box = (int)Math.Floor(KTT / KTSP);
            int slthung = (int)Math.Ceiling(SLSP * 1.0 / sp_per_box);
            return slthung;
        }
        public ActionResult EX()
        {

            DataTable dt = new DataTable();
            int id = Commons.ConvertToInt(Request.QueryString["id"]);
            string ss = Commons.ConvertToString(Request.QueryString["s"]);
            Export d = new Export();
            switch (id)
            {
                case 1:
                    dt = Commons.GetData("exec SP_BaoCaoChamHang N'" + Commons.Fix(GlobalVariables.DivisionID) + "'");
                    DataTable result = new DataTable();
                    result.Columns.Add("TT", id.GetType());
                    result.Columns.Add("From");
                    result.Columns.Add("ItemID");
                    result.Columns.Add("Quantity", id.GetType());
                    result.Columns.Add("To");

                    DataTable db = Commons.GetData("exec SP_GetDataChamHang N'" + Commons.Fix(GlobalVariables.DivisionID) + "'");
                    int i = 1;
                    foreach (DataRow item in dt.Rows)
                    {
                        string itemID = item["ItemID"].ToString();
                        int Quantity = Commons.ConvertToInt(item["Quantity"]);
                        decimal CM3 = Commons.ConvertToDecimal(item["CM3"]);
                        if (CM3 == 0)
                        {
                            CM3 = 700;
                        }

                        decimal M3 = Commons.ConvertToDecimal(item["M3"]);
                        int sosanphamcothechua = (int)Math.Floor(M3 * 1000000 / CM3);
                        if (sosanphamcothechua == 0)
                        {
                            continue;
                        }

                        Quantity = sosanphamcothechua;

                        string ToLocation = item["Location"].ToString();
                        db.DefaultView.RowFilter = "ItemID='" + itemID + "' and Location<>'" + ToLocation + "'";
                        //DataRow[] foundRows;
                        //string expression;
                        //expression = "ItemID = '"+ itemID + "' and Location<>'" + ToLocation + "' ";

                        //foundRows = db.Select(expression);

                        foreach (DataRowView item1 in db.DefaultView)
                        {
                            int SLHT = Commons.ConvertToInt(item1["Quantity"]);
                            if (SLHT > 0)
                            {
                                if (SLHT >= Quantity)
                                {
                                    item1["Quantity"] = SLHT - Quantity;
                                    DataRow r = result.NewRow();
                                    r["TT"] = i;
                                    r["From"] = item1["Location"];
                                    r["ItemID"] = itemID;
                                    r["Quantity"] = Quantity;
                                    r["To"] = ToLocation;
                                    result.Rows.Add(r);
                                    i++;
                                    break;
                                }
                                else
                                {
                                    Quantity = Quantity - Commons.ConvertToInt(item1["Quantity"]);

                                    DataRow r = result.NewRow();
                                    r["TT"] = i;
                                    r["From"] = item1["Location"];
                                    r["ItemID"] = itemID;
                                    r["Quantity"] = SLHT;
                                    r["To"] = ToLocation;
                                    result.Rows.Add(r);
                                    item1["Quantity"] = 0;

                                    i++;
                                }
                            }

                        }
                    }
                    result.Columns[0].ColumnName = "STT";
                    result.Columns[1].ColumnName = "VỊ TRÍ ĐI";
                    result.Columns[2].ColumnName = "MÃ HÀNG";
                    result.Columns[3].ColumnName = "SỐ LƯỢNG";
                    result.Columns[4].ColumnName = "VỊ TRÍ ĐẾN";

                    d.ToExcel(Response, result, "baocaochamhangchudong_" + GlobalVariables.UserName);
                    break;
                case 2:
                    DataTable sss = (DataTable)Session[ss];
                    d.ToExcel(Response, sss, "bc" + GlobalVariables.UserName);

                    break;
                default:
                    break;
            }
            return View();
        }

        public List<CH> TruSoTonCoSan(List<CH> ar)
        {
            List<CH> result = new List<CH>();
            string sSQL = "select P.ItemID,sum(P.Quantity) Quantity  from VTCH V ";
            sSQL += " inner join Locations L on V.Location=L.Location  ";
            sSQL += " and V.DivisionID=L.DivisionID ";
            sSQL += " inner join BalanceAll P on P.DivisionID=V.DivisionID ";
            sSQL += " and P.Location=V.Location ";
            sSQL += " where V.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " group by P.ItemID ";
            sSQL += " having  isnull(sum(P.Quantity),0) >0 ";
            DataTable dt = Commons.GetData(sSQL);
            foreach (CH item in ar)
            {
                dt.DefaultView.RowFilter = "ItemID = '" + item.ItemID + "' ";
                if (dt.DefaultView.Count > 0)
                {
                    DataRowView r = dt.DefaultView[0];
                    int Quantity = Commons.ConvertToInt(r["Quantity"]);
                    if (item.Quantity <= Quantity)
                    {
                        item.Quantity = 0;
                    }

                    else
                    {
                        item.Quantity = item.Quantity - Quantity;
                    }
                }
            }


            foreach (CH item in ar)
            {
                if (item.Quantity > 0)
                {
                    CH r = new CH();
                    r.ItemID = item.ItemID;
                    r.Quantity = item.Quantity;
                    result.Add(r);
                }
            }
            return result;
        }
        public ActionResult BCChamHangBiDong()
        {
            string sSQL = "";
            DataTable dt = new DataTable();
            if (GlobalVariables.ChamHang == null)
            {
                Response.Redirect("~/admin/AddMItem");
            }


            DataTable result = Commons.GetData("exec SP_ExportCHBD " + GlobalVariables.UserID.ToString("0") + ",  N'" + Fix(GlobalVariables.DivisionID) + "' ");

            Export d = new Export();
            d.ToExcel(Response, result, "baocaochamhangbidongchitiet");
            return View();
        }
        public ActionResult BCChamHangBiDong2()
        {
            string sSQL = "";
            DataTable dt = new DataTable();
            if (GlobalVariables.ChamHang == null)
            {
                Response.Redirect("~/admin/AddMItem");
            }


            DataTable result = Commons.GetData("exec SP_ExportCHBD2 " + GlobalVariables.UserID.ToString("0") + ",  N'" + Fix(GlobalVariables.DivisionID) + "' ");

            Export d = new Export();
            d.ToExcel(Response, result, "baocaochamhangbidongchitiet");
            return View();
        }
        //MC 
        public ActionResult ViewMC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Get_MC()
        {

            string sSQL = "select MC,MCD from  MC order by 1 ";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {

                            MC = Commons.ConvertToString(p["MC"]),
                            MCD = Commons.ConvertToString(p["MCD"])

                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult Add_MC(string MC, string MCD)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            MC = MC.Trim();


            string sWrite = "";

            sWrite += "if not exists (select MC from MC where MC='" + Commons.Fix(MC) + "') \n";
            sWrite += " Insert into MC(MC,MCD) values(N'" + Commons.Fix(MC) + "',N'" + Commons.Fix(MCD) + "');\n";
            sWrite += " else\n";
            sWrite += " Update MC set MCD=N'" + Commons.Fix(MCD) + "' where MC=N'" + Commons.Fix(MC) + "' ;\n";

            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult Delete_MC(string MC)
        {
            string sWrite = "delete MC where MC='" + Commons.Fix(MC) + "' ";

            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Xoá thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        //MC hang cham
        public ActionResult ViewMCHC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Get_MCHC()
        {

            string sSQL = "select A.MC,B.MCD from MCHC A left join MC B on A.MC=B.MC order by 1 ";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {

                            MC = Commons.ConvertToString(p["MC"]),
                            MCD = Commons.ConvertToString(p["MCD"])

                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult Add_MCHC(string MC)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            MC = MC.Trim();
            MC = MC.Trim('\n');
            string[] l = MC.Split('\n');

            string sWrite = "";
            foreach (string item in l)
            {
                sWrite += "if not exists (select MC from MCHC where MC='" + Commons.Fix(item) + "') Insert into MCHC(MC) values('" + Commons.Fix(item) + "');";
            }
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Thêm thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult Delete_MCHC(string MC)
        {
            string sWrite = "delete MCHC where MC='" + Commons.Fix(MC) + "' ";

            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Xoá thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public ActionResult CHFromDate()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CHTN(string FromDate, string ToDate)
        {
            try
            {
                FromDate = FromDate.Replace("'", "");
                ToDate = ToDate.Replace("'", "");

                string[] l = FromDate.Split('/');
                string f = l[2] + "." + l[1] + "." + l[0];
                l = ToDate.Split('/');
                string to = l[2] + "." + l[1] + "." + l[0];

                List<CH> ar = new List<CH>();
                string sSQL = "select D.ItemID,sum(D.Quantity) Quantity from WD D ";
                sSQL += " inner join DataOB M on D.OB=M.OB and D.DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += " where Convert(nvarchar(20),M.PlanDate,102) between '" + f + "' and '" + to + "'";

                sSQL += " group by  D.ItemID ";
                DataTable dt = Commons.GetData(sSQL);
                foreach (DataRow item in dt.Rows)
                {
                    CH r = new CH();
                    r.ItemID = item["ItemID"].ToString();
                    r.Quantity = Convert.ToInt32(item["Quantity"]);
                    r.Quantity1 = Convert.ToInt32(item["Quantity"]);
                    r.CM3 = 0;
                    ar.Add(r);
                }

                GlobalVariables.ChamHang = ar;

                return Json(new { msg = "Thành công", success = true });
            }
            catch (Exception e)
            {
                return Json(new { errorMsg = e.Message, success = false });


            }


        }


        public ActionResult ID8()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckPickingList(string VoucherID)
        {
            if (VoucherID.Trim() == "")
            {
                return Json(new { errorMsg = "Phiếu này không hợp lệ", success = false });
            }

            string sSQL = "";
            if (VoucherID.Substring(0, 1) == "8")
            {
                sSQL = "select top 1 OB,VoucherID,ReceiveQuantity from WD where OB=N'" + Commons.Fix(VoucherID) + "' ";
                sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' order by OB ";


            }

            else
            {
                sSQL = "select top 1 OB,VoucherID,ReceiveQuantity from WD where VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
                sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' order by OB ";

            }
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu này không tồn tại", success = false });
            }

            string OB = dt.Rows[0][0].ToString();
            int Quantity = Commons.ConvertToInt(dt.Rows[0][2]);
            //if (Quantity == 0)
            //{
            //    string sm = "";
            //    bool b = GetScannerFromOBCN(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), ref sm);
            //    if (b == false)
            //    {
            //        return Json(new { errorMsg = sm, success = false });
            //    }
            //}

            if (dt.Rows.Count > 0)
            {
                return Json(new { msg = "Xác nhận thành công", OB = OB, VoucherID = dt.Rows[0][1], success = true });
            }
            else
            {
                return Json(new { errorMsg = "Phiếu này không tồn tại", success = false });

            }

        }
        [HttpPost]
        public ActionResult UpdatePickingList(string VoucherID)
        {
            if (VoucherID.Trim() == "")
            {
                return Json(new { errorMsg = "Phiếu này không hợp lệ", success = false });
            }

            string sSQL = "";
            if (VoucherID.Substring(0, 1) == "8")
            {
                sSQL = "select top 1 OB,VoucherID,ReceiveQuantity from WD where OB=N'" + Commons.Fix(VoucherID) + "' ";
                sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' order by OB ";


            }

            else
            {
                sSQL = "select top 1 OB,VoucherID,ReceiveQuantity from WD where VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
                sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' order by OB ";

            }
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu này không tồn tại", success = false });
            }

            string v = dt.Rows[0][1].ToString();

            sSQL = "select OB from WD where VoucherID=N'" + Commons.Fix(v) + "' ";
            sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " group by  OB ";
            DataTable db = Commons.GetData(sSQL);
            if (db.Rows.Count > 1)
            {
                return Json(new { errorMsg = "Phiếu này có hơn 2 đầu 8. Bạn không được cập nhật ở đây", success = false });
            }

            string OB = dt.Rows[0][0].ToString();
            string sm = "";
            bool b = GetScannerFromOBCN(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), ref sm);
            if (b == false)
            {
                return Json(new { errorMsg = sm, success = false });
            }

            if (dt.Rows.Count > 0)
            {
                return Json(new { msg = "Cập nhật thành công", OB = OB, VoucherID = dt.Rows[0][1], success = true });
            }
            else
            {
                return Json(new { errorMsg = "Phiếu này không tồn tại", success = false });

            }

        }
        [HttpPost]
        public ActionResult ConfirmPickingListFromWinform(string VoucherID)
        {
            if (VoucherID.Trim() == "")
            {
                return Json(new { errorMsg = "Phiếu này không hợp lệ", content = "", success = false });
            }

            string sSQL = "";
            if (VoucherID.Substring(0, 1) == "8")
            {
                sSQL = "select top 1 OB,VoucherID,Confirmed from XH where OB=N'" + Commons.Fix(VoucherID) + "' ";
                sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' order by OB ";


            }

            else
            {
                sSQL = "select top 1 OB,VoucherID,Confirmed from XH where VoucherID=N'" + Commons.Fix(VoucherID) + "' ";
                sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' order by OB ";

            }

            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu này không tồn tại hoặc chưa có vị trí", content = "", success = false });
            }

            string OB = dt.Rows[0][0].ToString();
            VoucherID = dt.Rows[0][1].ToString();
            bool Confirmed = Commons.ConvertToBool(dt.Rows[0]["Confirmed"]);

            if (Confirmed)
            {
                return Json(new { errorMsg = "Phiếu này đã xác nhận rồi", content = GetOBReceive(OB), success = false });

            }
            string sMessage = "";
            bool b = GetScannerFromOBCN(OB, VoucherID, ref sMessage);
            if (b == false)
            {
                return Json(new { errorMsg = sMessage, content = "", success = false });
            }

            bool result = ConfirmXH(OB, ref sMessage);
            if (result)
            {
                string www = "exec SP_UpdateOB N'" + Commons.ConvertToString(OB) + "' , N'" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'";
                //www += ";exec UpdateOBListFromScannerResult N'" + Fix(OB) + "' , N'" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'";
                Commons.ExecuteNoneQuery(www);

                return Json(new { msg = sMessage, content = GetOBReceive(OB), OB = OB, VoucherID = VoucherID, success = true });
            }
            else
            {
                return Json(new { errorMsg = sMessage, content = "", success = false });

            }

        }
        ////xac nhan cho ung dung
        //[HttpPost]
        //public ActionResult CNS()
        //{
        //    if (Request.Headers.HasKeys() == false)
        //    {
        //        Response.Write("Chưa có APIKey");
        //        Response.End();
        //        return View();
        //    }
        //    string key = Request.Headers.Get("APIKey");
        //    GetAndPostController d = new GetAndPostController();
        //    if (key != d.Key)
        //    {
        //        Response.Write("Key " + key + " không hợp lệ");
        //        Response.End();
        //        return View();
        //    }
        //    string site = Commons.ConvertToString(Request.QueryString["site"]);
        //    if (site == "1100" || site == "")
        //    {
        //        GlobalVariables.CN = "CNMN";
        //    }
        //    else
        //        if (site == "1200")
        //    {
        //        GlobalVariables.CN = "CNMB";
        //    }
        //    Response.Write(Commons.ConnectionString);
        //    Response.End();
        //    return View();
        //}
        //xac nhan cho ung dung
        [HttpPost]
        public ActionResult XacNhanTuWin(string data)
        {
            try
            {
                if (Request.Headers.HasKeys() == false)
                {
                    Response.Write("Chưa có APIKey");
                    Response.End();
                    return View();
                }



                string key = Request.Headers.Get("APIKey");
                GetAndPostController d = new GetAndPostController();
                if (key != d.Key)
                {
                    Response.Write("Key " + key + " không hợp lệ");
                    Response.End();
                    return View();
                }

                string OB = Commons.ConvertToString(Request.QueryString["id"]);
                OB = OB.Replace("'", "").Replace(" ", "");
                GlobalVariables.CN = "KhoDC";
                try
                {
                    System.IO.File.WriteAllText(Server.MapPath("~/ob/" + OB), data);

                }
                catch
                {


                }

                //Response.Write("Thành công");
                //Response.End();
                //return View();

                if (OB.Trim() == "")
                {
                    Response.Write("Outbound này không hợp lệ");
                    Response.End();
                    return View();
                }
                else
                if (OB.Length < 10)
                {
                    Response.Write("Outbound này không hợp lệ");
                    Response.End();
                    return View();
                }
                string sSQL = "";
                sSQL = "select top 1 OB,VoucherID,DivisionID from WD where OB=N'" + Commons.Fix(OB) + "' ";

                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    if (data == "" || data == null)
                    {
                        Response.Write("Không có dữ liệu");
                        Response.End();
                        return View();
                    }


                    string[] lines1 = data.Split('|');
                    int TSL = 0;
                    foreach (string item in lines1)
                    {
                        string[] list = item.Split(' ');
                        string ItemID = list[0];
                        int Quantity = Commons.ConvertToInt(list[1]);
                        TSL += Quantity;

                        if (list.Length != 2)
                        {
                            Response.Write("Dữ liệu không hợp lệ ");
                            Response.End();
                            return View();
                        }

                    }
                    sSQL = "select TotalQuantityCT,DivisionID from OBList where OB=N'" + Commons.Fix(OB) + "' ";
                    dt = Commons.GetData(sSQL);
                    if (dt.Rows.Count == 0)
                    {
                        Response.Write("Bạn chưa nhập bao thùng");
                        Response.End();
                        return View();
                    }
                    if (TSL != Commons.ConvertToInt(dt.Rows[0][0]))
                    {
                        Response.Write("Số lượng nhập bao thùng không khớp");
                        Response.End();
                        return View();
                    }
                    string w1 = "update OBList set TotalQuantity=" + TSL.ToString("0") + ",Locked=1 where OB='" + Fix(OB) + "'";
                    w1 += ";exec [SP_UpdateOBDSum] N'" + Fix(Commons.ConvertToString(dt.Rows[0][1])) + "','" + Fix(OB) + "' ";
                    Commons.ExecuteNoneQuery(w1);
                    Response.Write("Thành công");
                    Response.End();
                    return View();
                }

                sSQL = "select top 1 OB,VoucherID,DivisionID,Confirmed from XH where OB=N'" + Commons.Fix(OB) + "' ";




                dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    Response.Write("Phiếu này không tồn tại hoặc chưa có vị trí");
                    Response.End();



                    return View();
                }
                string DivisionID = dt.Rows[0]["DivisionID"].ToString();
                bool Confirmed = Commons.ConvertToBool(dt.Rows[0]["Confirmed"]);
                string VoucherID = dt.Rows[0][1].ToString();


                //sSQL = "select dbo.[isConfirmOB] (N'" + Commons.Fix(OB) + "' ";
                //sSQL += " , N'" + Commons.Fix(DivisionID) + "' ) xn ";
                //dt = Commons.GetData(sSQL);

                if (Confirmed)
                {

                    Response.Write("Phiếu này đã xác nhận rồi");
                    Response.End();
                    return View();

                }
                string sMessage = "";

                if (data == "" || data == null)
                {
                    Response.Write("Không có dữ liệu");
                    Response.End();
                    return View();
                }


                dt = new DataTable();
                int Q = 0;
                dt.Columns.Add("ItemID", "".GetType());
                dt.Columns.Add("ReceiveQuantity", Q.GetType());
                dt.Columns.Add("DivisionID", "".GetType());

                Exception ex = null;

                string[] lines = data.Split('|');
                foreach (string item in lines)
                {
                    string[] list = item.Split(' ');
                    string ItemID = list[0];
                    int Quantity = Commons.ConvertToInt(list[1]);
                    if (list.Length != 2)
                    {
                        Response.Write("Dữ liệu không hợp lệ ");
                        Response.End();
                        return View();
                    }
                    DataRow r = dt.NewRow();
                    r[0] = ItemID;
                    r[1] = Quantity;
                    r[2] = DivisionID;
                    dt.Rows.Add(r);
                }
                if (BulkCopyOB(dt, "OutBound" + OB, ref ex) == false)
                {
                    Response.Write("Có lỗi trong quá trình cập nhật. Vui lòng kiểm tra lại dữ liệu " + ex.Message);
                    Response.End();
                    return View();
                }
                //cap nhat data vao web
                string sWrite = "";
                sWrite += " update WD set ReceiveQuantity= 0";
                sWrite += " where DivisionID = N'" + Commons.Fix(DivisionID) + "' ";
                sWrite += " and OB = N'" + Commons.Fix(OB) + "'; ";
                sWrite += " update WD set ReceiveQuantity= isnull((select ReceiveQuantity";
                sWrite += " from OutBound" + OB.Replace("'", "") + " where ItemID=WD.ItemID and DivisionID='" + Fix(DivisionID) + "' ";
                sWrite += "   ),0) ";
                sWrite += " where DivisionID = N'" + Commons.Fix(DivisionID) + "' ";
                sWrite += " and OB = N'" + Commons.Fix(OB) + "'; ";
                ex = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                Commons.ExecuteNoneQuery("drop table OutBound" + OB);
                if (b == false)
                {
                    Response.Write(ex.Message);
                    Response.End();
                    return View();
                }

                bool result = ConfirmXH(OB, ref sMessage, DivisionID);
                if (result)
                {
                    string www = "exec SP_UpdateOB N'" + Commons.ConvertToString(OB) + "' , N'" + Commons.ConvertToString(DivisionID) + "'";
                    Commons.ExecuteNoneQuery(www);
                    Response.Write("Thành công");
                    Response.End();
                    return View();
                }
                else
                {
                    Response.Write(sMessage);
                    Response.End();
                    return View();
                }
            }
            catch (Exception exx)
            {

                Response.Write(exx.Message);
                Response.End();
                return View();
            }


        }
        public bool BulkCopyOB(DataTable dt, string TableName, ref Exception e)
        {
            try
            {
                dt.TableName = TableName;
                string tablescript = "select ItemID,ReceiveQuantity,DivisionID into " + TableName + " from WD where 1=2 ";
                Commons.ExecuteNoneQuery("drop table " + TableName);
                Commons.ExecuteNoneQuery(tablescript);

                using (SqlConnection destinationConnection = new SqlConnection(Commons.ConnectionString))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    destinationConnection.Open();
                    bulkCopy.DestinationTableName = TableName;
                    bulkCopy.WriteToServer(dt);
                    destinationConnection.Close();

                }
            }
            catch (Exception ex)
            {
                e = ex;
                return false;

            }


            return true;
        }
        public string GetOBReceive(string OB)
        {
            string sSQL = "exec GetOBScannerInfo '" + Fix(OB) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string sContent = "";
            sContent = dt.Rows[0][0].ToString();
            return sContent;
        }

        public string ComparessOB(string OB, string ConnectionString)
        {
            string sResult = "";

            string sSQL = "select  [Mahh],sum(  [Luong] ) Luong from OutBound where OutBound='" + Commons.Fix(OB) + "' ";
            sSQL += " and Luong>0 ";
            sSQL += "  group by  [Mahh] ";
            DataTable dt = Commons.GetDataFromOtherDataBase(sSQL, ConnectionString);
            if (dt.Rows.Count == 0)
            {
                return "<br/><span style='color:red'>Scanner chưa có dữ liệu " + OB + "</span>";

            }
            sSQL = "select ItemID,sum(ReceiveQuantity) Quantity from WD where OB='" + Commons.Fix(OB) + "' and isnull(ReceiveQuantity,0) > 0  group by ItemID";

            DataTable dc = Commons.GetData(sSQL);
            if (dc.Rows.Count == 0)
            {
                return "<br/><span style='color:red'>chưa có dữ liệu quét " + OB + " bên web </span>";

            }
            int T1 = 0, T2 = 0;
            foreach (DataRow r in dc.Rows)
            {
                string ItemID = r["ItemID"].ToString();
                dt.DefaultView.RowFilter = "Mahh='" + ItemID + "'";
                if (dt.DefaultView.Count == 0)
                {
                    sResult += "<br/><span style='color:red'>" + OB + " Không có dữ liệu " + ItemID + " bên scanner </span>";

                }
                else
                {
                    int Q1 = Commons.ConvertToInt(r["Quantity"]);
                    int Q2 = Commons.ConvertToInt(dt.DefaultView[0]["Luong"]);
                    T1 += Q1;
                    T2 += Q2;

                    if (Q1 != Q2)
                    {
                        sResult += "<br/><span style='color:red'>Dữ liệu " + ItemID + " bên scanner là " + Q2 + " khác bên web(SL:" + Q1 + ") => không khớp </span>";
                    }
                }


            }
            if (dc.Rows.Count < dt.Rows.Count)
            {
                return "<br/><span style='color:red'>Dữ liệu " + OB + " bên scanner nhiều dòng hơn</span>";

            }
            if (sResult == "")
            {
                sResult = "<br/><span style='color:green'>Khớp dữ liệu " + OB + " Scanner: " + T2.ToString("N0") + " - Web: " + T1.ToString("N0") + "</span>";
            }

            return sResult;
        }
        [HttpPost]
        public ActionResult Comparess(string VoucherID)
        {
            string sSQL = "select ConnectionString from Divisions where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'  ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {

                return Json(new { errorMsg = "Chưa thiết lập kết nối với scanner", success = false });

            }

            string ConnectionString = Commons.ConvertToString(dt.Rows[0][0]);
            if (ConnectionString == "")
            {

                return Json(new { errorMsg = "Chưa thiết lập kết nối với scanner", success = false });
            }

            try
            {
                sSQL = "select OB from WD where VoucherID = N'" + Fix(VoucherID) + "' and DivisionID = N'" + Fix(GlobalVariables.DivisionID) + "' group by OB ";
                DataTable db = Commons.GetData(sSQL);
                foreach (DataRow item in db.Rows)
                {
                    string OB = item[0].ToString();
                    sSQL = "select  [Mahh],sum(  [Luong] ) Luong from OutBound where OutBound='" + Commons.Fix(OB) + "' ";
                    sSQL += "  group by  [Mahh] ";
                    dt = Commons.GetDataFromOtherDataBase(sSQL, ConnectionString);
                    if (dt.Rows.Count == 0)
                    {
                        return Json(new { errorMsg = "Scanner chưa có dữ liệu " + OB, success = false });

                    }
                    sSQL = "select ItemID,sum(ReceiveQuantity) Quantity from WD where OB='" + Commons.Fix(OB) + "' and VoucherID='" + Fix(VoucherID) + "' group by ItemID";

                    DataTable dc = Commons.GetData(sSQL);
                    if (dc.Rows.Count != dt.Rows.Count)
                    {
                        return Json(new { errorMsg = "Dữ liệu scanner và web ở " + OB + " không bằng nhau số dòng (Scanner: " + dt.Rows.Count + " - web: " + dc.Rows.Count + ")", success = false });

                    }
                    foreach (DataRow r in dc.Rows)
                    {
                        string ItemID = r["ItemID"].ToString();
                        dt.DefaultView.RowFilter = "Mahh='" + ItemID + "'";
                        if (dt.DefaultView.Count == 0)
                        {
                            return Json(new { errorMsg = "Không có dữ liệu " + ItemID + " bên scanner ", success = false });

                        }
                        int Q1 = Commons.ConvertToInt(r["Quantity"]);
                        int Q2 = Commons.ConvertToInt(dt.DefaultView[0]["Luong"]);
                        if (Q1 != Q2)
                        {
                            return Json(new { errorMsg = "Dữ liệu " + ItemID + " bên scanner là " + Q2 + " khác bên web(SL:" + Q1 + ") => không khớp", success = false });

                        }
                    }
                }

                return Json(new { msg = "Khớp số liệu", success = true });


            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

        }
        public bool GetScannerFromOBCN(string OB, string VoucherID, ref string Message, string DivisionID = "")
        {
            if (DivisionID == "")
            {
                DivisionID = GlobalVariables.DivisionID;
            }

            string sSQL = "select ConnectionString from Divisions where DivisionID=N'" + Commons.Fix(DivisionID) + "'  ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                Message = "Chưa thiết lập kết nối với scanner";
                return false;
            }

            string ConnectionString = Commons.ConvertToString(dt.Rows[0][0]);
            if (ConnectionString == "")
            {
                Message = "Chưa thiết lập kết nối với scanner";

                return false;
            }

            try
            {
                sSQL = "select  Mahh ItemID,sum(  [Luong] ) ReceiveQuantity,'" + Fix(GlobalVariables.DivisionID) + "' DivisionID from OutBound where OutBound='" + Commons.Fix(OB) + "' ";
                sSQL += " and isnull([Luong],0) > 0 group by  [Mahh] ";
                dt = Commons.GetDataFromOtherDataBase(sSQL, ConnectionString);
                if (dt.Rows.Count == 0)
                {
                    Message = "Scanner chưa có dữ liệu";

                    return false;
                }
                Exception ex = null;
                if (BulkCopyOB(dt, "OutBound" + OB, ref ex) == false)
                {
                    Message = ex.Message;
                    return false;
                }
                //cap nhat data vao web
                string sWrite = "";
                sWrite += " update WD set ReceiveQuantity= 0";
                sWrite += " where DivisionID = N'" + Commons.Fix(DivisionID) + "' ";
                sWrite += " and OB = N'" + Commons.Fix(OB) + "'; ";
                sWrite += " update WD set ReceiveQuantity= isnull((select ReceiveQuantity";
                sWrite += " from OutBound" + OB.Replace("'", "") + " where ItemID=WD.ItemID and DivisionID='" + Fix(DivisionID) + "' ";
                sWrite += "   ),0) ";
                sWrite += " where DivisionID = N'" + Commons.Fix(DivisionID) + "' ";
                sWrite += " and OB = N'" + Commons.Fix(OB) + "'; ";




                Exception xxx = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref xxx);

                Commons.ExecuteNoneQuery("drop table OutBound" + OB);

                if (b == true)
                {
                    return true;
                }
                else

                {
                    Message = xxx.Message;
                    return false;
                }
            }
            catch (Exception)
            {
                Message = "Không kết nối được với server scanner vui lòng liên hệ quản trị server";
                return false;

            }
            return true;
        }
        public ActionResult TruyVet()
        {
            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "select ItemID,Location,Quantity from BalanceAll where ItemID='" + Fix(ItemID) + "' ";
            DataTable dt = Commons.GetData(ssql);
            string Content = "";
            foreach (DataRow item in dt.Rows)
            {
                Content += "<p>" + item["Location"] + " : " + item["Quantity"] + "</p>";
            }
            ViewBag.content = Content;
            return View();
        }
        public ActionResult DiHangTheoMa()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/logout");
            }

            if (Global.Commons.CheckPermit("dihangtheoma") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            return View();
        }
        [HttpPost]
        public ActionResult GetByItem()
        {
            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);
            int G = Commons.ConvertToInt(Request.QueryString["g"]);
            string bc = "select m.tolocation from GroupM g inner join moveitems m on g.divisionid=m.divisionid and m.voucherid=g.voucherid   ";
            bc += " and g.groupid=" + G.ToString("0");
            DataTable db = Commons.GetData(bc);
            string ll = "'xxxxx'";

            foreach (DataRow I in db.Rows)
            {
                ll += ",'" + I[0] + "'";
            }

            string sSQL = "select sum(D.Quantity) Quantity,D.Location ";
            sSQL += " from balanceall D  ";
            sSQL += " where D.DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' and location not in(" + ll + ")";
            sSQL += " and D.ItemID='" + Commons.Fix(ItemID) + "'";
            sSQL += " group by D.ItemID,D.Location ";

            sSQL += " having sum(D.Quantity)>0 ";
            sSQL += " order by D.Location ";
            DataTable dt = Commons.GetData(sSQL);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Location = p["Location"],
                            LSX = "",
                            Quantity = p["Quantity"]
                        };
            return Json(query);

        }
        [HttpPost]
        public ActionResult GetGroupVoucher(string BarCode)
        {
            BarCode = BarCode.ToUpper();

            string ItemID = "";
            if (BarCode.Length == 14)
            {
                ItemID = BarCode;
            }
            else
             if (BarCode.Length == 18)
            {
                CC r = new CC();
                if (BarCode.Substring(BarCode.Length - 1, 1) == "0")
                {
                    r = GetFrom18(BarCode);
                }
                else
                {
                    List<CC> sL = GetFrom1811(BarCode);
                    if (sL.Count == 0)
                    {
                        return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                    }

                    r = sL[0];

                }

                if (r.i == "")
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }

                ItemID = r.i;
            }
            else
             if (BarCode.Length == 27)
            {
                CC r = GetFrom27(BarCode);
                if (r.i == "")
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }

                ItemID = r.i;
            }
            if (ItemID == "")
            {
                return Json(new { errorMsg = "Tem không hợp lệ", success = false });
            }

            string ssql = "exec SP_GetGroup " + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(ssql);
            Object v = dt.Rows[0][0];
            return Json(new { msg = "ok", g = v, itemid = ItemID, success = true });


        }


        [HttpPost]
        public ActionResult Add_ListItem(List<ItemL> LItem, string LocationTo, string Description, int G)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Tài khoản không hợp lệ. Vui lòng đăng nhập lại", success = false });
            }
            if (Description == null)
            {
                Description = "";
            }

            if (LItem.Count > 0)
            {
                for (int i = LItem.Count - 1; i >= 0; i--)
                {
                    if (LItem[i].ItemID == "" || LItem[i].ItemID == null)
                    {
                        LItem.RemoveAt(i);
                        break;
                    }

                }
            }
            //Remove_ListDetailFromGroup
            LocationTo = LocationTo.ToUpper();

            if (GlobalVariables.GHXuatThang)
            {
                if (LocationTo == "XUATTHANG")
                {
                    if (Commons.CheckPermit("xuatthang") == false)
                    {
                        return Json(new { errorMsg = "Bạn chưa được phân quyền sử dụng vị trí XUATTHANG", success = false });
                    }
                }
            }

            try
            {



                if (CheckLocationDest(LocationTo) == false)
                {
                    return Json(new { errorMsg = "Không tồn tại vị trí đích này hoặc đã bị khóa", success = false });

                }

            }
            catch (Exception loi)
            {

                return Json(new { errorMsg = loi.Message, success = false });

            }


            string sWrite = "";

            foreach (ItemL i in LItem)
            {
                if (CheckLocationSource(i.Location) == false)
                {
                    return Json(new { errorMsg = "Không tồn tại vị trí nguồn này hoặc đã bị khóa", success = false });
                }
                if (i.Location == LocationTo)
                {
                    return Json(new { errorMsg = "Vị trí nguồn và đích phải khác nhau ", success = false });
                }
                string VoucherID = GetNewKey();


                Exception exx = null;
                sWrite = "exec SP_InsertGroupM " + G.ToString("0") + ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "',N'" + VoucherID + "';";
                bool bd = Commons.ExecuteNoneQuery(sWrite, ref exx);
                if (bd == false)
                {
                    return Json(new { errorMsg = exx.Message, success = false });
                }



                string ssql = "exec [vanchuyenconhatky] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                ssql += ",N'" + Commons.Fix(VoucherID) + "'";
                ssql += ",N'" + Commons.Fix(Description) + "'";
                ssql += ",'" + Commons.Fix(i.Location) + "'";
                ssql += ",'" + Commons.Fix(LocationTo) + "'";
                ssql += ",'" + Commons.Fix(i.ItemID) + "'";
                ssql += "," + i.Quantity.ToString("0");
                ssql += "," + GlobalVariables.UserID.ToString();
                ssql += ",0;";

                DataTable dt = Commons.GetData(ssql);

                if (Commons.ConvertToInt(dt.Rows[0][0]) == 0)
                {
                    return Json(new { errorMsg = dt.Rows[0][1], success = false });
                }


                sWrite += "exec SP_UpdateVolumeUsed ";
                sWrite += " N'" + Commons.Fix(i.Location) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

                sWrite += "exec SP_UpdateVolumeUsed ";
                sWrite += " N'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

            }

            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }

            return Json(new { errorMsg = ex.Message, success = false });

        }




        [HttpPost]
        public ActionResult get_moveitembygroup()
        {
            int g = Commons.ConvertToInt(Request.QueryString["g"]);
            string sSQL = "exec Get_MoveItemByGroup " + g.ToString("0") + ", N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);


            DataRow r = dt.NewRow();

            dt.Rows.Add(r);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            VoucherID = p["VoucherID"],
                            LocationFrom = p["FromLocation"],
                            LocationTo = p["ToLocation"],
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            UnitID = p["UnitID"],
                            LSX = p["LSX"]
                        };
            return Json(query);

        }

        public ActionResult TraCuuTon()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("tracuuton") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "exec TraCuuTon N'" + Commons.Fix(keyword) + "'";
            sSQL += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            decimal tsl = Convert.ToDecimal(dt.Rows[0][1]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/tracuuton?key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/tracuuton?key=" + keyword + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/tracuuton?key=" + keyword + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);

                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.TSL = tsl.ToString("N0");
            return View();
        }

        [HttpPost]
        public ActionResult ChiTietTon()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;

            string sSQL = "exec ChiTietTon N'" + Commons.Fix(keyword) + "'";
            sSQL += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += "," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            UnitID = p["UnitID"],
                            Location = p["Location"],
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            Waiting = (Commons.ConvertToInt(p["Waiting"]) != 0 ? "<a href='javascript:showwaiting(\"" + p["ItemID"] + "\",\"" + p["Location"].ToString().Replace(" - khóa xuất", "") + "\")'>" + Commons.ConvertToInt(p["Waiting"]).ToString("N0") + "</a>" : Commons.ConvertToInt(p["Waiting"]).ToString("N0")),
                            Remain = Commons.ConvertToInt(p["Remain"]).ToString("N0")


                        };
            return Json(query);
        }


        [HttpPost]
        public ActionResult Remove_ListDetailFromGroup(List<ItemL2> LItem)
        {
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Tài khoản không hợp lệ. Vui lòng đăng nhập lại", success = false });
                }
                if (LItem.Count > 0)
                {
                    for (int i = LItem.Count - 1; i >= 0; i--)
                    {
                        if (LItem[i].ItemID == "" || LItem[i].ItemID == null)
                        {
                            LItem.RemoveAt(i);
                            break;
                        }

                    }
                }

                string sWrite = "";

                try
                {
                    foreach (ItemL2 i in LItem)
                    {
                        if (CheckLocationSource(i.LocationFrom) == false)
                        {
                            return Json(new { errorMsg = "Không tồn tại vị trí nguồn này hoặc đang bị khóa", success = false });

                        }


                        if (CheckMoveVoucherExists(i.VoucherID) == false)
                        {
                            return Json(new { errorMsg = "Không tồn tại phiếu di hàng này", success = false });

                        }

                        if (CheckLocationDest(i.LocationTo) == false)
                        {
                            return Json(new { errorMsg = "Không tồn tại vị trí đích này hoặc đang bị khóa", success = false });

                        }
                    }
                }
                catch (Exception loi)
                {

                    return Json(new { errorMsg = loi.Message, success = false });

                }


                foreach (ItemL2 i in LItem)
                {

                    string ssql = "exec [vanchuyenconhatky] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    ssql += ",N'" + Commons.Fix(i.VoucherID) + "'";
                    ssql += ",N''";
                    ssql += ",'" + Commons.Fix(i.LocationFrom) + "'";
                    ssql += ",'" + Commons.Fix(i.LocationTo) + "'";
                    ssql += ",'" + Commons.Fix(i.ItemID) + "'";
                    ssql += "," + i.Quantity.ToString("0");
                    ssql += "," + GlobalVariables.UserID.ToString();
                    ssql += ",1;";

                    DataTable dt = Commons.GetData(ssql);

                    if (Commons.ConvertToInt(dt.Rows[0][0]) == 0)
                    {
                        return Json(new { errorMsg = dt.Rows[0][1], success = false });
                    }
                }


                return Json(new { msg = "Cập nhật thành công", success = true });


            }
            catch (Exception exx)
            {
                return Json(new { errorMsg = exx.Message, success = false });

            }


        }
        ///kiem ke
        ///
        public ActionResult KK()
        {
            if (GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/Login");
            }

            if (Commons.CheckPermit("kk") == false)
            {
                Response.Redirect("~/admin/NotPermit");
            }

            ViewBag.ms = "";
            ViewBag.locked = false;
            string d = Commons.ConvertToString(Request.QueryString["d"]);
            string Location = Commons.ConvertToString(Request.QueryString["l"]);
            string Note = Commons.ConvertToString(Request.QueryString["note"]);
            if (d != "")
            {
                string[] l = d.Split('.');
                string year = l[0];
                string month = l[1];
                string day = l[2];
                string date = day + "/" + month + "/" + year;
                ViewBag.D = date;
                if (IsLockKK(d, Location, Note))
                {
                    ViewBag.locked = true;
                    ViewBag.ms = "<p style='font-weight:bold;color:red;font-size:16px'>Đang bị khóa</p>";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetKK()
        {
            string d = Commons.ConvertToString(Request.QueryString["d"]);
            string l = Commons.ConvertToString(Request.QueryString["l"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);

            string sSQL = "exec GetCTKK '" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",'" + Commons.Fix(l) + "'";
            sSQL += ",N'" + Commons.Fix(d) + "' ";
            sSQL += ",N'" + Commons.Fix(note) + "' ";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            int Total = 0;
            foreach (DataRow item in dt.Rows)
            {
                Total += Convert.ToInt32(item["Quantity"]);
            }

            DataRow r = dt.NewRow();
            r["ItemID"] = "";
            r["ItemName"] = "<strong>Tổng cộng</strong>";
            r["UnitID"] = "";
            r["Quantity"] = Total;
            dt.Rows.Add(r);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            TT = p["TT"],
                            ItemID = p["ItemID"],
                            ItemName = p["ItemName"],
                            Quantity = (p["ItemName"].ToString() == "<strong>Tổng cộng</strong>" ? "<strong>" + Commons.ConvertToInt(p["Quantity"]).ToString("N0") + "</strong>" : Commons.ConvertToInt(p["Quantity"]).ToString("N0")),
                            UnitID = p["UnitID"]
                        };
            return Json(query);

        }
        public bool CheckKKBarCodeExists(string BarCode, string D, string Location)
        {
            BarCode = BarCode.ToUpper();

            string sSQL = "select ItemID from KKBarCode ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and BarCode='" + Commons.Fix(BarCode) + "'";
            sSQL += " and D='" + Commons.Fix(D) + "'";
            sSQL += " and Location='" + Commons.Fix(Location) + "'";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }
        private void SaveBarCodeKK(string BarCode, string ItemID, string D, string Location)
        {
            BarCode = BarCode.ToUpper();

            string sWrite = "";
            sWrite += "exec SP_InsertKKBarCode ";
            sWrite += " N'" + Commons.Fix(Commons.ConvertToString(D)) + "'";
            sWrite += ",N'" + Commons.Fix(Commons.ConvertToString(BarCode)) + "'";
            sWrite += ",N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
            sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(ItemID)) + "'";
            sWrite += " ,N'" + Commons.Fix(Location) + "'";
            sWrite += ";";

            Commons.ExecuteNoneQuery(sWrite);
        }
        [HttpPost]
        public ActionResult PostKK(string BarCode, string D, string L, string sL)
        {
            BarCode = BarCode.ToUpper();

            string[] lll = BarCode.Split(' ');
            int q = 1;
            BarCode = lll[0];
            if (lll.Length == 2)
            {
                q = Commons.ConvertToInt(lll[1], 1);
            }

            try
            {
                if (sL.Trim() == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập ghi chú", success = false });
                }

                if (IsLockKK(D, L, sL))
                {
                    return Json(new { errorMsg = "Phần này đã bị khóa rồi. không thể thêm xóa sửa.", success = false });
                }

                string sWrite = "";

                if (CheckLocationExists(L) == false)
                {
                    return Json(new { errorMsg = "Vị trí này không tồn tại", success = false });
                }

                if (CheckKKBarCodeExists(BarCode, D, L))
                {
                    return Json(new { errorMsg = "Mã vạch này quét rồi", success = false });
                }

                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }

                if (Global.Commons.CheckPermit("kk") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }

                if (BarCode == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập barcode hoặc mã hàng", success = false });
                }

                Global.GlobalVariables.KKNote = sL;

                List<CC> list = new List<CC>();

                if (BarCode.Length == 27)
                {
                    if ((BarCode.Substring(16, 1) == "H" || BarCode.Substring(14, 1) == "H"))
                    {
                        List<CC> b = GetFromHappyBitis(BarCode);
                        if (b.Count > 0)
                        {
                            SaveBarCodeKK(BarCode, b[0].i, D, L);
                        }

                        list.AddRange(b);
                    }
                    else
                    {
                        CC p = GetFrom27(BarCode);
                        p.q = GetQuantity(BarCode, p.i);
                        if (p.i != "" && p.i != null)
                        {
                            SaveBarCodeKK(BarCode, p.i, D, L);
                        }

                        list.Add(p);
                    }

                }
                else
                if (BarCode.Length == 18)
                {
                    if (BarCode.Substring(BarCode.Length - 1, 1) == "0")
                    {
                        CC r = new CC();
                        r = GetFrom18(BarCode);
                        r.q = q;
                        list.Add(r);
                    }
                    else
                    {
                        list = GetFrom1811(BarCode);
                        if (list.Count == 0)
                        {
                            return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                        }
                        //if (list.Count > 0)
                        //    SaveBarCodeKK(BarCode, list[0].i, D, L);

                    }
                }
                else
                {
                    CC r = new CC();
                    r = GetFromItem(BarCode);
                    r.q = q;
                    list.Add(r);
                }
                if (list.Count == 0)
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }
                string itemname = "";
                sWrite = "";
                foreach (CC item in list)
                {
                    if (item.i == "")
                    {
                        return Json(new { errorMsg = "Tem không hợp lệ", success = false });

                    }
                    if (CheckValidInsertKK(D, item.i, L, sL) == false)
                    {
                        return Json(new { errorMsg = "Đã có user khác quét hàng này rồi", success = false });

                    }
                    sWrite += "exec SP_InsertKK ";
                    sWrite += " N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(L)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(sL)) + "'";
                    sWrite += " ,N'" + Commons.Fix(D) + "'";
                    sWrite += " ,N'" + Commons.Fix(item.i) + "'";
                    sWrite += " ,N'" + Commons.Fix(BarCode) + "'";
                    sWrite += " ,N'" + Commons.Fix(item.lsx) + "'";
                    sWrite += " ," + item.q;
                    sWrite += " ," + GlobalVariables.UserID.ToString("0");
                    sWrite += ";";
                    itemname += item.i + " - SL : " + item.q + " ,";
                }
                itemname = itemname.Trim(',');

                Exception ex = null;
                bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (result == true)
                {
                    return Json(new { msg = "Quét thành công " + itemname, success = true, mustload = true });
                }

                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            catch (Exception mmm)
            {

                return Json(new { errorMsg = mmm.Message, success = false });
            }
        }
        public bool CheckValidInsertKK(string D, string i, string L, string sL)
        {
            string ssql = "";
            ssql += "exec SP_CheckValidInsert ";
            ssql += " N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
            ssql += " ,N'" + Commons.Fix(Commons.ConvertToString(L)) + "'";
            ssql += " ,N'" + Commons.Fix(Commons.ConvertToString(sL)) + "'";
            ssql += " ,N'" + Commons.Fix(D) + "'";
            ssql += " ,N'" + Commons.Fix(i) + "'";
            ssql += " ," + GlobalVariables.UserID.ToString("0");
            ssql += ";";
            DataTable dt = Commons.GetData(ssql);
            int r = Convert.ToInt32(dt.Rows[0][0]);
            return r > 0;
        }
        public ActionResult ViewKKN()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("xkk") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec SP_GetCountKK N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(fromk) + "',N'" + Commons.Fix(tok) + "',N'" + Commons.Fix(note) + "'," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int nSum = Convert.ToInt32(dt.Rows[0][1]);

            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewKKN?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewKKN?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + 1;
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewKKN?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE);

                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }

            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");
            return View();
        }
        public ActionResult ViewKK()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("xkk") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec SP_GetCountKK N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(fromk) + "',N'" + Commons.Fix(tok) + "',N'" + Commons.Fix(note) + "'," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";
            DataTable dt = Commons.GetData(sSQL);

            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int nSum = Convert.ToInt32(dt.Rows[0][1]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewKK?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewKK?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + 1;
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewKK?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE);

                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);

                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");
            return View();
        }
        [HttpPost]
        public ActionResult get_kks()
        {
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetKK N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(fromk) + "',N'" + Commons.Fix(tok) + "',N'" + Commons.Fix(note) + "'," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            VoucherDate = CD(p["D"].ToString()),
                            Odd = (Commons.ConvertToBool(p["Odd"]) ? "Vị trí lẻ" : "Vị trí chẳn"),
                            Location = Commons.ConvertToString(p["Location"]),
                            SubLocation = Commons.ConvertToString(p["SubLocation"]),
                            D = Commons.ConvertToString(p["D"]),
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            UserName = Commons.ConvertToString(p["UserName"]),
                            B = Convert.ToDateTime(p["CreateDate"]).ToString("HH:mm"),
                            E = Convert.ToDateTime(p["LastModifyDate"]).ToString("HH:mm"),

                        };
            return Json(query);
        }

        [HttpPost]
        public ActionResult get_kksn()
        {
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetKK N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(fromk) + "',N'" + Commons.Fix(tok) + "',N'" + Commons.Fix(note) + "'," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";


            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            VoucherDate = "<p>" + CD(Commons.ConvertToString(p["D"])) + "</p><p style='color:green'>" + Commons.ConvertToString(p["UserName"]) + "</p>",
                            Location = Commons.ConvertToString(p["Location"]),
                            D = Commons.ConvertToString(p["D"]),
                            SubLocation = Commons.ConvertToString(p["SubLocation"]),
                            Location1 = "<p>" + Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy") + "</p><p style='color:green'>" + Commons.ConvertToString(p["UserName"]) + "</p>" + "<p>" + Commons.ConvertToString(p["Location"]) + "</p><p style='color:green'><textarea style='width:90%;height:40px' disabled='disabled'>" + Commons.ConvertToString(p["SubLocation"]) + "</textarea></p>",
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0")


                        };
            return Json(query);
        }

        [HttpPost]
        public ActionResult DeleteKK(string D, string L, string SubLocation)
        {
            //if (IsLockKK(D, L, ""))
            //    return Json(new { errorMsg = "Phần này đã bị khóa rồi. không thể thêm xóa sửa.", success = false });

            string sWrite = "exec SP_DeleteKK ";
            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(D) + "' ";
            sWrite += ",N'" + Commons.Fix(L) + "'";
            sWrite += ",N'" + Commons.Fix(SubLocation) + "';";
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Xoá thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult DeleteKKCT(string D, string L, string ItemID, string SubLocation)
        {
            //if (IsLockKK(D, L, SubLocation))
            //    return Json(new { errorMsg = "Phần này đã bị khóa rồi. không thể thêm xóa sửa.", success = false });

            string sWrite = "exec SP_DeleteKKCT ";
            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(D) + "' ";
            sWrite += ",N'" + Commons.Fix(L) + "'";
            sWrite += ",N'" + Commons.Fix(SubLocation) + "'";
            sWrite += ",N'" + Commons.Fix(ItemID) + "';";

            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Xoá thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult DeCreaseKK(int Q, string D, string L, string BarCode, string SubLocation)
        {
            BarCode = BarCode.ToUpper();

            if (Q < 1)
            {
                return Json(new { errorMsg = "Số lượng giảm phải >0 .", success = false });

            }
            if (IsLockKK(D, L, SubLocation))
            {
                return Json(new { errorMsg = "Phần này đã bị khóa rồi. không thể thêm xóa sửa.", success = false });
            }

            string sWrite = "";
            List<CC> list = new List<CC>();

            if (BarCode.Length == 27)
            {
                if ((BarCode.Substring(16, 1) == "H" || BarCode.Substring(14, 1) == "H"))
                {
                    List<CC> b = GetFromHappyBitis(BarCode);

                    list.AddRange(b);
                }
                else
                {
                    CC p = GetFrom27(BarCode);
                    p.q = GetQuantity(BarCode, p.i);
                    if (p.i != "" && p.i != null)
                    {
                        list.Add(p);
                    }
                }

            }
            else
            if (BarCode.Length == 18)
            {
                if (BarCode.Substring(BarCode.Length - 1, 1) == "0")
                {
                    CC r = new CC();
                    r = GetFrom18(BarCode);
                    r.q = 1;
                    list.Add(r);
                }
                else
                {
                    list = GetFrom1811(BarCode);
                    if (list.Count == 0)
                    {
                        return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                    }
                }
            }
            else
            {
                CC r = new CC();
                r = GetFromItem(BarCode);
                r.q = 1;
                list.Add(r);
            }
            if (list.Count == 0)
            {
                return Json(new { errorMsg = "Tem không hợp lệ", success = false });
            }

            bool did = false;
            foreach (CC item in list)
            {
                string ItemID = item.i;
                if (CheckValidInsertKK(D, ItemID, L, SubLocation) == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền thay đổi dữ liệu của user này", success = false });

                }
                string ssql = "select ItemID,Quantity from KK where ";
                ssql += " DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                ssql += " and Location='" + Commons.Fix(L) + "'";
                ssql += " and SubLocation='" + Commons.Fix(SubLocation) + "'";
                ssql += " and ItemID='" + Commons.Fix(ItemID) + "'";
                ssql += " and BarCode='" + Commons.Fix(BarCode) + "'";
                DataTable dt = Commons.GetData(ssql);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    int Quantity = Commons.ConvertToInt(r["Quantity"]);
                    if (Quantity < Q)
                    {
                        return Json(new { errorMsg = "Mã hàng " + ItemID + " ứng với barcode " + BarCode + " trong phiếu này chỉ còn " + Quantity + " không thể giảm " + Q, success = false });

                    }
                    sWrite += "Update KK set Quantity = Quantity - " + Q.ToString("0");
                    sWrite += " where  D='" + Commons.Fix(D) + "' ";
                    sWrite += " and Location='" + Commons.Fix(L) + "'";
                    sWrite += " and SubLocation='" + Commons.Fix(SubLocation) + "'";
                    sWrite += " and ItemID='" + Commons.Fix(ItemID) + "'";
                    sWrite += " and BarCode='" + Commons.Fix(BarCode) + "'";
                    sWrite += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "';";
                    if (Q == Quantity)
                    {
                        sWrite += "exec SP_DeleteKKCTHasBarCode ";
                        sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                        sWrite += ",N'" + Commons.Fix(D) + "' ";
                        sWrite += ",N'" + Commons.Fix(L) + "'";
                        sWrite += ",N'" + Commons.Fix(SubLocation) + "'";
                        sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                        sWrite += ",N'" + Commons.Fix(BarCode) + "';";

                    }

                    did = true;
                }
                //else
                //{
                //    return Json(new { errorMsg = "Không tìm thấy mã hàng " + ItemID + " ứng với barcode " + BarCode + " trong phiếu này", success = false });

                //}


            }

            if (did == false)
            {
                return Json(new { errorMsg = "Không tìm thấy mã hàng ứng với barcode " + BarCode + " trong phiếu này", success = false });

            }


            Exception ex = null;
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult LockKK(string D, string L, string SubLocation)
        {

            string sWrite = "update KK set Locked=1 where  D='" + Commons.Fix(D) + "' ";
            sWrite += " and Location='" + Commons.Fix(L) + "'";
            sWrite += " and SubLocation='" + Commons.Fix(SubLocation) + "'";
            sWrite += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";

            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Khóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult UnLockKK(string D, string L, string SubLocation)
        {

            string sWrite = "update KK set Locked=0 where  D='" + Commons.Fix(D) + "' ";
            sWrite += " and Location='" + Commons.Fix(L) + "'";
            sWrite += " and SubLocation='" + Commons.Fix(SubLocation) + "'";
            sWrite += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";

            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Khóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult UpdateKKNote(string ItemID, string D, string L, string SubLocation, string NewSubLocation)
        {
            if (CheckLocationExists(L) == false)
            {
                return Json(new { errorMsg = "Vị trí này không tồn tại", success = false });
            }

            string ssql = "exec SP_ExistsKKNote N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            ssql += ",'" + Commons.Fix(D) + "' ";
            ssql += ",N'" + Commons.Fix(L) + "'";
            ssql += ",N'" + Commons.Fix(SubLocation) + "'";
            ssql += ",N'" + Commons.Fix(NewSubLocation) + "'";
            ssql += ",N'" + Commons.Fix(ItemID) + "'";
            ssql += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(ssql);
            int result = Convert.ToInt32(dt.Rows[0][0]);
            if (result == 1)
            {
                return Json(new { errorMsg = "Đã tồn tại dữ liệu trùng từ ghi chú mới rồi", success = false });
            }

            string sWrite = "exec SP_UpdateKKNote N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",'" + Commons.Fix(D) + "' ";
            sWrite += ",N'" + Commons.Fix(L) + "'";
            sWrite += ",N'" + Commons.Fix(SubLocation) + "'";
            sWrite += ",N'" + Commons.Fix(NewSubLocation) + "'";
            sWrite += ",N'" + Commons.Fix(ItemID) + "'";
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", note = NewSubLocation.ToUpper(), success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult UpdateKKNoteAll(string D, string L, string SubLocation, string NewSubLocation)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Commons.CheckPermit("kk") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (CheckLocationExists(L) == false)
            {
                return Json(new { errorMsg = "Vị trí này không tồn tại", success = false });
            }
            string sSQL = "exec GetCTKK '" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",'" + Commons.Fix(L) + "'";
            sSQL += ",N'" + Commons.Fix(D) + "' ";
            sSQL += ",N'" + Commons.Fix(SubLocation) + "' ";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            string sWrite = "";
            foreach (DataRow item in dt.Rows)
            {
                string ItemID = item["ItemID"].ToString();
                string ssql = "exec SP_ExistsKKNote N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                ssql += ",'" + Commons.Fix(D) + "' ";
                ssql += ",N'" + Commons.Fix(L) + "'";
                ssql += ",N'" + Commons.Fix(SubLocation) + "'";
                ssql += ",N'" + Commons.Fix(NewSubLocation) + "'";
                ssql += ",N'" + Commons.Fix(ItemID) + "'";
                ssql += "," + GlobalVariables.UserID.ToString("0");
                DataTable db = Commons.GetData(ssql);
                int result = Convert.ToInt32(db.Rows[0][0]);
                if (result == 1)
                {
                    return Json(new { errorMsg = "Đã tồn tại dữ liệu trùng từ ghi chú mới rồi", success = false });
                }

                sWrite += "exec SP_UpdateKKNote N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",'" + Commons.Fix(D) + "' ";
                sWrite += ",N'" + Commons.Fix(L) + "'";
                sWrite += ",N'" + Commons.Fix(SubLocation) + "'";
                sWrite += ",N'" + Commons.Fix(NewSubLocation) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                sWrite += ";";

            }
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", note = NewSubLocation.ToUpper(), success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult UpdateKKLocationAll(string D, string L, string NewLocation, string SubLocation)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Commons.CheckPermit("kk") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (CheckLocationExists(L) == false)
            {
                return Json(new { errorMsg = "Vị trí này không tồn tại", success = false });
            }
            if (CheckLocationExists(NewLocation) == false)
            {
                return Json(new { errorMsg = "Vị trí mới này không tồn tại", success = false });
            }
            string sSQL = "exec GetCTKK '" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",'" + Commons.Fix(L) + "'";
            sSQL += ",N'" + Commons.Fix(D) + "' ";
            sSQL += ",N'" + Commons.Fix(SubLocation) + "' ";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            string sWrite = "";
            foreach (DataRow item in dt.Rows)
            {
                string ItemID = item["ItemID"].ToString();
                string ssql = "exec SP_ExistsKKNote N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                ssql += ",'" + Commons.Fix(D) + "' ";
                ssql += ",N'" + Commons.Fix(NewLocation) + "'";
                ssql += ",N'" + Commons.Fix(SubLocation) + "'";
                ssql += ",N'" + Commons.Fix(SubLocation) + "'";
                ssql += ",N'" + Commons.Fix(ItemID) + "'";
                ssql += "," + GlobalVariables.UserID.ToString("0");
                DataTable db = Commons.GetData(ssql);
                int result = Convert.ToInt32(db.Rows[0][0]);
                if (result == 1)
                {
                    return Json(new { errorMsg = "Đã tồn tại dữ liệu trùng từ ghi chú mới rồi", success = false });
                }

                sWrite += "exec SP_UpdateKKLocation N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",'" + Commons.Fix(D) + "' ";
                sWrite += ",N'" + Commons.Fix(L) + "'";
                sWrite += ",N'" + Commons.Fix(NewLocation) + "'";
                sWrite += ",N'" + Commons.Fix(SubLocation) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                sWrite += ";";

            }
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", newlocation = NewLocation.ToUpper(), success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public bool IsLockKK(string Date, string Location, string SubLocation = "")
        {

            string sWrite = "select count(ItemID) c from KK   where  Locked=1 and D='" + Commons.Fix(Date) + "' ";
            sWrite += " and Location='" + Commons.Fix(Location) + "'";
            if (SubLocation != "")
            {
                sWrite += " and SubLocation='" + Commons.Fix(SubLocation) + "'";
            }

            sWrite += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sWrite);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            int c = Convert.ToInt32(dt.Rows[0][0]);
            return c > 0;
        }
        public ActionResult ExportKK()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_ExportKK N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",N'" + Commons.Fix(from) + "'";
            sSQL += ",N'" + Commons.Fix(to) + "'";
            sSQL += ",N'" + Commons.Fix(fromk) + "'";
            sSQL += ",N'" + Commons.Fix(tok) + "'";
            sSQL += ",N'" + Commons.Fix(note) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";
            DataTable dt = Commons.GetData(sSQL);

            Export d = new Export();
            d.ToExcel(Response, dt, "kiemke");
            return View();
        }
        public ActionResult ExportKKTH()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_ExportKKTH N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",N'" + Commons.Fix(from) + "'";
            sSQL += ",N'" + Commons.Fix(to) + "'";
            sSQL += ",N'" + Commons.Fix(fromk) + "'";
            sSQL += ",N'" + Commons.Fix(tok) + "'";
            sSQL += ",N'" + Commons.Fix(note) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";
            DataTable dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                item[0] = CD(item[0].ToString());
            }
            Export d = new Export();
            d.ToExcel(Response, dt, "kiem ke chi tiet");
            return View();
        }
        public ActionResult PrintKKTH()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_PrintKKTH N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",N'" + Commons.Fix(from) + "'";
            sSQL += ",N'" + Commons.Fix(to) + "'";
            sSQL += ",N'" + Commons.Fix(fromk) + "'";
            sSQL += ",N'" + Commons.Fix(tok) + "'";
            sSQL += ",N'" + Commons.Fix(note) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int Quantity = 0;
            foreach (DataRow item in dt.Rows)
            {
                item["CreateDate"] = CD(item["CreateDate"].ToString());
                Quantity += Convert.ToInt32(item["Quantity"]);
            }
            ViewBag.data = dt.Rows;
            ViewBag.Quantity = Quantity.ToString("N0");

            return View();
        }
        public ActionResult ExportKKText()
        {

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_ExportKKText N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",N'" + Commons.Fix(from) + "'";
            sSQL += ",N'" + Commons.Fix(to) + "'";
            sSQL += ",N'" + Commons.Fix(fromk) + "'";
            sSQL += ",N'" + Commons.Fix(tok) + "'";
            sSQL += ",N'" + Commons.Fix(note) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";

            DataTable dt = Commons.GetData(sSQL);
            string content = "";
            foreach (DataRow item in dt.Rows)
            {
                for (int i = 0; i < Convert.ToInt32(item["Quantity"]); i++)
                {
                    content += item["BarCode"].ToString() + Environment.NewLine;
                }
            }


            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=kk.txt");
            Response.ContentType = "application/text";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(content);
            Response.End();

            return View();
        }
        public ActionResult ChuyenHangNhanh()
        {

            return View();
        }
        public ActionResult ExportXK()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_ExportXK N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",N'" + Commons.Fix(from) + "'";
            sSQL += ",N'" + Commons.Fix(to) + "'";

            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                item["OutBound"] = Commons.ConvertToString(item["OutBound"]) + "," + Commons.ConvertToString(item["OutBound2"]);
            }
            dt.Columns.Remove("OutBound2");
            Export d = new Export();
            d.ToExcel(Response, dt, "xuatkho");

            return View();
        }
        public ActionResult ExportXKCT()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_ExportXKCT N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",N'" + Commons.Fix(from) + "'";
            sSQL += ",N'" + Commons.Fix(to) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            foreach (DataRow item in dt.Rows)
            {
                item["OutBound"] = Commons.ConvertToString(item["OutBound"]) + "," + Commons.ConvertToString(item["OutBound2"]);
            }
            dt.Columns.Remove("OutBound2");
            Export d = new Export();
            d.ToExcel(Response, dt, "xuatkho");

            return View();
        }

        public ActionResult ChooseLocation()
        {
            return View();
        }
        public bool ChuyenRa(string LocationFrom)
        {
            string VoucherID = GetNewKey();
            string LocationTo = "XUATTHANG";
            string sSQL = "select ItemID,Quantity from ViewPallet ";
            sSQL += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += " and Location='" + Commons.Fix(LocationFrom) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string sWrite = "exec SP_InsertMoveItem ";
            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
            sWrite += ",N'Di hàng chỉnh kho bằng cách di ra trước'";
            sWrite += ",N'" + Commons.Fix(LocationFrom) + "'";
            sWrite += ",N'" + Commons.Fix(LocationTo) + "'";
            sWrite += "," + GlobalVariables.UserID.ToString();
            sWrite += ";";

            string LSX = "";
            foreach (DataRow i in dt.Rows)
            {
                int Quantity = Convert.ToInt32(i["Quantity"]);
                string ItemID = i["ItemID"].ToString();


                sWrite += "exec SP_InsertMoveItemDetail ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += ",N'" + LSX + "';";//LSX

                sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",1";
                sWrite += ",'" + Commons.Fix(ItemID) + "'";
                sWrite += ",'" + LSX + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += ";";
                sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",'" + Commons.Fix(LocationFrom) + "'";
                sWrite += ",-1";
                sWrite += ",'" + Commons.Fix(ItemID) + "'";
                sWrite += ",'" + Commons.Fix(LSX) + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += ";"; //ghi nhat ky
                sWrite += "exec SP_TranItemLog ";
                sWrite += " N'" + Commons.Fix(LocationFrom) + "'";
                sWrite += ",N'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += "," + GlobalVariables.UserID.ToString("0") + ";";
            }
            bool b = Commons.ExecuteNoneQuery(sWrite);
            return b;
        }
        public bool ChuyenVao(string LocationTo, List<CC> ItemList)
        {
            string VoucherID = GetNewKey();
            string LocationFrom = "TUNGOAI";

            string sWrite = "exec SP_InsertMoveItem ";
            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
            sWrite += ",N'Di hàng chỉnh kho bằng cách di vào vị trí từ bên ngoài'";
            sWrite += ",N'" + Commons.Fix(LocationFrom) + "'";
            sWrite += ",N'" + Commons.Fix(LocationTo) + "'";
            sWrite += "," + GlobalVariables.UserID.ToString();
            sWrite += ";";

            foreach (CC i in ItemList)
            {
                int Quantity = i.q;
                string ItemID = i.i;


                sWrite += "exec SP_InsertMoveItemDetail ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(VoucherID) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += ",N'" + i.lsx + "';";//LSX

                sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",1";
                sWrite += ",'" + Commons.Fix(ItemID) + "'";
                sWrite += ",'" + i.lsx + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += ";";
                sWrite += "exec [SP_InsertLocationDetail] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",'" + Commons.Fix(LocationFrom) + "'";
                sWrite += ",-1";
                sWrite += ",'" + Commons.Fix(ItemID) + "'";
                sWrite += ",'" + Commons.Fix(i.lsx) + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += ";"; //ghi nhat ky
                sWrite += "exec SP_TranItemLog ";
                sWrite += " N'" + Commons.Fix(LocationFrom) + "'";
                sWrite += ",N'" + Commons.Fix(LocationTo) + "'";
                sWrite += ",N'" + Commons.Fix(ItemID) + "'";
                sWrite += "," + Quantity.ToString("0");
                sWrite += "," + GlobalVariables.UserID.ToString("0") + ";";
            }
            bool b = Commons.ExecuteNoneQuery(sWrite);
            return b;
        }
        public ActionResult SuaKetQua()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "select OB from WD where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' group by OB order by OB ";
            DataTable dt = Commons.GetData(sSQL);

            ViewBag.ob = dt.Rows;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateChiaHang(string VoucherID, string OB, string ItemID, int Quantity)
        {
            if (VoucherID == "")
            {
                return Json(new { errorMsg = "Số phiếu không được rỗng", success = false });

            }
            if (OB == "")
            {
                return Json(new { errorMsg = "Outbound không được rỗng", success = false });

            }
            if (ItemID == "")
            {
                return Json(new { errorMsg = "Mã hàng không được rỗng", success = false });

            }
            if (Quantity < 0)
            {
                return Json(new { errorMsg = "Số lượng không hợp lệ", success = false });

            }
            string sSQL = "select count(ItemID) from XH ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID = N'" + Commons.Fix(VoucherID) + "' and Confirmed=1 ";
            DataTable dt = Commons.GetData(sSQL);
            if (Commons.ConvertToInt(dt.Rows[0][0]) > 0)
            {
                return Json(new { errorMsg = "Phiếu này đã xác nhận rồi", success = false });

            }
            sSQL = "select sum(Quantity),ItemID from WD ";
            sSQL += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += " and VoucherID = N'" + Commons.Fix(VoucherID) + "' ";
            sSQL += " and ItemID=N'" + Commons.Fix(ItemID) + "'";
            sSQL += " and OB=N'" + Commons.Fix(OB) + "'";
            sSQL += " group by ItemID ";
            dt = Commons.GetData(sSQL);
            if (dt.Rows.Count > 0)
            {
                if (Commons.ConvertToInt(dt.Rows[0][0]) < Quantity)
                {
                    return Json(new { errorMsg = "Số lượng nhận không được lớn hơn số lượng yêu cầu", success = false });

                }
            }
            else
            {
                return Json(new { errorMsg = "Dữ liệu này không tồn tại", success = false });
            }

            string sWrite = "Update WD set ReceiveQuantity=" + Quantity.ToString("0");
            sWrite += " where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += " and VoucherID=N'" + Commons.Fix(VoucherID) + "'";
            sWrite += " and ItemID=N'" + Commons.Fix(ItemID) + "'";
            sWrite += " and OB=N'" + Commons.Fix(OB) + "'";
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult UpdateFromKK()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("caoso") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }


            string sWrite = "exec SP_UpdateFromKK N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + GlobalVariables.UserID.ToString("0");
            sWrite += ",N'" + Commons.Fix(fromk) + "'";
            sWrite += ",N'" + Commons.Fix(tok) + "'";
            sWrite += ",N'" + Commons.Fix(note) + "'";
            sWrite += ",N'" + Commons.Fix(key) + "'";
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        [HttpPost]
        public ActionResult ClearAll()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("clearsl") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string sWrite = "exec SP_ClearLocation N'" + Commons.Fix(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString("0");
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == true)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public ActionResult AddMP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add_P(string text)
        {
            if (Commons.CheckPermit("ql") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            text = text.Trim('\n');
            text = text.Trim();
            if (text == "")
            {
                return Json(new { errorMsg = "Không có dữ liệu", success = false });
            }

            DataTable dt = new DataTable();
            int Q = 0;
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("UnitID");
            dt.Columns.Add("Quantity", Q.GetType());
            dt.Columns.Add("Location");
            dt.Columns.Add("Note");

            try
            {
                string[] lines = text.Split('\n');
                string sWrite = "";
                string OldLocation = "", OldNote = "";
                string PalletID = "";
                foreach (string item in lines)
                {

                    string[] data = item.Split('\t');
                    if (data.Length < 4)
                    {
                        return Json(new { errorMsg = "Cấu trúc không hợp lệ", success = false });
                    }

                    string ItemID, Quantity, Location, Note;
                    ItemID = data[0].Trim();
                    Quantity = data[1].Trim();
                    Location = data[2].Trim();
                    Note = data[3].Trim();
                    if (CheckLocationDest(Location) == false)
                    {
                        return Json(new { errorMsg = "Vị trí " + Location + " không tồn tại hoặc bị khóa nhập", success = false });

                    }
                    if (Commons.ConvertToInt(Quantity) < 1)
                    {
                        return Json(new { errorMsg = "Dữ không hợp lệ. Số lượng phải là số nguyên >0", success = false });
                    }

                    CC result = GetFromItem(ItemID);
                    if (result.i == "")
                    {
                        return Json(new { errorMsg = "Mã hàng " + ItemID + " không tồn tại", success = false });

                    }
                    DataRow r = dt.NewRow();
                    r["ItemID"] = result.i;
                    r["ItemName"] = result.d;
                    r["UnitID"] = result.u;
                    r["Location"] = Location;
                    r["Note"] = Note;
                    r["Quantity"] = Quantity;
                    dt.Rows.Add(r);
                }
                if (dt.Rows.Count == 0)
                {
                    return Json(new { errorMsg = "Không có dữ liệu", success = false });
                }

                foreach (DataRow item in dt.Rows)
                {


                    string ItemID, Location, ItemName, UnitID, Note;
                    int Quantity;

                    ItemID = item["ItemID"].ToString();
                    ItemName = item["ItemName"].ToString();
                    UnitID = item["UnitID"].ToString();
                    Quantity = Convert.ToInt32(item["Quantity"]);
                    Location = item["Location"].ToString();
                    Note = item["Note"].ToString();

                    if (OldLocation != Location || Note != OldNote)
                    {
                        OldLocation = Location;
                        OldNote = Note;

                        PalletID = OnlyNewPallet();
                    }
                    sWrite += "exec [SP_InsertPalletDetail] ";
                    sWrite += " N'" + Commons.Fix(Commons.ConvertToString(PalletID)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
                    sWrite += " ,N'" + Commons.Fix(ItemID) + "'";
                    sWrite += " ,N'" + Commons.Fix(ItemName) + "'";
                    sWrite += " ,N'" + Commons.Fix(UnitID) + "'";
                    sWrite += " ," + Commons.ConvertToInt(Quantity);
                    sWrite += " ,N''";//outbound
                    sWrite += " ,N''";//lsx

                    sWrite += ";\n";
                    sWrite += "update Pallets set Active=1,Finish=0,OutBound='',Location='" + Commons.Fix(Location) + "',Description=N'" + Fix(Note) + "',isOff=0 ";
                    sWrite += " where PalletID= '" + Commons.Fix(PalletID) + "' ";
                    sWrite += " and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "'; ";

                    sWrite += "\n";
                    AddPalletLog(PalletID, "[" + GlobalVariables.UserName + "] thêm  " + ItemID + " sl:" + Quantity.ToString("0"));
                }

                Exception ex = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                //System.IO.File.WriteAllText("d:\\aaa.sql", sWrite);
                if (b)
                {
                    return Json(new { msg = "Cập nhật thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });

                }

            }
            catch (Exception ex1)
            {
                return Json(new { errorMsg = ex1.Message, success = false });


            }

        }

        public ActionResult ViewKKTH()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("xkkth") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec SP_GetCountKKTH N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(fromk) + "',N'" + Commons.Fix(tok) + "',N'" + Commons.Fix(note) + "'," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int nSum = Convert.ToInt32(dt.Rows[0][1]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewKKTH?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewKKTH?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + 1;
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewKKTH?key=" + key + "&note=" + note + "&fromk=" + fromk + "&tok=" + tok + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE);

                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");
            return View();
        }
        [HttpPost]
        public ActionResult get_kkTH()
        {
            string fromk = Commons.ConvertToString(Request.QueryString["fromk"]);
            string tok = Commons.ConvertToString(Request.QueryString["tok"]);
            string note = Commons.ConvertToString(Request.QueryString["note"]);
            string key = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetKKTH N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(fromk) + "',N'" + Commons.Fix(tok) + "',N'" + Commons.Fix(note) + "'," + GlobalVariables.UserID.ToString("0");
            sSQL += ",N'" + Commons.Fix(key) + "'";

            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Position = Commons.ConvertToInt(p["Position"]).ToString("N0"),
                            CreateDate = CD(Commons.ConvertToString(p["CreateDate"])),
                            Location = Commons.ConvertToString(p["Location"]),
                            SubLocation = Commons.ConvertToString(p["SubLocation"]),
                            ItemID = Commons.ConvertToString(p["ItemID"]),
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            FullName = Commons.ConvertToString(p["FullName"])
                        };
            return Json(query);
        }
        public string CD(string datevalue)//convert date
        {
            string[] l = datevalue.Split('.');
            string v = l[2] + "/" + l[1] + "/" + l[0];
            return v;

        }
        public ActionResult ViewBackups()
        {
            if (GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/NotPermit");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Get_Backup()
        {
            decimal t = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("File");
            dt.Columns.Add("URL");
            dt.Columns.Add("Size", t.GetType());
            dt.Columns.Add("CreateDate", DateTime.Now.GetType());
            string tm = "backup";

            string[] file = System.IO.Directory.GetFiles(Server.MapPath("~/" + tm), "*.bak");
            foreach (string item in file)
            {
                DataRow r = dt.NewRow();
                string f = item.Substring(item.LastIndexOf("\\") + 1);
                r[0] = f;
                r[1] = "/" + tm + "/" + f;
                System.IO.FileInfo info = new System.IO.FileInfo(item);
                r[2] = Math.Round(info.Length / 1048576.00);
                r[3] = info.CreationTime;

                dt.Rows.Add(r);
            }
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ID = p["File"],
                            File = Commons.ConvertToString(p["File"]),
                            URL = "<a target='_blank' href='" + Commons.ConvertToString(p["URL"]) + "'>" + Commons.ConvertToString(p["URL"]) + "</a>",
                            CreateDate = Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy"),
                            Length = p["Size"] + " MB"
                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult Backup()
        {

            //DateTime d = DateTime.Now;
            //string sTime = DateTime.Now.ToString("yyyy-MM-dd") + "-" + d.Hour.ToString("00") + d.Minute.ToString("00") + d.Second.ToString("00");
            //string tm = "backup";
            //string[] list = { "WWW", "CNMB", "WUser" };
            //string sWrite = "";
            //foreach (string database in list)
            //{
            //    string f = Server.MapPath("~/" + tm + "/" + sTime + '_' + database + ".bak");
            //    sWrite += "backup database " + database + " to disk='" + f + "';";

            //}
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery("exec SP_Backup ", ref ex);
            if (b)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

        }
        [HttpPost]
        public ActionResult DeleteBackup(string File)
        {

            string tm = "backup";

            string f = Server.MapPath("~/" + tm + "/" + File);

            try
            {
                System.IO.File.Delete(f);
                return Json(new { msg = "Xoá thành công", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message + f, success = false });

            }



        }
        public ActionResult ImportKK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportKK(string text)
        {
            if (Global.Commons.CheckPermit("ql") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            text = text.Trim('\n');
            text = text.Trim();
            if (text == "")
            {
                return Json(new { errorMsg = "Không có dữ liệu", success = false });
            }
            string d = DateTime.Now.ToString("yyyy.MM.dd");


            try
            {
                string[] lines = text.Split('\n');
                string sWrite = "";
                int i = 0;
                foreach (string item in lines)
                {

                    string[] data = item.Split('\t');
                    if (data.Length < 4)
                    {
                        return Json(new { errorMsg = "Cấu trúc không hợp lệ", success = false });
                    }

                    string ItemID, Quantity, Location, Note;
                    ItemID = data[2].Trim();
                    Quantity = data[3].Trim();
                    Location = data[0].Trim();
                    Note = data[1].Trim();

                    if (CheckLocationDest(Location) == false)
                    {
                        return Json(new { errorMsg = "Vị trí " + Location + " không tồn tại hoặc bị khóa nhập", success = false });

                    }
                    if (Commons.ConvertToInt(Quantity) < 1)
                    {
                        return Json(new { errorMsg = "Dữ không hợp lệ. Số lượng phải là số nguyên >0", success = false });
                    }

                    CC result = GetFromItem(ItemID);
                    if (result.i == "")
                    {
                        return Json(new { errorMsg = "Mã hàng " + ItemID + " không tồn tại", success = false });

                    }

                    sWrite += "exec SP_InsertKK ";
                    sWrite += " N'" + Commons.Fix(Commons.ConvertToString(GlobalVariables.DivisionID)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(Location)) + "'";
                    sWrite += " ,N'" + Commons.Fix(Commons.ConvertToString(Note)) + "'";
                    sWrite += " ,N'" + d + "'";
                    sWrite += " ,N'" + Commons.Fix(ItemID.ToUpper()) + "'";
                    sWrite += " ,N'" + Commons.Fix(ItemID.ToUpper()) + "'";
                    sWrite += " ,N''";
                    sWrite += " ," + Quantity;
                    sWrite += " ," + GlobalVariables.UserID.ToString("0");
                    sWrite += ";";
                    i++;
                    if (i % 500 == 0)
                    {
                        Exception ex1 = null;
                        bool b1 = Commons.ExecuteNoneQuery(sWrite, ref ex1);
                        if (b1 == false)
                        {
                            return Json(new { errorMsg = ex1.Message, success = false });
                        }

                        sWrite = "";
                    }

                }
                if (sWrite != "")
                {
                    Exception ex = null;
                    bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    if (b)
                    {
                        return Json(new { msg = "Cập nhật thành công", success = true });
                    }
                    else
                    {
                        return Json(new { errorMsg = ex.Message, success = false });

                    }

                }
                return Json(new { msg = "Cập nhật thành công", success = true });

            }
            catch (Exception ex1)
            {
                return Json(new { errorMsg = ex1.Message, success = false });


            }

        }
        public ActionResult ReLoadDH(string VoucherID)
        {
            string ssql = "select distinct OB from WD where VoucherID='" + Commons.Fix(VoucherID) + "' ";
            ssql += " and DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable db = Commons.GetData(ssql);
            if (db.Rows.Count == 0)
            {
                return Json(new { errorMsg = "Phiếu lấy hàng này không tồn tại hoặc đã bị xoá rồi", success = false });
            }
            ssql = "select count(ItemID) from XH ";
            ssql += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            ssql += " and VoucherID = N'" + Commons.Fix(VoucherID) + "' and Confirmed=1 ";
            DataTable dt = Commons.GetData(ssql);
            if (Commons.ConvertToInt(dt.Rows[0][0]) > 0)
            {
                return Json(new { errorMsg = "Phiếu này đã xác nhận rồi", success = false });

            }

            System.Collections.ArrayList OutBound = new ArrayList();
            string listtt = "";
            foreach (DataRow item in db.Rows)
            {
                listtt += ",'" + Commons.Fix(item[0].ToString()) + "'";
                OutBound.Add(item[0].ToString());
            }
            listtt = listtt.Trim(',');
            GetAndPostController gp = new GetAndPostController();
            Dau8[] ds = gp.GetOutBound(OutBound);
            if (ds.Length == 0)
            {
                return Json(new { errorMsg = "Không lấy được dữ liệu từ SAP.", success = false });
            }
            string sWrite = "delete B where OB in(" + listtt + ") and DivisionID='" + Commons.Fix(GlobalVariables.DivisionID) + "' ;";
            foreach (Dau8 item in ds)
            {
                sWrite += "exec [SP_insertb] ";
                sWrite += " N'" + Commons.Fix(item.VBELN) + "'";
                sWrite += ",N'" + Commons.Fix(item.MATNR) + "'";
                sWrite += "," + Commons.ConvertToInt(item.LFIMG).ToString("0");
                sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "';";

            }
            sWrite += "exec ReLoadDH N'" + Commons.Fix(VoucherID) + "',N'" + Commons.Fix(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString("0") + " ; ";

            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public ActionResult ViewReLoadDH()
        {
            if (Global.GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/notpermit");
            }
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_ExportHC N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'";
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Export();
            d.ExportExcel(Response, "nhatkycapnhat", dt);
            return View();
        }
        public ActionResult ViewHC()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.GlobalVariables.IsAdmin == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec SP_GetCountHC N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/viewhc?&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/viewhc?from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/viewhc?from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();

        }
        [HttpPost]
        public ActionResult GetHC()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetHC N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0");

            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            CreateDate = Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy HH:mm"),
                            FromLocation = Commons.ConvertToString(p["FromLocation"]),
                            ToLocation = Commons.ConvertToString(p["ToLocation"]),
                            UserName = Commons.ConvertToString(p["UserName"]),
                            Description = Commons.ConvertToString(p["Description"])
                        };
            return Json(query);
        }
        public bool GetFixBug(string VoucherID, string OB)
        {
            string Message = "";
            string sSQL = "select ConnectionString from Divisions where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'  ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {
                Message = "Chưa thiết lập kết nối với scanner";
                return false;
            }

            string ConnectionString = Commons.ConvertToString(dt.Rows[0][0]);
            if (ConnectionString == "")
            {
                Message = "Chưa thiết lập kết nối với scanner";

                return false;
            }

            try
            {
                sSQL = "select  [Mahh],sum(  [Luong] ) Luong from OutBound where OutBound='" + Commons.Fix(OB) + "' ";
                sSQL += " group by  [Mahh] ";
                dt = Commons.GetDataFromOtherDataBase(sSQL, ConnectionString);
                if (dt.Rows.Count == 0)
                {
                    Message = "Chưa scanner dữ liệu";

                    return false;
                }

                string sWrite = "";
                sWrite += " update WD set ReceiveQuantity= 0";
                sWrite += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += " and OB = N'" + Commons.Fix(OB) + "' ";
                sWrite += " and VoucherID=N'" + Commons.Fix(VoucherID) + "'; ";


                foreach (DataRow item in dt.Rows)
                {
                    int Quantity = Commons.ConvertToInt(item[1]);
                    string ItemID = Commons.ConvertToString(item[0]);
                    sWrite += " update WD set ReceiveQuantity= " + Quantity.ToString("0");
                    sWrite += " where DivisionID = N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += " and ItemID ='" + Commons.Fix(ItemID) + "'  and OB = N'" + Commons.Fix(OB) + "' ";
                    sWrite += " and VoucherID=N'" + Commons.Fix(VoucherID) + "'; ";

                }

                Exception xxx = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref xxx);
                if (b == true)
                {
                    return true;
                }
                else

                {
                    Message = xxx.Message;
                    return false;
                }
            }
            catch (Exception)
            {
                Message = "Không kết nối được với server scanner vui lòng liên hệ quản trị server";
                return false;

            }
        }
        public ActionResult FixBug()
        {
            //LoadInfo();
            //string sSQL = "exec dau8 ";
            //DataTable dt = Commons.GetData(sSQL);
            //foreach (DataRow  item in dt.Rows)
            //{
            //    GetFixBug(item[0].ToString(), item[1].ToString());
            //}
            return View();
        }
        public ActionResult OBBarCode()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec [SP_PrintOBBarcode] N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;

            return View();
        }
        public ActionResult OBBarCodeForOut()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec [SP_PrintOBBarcodeForOut] N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;

            return View();
        }
        public ActionResult CheckOB(string OutBound)
        {
            try
            {
                if (OutBound.Trim() == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập đầu 8", success = false });

                }
                OutBound = OutBound.Replace("'", "");

                OutBound = OutBound.Replace(" ", "");
                OutBound = OutBound.Trim();



                string reffff = "";
                if (CheckAlreadyOutBound(OutBound, ref reffff))
                {
                    return Json(new { errorMsg = "Có đầu 8 đã nhận rồi bao gồm " + reffff.Trim(','), success = false });
                }

                GetAndPostController gp = new GetAndPostController();
                Dau8[] ds = gp.GetOutBound(OutBound, 1);
                if (ds.Length == 0)
                {
                    return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
                }
                Commons.ExecuteNoneQuery("delete MyOB where AdminID=" + GlobalVariables.UserID.ToString());

                foreach (string item in OutBound.Split('\n'))
                {
                    Commons.ExecuteNoneQuery("exec SP_InsertMyOB '" + Fix(item) + "'," + GlobalVariables.UserID.ToString());

                }
                DataTable dt = Commons.GetData("exec SP_CheckBalanceForOB " + GlobalVariables.UserID.ToString() + ",N'" + Fix(GlobalVariables.DivisionID) + "'");

                string obthieu = "";
                string obdu = "";
                foreach (DataRow item in dt.Rows)
                {
                    if (Commons.ConvertToInt(item[3]) > 0)
                    {
                        if (obthieu.IndexOf(item[0].ToString()) < 0)
                        {
                            obthieu = obthieu + item[0] + "<br/>";
                        }
                    }
                }
                string[] l = OutBound.Split('\n');
                foreach (string item in l)
                {
                    if (obthieu.IndexOf(item) < 0)
                    {
                        if (obdu.IndexOf(item) < 0)
                        {
                            obdu = obdu + item + "<br/>";
                        }
                    }
                }

                Session["thieu"] = dt;
                return Json(new { msg = "Cập nhật thành công", thieu = obthieu, du = obdu, success = true, link = "/admin/ex?id=2&s=thieu" });

            }
            catch (Exception x)
            {
                return Json(new { errorMsg = x.Message, success = false });


            }


        }
        public string NewPRC()
        {
            string sSQL = "exec [GetNewPRCKey] '" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            string PalletID = dt.Rows[0][0].ToString();
            return PalletID;
        }
        public string NewRaCong()
        {
            string sSQL = "exec [GetNewPRCKey] '" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            string PRCID = dt.Rows[0][0].ToString();
            return PRCID;
        }
        public ActionResult RaCong()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            int ForSmall = Commons.ConvertToInt(Request.QueryString["forsmall"]);

            if (VoucherID == "")
            {
                VoucherID = NewRaCong();
                Response.Redirect("~/admin/RaCong?id=" + VoucherID + "&forsmall=" + ForSmall);

            }
            ViewBag.NV = GlobalVariables.UserName;
            ViewBag.VoucherDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Finished = false;
            ViewBag.Locked = false;

            string ssql = "exec SP_PrintPRC N'" + Fix(VoucherID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(ssql);

            DataTable dd = GlobalVariables.GetAvalibleTrucks;


            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ViewBag.VoucherDate = Commons.ConvertToDateTime(r["VoucherDate"]).ToString("dd/MM/yyyy HH:mm");
                ViewBag.NV = r["NV"].ToString();
                ViewBag.NVCID = r["VCID"];
                ViewBag.SX = r["SX"].ToString();
                ViewBag.NVGH = r["NVGH"].ToString();
                ViewBag.Finished = Commons.ConvertToBool(r["Finished"]);
                ViewBag.Locked = Commons.ConvertToBool(r["Locked"]);
                ViewBag.Price = Commons.ConvertToDecimal(r["Price"]).ToString("0");
                ViewBag.MUnitID = r["MUnitID"];
                ViewBag.TotalAmount = Commons.ConvertToDecimal(r["TotalAmount"]).ToString("N0");
                ViewBag.WareHouse = r["WareHouse"];
                ViewBag.OutputTime = r["OutputTime"];
                ViewBag.MBag = Commons.ConvertToDecimal(r["MBag"]).ToString("0");
                ViewBag.MBox = Commons.ConvertToDecimal(r["MBox"]).ToString("0");
                ViewBag.MBag1 = Commons.ConvertToDecimal(r["MBag1"]).ToString("0");
                ViewBag.MBox1 = Commons.ConvertToDecimal(r["MBox1"]).ToString("0");
                ViewBag.MC = Commons.ConvertToDecimal(r["MC"]).ToString("0");
                ViewBag.Receiver = r["Receiver"];
                ViewBag.BB = r["BB"];
                ViewBag.M3 = Commons.ConvertToDecimal(r["M3"]).ToString("N3");
                ViewBag.Content = r["Content"];
                ViewBag.Title1 = r["Title"];
                ViewBag.Description = r["Description"];
            }
            else
            {
                ViewBag.OutputTime = DateTime.Now.ToString("HH:mm");
                ViewBag.TotalAmount = 0;
                ViewBag.Price = 0;
                ViewBag.MBag1 = 0;
                ViewBag.MC = 0;
                ViewBag.MBox1 = 0;
                ViewBag.MBag1 = 0;
            }
            ViewBag.TruckList = dd.Rows;
            ViewBag.listnvc = Commons.GetData("select NVCID,NVCName from NVC order by 1 ").Rows;

            return View();
        }



        [HttpPost]
        public ActionResult SP_GetSumPRC(string PRCID)
        {
            try
            {
                string ssql = "exec SP_GetSumPRC '" + Fix(PRCID) + "','" + Fix(GlobalVariables.DivisionID) + "'";
                DataTable dt = Commons.GetData(ssql);
                DataRow r = dt.Rows[0];
                int OBCount = Convert.ToInt32(r["OBCount"]);
                int Box = Convert.ToInt32(r["Box"]);
                int Bag = Convert.ToInt32(r["Bag"]);
                int Q2 = Convert.ToInt32(r["Q2"]);
                int Q3 = Convert.ToInt32(r["Q3"]);
                string content = "Danh sách kiện ra cổng ";
                if (OBCount > 0)
                {
                    content += " " + OBCount.ToString("N0") + " outbound";
                }

                if (Box > 0)
                {
                    content += ", " + Box.ToString("N0") + " thùng";
                }

                if (Bag > 0)
                {
                    content += ", " + Bag.ToString("N0") + " bao";
                }

                if (Q2 > 0)
                {
                    content += ", " + Q2.ToString("N0") + " sản phẩm ";
                }

                if (Q3 > 0)
                {
                    content += ", " + Q3.ToString("N0") + " túi xốp ";
                }

                return Json(new { msg = content, success = true });

            }
            catch (Exception ex)
            {

                return Json(new { errorMsg = ex.Message, success = false });

            }


        }
        [HttpPost]
        public ActionResult GetPRCCT()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "exec SP_GetPRCCT N'" + Commons.Fix(VoucherID) + "'";
            sSQL += ", N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            int Q2 = 0, Q3 = 0;

            DataTable dt = Commons.GetData(sSQL);

            foreach (DataRow item in dt.Rows)
            {
                Q2 += Commons.ConvertToInt(item["Q2"]);
                Q3 += Commons.ConvertToInt(item["Q3"]);

            }
            DataRow r = dt.NewRow();
            r["OB"] = "<strong>Tổng cộng</strong>";
            r["Q2"] = Q2;
            r["Q3"] = Q3;
            r["TT"] = 0;
            dt.Rows.Add(r);

            string[] label = { "Outbound", "Loại", "TT", "SL hàng", "SL túi xốp" };
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OB1 = p["OB"],
                            OB = FGetPRCCT(p["OB"], Commons.ConvertToString(p["OB"]), true),
                            Style = FGetPRCCT(p["OB"], BaoThung(p["Style"], p["SpecialBag"], p["SpecialCarton"]), false),
                            TT = FGetPRCCT(p["OB"], Commons.ConvertToInt(p["TT"]).ToString("N0"), false),
                            Q2 = FGetPRCCT(p["OB"], Commons.ConvertToInt(p["Q2"]).ToString("N0"), true),
                            Q3 = FGetPRCCT(p["OB"], Commons.ConvertToInt(p["Q3"]).ToString("N0"), true),
                            Info = MergeInfo(label, new object[] {
                                Commons.ConvertToString(p["OB"]),
                                Commons.ConvertToString(p["Style"]),
                                Commons.ConvertToInt(p["TT"]).ToString("N0"),
                                Commons.ConvertToInt(p["Q2"]).ToString("N0"),
                                Commons.ConvertToInt(p["Q3"]).ToString("N0")
                            })
                        };
            return Json(query);
        }
        public string FGetPRCCT(object s1, string s2, bool ah)
        {

            if (s1.ToString() == "<strong>Tổng cộng</strong>")
            {
                if (ah == false)
                {
                    return "";
                }

                return "<strong>" + s2 + "</strong>";
            }
            return s2;
        }
        public string MergeInfo(string[] label, object[] list, string Color = "")
        {
            if (list[0].ToString() == "<strong>Tổng cộng</strong>")
            {
                return "";
            }

            string sResult = "";
            int i = 0;
            foreach (object item in list)
            {
                sResult += "<p><strong>" + label[i] + "</strong> : " + Commons.ConvertToString(item) + "</p>";
                i++;
                if (Commons.ConvertToString(item).IndexOf("Tổng cộng") >= 0)
                {
                    return "";
                }
            }
            if (Color != "")
            {
                sResult = "<div style='color:" + Color + "'>" + sResult + "</div>";
            }
            return sResult;
        }
        [HttpPost]
        public ActionResult PostPRC(string BarCode, string VoucherID, string NV, string SX, string NVGH
          , string VCID, decimal Price,
          string WareHouse, string OutputTime, int MBag1, int MBox1, string Receiver
          , int MC, string BB, string Description, string Content, string From, string Title1

            )
        {
            //8820259724
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });

                }
                if (SX == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập số xe", success = false });
                }

                if (BarCode.Length < 24 && BarCode.Length != 10)
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }
                string sMessage = "";
                string OB = BarCode.Substring(0, 10);
                if (BarCode.Length > 10)
                {

                    string T = BarCode.Substring(10, 1);
                    string st = "";
                    if (T == "B")
                    {
                        st = "Bao";
                    }
                    else
                    {
                        st = "Thung";
                    }
                    int TT = 0;
                    int SLSP = 0;
                    int SLTX = 0;

                    TT = Commons.ConvertToInt(BarCode.Substring(11, 3));
                    if (TT == 0)
                    {
                        return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                    }
                    SLSP = Commons.ConvertToInt(BarCode.Substring(14, 5));
                    SLTX = Commons.ConvertToInt(BarCode.Substring(19, 5));

                    //add vao chi tiet

                    bool result = SP_InsertPRCD(VoucherID, NV, SX, NVGH, OB, st, TT, SLSP, SLTX, ref sMessage);
                    if (result == false)
                    {
                        return Json(new { errorMsg = sMessage, success = false });
                    }
                }
                else
                {

                    bool result = SP_InsertPRCD(VoucherID, NV, SX, NVGH, BarCode, "", 1, 0, 0, ref sMessage);
                    if (result == false)
                    {
                        return Json(new { errorMsg = sMessage, success = false });
                    }
                }


                if (From != "mobile")
                {
                    string sWrite = "exec SP_UpdateExtraPRC ";
                    sWrite += " N'" + Fix(VoucherID) + "'";
                    sWrite += ",N'" + Fix(GlobalVariables.DivisionID) + "'";
                    sWrite += ",N'" + Fix(Receiver) + "'";
                    sWrite += ",N'" + Fix(VCID) + "'";
                    sWrite += "," + Commons.DecimalToSQL(Price);
                    sWrite += ",N'" + Fix(WareHouse) + "'";
                    sWrite += "," + MBag1.ToString("0");
                    sWrite += "," + MBox1.ToString("0");
                    sWrite += "," + MC.ToString("0");
                    sWrite += ",N'" + Fix(BB) + "'";
                    sWrite += ",N'" + Fix(Description) + "'";
                    sWrite += ",N'" + Fix(OutputTime) + "'";
                    sWrite += ",N'" + Fix(NV) + "'";
                    sWrite += ",N'" + Fix(NVGH) + "'";
                    sWrite += ",N'" + Fix(SX) + "'";
                    sWrite += "," + GlobalVariables.UserID.ToString("0");
                    sWrite += ",N'" + Fix(Content) + "'";
                    sWrite += ",N'" + Fix(Title1) + "'";

                    Exception ex = null;

                    bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                    if (b == false)
                    {
                        return Json(new { errorMsg = ex.Message, success = false });

                    }


                }


                return Json(new { msg = sMessage, OB = OB, success = true });

            }
            catch (Exception mmm)
            {

                return Json(new { errorMsg = "Tem không hợp lệ " + mmm.Message, success = false });
            }
        }

        [HttpPost]
        public ActionResult PostPRCM(string VoucherID, string NV, string SX
           , string VCID, decimal Price, string WareHouse, string OutputTime, int MBag1
           , string NVGH, int MBox1, string Receiver, int MC, string BB, string Description
           , string Content, string Title1
         )
        {
            //8820259724
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });

                }
                if (SX == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập số xe", success = false });
                }




                string sWrite = "exec SP_UpdateExtraPRC ";
                sWrite += " N'" + Fix(VoucherID) + "'";
                sWrite += ",N'" + Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Fix(Receiver) + "'";
                sWrite += ",N'" + Fix(VCID) + "'";
                sWrite += "," + Commons.DecimalToSQL(Price);
                sWrite += ",N'" + Fix(WareHouse) + "'";
                sWrite += "," + MBag1.ToString("0");
                sWrite += "," + MBox1.ToString("0");
                sWrite += "," + MC.ToString("0");
                sWrite += ",N'" + Fix(BB) + "'";
                sWrite += ",N'" + Fix(Description) + "'";
                sWrite += ",N'" + Fix(OutputTime) + "'";
                sWrite += ",N'" + Fix(NV) + "'";
                sWrite += ",N'" + Fix(NVGH) + "'";
                sWrite += ",N'" + Fix(SX) + "'";
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                sWrite += ",N'" + Fix(Content) + "'";
                sWrite += ",N'" + Fix(Title1) + "'";

                Exception ex = null;

                bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (b == false)
                {
                    return Json(new { errorMsg = ex.Message, success = false });

                }





                return Json(new { msg = "Cập nhật thành công", success = true });

            }
            catch (Exception mmm)
            {

                return Json(new { errorMsg = "Cập nhật thất bại " + mmm.Message, success = false });
            }
        }

        public string Fix(string v)
        {
            return Commons.Fix(v);
        }
        public bool SP_InsertPRCD(string VoucherID, string NV, string SX, string NVGH, string OB, string Style, int TT, int SLSP, int SLTX, ref string sMessage)
        {
            if (IsPRCLocked(VoucherID))
            {
                sMessage = "Phiếu này bị khoá rồi.";
                return false;
            }
            string sWrite = "exec SP_InsertPRCD N'" + Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Fix(VoucherID) + "'";
            sWrite += ",N'" + Fix(NV) + "'";
            sWrite += ",N'" + Fix(OB) + "'";
            sWrite += ",N'" + Fix(Style) + "'";
            sWrite += "," + TT.ToString("0");
            sWrite += "," + SLSP.ToString("0");
            sWrite += "," + SLTX.ToString("0");
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ",N'" + Fix(SX) + "'";
            sWrite += ",N'" + Fix(NVGH) + "'";

            DataTable dt = Commons.GetData(sWrite);
            int result = Commons.ConvertToInt(dt.Rows[0][0]);
            sMessage = dt.Rows[0][1].ToString();
            if (result == 0)
            {
                return false;
            }

            return true;

        }

        public bool ExistsPRCD(string OB, string Style, int TT, string VoucherID)
        {
            string ssql = "select OB from PRCD where PRCID='" + Fix(VoucherID) + "' ";
            ssql += " and OB='" + Fix(OB) + "' and Style='" + Fix(Style) + "' and TT=" + TT.ToString();
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        public bool ExistsPending(string OB, string Style, int TT, string VoucherID)
        {
            string ssql = "select OB from PendingD where PendingID='" + Fix(VoucherID) + "' ";
            ssql += " and OB='" + Fix(OB) + "' and Style='" + Fix(Style) + "' and TT=" + TT.ToString();
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult DeletePRCD(string OB, string Style, int TT, string VoucherID)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }
                if (Global.Commons.CheckPermit("racong") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }
                if (ExistsPRCD(OB, Style, TT, VoucherID) == false)
                {
                    return Json(new { errorMsg = "Barcode này chưa quét trên phiếu này nên không thể xoá", success = false });

                }
                if (IsPRCLocked(VoucherID))
                {
                    return Json(new { errorMsg = "Phiếu này đã bị khoá rồi", success = false });

                }
                string sWrite = "exec SP_DeletePRCD ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(VoucherID) + "' ";
                sWrite += ",N'" + Commons.Fix(OB) + "'";
                sWrite += ",N'" + Commons.Fix(Style) + "'";
                sWrite += "," + TT.ToString("0");
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                Exception ex = null;
                bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (r)
                {
                    return Json(new { msg = "Xoá thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            catch (Exception exxx)
            {
                return Json(new { errorMsg = exxx.Message, success = false });


            }

        }
        public ActionResult ViewPRC()
        {

            int F = Commons.ConvertToInt(Request.QueryString["f"]);
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("racong") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec [SP_GetCountPRC] N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {

                            r[0] = "/admin/ViewPRC?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewPRC?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewPRC?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }
        [HttpPost]
        public ActionResult Get_PRC()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetPRC N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            VoucherDate = Convert.ToDateTime(p["VoucherDate"]).ToString("dd/MM/yyyy HH:mm"),
                            NV = Commons.ConvertToString(p["NV"]),
                            Title = Commons.ConvertToString(p["Title"]),
                            NVGH = Commons.ConvertToString(p["NVGH"]),
                            SX = Commons.ConvertToString(p["SX"]),
                            PRCID = Commons.ConvertToString(p["PRCID"]),
                            Q2 = Commons.ConvertToInt(p["Q2"]).ToString("N0"),
                            Q3 = Commons.ConvertToInt(p["Q3"]).ToString("N0"),
                            OBCount = Commons.ConvertToInt(p["OBCount"]).ToString("N0"),
                            Finished = (Commons.ConvertToBool(p["Finished"]) ? "<span style='color:green'>Hoàn thành</span>" : "<span style='color:red'>Dở dang</span>"),
                            Locked = (Commons.ConvertToBool(p["Locked"]) ? "<span style='color:green'>Đã khoá</span>" : "<span style='color:red'>Chưa khoá</span>")


                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult DeletePRC(string PRCID)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Global.Commons.CheckPermit("racong") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (IsPRCLocked(PRCID))
            {
                return Json(new { errorMsg = "Phiếu này bị khoá rồi", success = false });
            }
            string sWrite = "exec SP_DeletePRC ";
            sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",N'" + Commons.Fix(PRCID) + "' ";

            sWrite += "," + GlobalVariables.UserID.ToString("0");
            Exception ex = null;
            bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (r)
            {
                return Json(new { msg = "Xoá thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public ActionResult ExportPRC()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_ExportPRC N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Export();
            d.ToExcel(Response, dt, "PhieuRaCong");
            return View();
        }

        public ActionResult DiHangNhanh()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetInfo(string Location, string BarCode)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }
                if (Global.Commons.CheckPermit("dihang") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }
                List<CC> b = new List<CC>();

                if (BarCode == "")
                {
                    return Json(new { errorMsg = "Bạn chưa nhập barcode hoặc mã hàng", success = false });
                }

                if (BarCode.Length == 27 && (BarCode.Substring(16, 1) == "H" || BarCode.Substring(14, 1) == "H"))
                {
                    b = GetFromHappyBitis(BarCode);
                }
                else
                if (BarCode.Length == 18 && BarCode.Substring(BarCode.Length - 1, 1) != "0")
                {    //neu tem dây 18            
                    b = GetFrom1811(BarCode);

                }
                else
                {

                    CC a = GetItemFromBarCode(BarCode);
                    if (BarCode.Length == 14 || BarCode.Length == 18)
                    {
                        a.q = 1;
                    }


                    if (a.i != null && a.i != "")
                    {
                        b.Add(a);
                    }


                }
                string Result = "";
                int Q1 = 0;
                foreach (CC item in b)
                {
                    int Q = GetQuantityFromLocation(item.i, "", Location);
                    Result += "<p style='color:blue;font-weight:bold'>Tồn  " + item.i + " : " + Q.ToString("0") + " </p>";
                    Q1 = Q;

                }
                Result += "<p><input type='button' value='Di hàng' onclick='dihang()' />  <input style='width:50px;text-align:right' type='number' class='quantity' value='" + Q1.ToString("0") + "' /></p>";

                if (b.Count > 0)
                {
                    return Json(new { msg = "Thành công", Info = Result, success = true });
                }
                else
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });


                }
            }
            catch (Exception exx)
            {
                return Json(new { errorMsg = exx.Message, success = false });


            }


        }
        [HttpPost]
        public ActionResult GetInfo2(string VoucherID)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }

                string ssql = "select ItemID,Quantity from MoveItemDetail ";
                ssql += " where DivisionID='" + Fix(GlobalVariables.DivisionID) + "'";
                ssql += " and VoucherID='" + Fix(VoucherID) + "' ";
                DataTable dt = Commons.GetData(ssql);
                string Result = "";

                foreach (DataRow item in dt.Rows)
                {
                    int Q = Commons.ConvertToInt(item["Quantity"]);
                    Result += "<p style='color:green;font-weight:bold'>Đã di hàng " + item["ItemID"].ToString() + " : " + Q.ToString("0") + " </p>";
                }
                return Json(new { msg = "Thành công", Info = Result, success = true });

            }
            catch (Exception exx)
            {
                return Json(new { errorMsg = exx.Message, success = false });


            }


        }
        public ActionResult NhomDiHang()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("nhomdihang") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            try
            {
                string[] l = from.Split('/');
                fromdate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = to.Split('/');
                todate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }

            string sSQL = "exec SP_GetGroupMCount N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",'" + fromdate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + todate.ToString("yyyy.MM.dd") + "'";

            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {
                    string l = "";
                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            l = "/admin/NhomDiHang?from=" + fromdate.ToString("dd/MM/yyyy");
                            l += "&to=" + todate.ToString("dd/MM/yyyy");
                            l += "&page=" + e;

                            r[0] = l;
                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    l = "/admin/NhomDiHang?from=" + fromdate.ToString("dd/MM/yyyy");
                    l += "&to=" + todate.ToString("dd/MM/yyyy");
                    l += "&page=" + 1;

                    rd[0] = l;
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    l = "/admin/NhomDiHang?from=" + fromdate.ToString("dd/MM/yyyy");
                    l += "&to=" + todate.ToString("dd/MM/yyyy");
                    l += "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");

                    rc[0] = l;
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }
        [HttpPost]
        public ActionResult GetGroupM()
        {
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            try
            {
                string[] l = from.Split('/');
                fromdate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));
                l = to.Split('/');
                todate = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]));

            }
            catch
            {


            }
            string sSQL = "exec [SP_GetGroupM] ";
            sSQL += "N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += "," + CurrentPage.ToString("0");
            sSQL += "," + PAGE_SIZE.ToString("0");
            sSQL += ",'" + fromdate.ToString("yyyy.MM.dd") + "'";
            sSQL += ",'" + todate.ToString("yyyy.MM.dd") + "'";
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            GroupID = p["GroupID"],
                            CreateDate = Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy HH:mm"),
                            Quantity = Commons.ConvertToInt(p["Quantity"]).ToString("N0"),
                            UserName = p["UserName"],
                            Description = p["Description"]
                        };
            return Json(query);
        }
        public ActionResult ImportForGroupM()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportForGroupM(string sList, string Description, string LocationTo)
        {
            if (CheckLocationDest(LocationTo) == false)
            {
                return Json(new { errorMsg = "Không tồn tại vị trí đích này hoặc vị trí này đã bị khóa", success = false });

            }
            //select ProcName from DangHoatDong where ProcName like '%SP_LayHangVoucher%'
            if (Commons.GetData("select ProcName from DangHoatDong where ProcName like '%SP_LayHangVoucher%'").Rows.Count > 0)
            {
                return Json(new { errorMsg = "Có người đang chạy hệ thống xác định vị trí lấy hàng. Vui lòng thử lại sau vài giây", success = false });

            }
            string ssql = "exec SP_GetGroup " + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(ssql);
            int GroupID = Commons.ConvertToInt(dt.Rows[0][0]);

            sList = sList.Trim();
            string[] lines = sList.Split('\n');
            //kiem tra
            foreach (string item in lines)
            {
                string ItemID = "";
                int Quantity = 0;
                string Location = "";
                string[] ll = item.Split(' ');
                if (ll.Length == 1)
                {
                    ll = item.Split('\t');
                }

                if (ll.Length > 2)
                {
                    ItemID = ll[0];
                    Quantity = int.Parse(ll[2]);
                    Location = ll[1];
                    if (CheckLocationSource(Location) == false)
                    {
                        return Json(new { errorMsg = "Không tồn tại vị trí " + Location + " hoặc vị trí này đã bị khóa", success = false });

                    }
                    int CQ = GetQuantityFromLocation(ItemID, "", Location);
                    if (CQ < Quantity)
                    {
                        return Json(new { errorMsg = "Không đủ " + ItemID + " số lượng tại " + Location + " chỉ có " + CQ, success = false });

                    }
                }
                else
                {
                    return Json(new { errorMsg = "Dữ liệu không hợp lệ", success = false });

                }
            }


            foreach (string item in lines)
            {
                string ItemID = "";
                int Quantity = 0;
                string Location = "";
                string[] ll = item.Split(' ');
                if (ll.Length == 1)
                {
                    ll = item.Split('\t');
                }


                ItemID = ll[0];
                Quantity = int.Parse(ll[2]);
                Location = ll[1];
                string VoucherID = GetNewKey();//tao phieu
                Exception exx = null;
                string sWrite = "exec SP_InsertGroupM " + GroupID.ToString("0") + ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "',N'" + VoucherID + "';";
                bool bd = Commons.ExecuteNoneQuery(sWrite, ref exx);
                if (bd == false)
                {
                    return Json(new { errorMsg = exx.Message, success = false });
                }



                ssql = "exec [vanchuyenconhatky] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                ssql += ",N'" + Commons.Fix(VoucherID) + "'";
                ssql += ",N'" + Commons.Fix(Description) + "'";
                ssql += ",'" + Commons.Fix(Location) + "'";
                ssql += ",'" + Commons.Fix(LocationTo) + "'";
                ssql += ",'" + Commons.Fix(ItemID) + "'";
                ssql += "," + Quantity.ToString("0");
                ssql += "," + GlobalVariables.UserID.ToString();
                ssql += ",0;";


                DataTable dt1 = Commons.GetData(ssql);

                if (Commons.ConvertToInt(dt1.Rows[0][0]) == 0)
                {
                    return Json(new { errorMsg = dt1.Rows[0][1], success = false });
                }


            }

            return Json(new { msg = "Thành công", success = true });

        }
        public ActionResult GroupMDetail()
        {
            int GroupID = Commons.ConvertToInt(Request.QueryString["id"]);
            string ssql = "select GroupID, DivisionID, CreateDate, Description, CreateBy, UserName, Quantity ";
            ssql += " from VGroupM where GroupID=" + GroupID.ToString("0");
            DataTable db = Commons.GetData(ssql);
            if (db.Rows.Count > 0)
            {
                DataRow r = db.Rows[0];

                ViewBag.CreateDate = Commons.ConvertToDateTime(r["CreateDate"]).ToString("dd/MM/yyyy");
                ViewBag.Description = r["Description"];
                ViewBag.UserName = r["UserName"];


            }
            ssql = "select * from [VGroupMDetail] where GroupID=" + GroupID.ToString("0");
            ssql += " and DivisionID='" + Fix(GlobalVariables.DivisionID) + "' ";
            ssql += " order by GroupID,ItemID,FromLocation,ToLocation";
            DataTable dt = Commons.GetData(ssql);
            int Total = 0;
            foreach (DataRow item in dt.Rows)
            {
                Total += Commons.ConvertToInt(item["Quantity"]);
            }
            ViewBag.Total = Total.ToString("N0");
            ViewBag.data = dt.Rows;
            return View();
        }
        [HttpPost]
        public ActionResult GetOBPRCInfo()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string OB = Commons.ConvertToString(Request.QueryString["ob"]);
            string sSQL = "exec SP_GetOBPRCInfo N'" + Commons.Fix(VoucherID) + "'";
            sSQL += ", N'" + Commons.Fix(OB) + "'";

            sSQL += ", N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            int Q2 = 0, Q3 = 0;

            DataTable dt = Commons.GetData(sSQL);

            foreach (DataRow item in dt.Rows)
            {
                Q2 += Commons.ConvertToInt(item["Q2"]);
                Q3 += Commons.ConvertToInt(item["Q3"]);

            }
            //DataRow r = dt.NewRow();
            //r["Style"] = "<strong>Tổng cộng</strong>";
            //r["Q2"] = Q2;
            //r["Q3"] = Q3;
            //r["TT"] = 0;
            //r["PRCID"] = VoucherID;
            //dt.Rows.Add(r);

            string[] label = { "Outbound", "Loại", "TT", "SL hàng", "SL túi xốp" };
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OB1 = p["OB"],
                            Style1 = p["Style"],
                            TT1 = p["TT"],
                            PRCID1 = p["PRCID"],
                            OB = ToMauXanh(p["OB"], p["PRCID"].ToString() != "", false),
                            Style = ToMauXanh(BaoThung(p["Style"], p["SpecialBag"], p["SpecialCarton"]) + (p["PRCID"].ToString() != "" && p["PRCID"].ToString() != VoucherID ? " - " + p["PRCID"].ToString() : ""), p["PRCID"].ToString() != "", false),
                            TT = ToMauXanh(p["TT"], p["PRCID"].ToString() != "", false),
                            Q2 = ToMauXanh(Commons.ConvertToInt(p["Q2"]).ToString("N0"), p["PRCID"].ToString() != "", false),
                            Q3 = ToMauXanh(Commons.ConvertToInt(p["Q3"]).ToString("N0"), p["PRCID"].ToString() != "", false)

                        };
            return Json(query);
        }
        public string BaoThung(object Style, object SpecialBag, object SpecialCarton)
        {
            string result = "";
            if (Commons.ConvertToInt(SpecialBag) > 0)
            {
                result += Commons.ConvertToInt(SpecialBag).ToString("N0") + " bao ";
            }

            if (Commons.ConvertToInt(SpecialCarton) > 0)
            {
                result += Commons.ConvertToInt(SpecialCarton).ToString("N0") + " thùng ";
            }

            if (result == "")
            {
                if (Style.ToString().ToLower() == "thung")
                {
                    Style = "thùng";
                }

                result = "1 " + Style.ToString().ToLower();
            }

            return result;

        }
        [HttpPost]
        public ActionResult GetOBPRCSumaryInfo(string OB, string VoucherID)
        {
            try
            {
                int All = Commons.ConvertToInt(Request.QueryString["all"]);

                string sSQL = "exec SP_GetOBPRCInfo N'" + Commons.Fix(VoucherID) + "'";
                sSQL += ", N'" + Commons.Fix(OB) + "'";

                sSQL += ", N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sSQL += "," + All;
                int Q2 = 0, Q3 = 0;
                DataTable dt = Commons.GetData(sSQL);
                int bao = 0;
                int thung = 0;
                int b = 0;
                int t = 0;
                string style = "";

                foreach (DataRow item in dt.Rows)
                {
                    style = item["Style"].ToString();
                    Q2 += Commons.ConvertToInt(item["Q2"]);
                    Q3 += Commons.ConvertToInt(item["Q3"]);
                    if (item["PRCID"].ToString() != "")
                    {
                        if (style == "Bao") { bao++; }
                        if (style == "Thung") { thung++; }
                        if (style == "")
                        {
                            bao = bao + Commons.ConvertToInt(item["SpecialBag"]);
                            b = b + Commons.ConvertToInt(item["SpecialBag"]);
                            thung = thung + Commons.ConvertToInt(item["SpecialCarton"]);
                            t = t + +Commons.ConvertToInt(item["SpecialCarton"]);
                        }
                    }
                    if (style == "Bao") { b++; }
                    if (style == "Thung") { t++; }



                }
                string content = "<p style='font-weight:bold'>OutBound: " + OB + "</p>";
                if (t > 0)
                {
                    if (thung < t)
                    {
                        content += "<p style='color:red' >Đã quét " + thung.ToString("N0") + "/" + t.ToString("N0") + " Thùng</p>";
                    }
                    else
                    {
                        content += "<p style='color:green'>Đã quét " + thung.ToString("N0") + "/" + t.ToString("N0") + " Thùng</p>";
                    }
                }
                if (b > 0)
                {
                    if (bao < b)
                    {
                        content += " <p style='color:red' >Đã quét " + bao.ToString("N0") + "/" + b.ToString("N0") + " Bao</p>";
                    }
                    else
                    {
                        content += " <p style='color:green'>Đã quét " + bao.ToString("N0") + "/" + b.ToString("N0") + " Bao</p>";
                    }
                }


                return Json(new { msg = content, success = true });
            }
            catch (Exception exx)
            {
                return Json(new { errorMsg = exx.Message, success = false });

            }


        }
        public string ToMauXanh(object s1, bool dk, bool nguyenmau)
        {
            if (s1.ToString() == "0")
            {
                s1 = "";
            }

            if (nguyenmau)
            {
                return s1.ToString();
            }

            if (dk)
            {
                return "<strong style='color:blue;'>" + s1 + "</strong>";
            }
            else
            {
                return "<strong >" + s1 + "</strong>";
            }
        }
        public ActionResult ShowWaiting()
        {
            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);
            string Location = Commons.ConvertToString(Request.QueryString["location"]);
            string ssql = "select XH.ItemID,WD.OB,XH.ConfirmQuantity  from WD inner join XH on WD.VoucherID=XH.VoucherID";
            ssql += " and WD.DivisionID=XH.DivisionID and WD.ItemID=XH.ItemID ";
            ssql += " where XH.ItemID='" + Fix(ItemID) + "' and XH.Location='" + Fix(Location) + "' ";
            ssql += " and XH.DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' and XH.Confirmed=0 and XH.TTK=1 ";
            ssql += " order by WD.OB ";
            int Total = 0;
            DataTable dt = Commons.GetData(ssql);
            foreach (DataRow item in dt.Rows)
            {
                Total += Commons.ConvertToInt(item["ConfirmQuantity"]);
            }
            ViewBag.data = dt.Rows;
            ViewBag.Total = Total.ToString("N0");
            return View();
        }
        public ActionResult HistoryItem()
        {
            string ItemID = Commons.ConvertToString(Request.QueryString["id"]);
            string from = Commons.ConvertToString(Request.QueryString["from"]);
            string to = Commons.ConvertToString(Request.QueryString["to"]);

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sWrite = "exec HistoryItem '" + Fix(ItemID) + "'";
            sWrite += ",N'" + Fix(from) + "'";
            sWrite += ",N'" + Fix(to) + "'";
            sWrite += ",N'" + Fix(GlobalVariables.DivisionID) + "'";

            DataTable dt = Commons.GetData(sWrite);
            Export d = new Export();
            d.ExportExcel(Response, "history-" + ItemID, dt);
            return View();
        }
        public ActionResult CheckOBListTC()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CheckOBListTC(string OB)
        {
            OB = OB.Trim();
            OB = OB.Trim('\n');
            string sSQL = "select ConnectionString from Divisions where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'  ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {

                return Json(new { errorMsg = "Chưa thiết lập kết nối với scanner", success = false });

            }

            string ConnectionString = Commons.ConvertToString(dt.Rows[0][0]);
            if (ConnectionString == "")
            {

                return Json(new { errorMsg = "Chưa thiết lập kết nối với scanner", success = false });
            }

            string result = "";

            foreach (string item in OB.Split('\n'))
            {
                string v = item.Trim();
                result += ComparessOB(v, ConnectionString);

            }
            return Json(new { msg = result, success = true });


        }
        [HttpPost]
        public ActionResult GetInfoFromScanner(string OB)
        {
            string sSQL = "select ConnectionString from Divisions where DivisionID=N'" + Commons.Fix(GlobalVariables.DivisionID) + "'  ";
            DataTable dt = Commons.GetData(sSQL);
            if (dt.Rows.Count == 0)
            {

                return Json(new { errorMsg = "Chưa thiết lập kết nối với scanner", success = false });

            }

            string ConnectionString = Commons.ConvertToString(dt.Rows[0][0]);
            if (ConnectionString == "")
            {

                return Json(new { errorMsg = "Chưa thiết lập kết nối với scanner", success = false });
            }

            try
            {
                sSQL = "select ItemID from IES where  DivisionID = N'" + Fix(GlobalVariables.DivisionID) + "'  ";
                DataTable tx = Commons.GetData(sSQL);
                string ltx = "";
                foreach (DataRow item in tx.Rows)
                {
                    ltx += ",'" + Commons.Fix(item[0].ToString()) + "'";
                }
                ltx = ltx.Trim(',');

                int SLH = 0, SLTX = 0;
                sSQL = "select  sum(isnull([Luong],0)) Luong from OutBound where OutBound='" + Commons.Fix(OB) + "' ";
                sSQL += " and MaHH not in(" + ltx + ")";

                dt = Commons.GetDataFromOtherDataBase(sSQL, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    SLH = Commons.ConvertToInt(dt.Rows[0][0]);
                }
                if (SLH == 0)
                {
                    return Json(new { errorMsg = "Outbound này chưa có quét hàng", success = false });

                }
                sSQL = "select  sum(isnull([Luong],0)) Luong from OutBound where OutBound='" + Commons.Fix(OB) + "' ";
                sSQL += " and MaHH  in(" + ltx + ")";

                dt = Commons.GetDataFromOtherDataBase(sSQL, ConnectionString);

                if (dt.Rows.Count > 0)
                {
                    SLTX = Commons.ConvertToInt(dt.Rows[0][0]);
                }
                string sMessage = "";
                sMessage = "SL đôi: " + SLH.ToString("N0");
                if (SLTX > 0)
                {
                    sMessage += "\nSL túi xốp: " + SLTX.ToString("N0");
                }
                return Json(new { msg = sMessage, success = true });


            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

        }
        public ActionResult PRCD()
        {
            string PRCID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "exec GetPRC '" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ;";
            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            return View();
        }
        public ActionResult PRCDM()
        {
            string PRCID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "exec GetPRC '" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ;";
            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            return View();
        }
        public ActionResult PRCDRemain()
        {
            string PRCID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "exec GetPRCRemain '" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ;";
            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            return View();
        }
        [HttpPost]
        public ActionResult PRCDRemainNote(string PRCID)
        {
            string ssql = "exec GetPRCRemain '" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ;";
            DataTable dt = Commons.GetData(ssql);
            if (dt.Rows.Count == 0)
            {
                return Json(new { msg = "", success = true });
            }
            int T = 0;


            foreach (DataRow item in dt.Rows)
            {
                T += Commons.ConvertToInt(item["T"]) + Commons.ConvertToInt(item["B"]);

            }
            return Json(new { msg = "<a href='javascript:void(0)' onclick='viewOtherOBRemain()'>Có " + T.ToString("N0") + " kiện chưa quét</a>", success = true });


        }
        public ActionResult PRCDRemainM()
        {
            string PRCID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "exec GetPRCRemain '" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ;";
            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            return View();
        }



        public ActionResult ViewTrucks()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("viewtrucks") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "exec SP_GetTruckCount N'" + Commons.Fix(keyword) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewTrucks?key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewTrucks?key=" + keyword + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewTrucks?key=" + keyword + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }

        [HttpPost]
        public ActionResult Get_Trucks()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;

            string sSQL = "exec SP_GetTruck N'" + Commons.Fix(keyword) + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            TruckID = p["TruckID"],
                            TruckName = Commons.ConvertToString(p["TruckName"]),
                            TruckType = Commons.ConvertToString(p["TruckType"]),
                            MaxVolume = p["MaxVolume"],
                            IsProcessing1 = (Commons.ConvertToBool(p["ISProcessing"]) ? "Đang bận" : "Trống"),
                            Used1 = (Commons.ConvertToBool(p["Used"]) ? "Sử dụng" : "Khoá"),
                            IsProcessing = (Commons.ConvertToBool(p["IsProcessing"]) ? "1" : "0"),
                            Used = (Commons.ConvertToBool(p["Used"]) ? "1" : "0")

                        };
            return Json(query);
        }



        [HttpPost]
        public ActionResult Update_Truck(string TruckName, string TruckType, decimal MaxVolume, string IsProcessing, string Used)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }

                if (Global.Commons.CheckPermit("viewtrucks") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }


                string TruckID = Commons.ConvertToString(Request.QueryString["id"]);


                string[] l = { "@TruckID", "@TruckName", "@MaxVolume", "@IsProcessing", "@Used", "@CreateBy", "@TruckType" };
                object[] lv = { TruckID, TruckName, MaxVolume, Commons.ConvertToInt(IsProcessing), Commons.ConvertToInt(Used), GlobalVariables.UserID, TruckType };
                DbType[] ts = { DbType.String, DbType.String, DbType.Decimal, DbType.Boolean, DbType.Boolean, DbType.Int32, DbType.String };

                Exception ex = null;
                bool r = Commons.ExecuteNoneQueryP("SP_UpdateTruck", l, lv, ts, ref ex);
                if (r)
                {
                    return Json(new { msg = "Cập nhật thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            catch (Exception exx)
            {
                return Json(new { errorMsg = exx.Message, success = false });

            }

        }
        private bool IsTruckExists(string TruckID)
        {
            string ssql = "select TruckID from Trucks where TruckID='" + Fix(TruckID) + "' ";
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult Add_Truck(string TruckID, string TruckName, string TruckType, decimal MaxVolume, string IsProcessing, string Used)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }

                if (Global.Commons.CheckPermit("viewtrucks") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }

                if (TruckID == "")
                {
                    return Json(new { errorMsg = "Số xe không được rỗng", success = false });

                }

                string[] l = { "@TruckID", "@TruckName", "@MaxVolume", "@IsProcessing", "@Used", "@CreateBy", "@TruckType" };
                object[] lv = { TruckID, TruckName, MaxVolume, Commons.ConvertToInt(IsProcessing), Commons.ConvertToInt(Used), GlobalVariables.UserID, TruckType };
                DbType[] ts = { DbType.String, DbType.String, DbType.Decimal, DbType.Boolean, DbType.Boolean, DbType.Int32, DbType.String };

                Exception ex = null;
                bool r = Commons.ExecuteNoneQueryP("SP_UpdateTruck", l, lv, ts, ref ex);
                if (r)
                {
                    return Json(new { msg = "Thêm thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            catch (Exception exx)
            {
                return Json(new { errorMsg = exx.Message, success = false });

            }

        }

        //xoa doc
        [HttpPost]
        public ActionResult DeleteTruck(string TruckID)//xoa 
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("viewtrucks") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            decimal FileID = Commons.ConvertToDecimal(Request.QueryString["id"]);
            Exception ex = null;
            string sWrite = "exec SP_DeleteTruck N'" + Commons.Fix(TruckID) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sWrite);
            int Result = Commons.ConvertToInt(dt.Rows[0][0]);
            string M = Commons.ConvertToString(dt.Rows[0][1]);
            if (Result == 1)
            {
                return Json(new { msg = M, success = true });
            }
            else
            {
                return Json(new { errorMsg = M, success = false });
            }
        }

        public ActionResult PendingOB()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("pendingob") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string PendingID = Commons.ConvertToString(Request.QueryString["id"]);
            if (PendingID == "")
            {
                PendingID = NewPendingOB();
                Response.Redirect("~/admin/PendingOB?id=" + PendingID);

            }
            else
            {

                ViewBag.Note = Request.QueryString["note"];

            }
            string Note = Commons.ConvertToString(Request.QueryString["note"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string sSQL = "exec SP_GetPendingCT_Count N'" + Commons.Fix(VoucherID) + "'";
            sSQL += ", N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",N'" + Fix(Note) + "'";


            DataTable dt = Commons.GetData(sSQL);

            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/PendingOB?id=" + VoucherID + "&note=" + Note + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/PendingOB?id=" + VoucherID + "&note=" + Note + "&page=" + 1;
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/PendingOB?id=" + VoucherID + "&note=" + Note + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE);

                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);

                }
            }
            else
            {

            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }
        public string NewPendingOB()
        {
            string sSQL = "exec [GetNewPendingKey] '" + Commons.ConvertToString(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(sSQL);
            string PendingID = dt.Rows[0][0].ToString();
            return PendingID;
        }
        [HttpPost]
        public ActionResult GetPendingCT()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string Note = Commons.ConvertToString(Request.QueryString["note"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string sSQL = "exec SP_GetPendingCT N'" + Commons.Fix(VoucherID) + "'";
            sSQL += ", N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sSQL += ",N'" + Fix(Note) + "'";
            sSQL += "," + CurrentPage;
            sSQL += "," + 20;
            int Q2 = 0, Q3 = 0;

            DataTable dt = Commons.GetData(sSQL);

            foreach (DataRow item in dt.Rows)
            {
                Q2 += Commons.ConvertToInt(item["Q2"]);
                Q3 += Commons.ConvertToInt(item["Q3"]);

            }
            DataRow r = dt.NewRow();
            r["OB"] = "<strong>Tổng cộng</strong>";
            r["Q2"] = Q2;
            r["Q3"] = Q3;
            r["TT"] = 0;
            dt.Rows.Add(r);

            string[] label = { "Outbound", "Loại", "TT", "SL hàng", "SL túi xốp" };
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OB1 = p["OB"],
                            OB = FGetPRCCT(p["OB"], Commons.ConvertToString(p["OB"]), true),
                            Style = FGetPRCCT(p["OB"], Commons.ConvertToString(p["Style"]), false),
                            Note = p["Note"],
                            TT = FGetPRCCT(p["OB"], Commons.ConvertToInt(p["TT"]).ToString("N0"), false),
                            Q2 = FGetPRCCT(p["OB"], Commons.ConvertToInt(p["Q2"]).ToString("N0"), true),
                            Q3 = FGetPRCCT(p["OB"], Commons.ConvertToInt(p["Q3"]).ToString("N0"), true),
                            Status = p["Status"],
                            Info = MergeInfo(label, new object[] {
                                Commons.ConvertToString(p["OB"]),
                                Commons.ConvertToString(p["Style"]),
                                Commons.ConvertToInt(p["TT"]).ToString("N0"),
                                Commons.ConvertToInt(p["Q2"]).ToString("N0"),
                                Commons.ConvertToInt(p["Q3"]).ToString("N0")
                            })
                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult PendingOB(string Barcode)
        {
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });

                }
                string sSQL = "exec LookUpOB N'" + Barcode + "',N'" + Fix(GlobalVariables.DivisionID) + "'";
                DataTable dt = Commons.GetData(sSQL);

                return Json(new { msg = dt.Rows[0][0].ToString(), success = true });
            }
            catch (Exception ex)
            {

                return Json(new { errorMsg = ex.Message, success = false });
            }



        }

        //xuat kho
        public ActionResult GomHang()
        {
            W w = new W();
            ViewBag.VoucherDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.ObjectID = "";
            ViewBag.ObjectName = "";
            ViewBag.WareHouseID = "";
            if (Global.Commons.CheckPermit("thietlaphethong") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            if (GlobalVariables.XA != "")
            {
                try
                {
                    w = JsonConvert.DeserializeObject<W>(GlobalVariables.XA);
                }
                catch
                {

                }

            }
            int i = 0;

            try
            {
                foreach (WD item in w.WD)
                {
                    WD n = new WD();
                    n.ItemID = item.ItemID;
                    n.ItemName = item.ItemName;
                    n.UnitID = item.UnitID;
                    n.Quantity = item.Quantity;

                    i++;
                }
            }
            catch
            {


            }

            ViewBag.data = w.WD;
            ViewBag.VoucherDate = w.VoucherDate;
            ViewBag.ObjectID = w.ObjectID;
            ViewBag.ObjectName = w.ObjectName;
            ViewBag.WareHouseID = w.IW;
            ViewBag.palletlist = w.Pallets;



            return View();
        }

        [HttpPost]
        public ActionResult PostPending(string BarCode, string VoucherID, string Note)
        {
            //8820960208B0010003800000
            try
            {

                if (BarCode.Length < 24)
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }

                string OB = BarCode.Substring(0, 10);
                string T = BarCode.Substring(10, 1);
                string st = "";
                if (T == "B")
                {
                    st = "Bao";
                }
                else
                {
                    st = "Thung";
                }
                int TT = 0;
                int SLSP = 0;
                int SLTX = 0;
                //8820760327B0020003000000
                TT = Commons.ConvertToInt(BarCode.Substring(11, 3));
                if (TT == 0)
                {
                    return Json(new { errorMsg = "Tem không hợp lệ", success = false });
                }
                SLSP = Commons.ConvertToInt(BarCode.Substring(14, 5));
                SLTX = Commons.ConvertToInt(BarCode.Substring(19, 5));

                //add vao chi tiet
                string sMessage = "";
                bool result = SP_InsertPending(VoucherID, Note, OB, st, TT, SLSP, SLTX, ref sMessage);
                if (result == false)
                {
                    return Json(new { errorMsg = sMessage, success = false });
                }

                return Json(new { msg = sMessage, OB = OB, success = true });

            }
            catch (Exception mmm)
            {

                return Json(new { errorMsg = "Tem không hợp lệ " + mmm.Message, success = false });
            }
        }
        public bool SP_InsertPending(string VoucherID, string Note, string OB, string Style, int TT, int SLSP, int SLTX, ref string sMessage)
        {
            string sWrite = "exec SP_InsertPending N'" + Fix(GlobalVariables.DivisionID) + "'";
            sWrite += ",'" + Fix(VoucherID) + "'";
            sWrite += ",'" + Fix(OB) + "'";
            sWrite += ",'" + Fix(Style) + "'";
            sWrite += "," + TT.ToString("0");
            sWrite += "," + SLSP.ToString("0");
            sWrite += "," + SLTX.ToString("0");
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ",'" + Fix(Note) + "'";

            DataTable dt = Commons.GetData(sWrite);
            int result = Commons.ConvertToInt(dt.Rows[0][0]);
            sMessage = dt.Rows[0][1].ToString();
            if (result == 0)
            {
                return false;
            }

            return true;

        }
        [HttpPost]
        public ActionResult DeletePending(string OB, string Style, int TT, string VoucherID)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }
                if (Global.Commons.CheckPermit("pendingob") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }
                if (ExistsPending(OB, Style, TT, VoucherID) == false)
                {
                    return Json(new { errorMsg = "Barcode này chưa quét trên phiếu này nên không thể xoá", success = false });

                }

                string sWrite = "exec SP_DeletePending ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(VoucherID) + "' ";
                sWrite += ",N'" + Commons.Fix(OB) + "'";
                sWrite += ",N'" + Commons.Fix(Style) + "'";
                sWrite += "," + TT.ToString("0");
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                Exception ex = null;
                bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (r)
                {
                    return Json(new { msg = "Xoá thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            catch (Exception exxx)
            {
                return Json(new { errorMsg = exxx.Message, success = false });


            }

        }
        public ActionResult DeletePendingMaster(string PendingID, string Note)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }
                if (Global.Commons.CheckPermit("pendingob") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }
                string sWrite = "exec SP_DeletePendingMaster ";
                sWrite += " N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sWrite += ",N'" + Commons.Fix(PendingID) + "' ";
                sWrite += ",N'" + Commons.Fix(Note) + "' ";
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                Exception ex = null;
                bool r = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (r)
                {
                    return Json(new { msg = "Xoá thành công", success = true });
                }
                else
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
            }
            catch (Exception exxx)
            {
                return Json(new { errorMsg = exxx.Message, success = false });


            }

        }
        public ActionResult ExportPendingOB()
        {
            string PendingID = Commons.ConvertToString(Request.QueryString["id"]);

            string sSQL = "exec [ExportPendingOB] N'" + Commons.Fix(PendingID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Export();
            d.ToExcel(Response, dt, "ExportPendingOB");
            return View();
        }
        public ActionResult ExportSummaryPendingOB()
        {
            string PendingID = Commons.ConvertToString(Request.QueryString["id"]);

            string sSQL = "exec [ExportSummaryPendingOB] N'" + Commons.Fix(PendingID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            Export d = new Export();
            d.ToExcel(Response, dt, "ExportSummaryPendingOB");
            return View();
        }
        public ActionResult ViewPending()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("viewpending") == false && Global.Commons.CheckPermit("PendingOB") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec [SP_GetPendingCount] N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(keyword) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/viewpending?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/viewpending?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/viewpending?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");


            return View();
        }
        [HttpPost]
        public ActionResult Get_Pending()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetPending N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(keyword) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            VoucherDate = Convert.ToDateTime(p["VoucherDate"]).ToString("dd/MM/yyyy HH:mm"),
                            PendingID = Commons.ConvertToString(p["PendingID"]),
                            Note = Commons.ConvertToString(p["Note"]),
                            PQuantity = Commons.ConvertToInt(p["PQuantity"]).ToString("N0"),
                            EQuantity = Commons.ConvertToInt(p["EQuantity"]).ToString("N0"),
                            OBCount = Commons.ConvertToInt(p["OBCount"]).ToString("N0"),
                            UserName = p["UserName"]
                        };
            return Json(query);
        }
        public ActionResult ShowNoXH()
        {
            string sSQL = "exec ShowNoXH N'" + Fix(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;
            return View();
        }
        public ActionResult ShowXH()
        {
            string sSQL = "exec ShowXH N'" + Fix(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;
            return View();
        }
        public ActionResult UpdateLocation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateLocation(string sList)
        {


            sList = sList.Trim();
            string[] lines = sList.Split('\n');
            foreach (string item in lines)
            {
                string[] ll = item.Split('\t');

                if (ll.Length >= 4)
                {
                    string Location = ll[0];
                    string Le = ll[1];
                    string Tam = ll[2];
                    string UuTien = ll[3];
                    decimal Volume = 0;
                    int Rack = 0;
                    if (ll.Length > 4)
                    {
                        Volume = Commons.ConvertToDecimal(ll[4]);
                    }
                    if (ll.Length > 5)
                    {
                        Rack = Commons.ConvertToInt(ll[5]);
                    }
                    //if (CheckLocationExists(Location) == false)
                    //{
                    //    return Json(new { errorMsg = "Vị trí " + Location + " không tồn tại", success = false });
                    //}

                    string sWrite = "if exists(select * from locations  ";
                    sWrite += " where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += " and  Location=N'" + Fix(Location) + "') ";

                    sWrite += "update Locations set Odd=" + Commons.ConvertToInt(Le);
                    sWrite += ",Temp=" + Commons.ConvertToInt(Tam);
                    sWrite += ",UuTien=" + Commons.ConvertToInt(UuTien);
                    if (Volume > 0)
                    {
                        sWrite += ",Volume=" + Commons.DecimalToSQL(Volume);

                    }
                    if (ll.Length > 5)
                    {
                        sWrite += ",rack=" + Rack;

                    }
                    sWrite += " where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += " and Location=N'" + Fix(Location) + "' ";
                    sWrite += "  else ";
                    sWrite += @"insert into locations(DivisionID, Location, Volume, VolumeUsed,
                    MinVolume, Temp, Odd, LockedForIn, LockedForOut, UuTien)";
                    sWrite += " values(";
                    sWrite += " N'" + Fix(GlobalVariables.DivisionID) + "'";
                    sWrite += ",N'" + Fix(Location) + "'";
                    sWrite += "," + Commons.DecimalToSQL(Volume);
                    sWrite += ",0";
                    sWrite += ",0";
                    sWrite += "," + Commons.ConvertToInt(Tam);
                    sWrite += "," + Commons.ConvertToInt(Le);
                    sWrite += ",0";
                    sWrite += ",0";
                    sWrite += "," + Commons.ConvertToInt(UuTien);
                    sWrite += ")";

                    Exception ex = null;
                    bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);

                    if (b == false)
                    {
                        return Json(new { errorMsg = "Vị trí " + Location + " không lưu được . lỗi " + ex.Message, success = false });

                    }
                }
                else
                {
                    return Json(new { errorMsg = "Dữ liệu không hợp lệ tại dòng " + item, success = false });

                }

            }

            return Json(new { msg = "Cập nhật thành công", success = true });

        }


        public ActionResult LockLocation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LockLocation(string sList)
        {


            sList = sList.Trim();
            string[] lines = sList.Split('\n');
            foreach (string item in lines)
            {
                string[] ll = item.Split('\t');

                if (ll.Length == 3)
                {
                    string Location = ll[0];
                    string Nhap = ll[1];
                    string Xuat = ll[2];


                    //if (CheckLocationExists(Location) == false)
                    //{
                    //    return Json(new { errorMsg = "Vị trí " + Location + " không tồn tại", success = false });
                    //}

                    string sWrite = "";
                    sWrite += "update Locations set LockedForIn=" + Commons.ConvertToInt(Nhap);
                    sWrite += ",LockedForOut=" + Commons.ConvertToInt(Xuat);

                    sWrite += " where DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' ";
                    sWrite += " and Location=N'" + Fix(Location) + "' ";


                    Exception ex = null;
                    bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);

                    if (b == false)
                    {
                        return Json(new { errorMsg = "Vị trí " + Location + " không lưu được . lỗi " + ex.Message, success = false });

                    }
                }
                else
                {
                    return Json(new { errorMsg = "Dữ liệu không hợp lệ tại dòng " + item, success = false });

                }

            }

            return Json(new { msg = "Cập nhật thành công", success = true });

        }

        public ActionResult ExportPendingList()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int detail = Commons.ConvertToInt(Request.QueryString["d"]);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            if (detail == 0)
            {
                string sSQL = "exec SP_ExportPendingList N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sSQL += ",N'" + Commons.Fix(from) + "'";
                sSQL += ",N'" + Commons.Fix(to) + "'";
                sSQL += "," + GlobalVariables.UserID.ToString("0");
                DataTable dt = Commons.GetData(sSQL);

                Export d = new Export();
                d.ToExcel(Response, dt, "kiemtraoutbound");
            }
            else
            {
                string sSQL = "exec ExportPendingOBList ";
                sSQL += " N'" + Commons.Fix(from) + "'";
                sSQL += ",N'" + Commons.Fix(to) + "'";
                sSQL += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
                sSQL += "," + GlobalVariables.UserID.ToString("0");
                DataTable dt = Commons.GetData(sSQL);

                Export d = new Export();
                d.ToExcel(Response, dt, "kiemtraoutbound");
            }

            return View();
        }
        public ActionResult ExportPendingDetail()
        {
            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");

            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }
            string sSQL = "exec SP_ExportPendingDetail N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sSQL += ",N'" + Commons.Fix(from) + "'";
            sSQL += ",N'" + Commons.Fix(to) + "'";
            sSQL += "," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            Export d = new Export();
            d.ToExcel(Response, dt, "kiemke");
            return View();
        }
        public bool IsOBListExists(string OB)
        {
            string ssql = "select ob from oblist where ob='" + Fix(OB) + "' and DivisionID='" + Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult LockOB(string OB)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Commons.CheckPermit("lockob") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (IsOBListExists(OB) == false)
            {
                return Json(new { errorMsg = "Không tồn tại outbound này", success = false });
            }
            Exception ex = null;


            string[] l = { "@OB", "@Lock", "@DivisionID", "@UserID" };
            object[] lv = { OB, true, GlobalVariables.DivisionID, GlobalVariables.UserID };
            DbType[] ts = { DbType.String, DbType.Boolean, DbType.String, DbType.String, DbType.Int32 };

            Commons.ExecuteNoneQueryP("LockOB", l, lv, ts, ref ex);
            if (ex == null)
            {
                return Json(new { msg = "Khoá thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

        }
        [HttpPost]
        public ActionResult UnLockOB(string OB)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Commons.CheckPermit("lockob") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (IsOBListExists(OB) == false)
            {
                return Json(new { errorMsg = "Không tồn tại outbound này", success = false });
            }
            Exception ex = null;


            string[] l = { "@OB", "@Lock", "@DivisionID", "@UserID" };
            object[] lv = { OB, false, GlobalVariables.DivisionID, GlobalVariables.UserID };
            DbType[] ts = { DbType.String, DbType.Boolean, DbType.String, DbType.String, DbType.Int32 };

            Commons.ExecuteNoneQueryP("LockOB", l, lv, ts, ref ex);
            if (ex == null)
            {
                return Json(new { msg = "Mở thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

        }
        public ActionResult OBInfo()
        {
            string OB = Commons.ConvertToString(Request.QueryString["id"]);

            string ssql = "select TT,Style,Q2,Q3,Sent from OBD where OutBound='" + Fix(OB) + "' ";
            ssql += " and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'  ";
            ssql += " and Style='Thung'";
            ssql += " order by TT ";

            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            if (dt.Rows.Count > 0)
            {
                ViewBag.IsSpecial = 0;
            }

            ssql = "select TT,Style,Q2,Q3,Sent from OBD where OutBound='" + Fix(OB) + "' ";
            ssql += " and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' ";
            ssql += " and Style='Bao'";
            ssql += " order by TT ";
            dt = Commons.GetData(ssql);
            ViewBag.data1 = dt.Rows;
            if (dt.Rows.Count > 0)
            {
                ViewBag.IsSpecial = 0;
            }

            ssql = "select * from OBOut where OB='" + Fix(OB) + "' ";
            ssql += " and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "' ";

            dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            if (dt.Rows.Count > 0)
            {
                ViewBag.IsSpecial = 1;
            }

            return View();
        }
        //outbound trung chuyen
        public ActionResult UpdateScannerForOut()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateScannerForOut(string OutBound)
        {
            DataTable IES = GetTX();
            GetAndPostController gp = new GetAndPostController();
            Dau8[] ds = gp.GetOutBound(OutBound);
            if (ds.Length == 0)
            {
                return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
            }

            int TotalQuantity = 0;
            decimal TotalAmount = 0;
            string itemlist = "";
            foreach (Dau8 item in ds)
            {
                itemlist = itemlist + "N'" + Commons.Fix(item.MATNR) + "',";
            }
            itemlist = itemlist.Trim(',');
            DataTable dbb = Commons.GetData("select ItemID,Cm3 from ItemVolumes where ItemID in(" + itemlist + ")");
            decimal TotalCM3 = 0;
            int TangPham = 0;
            foreach (Dau8 item in ds)
            {
                dbb.DefaultView.RowFilter = "ItemID='" + item.MATNR.Replace("'", "") + "'";
                IES.DefaultView.RowFilter = "ItemID='" + item.MATNR.Replace("'", "") + "'";
                int Q = Commons.ConvertToInt(item.LFIMG.Replace(",", ""));
                TotalQuantity += Q;
                int TT = Commons.ConvertToInt(item.LFIMG.Replace(",", "")) * Commons.ConvertToInt(item.KBETR.Replace(",", ""));
                decimal ck = Commons.ConvertToDecimal(item.DISC.Replace(",", "")) * TT / 100;

                TotalAmount += Math.Round((TT - ck), 0);
                if (dbb.DefaultView.Count > 0)
                {
                    decimal CM3 = Commons.ConvertToDecimal(dbb.DefaultView[0]["CM3"]);
                    TotalCM3 += CM3 * Q;
                }
                if (IES.DefaultView.Count > 0)
                {
                    TangPham += Q;
                }
            }
            TotalCM3 = TotalCM3 / 1000000;

            OBList m = new OBList();
            string address = Commons.ConvertToString(ds[0].ADDRESS);
            //if (address.Trim() == "")
            address = GetAddress(ds[0].KUNNR);
            //if (address == "")
            //{
            //    address = Commons.ConvertToString(ds[0].ADDRESS);
            //}

            m.Address = address;
            m.Bag = 0;
            m.Box = 0;
            m.CustomerName = ds[0].NAME;
            m.CustomerID = ds[0].KUNNR;
            m.PlanDate = ds[0].WADAT;
            m.EmployeeName = "";
            m.ScannerID = GlobalVariables.FullName;
            m.OB = OutBound;
            m.TotalQuantity = TotalQuantity;
            m.TotalAmount = TotalAmount;
            m.M3 = TotalCM3;
            m.OB = OutBound;
            m.TotalTX = TangPham;
            Session["obout"] = m;

            return Json(new { msg = "Thành công", success = true });



        }

        public ActionResult UpdateScannerForOut1()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("updatescannerforout") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadUpdateScannerForOut();

            return View();
        }
        public void LoadUpdateScannerForOut()
        {
            DataTable tttt = new DataTable();
            ViewBag.data = tttt.Rows;
            string OB = Commons.ConvertToString(Request.QueryString["id"]);
            if (OB != "")
            {
                string sSQL = "select  OB, TotalQuantity,Bag, Box,BX";
                sSQL += ", CustomerName, Address, M3,CustomerID,PlanDate,TotalAmount,Note ";
                sSQL += " from OBOut where OB = N'" + Commons.Fix(OB) + "' ";
                sSQL += " and DivisionID='" + Fix(GlobalVariables.DivisionID) + "' ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    ViewBag.TotalQuantity = Commons.ConvertToInt(r["TotalQuantity"]).ToString("N0");
                    ViewBag.TotalAmount = Commons.ConvertToInt(r["TotalAmount"]).ToString("N0");
                    ViewBag.Bag = Commons.ConvertToInt(r["Bag"]);
                    ViewBag.Box = Commons.ConvertToInt(r["Box"]);
                    ViewBag.BX = Commons.ConvertToInt(r["BX"]);
                    ViewBag.CustomerName = Commons.ConvertToString(r["CustomerName"]);
                    ViewBag.CustomerID = Commons.ConvertToString(r["CustomerID"]);
                    ViewBag.PlanDate = Commons.ConvertToString(r["PlanDate"]);
                    ViewBag.Address = Commons.ConvertToString(r["Address"]);
                    ViewBag.M3 = Commons.ConvertToDecimal(r["M3"]).ToString("N6");
                    ViewBag.OB = OB;
                    ViewBag.Note = r["Note"];
                }



            }
            else if (Session["obout"] != null)
            {
                OBList m = (OBList)(Session["obout"]);
                ViewBag.OB = m.OB;

                ViewBag.TotalQuantity = m.TotalQuantity.ToString("N0");
                ViewBag.TotalAmount = m.TotalAmount.ToString("N0");
                ViewBag.ScannerID = m.ScannerID;
                ViewBag.BX = m.TotalTX;
                ViewBag.EmployeeName = m.EmployeeName;
                ViewBag.Bag = m.Bag;
                ViewBag.Box = m.Box;
                ViewBag.CustomerName = m.CustomerName;
                ViewBag.CustomerID = m.CustomerID;
                ViewBag.PlanDate = m.PlanDate;
                ViewBag.Address = m.Address;
                ViewBag.M3 = m.M3.ToString("N6");

            }
        }
        [HttpPost]
        public ActionResult UpdateScannerForOut1(
            int Bag, int Box, int BX, int TotalQuantity, decimal TotalAmount
            , string CustomerName
            , string Address
            , decimal M3
            , string CustomerID
            , string PlanDate
            , string OB
            , string Note
            )
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Commons.CheckPermit("updatescannerforout") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            if (Box <= 0 && Bag <= 0)
            {
                return Json(new { errorMsg = "Bạn chưa nhập thùng hoặc bao", success = false });
            }

            string sWrite = "exec SP_UpdateOutBoundForOut ";
            sWrite += "N'" + Commons.Fix(OB) + "'";
            sWrite += ",N'" + Commons.Fix(GlobalVariables.DivisionID) + "'";
            sWrite += "," + TotalQuantity.ToString("0");
            sWrite += "," + Commons.DecimalToSQL(TotalAmount);
            sWrite += "," + Bag.ToString("0");
            sWrite += "," + Box.ToString("0");
            sWrite += "," + BX.ToString("0");
            sWrite += ",N'" + Commons.Fix(CustomerName) + "'";
            sWrite += ",N'" + Commons.Fix(Address) + "'";
            sWrite += "," + Commons.DecimalToSQL(M3);
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ",N'" + Commons.Fix(CustomerID) + "'";
            sWrite += ",N'" + Commons.Fix(PlanDate) + "'";
            sWrite += ",N'" + Commons.Fix(Note) + "'";
            sWrite += ";";

            Exception ex = null;
            GetAndPostController d = new GetAndPostController();
            bool b = d.TranToSAP(OB, Bag, Box);

            if (b == false)
            {
                return Json(new { errorMsg = "Không kết nối SAP được ", success = false });
            }

            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {

                return Json(new { msg = "Cập nhật thành công", success = true });

            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }

        public ActionResult ViewScannerForOut()
        {

            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("viewscannerforout") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];

            }

            string sSQL = "exec [SP_GetCountOBForOut] N'" + Commons.Fix(GlobalVariables.DivisionID) + "','" + from + "','" + to + "',N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int nSum = Commons.ConvertToInt(dt.Rows[0][1]);
            int nSum1 = Commons.ConvertToInt(dt.Rows[0][2]);
            int nSP = Commons.ConvertToInt(dt.Rows[0][3]);
            int nBX = Commons.ConvertToInt(dt.Rows[0][4]);
            decimal nTotalAmount = Commons.ConvertToDecimal(dt.Rows[0][5]);


            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {

                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {

                            r[0] = "/admin/ViewScannerForOut?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewScannerForOut?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewScannerForOut?key=" + keyword + "&from=" + Commons.ConvertToString(Request.QueryString["from"]) + "&to=" + Commons.ConvertToString(Request.QueryString["to"]) + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            ViewBag.Sum = nSum.ToString("N0");
            ViewBag.Sum1 = nSum1.ToString("N0");
            ViewBag.Sum3 = nSP.ToString("N0");
            ViewBag.Sum4 = nBX.ToString("N0");
            ViewBag.Sum5 = nTotalAmount.ToString("N0");

            return View();
        }

        [HttpPost]
        public ActionResult Get_OBForOut()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);

            string from = Commons.ConvertToString(Request.QueryString["from"]).Replace("'", "");
            string to = Commons.ConvertToString(Request.QueryString["to"]).Replace("'", "");
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            int PAGE_SIZE = 20;
            if (from == "" || to == "")
            {
                from = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd");
                to = DateTime.Now.ToString("yyyy.MM.dd");
            }
            else
            {
                string[] l1 = from.Split('/');
                from = l1[2] + "." + l1[1] + "." + l1[0];
                l1 = to.Split('/');
                to = l1[2] + "." + l1[1] + "." + l1[0];
            }
            string sSQL = "exec SP_GetOBForOut N'" + Commons.Fix(GlobalVariables.DivisionID) + "', '" + from + "', '" + to + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0") + ",N'" + Commons.Fix(keyword) + "'," + GlobalVariables.UserID.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            CreateDate = Convert.ToDateTime(p["CreateDate"]).ToString("dd/MM/yyyy HH:mm"),
                            LastModifyDate = Convert.ToDateTime(p["LastModifyDate"]).ToString("dd/MM/yyyy HH:mm"),
                            OB = p["OB"],
                            Bag = p["Bag"],
                            Box = p["Box"],
                            TotalQuantity = Commons.ConvertToInt(p["TotalQuantity"]).ToString("N0"),
                            Q2 = Commons.ConvertToInt(p["Q2"]).ToString("N0"),
                            BX = Commons.ConvertToInt(p["BX"]).ToString("N0"),
                            TotalAmount = Commons.ConvertToInt(p["TotalAmount"]).ToString("N0"),
                            CustomerName = "<p>" + Commons.ConvertToString(p["CustomerID"]) + "<p>" + Commons.ConvertToString(p["CustomerName"]) + "</p> <p>" + Commons.ConvertToString(p["Address"]) + "</p>",
                            Address = Commons.ConvertToString(p["Address"]),
                            Note = p["Note"]

                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult DeleteOBForOut(string OB)
        {
            try
            {
                if (GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }

                if (Commons.CheckPermit("updatescannerforout") == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }

                string sWrite = "exec SP_OBForOutDelete '" + Fix(OB) + "'";
                sWrite += ",N'" + Fix(GlobalVariables.DivisionID) + "'";
                sWrite += "," + GlobalVariables.UserID.ToString("0");

                DataTable dt = Commons.GetData(sWrite);
                DataRow r = dt.Rows[0];

                if (Convert.ToInt32(r[0]) == 1)
                {
                    return Json(new { msg = r[1].ToString(), success = true });
                }

                else
                {
                    return Json(new { errorMsg = r[1].ToString(), success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

        }
        private bool IsOBLocked(string OB)
        {
            string ssql = "select OB from OBList where OB='" + Fix(OB) + "' and DivisionID= N'" + Fix(GlobalVariables.DivisionID) + "' and Locked=1 ";
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        private bool IsPRCLocked(string PRCID)
        {
            string ssql = "select PRCID from PRC where PRCID='" + Fix(PRCID) + "' and DivisionID= N'" + Fix(GlobalVariables.DivisionID) + "' and Locked=1 ";
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult ConfirmFinished(string PRCID)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            try
            {
                string sWrite = "exec SP_ConfirmFinished '" + Fix(PRCID) + "'";
                sWrite += ",N'" + Fix(GlobalVariables.DivisionID) + "'";
                sWrite += "," + GlobalVariables.UserID.ToString("0");

                DataTable dt = Commons.GetData(sWrite);
                DataRow r = dt.Rows[0];

                if (Convert.ToInt32(r[0]) == 1)
                {
                    return Json(new { msg = r[1].ToString(), success = true });
                }

                else
                {
                    return Json(new { errorMsg = r[1].ToString(), success = false });
                }

            }
            catch (Exception ex)
            {

                return Json(new { errorMsg = ex.Message, success = false });
            }

        }
        public bool IsPRCExists(string PRCID)
        {
            string ssql = "select PRCID from PRC where PRCID='" + Fix(PRCID) + "' and DivisionID='" + Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        public bool IsPRCFinished(string PRCID)
        {
            string ssql = "select PRCID from PRC where PRCID='" + Fix(PRCID) + "' and DivisionID='" + Fix(GlobalVariables.DivisionID) + "' and Finished=1 ";
            DataTable dt = Commons.GetData(ssql);
            return dt.Rows.Count > 0;
        }
        [HttpPost]
        public ActionResult LockPRC(string PRCID)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Commons.CheckPermit("lockprc") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (IsPRCExists(PRCID) == false)
            {
                return Json(new { errorMsg = "Không tồn tại phiếu này", success = false });
            }
            if (IsPRCFinished(PRCID) == false)
            {
                return Json(new { errorMsg = "Phiếu này còn đang dở dang", success = false });
            }
            Exception ex = null;


            string[] l = { "@PRCID", "@Lock", "@DivisionID", "@UserID" };
            object[] lv = { PRCID, true, GlobalVariables.DivisionID, GlobalVariables.UserID };
            DbType[] ts = { DbType.String, DbType.Boolean, DbType.String, DbType.String, DbType.Int32 };

            Commons.ExecuteNoneQueryP("LockPRC", l, lv, ts, ref ex);
            if (ex == null)
            {
                return Json(new { msg = "Khoá thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

        }
        [HttpPost]
        public ActionResult UnLockPRC(string PRCID)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Commons.CheckPermit("lockprc") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            if (IsPRCExists(PRCID) == false)
            {
                return Json(new { errorMsg = "Không tồn tại phiếu này", success = false });
            }
            Exception ex = null;


            string[] l = { "@PRCID", "@Lock", "@DivisionID", "@UserID" };
            object[] lv = { PRCID, false, GlobalVariables.DivisionID, GlobalVariables.UserID };
            DbType[] ts = { DbType.String, DbType.Boolean, DbType.String, DbType.String, DbType.Int32 };

            Commons.ExecuteNoneQueryP("LockPRC", l, lv, ts, ref ex);
            if (ex == null)
            {
                return Json(new { msg = "Mở thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }

        }
        public ActionResult GetPRCSummaryInfo()
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            try
            {
                string sWrite = "exec SP_GetPRCSummaryInfo ";
                sWrite += " N'" + Fix(GlobalVariables.DivisionID) + "'";
                sWrite += "," + GlobalVariables.UserID.ToString("0");

                DataTable dt = Commons.GetData(sWrite);
                DataRow r = dt.Rows[0];

                if (Convert.ToInt32(r[0]) == 1)
                {
                    return Json(new { msg = r[1].ToString(), success = true });
                }

                else
                {
                    return Json(new { errorMsg = r[1].ToString(), success = false });
                }

            }
            catch (Exception ex)
            {

                return Json(new { errorMsg = ex.Message, success = false });
            }
        }
        public ActionResult ShowPRCNotFinished()
        {
            string sSQL = "exec SP_ShowPRCNotFinished N'" + Fix(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;
            return View();
        }
        public ActionResult ShowPRCNotLocked()
        {
            string sSQL = "exec SP_ShowPRCNotLocked N'" + Fix(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString();
            DataTable dt = Commons.GetData(sSQL);
            ViewBag.data = dt.Rows;
            return View();
        }
        [HttpPost]
        public ActionResult TachLe(string OBList, string OldVoucher)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }
            if (Commons.CheckPermit("ViewXK") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });

            }
            if (IsXN(OldVoucher))
            {
                return Json(new { errorMsg = "Phiếu này đã xác nhận lấy hàng rồi", success = false });

            }
            string[] slist = OBList.Split(',');
            foreach (string item in slist)
            {
                if (item.Trim() == "")
                {
                    continue;
                }

                string sSQL = "exec [SP_GetWK] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                DataTable dt = Commons.GetData(sSQL);
                string VoucherID = dt.Rows[0][0].ToString();
                string sWrite = "";
                sWrite += "exec [SP_InsertW] ";
                sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                sWrite += ",'" + VoucherID + "' ";
                sWrite += ",'' ";
                sWrite += ",'' ";
                sWrite += ",'" + DateTime.Now.ToString("MM/dd/yyyy") + "'";
                sWrite += ",'GC'";
                sWrite += ",N''";
                sWrite += ",N''";
                sWrite += ",N''";
                sWrite += "," + GlobalVariables.UserID.ToString("0");
                sWrite += ",N'" + Commons.Fix(item) + "'";//outbound luu dang cach nhau dau phay
                sWrite += ",1";
                sWrite += ";";
                sWrite += "update WD set VoucherID='" + VoucherID + "' where OB='" + Fix(item) + "'";
                sWrite += " and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "';";
                sWrite += "update XH set VoucherID='" + VoucherID + "' where OB='" + Fix(item) + "'";
                sWrite += " and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "';";
                sWrite += "update WDOld set VoucherID='" + VoucherID + "' where OB='" + Fix(item) + "'";
                sWrite += " and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "';";
                sWrite += "update XHOld set VoucherID='" + VoucherID + "' where OB='" + Fix(item) + "'";
                sWrite += " and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "';";

                Exception ex = null;
                bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (b == false)
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }
                sWrite = "exec SP_UpdateOBHTML '" + VoucherID + "' ,N'" + Fix(GlobalVariables.DivisionID) + "';";
                ex = null;
                b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (b == false)
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }

                sWrite = "exec SP_UpdateOBHTML '" + OldVoucher + "' ,N'" + Fix(GlobalVariables.DivisionID) + "';";
                ex = null;
                b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                if (b == false)
                {
                    return Json(new { errorMsg = ex.Message, success = false });
                }

            }



            return Json(new { msg = "Cập nhật thành công", success = true });
        }
        public ActionResult MAP()
        {
            string PRCID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "exec SP_GetPRCAddress N'" + Fix(PRCID) + "'";
            ssql += ",N'" + Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;

            return View();
        }
        public ActionResult ReInstallOB()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "exec SP_GetGroupOBFromXK '" + Fix(VoucherID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            if (dt.Rows.Count == 0)
            {
                ViewBag.message = "Chưa xác định vị trí";
            }
            else
            {
                ViewBag.message = "";
            }
            return View();

        }
        public ActionResult ExportOBToText()
        {
            string VoucherID = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "select OB from WD where VoucherID=N'" + Fix(VoucherID) + "' ";
            ssql += " group by OB order by OB ";
            DataTable dt = Commons.GetData(ssql);
            string content = "";
            foreach (DataRow item in dt.Rows)
            {

                content += item[0].ToString() + Environment.NewLine;

            }


            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + VoucherID + ".txt");
            Response.ContentType = "application/text";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(content);
            Response.End();
            return View();
        }
        [HttpPost]
        public ActionResult Caculate_Road(string VoucherID)
        {
            try
            {
                GetAndPostController d = new GetAndPostController();
                string url = Global.Commons.MAP + "/VC/PostVC?id=" + VoucherID + "&d=" + GlobalVariables.DivisionID;
                System.Net.HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers.Clear();
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("APIKey", d.Key);
                httpWebRequest.Timeout = 60000;
                string content = "";
                content += GlobalVariables.Address + "|\n";
                string ssql = "exec SP_GetPRCAddress N'" + Fix(VoucherID) + "'";
                ssql += ",N'" + Fix(GlobalVariables.DivisionID) + "' ";
                DataTable dt = Commons.GetData(ssql);
                foreach (DataRow item in dt.Rows)
                {
                    content += item["Address"].ToString() + "|" + item["CustomerID"] + "\n";
                }
                content = content.Trim('\n').Trim();

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{Address:\"" + content + "\"}";
                    //System.IO.File.WriteAllText("d:\\json.txt", json.Replace("},", "},\n").Replace("],", "],\n"));
                    streamWriter.Write(json);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();

                }

                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }

        }


        public ActionResult ViewCustomers()
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                Response.Redirect("~/admin/login");
            }

            if (Global.Commons.CheckPermit("ViewCustomers") == false)
            {
                Response.Redirect("~/admin/notpermit");
            }

            LoadInfo();
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "exec SP_GetCusomterCount N'" + Commons.Fix(keyword) + "'";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewCustomers?key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }

                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewCustomers?key=" + keyword + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewCustomers?key=" + keyword + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);

                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");
            return View();
        }

        [HttpPost]
        public ActionResult Get_Customers()
        {
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int PAGE_SIZE = 20;

            string sSQL = "exec SP_GetCustomers N'" + Commons.Fix(keyword) + "'," + CurrentPage.ToString("0") + "," + PAGE_SIZE.ToString("0");
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            CustomerID = p["CustomerID"],
                            CustomerName = p["CustomerName"],
                            Address = p["Address"],
                            CustomerLine = p["CustomerLine"],
                            CustomerLineName = p["CustomerLineName"],
                            CustomerLine1 = (p["CustomerLine"].ToString() != "" ? p["CustomerLine"] + "-" + p["CustomerLineName"] : ""),
                            Street = p["Street"],
                            Ward = p["Ward"],
                            District = p["District"],
                            City = p["City"],
                            Country = p["Country"],

                        };
            return Json(query);
        }



        [HttpPost]
        public ActionResult Update_Customer(string CustomerID, string CustomerName,
            string Address, string CustomerLine,
            string CustomerLineName, string Street, string Ward,
            string District, string City, string Country
            )
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewCustomers") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            string[] l = { "@CustomerID", "@CustomerName", "@Address", "@CustomerLine", "@CustomerLineName", "@Street",
                "@Ward","@District","@City","@Country"
            };
            object[] lv = { CustomerID, CustomerName, Address , CustomerLine
                    , CustomerLineName , Street , Ward, District, City , Country };
            DbType[] ts = { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String
                    , DbType.String, DbType.String, DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_InsertCustomerE", l, lv, ts, ref ex);
            if (r)
            {
                return Json(new { msg = "Cập nhật thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = ex.Message, success = false });
            }
        }



        //xoa doc
        [HttpPost]
        public ActionResult DeleteCustomer(string CustomerID)//xoa 
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Global.Commons.CheckPermit("ViewCustomers") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }

            Exception ex = null;
            string sWrite = "delete Customers where CustomerID = N'" + Fix(CustomerID) + "'";
            bool result = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (result)
            {
                return Json(new { msg = "Xóa thành công", success = true });
            }
            else
            {
                return Json(new { errorMsg = "Không thể xóa", success = false });
            }
        }
        //import customers
        public ActionResult ImportCustomers()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportCustomers(string sList)
        {
            if (GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Người dùng không hợp lệ. Vui lòng đăng nhập lại", success = false });
            }
            try
            {
                sList = sList.Trim();
                string[] lines = sList.Split('\n');

                int i = 0;

                string CustomerID, CustomerName, Street, Ward, District, City, Country;

                foreach (string item in lines)
                {
                    string[] ll = item.Split('\t');

                    if (ll.Length >= 7)
                    {

                        CustomerID = ll[0].Trim();
                        CustomerName = ll[1].Trim();
                        Street = ll[2].Trim();
                        Ward = ll[3].Trim();
                        District = ll[4].Trim();
                        City = ll[5].Trim();
                        Country = ll[6].Trim();



                        Exception ex = null;
                        bool b = false;

                        b = SP_InsertCustomerE(CustomerID, CustomerName, Street, Ward, District, City, Country);

                        if (b == false)
                        {


                            return Json(new { errorMsg = "Không thể thêm khách hàng " + CustomerID + " " + ex.Message, success = false });
                        }


                    }
                    else
                    {

                        return Json(new { errorMsg = "Dữ liệu không hợp lệ", success = false });

                    }
                    i = i + 1;

                }



                return Json(new { msg = "Thành công", success = true });

            }
            catch (Exception exx)
            {

                return Json(new { errorMsg = exx.Message, success = false });
            }


        }
        public bool SP_InsertCustomerE(string CustomerID, string CustomerName, string Street, string Ward, string District, string City, string Country)

        {
            string Address = Street;
            if (Commons.ConvertToInt(Ward) > 0)
            {
                Address += ", Phường " + Ward;
            }
            else
            {
                Address += ", " + Ward;
            }

            if (Commons.ConvertToInt(District) > 0)
            {
                Address += ", Quận " + District;
            }
            else
            {
                Address += ", " + District;
            }

            Address += ", " + City + ", " + Country;

            string CustomerLine = "", CustomerLineName = "";
            string[] l = { "@CustomerID", "@CustomerName", "@Address", "@CustomerLine", "@CustomerLineName", "@Street",
                "@Ward","@District","@City","@Country"
            };
            object[] lv = { CustomerID, CustomerName, Address , CustomerLine
                    , CustomerLineName , Street , Ward, District, City , Country };
            DbType[] ts = { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String
                    , DbType.String, DbType.String, DbType.String, DbType.String };

            Exception ex = null;
            bool r = Commons.ExecuteNoneQueryP("SP_InsertCustomerE", l, lv, ts, ref ex);
            if (r)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult ExportCustomersToExcel()
        {
            string Keyword = Commons.ConvertToString(Request.QueryString["key"]);
            string ssql = "exec SP_ExportCustomersToExcel N'" + Fix(Keyword) + "'";
            DataTable dt = Commons.GetData(ssql);

            Export d = new Export();
            d.ExportExcel(Response, "CustomersToExcel", dt);
            return View();

        }
        public ActionResult UpdateNewDataFromSAP(string VoucherID)
        {
            try
            {
                if (Global.GlobalVariables.UserID == 0)
                {
                    return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
                }
                if (Global.GlobalVariables.IsAdmin == false)
                {
                    return Json(new { errorMsg = "Bạn không có quyền này", success = false });
                }
                if (LayHangDangChay())
                {
                    return Json(new { errorMsg = "Có người đang chạy hệ thống lấy hàng", success = false });

                }

                string ssql = "select OB from WD where VoucherID=N'" + Fix(VoucherID) + "' and DivisionID= N'" + Fix(GlobalVariables.DivisionID) + "' group by OB";
                DataTable dt = Commons.GetData(ssql);
                Exception exx = null;
                string sWrite = "delete WDTT where VoucherID=N'" + Fix(VoucherID) + "' and DivisionID=N'" + Fix(GlobalVariables.DivisionID) + "'";
                bool b = Commons.ExecuteNoneQuery(sWrite, ref exx);
                if (b == false)
                {
                    return Json(new { errorMsg = exx.Message, success = false });
                }

                GetAndPostController gp = new GetAndPostController();
                int OrderNo = 0;
                foreach (DataRow item in dt.Rows)
                {
                    string OB = item[0].ToString();
                    Dau8[] ds = gp.GetOutBound(OB, 2);
                    if (ds.Length == 0)
                    {
                        return Json(new { errorMsg = "Không lấy được dữ liệu. Vui lòng kiểm tra lại đầu 8 bạn dán vào đúng chưa", success = false });
                    }

                    foreach (Dau8 i8 in ds)
                    {
                        OrderNo++;
                        sWrite = "exec [SP_InsertWDTT] ";
                        sWrite += " N'" + Fix(GlobalVariables.DivisionID) + "' ";//don vi
                        sWrite += ",N'" + Fix(VoucherID) + "' ";//phieu
                        sWrite += "," + OrderNo.ToString();//thu tu
                        sWrite += ",N'" + Fix(i8.MATNR) + "' ";//ma hang
                        sWrite += "," + Commons.ConvertToInt(i8.LFIMG).ToString("0");//so luong
                        sWrite += ",N'" + Fix(i8.MEINS) + "' ";//don vi tinh
                        sWrite += ",''";//ghi chu
                        sWrite += ",N'" + Fix(i8.VBELN) + "' ";//dau 8
                        sWrite += "," + Commons.ConvertToInt(i8.KBETR).ToString("0");//gia
                        sWrite += ",N" + Commons.DecimalToSQL(Commons.ConvertToDecimal(i8.DISC));//chiet khau
                        sWrite += ",N'" + Fix(i8.WADAT) + "' ";//ngay giao
                        sWrite += ",N'" + Fix(i8.VGBEL) + "' ";//STO
                        sWrite += ",N'" + Fix(i8.LGOBE) + "' ";//kho
                        sWrite += ",N'" + Fix(i8.NAME) + "' ";//ma khach
                        sWrite += ",N'" + Fix(i8.ADDRESS) + "' ";//dia chi

                        sWrite += ",N'" + Fix(i8.REGION) + "' ";//quan huyen
                        sWrite += ",N'" + Fix(i8.VTEXT) + "' ";//loai hinh
                        sWrite += ",N'" + Fix(i8.HOPDONG) + "' ";//So HD
                        sWrite += ",N'" + Fix(i8.DIENGIAI) + "' ";//dien giai hop dong
                        Exception ex = null;
                        b = Commons.ExecuteNoneQuery(sWrite, ref ex);
                        if (b == false)
                        {
                            return Json(new { errorMsg = ex.Message, success = false });
                        }
                    }
                }

                if (LayHangDangChay())
                {
                    return Json(new { errorMsg = "Có người đang chạy hệ thống lấy hàng", success = false });

                }

                sWrite = "exec UpdateNewDataFromSAP N'" + Fix(VoucherID) + "',N'" + Fix(GlobalVariables.DivisionID) + "'," + GlobalVariables.UserID.ToString("0");
                exx = null;
                b = Commons.ExecuteNoneQuery(sWrite, ref exx);
                if (b)
                {
                    if (CheckValidXH(VoucherID) == false)
                    {
                        return Json(new { errorMsg = "Chưa có đủ hết các vị trí. còn thiếu hàng", success = false });
                    }
                    else
                    {
                        return Json(new { msg = "Cập nhật thành công", success = true });
                    }
                }
                else
                {
                    return Json(new { errorMsg = exx.Message, success = false });
                }
            }
            catch (Exception exxx)
            {

                return Json(new { errorMsg = exxx.Message, success = false });
            }

        }
        public bool LayHangDangChay()
        {
            string ssql = "select count(*) from DangHoatDong where ProcName like '%SP_LayHangVoucher%' ";
            DataTable dt = Commons.GetData(ssql);
            return Commons.ConvertToInt(dt.Rows[0][0]) > 0;
        }
        public ActionResult ViewChanging()
        {
            string ssql = "exec [SP_GetInfoChange] N'" + Fix(Commons.ConvertToString(Request.QueryString["id"])) + "','" + Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(ssql);
            ViewBag.data = dt.Rows;
            return View();
        }
        public ActionResult ExportChanging()
        {
            string id = Commons.ConvertToString(Request.QueryString["id"]);
            string ssql = "exec [SP_ExportInfoChange] N'" + Fix(id) + "','" + Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(ssql);
            Export d = new Export();
            d.ExportExcel(Response, "pickinglist_" + id, dt);
            return View();
        }
        public bool CheckValidXH(string VoucherID)
        {
            bool b = false;
            string ssql = "select dbo.CheckValidForXH N'" + Fix(VoucherID) + "' ,N'" + Fix(GlobalVariables.DivisionID) + "'";
            DataTable dt = Commons.GetData(ssql);
            if (Commons.ConvertToBool(dt.Rows[0][0]))
            {
                b = true;
            }

            return b;
        }
        public DataTable GetData(string ssql)
        {
            return Commons.GetData(ssql);
        }
        [HttpPost]
        public ActionResult CheckAllPalletInfo()
        {
            try
            {
                string ssql = "exec CheckAllPalletInfo N'" + Fix(GlobalVariables.DivisionID) + "', " + GlobalVariables.UserID.ToString("0");
                DataTable dt = GetData(ssql);
                return Json(new { msg = dt.Rows[0][0], success = true });

            }
            catch (Exception ex)
            {

                return Json(new { errorMsg = ex.Message, success = false });
            }

        }
        public ActionResult PalletNoPosition()
        {
            ViewBag.data = GetData("exec SP_GetPalletNoPosition N'" + Fix(GlobalVariables.DivisionID) + "', " + GlobalVariables.UserID.ToString("0")).Rows;
            return View();
        }
        public ActionResult PalletNotFinal()
        {
            ViewBag.data = GetData("exec SP_GetPalletNotFinal N'" + Fix(GlobalVariables.DivisionID) + "' , " + GlobalVariables.UserID.ToString("0")).Rows;
            return View();
        }
        public ActionResult ExportPalletNotFinal()
        {
            DataTable dt = Commons.GetData("exec SP_ExportPalletNotFinal  N'" + GlobalVariables.DivisionID + "'," + GlobalVariables.UserID.ToString("0"));
            Export d = new Export();
            d.ExportExcel(Response, "SP_ExportPalletDataNotFinal", dt);
            return View();
        }
        public ActionResult ExportPalletDataNoPosition()
        {
            DataTable dt = Commons.GetData("exec SP_ExportPalletNoPosition  N'" + GlobalVariables.DivisionID + "'," + GlobalVariables.UserID.ToString("0"));
            Export d = new Export();
            d.ExportExcel(Response, "SP_ExportPalletNoPosition", dt);
            return View();
        }
        public ActionResult ViewNVC()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetNVC()
        {
            string ssql = "exec SP_GetNVC ";
            DataTable dt = Commons.GetData(ssql);
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            NVCID = p["NVCID"],
                            NVCName = p["NVCName"],
                            Used = (Commons.ConvertToBool(p["Used"]) ? "1" : "0"),
                            Status = (Commons.ConvertToBool(p["Used"]) ? "Đang sử dụng" : "Không sử dụng")

                        };
            return Json(query);
        }
        [HttpPost]
        public ActionResult UpdateNVC(string NVCName, string Used)
        {
            if (NVCName == "")
            {

                return Json(new { errorMsg = "Bạn chưa nhập tên nhà vận chuyển", success = false });

            }
            string NVCID = Commons.ConvertToString(Request.QueryString["id"]);
            string sWrite = "exec SP_UpdateNVC N'" + Fix(NVCID) + "',N'" + Fix(NVCName) + "'," + (Used == "1" ? "1" : "0");
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }
            return Json(new { msg = "Cập nhật thành công", success = true });

        }
        [HttpPost]
        public ActionResult AddNVC(string NVCID, string NVCName, string Used)
        {
            if (NVCName == "")
            {
                return Json(new { errorMsg = "Bạn chưa nhập tên nhà vận chuyển", success = false });
            }
            DataTable dt = Commons.GetData("select top 1 NVCID from NVC where NVCID ='" + Fix(NVCID) + "' ");
            if (dt.Rows.Count > 0)
            {
                return Json(new { errorMsg = "Mã nhà vận chuyển này có rồi. Vui lòng nhập mã khác", success = false });
            }
            string sWrite = "exec SP_UpdateNVC N'" + Fix(NVCID) + "',N'" + Fix(NVCName) + "'," + (Used == "1" ? "1" : "0");
            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }
            return Json(new { msg = "Cập nhật thành công", success = true });

        }
        public ActionResult PrintPRC()
        {
            string PRCID = Commons.ConvertToString(Request.QueryString["id"]);
            DataTable dt = Commons.GetData("exec SP_PrintPRC N'" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "'");
            ViewBag.data = dt.Rows;
            DataTable dt1 = Commons.GetData("exec SP_PrintPRCCT N'" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "'");
            ViewBag.datact = dt1.Rows;
            return View();
        }
        public ActionResult ImportO_8()
        {
            if (Session["obout"] != null)
            {
                OBList m = (OBList)(Session["obout"]);
                ViewBag.OB = m.OB;

                ViewBag.TotalQuantity = m.TotalQuantity.ToString("N0");
                ViewBag.TotalAmount = m.TotalAmount.ToString("N0");
                ViewBag.ScannerID = m.ScannerID;
                ViewBag.BX = m.TotalTX;
                ViewBag.EmployeeName = m.EmployeeName;
                ViewBag.Bag = m.Bag;
                ViewBag.Box = m.Box;
                ViewBag.CustomerName = m.CustomerName;
                ViewBag.CustomerID = m.CustomerID;
                ViewBag.PlanDate = m.PlanDate;
                ViewBag.Address = m.Address;
                ViewBag.M3 = m.M3.ToString("N6");

            }

            return View();
        }
        [HttpPost]
        public ActionResult Tach(string VoucherID, string OB)
        {
            string list = "";
            string[] l = OB.Split('\n');
            foreach (string item in l)
            {
                list += "'" + item.Replace("'", "") + "',";
            }
            list = list.Trim(',');
            string sSQL = "exec [SP_GetWK] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            string NewVoucher = dt.Rows[0][0].ToString();
            string sWrite = "";
            sWrite += "exec [SP_InsertW] ";
            sWrite += " '" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
            sWrite += ",'" + Commons.Fix(NewVoucher) + "' ";
            sWrite += ",'' ";
            sWrite += ",'' ";
            sWrite += ",'" + DateTime.Now.ToString("MM/dd/yyyy") + "'";
            sWrite += ",'GC'";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += ",N''";
            sWrite += "," + GlobalVariables.UserID.ToString("0");
            sWrite += ",N''";//outbound luu dang cach nhau dau phay
            sWrite += ",1";
            sWrite += ";";


            sWrite += "Update WD set VoucherID=N'" + NewVoucher + "' where OB in(" + list + ")";
            sWrite += " and VoucherID=N'" + Fix(VoucherID) + "';";
            sWrite += "Update XH set VoucherID=N'" + NewVoucher + "' where OB in(" + list + ")";
            sWrite += " and VoucherID=N'" + Fix(VoucherID) + "';";
            sWrite += "exec SP_UpdateOBHTML '" + Fix(VoucherID) + "' ,N'" + Fix(GlobalVariables.DivisionID) + "';";
            sWrite += "exec SP_UpdateOBHTML '" + Fix(NewVoucher) + "' ,N'" + Fix(GlobalVariables.DivisionID) + "';";

            Exception ex = null;
            bool b = Commons.ExecuteNoneQuery(sWrite, ref ex);
            if (b == false)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }
            return Json(new { msg = "Tách thành công", VoucherID = NewVoucher, success = true });

        }
        public ActionResult XNPH()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XNPH(string OB)
        {
            try
            {
                string ssql = "exec [SP_XNPH] N'" + Commons.Fix(GlobalVariables.DivisionID) + "' ";
                ssql += ",N'" + Commons.Fix(OB) + "'";
                ssql += "," + GlobalVariables.UserID.ToString();

                DataTable dt = Commons.GetData(ssql);

                if (Commons.ConvertToInt(dt.Rows[0][0]) == 0)
                {
                    return Json(new { errorMsg = dt.Rows[0][1], html = dt.Rows[0][1], success = false });
                }


                return Json(new { msg = dt.Rows[0][1], html = dt.Rows[0][1], success = true });

            }
            catch (Exception ex)
            {

                return Json(new { errorMsg = ex.Message, html = ex.Message, success = false });
            }

        }
        public ActionResult DeleteOBFromPRC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetOBInfoFromPRC(string BarCode, string PRCID, string OB)
        {
            try
            {
                string ssql = "exec GetOBInfoFromPRC N'" + Fix(BarCode) + "',N'" + Fix(PRCID) + "',N'" + Fix(GlobalVariables.DivisionID) + "' ";
                DataTable dt = Commons.GetData(ssql);
                DataRow r = dt.Rows[0];

                if (Commons.ConvertToBool(r[0]))
                {
                    if (OB != "")
                    {
                        if (r["OB"].ToString() != OB)
                        {
                            return Json(new { errorMsg = "Bạn phải quét kiện của đầu 8: " + OB, success = false });
                        }
                    }

                    return Json(new
                    {
                        msg = r[1],
                        OrderNo = r["OrderNo"],
                        Style = r["Style"],
                        Quantity = r["Quantity"],
                        BX = r["BX"],
                        OB = r["OB"],
                        OBCount = r["OBCount"],
                        success = true
                    });
                }
                else
                {
                    return Json(new { errorMsg = r[1], success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });


            }


        }
        [HttpPost]
        public ActionResult DeleteOBInfoFromPRC(string PRCID, string OB)
        {
            try
            {
                string ssql = "exec DeleteOBInfoFromPRC N'" + Fix(OB) + "'";
                ssql += ",N'" + Fix(PRCID) + "'";
                ssql += ",N'" + Fix(GlobalVariables.DivisionID) + "'";
                ssql += ", " + GlobalVariables.UserID.ToString("0");
                DataTable dt = Commons.GetData(ssql);
                DataRow r = dt.Rows[0];

                if (Commons.ConvertToBool(r[0]))
                {
                    return Json(new
                    {
                        msg = r[1],
                        success = true
                    });

                }
                return Json(new { errorMsg = r[1], success = false });
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });

            }



        }
        public ActionResult SP_ExportTTT()
        {
            Export d = new Export();
            string ssql = "exec SP_ExportTTT ";
            DataTable dt = Commons.GetData(ssql);
            d.ExportExcel(Response, "ob", dt);
            return View();
        }
        public ActionResult UpdateAllItem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateAllItem(string sList)
        {

            List<CH> ar = new List<CH>();
            sList = sList.Trim();
            string[] lines = sList.Split('\n');
            foreach (string item in lines)
            {
                string[] ll = item.Split('\t');

                if (ll.Length >= 6)
                {
                    string itemid = ll[0];
                    string dai = ll[1];
                    string rong = ll[2];
                    string cao = ll[3];
                    string sokhoi = ll[4];
                    string sothung = ll[5];

                    string sWrite = " update itemvolumes set Length=" + Commons.ConvertToDecimal(dai);
                    sWrite += ",Width=" + Commons.ConvertToDecimal(rong);
                    sWrite += ",Height=" + Commons.ConvertToDecimal(cao);
                    sWrite += ",box=" + Commons.ConvertToInt(sothung);
                    sWrite += ",cm3=" + Commons.ConvertToDecimal(sokhoi);
                    sWrite += " where ItemID=N'" + Fix(itemid) + "'";
                    Commons.ExecuteNoneQuery(sWrite);
                }
                else
                {
                    return Json(new { errorMsg = "Dữ liệu không hợp lệ", success = false });

                }

            }

            return Json(new { msg = "Thành công", success = true });

        }
        public ActionResult ViewOBLoaiTru()
        {
            if (Commons.CheckPermit("obloaitru") == false)
            {
                Response.Redirect("~/admin/notpermit");
                return View();
            }
            string keyword = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);

            string sSQL = "select count(OB) c from OBLoaiTru ";
            if (keyword != "")
                sSQL += " where OB=N'" + Fix(keyword) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            int nTotal = Convert.ToInt32(dt.Rows[0][0]);
            int PAGE_SIZE = 20;
            DataTable dp = new DataTable();
            dp.Columns.Add("link", "".GetType());
            dp.Columns.Add("class", "".GetType());
            dp.Columns.Add("text", "".GetType());

            if (nTotal > 0)
            {
                ArrayList cactrang = GetPage(nTotal, 10, PAGE_SIZE, CurrentPage);
                if (cactrang.Count > 0)
                {

                    foreach (int e in cactrang)
                    {
                        DataRow r = dp.NewRow();

                        if (e != CurrentPage)
                        {
                            r[0] = "/admin/ViewOBLoaiTru?key=" + keyword + "&page=" + e;

                            r[1] = "page";
                            r[2] = e.ToString();
                        }
                        else
                        {
                            r[0] = "";
                            r[1] = "selectedpage";
                            r[2] = e.ToString();
                        }
                        dp.Rows.Add(r);
                    }
                    DataRow rd = dp.NewRow();
                    rd[0] = "/admin/ViewOBLoaiTru?key=" + keyword + "&page=1";
                    rd[1] = "page";
                    rd[2] = "Về đầu";
                    dp.Rows.InsertAt(rd, 0);
                    DataRow rc = dp.NewRow();
                    rc[0] = "/admin/ViewOBLoaiTru?key=" + keyword + "&page=" + Math.Ceiling(nTotal * 1.0 / PAGE_SIZE).ToString("0");
                    rc[1] = "page";
                    rc[2] = "Về cuối";
                    dp.Rows.Add(rc);
                }
            }
            ViewBag.Paging = dp;
            ViewBag.Count = nTotal.ToString("N0");

            return View();
        }
        [HttpPost]
        public ActionResult Add_OBLoaiTru(string OB)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Commons.CheckPermit("obloaitru") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            Commons.ExecuteNoneQuery("insert into OBLoaiTru (OB) values(N'" + Commons.Fix(OB) + "')");
            return Json(new { msg = "Thêm thành công", success = true });
        }
        [HttpPost]
        public ActionResult Delete_OBLoaiTru(string OB)
        {
            if (Global.GlobalVariables.UserID == 0)
            {
                return Json(new { errorMsg = "Bạn chưa đăng nhập", success = false });
            }

            if (Commons.CheckPermit("obloaitru") == false)
            {
                return Json(new { errorMsg = "Bạn không có quyền này", success = false });
            }
            Commons.ExecuteNoneQuery("Delete OBLoaiTru Where OB=N'" + Commons.Fix(OB) + "' ");
            return Json(new { msg = "Thêm thành công", success = true });
        }
        [HttpPost]
        public ActionResult Get_OBLoaiTru()
        {
            string key = Commons.ConvertToString(Request.QueryString["key"]);
            int CurrentPage = Commons.ConvertToInt(Request.QueryString["page"], 1);


            string sSQL = @" 
declare @CurrentPage int,@PageSize int
select @CurrentPage=" + CurrentPage + @"
select @PageSize=20

select v.* from (select top 100 percent Row_number () over (order by OB) position, OB from OBLoaiTru ";
            if (key != "")
                sSQL += " where OB=N'" + Fix(key) + "' ";
            sSQL += " order by OB) v where v.Position between  (@CurrentPage-1)*@PageSize +1 and @CurrentPage*@PageSize ";
            //System.IO.File.WriteAllText("d:\\ss.sql", sSQL);
            DataTable dt = Commons.GetData(sSQL);

            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            OB = p["OB"],

                        };
            return Json(query);
        }
        
    }
}




