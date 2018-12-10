/* This project was based on the previous coding of
 * Gerald Nelson (CodeProject.com) and Jayesh Jain(CodeProject.com). But since there
 * where alot of errors and uncompleted code i decided
 * to freshen things up and get it working with more comments
 * better and cleaner code.
 * 
 * Hope u guys like it.
 * - Max Persson
 */
using POSIntegratorV2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

// Using System.Threading because the event that will
// be raised from the monitor is comming from a different
// thread that will run on your code.

namespace POSIntegratorV2
{
    public partial class frmMain : Form
    {
        public static Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

        //===========================================================================
        //                  Folder Monitoring 
        //===========================================================================
        public FileSystemWatcher _watchFolder = new FileSystemWatcher();
        public Boolean FileWatcherIsStarted;
        public string folderToWatch;
        public string fileNameTxt;
        delegate void SetTextCallback(string text);
        public static String LogFileOutputFM;

        //===========================================================================
        //                  POS Local Monitoring 
        //===========================================================================

        public static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public static String LogFileOutput;

        public static Boolean IsConnected = false;

        public static Boolean IsIntegrating = false;

        public static Boolean IsIntegratingCustomer = false;
        public static Boolean IsIntegratingItem = false;
        public static Boolean IsIntegratingSupplier = false;
        public static Boolean IsIntegratingCollection = false;
        public static Boolean IsIntegratingItemPrice = false;
        public static Boolean IsIntegratingReceivingReceipt = false;
        public static Boolean IsIntegratingSalesReturn = false;
        public static Boolean IsIntegratingStockIn = false;
        public static Boolean IsIntegratingStockOut = false;
        public static Boolean IsIntegratingTransferIn = false;
        public static Boolean IsIntegratingTransferOut = false;

        private int logMessageCount = 0;
        private DataTable dt;
        private string[] dataWords;
        private int columnIndex;

        //
        // form loading
        //
        public frmMain()
        {
            InitializeComponent();
            readConfig();
            //txtDate.Value = DateTime.Now.AddDays(-1);

            if (timer.Enabled == true)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        // configure and integrate

        public void readConfig()
        {
            String settingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"System.json");
            String json;
            JavaScriptSerializer js = new JavaScriptSerializer();
            DateTime datetimeToday = DateTime.Now;
            var posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());
            using (StreamReader trmRead = new StreamReader(settingsPath))
            {
                json = trmRead.ReadToEnd();
            }
            Entities.System s = js.Deserialize<Entities.System>(json);

            txtDate.Text = datetimeToday.ToString("MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            txtDomain.Text = s.Domain;
            txtLocalConn.Text = s.LocalConnection;
            LogFileOutput = s.LogFileLocation;
            if (posData.DatabaseExists())
            {
                var Current = from d in posData.SysSettings select d;

                txtBranchCode.Text = Current.FirstOrDefault().BranchCode;
                txtUserCode.Text = Current.FirstOrDefault().UserCode;
                txtUseItemPrice.Checked = Current.FirstOrDefault().UseItemPrice;

                initiateIntegration();
            }
            else
            {
                MessageBox.Show("Local Connection Error");

                btnStart.Enabled = false;
            }
        }

