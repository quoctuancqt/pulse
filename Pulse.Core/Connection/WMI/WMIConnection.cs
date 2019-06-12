namespace Pulse.Core.WMI
{
    using System;
    using System.Management;
 
    public class WMIConnection
    {
        ManagementScope connectionScope;
        ConnectionOptions options;

        #region "properties"
        public ManagementScope GetConnectionScope
        {
            get { return connectionScope; }
        }
        public ConnectionOptions GetOptions
        {
            get { return options; }
        }
        #endregion

        #region "static helpers"
        public static ConnectionOptions SetConnectionOptions()
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.Authentication = AuthenticationLevel.Default;
            options.EnablePrivileges = true;
            return options;
        }

        public static ManagementScope SetConnectionScope(string machineName,
                                                   ConnectionOptions options,string scope)
        {
            ManagementScope connectScope = new ManagementScope();
            connectScope.Path = new ManagementPath(@"\\" + machineName + @"\root\" + scope);
            connectScope.Options = options;

            try
            {
                connectScope.Connect();
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message.ToString());
            }
            return connectScope;
        }
        #endregion

        #region "constructors"
        public WMIConnection()
        {
            EstablishConnection(null, null, null, Environment.MachineName, null);
        }

        public WMIConnection(string userName,
                          string password,
                          string domain,
                          string machineName,
                          string scope)
        {
            EstablishConnection(userName, password, domain, machineName,scope);
        }
        #endregion

        #region "private helpers"
        private void EstablishConnection(string userName, string password, string domain, string machineName, string scope)
        {
            options = SetConnectionOptions();
            if (domain != null || userName != null)
            {
                options.Username = domain + "\\" + userName;
                options.Password = password;
            }
            connectionScope = SetConnectionScope(machineName, options, scope);
        }
        #endregion
      
   }
}
