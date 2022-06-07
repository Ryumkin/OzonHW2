using DeviceDataGenerator.Data;
using DeviceServerGenerator.Services.Interfaces;
using DeviceServerGenerator.Settings;
using Microsoft.Extensions.Options;

namespace DeviceServerGenerator.Services;

public class DeviceEventGeneratorHostedService: BackgroundService
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DeviceEventGeneratorHostedService> _logger;
    private readonly DeviceEventGeneratorOptions _settings;

    public DeviceEventGeneratorHostedService(IOptions<DeviceEventGeneratorOptions> options, IDeviceService deviceService,
        ILogger<DeviceEventGeneratorHostedService> logger)
    {
        _deviceService = deviceService;
        _logger = logger;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        
        try
        {
            while (!cts.IsCancellationRequested)
            {
                _deviceService.Enqueue(GenerateDeviceEvent());
                await Task.Delay(_settings.Duration, cts.Token);
            }
        }
        catch (TaskCanceledException exception)
        {
            _logger.LogInformation(exception,"Handled Exception, DeviceGenerator has been stopped by cancellation");
        }

        _logger.LogInformation("DeviceEventGeneratorHostedService has stopped");
    }

    private static DeviceEvent GenerateDeviceEvent()
    {
        var random = new Random();
        return new DeviceEvent()
        {
            Humidity = random.Next(50,90),
            CarbonDioxideLevel = random.Next(70,100),
            Temperature = random.Next(15,40),
            Type = (DeviceEventType)random.Next(1,3),
            Created = DateTimeOffset.UtcNow
        };
    }
}