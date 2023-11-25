using HenryMedsCodeAssessment.Data;
using HenryMedsCodeAssessment.Tasks;

namespace HenryMedsCodeAssessment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // TODO: Source database connection string from configuration
            var connectionString = "Data Source=test.sqlite";

            AddServices(builder.Services, connectionString, builder.Environment.IsDevelopment());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }

        public static void AddServices(IServiceCollection services, string connectionString, bool isDev = true)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            if (isDev)
            {
                // Use SQLite for test/dev purposes, no necessary installation
                services.AddSqlite<ReservationDb>(connectionString);
            }

            else
            {
                // TODO: Connect to existing production DB, e.g. PostgreSQL
                throw new NotImplementedException("No production DB connection implemented");
            }

            services.AddHostedService<ClearExpiredReservations>();
        }
    }
}