using System.Collections.Concurrent;
using DeviceDataGenerator.Data;
using DeviceServerGenerator.Services.Interfaces;

namespace DeviceServerGenerator.Services;

public class DeviceService : IDeviceService
{
    private readonly ConcurrentDictionary<DeviceEventType, ConcurrentQueue<DeviceEvent>> _data = new();
    private readonly ConcurrentDictionary<DeviceEventType, DeviceEvent> _lastGeneratedData = new();
    private readonly ILogger<DeviceService> _logger;

    public DeviceService(ILogger<DeviceService> logger)
    {
        _logger = logger;
    }

    public void Enqueue(DeviceEvent deviceEvent)
    {
        var queue = _data.GetValueOrDefault(deviceEvent.Type, new ConcurrentQueue<DeviceEvent>());
        queue.Enqueue(deviceEvent);
        _data[deviceEvent.Type] = queue;
        _lastGeneratedData[deviceEvent.Type] = deviceEvent;
        _logger.LogInformation("Device @{DeviceEvent} was added to the storage", deviceEvent);
    }

    public (DeviceEvent? DeviceEvent, bool Success) TryDequeue(DeviceEventType deviceEventType)
    {
        if (!_data.TryGetValue(deviceEventType, out var queue))
        {
            return (null, false);
        }
        var result = queue.TryDequeue(out var deviceEvent);
        return (deviceEvent, result);
    }

    public IEnumerable<DeviceEvent> GetAllData(IReadOnlyCollection<DeviceEventType>? types)
    {
        var filteredTypes = types?.ToHashSet() ?? _data.Keys;
        foreach (var type in filteredTypes)
        {
            var (deviceEvent, success) = TryDequeue(type);
            if (!success)
            {
                continue;
            }

            yield return deviceEvent!;
        }
    }

    public DeviceEvent? GetLastDeviceEvent(DeviceEventType deviceEventType)
    {
        return _lastGeneratedData.GetValueOrDefault(deviceEventType);
    }
}