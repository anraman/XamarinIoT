# Using Azure IoT Hub Device Twins with Xamarin.Forms Apps

>For a full description of the problem/solution, please refer to [this]() blog post.

Sample Xamarin projects demonstrating how to use Device Twins with Xamarin.Forms apps. All samples are based on the following two tutorials (with modifications made for compatibility with Xamarin.Forms):

- [Get started with device twins (.NET/.NET)](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-csharp-csharp-twin-getstarted)

- [Use desired properties to configure devices](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-csharp-csharp-twin-how-to-configure)

## Solution Details

There are several projects included with this repo. Summary is as follows:
### Project Descriptions

- **ConsoleIoTService:** Console project for setting desired configuration.
- **ConsoleIoT:** Simulated device app. Used as a control.
- **XamarinIoTShared:** Shared project-based Xamarin.Forms device app.
- **XamarinIoTStandard:** .NET Standard-based based Xamarin.Forms device app. This project was originally a PCL-based project that was migrated to .NET Standard.