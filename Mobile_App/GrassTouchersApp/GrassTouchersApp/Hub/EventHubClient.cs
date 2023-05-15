/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * IoTHub related values and keys.
*/

using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace GrassTouchersApp.Hub
{
    /// <summary> IoTHub client that retrieves data from an event hub. </summary>
    public class EventHubClient
    {
        private readonly EventHubConsumerClient consumer;

        /// <summary> Generate a new client using Config values. </summary>
        public EventHubClient()
        {
            consumer = new EventHubConsumerClient(
              EventHubConsumerClient.DefaultConsumerGroupName,
              Config.EventHubConnectionString,
              Config.EventHubName
            );
        }

        /// <summary>
        /// Listen on all event hub partitions for incoming events and 
        /// yield the deserialized body for each of them.
        /// </summary>
        /// <returns> The deserialized event body for incoming events. </returns>
        public async IAsyncEnumerable<T> Read<T>()
        {
            await foreach (PartitionEvent e in consumer.ReadEventsAsync())
            {
                T model;

                try
                {
                    model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(e.Data.EventBody.ToArray()));
                }
                catch
                {
                    continue;
                }

                yield return model;
            }
        }

        /// <summary>
        /// Allows for the desired telemetry interval to be updated within the app.  It gets the device twin of the Pi, sets the property and the updates it.
        /// </summary>
        /// <param name="interval">An integer that the user has chosen that will now represent the new interval telemetry</param>
        public async void UpdateTelemetryInterval(int interval=5)
        {
            try
            {
                // It gets the given IOT hub using it's connection string
                var registry_manager = RegistryManager.CreateFromConnectionString(Config.IotHubConnectionString);
                // Gets the device twin, in this case of our Rasperry Pi using its Id
                var device_twin = await registry_manager.GetTwinAsync(Config.IotHubDeviceId);
                // Sets the telemetryInterval property to the passed interval
                device_twin.Properties.Desired["telemetryInterval"] = interval;
                // Updates the desired property in IOT hub
                await registry_manager.UpdateTwinAsync(Config.IotHubDeviceId, device_twin, device_twin.ETag);
            }
            catch
            {
                //An error message in the case that one of these fetches or operations fail.
                Debug.WriteLine($"Unable to update desired property telemetryInterval on device : {Config.IotHubDeviceId}");
            }
        }

        public async  Task<int> GetTelemetryInterval()
        {
            int interval = 0;
            try
            {
                //It gets the given IOT hub using it's connection string
                var registry_manager = RegistryManager.CreateFromConnectionString(Config.IotHubConnectionString);
                //Gets the device twin, in this case of our Rasperry Pi using it's id.
                var device_twin = await registry_manager.GetTwinAsync(Config.IotHubDeviceId);
                //sets the telemetryIntervalProperty to the passed interval
                interval = device_twin.Properties.Desired["telemetryInterval"];
                //Updates the desired property in IOT hub
                await registry_manager.UpdateTwinAsync(Config.IotHubDeviceId, device_twin, device_twin.ETag);
            }
            catch
            {
                //An error message in the case that one of these fetches or operations fail.
                Debug.WriteLine($"Unable to update desired property telemetryInterval on device : {Config.IotHubDeviceId}");
            }
            return interval;
        }
    }
}