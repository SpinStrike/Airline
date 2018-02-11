using Ninject.Modules;
using Airline.AppData.Repository;
using Airline.AppData.Repository.Implementation;
using Airline.AppLogic.Service;
using Airline.AppLogic.Service.Implementation;
using Airline.AppLogic.Logging;

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
            Bind<ICityRepository>().To<CityRepository>();
            Bind<IProfessionRepository>().To<ProfessionRepository>();
            Bind<IAircrewMemberRepository>().To<AircrewMemberReository>();
            Bind<IFlightRepository>().To<FlightRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IFlightRequestRepository>().To<FlightRequestRepository>();
            
            Bind<ICityService>().To<CityService>()
                .WithConstructorArgument("logger", LoggerFactory.GetServiceLogger(typeof(CityService).FullName));

            Bind<IProfessionService>().To<ProfessionService>()
                .WithConstructorArgument("logger", LoggerFactory.GetServiceLogger(typeof(ProfessionService).FullName));

            Bind<IAircrewMemberService>().To<AircrewMemberService>()
                .WithConstructorArgument("logger", LoggerFactory.GetServiceLogger(typeof(AircrewMemberService).FullName));

            Bind<IFlightService>().To<FlightService>()
                .WithConstructorArgument("logger", LoggerFactory.GetServiceLogger(typeof(FlightService).FullName));

            Bind<IFlightRequestService>().To<FlightRequestService>()
                .WithConstructorArgument("logger", LoggerFactory.GetServiceLogger(typeof(FlightRequestService).FullName));

            Bind<IUserSearchService>().To<UserSearchService>()
                .WithConstructorArgument("logger", LoggerFactory.GetServiceLogger(typeof(UserSearchService).FullName));
        }
    }
}
