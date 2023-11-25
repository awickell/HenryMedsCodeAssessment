namespace HenryMedsCodeAssessment.Models
{
    public class ProviderScheduleGetRequest
    {
        public Guid provider_id { get; set; }
    }

    public class ProviderAvailabilitySlot
    {
        public DateTime start_time { get; set; }
    }

    public class ProviderScheduleGetResponse
    {
        public bool success { get; set; }
        public ProviderAvailabilitySlot[]? available_slots { get; set; }
        public string failure_message { get; set; }
    }

    public class ProviderScheduleUpdateRequest
    {
        public Guid provider_id { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }

    public class ProviderScheduleUpdateResponse
    {
        public bool success { get; set; }
        public string failure_message { get; set; }
    }
}