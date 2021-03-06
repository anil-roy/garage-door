using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using garagedoor.Shared;
using MMALSharp;
using MMALSharp.Common.Utility;
using MMALSharp.Handlers;
using MMALSharp.Native;

namespace garagedoor.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class IMAGEController : ControllerBase
    {
        private readonly ILogger<IMAGEController> logger;
        private MMALCamera cam;
             
        public IMAGEController(
            ILogger<IMAGEController> logger)
        {
            this.logger = logger;
            this.cam = MMALCamera.Instance;
            MMALCameraConfig.StillResolution = new Resolution(640, 480); 
        }

        [HttpPost]
        public async Task<ImageDetails> Post([FromBody] bool refresh)
        {            

            String fileName="NotAnImage.jpg";
            logger.LogInformation("Will refresh Image");

            using (var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/doorimages/", "jpg"))        
            {            
                await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
                fileName = imgCaptureHandler.GetFilename(); 
                logger.LogInformation(fileName);
            }
            ImageDetails imageDetails = new ImageDetails();
            imageDetails.Name = "/doorimages/" + Uri.EscapeDataString (fileName) + ".jpg";
            return imageDetails;
        }

    }
}