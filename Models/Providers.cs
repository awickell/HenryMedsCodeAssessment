namespace HenryMedsCodeAssessment.Models
{
    public class ProvidersResponse
    {
        public Provider[] providers { get; set; }
    }

    public class Provider
    {
        public Guid id { get; set; }
        public string display_name { get; set; }
    }
}