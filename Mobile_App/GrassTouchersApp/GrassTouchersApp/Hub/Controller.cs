using Microsoft.Azure.Devices;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrassTouchersApp.Hub
{
    public class Controller
    {
        private readonly ServiceClient client;

        public Controller()
        {
            client = ServiceClient.CreateFromConnectionString(Config.IotHubConnectionString);
            Task.Run(() => client.OpenAsync());
        }

        public CloudToDeviceMethodResult Invoke(string methodName, Dictionary<string, string> payload)
        {
            var method = new CloudToDeviceMethod(methodName);

            var sb = new StringBuilder();

            sb.Append("{");

            foreach (var pair in payload)
            {
                sb.Append($" \"{pair.Key}\": \"{pair.Value}\", ");
            }

            sb.Append("}");

            method.SetPayloadJson(sb.ToString());

            var response = Task.Run(async () =>
            {
                return await client.InvokeDeviceMethodAsync(
                  Config.IotHubDeviceId,
                  Config.IotHubModuleId,
                  method
                );
            }).Result;

            return response;
        }
    }
}