        public void initiateIntegration()
        {
            try
            {
                String apiUrlHost = txtDomain.Text;

                var newConnectionString = txtLocalConn.Text;
                var posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                if (!apiUrlHost.Equals(""))
                {
                    txtActivity.Text += "Connecting to server...\r\n\n";

                    if (posData.DatabaseExists())
                    {
                        var sysSettings = from d in posData.SysSettings select d;
                        if (sysSettings.Any())
                        {
                            var branchCode = sysSettings.FirstOrDefault().BranchCode;
                            var userCode = sysSettings.FirstOrDefault().UserCode;
                            var useItemPrice = sysSettings.FirstOrDefault().UseItemPrice;

                            txtActivity.Text += "Connected! Branch Code: " + branchCode + "\r\n\n";

                            timer.Interval = 5000;
                            timer.Tick += new EventHandler(integrate);

                            startTimer();
                        }
                    }
                    else
                    {
                        txtActivity.Text = "Database not found!";
                    }
                }
                else
                {
                    txtActivity.Text = "Invalid URL Host!";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void manualIntegrate()
        {
            if (IsIntegrating == false)
            {
                String apiUrlHost = txtDomain.Text;
                var posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                IsIntegratingCustomer = true;
                IsIntegratingItem = true;
                IsIntegratingSupplier = true;

                Controllers.MstItemController objMstItem = new Controllers.MstItemController(this, txtDate.Text, false);
                Controllers.MstCustomerController objMstCustomer = new Controllers.MstCustomerController(this, txtDate.Text, false);
                Controllers.MstSupplierController objMstSupplier = new Controllers.MstSupplierController(this, txtDate.Text, false);

                objMstCustomer.GetCustomer(apiUrlHost);
                objMstSupplier.GetSupplier(apiUrlHost);
                objMstItem.GetItem(apiUrlHost);
            }
            stopTimer();
        }

        public void integrate(object sender, EventArgs e)
        {
            if (IsIntegrating == false)
            {
                String apiUrlHost = txtDomain.Text;
                var posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());

                var sysSettings = from d in posData.SysSettings select d;
                if (sysSettings.Any())
                {
                    var branchCode = sysSettings.FirstOrDefault().BranchCode;
                    var userCode = sysSettings.FirstOrDefault().UserCode;
                    var useItemPrice = sysSettings.FirstOrDefault().UseItemPrice;

                    IsIntegrating = true;

                    IsIntegratingCustomer = true;
                    IsIntegratingItem = true;
                    IsIntegratingSupplier = true;
                    IsIntegratingCollection = true;
                    if (useItemPrice)
                    {
                        IsIntegratingItemPrice = true;
                    }

                    IsIntegratingReceivingReceipt = true;
                    IsIntegratingSalesReturn = true;
                    IsIntegratingStockIn = true;
                    IsIntegratingStockOut = true;
                    IsIntegratingTransferIn = true;
                    IsIntegratingTransferOut = true;

                    Controllers.MstItemController objMstItem = new Controllers.MstItemController(this, txtDate.Text, true);
                    Controllers.MstCustomerController objMstCustomer = new Controllers.MstCustomerController(this, txtDate.Text, true);
                    Controllers.MstSupplierController objMstSupplier = new Controllers.MstSupplierController(this, txtDate.Text, true);
                    Controllers.TrnStockTransferInController objTrnStockTransferIn = new Controllers.TrnStockTransferInController(this, txtDate.Text);
                    Controllers.TrnStockTransferOutController objTrnStockTransferOut = new Controllers.TrnStockTransferOutController(this, txtDate.Text);
                    Controllers.TrnStockInController objTrnStockIn = new Controllers.TrnStockInController(this, txtDate.Text);
                    Controllers.TrnStockOutController objTrnStockOut = new Controllers.TrnStockOutController(this, txtDate.Text);
                    Controllers.TrnReceivingReceiptController objTrnReceivingReceipt = new Controllers.TrnReceivingReceiptController(this, txtDate.Text);
                    Controllers.TrnCollectionController objTrnCollection = new Controllers.TrnCollectionController(this);
                    Controllers.TrnSalesReturnController objTrnSalesReturn = new Controllers.TrnSalesReturnController(this);
                    Controllers.TrnItemPriceController objTrnItemPrice = new Controllers.TrnItemPriceController(this, txtDate.Text);

                    // ============
                    // Master Files
                    // ============
                    objMstCustomer.GetCustomer(apiUrlHost);
                    objMstSupplier.GetSupplier(apiUrlHost);
                    objMstItem.GetItem(apiUrlHost);

                    // ==================
                    // Inventory Movement
                    // ==================
                    objTrnReceivingReceipt.GetReceivingReceipt(apiUrlHost, branchCode);
                    objTrnStockIn.GetStockIn(apiUrlHost, branchCode);
                    objTrnStockOut.GetStockOut(apiUrlHost, branchCode);
                    objTrnStockTransferIn.GetStockTransferIN(apiUrlHost, branchCode);
                    objTrnStockTransferOut.GetStockTransferOT(apiUrlHost, branchCode);

                    // ====================
                    // Sales and Collection
                    // ====================
                    objTrnCollection.GetCollection(apiUrlHost, branchCode, userCode);
                    objTrnSalesReturn.GetSalesReturn(apiUrlHost, branchCode, userCode);
                    if (useItemPrice) { objTrnItemPrice.GetItemPrice(apiUrlHost, branchCode); }
                }
            }

        }

        // timer and logs
        public void logMessages(String message)
        {
            string displayMessage = message + "\r\n\n";

            if (message.Equals("Customer Integration Done.")) { IsIntegratingCustomer = false; displayMessage = ""; }
            if (message.Equals("Item Integration Done.")) { IsIntegratingItem = false; displayMessage = ""; }
            if (message.Equals("Supplier Integration Done.")) { IsIntegratingSupplier = false; displayMessage = ""; }
            if (message.Equals("Collection Integration Done.")) { IsIntegratingCollection = false; displayMessage = ""; }
            if (message.Equals("ItemPrice Integration Done.")) { IsIntegratingItemPrice = false; displayMessage = ""; }
            if (message.Equals("Receiving Receipt Integration Done.")) { IsIntegratingReceivingReceipt = false; displayMessage = ""; }
            if (message.Equals("Sales Return Integration Done.")) { IsIntegratingSalesReturn = false; displayMessage = ""; }
            if (message.Equals("StockIn Integration Done.")) { IsIntegratingStockIn = false; displayMessage = ""; }
            if (message.Equals("StockOut Integration Done.")) { IsIntegratingStockOut = false; displayMessage = ""; }
            if (message.Equals("Stock Transfer In Integration Done.")) { IsIntegratingTransferIn = false; displayMessage = ""; }
            if (message.Equals("Stock Transfer Out Integration Done.")) { IsIntegratingTransferOut = false; displayMessage = ""; }

            if (IsIntegratingCustomer == false &&
                 IsIntegratingItem == false &&
                 IsIntegratingSupplier == false &&
                 IsIntegratingCollection == false &&
                 IsIntegratingItemPrice == false &&
                 IsIntegratingReceivingReceipt == false &&
                 IsIntegratingSalesReturn == false &&
                 IsIntegratingStockIn == false &&
                 IsIntegratingStockOut == false &&
                 IsIntegratingTransferIn == false &&
                 IsIntegratingTransferOut == false)
            {
                IsIntegrating = false;
                displayMessage = "Ready to integrate." + "\r\n\n";

                logMessageCount++;
                if (logMessageCount > 20)
                {
                    logMessageCount = 0;
                    File.WriteAllText(LogFileOutput + "\\" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt", txtActivity.Text);
                    txtActivity.Text = "";
                }
            }

            txtActivity.Text += displayMessage;
            txtActivity.SelectionStart = txtActivity.Text.Length;
            txtActivity.ScrollToCaret();
        }

        public void startTimer()
        {
            timer.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnManualIntegrate.Enabled = false;

            txtActivity.Text += "Timer Started.  \r\n\n";
            txtDate.Enabled = false;
            txtDomain.Enabled = false;
            txtLocalConn.Enabled = false;
            btnSettings.Enabled = false;
            tabFM.Enabled = false;
        }

        public void stopTimer()
        {
            timer.Enabled = false;

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnManualIntegrate.Enabled = true;

            txtActivity.Text += "Timer Stoped.  \r\n\n";
            logMessageCount = 0;
            txtDate.Enabled = true;
            txtDomain.Enabled = true;
            txtLocalConn.Enabled = true;

            btnSettings.Enabled = true;
            tabFM.Enabled = true;

        }

        public void reloadForm(String BranchCode, String UserCode, Boolean UseItemPrice)
        {
            txtBranchCode.Text = BranchCode;
            txtUserCode.Text = UserCode;
            txtUseItemPrice.Checked = UseItemPrice;

            String settingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"System.json");
            String json;
            JavaScriptSerializer js = new JavaScriptSerializer();
            DateTime datetimeToday = DateTime.Now;
            using (StreamReader trmRead = new StreamReader(settingsPath))
            {
                json = trmRead.ReadToEnd();
            }
            Entities.System s = js.Deserialize<Entities.System>(json);

            txtDate.Text = datetimeToday.ToString("MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            txtDomain.Text = s.Domain;
            txtLocalConn.Text = s.LocalConnection;
            LogFileOutput = s.LogFileLocation;
        }

        //private void btnConnect_Click(object sender, EventArgs e)
        //{
        //    if (IsIntegrating == false && timer.Enabled == false)
        //    {
        //        readConfig();
        //    }
        //    else
        //    {
        //        MessageBox.Show("You must stop the integration before connecting to local database.");
        //    }
        //}

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnSystemSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSysSettings frm = new frmSysSettings(this);
            if (Application.OpenForms.OfType<frmSysSettings>().Any())
            {
                frm.BringToFront();
            }
            else
            {
                frm.Show();
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            if (Application.OpenForms.OfType<frmAbout>().Any())
            {
                about.BringToFront();
            }
            else
            {
                about.Show();

            }
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            stopTimer();
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            readConfig();
        }

        private void btnManualIntegrate_Click_1(object sender, EventArgs e)
        {
            startTimer();
            manualIntegrate();
        }

        //==========================================================
        // Folder Monitoring
        //==========================================================

        // start and stop file watcher method
        public void startTimerFM()
        {
            FileWatcherIsStarted = true;
            btnSettings.Enabled = false;
            btnStartFM.Enabled = false;
            btnStopFM.Enabled = true;
            tabDM.Enabled = false;

            systemFileWatcher();
        }

        public void stopTimerFM()
        {
            FileWatcherIsStarted = false;
            btnStartFM.Enabled = true;
            btnStopFM.Enabled = false;
            tabDM.Enabled = true;

            systemFileWatcher();
        }

        // Print activity log
        private void logMessageFM(String txt)
        {
            //txtActivityLogFM.Text += txt;
            if (this.txtActivityLogFM.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(logMessageFM);
                this.Invoke(d, new object[] { txt });
            }
            else
            {
                this.txtActivityLogFM.Text += LogFileOutputFM + "\r\n\n  ";
                this.txtActivityLogFM.Text += txt + "\r\n\n ";
            }
        }

        // file watcher method
        private void systemFileWatcher()
        {
            String settingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"System.json");
            String json;
            JavaScriptSerializer js = new JavaScriptSerializer();
            DateTime datetimeToday = DateTime.Now;
            var posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());
            using (StreamReader trmRead = new StreamReader(settingsPath))
            {
                json = trmRead.ReadToEnd();
            }
            Entities.System s = js.Deserialize<Entities.System>(json);

            if (FileWatcherIsStarted == true)
            {
                _watchFolder.Filter = "*.csv";
                _watchFolder.IncludeSubdirectories = true;
                _watchFolder.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                            | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                _watchFolder.Path = s.FoldertoMonitor;

                _watchFolder.Created += new FileSystemEventHandler(fileCreated);
                _watchFolder.Deleted += new FileSystemEventHandler(fileDeleted);
                _watchFolder.EnableRaisingEvents = true;
            }
            else
            {
                _watchFolder.EnableRaisingEvents = false;
                _watchFolder.Created -= new FileSystemEventHandler(fileCreated);
                _watchFolder.Deleted -= new FileSystemEventHandler(fileDeleted);
                //_watchFolder.Dispose();
            }
        }

        // event on delete file
        private void fileDeleted(object sender, FileSystemEventArgs e)
        {
            string prevFileDeleted = null;
            string fileNameDeleted;
            if (prevFileDeleted != e.Name)
            {
                fileNameDeleted = "File Deleted. Name {0}" + e.Name;
                logMessageFM(fileNameDeleted);
            }
            prevFileDeleted = e.Name;
        }

        //event on created file
        private void fileCreated(object sender, FileSystemEventArgs e)
        {
            string oldPath = null;
            if (oldPath != e.FullPath)
            {
                fileNameTxt = e.FullPath.Substring(0, e.FullPath.Length - 4);
                processFile(e.FullPath);
            }
            oldPath = e.FullPath;
        }

        //process the file created and print file name location
        private void processFile(string fullPath)
        {
            string DirName = fullPath;
            bool IsWatching = true;
            while (IsWatching == true)
            {
                try
                {
                    string[] value = DirName.Split('\\');
                    Debug.WriteLine(value[2]);
                    logMessageFM(fullPath.ToString());
                    if (value[2].Equals("CV")) { BindDataCVCSV(fullPath.ToString()); }
                    if (value[2].Equals("IN")) { BindDataINCSV(fullPath.ToString()); }
                    if (value[2].Equals("JV")) { BindDataJVCSV(fullPath.ToString()); }
                    if (value[2].Equals("OR")) { BindDataORCSV(fullPath.ToString()); }
                    if (value[2].Equals("OT")) { BindDataOTCSV(fullPath.ToString()); }
                    if (value[2].Equals("RR")) { BindDataRRCSV(fullPath.ToString()); }
                    if (value[2].Equals("SI")) { BindDataSICSV(fullPath.ToString()); }
                    if (value[2].Equals("ST")) { BindDataSTCSV(fullPath.ToString()); }
                }
                catch (IOException)
                {
                    Thread.Sleep(3000);
                }
                IsWatching = false;
            }
        }

        // ===================================
        //  convert csv proccessed file into json object S
        //  ST
        //====================================
        private void BindDataSTCSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnST> newStockTransfer = new List<Models.TrnST>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newStockTransfer.Add(new Models.TrnST
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            STDate = dr.ItemArray[1].ToString(),
                            ToBranchCode = dr.ItemArray[2].ToString(),
                            ArticleCode = dr.ItemArray[3].ToString(),
                            Remarks = dr.ItemArray[4].ToString(),
                            ManualSTNumber = dr.ItemArray[5].ToString(),
                            UserCode = dr.ItemArray[6].ToString(),
                            CreatedDateTime = dr.ItemArray[7].ToString(),
                            ItemCode = dr.ItemArray[8].ToString(),
                            Particulars = dr.ItemArray[9].ToString(),
                            Unit = dr.ItemArray[10].ToString(),
                            Quantity = dr.ItemArray[11].ToString(),
                            Cost = dr.ItemArray[12].ToString(),
                            Amount = dr.ItemArray[13].ToString(),
                        });
                    }

