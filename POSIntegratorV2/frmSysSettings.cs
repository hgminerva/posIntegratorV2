using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace POSIntegratorV2
{
    public partial class frmSysSettings : Form
    {
        public static Data.POSDatabaseDataContext posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());
        public static Entities.System s;
        public static Boolean EnableForm = false;
        public static frmMain FormMain;


        public frmSysSettings(frmMain parent)
        {
            FormMain = parent;
            InitializeComponent();
            cmdEnableDisable();
            CurrentValue();
            loadFormData();
        }


        public void UpdateSysSettings()
        {
            if (posData.DatabaseExists())
            {
                var UpdateSysSetting = from d in posData.SysSettings select d;

                if (UpdateSysSetting.Any())
                {
                    var UpdateSettings = UpdateSysSetting.FirstOrDefault();
                    UpdateSettings.BranchCode = txtBranchCode.Text;
                    UpdateSettings.UserCode = txtUserCode.Text;
                    UpdateSettings.PostUserId = Convert.ToInt32(txtPostUser.SelectedValue);
                    UpdateSettings.PostSupplierId = Convert.ToInt32(txtPostSupplier.SelectedValue);
                    UpdateSettings.UseItemPrice = txtUseItemPrice.Checked;
                    posData.SubmitChanges();

                    FormMain.reloadForm(UpdateSettings.BranchCode, UpdateSettings.UserCode, UpdateSettings.UseItemPrice);
                }
            }
            else
            {
                loadFormData();
                FormMain.reloadForm(txtBranchCode.Text, txtUserCode.Text, txtUseItemPrice.Checked);
            }
        }

        public void loadFormData()
        {
            if (posData.DatabaseExists())
            {
                // Load Form
                var sysSettings = from d in posData.SysSettings select d;
                if (sysSettings.Any())
                {
                    txtBranchCode.Text = sysSettings.FirstOrDefault().BranchCode;
                    txtUserCode.Text = sysSettings.FirstOrDefault().UserCode;
                    txtPostSupplier.SelectedValue = sysSettings.FirstOrDefault().PostSupplierId;
                    txtPostUser.SelectedValue = sysSettings.FirstOrDefault().PostUserId;
                    txtUseItemPrice.Checked = sysSettings.FirstOrDefault().UseItemPrice;
                }

                // Combo box
                txtPostUser.DataSource = from d in posData.MstUsers select d;
                txtPostUser.ValueMember = "Id";
                txtPostUser.DisplayMember = "Username";

                txtPostSupplier.DataSource = from d in posData.MstSuppliers select d;
                txtPostSupplier.ValueMember = "Id";
                txtPostSupplier.DisplayMember = "Supplier";
            }
            else
            {
                posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());
                if (posData.DatabaseExists())
                {
                    loadFormData();
                }
            }
        }

        public void CurrentValue()
        {

            String settingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"System.json");

            String json;
            using (StreamReader trmRead = new StreamReader(settingsPath))
            {
                json = trmRead.ReadToEnd();
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            s = js.Deserialize<Entities.System>(json);
            txtFileLocation.Text = s.LogFileLocation;
            txtLocalConnection.Text = s.LocalConnection;
            txtDomain.Text = s.Domain;
            txtFolderToMonitor.Text = s.FoldertoMonitor;
            //txtDefaultDate.Checked = s.IsDefaultDate;
        }

        public void SaveOutputFileLocation()
        {
            //posData = new Data.POSDatabaseDataContext(SysGlobal.ConnectionStringConfig());
            Entities.System settingsData = new Entities.System()
            {
                Domain = txtDomain.Text,
                LocalConnection = txtLocalConnection.Text,
                LogFileLocation = txtFileLocation.Text,
                FoldertoMonitor=txtFolderToMonitor.Text
            };

            String json = new JavaScriptSerializer().Serialize(settingsData);
            String settingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"System.json");
            File.WriteAllText(settingsPath, json);
            UpdateSysSettings();
        }

        public void cmdEnableDisable()
        {
            if (EnableForm == false)
            {
                txtBranchCode.Enabled = false;
                txtFileLocation.Enabled = false;
                txtPostSupplier.Enabled = false;
                txtPostUser.Enabled = false;
                txtUseItemPrice.Enabled = false;
                txtUserCode.Enabled = false;
                txtDomain.Enabled = false;
                txtLocalConnection.Enabled = false;
                txtFolderToMonitor.Enabled = false;
                //txtDefaultDate.Enabled = false;
            }
            else
            {
                txtBranchCode.Enabled = true;
                txtFileLocation.Enabled = true;
                txtPostSupplier.Enabled = true;
                txtPostUser.Enabled = true;
                txtUseItemPrice.Enabled = true;
                txtUserCode.Enabled = true;
                txtDomain.Enabled = true;
                //txtDefaultDate.Enabled = true;
                txtLocalConnection.Enabled = true;
                txtFolderToMonitor.Enabled = true;
            }
        }

        //Events

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            EnableForm = false;
            cmdEnableDisable();
            SaveOutputFileLocation();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EnableForm = true;
            cmdEnableDisable();
        }
    }
}
