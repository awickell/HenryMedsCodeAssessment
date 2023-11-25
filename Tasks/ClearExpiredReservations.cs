using HenryMedsCodeAssessment.Data;

namespace HenryMedsCodeAssessment.Tasks
{
    public class ClearExpiredReservations : BackgroundService
    {
        private IServiceScopeFactory ScopeFactory;

        public ClearExpiredReservations(IServiceScopeFactory scopeFactory)
        {
            ScopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                while (!token.IsCancellationRequested)
                {
                    var reservationDbContext = scope.ServiceProvider.GetRequiredService<ReservationDb>();

                    try
                    {
                        await Task.Delay(10000, token);

                        var expiredReservations = reservationDbContext.Reservations
                            .Where(row => !row.confirmed)
                            .Where(row => !row.expired)
                            .Where(row => row.reservation_placed > DateTime.Now.AddMinutes(30));

                        if (expiredReservations != null)
                        {
                            foreach (var expired in expiredReservations)
                            {
                                var expiredSlot = reservationDbContext.ProviderSchedules
                                .Single(row => row.reservation_id == expired.id);

                                expiredSlot.reservation_id = null;
                                expiredSlot.reserved = false;
                                expiredSlot.reserved_by = null;

                                reservationDbContext.ProviderSchedules.Update(expiredSlot);

                                expired.expired = true;

                                reservationDbContext.Reservations.Update(expired);
                            }
                        }

                        reservationDbContext.SaveChanges();
                    }

                    catch (OperationCanceledException ex) { }
                }
            }
        }
    }
}