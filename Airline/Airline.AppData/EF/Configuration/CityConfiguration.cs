using Airline.AppData.Model;
using System.Data.Entity.ModelConfiguration;

namespace Airline.AppData.EF.Configuration
{
    internal class CityConfiguration : EntityTypeConfiguration<City>
    {
        public CityConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();

            HasMany(x => x.Points)
                .WithRequired(x => x.City)
                .HasForeignKey(x => x.CityId)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Users)
                .WithRequired(x => x.CurrentLocation)
                .HasForeignKey(x => x.CityId)
                .WillCascadeOnDelete(false);
        }
    }
}
