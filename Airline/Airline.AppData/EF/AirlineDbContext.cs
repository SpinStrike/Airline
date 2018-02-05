using System.Data.Entity;
using Airline.AppData.EF.Configuration;
using Airline.AppData.Model;

namespace Airline.AppData.EF
{
    public class AirlineDbContext : DbContext
    {
        public IDbSet<City> Cities { get; set; }

        public IDbSet<Flight> Flights { get; set; }

        public IDbSet<Profession> Professions { get; set; }

        public IDbSet<FlightRequest> FlightRequests { get; set; }

        public IDbSet<SendReceiveBroker> Brokers {get;set;}

        public IDbSet<AppUser> Users { get; set; }

        public IDbSet<AppRole> Roles { get; set; }

        public AirlineDbContext(string connectionName)
            :base(connectionName)
        { }

        static AirlineDbContext()
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<AirlineDbContext>()
            );

            Database.SetInitializer<AirlineDbContext>(new AirlineDbInitializer());
        }
                                                         
        public static AirlineDbContext Create()
        {
            return new AirlineDbContext("AirlineDbConnection");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
            modelBuilder.Configurations.Add(new CityConfiguration());
            modelBuilder.Configurations.Add(new FlightConfiguration());
            modelBuilder.Configurations.Add(new FlightPointConfiguration());
            modelBuilder.Configurations.Add(new AppUserConfiguration());
            modelBuilder.Configurations.Add(new AppRoleConfiguraton());
            modelBuilder.Configurations.Add(new AircrewMemberConfiguration());
            modelBuilder.Configurations.Add(new ProfessionConfiguration());
            modelBuilder.Configurations.Add(new FlightRequestConfiguration());
            modelBuilder.Configurations.Add(new SendReceiveBrokerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
