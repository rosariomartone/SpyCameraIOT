using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyCameraIOT.Models
{
    public class IOTDevice: Device
    {
        public bool IsEnabled { get; set; }

        public DateTime LastExecutionTime { get; set; }

        public string ConnectionStateIOT { get; set; }

        public string IdIOT { get; set; }

        public string IsDeviceHome { get; set; }
    }
}
