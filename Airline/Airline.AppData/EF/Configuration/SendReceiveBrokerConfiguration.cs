using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class SendReceiveBrokerConfiguration :  EntityTypeConfiguration<SendReceiveBroker>
    {
        public SendReceiveBrokerConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Direction).IsRequired();
        }
    }
}
