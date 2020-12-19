using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiOrder.Models
{
    public class Dau8
    {
        public string VBELN { get; set; } //đầu 8
        public string VGBEL { get; set; } //đầu SO/STO
        public string MATNR { get; set; } //mã hàng
        public string LFIMG { get; set; } //số lượng
        public string MEINS { get; set; } //DVT
        public string ERDAT { get; set; } //ngày soạn hàng
        public string KBETR { get; set; } //đơn giá
        public string DISC { get; set; } //chiết khấu
        public string LGOBE { get; set; } //kho
        public string NAME { get; set; } //TÊN KH
        public string ADDRESS { get; set; } //DIA CHI
        public string REGION { get; set; } //QUẬN HUYỆN
        public string VTEXT { get; set; } //lOẠI HÌNH
        public string HOPDONG { get; set; } //SỐ HỢP ĐỒNG
        public string DIENGIAI { get; set; } //DIEN GIẢI
        public string MATKL { get; set; } //MC
        public string WGBEZ { get; set; } //diễn giải MC
        public string KUNNR { get; set; } //mã khách
        public string WADAT { get; set; } //ngày kế hoạch xuất hàng
    }
}