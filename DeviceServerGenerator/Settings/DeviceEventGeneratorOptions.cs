namespace DeviceServerGenerator.Settings;

public sealed class DeviceEventGeneratorOptions
{
    private static readonly TimeSpan _minValue = new (0, 0, 0, 0, 200);
    private static readonly TimeSpan _maxValue = new (0, 0, 2);
    
    private TimeSpan _timeSpan  = _maxValue;

    public TimeSpan Duration
    {
        get => _timeSpan;
        set
        {
            if (value > _maxValue
                || value < _minValue)
            {
                throw new ArgumentOutOfRangeException(nameof(Duration), value,
                    "Duration should be between 200 - 2000ms");
            }

            _timeSpan = value;
        }
    }
}