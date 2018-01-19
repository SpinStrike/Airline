using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class FlightRequestConfiguration : EntityTypeConfiguration<FlightRequest>
    {
        public FlightRequestConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Message);
            Property(x => x.Status);

            HasMany(x => x.RequestBrokers)
                .WithRequired(x => x.FlightRequest)
                .HasForeignKey(x => x.FlightRequestId)
                .WillCascadeOnDelete(true);

            Ignore(x => x.From);
            Ignore(x => x.To);
        }
    }
}
