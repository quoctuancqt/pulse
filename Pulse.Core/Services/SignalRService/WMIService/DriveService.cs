namespace Pulse.Core.Services
{
    using Common.Helpers;
    using System.IO;
    using System.Threading.Tasks;

    public class DriveService : BaseService
    {
        public override Task<string> GetValueAsync()
        {
            return AsyncHelper.RunAsync(() => GetDataDrive());
        }

        private string GetDataDrive()
        {
            var output = "\"details\" : {";
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    double freeSpace = drive.TotalFreeSpace;
                    double totalSpace = drive.TotalSize;
                    double percentFree = (freeSpace / totalSpace) * 100;
                    output += $"\"{drive.Name.Split(':')[0]}\" : \"{(int)percentFree}\",";
                }
            }
            output += "}";
            output = output.Remove(output.Length - 2, 1);
            return output;
        }

    }
}
