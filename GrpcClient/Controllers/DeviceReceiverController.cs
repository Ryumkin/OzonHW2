using GrpcClient;
using GrpcClient.Services;
using GrpcClient.Utils;
using GrpcClient.WebApiModels;
using Microsoft.AspNetCore.Mvc;

namespace DeviceServerGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceReceiverController : ControllerBase
    {
        private readonly IDeviceReceiver _deviceReceiver;

        public DeviceReceiverController(IDeviceReceiver deviceReceiver)
        {
            _deviceReceiver = deviceReceiver;
        }
        
        [HttpPost("start")]
        public async Task<ActionResult> StartAggregating([FromBody]IReadOnlyCollection<DeviceTypeEventApi> types)
        {
            var filteredTypes = new List<DeviceType>(types.Count);
            foreach (var type in types)
            {
                if (type == DeviceTypeEventApi.Unknown)
                {
                    return BadRequest("Unknown type is not supported");
                }
                filteredTypes.Add(Mapper.MapToDeviceType(type));
            }
            await _deviceReceiver.StartAsync(filteredTypes);
            return Ok();
        }
        
        [HttpPost("stop")]
        public async Task<ActionResult> StopAggregating()
        {
            await _deviceReceiver.StopAsync();
            return Ok();
        }
    }
}
