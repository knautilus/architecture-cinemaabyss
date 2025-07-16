using Confluent.Kafka;
using EventsService.Models;
using Newtonsoft.Json;

namespace EventsService.Consumers
{
    public class MovieConsumer(IConfiguration config) : ConsumerBase<string, string>(config)
    {
        protected override string TopicSettingKey => "MovieTopic";

        protected override void Consume(ConsumeResult<string, string> result)
        {
            var movieEvent = JsonConvert.DeserializeObject<MovieEvent>(result.Message.Value)!;
            Console.WriteLine("Принято событие фильма: {0}", movieEvent.movie_id);
        }
    }
}
