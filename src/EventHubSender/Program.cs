using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

int numOfEvents = 3;

EventHubProducerClient producerClient = new EventHubProducerClient("", "myeventhub");
using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
for (int i = 1; i < numOfEvents; i++)
{
    if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
    {
        throw new Exception($"Event {i} is tool large to batch and cannot be sent");
    }
}

try
{
    await producerClient.SendAsync(eventBatch);
    Console.WriteLine($"A batch of {numOfEvents} events has been published.");
    Console.ReadLine();
}
finally
{
    await producerClient.DisposeAsync();
}