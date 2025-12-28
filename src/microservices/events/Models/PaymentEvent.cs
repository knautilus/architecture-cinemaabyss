namespace EventsService.Models
{
    public class PaymentEvent
    {
        public int payment_id { get; set; }
        public int user_id { get; set; }
        public float amount { get; set; }
        public string status { get; set; }
        public string timestamp { get; set; }
        public string method_type { get; set; }
    }
}
