using System.Data.Entity.ModelConfiguration;
using Airline.AppData.Model;

namespace Airline.AppData.EF.Configuration
{
    internal class AppUserConfiguration : EntityTypeConfiguration<AppUser>
    {
        public AppUserConfiguration()
        {
            Property(x => x.UserName).IsOptional();
            Property(x => x.FirstName).IsRequired();
            Property(x => x.SecondName).IsRequired();          
            Property(x => x.BornDate).IsRequired();
            Property(x => x.PhoneNumber).IsRequired();
            Property(x => x.Email).IsRequired();
            Property(x => x.PasswordHash).IsRequired();
            Property(x => x.SecurityStamp).IsRequired();

            HasMany(x => x.RequestBrokers)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(x =>
                {
                    x.ToTable("UserRoles");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("RoleId");
                });
        }
    }
}
