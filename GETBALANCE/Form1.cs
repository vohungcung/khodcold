using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace GetBalance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        System.Threading.Thread d;
        bool Error = false;
        private string Key
        {
            get
            {
                try
                {
                    string path = Application.StartupPath + "\\key.txt";
                    if (System.IO.File.Exists(path) == false)
                    {
                        WriteLog("Không tồn tại file key.txt", " get key");
                        Application.Exit();
                    }

                    string[] lines = System.IO.File.ReadAllLines(path);
                    return lines[0];
                }
                catch (Exception ex)
                {
                    WriteLog(ex, "get key");
                    Application.Exit();
                }
                return "";
            }
        }
        private int TimeOut
        {
            get
            {
                try
                {
                    string path = Application.StartupPath + "\\time.txt";
                    if (System.IO.File.Exists(path) == false)
                    {
                        WriteLog("Không tồn tại file time.txt", " get time");
                        Application.Exit();
                    }

                    string[] lines = System.IO.File.ReadAllLines(path);
                    return ToInt(lines[0]);
                }
                catch (Exception ex)
                {
                    WriteLog(ex, "get time");
                    Application.Exit();
                }
                return 3000;
            }
        }
        private string Domain
        {
            get
            {
                try
                {
                    string path = Application.StartupPath + "\\domain.txt";
                    if (System.IO.File.Exists(path) == false)
                    {
                        WriteLog("file domain.txt is not exists", " get domain");
                        Application.Exit();
                    }

                    string[] lines = System.IO.File.ReadAllLines(path);
                    return lines[0];
                }
                catch (Exception ex)
                {
                    WriteLog(ex, "get domain");
                    Application.Exit();
                }
                return "";
            }
        }
        public string LogURL => Application.StartupPath + "\\" + DateTime.Now.ToString("yyyy.MM.dd") + ".txt";


        private void WriteLog(Exception ex, string From)
        {
            Error = true;
            System.IO.File.AppendAllText(LogURL, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " " + ex.Message + " - from " + From + Environment.NewLine);
        }
        private void WriteLog(string ex, string Form)
        {
            Error = true;
            System.IO.File.AppendAllText(LogURL, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " " + ex + " from " + Form + Environment.NewLine);
        }
        private void WriteLog(string m)
        {
          
            System.IO.File.AppendAllText(LogURL, m + Environment.NewLine);
        }
        public string Fix(object v)
        {
            if (v == null)
            {
                return "";
            }

            return v.ToString().Replace("'", "''").Trim();
        }
        private void DownloadItem()
        {
            label1.Text = "Item downloading...";



            ExecuteNoneQuery("exec SP_ImportAll");
            label1.Text = "Item updated -  Done";

        }
        private void DownloadBalance()
        {

            string path = Application.StartupPath + "\\warehouse.txt";
            if (System.IO.File.Exists(path) == false)
            {
                WriteLog("File warehouse.txt is not exists", "DownloadBalance");
                Application.Exit();
                return;
            }

            string[] lines = System.IO.File.ReadAllLines(path);
            ExecuteNoneQuery("truncate table balances");
            foreach (string url in lines)
            {
                if (url.Trim() != "")
                {
                    label1.Text = url + " downloading...";

                    DownloadBalance(url);



                }
            }
        }
        private void DownloadBalance(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {


                    //Commons.ExecuteNoneQuery("exec WriteLog '" + url + "'");
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("APIKey", Key);
                    client.Timeout = TimeSpan.FromMinutes(TimeOut);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var reponse = client.GetStringAsync("/api/CheckArticle/GetMahhError").Result;
                    string reponse = client.GetStringAsync(url).Result;

                    List<BalanceMap> rs = JsonConvert.DeserializeObject<List<BalanceMap>>(reponse);
                    DataTable dt = GetSourceData("select WareHouseID, Site, ItemID, Quantity from Balances where 1=2 ");
                    textBox4.Text += "Balance Downloaded from " + url + Environment.NewLine;
                    WriteLog("Balance Downloaded from " + url);

                    foreach (BalanceMap item in rs)
                    {
                        DataRow r = dt.NewRow();
                        r["WareHouseID"] = ToString(item.Sloc);
                        r["Site"] = ToString(item.Site);
                        r["ItemID"] = ToString(item.ItemId);
                        r["Quantity"] = ToInt(item.TonKho);
                        dt.Rows.Add(r);
                    }
                    using (SqlConnection destinationConnection = new SqlConnection(MyConnectionString))
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                    {
                        destinationConnection.Open();
                        bulkCopy.DestinationTableName = "Balances";
                        bulkCopy.WriteToServer(dt);
                        destinationConnection.Close();

                    }
                    System.IO.File.AppendAllText(LogURL, url + " - Done " + rs.Count.ToString("N0") + " records" + Environment.NewLine);
                    label1.Text = url + " - Done " + rs.Count.ToString("N0") + " records";
                    textBox4.Text += "Has " + rs.Count.ToString("N0") + Environment.NewLine;
                    WriteLog("Has " + rs.Count.ToString("N0"));

                }
            }
            catch (Exception ex)
            {
                WriteLog(ex, "download balance " + url + Environment.NewLine);
                label1.Text = url + " " + ex.Message;
            }

        }
        private string ToString(object v)
        {
            if (v == null)
            {
                return "";
            }

            return v.ToString();
        }
        private int ToInt(object v)
        {
            try
            {
                if (v == null)
                {
                    return 0;
                }

                return Convert.ToInt32(v);
            }
            catch
            {

                return 0;
            }

        }
        private decimal ToDecimal(object v)
        {
            try
            {
                if (v == null)
                {
                    return 0;
                }

                return Convert.ToDecimal(v);
            }
            catch
            {

                return 0;
            }

        }
        private void DownloadCustomer()
        {
            try
            {
                string sWrite = "";
                int n = 0, i = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = n;
                progressBar1.Value = 0;

                label1.Text = "Customer Downloading....";
                textBox4.Text += "Customer Downloading...."+Environment.NewLine;
                //download customer
                using (HttpClient client = new HttpClient())
                {
                    string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + "\\Customer.txt");
                    string url = lines[0];


                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("APIKey", Key);
                    client.Timeout = TimeSpan.FromMinutes(TimeOut);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var reponse = client.GetStringAsync("/api/CheckArticle/GetMahhError").Result;
                    string reponse = client.GetStringAsync(url).Result;

                    List<CustomerMap> rs = JsonConvert.DeserializeObject<List<CustomerMap>>(reponse);
                    textBox4.Text += "Customers "+rs.Count.ToString("N0")+" records" + Environment.NewLine;

                    sWrite = "";
                    i = 0;
                    label1.Text = "Customers was Copied 0 / " + n.ToString("N0");
                    n = rs.Count;
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = n;
                    progressBar1.Value = 0;
                    foreach (CustomerMap item in rs)
                    {
                        sWrite = "";
                        sWrite += "exec SP_InsertOrUpdateCustomer ";
                        sWrite += " N'" + Fix(item.KUNNR) + "'";//ma khach
                        sWrite += ",N'" + Fix(item.NAME1) + "'";//ten khach
                        sWrite += ",N'" + Fix(item.ADDRESS_CUST) + "'";//dia chi
                        sWrite += ",N'" + Fix(item.KONDA) + "'";//loai hinh

                        sWrite += ",N'" + Fix(item.TEL_NUMBER) + "'";//dien thoai
                        sWrite += ",N'" + Fix(item.BZIRK) + "'";//khu vuc
                        sWrite += ",N'" + Fix(item.VWERK) + "'";//site
                        sWrite += ",N'" + Fix(item.QUANHUYEN) + "'";//QUAN HUYEN

                        sWrite += ",N'" + Fix(item.LIFNR) + "'";//ma tuyen
                        sWrite += ",N'" + Fix(item.NAME2) + "'";//ten tuyen
                        sWrite += ",N'" + Fix(item.EMAIL) + "'";//Email
                        sWrite += ";";
                        i++;
                        //if (i % 50 == 0)
                        //{

                        ExecuteNoneQuery(sWrite);
                        label1.Text = "Customers was Copied " + i.ToString("N0") + " / " + n.ToString("N0") + " (" + (i * 1.0 / n * 100).ToString("N2") + "%)";

                        progressBar1.Value = i;

                        //}


                    }
                    if (sWrite != "")
                    {
                        ExecuteNoneQuery(sWrite);

                    }
                    progressBar1.Value = i;
                    label1.Text = "Customers was Copied " + i.ToString("N0") + " / " + n.ToString("N0");
                    System.IO.File.AppendAllText(LogURL, "Customers was Copied " + rs.Count.ToString("N0") + " records" + Environment.NewLine);
                }



            }
            catch (Exception ex)
            {
                WriteLog(ex, "Download Customer");

            }

        }
        private void DownloadDiscount()
        {
            bool b = false;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sSQL = "select ID from OrderTypes order by 1";
            DataTable dtContractType = GetMyData(sSQL);
            sSQL = "select CustomerType from Customers group by CustomerType";
            DataTable dtCustomerType = GetMyData(sSQL);
            label1.Text = "Download discount programs";
            ExecuteNoneQuery("truncate table DiscountData");
            int c = 0;
            foreach (DataRow CustomerType in dtCustomerType.Rows)
            {
                foreach (DataRow ContractType in dtContractType.Rows)
                {
                    string Contract = ContractType[0].ToString();
                    string Customer = CustomerType[0].ToString();
                    if (Customer == "")
                    {
                        continue;
                    }
                    string url = "";
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            bool ok = true;

                            url = Domain + "/api/Article/GetDiscount/{date}/{type}/{konda}";
                            url = url.Replace("{date}", date);
                            url = url.Replace("{type}", Contract);
                            url = url.Replace("{konda}", Customer);
                            client.BaseAddress = new Uri(url);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Add("APIKey", Key);
                            client.Timeout = TimeSpan.FromSeconds(TimeOut);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            string reponse = client.GetStringAsync(url).Result;

                            List<DiscountMap> rs = JsonConvert.DeserializeObject<List<DiscountMap>>(reponse);
                            textBox4.Text += "Discount Downloaded from "+url + Environment.NewLine;
                            textBox4.Text += "Has " + rs.Count.ToString("N0") + Environment.NewLine;

                            string sWrite = "";
                            int i = 0;
                            foreach (DiscountMap item in rs)
                            {

                                sWrite += "exec SP_InsertDiscountData ";
                                sWrite += "N'" + Fix(item.MATKL) + "'";
                                sWrite += ",N'" + Fix(item.WGBEZ) + "'";
                                sWrite += "," + item.DISCOUNT.ToString().Replace(",", "");
                                sWrite += ",N'" + Fix(item.DATBI) + "'";
                                sWrite += ",N'" + Fix(item.DATAB) + "'";
                                sWrite += ",N'" + Fix(Contract) + "'";
                                sWrite += ",N'" + Fix(Customer) + "'";
                                sWrite += " ;";
                                if (i % 50 == 0)
                                {
                                    b = ExecuteNoneQuery(sWrite);
                                    if (b == false)
                                    {
                                        ok = false;
                                    }
                                    sWrite = "";

                                }
                                i++;
                                c++;
                            }

                            if (sWrite != "")
                            {
                                b = ExecuteNoneQuery(sWrite);
                                if (b == false)
                                {
                                    ok = false;
                                }
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex, "discount - " + url);

                    }
                }
            }
            System.IO.File.AppendAllText(LogURL, "Downloaded Discount programs " + c.ToString("N0") + Environment.NewLine);
        }
        private string MyConnectionString
        {
            get
            {
                string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + @"\cn.txt");
                return lines[0];
            }
        }
        private string SourceConnectionString
        {
            get
            {
                string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + @"\cn.txt");
                return lines[0];
            }
        }
        public System.Data.DataTable GetMyData(string ssql)
        {
            DataTable dt = new DataTable();
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(MyConnectionString);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(ssql, cn);
            da.SelectCommand.CommandTimeout = 3000;
            cn.Open();
            da.Fill(dt);
            cn.Close();
            return dt;
        }
        public System.Data.DataTable GetSourceData(string ssql)
        {
            DataTable dt = new DataTable();
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(SourceConnectionString);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(ssql, cn);
            da.SelectCommand.CommandTimeout = 3000;
            cn.Open();
            da.Fill(dt);
            cn.Close();
            return dt;
        }

        public bool ExecuteNoneQuery(string ssql)
        {
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(MyConnectionString);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(ssql, cn);
            cmd.CommandTimeout = 30000;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                WriteLog(ex, ssql);
                cn.Close();

                return false;
            }

            return true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            d.Abort();

            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //d.Abort();
        }

        private void LoadData()
        {
            if (System.IO.File.Exists(LogURL))
            {
                try
                {
                    System.IO.File.Delete(LogURL);
                }
                catch
                {

                }
            }
            textBox4.Text = "";
            DateTime b = DateTime.Now;
            System.IO.File.AppendAllText(LogURL, "Start: " + b.ToString("dd/MM/yyyy hh:mm:ss tt") + "--------" + Environment.NewLine);
            //danh sach khach hang
            System.IO.File.AppendAllText(LogURL, "Download Customers " + Environment.NewLine);
            DownloadCustomer();

            System.IO.File.AppendAllText(LogURL, "Download Items " + Environment.NewLine);

            //lay danh sach hang
            DownloadItem();

            System.IO.File.AppendAllText(LogURL, "Download balance " + Environment.NewLine);

            //lay cac ton kho 3006
            DownloadBalance();

            //lay giam gia
            System.IO.File.AppendAllText(LogURL, "Download discount programs " + Environment.NewLine);

            DownloadDiscount();
            DateTime e = DateTime.Now;
            System.IO.File.AppendAllText(LogURL, "Done: " + e.ToString("dd/MM/yyyy hh:mm:ss tt") + "---------" + Environment.NewLine);

            Application.Exit();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            //LoadData();

            d = new System.Threading.Thread(new System.Threading.ThreadStart(LoadData));
            d.Start();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
