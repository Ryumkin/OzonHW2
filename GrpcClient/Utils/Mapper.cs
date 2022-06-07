using DeviceDataGenerator.Data;
using GrpcClient.WebApiModels;

namespace GrpcClient.Utils;

public static class Mapper
{
    public static DeviceType MapToDeviceType(DeviceTypeEventApi type)
    {
        return type switch
        {
            DeviceTypeEventApi.Inside => DeviceType.Inside,
            DeviceTypeEventApi.Outside => DeviceType.Outside,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
    
    public static DeviceEventType MapToDeviceEventType(DeviceTypeEventApi type)
    {
        return type switch
        {
            DeviceTypeEventApi.Inside => DeviceEventType.Inside,
            DeviceTypeEventApi.Outside => DeviceEventType.Outside,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
    
    public static DeviceEvent MapToDeviceEvent(DeviceEventResponse response)
    {
        return new DeviceEvent()
        {
            Created = response.Created.ToDateTimeOffset(),
            Humidity = response.Humidity,
            Temperature = response.Temperature,
            Type = MapToDeviceType(response.Type),
            CarbonDioxideLevel = response.CarbonDioxideLevel
        };
    }
    
    private static DeviceEventType MapToDeviceType(DeviceType deviceType) => deviceType switch
    {
        DeviceType.Inside => DeviceEventType.Inside,
        DeviceType.Outside => DeviceEventType.Outside,
        _ => throw new ArgumentOutOfRangeException(nameof(deviceType))
    };
}