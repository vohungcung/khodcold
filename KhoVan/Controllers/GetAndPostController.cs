using ApiOrder.Models;
using Global;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace MvcApplication5.Controllers
{
    public class GetAndPostController : Controller
    {

        //public string domain = "http://apioder.bitis-corp.com/";
        //http://apioder.bitis-corp.com/
        //public string domain = "http://192.168.23.1:9090/";
        public string domain
        {
            get
            {
                return Commons.APIHost;
            }
        }
        //public string Key = "abf@aoiRyfh@1938Fjadjfkgj";
        public string Key
        {
            get
            {
                return Commons.APIHostKey;
            }
        }
        public int TimeOut = 300;
        public int TimeOutMilisecond = 10000;


        public bool DownloadOutBound(string OutBound)
        {
            if (Global.Commons.CheckPermit("ql") == false)
            {
                return false;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = domain + "/api/OrderNumber/GetInfoDau8/" + OutBound;
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("APIKey", Key);
                    client.Timeout = TimeSpan.FromSeconds(TimeOut);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string reponse = client.GetStringAsync(url).Result;

                    List<Dau8> rs = JsonConvert.DeserializeObject<List<Dau8>>(reponse);

                    string sWrite = "delete outbound where outbound='" + Commons.Fix(OutBound) + "';";

                    foreach (Dau8 item in rs)
                    {
                        sWrite += "exec SP_InsertOutBound ";
                        sWrite += " N'" + Commons.Fix(item.VBELN) + "'";
                        sWrite += ",N'" + Commons.Fix(item.MATNR) + "'";
                        sWrite += "," + int.Parse(item.LFIMG.Replace(",", "").Replace(".", "")).ToString("0");
                        sWrite += ",N'" + Commons.Fix(item.VGBEL) + "'";
                        sWrite += ",N'" + Commons.Fix(item.MEINS) + "'";
                        sWrite += ";";


                    }
                    if (rs.Count == 0)
                        return false;

                    Commons.ExecuteNoneQuery(sWrite);
                   
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return true;
        }

        public Dau8[] GetOutBound(string OutBound, int lay = 0)
        {
            OutBound = OutBound.Trim();

            List<Dau8> results = new List<Dau8>();
            string[] list = OutBound.Split('\n');
            foreach (string item in list)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string url = domain + "/api/OrderNumber/GetInfoDau8/" + item;
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Add("APIKey", Key);
                        client.Timeout = TimeSpan.FromSeconds(TimeOut);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string reponse = client.GetStringAsync(url).Result;

                        List<Dau8> rs = JsonConvert.DeserializeObject<List<Dau8>>(reponse);
                        results.AddRange(rs);
                        if (lay == 1)
                        {
                            if (rs.Count > 0)
                            {
                                string sWrite = "delete DataOBDetail where OB=N'" + Commons.Fix(item) + "' ;";
                                sWrite += "exec SP_InsertDataOB N'" + Commons.Fix(item) + "','" + Commons.Fix(rs[0].WADAT) + "';";
                                foreach (Dau8 ii in rs)
                                {
                                    sWrite += "exec SP_InsertDataOBDetail N'" + Commons.Fix(ii.VBELN) + "','" + Commons.Fix(ii.MATNR) + "'," + ii.LFIMG.Replace("'", "").Replace(",", "") + ";";

                                }
                                Commons.ExecuteNoneQuery(sWrite);
                            }
                        }

                        else if (lay==2)
                        {
                            if (rs.Count > 0)
                            {
                                string sWrite = "";
                                sWrite += "exec SP_InsertDataOB N'" + Commons.Fix(item) + "','" + Commons.Fix(rs[0].WADAT) + "';";
                              
                                Commons.ExecuteNoneQuery(sWrite);
                            }

                        }

                    }
                }
                catch
                {


                }
            }

            return results.ToArray();
        }
        public Dau8[] GetOutBoundNotSave(string OutBound)
        {
            OutBound = OutBound.Trim();

            List<Dau8> results = new List<Dau8>();
            string[] list = OutBound.Split('\n');
            foreach (string item in list)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string url = domain + "/api/OrderNumber/GetInfoDau8/" + item;
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Add("APIKey", Key);
                        client.Timeout = TimeSpan.FromSeconds(TimeOut);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string reponse = client.GetStringAsync(url).Result;

                        List<Dau8> rs = JsonConvert.DeserializeObject<List<Dau8>>(reponse);
                        results.AddRange(rs);
                        

                    }
                }
                catch
                {


                }
            }

            return results.ToArray();
        }

        public Dau8[] GetOutBound(System.Collections.ArrayList OutBound)
        {

            List<Dau8> results = new List<Dau8>();
            foreach (string item in OutBound)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string url = domain + "/api/OrderNumber/GetInfoDau8/" + item;
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Add("APIKey", Key);
                        client.Timeout = TimeSpan.FromSeconds(TimeOut);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string reponse = client.GetStringAsync(url).Result;

                        List<Dau8> rs = JsonConvert.DeserializeObject<List<Dau8>>(reponse);
                        results.AddRange(rs);


                    }
                }
                catch
                {


                }
            }

            return results.ToArray();
        }
        public Dau8[] GetOutBoundForOne(string OutBound, int lay = 0)
        {
            OutBound = OutBound.Trim();
            List<Dau8> results = new List<Dau8>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = domain + "/api/OrderNumber/GetInfoDau8/" + OutBound;
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("APIKey", Key);
                    client.Timeout = TimeSpan.FromSeconds(TimeOut);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string reponse = client.GetStringAsync(url).Result;

                    List<Dau8> rs = JsonConvert.DeserializeObject<List<Dau8>>(reponse);
                    results.AddRange(rs);
                    if (lay == 1)
                    {
                        if (rs.Count > 0)
                        {
                            string sWrite = "delete DataOBDetail where OB=N'" + Commons.Fix(OutBound) + "' ;";
                            sWrite += "exec SP_InsertDataOB N'" + Commons.Fix(OutBound) + "','" + Commons.Fix(rs[0].WADAT) + "';";
                            foreach (Dau8 ii in rs)
                            {
                                sWrite += "exec SP_InsertDataOBDetail N'" + Commons.Fix(ii.VBELN) + "','" + Commons.Fix(ii.MATNR) + "'," + ii.LFIMG.Replace("'", "").Replace(",", "") + ";";

                            }
                            Commons.ExecuteNoneQuery(sWrite);
                        }
                    }
                }
            }
            catch
            {


            }


            return results.ToArray();
        }
        [HttpPost]
        public ActionResult DownloadCustomer()
        {
            if (Global.Commons.CheckPermit("viewcustomers") == false)
                return Json(new { errorMsg = "Bạn không có quyền", success = false });

            Exception ex11 = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = domain + "/api/Article/GetListCusomterAll";
                    if (Commons.HasBlackList()) return Json(new { errorMsg = "IP này bị chặn", success = false });
                    //Commons.LogAPI(url);
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("APIKey", Key);
                    client.Timeout = TimeSpan.FromSeconds(TimeOut);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var reponse = client.GetStringAsync("/api/CheckArticle/GetMahhError").Result;
                    var reponse = client.GetStringAsync(url).Result;

                    List<CustomerMap> rs = JsonConvert.DeserializeObject<List<CustomerMap>>(reponse);

                    string sWrite = "";
                    int i = 0;
                    foreach (CustomerMap item in rs)
                    {
                        sWrite += "exec SP_InsertCustomer ";
                        sWrite += " N'" + Commons.Fix(item.KUNNR) + "'";//ma khach
                        sWrite += ",N'" + Commons.Fix(item.NAME1) + "'";//ten khach
                        sWrite += ",N'" + Commons.Fix(item.ADDRESS_CUST) + "'";//dia chi
                        
                        sWrite += ",N'" + Commons.Fix(item.LIFNR) + "'";
                        sWrite += ",N'" + Commons.Fix(item.NAME2) + "'";
                        sWrite += ";";
                        i++;
                        if (i % 50 == 0)
                        {

                            Commons.ExecuteNoneQuery(sWrite, ref ex11);
                            if (ex11 != null)
                                return Json(new { errorMsg = ex11.Message, success = false });

                            sWrite = "";
                        }
                    }
                    if (sWrite != "")
                    {
                        Commons.ExecuteNoneQuery(sWrite, ref ex11);
                        if (ex11 != null)
                            return Json(new { errorMsg = ex11.Message, success = false });
                    }
                 }
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message, success = false });


            }
            return Json(new { msg = "Cập nhật thành công", success = true });


        }
        public bool TranToSAP(string OB, int SB, int ST)
        {


            try
            {


                ApiOrder.Models.OBPost PO = new ApiOrder.Models.OBPost();
                PO.S_VBELN = OB;
                PO.S_CARTON = ST.ToString("0");
                PO.S_PACKAGE = SB.ToString("0");
                string url = domain + "/api/OrderNumber/InsertOB";


                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers.Clear();
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("APIKey", Key);
                httpWebRequest.Timeout = TimeOutMilisecond;

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(PO);
                    //System.IO.File.WriteAllText("d:\\json.txt", json.Replace("},", "},\n").Replace("],", "],\n"));
                    streamWriter.Write(json);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    if (result.IndexOf("200") > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {

                return false;


            }


        }
    }


}
