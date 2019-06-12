namespace Pulse.Core.Services
{
    using System.Collections.Generic;
    using System.Management;
    using System.Threading.Tasks;

    public interface IWMIService
    {
        Task<PropertyDataCollection> GetPropertyValuesAsync(string query, string className);

        Task<string> GetValueAsync();

        Task<IList<ManagementObject>> GetAllInstancesAsync(string query, string className);
    }
}
