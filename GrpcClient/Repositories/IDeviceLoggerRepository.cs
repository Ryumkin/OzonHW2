using DeviceDataGenerator.Data;

namespace GrpcClient.Repositories;

public interface IDeviceLoggerRepository
{
    void AddDevice(DeviceEvent deviceEvent);
    IReadOnlyCollection<DeviceEvent> GetDeviceEvents(DeviceEventType type);
}