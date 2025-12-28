using Confluent.Kafka;
using EventsService.Models;
using Newtonsoft.Json;

namespace EventsService.Consumers
{
    public class UserConsumer(IConfiguration config, ILogger<UserConsumer> logger) : ConsumerBase<string, string>(config, logger)
    {
        protected override string TopicSettingKey => "UserTopic";

        protected override void Consume(ConsumeResult<string, string> result)
        {
            var userEvent = JsonConvert.DeserializeObject<UserEvent>(result.Message.Value)!;
            Logger.LogInformation("Принято событие пользователя: {0}", userEvent.user_id);
        }
    }
}
