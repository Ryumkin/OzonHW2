using DeviceDataGenerator.Data;
using DeviceServerGenerator.Services.Interfaces;
using DeviceServerGenerator.Settings;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcClient;
using Microsoft.Extensions.Options;

namespace DeviceServerGenerator.GrpcServices;

public class DeviceEventGrpcService : GrpcClient.DeviceEventGrpcService.DeviceEventGrpcServiceBase
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DeviceEventGrpcService> _logger;
    private readonly DeviceEventGrpcServiceOptions _options;

    public DeviceEventGrpcService(IDeviceService deviceService, ILogger<DeviceEventGrpcService> logger, IOptions<DeviceEventGrpcServiceOptions> options)
    {
        _deviceService = deviceService;
        _logger = logger;
        _options = options.Value;
    }
    
    

    public override async Task GetDeviceEvents(DeviceEventRequest request, IServerStreamWriter<DeviceEventResponse> responseStream, ServerCallContext context)
    {
        var types = request.Types_.Select(MapToDeviceEventType).ToList();
        try
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                foreach (var deviceEvent in _deviceService.GetAllData(types))
                {
                    await responseStream.WriteAsync(MapToDeviceEventResponse(deviceEvent));
                }
                
                await Task.Delay(_options.Duration, context.CancellationToken);
            }
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogWarning(exception,"An operation was canceled");
        }
    }

    private static DeviceEventType MapToDeviceEventType(DeviceType deviceEventType) => deviceEventType switch
    {
        DeviceType.Inside => DeviceEventType.Inside,
        DeviceType.Outside => DeviceEventType.Outside,
        DeviceType.Unknown => throw new ArgumentOutOfRangeException(nameof(deviceEventType)),
        _ => throw new ArgumentOutOfRangeException(nameof(deviceEventType))
    };

    private static DeviceType MapToDeviceType(DeviceEventType deviceEventType) => deviceEventType switch
    {
        DeviceEventType.Inside => DeviceType.Inside,
        DeviceEventType.Outside => DeviceType.Outside,
        DeviceEventType.Unknown => throw new ArgumentOutOfRangeException(nameof(deviceEventType)),
        _ => throw new ArgumentOutOfRangeException(nameof(deviceEventType))
    };
    
    private static DeviceEventResponse MapToDeviceEventResponse(DeviceEvent deviceEvent)
    {
        return new DeviceEventResponse()
        {
            Humidity = deviceEvent.Humidity,
            Temperature = deviceEvent.Temperature,
            CarbonDioxideLevel = deviceEvent.CarbonDioxideLevel,
            Type = MapToDeviceType(deviceEvent.Type),
            Created = Timestamp.FromDateTimeOffset(deviceEvent.Created)
        };
    }
}