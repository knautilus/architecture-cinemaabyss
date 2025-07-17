using Confluent.Kafka;
using EventsService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsService.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController(ILogger<EventsController> logger,
        KafkaDependentProducer<string, string> movieProducer,
        KafkaDependentProducer<string, string> userProducer,
        KafkaDependentProducer<string, string> paymentProducer,
        IConfiguration config) : Controller
    {
        private readonly string _movieTopic = config.GetValue<string>("MovieTopic")!;
        private readonly string _userTopic = config.GetValue<string>("UserTopic")!;
        private readonly string _paymentTopic = config.GetValue<string>("PaymentTopic")!;

        /// <summary>
        /// Send MovieEvent
        /// </summary>
        /// <response code="201">MovieEvent sent</response>
        [HttpPost("movie")]
        public async Task<IActionResult> CreateMovieEvent([FromBody] MovieEvent movieEvent, CancellationToken cancellationToken)
        {
            await movieProducer.ProduceAsync(_movieTopic,
                new Message<string, string>
                {
                    Key = movieEvent.movie_id.ToString(),
                    Value = JsonConvert.SerializeObject(movieEvent)
                });
            logger.LogInformation("Отправлено событие фильма: {0}", movieEvent.movie_id);
            return GetSuccessResponse();
        }

        /// <summary>
        /// Send UserEvent
        /// </summary>
        /// <response code="201">UserEvent sent</response>
        [HttpPost("user")]
        public async Task<IActionResult> CreateUserEvent([FromBody] UserEvent userEvent, CancellationToken cancellationToken)
        {
            await userProducer.ProduceAsync(_userTopic,
            new Message<string, string>
            {
                Key = userEvent.user_id.ToString(),
                Value = JsonConvert.SerializeObject(userEvent)
            });
            logger.LogInformation("Отправлено событие пользователя: {0}", userEvent.user_id);
            return GetSuccessResponse();
        }

        /// <summary>
        /// Send PaymentEvent
        /// </summary>
        /// <response code="201">PaymentEvent sent</response>
        [HttpPost("payment")]
        public async Task<IActionResult> CreatePaymentEvent([FromBody] PaymentEvent paymentEvent, CancellationToken cancellationToken)
        {
            await paymentProducer.ProduceAsync(_paymentTopic,
            new Message<string, string>
            {
                Key = paymentEvent.payment_id.ToString(),
                Value = JsonConvert.SerializeObject(paymentEvent)
            });
            logger.LogInformation("Отправлено событие оплаты: {0}", paymentEvent.payment_id);
            return GetSuccessResponse();
        }

        private static IActionResult GetSuccessResponse()
        {
            return new ObjectResult(new Response { status = Status.Success }) { StatusCode = StatusCodes.Status201Created };
        }

        private static IActionResult GetErrorResponse(string error)
        {
            return new ObjectResult(new Response { status = Status.Error, error = error }) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
}
