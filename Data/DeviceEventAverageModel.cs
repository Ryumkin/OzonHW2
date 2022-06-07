namespace DeviceDataGenerator.Data;

public sealed class DeviceEventAverageModel
{
    public double AverageTemperature { get; set; }
    public double AverageHumidity { get; set; }
    public int MaxCarbonDioxideLevel { get; set; }
    public int MinCarbonDioxideLevel { get; set; }

    public DeviceEventType Type { get; set; }
    
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset Finish { get; set; }
}