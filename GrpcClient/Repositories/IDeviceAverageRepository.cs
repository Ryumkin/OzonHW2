using DeviceDataGenerator.Data;

namespace GrpcClient.Repositories;

public interface IDeviceAverageRepository
{
    void AddNewAverageModel(DeviceEventAverageModel model);

    IReadOnlyCollection<DeviceEventAverageModel> GetAverageModels(DeviceEventType type);
}