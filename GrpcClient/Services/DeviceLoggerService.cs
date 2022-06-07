using System.Collections.Concurrent;
using DeviceDataGenerator.Data;
using GrpcClient.Repositories;

namespace GrpcClient.Services;

public class DeviceLoggerService : IDeviceLoggerService
{
    private readonly IDeviceLoggerRepository _deviceLoggerRepository;

    public DeviceLoggerService(IDeviceLoggerRepository deviceLoggerRepository)
    {
        _deviceLoggerRepository = deviceLoggerRepository;
    }
   
    public void AddDevice(DeviceEvent deviceEvent)
    {
            _deviceLoggerRepository.AddDevice(deviceEvent);
    }
    
    public IReadOnlyCollection<DeviceEvent> GetDataByInterval(DeviceEventType type)
    {
        return _deviceLoggerRepository.GetDeviceEvents(type);
    }
}