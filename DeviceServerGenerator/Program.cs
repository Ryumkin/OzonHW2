using System.Runtime.InteropServices.ComTypes;
using DeviceDataGenerator;
using DeviceServerGenerator.GrpcServices;
using DeviceServerGenerator.Services;
using DeviceServerGenerator.Services.Interfaces;
using DeviceServerGenerator.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DeviceEventGeneratorOptions>(builder.Configuration.GetSection(nameof(DeviceEventGeneratorOptions)));
builder.Services.Configure<DeviceEventGrpcServiceOptions>(builder.Configuration.GetSection(nameof(DeviceEventGrpcServiceOptions)));
builder.Services.AddHostedService<DeviceEventGeneratorHostedService>();
builder.Services.AddSingleton<IDeviceService, DeviceService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

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
app.MapGrpcService<DeviceEventGrpcService>();
app.Run();