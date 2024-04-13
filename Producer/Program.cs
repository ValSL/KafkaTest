using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Producer;
using System.Text.Json;

var services = new ServiceCollection();

const string topicName = "sample-topic-2";
const string producerName = "say-hello";

services.AddKafka(
    kafka => kafka
        .UseConsoleLog()
        .AddCluster(
            cluster => cluster
                .WithBrokers(new[] { "localhost:9092" })
                .CreateTopicIfNotExists(topicName, 1, 1)
                .AddProducer(
                    producerName,
                    producer => producer
                        .DefaultTopic(topicName)
                        .AddMiddlewares(m =>
                            m.AddSerializer<JsonCoreSerializer>()
                        )
                )
        )
);

var serviceProvider = services.BuildServiceProvider();

var producer = serviceProvider
    .GetRequiredService<IProducerAccessor>()
    .GetProducer(producerName);

int num = 0;
while (true)
{
    await producer.ProduceAsync(
        topicName,
        Guid.NewGuid().ToString(),
        new HelloMessage { Text = num.ToString() });

    Console.WriteLine("Message sent!");
    num++;
    await Task.Delay(1000);
}
