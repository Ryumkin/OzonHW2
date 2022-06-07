using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceDataGenerator.Data;
using DeviceDataGenerator.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceServerGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceEventController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceEventController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }
        [HttpGet]
        public ActionResult<DeviceEvent> Get(DeviceEventType deviceEventType)
        {
            if (deviceEventType == DeviceEventType.Unknown)
            {
                return BadRequest($"Unsupported deviceEventType = {deviceEventType}");
            }
            var deviceEvent = _deviceService.GetLastDeviceEvent(deviceEventType);
            if (deviceEvent is null)
            {
                return NotFound();
            }

            return Ok(deviceEvent);
        }
    }
}
