using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBusMVC
{
    public class SbService : ISbService
    {
        private readonly IConfiguration _config;
        private static ServiceBusAdministrationClient? sbAdminClient;
        private static string? sbConnString;
        private static DateTime myTime = DateTime.UtcNow.AddMinutes(5);

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

        public async Task PopulateSB(string queueName)
        {
            // Create a client that will authenticate using a connection string
            var client = new ServiceBusClient(sbConnString);

            // Total messages to send
            int totalMessages = 300;
            int batchSize = 20;
            int numberOfTasks = 20;
            int messagesPerTask = totalMessages / numberOfTasks;

            // Create and start tasks
            var tasks = new List<Task>();
            for (int taskNum = 0; taskNum < numberOfTasks; taskNum++)
            {
                int start = taskNum * messagesPerTask;
                int end = (taskNum + 1) * messagesPerTask;
                tasks.Add(SendMessagesBatch(client, queueName, start, end, batchSize));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // Clean up resources by closing the client. This is important to free up resources.
            await client.DisposeAsync();
        }

        private static async Task SendMessagesBatch(ServiceBusClient client, string queueName, int start, int end, int batchSize)
        {
            // Create a sender for the queue
            var sender = client.CreateSender(queueName);

            try
            {
                for (int i = start; i < end; i += batchSize)
                {
                    using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
                    for (int j = i; j < i + batchSize && j < end; j++)
                    {
                        var message = new ServiceBusMessage($"Message {j}")
                        {
                            ScheduledEnqueueTime = myTime
                        };

                        if (!messageBatch.TryAddMessage(message))
                        {
                            break; // If the batch is full, we can't add more messages to this batch
                        }
                    }

                    await sender.SendMessagesAsync(messageBatch);
                    Console.WriteLine($"Task {Task.CurrentId}: Sent batch starting at message {i}, scheduled for 5 minutes in the future");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);

                Console.WriteLine("Trying Again!!!");

                for (int i = start; i < end; i += batchSize)
                {
                    using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
                    for (int j = i; j < i + batchSize && j < end; j++)
                    {
                        var message = new ServiceBusMessage($"Message {j}")
                        {
                            ScheduledEnqueueTime = myTime
                        };

                        if (!messageBatch.TryAddMessage(message))
                        {
                            break; // If the batch is full, we can't add more messages to this batch
                        }
                    }

                    await sender.SendMessagesAsync(messageBatch);
                    Console.WriteLine($"Task {Task.CurrentId}: Sent batch starting at message {i}, scheduled for 5 minutes in the future");
                }
            }
            finally
            {
                await sender.DisposeAsync();
            }
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