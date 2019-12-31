using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO; 
using System;

namespace garagedoor
{
    internal class PiMonitorWorker : BackgroundService
    {        
        private readonly ILogger<PiMonitorWorker> logger;

        public PiMonitorWorker(ILogger<PiMonitorWorker> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {               
                logger.LogInformation("Executing...");
                // TODO: make this an app setting
                string[] files = Directory.GetFiles("/home/pi/doorimages/");

                foreach (string file in files)
                {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                    fi.Delete();
                }

                //Run the background service for every hour 
                await Task.Delay(3600000);
            }
        }
    }
}