using DeviceDataGenerator.Data;

namespace GrpcClient.Services;

public class DeviceAverageCalculator : IDeviceAverageCalculator
{
    public DeviceEventAverageModel Calculate(IReadOnlyCollection<DeviceEvent> deviceEvents)
    {
        return new DeviceEventAverageModel()
        {
            MinCarbonDioxideLevel = deviceEvents.Select(x => x.CarbonDioxideLevel).Min(),
            MaxCarbonDioxideLevel = deviceEvents.Select(x => x.CarbonDioxideLevel).Max(),
            AverageHumidity = deviceEvents.Average(x => x.Humidity),
            AverageTemperature = deviceEvents.Average(x => x.Temperature),
            Type = deviceEvents.First().Type,
            Start = deviceEvents.Select(x => x.Created).Min(),
            Finish = deviceEvents.Select(x => x.Created).Max()
        };
    }
}