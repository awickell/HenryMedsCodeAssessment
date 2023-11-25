namespace HenryMedsCodeAssessment.Models
{
    public class ReserveRequest
    {
        public Guid provider_id { get; set; }
        public Guid client_id { get; set; }
        public DateTime start_time { get; set; }
    }

    public class ReserveResponse
    {
        public bool success { get; set; }
        public Guid? reservation_ID { get; set; }
        public string failure_message { get; set; }
    }

    public class ConfirmRequest
    {
        public Guid reservation_ID { get; set; }
    }

    public class ConfirmResponse
    {
        public bool success { get; set; }
        public string failure_message { get; set; }
    }
}