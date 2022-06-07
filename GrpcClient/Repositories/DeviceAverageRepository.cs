using System.Collections.Concurrent;
using DeviceDataGenerator.Data;

namespace GrpcClient.Repositories;

public class DeviceAverageRepository : IDeviceAverageRepository
{
    private ConcurrentDictionary<DeviceEventType, ConcurrentBag<DeviceEventAverageModel>> _data = new ();

    public void AddNewAverageModel(DeviceEventAverageModel model)
    {
        var bag = _data.GetValueOrDefault(model.Type, new ConcurrentBag<DeviceEventAverageModel>());
        bag.Add(model);
        _data[model.Type] = bag;
    }


    public IReadOnlyCollection<DeviceEventAverageModel> GetAverageModels(DeviceEventType type)
    {
        return _data.GetValueOrDefault(type, new ConcurrentBag<DeviceEventAverageModel>());
    }
}