using Grpc.Core;
using GrpcClient.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GrpcClient.Services;

public class DeviceReceiver : IDeviceReceiver
{
    private readonly DeviceEventGrpcService.DeviceEventGrpcServiceClient _grpcServiceClient;
    private readonly IDeviceAggregator _deviceAggregator;
    private readonly IDeviceLoggerService _deviceLoggerService;
    private readonly ILogger<DeviceReceiver> _logger;
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public DeviceReceiver(DeviceEventGrpcService.DeviceEventGrpcServiceClient grpcServiceClient,
        IDeviceAggregator deviceAggregator,
        IDeviceLoggerService deviceLoggerService,
        ILogger<DeviceReceiver> logger)
    {
        _grpcServiceClient = grpcServiceClient;
        _deviceAggregator = deviceAggregator;
        _deviceLoggerService = deviceLoggerService;
        _logger = logger;
    }


    public async Task ExecuteAsync(IReadOnlyCollection<DeviceType> deviceTypes, CancellationToken stoppingToken)
    {
        while (!_cts.IsCancellationRequested)
        {
            var streamingCall = _grpcServiceClient.GetDeviceEvents(new DeviceEventRequest()
            {
                Types_ = { deviceTypes }
            });
            while (await streamingCall.ResponseStream.MoveNext(_cts.Token))
            {
                _logger.LogInformation("Received  {Current}", streamingCall.ResponseStream.Current);
                var deviceEvent = Mapper.MapToDeviceEvent(streamingCall.ResponseStream.Current);
                _deviceLoggerService.AddDevice(deviceEvent);
                _deviceAggregator.AddDevice(deviceEvent);
            }
        }
    }

    public Task StartAsync(IReadOnlyCollection<DeviceType> deviceTypes)
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        Task.Run(() => ExecuteAsync(deviceTypes, _cts.Token));
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _cts.Cancel();
        _cts.Dispose();
        return Task.CompletedTask;
    }
}