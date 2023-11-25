using HenryMedsCodeAssessment.Data;
using HenryMedsCodeAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HenryMedsCodeAssessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProvidersController : ControllerBase
    {
        private ReservationDb ReservationDbContext;
        
        public ProvidersController(ReservationDb reservationDb)
        {
            ReservationDbContext = reservationDb;
        }

        // Get list of providers
        [HttpGet]
        public ProvidersResponse Get()
        {
            var data =  ReservationDbContext.Providers
                .Select(row => new Provider
                {
                    id = row.id,
                    display_name = row.display_name
                }).ToArray();

            return new ProvidersResponse
            {
                providers = data
            };
        }
    }
}