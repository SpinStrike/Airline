using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class FlightConfiguration : EntityTypeConfiguration<Flight>
    {
        public FlightConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.DepartureDate).IsRequired();
            Property(x => x.ArrivalDate).IsRequired();
            Property(x => x.Status).IsRequired();

            HasMany(x => x.Points)
                .WithRequired(x => x.Flight)
                .HasForeignKey(x => x.FlightId)
                .WillCascadeOnDelete(true);

            HasMany(x => x.Aircrew)
                .WithRequired(x => x.Flight)
                .HasForeignKey(x => x.FlightId)
                .WillCascadeOnDelete(false);

            HasMany(x => x.ConfirmationRequests)
                .WithRequired(x => x.Flight)
                .HasForeignKey(x => x.FlightId)
                .WillCascadeOnDelete(false);

            Ignore(x => x.From);
            Ignore(x => x.To);
            Ignore(x => x.Name);
        }
    }
}
