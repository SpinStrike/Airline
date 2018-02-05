using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    public class AppRoleConfiguraton : EntityTypeConfiguration<AppRole>
    {
        public AppRoleConfiguraton()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired();
        }
    }
}
