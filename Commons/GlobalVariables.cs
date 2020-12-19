using CaptchaDotNet2.Security.Cryptography;
using System;
using System.Data;
using System.Web;

namespace Global
{
    public class GlobalVariables
    {
        public GlobalVariables()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static int UserID
        {
            get
            {

                try
                {
                    int value = Commons.ConvertToInt(Encryptor.Decrypt(HttpContext.Current.Request.Cookies["CurrentUserID"].Value), 0);

                    return value;
                }
                catch (Exception ex)
                {
                }
                return 0;

            }
            set
            {
                HttpCookie cookie_CurrentUserID = new HttpCookie("CurrentUserID", Encryptor.Encrypt(value + "") + "");
                cookie_CurrentUserID.Expires = DateTime.Now.AddDays(5);
                HttpContext.Current.Response.Cookies.Add(cookie_CurrentUserID);

                //HttpContext.Current.Session["CurrentUserID"] = value;
            }
        }
        public static string CN
        {
            get
            {

                try
                {
                    if (HttpContext.Current.Session["CN"] != null)
                    {
                        if (HttpContext.Current.Session["CN"].ToString() != "")
                        {
                            return HttpContext.Current.Session["CN"].ToString();
                        }
                    }

                    string value = Commons.ConvertToString(Encryptor.Decrypt(HttpContext.Current.Request.Cookies["CN"].Value));

                    HttpContext.Current.Session["CN"] = value;

                    return value;
                }
                catch
                {
                }
                return "";

            }
            set
            {
                HttpCookie cookie_CN = new HttpCookie("CN", Encryptor.Encrypt(value));
                cookie_CN.Expires = DateTime.Now.AddDays(15);
                HttpContext.Current.Response.Cookies.Add(cookie_CN);

                HttpContext.Current.Session["CN"] = value;
            }
        }
        public static bool IsAdmin
        {
            get
            {
                if (UserID == 0)
                {
                    return false;
                }

                string sSQL = "select isadmin from admins where adminid=" + UserID.ToString();
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                return Commons.ConvertToBool(dt.Rows[0][0]);
            }
            set { HttpContext.Current.Session["isadmin"] = value; }
        }
        public static bool ForOne
        {
            get
            {

                string sSQL = "select ForOne from Settings ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                return Commons.ConvertToBool(dt.Rows[0][0]);
            }

        }
        public static bool thongtintudonhang
        {
            get
            {

                string sSQL = "select thongtintudonhang from Settings ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                return Commons.ConvertToBool(dt.Rows[0][0]);
            }

        }
        public static bool GHXuatThang
        {
            get
            {

                string sSQL = "select XuatThang from Settings ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                return Commons.ConvertToBool(dt.Rows[0][0]);
            }

        }
        public static bool OpenForOut
        {
            get
            {

                string sSQL = "select OpenForOut from Settings ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                return Commons.ConvertToBool(dt.Rows[0][0]);
            }

        }
        public static string UserName
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Request.Cookies["AdminUserName"].Value);

