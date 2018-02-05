using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class ProfessionConfiguration : EntityTypeConfiguration<Profession>
    {
        public ProfessionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name);

            HasMany(x => x.AircrewMember)
                .WithOptional(x => x.Profession)
                .HasForeignKey(x => x.ProfessionId)
                .WillCascadeOnDelete(false);
        }
    }
}
