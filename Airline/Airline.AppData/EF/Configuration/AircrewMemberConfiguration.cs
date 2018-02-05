using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class AircrewMemberConfiguration : EntityTypeConfiguration<AircrewMember>
    {
        public AircrewMemberConfiguration()
        {
            Property(x => x.Status).IsRequired();
        }
    }
}