                    return value;
                }
                catch (Exception ex)
                {
                }

                try
                {
                    string ssql = "select UserName from Admins where AdminID=" + GlobalVariables.UserID.ToString();
                    DataTable dt = Commons.GetData(ssql);
                    if (dt.Rows.Count > 0)
                    {
                        HttpCookie cookie_AdminUserName = new HttpCookie("AdminUserName", HttpContext.Current.Server.UrlEncode(dt.Rows[0][0].ToString()));
                        cookie_AdminUserName.Expires = DateTime.Now.AddDays(5);
                        HttpContext.Current.Response.Cookies.Add(cookie_AdminUserName);

                        return dt.Rows[0][0].ToString();
                    }
                }
                catch
                {


                }

                return "";

            }
            set
            {
                HttpCookie cookie_AdminUserName = new HttpCookie("AdminUserName", HttpContext.Current.Server.UrlEncode(value));
                cookie_AdminUserName.Expires = DateTime.Now.AddDays(5);
                HttpContext.Current.Response.Cookies.Add(cookie_AdminUserName);

            }
        }
        public static string KKNote
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Session["KKNote"]);

                    return value;
                }
                catch (Exception ex)
                {
                }
                return "";

            }
            set { HttpContext.Current.Session["KKNote"] = value; }
        }
        public static string FullName
        {
            get
            {
                if (HttpContext.Current.Session["FullName"] != null)
                    return HttpContext.Current.Session["FullName"].ToString();

                string sSQL = "select FullName from Admins where AdminID=" + GlobalVariables.UserID.ToString();
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count == 0)
                {
                    return "";
                }
                HttpContext.Current.Session["FullName"] = dt.Rows[0][0];
                return dt.Rows[0][0].ToString();
            }
            set
            {
                HttpContext.Current.Session["FullName"] = value;
            }
        }
        public static string DivisionID
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Request.Cookies["DivisionID"].Value);

                    return value;
                }
                catch (Exception ex)
                {


                }
                string sSQL = "select DivisionID from Admins where AdminID=" + UserID.ToString();
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    DivisionID = dt.Rows[0][0].ToString();
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("DivisionID", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }
        public static string Address
        {
            get
            {

                string sSQL = "select Address from Divisions where DivisionID=N'" + Commons.Fix(DivisionID) + "'";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    string A = dt.Rows[0][0].ToString();
                    return A;
                }
                return "";
            }

        }
        public static string GoogleAPIKey
        {
            get
            {

                string sSQL = "select GoogleAPIKey from Settings ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    string A = Commons.ConvertToString(dt.Rows[0][0]);
                    return A;
                }
                return "";
            }

        }
        public static string DivisionName
        {
            get
            {


                string sSQL = "select DivisionName from Divisions where DivisionID=N'" + Commons.Fix(DivisionID) + "' ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }

        }
        public static string LockedDate
        {
            get
            {


                string sSQL = "select InventoryLockedDate from KKLocks where DivisionID=N'" + Commons.Fix(DivisionID) + "' ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }

        }
        public static bool AllowNotFull
        {
            get
            {


                string sSQL = "select AllowNotFull from Settings ";
                DataTable dt = Commons.GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    return Commons.ConvertToBool(dt.Rows[0][0]);
                }
                return false;
            }

        }
        public static string TokenEditDonHang
        {
            get
            {
                try
                {
                    string value = HttpContext.Current.Server.UrlDecode(Commons.ConvertToString(HttpContext.Current.Request.Cookies["TokenEditDonHang"].Value));

                    return value;
                }
                catch (Exception ex)
                {

                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("TokenEditDonHang", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }
        public static string VoucherEditing
        {
            get
            {
                try
                {
                    string value = HttpContext.Current.Server.UrlDecode(Commons.ConvertToString(HttpContext.Current.Request.Cookies["VoucherEditing"].Value));

                    return value;
                }
                catch (Exception ex)
                {

                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("VoucherEditing", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }

        public static string PalletID
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["PalletID"].Value));

                    return value;
                }
                catch (Exception ex)
                {
                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("PalletID", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }
        public static string TimeLogin
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["TimeLogin"].Value));

                    return value;
                }
                catch (Exception ex)
                {
                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("TimeLogin", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(10);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }

        public static string WA
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["WA"].Value));
                    return value;
                }
                catch
                {
                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("WA", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }
        public static string WE
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["WE"].Value));
                    return value;
                }
                catch
                {
                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("WE", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }


        public static string XA
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["XA"].Value));
                    return value;
                }
                catch
                {
                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("XA", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }
        public static string XE
        {
            get
            {
                try
                {
                    string value = Commons.ConvertToString(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["XE"].Value));
                    return value;
                }
                catch
                {
                }
                return "";

            }
            set
            {

                HttpCookie cookie_DivisionID = new HttpCookie("XE", HttpContext.Current.Server.UrlEncode(value));
                cookie_DivisionID.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie_DivisionID);

            }
        }
        public static double Random
        {
            get
            {
                Random d = new Random();
                return d.NextDouble();
            }
        }
        public static object ChamHang
        {
            get {return HttpContext.Current.Session["chamhang"]; }
            set { HttpContext.Current.Session["chamhang"] = value; }
        }
        public static DataTable GetAvalibleTrucks
        {
            get
            {
                string sSQL = "exec SP_GetAvalibleTrucks ";
                DataTable dt = Commons.GetData(sSQL);
                return dt;
            }

        }
    }
}
