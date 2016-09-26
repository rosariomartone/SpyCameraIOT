using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SpyCameraIOT.IOT
{
    public static class IOTMessages
    {
        static DeviceClient deviceClient;
        static string iotHubUri = App.IOTUrl;
        static string deviceKey = App.deviceKey;
        public static async void SendDeviceToCloudMessagesAsync(string messageText)
        {
            var message = new Message(Encoding.ASCII.GetBytes(messageText));
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(App.deviceID, deviceKey));

            await deviceClient.SendEventAsync(message);
        }
    }

}