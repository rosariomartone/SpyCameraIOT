using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices;

namespace SpyCameraIOT.IOT
{
    public static class IOTMessages
    {
        static DeviceClient deviceClient;
        static string iotHubUri = App.IOTUrl;
        static string deviceKey = App.deviceKeyMobile;

        static ServiceClient serviceClient;
        public static async void SendDeviceToCloudMessagesAsync(string messageText)
        {
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageText));
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(App.deviceIDMobile, deviceKey));

            await deviceClient.SendEventAsync(message);
        }
        public async static Task SendCloudToDeviceMessageAsync(string messageText)
        {
            serviceClient = ServiceClient.CreateFromConnectionString("HostName=SpyCameraIOT.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=yYR677bWuhfAS2rDQ3FFBzRzWd3PvFQw+u4U/5AVzBI=");
            var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(messageText));

            await serviceClient.SendAsync(App.deviceIDMobile, commandMessage);
        }
        public async static Task<List<Device>> GetDevicesList()
        {
            var registryManager = RegistryManager.CreateFromConnectionString("HostName=SpyCameraIOT.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=bsiAKjVgsMcHYg033tEgkHqy0UOmhb51FV5a1pe+rBE=");
            var devices = await registryManager.GetDevicesAsync(10);
            
            return devices.ToList<Device>();
        }
    }

}