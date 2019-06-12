namespace Pulse.Core.Services
{
    using WMI;
    using System.Collections.Generic;
    using System.Management;
    using System.Threading.Tasks;
    using Common.Helpers;
    using Repository.SignalR;

    public abstract class BaseService : IWMIService
    {
        protected readonly WMIConnection _wmiConnection;
        
        public BaseService() { }

        public BaseService(WMIConnection wmiConnection)
        {
            _wmiConnection = wmiConnection;
        }

        #region Public Method
        public abstract Task<string> GetValueAsync();

        public virtual Task<PropertyDataCollection> GetPropertyValuesAsync(string query, string className)
        {
            return AsyncHelper.RunAsync(() => GetPropertyValues(query, className));
        }

        public virtual Task<IList<ManagementObject>> GetAllInstancesAsync(string query, string className)
        {
            return AsyncHelper.RunAsync(() => GetAllInstances(query, className));
        }

        #endregion

        #region Private Method

        private PropertyDataCollection GetPropertyValues(string query, string className)
        {
            return WMIRepository.GetPropertyValues(_wmiConnection, query, className);
        }

        private IList<ManagementObject> GetAllInstances(string query, string className )
        {
            return WMIRepository.GetAllInstances(_wmiConnection, query, className);
        }

        #endregion

    }
}
