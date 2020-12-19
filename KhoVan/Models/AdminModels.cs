using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace MvcApplication5.Models
{

    public class AdminModel
    {
        [Required]
        [Display(Name = "Mã quản trị")]
        public int AdminID { get; set; }

        [Required]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Nhập lại mật khẩu")]
        public string RePassword { get; set; }
        [Required]
        [Display(Name = "ZIPCode")]
        public string ZIPCode { get; set; }

        [Display(Name = "Là quản trị")]
        public string IsAdmin { get; set; }

    }
    public class CustomerModel
    {
        [Required]
        [Display(Name = "Mã khách hàng")]
        public string CustomerID { get; set; }

        [Required]
        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Quận huyện")]
        public string District { get; set; }

        [Required]
        [Display(Name = "Loại khách")]
        public string CustomerType { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Xác nhận lại mật khẩu")]
        public string RetypePassword { get; set; }

        [Required]
        [Display(Name = "Khu vực")]
        public string Area { get; set; }
        [Required]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "ZO1")]
        public bool ZO1 { get; set; }
        [Required]
        [Display(Name = "ZOD")]
        public bool ZOD { get; set; }
        [Required]
        [Display(Name = "ZO2")]
        public bool ZO2 { get; set; }
        [Required]
        [Display(Name = "ZOC")]
        public bool ZOC { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ItemModel
    {
        [Required]
        [Display(Name = "Mã hàng")]
        public string ItemID { get; set; }

        [Required]
        [Display(Name = "Tên hàng")]
        public string ItemName { get; set; }
        [Required]
        [Display(Name = "ĐVT")]
        public string UoM { get; set; }
        [Required]
        [Display(Name = "MC")]
        public string MC { get; set; }

        [Required]
        [Display(Name = "Tồn tham chiếu trên SAP")]
        public decimal StockSAP { get; set; }

        [Required]
        [Display(Name = "Số lượng đặt")]
        public decimal OrderQuantity { get; set; }

        [Required]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }


        [Required]
        [Display(Name = "Hình đại diện")]
        public string ThumbImage { get; set; }

        [Required]
        [Display(Name = "Loại hàng")]
        public string ItemType { get; set; }
        [Required]
        [Display(Name = "Là hàng mới")]
        public int IsNew { get; set; }
        [Required]
        [Display(Name = "Giới tính")]
        public int Sex { get; set; }

    }
    public class OrderTypeModel
    {
        [Required]
        [Display(Name = "Mã loại")]
        public string ID { get; set; }

        [Required]
        [Display(Name = "Tên loại hàng")]
        public string SAPType { get; set; }
        [Required]
        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Mã kho")]
        public string WareHouseID { get; set; }

    }
    public class OrderModel
    {
        [Required]
        [Display(Name = "Mã đơn hàng")]
        public string OrderID { get; set; }
        [Required]
        [Display(Name = "Mã khách hàng")]
        public string CustomerID { get; set; }
        [Required]
        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }
        [Required]
        [Display(Name = "Loại đơn hàng")]
        public string OrderType { get; set; }
        [Required]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DeliveryAddress { get; set; }
        [Required]
        [Display(Name = "Quận huyện")]
        public string District { get; set; }
        [Required]
        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [Required]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Số đơn hàng tham chiếu")]
        public string SAPOrderNumber { get; set; }
        [Required]
        [Display(Name = "Số phiếu soạn hàng")]
        public string Ref2 { get; set; }
        [Required]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
        [Required]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }
        [Required]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }
        [Required]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }

    }
    public class CustomerTypeModel
    {
        [Required]
        [Display(Name = "Mã loại khách hàng")]
        public string CustomerTypeID { get; set; }

        [Required]
        [Display(Name = "Tên loại khách hàng")]
        public string CustomerTypeName { get; set; }


    }
    public class Export
    {
        public void ToExcel(HttpResponseBase Response, object clientsList)
        {
            var grid = new System.Web.UI.WebControls.GridView();

            grid.DataSource = clientsList;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=FileName.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        internal void ToExcel(HttpResponseBase Response, System.Data.DataTable dt, int colimage, int height)
        {
            var grid = new System.Web.UI.WebControls.GridView();
            grid.AutoGenerateColumns = false;
            grid.DataSource = dt;

            System.Web.UI.WebControls.BoundField col1 = new System.Web.UI.WebControls.BoundField();
            col1.DataField = dt.Columns[0].ColumnName;
            grid.Columns.Add(col1);

            System.Web.UI.WebControls.ImageField img = new System.Web.UI.WebControls.ImageField();
            img.DataImageUrlField = dt.Columns[colimage].ColumnName;
            grid.Columns.Add(img);
          
            grid.Columns[0].HeaderStyle.Width = 100;
            grid.Columns[dt.Columns.Count - 1].HeaderStyle.Width = 300;
            grid.Columns[1].HeaderStyle.Width = 200;
            grid.Columns[1].ItemStyle.Height = height;
            grid.DataBind();


            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=FileName.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}
