namespace GrpcClient.Services;

public interface IDeviceReceiver
{
    Task ExecuteAsync(IReadOnlyCollection<DeviceType> deviceTypes, CancellationToken stoppingToken);
    Task StartAsync(IReadOnlyCollection<DeviceType> deviceTypes);
    Task StopAsync();
}