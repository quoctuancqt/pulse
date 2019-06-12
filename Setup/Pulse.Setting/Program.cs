namespace Pulse.Setting
{
    using Core.IoC.SignalR;
    using System;
    using System.Windows.Forms;
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            NinjectConfiguration.Config();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KioskSetting());
        }
    }
}
