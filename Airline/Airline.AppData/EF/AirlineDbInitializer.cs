using System.Data.Entity;
using Airline.AppData.Model;

namespace Airline.AppData.EF
{
    class AirlineDbInitializer : DropCreateDatabaseIfModelChanges<AirlineDbContext>
    {
        protected override void Seed(AirlineDbContext context)
        {
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

            



            base.Seed(context);
        }

    }
}
