using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class AppUserConfiguration : EntityTypeConfiguration<AppUser>
    {
        public AppUserConfiguration()
        {
            Property(x => x.FirstName).IsRequired();
            Property(x => x.SecondName).IsRequired();

            HasMany(x => x.RequestBrokers)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
