using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HenryMedsCodeAssessment.Data
{
    public class ProviderSchedules
    {
        [Key]
        public Guid id { get; set; }
        public Guid provider_id { get; set; }
        public DateTime block_start { get; set; }
        public bool reserved { get; set; }
        public Guid? reservation_id { get; set; }
        public Guid? reserved_by { get; set; }
    }
}