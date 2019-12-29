using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using garagedoor.Shared;

namespace garagedoor.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class IMAGEController : ControllerBase
    {
        private readonly ILogger<IMAGEController> logger;
        
        public IMAGEController(
            ILogger<IMAGEController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public void Post([FromBody] bool refresh)
        {            
            logger.LogInformation("Will refresh Image");
        }

    }
}