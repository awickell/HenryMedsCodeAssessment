using HenryMedsCodeAssessment.Data;
using HenryMedsCodeAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HenryMedsCodeAssessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationsController : ControllerBase
    {
        private ReservationDb ReservationDbContext;

        public ReservationsController(ReservationDb reservationDb)
        {
            ReservationDbContext = reservationDb;
        }

        // Make reservation
        [HttpPost]
        public ReserveResponse Post(ReserveRequest request)
        {
            var validation = ValidatePostRequest(request);

            if (string.Equals(ValidatePostRequest(request), "valid"))
            {
                var reservationId = Guid.NewGuid();

                ReservationDbContext.Reservations.Add(new Reservations
                {
                    id = reservationId,
                    provider_id = request.provider_id,
                    reservation_placed = DateTime.Now,
                    start_time = request.start_time,
                    confirmed = false,
                    expired = false
                });

                // assuming single slot existence
                var providerSlot = ReservationDbContext.ProviderSchedules
                    .Where(row => row.provider_id == request.provider_id)
                    .Single(row => row.block_start == request.start_time);

                providerSlot.reservation_id = reservationId;
                providerSlot.reserved = true;
                providerSlot.reserved_by = request.client_id;

                ReservationDbContext.ProviderSchedules.Update(providerSlot);

                ReservationDbContext.SaveChanges();

                return new ReserveResponse
                {
                    success = true,
                    reservation_ID = reservationId
                };
            }

            else
            {
                return new ReserveResponse
                {
                    success = false,
                    reservation_ID = null,
                    failure_message = validation
                };
            }
        }

        // Confirm reservation
        [HttpPut]
        public ConfirmResponse Put(ConfirmRequest request)
        {
            var validation = ValidatePutRequest(request);

            if (string.Equals(ValidatePutRequest(request), "valid"))
            {
                var reservation = ReservationDbContext.Reservations
                     .Single(row => row.id == request.reservation_ID);

                reservation.confirmed = true;

                ReservationDbContext.Reservations.Update(reservation);
                ReservationDbContext.SaveChanges();

                return new ConfirmResponse
                {
                    success = true
                };
            }

            else
            {
                return new ConfirmResponse
                {
                    success = false,
                    failure_message = validation
                };
            }
        }

        private string ValidatePostRequest(ReserveRequest request)
        {
            // assumption: frontend presents 15 minute duration slots

            // not a 15 minute multiple
            if (request.start_time.Minute % 15 != 0)
            {
                return "Reservation does not occur on the 15 mintue multiple";
            }

            // not 24 hours in advance
            if (request.start_time < DateTime.Now.AddHours(24))
            {
                return "Reservation is not at least 24 hours in advance";
            }

            // not available
            var availabilities = ReservationDbContext.ProviderSchedules
                .Where(row => row.provider_id == request.provider_id)
                .Where(row => !row.reserved)
                .Select(row => row.block_start);

            if (availabilities == null)
            {
                return "There are no availabilies for this provider";
            }

            else if (!availabilities.Contains(request.start_time))
            {
                return "Requested reservation is not available";
            }

            return "valid";
        }

        private string ValidatePutRequest(ConfirmRequest request)
        {
            var reservations = ReservationDbContext.Reservations
                .Where(row => row.id == request.reservation_ID);

            if (reservations == null)
            {
                return "This reservation doesn't exist";
            }

            // assuming just one reservation db
            var reservation = reservations.Single();

            if (reservation.confirmed)
            {
                return "Already confirmed";
            }

            else if (reservation.expired)
            {
                return "Reservation expired";
            }

            return "valid";
        }
    }
}