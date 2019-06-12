namespace Pulse.Core.Repository.SignalR
{
    using Pulse.Core.WMI;
    using System.Collections.Generic;
    using System.Management;
    public class WMIRepository
    {
        public static PropertyDataCollection GetPropertyValues(WMIConnection WMIConnection,
                                                      string SelectQuery,
                                                      string className)
        {
            ManagementScope connectionScope = WMIConnection.GetConnectionScope;
            SelectQuery msQuery = new SelectQuery(SelectQuery);
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(connectionScope, msQuery);

            try
            {
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    return item.Properties;
                }
            }
            catch
            {
                /* Do Nothing */
            }
            return null;
        }

        public static List<ManagementObject> GetAllInstances(WMIConnection WMIConnection,
                                                      string SelectQuery,
                                                      string className)
        {
            ManagementScope connectionScope = WMIConnection.GetConnectionScope;
            SelectQuery msQuery = new SelectQuery(SelectQuery);
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(connectionScope, msQuery);
            List<ManagementObject> instances = new List<ManagementObject>();
            try
            {
                foreach (ManagementObject instance in searchProcedure.Get())
                {
                    instances.Add(instance);
                }
            }
            catch (ManagementException)
            {
                /* Do Nothing */
            }
            return instances;
        }
    }
}