                    WriteSTJason(newStockTransfer);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        private void WriteSTJason(List<TrnST> newStockTransfer)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newStockTransfer);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnStockTransferFileWatcherController objTrnStockTransferFM = new FileWatcherController.TrnStockTransferFileWatcherController(this, txtDate);

                objTrnStockTransferFM.SendStockTransfer(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // ==================================
        //  convert csv proccessed file into json object 
        //  RR
        //===================================
        private void BindDataRRCSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnRR> newReceivingReceipt = new List<Models.TrnRR>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newReceivingReceipt.Add(new Models.TrnRR
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            RRDate = dr.ItemArray[1].ToString(),
                            SupplierCode = dr.ItemArray[2].ToString(),
                            Term = dr.ItemArray[3].ToString(),
                            ManualRRNumber = dr.ItemArray[4].ToString(),
                            DocumentReference = "NA",
                            Remarks = dr.ItemArray[5].ToString(),
                            UserCode ="NA",
                            CreatedDateTime = dr.ItemArray[6].ToString(),
                            PONumber = dr.ItemArray[7].ToString(),
                            PODate = dr.ItemArray[8].ToString(),
                            PODateNeeded = dr.ItemArray[9].ToString(),
                            ItemCode = dr.ItemArray[10].ToString(),
                            Particulars = dr.ItemArray[11].ToString(),
                            Unit = dr.ItemArray[12].ToString(),
                            Quantity = dr.ItemArray[13].ToString(),
                            Cost = dr.ItemArray[14].ToString(),
                            Amount = dr.ItemArray[15].ToString(),
                            ReceivedBranchCode = dr.ItemArray[16].ToString()
                        });
                    }

                    WriteRRJason(newReceivingReceipt);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        private void WriteRRJason(List<TrnRR> newReceivingReceipt)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newReceivingReceipt);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnReceivingReceiptFileWatcherController objTrnReceivingReceiptFM = new FileWatcherController.TrnReceivingReceiptFileWatcherController(this, txtDate);

                objTrnReceivingReceiptFM.SendReceivingReceipt(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //====================================
        //  convert csv proccessed file into json object 
        //  OT
        //====================================
        private void BindDataOTCSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnOT> newStockOut = new List<Models.TrnOT>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newStockOut.Add(new Models.TrnOT
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            ManualOTNumber = dr.ItemArray[1].ToString(),
                            OTDate = dr.ItemArray[2].ToString(),
                            AccountCode = dr.ItemArray[3].ToString(),
                            ArticleCode = dr.ItemArray[4].ToString(),
                            Remarks = dr.ItemArray[5].ToString(),
                            UserCode ="NA",
                            CreatedDateTime = dr.ItemArray[6].ToString(),
                            ItemCode = dr.ItemArray[7].ToString(),
                            Particulars = dr.ItemArray[8].ToString(),
                            Unit = dr.ItemArray[9].ToString(),
                            Quantity = dr.ItemArray[10].ToString(),
                            Cost = dr.ItemArray[11].ToString(),
                            Amount = dr.ItemArray[12].ToString(),
                        });
                    }

                    WriteOTJason(newStockOut);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        private void WriteOTJason(List<TrnOT> newStockOut)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newStockOut);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnStockOutFileWatcherController objTrnStockOutFM = new FileWatcherController.TrnStockOutFileWatcherController(this, txtDate);

                objTrnStockOutFM.sendStockOut(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //===================================
        // convert csv proccessed file into json object S
        // OR
        //====================================
        private void BindDataORCSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnOR> newCollection = new List<Models.TrnOR>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newCollection.Add(new Models.TrnOR
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            ORDate = dr.ItemArray[1].ToString(),
                            CustomerCode = dr.ItemArray[2].ToString(),
                            Remarks = dr.ItemArray[3].ToString(),
                            ManualORNumber = dr.ItemArray[5].ToString(),
                            UserCode = "NA",
                            CreatedDateTime = dr.ItemArray[6].ToString(),
                            AccountCode = dr.ItemArray[6].ToString(),
                            ArticleCode = dr.ItemArray[7].ToString(),
                            SINumber = dr.ItemArray[8].ToString(),
                            Particulars = dr.ItemArray[9].ToString(),
                            Amount = dr.ItemArray[10].ToString(),
                            PayType = dr.ItemArray[11].ToString(),
                            CheckNumber = dr.ItemArray[12].ToString(),
                            CheckDate = dr.ItemArray[13].ToString(),
                            CheckBank = dr.ItemArray[14].ToString(),
                            DepositoryBankCode = dr.ItemArray[15].ToString(),
                            IsClear = dr.ItemArray[16].ToString()
                        });
                    }

                    WriteORJason(newCollection);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        private void WriteORJason(List<TrnOR> newCollection)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newCollection);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnCollectionFileWatcherController objTrnCollectionFM = new FileWatcherController.TrnCollectionFileWatcherController(this, txtDate);

                objTrnCollectionFM.SendCollection(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //======================================
        // convert csv proccessed file into json object S
        // JV
        //=======================================
        private void BindDataJVCSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnJV> newJournalVoucher = new List<Models.TrnJV>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newJournalVoucher.Add(new Models.TrnJV
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            ManualJVNumber = dr.ItemArray[1].ToString(),
                            JVDate = dr.ItemArray[2].ToString(),
                            Remarks = dr.ItemArray[3].ToString(),
                            UserCode = "NA",
                            CreatedDateTime = dr.ItemArray[4].ToString(),
                            EntryBranchCode = dr.ItemArray[5].ToString(),
                            AccountCode = dr.ItemArray[6].ToString(),
                            ArticleCode = dr.ItemArray[7].ToString(),
                            Particulars = dr.ItemArray[8].ToString(),
                            DebitAmount = dr.ItemArray[9].ToString(),
                            CreditAmount = dr.ItemArray[10].ToString(),
                            ARRRNumber = dr.ItemArray[11].ToString(),
                            ARSINumber = dr.ItemArray[12].ToString(),
                            IsClear = dr.ItemArray[13].ToString(),
                        });
                    }

                    WriteJVJason(newJournalVoucher);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        private void WriteJVJason(List<TrnJV> newJournalVoucher)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newJournalVoucher);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnJournalVoucherFileWatcherController objTrnJournalVoucherFM = new FileWatcherController.TrnJournalVoucherFileWatcherController(this, txtDate);

                objTrnJournalVoucherFM.sendJournalVoucher(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //==============================================
        // convert csv proccessed file into json object S
        //  IN
        //==============================================
        private void BindDataINCSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnIN> newStockIn = new List<Models.TrnIN>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newStockIn.Add(new Models.TrnIN
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            ManualINNumber = dr.ItemArray[1].ToString(),
                            INDate = dr.ItemArray[2].ToString(),
                            AccountCode = dr.ItemArray[3].ToString(),
                            ArticleCode = dr.ItemArray[4].ToString(),
                            Remarks = dr.ItemArray[5].ToString(),
                            IsProduce = "NA",
                            UserCode = "NA",
                            CreatedDateTime = dr.ItemArray[6].ToString(),
                            ItemCode = dr.ItemArray[7].ToString(),
                            Particulars = dr.ItemArray[8].ToString(),
                            Unit = dr.ItemArray[9].ToString(),
                            Quantity = dr.ItemArray[10].ToString(),
                            Cost = dr.ItemArray[11].ToString(),
                            Amount = dr.ItemArray[12].ToString()
                        });
                    }

                    WriteINJason(newStockIn);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        private void WriteINJason(List<TrnIN> newStockIn)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newStockIn);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnStockInFileWatcherController objTrnStockInFM = new FileWatcherController.TrnStockInFileWatcherController(this, txtDate);

                objTrnStockInFM.SendCollection(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //=====================================
        // convert csv proccessed file into json object S
        //  CV
        //======================================
        private void BindDataCVCSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnCV> newCheckVoucher = new List<Models.TrnCV>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newCheckVoucher.Add(new Models.TrnCV
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            ManualCVNumber = dr.ItemArray[1].ToString(),
                            CVDate = dr.ItemArray[2].ToString(),
                            SupplierCode = dr.ItemArray[3].ToString(),
                            Payee = dr.ItemArray[4].ToString(),
                            PayType = dr.ItemArray[5].ToString(),
                            Remarks = dr.ItemArray[6].ToString(),
                            BankCode = dr.ItemArray[7].ToString(),
                            CheckNumber = dr.ItemArray[8].ToString(),
                            CheckDate = dr.ItemArray[9].ToString(),
                            TotalAmount = dr.ItemArray[10].ToString(),
                            IsCrossCheck = dr.ItemArray[11].ToString(),
                            IsClear = dr.ItemArray[12].ToString(),
                            CreatedDateTime = dr.ItemArray[13].ToString(),
                            UserCode = dr.ItemArray[13].ToString(),
                            AccountCode = dr.ItemArray[14].ToString(),
                            ArticleCode = dr.ItemArray[15].ToString(),
                            Particulars = dr.ItemArray[16].ToString(),
                            RRNumber = dr.ItemArray[17].ToString(),
                            Amount = dr.ItemArray[18].ToString()
                        });
                    }

                    WriteCVJason(newCheckVoucher);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        private void WriteCVJason(List<TrnCV> newCheckVoucher)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newCheckVoucher);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnCheckVoucherFileWatcherController objTrnCheckVoucherFM = new FileWatcherController.TrnCheckVoucherFileWatcherController(this, txtDate);

                objTrnCheckVoucherFM.sendCheckVoucher(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //==========================================
        // convert csv proccessed file into json object S
        //  SI
        //===========================================
        private void BindDataSICSV(string text)
        {
            try
            {
                dt = new DataTable();
                string[] lines = File.ReadAllLines(text);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');

                    // header
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    List<Models.TrnSI> newSalesInvoice = new List<Models.TrnSI>();

                    // lines
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string line = lines[r];
                        dataWords = line.Split(',');

                        DataRow dr = dt.NewRow();
                        columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);

                        newSalesInvoice.Add(new Models.TrnSI
                        {
                            BranchCode = dr.ItemArray[0].ToString(),
                            BranchName = dr.ItemArray[1].ToString(),
                            SINumber = dr.ItemArray[2].ToString(),
                            SIDate = dr.ItemArray[3].ToString(),
                            DocumentReference = dr.ItemArray[4].ToString(),
                            CustomerCode = dr.ItemArray[5].ToString(),
                            Term = dr.ItemArray[6].ToString(),
                            Remarks = dr.ItemArray[7].ToString(),
                            PreparedByUser = dr.ItemArray[8].ToString(),
                            SoldByUser = dr.ItemArray[9].ToString(),
                            CreatedDateTime = dr.ItemArray[10].ToString(),
                            ItemCode = dr.ItemArray[11].ToString(),
                            Particulars = dr.ItemArray[12].ToString(),
                            Quantity = dr.ItemArray[13].ToString(),
                            Unit = dr.ItemArray[14].ToString(),
                            Price = dr.ItemArray[15].ToString(),
                            Discount = dr.ItemArray[16].ToString(),
                            DiscountRate = dr.ItemArray[17].ToString(),
                            NetPrice = dr.ItemArray[18].ToString(),
                            Amount = dr.ItemArray[19].ToString()
                        });
                    }

                    WriteSIJason(newSalesInvoice);
                }
            }
            catch (Exception e)
            {
                logMessageFM(e.Message + "\r\n\n" + "Please check File Name");
            }
        }
        // save json file to folder
        public void WriteSIJason(List<Models.TrnSI> newSalesInvoice)
        {
            try
            {
                string txtFile = fileNameTxt + ".json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(newSalesInvoice);
                File.WriteAllText(txtFile, json);

                String apiUrlHost = txtDomain.Text;

                FileWatcherController.TrnSalesInvoiceFileWatcherController objTrnSalesInvoiceFM = new FileWatcherController.TrnSalesInvoiceFileWatcherController(this, txtDate);

                objTrnSalesInvoiceFM.SendInvoice(apiUrlHost, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // start and stop button events
        private void btnStartFM_Click(object sender, EventArgs e)
        {
            startTimerFM();
            logMessageFM("File Watcher Started");
        }

        private void btnStopFM_Click(object sender, EventArgs e)
        {
            stopTimerFM();
            logMessageFM("File Watcher Stoped");
        }
    }
}