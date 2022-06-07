using DeviceDataGenerator.Data;
using GrpcClient.Repositories;

namespace GrpcClient.Services
{
    public interface IDeviceLoggerService
    {
        void AddDevice(DeviceEvent deviceEvent);
        IReadOnlyCollection<DeviceEvent> GetDataByInterval(DeviceEventType type);
    }
}