using System.Collections.Concurrent;
using DeviceDataGenerator.Data;

namespace GrpcClient.Repositories;

public class DeviceLoggerRepository : IDeviceLoggerRepository
{
    private ConcurrentDictionary<DeviceEventType, ConcurrentBag<DeviceEvent>> _data = new ();

    public void AddDevice(DeviceEvent deviceEvent)
    {
        var bag = _data.GetValueOrDefault(deviceEvent.Type, new ConcurrentBag<DeviceEvent>());
        bag.Add(deviceEvent);
        _data[deviceEvent.Type] = bag;
    }


    public IReadOnlyCollection<DeviceEvent> GetDeviceEvents(DeviceEventType type)
    {
        return _data.GetValueOrDefault(type, new ConcurrentBag<DeviceEvent>());
    }
}