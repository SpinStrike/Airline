using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class ConfirmationRequestConfiguration : EntityTypeConfiguration<ConfirmationRequest>
    {
        public ConfirmationRequestConfiguration()
        {
            HasKey(x => x.Id);
        }
    }
}
