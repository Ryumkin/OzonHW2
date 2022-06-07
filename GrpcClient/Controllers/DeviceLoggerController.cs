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
    public class DeviceLoggerController : ControllerBase
    {
        private readonly IDeviceLoggerService _deviceLoggerService;

        public DeviceLoggerController(IDeviceLoggerService deviceLoggerService)
        {
            _deviceLoggerService = deviceLoggerService;
        }
        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<DeviceEvent>> GetDeviceEvents(DeviceTypeEventApi type)
        {
            if (type == DeviceTypeEventApi.Unknown)
            {
                return BadRequest("Unknown type is not supported");
            }
            
            var result = _deviceLoggerService.GetDataByInterval(Mapper.MapToDeviceEventType(type));
            return Ok(result);
        }
      
    }
}
