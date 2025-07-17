using Confluent.Kafka;
using EventsService.Models;
using Newtonsoft.Json;

namespace EventsService.Consumers
{
    public class MovieConsumer(IConfiguration config, ILogger<MovieConsumer> logger) : ConsumerBase<string, string>(config, logger)
    {
        protected override string TopicSettingKey => "MovieTopic";

        protected override void Consume(ConsumeResult<string, string> result)
        {
            var movieEvent = JsonConvert.DeserializeObject<MovieEvent>(result.Message.Value)!;
            Logger.LogInformation("Принято событие фильма: {0}", movieEvent.movie_id);
        }
    }
}
