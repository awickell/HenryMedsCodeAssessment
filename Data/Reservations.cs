using System.ComponentModel.DataAnnotations;

namespace HenryMedsCodeAssessment.Data
{
    public class Reservations
    {
		[Key]
		public Guid id { get; set; }
		public Guid client_id { get; set; }
		public Guid provider_id { get; set; }
		public DateTime start_time { get; set; }
		public DateTime reservation_placed { get; set; }
		public bool confirmed { get; set; }
		public bool expired { get; set; }
    }
}