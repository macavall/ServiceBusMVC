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
        private static string? sbConnString;

        public SbService(IConfiguration config)
        {
            _config = config;

            AdminClientSetup();
        }

        public void AdminClientSetup()
        {
            SetConnectionString();
            sbAdminClient = new ServiceBusAdministrationClient(sbConnString);
        }

        public void SetConnectionString()
        {
            if (Environment.GetEnvironmentVariable("sbConnString")?.Count() < 5 || Environment.GetEnvironmentVariable("sbConnString") is null)
            {
                sbConnString = _config["sbConnString"];
            }
            else
            {
                sbConnString = Environment.GetEnvironmentVariable("sbConnString");
            }
        }

        public async Task<int> GetQueueMsgCount(string queueName)
        {
            long messageCount = 0;

            if (sbAdminClient is not null)
            {
                messageCount = (await sbAdminClient.GetQueueRuntimePropertiesAsync(queueName)).Value.ActiveMessageCount;
            }

            return Convert.ToInt32(messageCount);
        }

        // Schedule Message Count
        public async Task<int> GetQueueSchMsgCount(string queueName)
        {
            long scheduledMessageCount = 0;

            if (sbAdminClient is not null)
            {
                scheduledMessageCount = (await sbAdminClient.GetQueueRuntimePropertiesAsync(queueName)).Value.ScheduledMessageCount;
            }

            return Convert.ToInt32(scheduledMessageCount);
        }
    }
}