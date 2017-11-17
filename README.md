# Xamarin IoT
Sample Xamarin projects to demonstrate how to use Device Twins with Xamarin.Forms apps. All samples are built using the following two tutorials with modifications made for compatibility with Xamarin.Forms:

- [Get started with device twins (.NET/.NET)](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-csharp-csharp-twin-getstarted)

- [Use desired properties to configure devices](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-csharp-csharp-twin-how-to-configure)

## Project Descriptions

- **ConsoleIoTService:** Console project for setting desired configuration
- **ConsoleIoT:** Simulated device app. Used as a control.
- **XamarinIoTPCL:** PCL based Xamarin.Forms device app. This does not work as MQTT is not supported in PCL projects.
- **XamarinIoTShared:** Shared project based Xamarin.Forms device app. This does work.
- **XamarinIoTStandard:** .NET Standard-based based Xamarin.Forms device app. This project was originally a PCL-based project which was converted to .NET Standard. This also works.