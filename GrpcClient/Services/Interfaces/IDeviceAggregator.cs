using DeviceDataGenerator.Data;

namespace GrpcClient.Services;

public interface IDeviceAggregator
{
    void AddDevice(DeviceEvent deviceEvent);
    IReadOnlyCollection<DeviceEventAverageModel> GetDataByInterval(DeviceEventType type,DateTimeOffset dateTimeStart, int intervalInSeconds);
}