using Confluent.Kafka;
using EventsService.Models;
using Newtonsoft.Json;

namespace EventsService.Consumers
{
    public class PaymentConsumer(IConfiguration config, ILogger<PaymentConsumer> logger) : ConsumerBase<string, string>(config, logger)
    {
        protected override string TopicSettingKey => "PaymentTopic";

        protected override void Consume(ConsumeResult<string, string> result)
        {
            var paymentEvent = JsonConvert.DeserializeObject<PaymentEvent>(result.Message.Value)!;
            Logger.LogInformation("Принято событие оплаты: {0}", paymentEvent.payment_id);
        }
    }
}
