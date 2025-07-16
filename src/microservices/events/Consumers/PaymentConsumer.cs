using Confluent.Kafka;
using EventsService.Models;
using Newtonsoft.Json;

namespace EventsService.Consumers
{
    public class PaymentConsumer(IConfiguration config) : ConsumerBase<string, string>(config)
    {
        protected override string TopicSettingKey => "PaymentTopic";

        protected override void Consume(ConsumeResult<string, string> result)
        {
            var paymentEvent = JsonConvert.DeserializeObject<PaymentEvent>(result.Message.Value)!;
            Console.WriteLine("Принято событие оплаты: {0}", paymentEvent.payment_id);
        }
    }
}
