using Confluent.Kafka;
using EventsService.Models;
using Newtonsoft.Json;

namespace EventsService.Consumers
{
    public class UserConsumer(IConfiguration config) : ConsumerBase<string, string>(config)
    {
        protected override string TopicSettingKey => "UserTopic";

        protected override void Consume(ConsumeResult<string, string> result)
        {
            var userEvent = JsonConvert.DeserializeObject<UserEvent>(result.Message.Value)!;
            Console.WriteLine("Принято событие пользователя: {0}", userEvent.user_id);
        }
    }
}
