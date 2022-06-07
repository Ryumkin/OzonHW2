using System.Collections.Concurrent;
using DeviceDataGenerator.Data;
using DeviceServerGenerator.Settings;
using GrpcClient.Repositories;
using Microsoft.Extensions.Options;

namespace GrpcClient.Services;

public class DeviceAggregator : IDeviceAggregator
{
    private readonly IDeviceAverageCalculator _averageCalculator;
    private readonly IDeviceAverageRepository _deviceAverageRepository;
    private readonly int _intervalInSeconds;
    private readonly ConcurrentDictionary<DeviceEventType, ConcurrentQueue<DeviceEvent>> _data = new();

    public DeviceAggregator(IDeviceAverageCalculator averageCalculator,
        IDeviceAverageRepository deviceAverageRepository,
        IOptions<DeviceAggregatorOptions> options)
    {
        _averageCalculator = averageCalculator;
        _deviceAverageRepository = deviceAverageRepository;
        _intervalInSeconds = (int)options.Value.Duration.TotalSeconds;
    }

    public void AddDevice(DeviceEvent deviceEvent)
    {
        var _queue = _data.GetValueOrDefault(deviceEvent.Type, new ConcurrentQueue<DeviceEvent>());
        if (!_queue.TryPeek(out var lastDeviceEvent))
        {
            _queue.Enqueue(deviceEvent);
            _data[deviceEvent.Type] = _queue;
            return;
        }

        var difference = deviceEvent.Created.ToUniversalTime() - lastDeviceEvent.Created.ToUniversalTime();
        if (Math.Abs(difference.TotalSeconds) >= _intervalInSeconds)
        {
            var listToAggregate = new List<DeviceEvent>(_queue.Count);
            while (_queue.TryDequeue(out var item))
            {
                listToAggregate.Add(item);
            }

            var averageModel = _averageCalculator.Calculate(listToAggregate);
            _deviceAverageRepository.AddNewAverageModel(averageModel);
        }

        _queue.Enqueue(deviceEvent);
        _data[deviceEvent.Type] = _queue;
    }
    
    public IReadOnlyCollection<DeviceEventAverageModel> GetDataByInterval(DeviceEventType type,DateTimeOffset dateTimeStart, int intervalInSeconds)
    {
        if (intervalInSeconds % _intervalInSeconds != 0)
        {
            throw new InvalidOperationException("intervalInSeconds should be divided by _intervalInSeconds in settings");
        }
        var chunkSize= intervalInSeconds / _intervalInSeconds;

        var averages = _deviceAverageRepository.GetAverageModels(type)
            .Where(x=>x.Start>=dateTimeStart)
            .OrderBy(x=>x.Start);
        var result = averages.Chunk(chunkSize).Select(x => new DeviceEventAverageModel()
        {
            Finish = x.Last().Finish,
            Start = x.First().Start,
            Type = x.First().Type,
            AverageHumidity = x.Average(y => y.AverageHumidity),
            AverageTemperature = x.Average(y=>y.AverageTemperature),
            MaxCarbonDioxideLevel = x.Max(y=>y.MaxCarbonDioxideLevel),
            MinCarbonDioxideLevel = x.Max(y=>y.MinCarbonDioxideLevel)
        }).ToList();

        return result;
    }
}
