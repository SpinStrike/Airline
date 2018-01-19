using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class FlightPointConfiguration : EntityTypeConfiguration<FlightPoint>
    {
        public FlightPointConfiguration()
        {
            HasKey(x => new { x.FlightId, x.CityId });
            Property(x => x.Direction).IsRequired();
        }
    }
}
