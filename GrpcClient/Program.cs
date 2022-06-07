
using DeviceServerGenerator.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using GrpcClient;
using GrpcClient.Repositories;
using GrpcClient.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DeviceAggregatorOptions>(builder.Configuration.GetSection(nameof(DeviceAggregatorOptions)));
builder.Services.AddSingleton<IDeviceReceiver, DeviceReceiver>();
builder.Services.AddSingleton<IDeviceAverageCalculator, DeviceAverageCalculator>();
builder.Services.AddSingleton<IDeviceAverageRepository, DeviceAverageRepository>();
builder.Services.AddSingleton<IDeviceAggregator, DeviceAggregator>();
builder.Services.AddSingleton<IDeviceLoggerRepository, DeviceLoggerRepository>();
builder.Services.AddSingleton<IDeviceLoggerService, DeviceLoggerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

var defaultMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(60),
        BackoffMultiplier = 2,
        RetryableStatusCodes = { StatusCode.Unavailable, StatusCode.Aborted}
    }
};
builder.Services.AddGrpcClient<DeviceEventGrpcService.DeviceEventGrpcServiceClient>(
    options =>
    {
        options.Address = new Uri("https://localhost:7132");
        options.ChannelOptionsActions.Add(x =>
        {
            x.ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } };
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();