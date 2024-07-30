using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Extensions.Azure;

await using (var consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, "", "myeventhub"))
{
    using var cancellationSource = new CancellationTokenSource();
    cancellationSource.CancelAfter(TimeSpan.FromSeconds(60));
    await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
    {
        Console.WriteLine($"event received: {receivedEvent.Data.ToString()}");
    }
}

