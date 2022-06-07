using DeviceDataGenerator.Data;
using GrpcClient;
using GrpcClient.Services;
using GrpcClient.Utils;
using GrpcClient.WebApiModels;
using Microsoft.AspNetCore.Mvc;

namespace DeviceServerGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceAggregatorController : ControllerBase
    {
        private readonly IDeviceAggregator _deviceAggregator;

        public DeviceAggregatorController(IDeviceAggregator deviceAggregator)
        {
            _deviceAggregator = deviceAggregator;
        }
        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<DeviceEvent>> GetAggregateData(DateTimeOffset startDateTime, int intervalInSeconds, DeviceTypeEventApi type)
        {
            if (type == DeviceTypeEventApi.Unknown)
            {
                return BadRequest("Unknown type is not supported");
            }

            var result = _deviceAggregator.GetDataByInterval(Mapper.MapToDeviceEventType(type),startDateTime,intervalInSeconds);
            return Ok(result);
        }
      
    }
}
