namespace DeviceDataGenerator.Data;

public sealed class DeviceEvent
{
    public int Temperature { get; set; }
    public int Humidity { get; set; }
    public int CarbonDioxideLevel { get; set; }

    public DeviceEventType Type { get; set; }
}