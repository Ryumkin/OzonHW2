using DeviceDataGenerator.Data;

namespace GrpcClient.Services;

public interface IDeviceAverageCalculator
{
    DeviceEventAverageModel Calculate(IReadOnlyCollection<DeviceEvent> deviceEvents);
}