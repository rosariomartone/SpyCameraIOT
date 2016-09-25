using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System.Configuration;

namespace CreateDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = System.Configuration.ConfigurationSettings.AppSettings["IOTKey"].ToString();

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);

            addDevice();
        }

        private static void addDevice()
        {
            Console.WriteLine("Insert name o device:");
            AddDeviceAsync(Console.ReadLine()).Wait();

            addDevice();
        }

        private static async Task AddDeviceAsync(string deviceName)
        {
            
            Device device;

            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceName));
                Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceName);
                Console.WriteLine("Device already present with key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
            }

        }
    }
}
