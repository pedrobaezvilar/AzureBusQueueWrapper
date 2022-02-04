using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusWrapper
{
    public static class AzureBusPublisher
    {
        
        public static Task Publish<TBody>(TBody body,string connectionString,string queueName)
        {
            return Task.Run(async () =>
            {
                ServiceBusClient client = default;
                ServiceBusSender sender = default;
                try
                {
                    client = new ServiceBusClient(connectionString);
                    sender = client.CreateSender(queueName);
                    string serializedBody = JsonConvert.SerializeObject(body);
                    ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(serializedBody));
                    await sender.SendMessageAsync(message);
                }
                finally
                {
                    await sender.DisposeAsync();
                    await client.DisposeAsync();
                }
            });
        }
    }
}
