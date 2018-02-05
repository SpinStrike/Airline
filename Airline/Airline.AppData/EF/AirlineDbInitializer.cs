using System.Data.Entity;
using Airline.AppData.Model;

namespace Airline.AppData.EF
{
    class AirlineDbInitializer : DropCreateDatabaseIfModelChanges<AirlineDbContext>
    {
        protected override void Seed(AirlineDbContext context)
        {
            var adminRole = new AppRole()
            {
                Name = "Administrator"
            };

            context.Roles.Add(adminRole);

            context.Roles.Add(new AppRole()
            {
                Name = "AirTrafficController"
            });

            context.Roles.Add(new AppRole()
            {
                Name = "AircrewMember"
            });

            context.Cities.Add(new City()
            {
                Name = "Kharkov"
            });

            context.Cities.Add(new City()
            {
                Name = "Kiev"
            });

            context.Cities.Add(new City()
            {
                Name = "Lviv"
            });

            context.Professions.Add(new Profession()
            {
                Name = "Pilot"
            });

            context.Professions.Add(new Profession()
            {
                Name = "Aircraft Navigator"
            });

            context.Professions.Add(new Profession()
            {
                Name = "Radio Operator"
            });

            context.Professions.Add(new Profession()
            {
                Name = "Flight Engineer"
            });

            context.Professions.Add(new Profession()
            {
                Name = "Stewardess"
            });


            var administrator = new AppUser()
            {
                Id = new System.Guid("A73BB58D-EA73-4F78-AB17-92D5E4A3D932"),
                UserName = "administrator@gmail.com",
                FirstName = "Anton",
                SecondName = "Bezverkhyi",
                BornDate = new System.DateTime(1995, 7, 22),
                PhoneNumber = "+380994564907",
                Email = "administrator@gmail.com",
                PasswordHash = "AO3seCe1iN1PidBxVtiPk+6jKf8LXLmHCZAEsi6IzOwCWBHUo9OB2jR04jGRliIf3w==",
                SecurityStamp = "144f533f-6c0a-4736-9047-bb528264ad99"
            };

            administrator.Roles.Add(adminRole);
            context.Users.Add(administrator);

            base.Seed(context);
        }
    }
}
