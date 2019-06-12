namespace Pulse.Setting
{
    using Core.Dto.Entity;
    using Core.Services;
    using Common.Helpers;
    using Common.ResolverFactories;
    using System;
    using System.Windows.Forms;
    using System.ServiceProcess;

    public partial class KioskSetting : Form
    {
        private readonly HelperConfiguration _helperConfiguration;
        private const string OWIN_APPLICATION_NAME = "KioskService";
        private const int CHECKING_STATUS_INTERVAL = 500;

        public KioskSetting()
        {
            InitializeComponent();
            _helperConfiguration = new HelperConfiguration();
            ProcessPanelLiscense();
            lbl_copyrigth.Text = "©Tekcent " + DateTime.Now.Year;
        }

        private async void btn_active_Click(object sender, EventArgs e)
        {
            string licenseKey = txt_license_key.Text;

            if (!string.IsNullOrEmpty(licenseKey))
            {
                panel_liscense.Enabled = false;
                var apiService = ResolverFactory.GetService<KioskApiService>();
                var result = await apiService.CheckLicenseKeyAsync(licenseKey);
                UpdateKioskSecurity(result);
            }
            else
            {
                MessageBox.Show("Please input license key.");
            }
        }

        private void ProcessPanelLiscense()
        {
            var licenseKey = KioskConfigurationHepler.GetValueFromSecurity("LicenseKey");

            if (!string.IsNullOrEmpty(licenseKey))
            {
                BindSetting();
                txt_license_key.Text = licenseKey;
                panel_liscense.Enabled = false;
            }
            else
            {
                panel_liscense.Enabled = true;
            }
        }

        private void UpdateKioskSecurity(KioskSecurityDto kioskSecurityDto)
        {
            if (kioskSecurityDto != null)
            {
                var item = kioskSecurityDto.EncryptionUser;
                KioskConfigurationHepler.UpdateValueForElement("LicenseKey", txt_license_key.Text);
                KioskConfigurationHepler.UpdateValueForElement("UserEncrypted", item.UserEncrypted);
                KioskConfigurationHepler.UpdateValueForElement("SaltKey", item.SaltKey);
                KioskConfigurationHepler.UpdateValueForElement("VIKey", item.VIKey);
                KioskConfigurationHepler.UpdateValueForElement("PasswordHash", item.PasswordHash);
                KioskConfigurationHepler.UpdateValueForElement("MachineId", kioskSecurityDto.MachineId);
                KioskConfigurationHepler.UpdateValueForElement("ClientId", kioskSecurityDto.ClientId);
                BindSetting();
            }
            else
            {
                MessageBox.Show("License key is invalid");
                panel_liscense.Enabled = true;
            }
        }

        private void btn_start_service_Click(object sender, EventArgs e)
        {
            StartService();
            btn_start_service.Enabled = false;
        }

        private void StartService()
        {
            if (Helper.IsAnAdministrator())
            {
                ServiceController service = new ServiceController(OWIN_APPLICATION_NAME);

                try
                {
                    service.Start();

                    while (true)
                    {
                        if (CheckServiceStatus(ServiceControllerStatus.Running))
                        {
                            MessageBox.Show("Service Started.");
                            btn_stop_service.Enabled = true;
                            return;
                        }

                        System.Threading.Thread.Sleep(CHECKING_STATUS_INTERVAL);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                }
            }
        }

        private void btn_stop_service_Click(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController(OWIN_APPLICATION_NAME);
            try
            {
                service.Stop();
                while (true)
                {
                    if (CheckServiceStatus(ServiceControllerStatus.Stopped))
                    {
                        MessageBox.Show("Service Stoped.");
                        btn_stop_service.Enabled = false;
                        btn_start_service.Enabled = true;
                        return;
                    }

                    System.Threading.Thread.Sleep(CHECKING_STATUS_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void BindSetting()
        {
            setting_penal.Enabled = true;
            ServiceController service = new ServiceController(OWIN_APPLICATION_NAME);
            if (service.Status == ServiceControllerStatus.Running)
            {
                btn_start_service.Enabled = false;
                btn_stop_service.Enabled = true;
            }
            else
            {
                btn_start_service.Enabled = true;
                btn_stop_service.Enabled = false;
            }
        }

        private bool CheckServiceStatus(ServiceControllerStatus expectedStatus)
        {
            var service = new ServiceController(OWIN_APPLICATION_NAME);
            return expectedStatus == service.Status ? true : false;
        }
    }
}
