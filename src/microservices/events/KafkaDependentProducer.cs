using Confluent.Kafka;

namespace EventsService
{
    /// <summary>
    ///     Leverages the injected KafkaClientHandle instance to allow
    ///     Confluent.Kafka.Message{TKey,TValue}s to be produced to Kafka.
    /// </summary>
    public class KafkaDependentProducer<TKey, TValue>(KafkaClientHandle handle)
    {
        private readonly IProducer<TKey, TValue> _kafkaHandle = new DependentProducerBuilder<TKey, TValue>(handle.Handle).Build();

        /// <summary>
        ///     Asychronously produce a message and expose delivery information
        ///     via the returned Task. Use this method of producing if you would
        ///     like to await the result before flow of execution continues.
        /// <summary>
        public Task ProduceAsync(string topic, Message<TKey, TValue> message)
            => _kafkaHandle.ProduceAsync(topic, message);

        /// <summary>
        ///     Asynchronously produce a message and expose delivery information
        ///     via the provided callback function. Use this method of producing
        ///     if you would like flow of execution to continue immediately, and
        ///     handle delivery information out-of-band.
        /// </summary>
        public void Produce(string topic, Message<TKey, TValue> message, Action<DeliveryReport<TKey, TValue>>? deliveryHandler = null)
            => _kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout)
            => _kafkaHandle.Flush(timeout);
    }
}
