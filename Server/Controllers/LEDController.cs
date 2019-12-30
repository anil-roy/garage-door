using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using garagedoor.Shared;
using Microsoft.AspNetCore.Identity;

namespace garagedoor.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LEDController : ControllerBase
    {
        private readonly int ledPin = 17;
        private readonly ILogger<LEDController> logger;
        private readonly GpioController _controller;

        
        public LEDController(
            ILogger<LEDController> logger,
            GpioController controller
            )
        {
            _controller = controller;
            this.logger = logger;
            
            if (!controller.IsPinOpen(ledPin))
            {
                controller.OpenPin(ledPin, PinMode.Output);
            }
        }

        [HttpPost]
        public void Post([FromBody] bool toggle)
        {            
            logger.LogInformation("Turning ON PIN");
            _controller.Write(ledPin, PinValue.High);
            Task.Delay(1000).Wait();
            logger.LogInformation("Turning OFF PIN");
            _controller.Write(ledPin, PinValue.Low);
        }

    }
}