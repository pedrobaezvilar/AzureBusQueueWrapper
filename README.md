# AzureBusQueueWrapper
This library provided a wrapper to suscribe and broadcast azure bus queue events

## Setting up the publisher
```csharp
 class Program
    {
        static string connectionString = "Endpoint=sb://xxxxxxxxxx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxxxxxxxxxx";

        // name of your Service Bus queue
        static string queueName = "queueName";

        static async Task Main()
        {
            await AzureBusPublisher.Publish(new Person { name = "Jane", age = 30}, connectionString,queueName);
          
            await AzureBusPublisher.Publish(new Person { name = "Jhon", age = 55}, connectionString, queueName);
          
            await AzureBusPublisher.Publish(new Car { make = "Toyota", year = 2020, owner = new Person { name = "Jhon", age = 55 } }, connectionString, queueName);

        }
    }
    public class Person
    {
        public string name { get; set; }
        public int age { get; set; }
    }
    public class Car
    {
        public string make { get; set; }
        public int year{ get; set; }
        public Person owner { get; set; }
    }
```

## Setting up the subscriber
```csharp
    class Program
    {
        static readonly string connectionString = "Endpoint=sb://xxxxxxxx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxxxxxxx";
        static readonly string queueName = "queueName";

        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

       
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        static async Task Main()
        {
            try
            {
                await AzureServiceBusWrapper.AzureBusSusbcriber.Susbscribe(connectionString, queueName, MessageHandler, ErrorHandler);
              
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
```

