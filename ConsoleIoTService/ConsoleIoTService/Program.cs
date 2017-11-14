using Microsoft.Azure.Devices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleIoTService
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = Settings.Settings.IoTHubConnStr;

        public static async Task AddTagsAndQuery()
        {
            var twin = await registryManager.GetTwinAsync("testDeviceStandard");
            var patch =
                @"{
             tags: {
                 location: {
                     region: 'US',
                     plant: 'Redmond43'
                 }
             }
         }";
            await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);

            var query = registryManager.CreateQuery("SELECT * FROM devices WHERE tags.location.plant = 'Redmond43'", 100);
            var twinsInRedmond43 = await query.GetNextAsTwinAsync();
            Console.WriteLine("Devices in Redmond43: {0}", string.Join(", ", twinsInRedmond43.Select(t => t.DeviceId)));

            query = registryManager.CreateQuery("SELECT * FROM devices WHERE tags.location.plant = 'Redmond43' AND properties.reported.connectivity.type = 'cellular'", 100);
            var twinsInRedmond43UsingCellular = await query.GetNextAsTwinAsync();
            Console.WriteLine("Devices in Redmond43 using cellular network: {0}", string.Join(", ", twinsInRedmond43UsingCellular.Select(t => t.DeviceId)));
        }

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddTagsAndQuery().Wait();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
