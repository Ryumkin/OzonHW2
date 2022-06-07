using DeviceDataGenerator.Data;

namespace DeviceServerGenerator.Services.Interfaces;

public interface IDeviceService
{
    void Enqueue(DeviceEvent deviceEvent);
    (DeviceEvent? DeviceEvent, bool Success) TryDequeue(DeviceEventType deviceEventType);
    IEnumerable<DeviceEvent> GetAllData(IReadOnlyCollection<DeviceEventType>? types);
    DeviceEvent? GetLastDeviceEvent(DeviceEventType deviceEventType);
}