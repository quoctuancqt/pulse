namespace Pulse.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using log4net;

    public class CollectDeviceService : ICollectDeviceService
    {
        private readonly IWMIService _memoryService;
        private readonly IWMIService _netWorkService;
        private readonly IWMIService _physicalDiskService;
        private readonly IWMIService _processorService;
        private readonly IWMIService _temperatureService;
        private readonly IWMIService _driveService;
        private readonly IWMIService _eventLogService;
        private readonly ILog _log = LogManager.GetLogger(typeof(CollectDeviceService));

        public CollectDeviceService(MemoryService memoryService, NetWorkService netWorkService,
            PhysicalDiskService physicalDiskService, ProcessorService processorService,
            TemperatureService temperatureService, DriveService driveService, EventLogService eventLogService)
        {
            _memoryService = memoryService;
            _netWorkService = netWorkService;
            _physicalDiskService = physicalDiskService;
            _processorService = processorService;
            _temperatureService = temperatureService;
            _driveService = driveService;
            _eventLogService = eventLogService;
        }

        public async Task<string> CollectDeviceInfoAsync()
        {
            try
            {
                var disk = await _physicalDiskService.GetValueAsync();
                var cpu = await _processorService.GetValueAsync();
                var memory = await _memoryService.GetValueAsync();
                var temperature = await _temperatureService.GetValueAsync();
                var network = await _netWorkService.GetValueAsync();
                var disk_detail = await _driveService.GetValueAsync();
                var eventLog = await _eventLogService.GetValueAsync();
                var output = $"{{{cpu} , {memory} , {temperature} , {disk} , {disk_detail} , {network} , {eventLog}}}";
                return output;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception("There is an exception that was happening at server when CollectDeviceInfoAsync. Please check the error log: " + ex.Message);
            }

        }
    }
}


