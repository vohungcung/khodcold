using System;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Global
{
    public static class Commons
    {
        public static string ReadFromFile(string path)
        {
            string strResult = "";
            System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                try
                {
                    strResult = reader.ReadToEnd();
                }
                catch
                {
                    strResult = "";
                }

                reader.Close();
                stream.Close();
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }


        public static bool SendEmailMessage(string EmailAddressFrom, string EmailAddressTo, string serverSMTP, string UserName, string PassWord, string Subject, string Body, int Port, bool isHTMl)
        {
            try
            {
                System.Net.Mail.MailMessage MS = new System.Net.Mail.MailMessage(EmailAddressFrom, EmailAddressTo, Subject, Body);
                System.Net.Mail.SmtpClient Email = new System.Net.Mail.SmtpClient(serverSMTP, Port);
                Email.Credentials = new System.Net.NetworkCredential(UserName, PassWord);
                Email.EnableSsl = true;
                MS.IsBodyHtml = isHTMl;
                Email.Send(MS);

            }
            catch (Exception ex)
            {
                WriteToLog(ex.Message);
                return false;

            }
            return true;


        }


        public static int ConvertToInt(object value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
            }
            return 0;
        }
        public static bool ConvertToBool(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
            }
            return false;
        }
        public static decimal ConvertToDecimal(object value)
        {
            try
            {
                return Convert.ToDecimal(value.ToString().Trim().Replace(",", ""));
            }
            catch
            {
            }
            return 0;
        }
        public static DateTime ConvertToDateTime(object value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
            }
            return DateTime.Now;
        }
        public static int ConvertToInt(object value, int defaultvalue)
        {
            if (ConvertToString(value) == "")
            {
                return defaultvalue;
            }

            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
            }
            return defaultvalue;
        }
        public static string ConvertToString(object value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
            }
            return "";
        }
        public static string GoogleAPIKey
        {
            get
            {
                try
                {
                  
                        return System.Configuration.ConfigurationManager.AppSettings["GoogleAPIKey"];
                    
                   
                }
                catch
                {


                }
                return "";

            }
        }
        public static string ConnectionString
        {
            get
            {
                try
                {
                    if (GlobalVariables.CN == "CNMN" || GlobalVariables.CN == "")
                    {
                        return System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    }
                    else
                    {
                        return System.Configuration.ConfigurationManager.ConnectionStrings[GlobalVariables.CN].ConnectionString;
                    }
                }
                catch
                {
                    //GlobalVariables.UserID = 0;
                    //GlobalVariables.DivisionID = "CNMN";

                }
                return "";

            }
        }
        public static string MAP
        {
            get
            {
                try
                {

                    return System.Configuration.ConfigurationManager.AppSettings["Map"];

                }
                catch
                {


                }
                return "";

            }
        }

        public static string APIHost
        {
            get
            {
                try
                {
                   
                        return System.Configuration.ConfigurationManager.AppSettings["APIHost"];
                  
                }
                catch
                {


                }
                return "";

            }
        }
        public static string APIHostKey
        {
            get
            {
                try
                {

                    return System.Configuration.ConfigurationManager.AppSettings["APIHostKey"];

                }
                catch
                {


                }
                return "";

            }
        }
        public static string WUSERConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["WUSER"].ConnectionString;
        public static string ConnectionString11 => System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        public static System.Data.DataTable GetDataFromOtherDataBase(string ssql)
        {
            DataTable dt = new DataTable();
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConnectionString11);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(ssql, cn);
            cn.Open();
            da.Fill(dt);
            cn.Close();
            return dt;
        }
        public static System.Data.DataTable GetDataFromOtherDataBase(string ssql, string sConnectionString)
        {
            DataTable dt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(ssql, sConnectionString);
            da.Fill(dt);
           
            return dt;
        }
        public static System.Data.DataTable GetData(string ssql)
        {
            DataTable dt = new DataTable();
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(ssql, cn);
            da.SelectCommand.CommandTimeout = 3000;
            cn.Open();
            da.Fill(dt);
            cn.Close();
            return dt;
        }
        public static System.Data.DataSet GetMData(string ssql)
        {
            DataSet dt = new DataSet();
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(ssql, cn);
            da.SelectCommand.CommandTimeout = 3000;
            da.Fill(dt);
            return dt;
        }
        public static bool ExecuteNoneQuery(string ssql)
        {

            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(ssql, cn);
            cmd.CommandTimeout = 30000;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch
            {
                cn.Close();
                return false;
            }

            return true;
        }
        public static bool ExecuteNoneQuery(string ssql, ref Exception ex)
        {
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(ssql, cn);
            cmd.CommandTimeout = 30000;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex1)
            {
                ex = ex1;
                cn.Close();
                return false;
            }
            ex = null;
            return true;
        }

        public static bool ExecuteNoneQueryP(string ssql, string[] l, object[] o, DbType[] types, ref Exception ex)
        {

            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(ssql, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;
            for (int i = 0; i < l.Length; i++)
            {
                System.Data.SqlClient.SqlParameter p = new System.Data.SqlClient.SqlParameter();
                p.SourceColumnNullMapping = true;
                p.Value = o[i];
                p.SourceColumn = l[i];
                p.ParameterName = l[i];
                p.Direction = ParameterDirection.Input;
                p.DbType = types[i];
                if (types[i] == DbType.Decimal)
                {
                    p.SqlDbType = SqlDbType.Decimal;

                    p.Scale = 2;
                    p.Precision = 18;
                }

                cmd.Parameters.Add(p);
            }
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex1)
            {
                ex = ex1;
                cn.Close();
                return false;
            }
            ex = null;
            return true;
        }
        public static string FixContent(string content)
        {
            try
            {
                System.Web.UI.Control d = new Control();
                return content.Replace("../", d.ResolveClientUrl("~/"));

            }
            catch
            {


            }
            return content;
        }

        public static DataTable GetData(string ssql, int currentpage, int size, ref int total)
        {
            DataTable results = new DataTable();
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtr = new DataTable();
            dt = GetData(ssql);
            dtr = dt.Clone();
            int i = 0;
            total = dt.Rows.Count;

            foreach (System.Data.DataRow r in dt.Rows)
            {
                if (i >= (currentpage - 1) * size && i < currentpage * size)
                {
                    DataRow a = dtr.NewRow();

                    for (int c = 0; c < dt.Columns.Count; ++c)
                    {
                        a[c] = r[c];
                    }

                    dtr.Rows.Add(a);
                }
                i++;
            }
            return dtr;
        }
        public static DataTable GetData(System.Data.DataTable dt, int currentpage, int size, ref int total)
        {
            DataTable results = new DataTable();
            System.Data.DataTable dtr = new DataTable();
            dtr = dt.Clone();
            int i = 0;
            total = dt.Rows.Count;

            foreach (System.Data.DataRow r in dt.Rows)
            {
                if (i >= (currentpage - 1) * size && i < currentpage * size)
                {
                    DataRow a = dtr.NewRow();

                    for (int c = 0; c < dt.Columns.Count; ++c)
                    {
                        a[c] = r[c];
                    }

                    dtr.Rows.Add(a);
                }
                i++;
            }
            return dtr;
        }
        public static string Fix(string value)
        {
            if (value == null)
            {
                return "";
            }

            return value.Replace("'", "''").Trim();
        }
        public static bool EmailIsExists(string Email)
        {
            string ssql = "select top 1 Email from admins where Email like N'" + Fix(Email) + "'";
            DataTable dt = GetData(ssql);
            return dt.Rows.Count > 0;
        }


        public static bool UserIsExists(string UserName)
        {
            string ssql = "select dbo.IsUserExists( N'" + Fix(UserName) + "') result ";
            DataTable dt = GetData(ssql);
            return ConvertToBool(dt.Rows[0][0]);
        }

        public static string SEO(string keyword, string description)
        {
            string sResult = "";

            sResult += "<meta name=\"keywords\" content=\"" + keyword + "\" />";
            sResult += "<meta name=\"description\" content=\"" + description + "\" />";
            sResult += "<meta name=\"owner\" content=\"GB\" />";
            sResult += "<meta content=\"" + keyword + "\" name=\"title\">";


            return sResult;
        }


        public static bool IsEmail(string expression)
        {
            string emailPattern = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
            Regex regex = new Regex(emailPattern);

            return regex.IsMatch(expression.Trim());
        }
        public static string SubString(string value, int len)
        {
            if (value.Length < len)
            {
                return value;
            }

            return value.Substring(0, len) + "...";
        }
        public static string SubString1(string value, int len)
        {
            if (value.Length < len)
            {
                return value;
            }

            return value.Substring(0, len);
        }

        public static string DecimalToSQL(decimal value)
        {
            return value.ToString().Replace(",", "");
        }


        public static string ConvertToURLName(object Name)
        {
            return FixStringUrl(Commons.RemoveSign4VietnameseString(ConvertToString(Name))).Replace(".", "");

        }
        public static string ConvertTitle(object Title)
        {
            return ConvertToString(Title).Replace("'", "").Replace("\"", "");

        }
        public static string FixStringUrl(string value)
        {
            value = RemoveSign4VietnameseString(value);
            value = value.Replace(" ", "-");
            value = value.Replace("--", "-");

            string ar = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-";
            string newvalue = "";
            foreach (char item in value)
            {
                if (ar.IndexOf(item) >= 0)
                {
                    newvalue += item;
                }
            }
            newvalue = newvalue.ToLower();
            return ConvertToString(newvalue).Replace(".", "");
        }
        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                {
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }
            return str;
        }

        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static Regex MobileCheck = new Regex(@"android|(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        public static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        public static bool fBrowserIsMobile()
        {
            Debug.Assert(HttpContext.Current != null);

            if (HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

                if (u.Length < 4)
                {
                    return false;
                }

                if (MobileCheck.IsMatch(u) || MobileVersionCheck.IsMatch(u.Substring(0, 4)))
                {
                    return true;
                }
            }

            return false;
        }
        public static void WriteToLog(string Content)
        {
            Commons.ExecuteNoneQuery("exec WriteLog N'" + Commons.Fix(Content) + "'");
        }

        public static bool CheckPermit(string ScreenID)
        {
            string sSQL = "select dbo.CheckPermit("+Global.GlobalVariables.UserID.ToString();
            sSQL += " ,'" + Commons.ConvertToString(ScreenID) + "') ";
            DataTable dt = GetData(sSQL);
            return Commons.ConvertToBool(dt.Rows[0][0]);
        }
        public static bool CheckPermit(string ScreenID, int AdminID)
        {
            string sSQL = "select dbo.CheckPermit(" + AdminID.ToString();
            sSQL += " ,'" + Commons.ConvertToString(ScreenID) + "') ";
            DataTable dt = GetData(sSQL);
            return Commons.ConvertToBool(dt.Rows[0][0]);
        }


        // Hàm này có thực hiện cắt các số 0
        // ví dụ 005 sẽ đọc là không trăm linh năm
        //005
        public static string Group32StrX(string num, bool superlevel)
        {
            string[] No = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string kq, tram, chuc, donvi;
            // Trăm
            //1.005.5
            //005
            if (superlevel)
            {
                if (num.Substring(0, 1) == "0" && num.Substring(1, 1) == "0" && num.Substring(2, 1) == "0")
                {

                    tram = "";

                }
                else
                {
                    tram = No[Convert.ToByte(num.Substring(0, 1))] + " trăm ";
                }
            }
            else
            {
                if (num.Substring(0, 1) == "0")
                {
                    tram = "";
                }
                else
                {
                    tram = No[Convert.ToByte(num.Substring(0, 1))] + " trăm ";
                }
            }

            // Chục
            switch (num.Substring(1, 1))
            {
                case "0":
                    if ((num.Substring(2, 1) != "0" && num.Substring(0, 1) != "0") || (num.Substring(2, 1) != "0" && superlevel))
                    {
                        chuc = "lẻ ";
                    }
                    else { chuc = ""; }; break;
                case "1": chuc = "mười "; break;
                default:
                    chuc = No[Convert.ToByte(num.Substring(1, 1))] + " mươi "; break;
            }
            // Đơn vị
            switch (num.Substring(2, 1))
            {
                case "0": donvi = ""; break;
                case "1":
                    if ((num.Substring(1, 1) == "0") || (num.Substring(1, 1) == "1"))
                    {
                        donvi = "một";
                    }
                    else
                    {
                        donvi = "mốt";
                    }; break;
                case "5":
                    if (num.Substring(1, 1) != "0")
                    {
                        donvi = "lăm";
                    }
                    else
                    {
                        donvi = "năm";
                    }; break;
                default:
                    donvi = No[Convert.ToByte(num.Substring(2, 1))]; break;
            }
            kq = tram + chuc + donvi;
            return kq;
        }
        public static string IntNum2Str(string num)
        {
            string[] Cap = { "", " ngàn ", " triệu ", " tỷ ", " ngàn tỷ ", " triệu tỷ ", " tỷ tỷ ", " ngàn tỷ tỷ " };
            string kq = "", str = num, g3, kqtg;
            int caps = 0;
            //1 025 000
            while (str.Length > 3)
            {
                g3 = str.Substring(str.Length - 3, 3);
                str = str.Substring(0, str.Length - 3);
                bool superlevel = str.Length > 0;

                if (g3 != "000")
                { kqtg = Group32StrX(g3, superlevel) + Cap[Convert.ToByte(caps)]; }
                else { kqtg = ""; }
                kq = kqtg + kq;
                caps++;
            }

            //Chuẩn bị trước khi sử dụng hàm Group32Str1
            while (str.Length < 3)
            { str = "0" + str; }

            if ((str == "000") && (num.Length <= 3))
            { kqtg = "không"; }
            else
            { kqtg = Group32StrX(str, false) + Cap[Convert.ToByte(caps)]; }
            kq = kqtg + kq;
            return kq;
        }
        public static string FracNum2Str(string num)
        {
            string[] No = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string kq = "";
            for (int i = 0; i < num.Length; i++)
            {
                kq += No[Convert.ToByte(num.Substring(i, 1))] + " ";
            }
            return kq;
        }
        public static string No2Str(string num)
        {
            string intstr, fracstr, am;
            // Xử lý khi là số âm
            if (num.Substring(0, 1) == "-")
            {
                am = "âm ";
                num = num.Replace("-", "");
            }
            else { am = ""; }

            string[] str = num.Split('.');
            // Số quá lớn nhiều hơn 27 chữ số
            if (str[0].Length > 28)
            {
                throw new Exception("Số lớn quá không hiển thị được! ");
            }

            string s;
            // Xử lý phần số, nếu là có số thập phân hoặc không
            if (str.Length == 2)
            {
                intstr = IntNum2Str(str[0]);
                if (str[1].Length <= 2)
                {
                    if (str[1].Substring(0, 1) == "0")
                    {
                        fracstr = FracNum2Str(str[1]);
                    }
                    else
                    {
                        fracstr = IntNum2Str(str[1]);
                    }
                }
                else
                {
                    fracstr = FracNum2Str(str[1]);
                }
                s = (intstr + " phảy " + fracstr);
            }
            else
            {
                intstr = IntNum2Str(str[0]);
                s = intstr;
            }
            s = am + s;
            string chuhoa = s.Substring(0, 1).ToUpper();
            s = s.Substring(1, s.Length - 1);
            return (chuhoa + s);
        }
        public static string NumNormalize(string num)
        {
            char ThousandSpace = ',';
            char PointScape = '.';
            string[] s = num.Split('.');
            string g3, str = "";
            while (s[0].Length > 3)
            {
                g3 = s[0].Substring(s[0].Length - 3, 3);
                s[0] = s[0].Substring(0, s[0].Length - 3);
                str = ThousandSpace + g3 + str;
            }
            str = s[0] + str;
            if (s.Length == 2)
            {
                str = (str + PointScape + s[1]);
            }
            return str.Replace("-,", "-");
        }
        public static string NumberToString(string no)
        {

            // xử lý trường hợp dáu phảy thay cho dấu chấm
            if (no.IndexOf(",", 0, 1) != 0) { no = no.Replace(",", "."); }
            // Xoá các ký tự trắng ở đầu và cuối
            no = no.Trim();
            // Xử lý khi nó là chữ chứ không phải là số
            if (no == "0")
            {
                return "không";
            }

            if (no == "")
            {
                return "không";
            }

            double val;
            try
            {
                val = Convert.ToDouble(no);
            }
            catch
            {
                throw new Exception("Đây không phải là số");
            }
            // tiêu diệt các số không
            while (no.Substring(0, 1) == "0")
            {
                no = no.Substring(1, no.Length - 1);
            }

            if (no.IndexOf(".", 0, 1) != -1)
            {
                while (no.Substring(no.Length - 1, 1) == "0")
                {
                    no = no.Substring(0, no.Length - 1);
                }
            }
            no = No2Str(no);

            return no;
        }
        public static string NumberToStringTP(string no)
        {
            // xử lý trường hợp dáu phảy thay cho dấu chấm
            if (no.IndexOf(",", 0, 1) != 0) { no = no.Replace(",", "."); }
            // Xoá các ký tự trắng ở đầu và cuối
            no = no.Trim();
            // Xử lý khi nó là chữ chứ không phải là số
            if (no == "0")
            {
                return "không";
            }

            if (no == "")
            {
                return "không";
            }

            double val;
            try
            {
                val = Convert.ToDouble(no);
            }
            catch
            {
                throw new Exception("Đây không phải là số");
            }
            // tiêu diệt các số không
            while (no.Substring(0, 1) == "0")
            {
                no = no.Substring(1, no.Length - 1);
            }

            if (no.IndexOf(".", 0, 1) != -1)
            {
                while (no.Substring(no.Length - 1, 1) == "0")
                {
                    no = no.Substring(0, no.Length - 1);
                }
            }
            no = No2Str(no);

            return no;
        }
        ////////////////Hàm khác
        /////////////
        public static string WriteNum(Int64 num)
        {
            byte i;
            long sochia, T1, T2, T3;
            long luu;
            string st = "";
            string[] hang;
            string[] donvi;
            hang = new string[10];
            hang[0] = "";
            hang[1] = " một";
            hang[2] = " hai";
            hang[3] = " ba";
            hang[4] = " bốn";
            hang[5] = " năm";
            hang[6] = " sáu";
            hang[7] = " bảy";
            hang[8] = " tám";
            hang[9] = " chín";
            donvi = new string[4];
            donvi[0] = " tỉ";
            donvi[1] = " triệu";
            donvi[2] = " ngàn";
            donvi[3] = "";
            sochia = 1000000000;

            for (i = 0; i < 4; i++)
            {
                luu = num;
                luu = luu / sochia % 1000;
                T1 = luu / 100;
                T2 = luu / 10 % 10;
                T3 = luu % 10;
                if (T1 == 0 && T2 == 0 && T3 == 0)
                {
                    sochia = sochia / 1000;
                }
                else
                {
                    if (T1 == 0 && st != "") { st = st + " không"; }
                    st = st + hang[T1];
                    if (st != "") { st = st + " trăm"; }
                    if (T2 != 0)
                    {
                        if (T2 == 1)
                        {
                            st = st + " mười";
                            if (T3 != 5) { st = st + hang[T3]; }
                            if (T3 == 5) { st = st + " lăm"; }
                        }
                        else
                        {
                            st = st + hang[T2] + " mươi";
                            if (T3 != 1 && T3 != 5) { st = st + hang[T3]; }
                            if (T3 == 1 && T2 > 1) { st = st + " mốt"; }
                            if (T3 == 5) { st = st + " lăm"; }
                        }
                    }
                    else
                    {
                        if (T3 != 0 && st != "") { st = st + " linh"; }
                        st = st + hang[T3];
                    }

                    if (st != "") { st = st + donvi[i]; }
                    sochia = sochia / 1000;
                }
            }
            return st.Trim();
        }



        public static string NotShowIfZero(object v)
        {
            decimal value = Commons.ConvertToDecimal(v);
            if (value == 0)
            {
                return "";
            }

            return value.ToString("N0");
        }



        public static string GetIPAddress()
        {
            string stringIpAddress = "";
            try
            {
                stringIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (stringIpAddress == null)
                {
                    stringIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch (Exception ex)
            {

                stringIpAddress = ex.Message;
            }

            return stringIpAddress;
        }
        public static bool HasBlackList()
        {
            string IP = GetIPAddress();
            string sSQL = "select top 1 IP from BlackList where IP=N'" + Fix(IP) + "' ";
            DataTable dt = Commons.GetData(sSQL);
            return dt.Rows.Count > 0;
        }

        public static string GetIcon(string path)
        {
            string sResult = "mdi mdi-file";
            int p = path.LastIndexOf(".");
            if (p < 0)
            {
                return sResult;
            }

            string ext = path.Substring(p + 1).ToLower();
            switch (ext)
            {
                case "xls":
                    sResult = "mdi mdi-file-excel";
                    break;
                case "xlsx":
                    sResult = "mdi mdi-file-excel";
                    break;
                case "doc":
                    sResult = "mdi mdi-file-word-box";
                    break;
                case "docx":
                    sResult = "mdi mdi-file-word-box";
                    break;
                case "pdf":
                    sResult = "mdi mdi-file-pdf-box";
                    break;
                case "png":
                    sResult = "mdi mdi-file-image";
                    break;
                case "jpg":
                    sResult = "mdi mdi-file-image";
                    break;
                case "jpeg":
                    sResult = "mdi mdi-file-image";
                    break;

                case "bmp":
                    sResult = "mdi mdi-file-image";
                    break;
                case "ttf":
                    sResult = "mdi mdi-file-image";
                    break;
                case "ppt":
                    sResult = "mdi mdi-file-powerpoint";
                    break;
                case "pptx":
                    sResult = "mdi mdi-file-powerpoint";
                    break;
                case "rar":
                    sResult = "mdi mdi-package-variant";
                    break;
                case "zip":
                    sResult = "mdi mdi-package-variant";
                    break;
                case "mp4":
                    sResult = "mdi mdi-file-video";
                    break;
                case "wmv":
                    sResult = "mdi mdi-file-video";
                    break;
                default:
                    break;
            }

            return sResult;
        }
        public static string GetBG(string path)
        {
            path = path.ToLower();
            string sReult = "";
            if (IsImage(path))
            {
                sReult = "background-image:url('" + path.Replace("~/files", "/files/thumbs") + "');background-size: cover";
            }

            return sReult;
        }
        public static bool IsImage(string file)
        {
            bool result = false;
            file = file.ToLower();
            string ext = file.Substring(file.LastIndexOf(".") + 1);
            switch (ext)
            {
                case "jpg": result = true; break;
                case "jpeg": result = true; break;
                case "png": result = true; break;
                case "bmp": result = true; break;

                default:
                    break;
            }
            return result;

        }
        public static int AccessCount
        {
            get
            {
                string sSQL = "select top 1 [count] c from Access ";
                DataTable dt = GetData(sSQL);
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0]);
                }

                return 0;
            }
        }


        public static string GetQRImageFromID(object ItemID)
        {
            return "https://chart.googleapis.com/chart?cht=qr&choe=UTF-8&chs=170x170&chl=" + ItemID.ToString();
        }
        public static string GetQRImageFromLink(object ItemID)
        {
            return "https://chart.googleapis.com/chart?cht=qr&choe=UTF-8&chs=170x170&chl=https://app.bitis.com.vn/Home/AddQR?id=" + ItemID.ToString();
        }

        public static DataTable GetDivision()
        {
            DataTable dt = GetData("exec SP_GetDivision");
            return dt;
        }
        public static DataTable GetSlots()
        {
            DataTable dt = GetData("exec SP_GetSlots");
            return dt;
        }
        public static DataTable GetPalletDetail(string PalletID)
        {
            DataTable dt = GetData("exec SP_GetPalletDetail '" + Fix(PalletID) + "',N'" + Fix(GlobalVariables.DivisionID) + "'");
            return dt;
        }
        public static DataTable GetPalletOutBound(string PalletID)
        {
            DataTable dt = GetData("exec SP_GetOutBound N'" + Fix(GlobalVariables.DivisionID) + "','" + Fix(PalletID) + "'");

            return dt;
        }
        public static DataTable GetChiTietDau8BiThieu(string OB, string VoucherID)
        {
            DataTable dt = GetData("exec SP_GetOutBoundDetail '" + Fix(OB) + "','" + Fix(VoucherID) + "', N'" + Fix(GlobalVariables.DivisionID) + "'");

            return dt;
        }
        public static string YMDToDMY(object e)
        {
            if (e == null)
            {
                return "";
            }

            string p = ConvertToString(e);
            string[] list = p.Split('-');
            if (list.Length < 3)
            {
                return p;
            }

            return "Ngày " + list[2] + " tháng " + list[1] + " năm " + list[0];
        }
        public static DataTable GetChiTietDau8Full(string OB, string VoucherID)
        {
            DataTable dt = GetData("exec SP_GetOutBoundDetailFull '" + Fix(OB) + "','" + Fix(VoucherID) + "', N'" + Fix(GlobalVariables.DivisionID) + "'");

            return dt;
        }

    }
}
