using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Model;
using Airline.AppData.Repository;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service.Implementation
{
    /// <summary>
    /// Represent features and tools to work with flights data
    /// </summary>
    public class FlightService : IFlightService
    {
        /// <summary>
        /// Flight service constructor
        /// </summary>
        public FlightService(IFlightRepository flightRepository,
            IAircrewMemberRepository aircrewMemberRepository,
            ICityRepository cityRepository)
        {
            _flightRepository = flightRepository;
            _aircrewMemberRepository = aircrewMemberRepository;
            _cityRepository = cityRepository;
        }

        /// <summary>
        /// Create new flight
        /// </summary>
        public ServiceAnswer Crete(FlightDto flight)
        {
            var result = new ServiceAnswer() { Status = AnswerStatus.Success };

            try
            {
                if (!CheckNumber(result, flight.Number))
                {
                    return result;
                };

                if (!CheckDate(result, flight.DepartureDate, flight.ArrivalDate))
                {
                    return result;
                };

                if (!CheckExistencePilot(result, flight.AircrewMembers.Select(x => x.Id)))
                {
                    return result;
                }

                var cities = GetCities(result, flight.From.Id, flight.To.Id);
                if (cities.Count() == 0)
                {
                    return result;
                };

                _flightRepository.StartTransaction();

                var newFligth = _flightRepository.Crete(flight.Number,
                      cities.First(x => x.Id == flight.From.Id),
                      cities.First(x => x.Id == flight.To.Id),
                      flight.DepartureDate,
                      flight.ArrivalDate,
                      flight.Status);


                if (!SetAircrew(result, newFligth, flight.AircrewMembers.Select(x => x.Id)))
                {
                    _flightRepository.RollBack();
                    return result;
                }

                _flightRepository.Commit();

                result.Status = AnswerStatus.Success;

                return result;
            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        /// <summary>
        /// Delete selected flight 
        /// </summary>
        public ServiceAnswer Delete(Guid id)
        {
            var result = new ServiceAnswer();

            try
            {
                var targetFlight = _flightRepository.FindById(id); ;

                if (targetFlight != null)
                {
                    _flightRepository.StartTransaction();

                    ManageAircrewByStatus(targetFlight, true);
                    SaveOrDisbandAircrew(targetFlight, true);

                    _flightRepository.Delete(targetFlight);

                    _flightRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found.");
            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        /// <summary>
        /// Find flight by id
        /// </summary>
        public ServiceResult<FlightDto> FindById(Guid id)
        {
            var result = new ServiceResult<FlightDto>();

            try
            {
                var targetFlight = _flightRepository.FindById(id); ;

                result.Result = targetFlight.ToDto();
                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        /// <summary>
        /// Get list of all flights
        /// </summary>
        public ServiceResult<IEnumerable<FlightDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<FlightDto>>();

            try
            {
                var targetFlights = _flightRepository.GetAll().ToList(); ;

                result.Result = targetFlights.Select(x => x.ToDto(false));
                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Result = new List<FlightDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        /// <summary>
        /// Get list of flights by selected parameters (departure city and date. arrival city and date)
        /// </summary>
        public ServiceResult<IEnumerable<FlightDto>> GetFilteredList(Guid? fromCityId,
            Guid? toCityId,
            DateTime? departDate,
            DateTime? arriveDate)
        {
            var result = new ServiceResult<IEnumerable<FlightDto>>();

            try
            {
                var targetFlights = _flightRepository.GetAll();

                if (fromCityId != null)
                {
                    targetFlights =  _flightRepository.FindByFromCity(fromCityId.Value, targetFlights); //targetFlights.Where(x => x.Points.Where(y => y.CityId == fromCityId.Value && y.Direction == Direction.From).Count() == 1);//
                }

                if (toCityId != null)
                {
                    targetFlights = _flightRepository.FindByToCity(toCityId.Value, targetFlights); //targetFlights.Where(x => x.Points.Where(y => y.CityId == toCityId.Value && y.Direction == Direction.To).Count() == 1);//
                }

                if (departDate != null)
                {
                    targetFlights = _flightRepository.FindByDepartureDate(departDate.Value, targetFlights); //targetFlights.Where(x => x.DepartureDate == departDate.Value); //
                }

                if (arriveDate != null)
                {
                    targetFlights = _flightRepository.FindByArrivalDate(arriveDate.Value, targetFlights); //targetFlights.Where(x => x.ArrivalDate == arriveDate.Value);//
                }

                result.Result = targetFlights.ToList()
                    .OrderByDescending(x => x.DepartureDate)
                    .Select(x => x.ToDto(false));

                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Result = new List<FlightDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        /// <summary>
        /// Finde flight by number
        /// </summary>
        public ServiceResult<IEnumerable<FlightDto>> FindByNumber(string flightNumber)
        {
            var result = new ServiceResult<IEnumerable<FlightDto>>();
            result.Result = new List<FlightDto>();

            try
            {
                if (flightNumber == null || flightNumber == string.Empty)
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Number error", "Value of flight number is empty ot null");
                }

                var targetFlight = _flightRepository.FindByNumber(flightNumber);
                if (targetFlight != null)
                {
                    (result.Result as IList<FlightDto>).Add(targetFlight.ToDto(false));
                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found");
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        /// <summary>
        /// Set new status to selected flight
        /// </summary>
        public ServiceAnswer SetStatus(Guid idFlight, string status)
        {
            var result = new ServiceAnswer();

            if (!GetAvailableStatuses().Result.Contains(status) || status == null)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Argument error", "Invalid value of status.");

                return result;
            }

            try
            {
                var targetFlight = _flightRepository.FindById(idFlight); ;

                if (targetFlight != null)
                {
                    _flightRepository.StartTransaction();

                    targetFlight.Status = GetEnumStatus(status, targetFlight.Status);

                    ManageAircrewByStatus(targetFlight);
                    SaveOrDisbandAircrew(targetFlight);

                    _flightRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found.");

            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }
        
        /// <summary>
        /// Get set of available statuses to selectet flight 
        /// </summary>
        public ServiceResult<IEnumerable<string>> GetAvailableStatuses(FlightDto flight = null)
        {
            var result = new ServiceResult<IEnumerable<string>>()
            {
                Result = new List<string>() { "Cancelled" },
                Status = AnswerStatus.Success
            };

            if (flight == null)
            {
                (result.Result as IList<string>).Add("Preparing");
                (result.Result as IList<string>).Add("In Air");
                (result.Result as IList<string>).Add("Landed");

                return result;
            }
            else if (flight.DepartureDate > flight.ArrivalDate)
            {
                (result.Result as IList<string>).Clear();
                result.Errors.Add("Date error", "Departure date is later than arrival.");
                result.Status = AnswerStatus.Failure;

            }
            else if (flight.Status == FlightStatus.Preparing)
            {
                if (DateTime.Now.Date >= flight.DepartureDate.Date)
                {
                    (result.Result as IList<string>).Add("In Air");

                    if (DateTime.Now.Date >= flight.ArrivalDate.Date)
                    {
                        (result.Result as IList<string>).Add("Landed");
                    }
                }
            }
            else if (flight.Status == FlightStatus.InAir)
            {
                if (DateTime.Now.Date >= flight.ArrivalDate.Date)
                {
                    (result.Result as IList<string>).Add("Landed");

                }
            }
            else if (flight.Status == FlightStatus.Landed)
            {
                if (DateTime.Now.Date >= flight.ArrivalDate.Date)
                {
                    (result.Result as IList<string>).Remove("Cancelled");
                }
            }
            else
            {
                (result.Result as IList<string>).Clear();
                result.Errors.Add("Flight status error", "Invalid value of flight.");
                result.Status = AnswerStatus.Failure;
            }

            return result;
        }

        /// <summary>
        /// Convert string to flight status
        /// </summary>
        public FlightStatus Status(string status)
        {
            return GetEnumStatus(status, FlightStatus.Preparing);
        }

        /// <summary>
        /// Update existing flight
        /// </summary>
         public ServiceAnswer UpdateFlight(FlightDto updatedFlight)
        {
            var result = new ServiceAnswer() { Status = AnswerStatus.Success };

            try
            {
                var originalFlight = _flightRepository.FindById(updatedFlight.Id);
                if (originalFlight != null)
                {
                    if (originalFlight.Number == updatedFlight.Number)
                    { }
                    else if (!CheckNumber(result, updatedFlight.Number))
                    {
                        return result;
                    };
              
                    if (!CheckDate(result, updatedFlight.DepartureDate, updatedFlight.ArrivalDate, false))
                    {
                        return result;
                    };
          
                    if (!CheckExistencePilot(result, updatedFlight.AircrewMembers.Select(x => x.Id)))
                    {
                        return result;
                    }

                    var cities = GetCities(result, updatedFlight.From.Id, updatedFlight.To.Id);
                    if (cities.Count() == 0)
                    {
                        return result;
                    };

                    _flightRepository.StartTransaction();

                    SetFlightData(originalFlight, updatedFlight, cities);

                    if (!SetAircrew(result, originalFlight, updatedFlight.AircrewMembers.Select(x => x.Id)))
                    {
                        _flightRepository.RollBack();
                        return result;
                    }

                    SaveOrDisbandAircrew(originalFlight);

                    _flightRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found.");
            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        /// <summary>
        ///  Check flight number. It must be unique and isn't empty
        /// </summary>
        private bool CheckNumber(ServiceAnswer result, string Number)
        {
            if(result.Status == AnswerStatus.Failure)
            {
                return false;
            }

            if (Number == string.Empty || Number == null)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Number error", "Flight number empty or null.");

                return false;
            }

            var uniqeNumber = _flightRepository.FindByNumber(Number) == null ? true : false;
            if (!uniqeNumber)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Number error", "Flight number is not uniqe.");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check correct of departure and arrival date. Departure date must be before arrival
        /// </summary>
        private bool CheckDate(ServiceAnswer result, DateTime departureDate, DateTime arrivalDate, bool isCheckDepartureAfterCurrent = true)
        {
            if (result.Status == AnswerStatus.Failure)
            {
                return false;
            }

            if(departureDate.Date < DateTime.Now.Date && isCheckDepartureAfterCurrent)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Date error", "Departure must be after or equal current date.");

                return false;
            }

            if (departureDate.Date > arrivalDate.Date)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Date error", "Departure or arrival date is not valid.");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check existing at system and return required cities 
        /// </summary>
        private IEnumerable<City>GetCities(ServiceAnswer result, Guid idFromCity, Guid idToCity)
        {
            if (result.Status == AnswerStatus.Failure)
            {
                return new List<City>();
            }

            var cities = _cityRepository.GetAll().Where(x => x.Id.Equals(idFromCity) || x.Id.Equals(idToCity)).ToList();
            if (cities.Count() != 2)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("City error", "One of the cities is not found or you selected one same city twice. ");

                return new List<City>();
            }

            return cities;
        }

        /// <summary>
        /// Check that flight aircrew contain least one pilot
        /// </summary>
        private bool CheckExistencePilot(ServiceAnswer result, IEnumerable<Guid> members)
        {
            if (result.Status == AnswerStatus.Failure)
            {
                return false;
            }

            var pilots = _aircrewMemberRepository.GetAll()
                .Where(x => x.Profession.Name == "Pilot")
                .Where(x => members.Contains(x.Id))
                .Count();

            if (pilots == 0)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Pilot error", "In aircrew is no one pilot.");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Set new or edit existing aircrew of flight 
        /// </summary>
        private bool SetAircrew(ServiceAnswer result, Flight flight, IEnumerable<Guid> members)
        {
            if (result.Status == AnswerStatus.Failure)
            {
                return false;
            }

            var newMembers = GetNewMembers(flight, members);

            var aircrewMembers = _aircrewMemberRepository.GetAll()
                .Where(x => x.Status == AircrewMemberStatus.Available)
                .Where(x => newMembers.Contains(x.Id))
                .ToList();

            if (newMembers.Count() != aircrewMembers.Count())
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Some of the aircrew member are not found or unawailable.");

                return false;
            }

            _flightRepository.AddAircrewMembers(flight, aircrewMembers);
            ManageAircrewByStatus(flight);

            return true;
        }

        /// <summary>
        /// Set new flight data without aircrew members
        /// </summary>
        private void SetFlightData(Flight flight, FlightDto updatedFlight , IEnumerable<City> cityes)
        {
            var fromCity = cityes.First(x => x.Id == updatedFlight.From.Id);
            if (flight.From == null || flight.From.Id != fromCity.Id)
            {
                _flightRepository.SetFromCity(flight, fromCity);
            }

            var toCity = cityes.First(x => x.Id == updatedFlight.To.Id);
            if (flight.To == null || flight.To.Id != toCity.Id)
            {
                _flightRepository.SetToCity(flight, toCity);
            }

            flight.Number = updatedFlight.Number.ToUpper();
            flight.DepartureDate = updatedFlight.DepartureDate.Date;
            flight.ArrivalDate = updatedFlight.ArrivalDate.Date;
            flight.Status = updatedFlight.Status;
        }

        /// <summary>
        /// Return string value of flight status
        /// </summary>
        private FlightStatus GetEnumStatus(string status, FlightStatus currentStatus)
        {
            switch (status)
            {
                case "Preparing":
                    return FlightStatus.Preparing;
                case "In Air":
                    return FlightStatus.InAir;
                case "Landed":
                    return FlightStatus.Landed;
                case "Cancelled":
                    return FlightStatus.Cancelled;
                default:
                    return currentStatus;
            }
        }

        /// <summary>
        /// Select new aircrew members and delete old that not required
        /// </summary>
        private IEnumerable<Guid> GetNewMembers(Flight flight, IEnumerable<Guid> newAircrewMembers)
        {
            var toDelete = flight.Aircrew.Where(x => !newAircrewMembers.Contains(x.Id)).ToList();

            var existingMembersIds = flight.Aircrew.Select(x => x.Id).ToList();

            var newMembers = newAircrewMembers.Where(x => !existingMembersIds.Contains(x)).ToList();

            foreach(var user in toDelete)
            {
                user.Status = AircrewMemberStatus.Available;
                user.Flight = null;
                user.FlightId = null;
                flight.Aircrew.Remove(user);
            }

            return newMembers;
        }

        /// <summary>
        /// Set for all flight aircrew members required logic status 
        /// </summary>
        private void ManageAircrewByStatus(Flight flight, bool isDisband = false)
        {
            AircrewMemberStatus requredStatus;
            City requiredCity;

            switch (flight.Status)
            {
                case FlightStatus.Preparing:
                    requredStatus = AircrewMemberStatus.InFlight;
                    requiredCity = flight.From;
                    break;
                case FlightStatus.InAir:
                    requredStatus = AircrewMemberStatus.InFlight;
                    requiredCity = flight.From;
                    break;
                case FlightStatus.Landed:
                    requredStatus = AircrewMemberStatus.Available;
                    requiredCity = flight.To;
                    break;
                case FlightStatus.Cancelled:
                    requredStatus = AircrewMemberStatus.Available;
                    requiredCity = flight.To;
                    break;
                default:
                    return;
            }

            foreach (var member in flight.Aircrew)
            {
                member.Status = !isDisband ? requredStatus : AircrewMemberStatus.Available;
                member.CurrentLocation = requiredCity;
            }
        }

        /// <summary>
        /// Delete users from flight if it needed or if it is needed by app logic (for required status) 
        /// </summary>
        private void SaveOrDisbandAircrew(Flight flight, bool isDisbaned = false)
        {
            if (flight.Status == FlightStatus.Landed || flight.Status == FlightStatus.Cancelled || isDisbaned)
            {
                flight.Aircrew.Clear();
            }
        }

        private IFlightRepository _flightRepository;
        private IAircrewMemberRepository _aircrewMemberRepository;
        private ICityRepository _cityRepository;
    }
}
