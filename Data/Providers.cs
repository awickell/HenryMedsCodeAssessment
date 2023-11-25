using System.ComponentModel.DataAnnotations;

namespace HenryMedsCodeAssessment.Data
{
    public class Providers
    {
        [Key]
        public Guid id { get; set; }
        public string display_name { get; set; }
    }
}