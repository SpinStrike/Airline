using Ninject.Modules;
using Airline.AppData.Repository;
using Airline.AppData.Repository.Implementation;
using Airline.AppLogic.Service;
using Airline.AppLogic.Service.Implementation;

namespace Airline.Dependency
{
    /// <summary>
    /// Configure classes dependency in  Ninject DI container
    /// </summary>
    public class AirlineDependencies : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbRepository>().To<DbRepository>().InSingletonScope();
            Bind<ICityService>().To<CityService>();
            Bind<ICityRepository>().To<CityRepository>();
            Bind<IProfessionService>().To<ProfessionService>();
            Bind<IProfessionRepository>().To<ProfessionRepository>();
            Bind<IAircrewMemberRepository>().To<AircrewMemberReository>();
            Bind<IAircrewMemberService>().To<AircrewMemberService>();
            Bind<IFlightRepository>().To<FlightRepository>();
            Bind<IFlightService>().To<FlightService>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IFlightRequestRepository>().To<FlightRequestRepository>();
            Bind<IFlightRequestService>().To<FlightRequestService>();
            Bind<IUserSearchService>().To<UserSearchService>();
        }
    }
}
