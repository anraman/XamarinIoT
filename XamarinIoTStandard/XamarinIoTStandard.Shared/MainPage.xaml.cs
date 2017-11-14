using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System.Diagnostics;

namespace XamarinIoTStandard
{
    public partial class MainPage : ContentPage
    {
        static string DeviceConnectionString = Settings.Settings.DeviceConnStrStandard;
        static DeviceClient Client = null;

        public static async void InitClient()
        {

                Debug.WriteLine("Connecting to hub");
                Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
                Debug.WriteLine("Retrieving twin");
                await Client.GetTwinAsync();

        }

        public static async void ReportConnectivity()
        {

                Debug.WriteLine("Sending connectivity data as reported property");

                TwinCollection reportedProperties, connectivity;
                reportedProperties = new TwinCollection();
                connectivity = new TwinCollection
                {
                    ["type"] = "cellular"
                };
                reportedProperties["connectivity"] = connectivity;
                await Client.UpdateReportedPropertiesAsync(reportedProperties);
          
        }

        public MainPage()
        {
            InitializeComponent();
           
                InitClient();
                ReportConnectivity();
        }
    }
}
