using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBusMVC
{
    public class SbService : ISbService
    {
        private readonly IConfiguration _config;
        private static ServiceBusAdministrationClient? sbAdminClient;

        public SbService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> GetQueueMsgCount()
        {
            await Task.Delay(1);
            string? sbConnString = String.Empty;

            if (Environment.GetEnvironmentVariable("sbConnString")?.Count() < 5 || Environment.GetEnvironmentVariable("sbConnString") is null)
            {
                sbConnString = _config["sbConnString"];
            }
            else
            {
                sbConnString = Environment.GetEnvironmentVariable("sbConnString");
            }

            var administrationClient = new ServiceBusAdministrationClient(sbConnString);
            var props = await administrationClient.GetQueueRuntimePropertiesAsync("myqueue");
            var messageCount = props.Value.ActiveMessageCount;
            var scheduledMessageCount = props.Value.ScheduledMessageCount;

            return Convert.ToInt32(messageCount);
        }
    }
}