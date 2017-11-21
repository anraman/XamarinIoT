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
        static string DeviceConnectionString = Settings.Settings.DeviceConnStriOS;
        static DeviceClient Client = null;
        static TwinCollection reportedProperties = new TwinCollection();

        public static async void InitClient()
        {
            try
            {
                Debug.WriteLine("Connecting to hub");
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString);
                }
                else
                {
                    Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
                }
                Debug.WriteLine("Retrieving twin");
                await Client.GetTwinAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in sample: " + ex.Message);
            }
        }

        public static async void ReportConnectivity()
        {
            try
            {
                Debug.WriteLine("Sending connectivity data as reported property");

                TwinCollection reportedProperties, connectivity;
                reportedProperties = new TwinCollection();
                connectivity = new TwinCollection();
                connectivity["type"] = "cellular";
                reportedProperties["connectivity"] = connectivity;
                await Client.UpdateReportedPropertiesAsync(reportedProperties);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in sample: " + ex.Message);
            }
        }

        public static async void InitTelemetry()
        {
            try
            {
                Debug.WriteLine("Report initial telemetry config:");
                TwinCollection telemetryConfig = new TwinCollection();

                telemetryConfig["configId"] = "0";
                telemetryConfig["sendFrequency"] = "24h";
                reportedProperties["telemetryConfig"] = telemetryConfig;
                Debug.WriteLine(JsonConvert.SerializeObject(reportedProperties));

                await Client.UpdateReportedPropertiesAsync(reportedProperties);
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {

                    Debug.WriteLine("Error in sample: " + exception);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Error in sample: " + ex.Message);
            }
        }

        private static async Task OnDesiredPropertyChanged(TwinCollection desiredProperties, object userContext)
        {
            try
            {
                Debug.WriteLine("Desired property change:");
                Debug.WriteLine(JsonConvert.SerializeObject(desiredProperties));

                var currentTelemetryConfig = reportedProperties["telemetryConfig"];
                var desiredTelemetryConfig = desiredProperties["telemetryConfig"];

                if ((desiredTelemetryConfig != null) && (desiredTelemetryConfig["configId"] != currentTelemetryConfig["configId"]))
                {
                    Debug.WriteLine("\nInitiating config change");
                    currentTelemetryConfig["status"] = "Pending";
                    currentTelemetryConfig["pendingConfig"] = desiredTelemetryConfig;

                    await Client.UpdateReportedPropertiesAsync(reportedProperties);

                    CompleteConfigChange();
                }
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {

                    Debug.WriteLine("Error in sample: " + exception);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Error in sample: " + ex.Message);
            }
        }

        public static async void CompleteConfigChange()
        {
            try
            {
                var currentTelemetryConfig = reportedProperties["telemetryConfig"];

                Debug.WriteLine("\nSimulating device reset");
                await Task.Delay(30000);

                Debug.WriteLine("\nCompleting config change");
                currentTelemetryConfig["configId"] = currentTelemetryConfig["pendingConfig"]["configId"];
                currentTelemetryConfig["sendFrequency"] = currentTelemetryConfig["pendingConfig"]["sendFrequency"];
                currentTelemetryConfig["status"] = "Success";
                currentTelemetryConfig["pendingConfig"] = null;

                await Client.UpdateReportedPropertiesAsync(reportedProperties);
                Debug.WriteLine("Config change complete \nPress any key to exit.");
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {

                    Debug.WriteLine("Error in sample: " + exception);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Error in sample: " + ex.Message);
            }
        }

        public MainPage()
        {
            InitializeComponent();
            try
            {
                InitClient();
                InitTelemetry();
                ReportConnectivity();
                Client.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChanged, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in sample: " + ex.Message);
            }
        }
    }
}
