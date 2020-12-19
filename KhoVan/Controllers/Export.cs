using Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MvcApplication5.Controllers
{
    public class Export
    {


        internal void ToExcel(HttpResponseBase Response, System.Data.DataTable dt, string FileName)
        {
            ExportExcel(Response, FileName, dt);
          
        }
        public bool IsNumber(object v)
        {
            bool r = true;
            string o = Commons.ConvertToString(v);

            foreach (char item in o.ToCharArray())
            {
                if (item < '0' || item > '9')
                {
                    r = false;
                    break;
                }
            }
            return r;
        }
        public void ExportExcel(HttpResponseBase Response, string filename, System.Data.DataTable dt)
        {

            Response.Clear();

            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename + ".xls"));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Charset = "UTF-8";
            Response.ContentType = "application/ms-excel";

            //chuan hoa du lieu
            System.Collections.ArrayList cotngay = new System.Collections.ArrayList();

            int c = 0;
            foreach (DataColumn item in dt.Columns)
            {
                if (item.DataType.ToString() == "System.DateTime")
                {
                    cotngay.Add(c);
                }

                c++;
            }
            DataTable db = new DataTable();

            if (cotngay.Count > 0)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    if (cotngay.IndexOf(col) < 0)
                        db.Columns.Add(dt.Columns[col].ColumnName, dt.Columns[col].DataType);
                    else
                        db.Columns.Add(dt.Columns[col].ColumnName, "".GetType());

                }

                int nSoCot = dt.Columns.Count;
                foreach (DataRow item in dt.Rows)
                {
                    DataRow r = db.NewRow();
                    for (int col = 0; col < nSoCot; col++)
                    {
                        if (cotngay.IndexOf(col) >= 0)
                        {
                            if (item[col] == null || Commons.ConvertToString(item[col]) == "")
                                r[col] = "";
                            else
                                r[col] = Commons.ConvertToDateTime(item[col]).ToString("dd/MM/yyyy HH:mm");
                        }
                        else
                            r[col] = item[col];
                    }
                    db.Rows.Add(r);

                }
            }


            DateTime rrr = DateTime.Now;
            string stime = rrr.Year.ToString("0000") + rrr.Month.ToString("00") + rrr.Day.ToString("00");

            string dir = HttpContext.Current.Server.MapPath("~/excel/" + GlobalVariables.DivisionID + "/" + GlobalVariables.UserID.ToString("0"));
            if (System.IO.Directory.Exists(dir) == false)
                System.IO.Directory.CreateDirectory(dir);


            string[] files = System.IO.Directory.GetFiles(dir);

            foreach (string item in files)
            {
                try
                {
                    if (item.IndexOf(stime) < 0)
                        System.IO.File.Delete(item);
                }
                catch
                {

                }
            }

            string f = dir + "\\" + filename + stime + ".xlsx";
            string newname = filename + stime;
            int i = 1;
            while (System.IO.File.Exists(f))
            {
                newname = filename + stime + i;
                f = dir + "\\" + newname + ".xlsx";
                i++;
            }
            string v = "~/excel/" + GlobalVariables.DivisionID + "/" + GlobalVariables.UserID.ToString("0") + "/" + newname + ".xlsx";


            if (cotngay.Count > 0)
                DataTableToExcel.DataTableToExcel.ExportToExcel(f, "data", db);
            else
                DataTableToExcel.DataTableToExcel.ExportToExcel(f, "data", dt);

            Response.Redirect(v);

            Response.End();
        }
    }
}