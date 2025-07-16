using Confluent.Kafka;

namespace EventsService.Consumers
{
    public abstract class ConsumerBase<TKey, TValue> : BackgroundService
    {
        private readonly string _topic;
        private readonly IConsumer<TKey, TValue> _kafkaConsumer;

        protected abstract string TopicSettingKey { get; }

        public ConsumerBase(IConfiguration config)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = config.GetSection("KAFKA_BROKERS")!.Value,
                GroupId = "events_group"
            };
            _topic = config.GetValue<string>(TopicSettingKey)!;
            _kafkaConsumer = new ConsumerBuilder<TKey, TValue>(consumerConfig).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _kafkaConsumer.Consume(cancellationToken);
                    Consume(cr);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }

        protected virtual void Consume(ConsumeResult<TKey, TValue> result)
        {
            // Handle message...
            Console.WriteLine($"{result.Message.Key}: {result.Message.Value}");
        }

        public override void Dispose()
        {
            _kafkaConsumer.Close(); // Commit offsets and leave the group cleanly.
            _kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}
