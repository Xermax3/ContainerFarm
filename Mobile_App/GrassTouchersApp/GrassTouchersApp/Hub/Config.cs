/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * IoTHub related values and keys.
*/

namespace GrassTouchersApp.Hub
{
    /// <summary> IoTHub related values and keys. </summary>
    public static class Config
    {
        public static string EventHubConnectionString = "Endpoint=sb://ihsuprodblres094dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=FHi9mLIpTlAZC3im336ASe43wO7e4Ydx+WrY4BcLe2k=;EntityPath=iothub-ehub-420-6p3-18203029-88a4b3216e";
        public static string EventHubName = "iothub-ehub-420-6p3-18203029-88a4b3216e";
        public static string IotHubConnectionString = "HostName=420-6P3.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=FHi9mLIpTlAZC3im336ASe43wO7e4Ydx+WrY4BcLe2k=";
        public static string IotHubDeviceConnectionString = "HostName=420-6P3.azure-devices.net;DeviceId=Grass-Touchers;SharedAccessKey=8+Um5Wtl6vfV0wVfukg1wE3nh4fElnEXz1BaQFkEOqI=";
        public static string IotHubDeviceId = "device-1-lab8";
        public static string IotHubModuleId = "grass-touchers";
    }
}