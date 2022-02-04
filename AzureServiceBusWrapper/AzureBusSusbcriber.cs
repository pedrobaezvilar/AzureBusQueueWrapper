using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusWrapper
{
    public static class AzureBusSusbcriber
    {
        public static async Task Susbscribe(string connectionString,string queueName, Func<ProcessMessageEventArgs, Task> onSuccess, Func<ProcessErrorEventArgs, Task> onError)
        {

            ServiceBusClient client = new ServiceBusClient(connectionString);
            ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += onSuccess;

                // add handler to process any errors
                processor.ProcessErrorAsync += onError;

                // start processing 
                await processor.StartProcessingAsync();

                // stop processing 
                await processor.StopProcessingAsync();
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
