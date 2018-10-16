/* This project was based on the previous coding of
 * Gerald Nelson (CodeProject.com) and Jayesh Jain(CodeProject.com). But since there
 * where alot of errors and uncompleted code i decided
 * to freshen things up and get it working with more comments
 * better and cleaner code.
 * 
 * Hope u guys like it.
 * - Max Persson
 */

using System;
// Using System.IO to get the hooks from the system
using System.IO;
using System.Linq;
using System.Reflection;
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

        public FileSystemWatcher _watchFolder = new FileSystemWatcher();
        public static Timer timer = new Timer();
        public static String FileOutput;

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
        //


        // form loading

        public frmMain()
        {
            InitializeComponent();
            readConfig();

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

        // folder monitoring
        public void abortAcitivityMonitoring()
        {

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
            FileOutput = s.OutputFileLocation;
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

                    Controllers.MstItemController objMstItem = new Controllers.MstItemController(this, txtDate.Text);
                    Controllers.MstCustomerController objMstCustomer = new Controllers.MstCustomerController(this, txtDate.Text);
                    Controllers.MstSupplierController objMstSupplier = new Controllers.MstSupplierController(this, txtDate.Text);
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
                    File.WriteAllText(FileOutput + "\\" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt", txtActivity.Text);
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

            txtActivity.Text += "Timer Started.  \r\n\n";
            txtDate.Enabled = false;
            txtDomain.Enabled = false;
            txtLocalConn.Enabled = false;
            btnSettings.Enabled = false;
        }

        public void stopTimer()
        {
            timer.Enabled = false;

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            txtActivity.Text += "Timer Stoped.  \r\n\n";
            logMessageCount = 0;
            txtDate.Enabled = true;
            txtDomain.Enabled = true;
            txtLocalConn.Enabled = true;

            btnSettings.Enabled = true;
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
            FileOutput = s.OutputFileLocation;
        }

        // button events

        private void btnStart_Click(object sender, EventArgs e)
        {
            readConfig();
            //startTimer();
            //frmMain();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopTimer();
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
    }
}