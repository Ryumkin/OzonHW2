using DeviceDataGenerator;
using DeviceDataGenerator.Services.Interfaces;
using DeviceDataGenerator.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DeviceEventGeneratorOptions>(builder.Configuration.GetSection(nameof(DeviceEventGeneratorOptions)));
builder.Services.AddHostedService<DeviceEventGeneratorHostedService>();
builder.Services.AddSingleton<IDeviceService, DeviceService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();