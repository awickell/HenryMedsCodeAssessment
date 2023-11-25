using HenryMedsCodeAssessment.Data;
using HenryMedsCodeAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HenryMedsCodeAssessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProviderScheduleController : ControllerBase
    {
        private ReservationDb ReservationDbContext;

        public ProviderScheduleController(ReservationDb reservationDb)
        {
            ReservationDbContext = reservationDb;
        }

        // Get provider availability
        [HttpGet]
        public ProviderScheduleGetResponse Get(Guid request)
        {
            var validation = ValidateGetRequest(request);

            if (string.Equals(validation, "valid"))
            {
                var data = ReservationDbContext.ProviderSchedules
                .Where(row => row.provider_id == request)
                .Where(row => !row.reserved)
                .Select(row => new ProviderAvailabilitySlot
                {
                    start_time = row.block_start
                }).ToArray();

                return new ProviderScheduleGetResponse
                {
                    success = true,
                    available_slots = data
                };
            }

            else
            {
                return new ProviderScheduleGetResponse
                {
                    success = false,
                    available_slots = null,
                    failure_message = validation
                };
            }
        }

        // Provider sets availability
        [HttpPost]
        public ProviderScheduleUpdateResponse Update(ProviderScheduleUpdateRequest request)
        {
            var validation = ValidateUpdateRequest(request);

            if (string.Equals(validation, "valid"))
            {
                var toInsert = PartitionTimeBlock(request);

                foreach (var row in toInsert)
                {
                    ReservationDbContext.Add(row);
                }

                // only save after all rows have been queued
                ReservationDbContext.SaveChanges();

                return new ProviderScheduleUpdateResponse
                {
                    success = true
                };
            }

            else
            {
                return new ProviderScheduleUpdateResponse
                {
                    success = false,
                    failure_message = validation
                };
            }
        }

        private string ValidateGetRequest(Guid request)
        {
            var provider_ids = ReservationDbContext.Providers.Select(row => row.id);

            if (provider_ids.Contains(request))
            {
                return "valid";
            }

            else
            {
                return "Provider ID doesn't exist";
            }
        }

        private string ValidateUpdateRequest(ProviderScheduleUpdateRequest request)
        {
            // assumption: only times that start and end on 15 mimute multiples are valid

            // check start is before end and is at least 15 minutes
            if (request.start_time.AddMinutes(15) > request.end_time)
            {
                return "Slot is too short, or end time occurs before start time";
            }

            // check if start and end are 15 minute multiples
            if (!Is15MinuteMultiple(request.start_time) || !Is15MinuteMultiple(request.end_time))
            {
                return "Start or end times are not 15 minute multiples";
            }

            // check if there's any overlap of reservations
            var providersReservations = ReservationDbContext.ProviderSchedules
                .Where(row => row.provider_id == request.provider_id)
                .Where(row => row.reserved)
                .Select(row => row.block_start);

            foreach (var reservation in providersReservations)
            {
                if (reservation >= request.start_time &
                   reservation < request.end_time)
                {
                    return "There is already a reservation in the selected time block";
                }
            }


            return "valid";
        }

        private bool Is15MinuteMultiple(DateTime timestamp)
        {
            return timestamp.Minute % 15 == 0;
        }

        private ProviderSchedules[] PartitionTimeBlock(ProviderScheduleUpdateRequest request)
        {
            var totalMinutes = (request.end_time - request.start_time).Minutes;

            var total15Slots = totalMinutes / 15;
            var modeledSlots = new ProviderSchedules[total15Slots];

            for (int idx = 0; idx < total15Slots; idx++)
            {
                modeledSlots[idx] = new ProviderSchedules
                {
                    provider_id = request.provider_id,
                    block_start = request.start_time.AddMinutes(15 * idx),
                    reserved = false,
                    reservation_id = null
                };
            }

            return modeledSlots;
        }
    }
}