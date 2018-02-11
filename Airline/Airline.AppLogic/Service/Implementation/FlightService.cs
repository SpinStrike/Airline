using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Model;
using Airline.AppData.Repository;
using Airline.AppLogic.Dto;
using Airline.AppLogic.Logging;

namespace Airline.AppLogic.Service.Implementation
{
    /// <summary>
    /// Represent implementation of functionality to work with flights.
    /// </summary>
    public class FlightService : IFlightService
    {
        /// <summary>
        /// Flight service constructor
        /// </summary>
        /// 
        public FlightService(IFlightRepository flightRepository,
            IAircrewMemberRepository aircrewMemberRepository,
            ICityRepository cityRepository,
            IServiceLogger logger)
        {
            _flightRepository = flightRepository;
            _aircrewMemberRepository = aircrewMemberRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        /// <summary>
        /// Create ne flight. 
        /// </summary>
        /// <param name="flight">flight data.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Crete(FlightDto flight)
        {
            var result = new ServiceAnswer() { Status = AnswerStatus.Success };

            _logger.Debug("Start create new flight method.");

            try
            {
                if (!CheckNumber(result, flight.Number))
                {
                    _logger.Debug("Finish create new flight method.");

                    return result;
                };

                if (!CheckDate(result, flight.DepartureDate, flight.ArrivalDate))
                {
                    _logger.Debug("Finish create new flight method.");

                    return result;
                };

                if (!CheckExistencePilot(result, flight.AircrewMembers.Select(x => x.Id)))
                {
                    _logger.Debug("Finish create new flight method.");

                    return result;
                }

                var cities = GetCities(result, flight.From.Id, flight.To.Id);
                if (cities.Count() == 0)
                {
                    _logger.Debug("Finish create new flight method.");

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

                    _logger.Debug("Finish create new flight method.");

                    return result;
                }

                _flightRepository.Commit();

                result.Status = AnswerStatus.Success;

                _logger.Info($"Added new flight.\r\n {newFligth.ToString()}");
                _logger.Debug("Finish create new flight method.");

                return result;
            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();

                _logger.Error($"Exception occurred during the addition of a new flight.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish create new flight method.");

            return result;
        }

        /// <summary>
        /// Delete selected flight 
        /// </summary>
        /// <param name="id">Flight identifier.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Delete(Guid id)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start delete flight method.");

            try
            {
                var targetFlight = _flightRepository.FindById(id); 
                if (targetFlight != null)
                {
                    _flightRepository.StartTransaction();

                    ManageAircrewByStatus(targetFlight, true);
                    SaveOrDisbandAircrew(targetFlight, true);

                    _flightRepository.Delete(targetFlight);

                    _flightRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Flight with id: {id}, has been deleted.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found.");

                _logger.Warning($"Flight (id: {id}) was not found.");
            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();

                _logger.Error($"Exception occurred during the deletting of a  flight, id: {id}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish delete flight method.");

            return result;
        }

        /// <summary>
        /// Find flight by id.
        /// </summary>
        /// <param name="id">Flight identifier.</param>
        /// <returns>Service result that contain result(found flight) success/failure execution and error list of method.</returns>
        public ServiceResult<FlightDto> FindById(Guid id)
        {
            var result = new ServiceResult<FlightDto>();

            _logger.Debug("Start find flight by id method.");

            try
            {
                var targetFlight = _flightRepository.FindById(id); ;

                result.Result = targetFlight.ToDto();
                result.Status = AnswerStatus.Success;

                if (targetFlight != null)
                {
                   _logger.Info($"Flight (Id : {id}) was found.");
                }
                else
                {
                  _logger.Warning($"Flight (Id : {id}) was not found.");
                }
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of a flight, id: {id}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find flight by id method.");

            return result;
        }

        /// <summary>
        /// Get list of all flights.
        /// </summary>
        /// <returns>Service result that contain result(found flights) success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<FlightDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<FlightDto>>();

            _logger.Debug("Start get all flights method.");

            try
            {
                var targetFlights = _flightRepository.GetAll().ToList(); ;

                result.Result = targetFlights.Select(x => x.ToDto(false));
                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {targetFlights.Count()} flights.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting a list of all flights.\r\n Exception: {exc.ToString()}");

                result.Result = new List<FlightDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish get all flights method.");

            return result;
        }

        /// <summary>
        /// Get list of flights by selected parameters (departure city and date. arrival city and date).
        /// </summary>
        /// <param name="fromCityId">From city identifier.</param>
        /// <param name="toCityId">To city identifier.</param>
        /// <param name="departDate">Departure date.</param>
        /// <param name="arriveDate">Arrival date.</param>
        /// <returns>Service result that contain result(found flights) success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<FlightDto>> GetFilteredList(Guid? fromCityId,
            Guid? toCityId,
            DateTime? departDate,
            DateTime? arriveDate)
        {
            var result = new ServiceResult<IEnumerable<FlightDto>>();

           _logger.Debug("Start get filterd list of flights method.");

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
                    //.OrderByDescending(x => x.DepartureDate)
                    .Select(x => x.ToDto(false));

                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {result.Result.Count()} flights.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting a list of flights by parameters.\r\n Exception: {exc.ToString()}");

                result.Result = new List<FlightDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

           _logger.Debug("Finish get filterd list of flights method.");

            return result;
        }

        /// <summary>
        /// Finde flight by number.
        /// </summary>
        /// <param name="flightNumber">Flight number.</param>
        /// <returns>Service result that contain result(found flights) success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<FlightDto>> FindByNumber(string flightNumber)
        {
            var result = new ServiceResult<IEnumerable<FlightDto>>();
            result.Result = new List<FlightDto>();

            _logger.Debug("Start find flights by number method.");

            try
            {
                if (flightNumber == null || flightNumber == string.Empty)
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Number error", "Value of flight number is empty ot null");

                    _logger.Warning("Property flight number is empty or null.");
                    _logger.Debug("Finish find flights by number method.");

                    return result;
                }

                var targetFlight = _flightRepository.FindByNumber(flightNumber);
                if (targetFlight != null)
                {
                    (result.Result as IList<FlightDto>).Add(targetFlight.ToDto(false));
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Was found {result.Result.Count()} floghts.");

                    return result;
                }
                //else
                //{
                //    result.Status = AnswerStatus.Failure;
                //    result.Errors.Add("Finding error", "Flight is not found");
                //}
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting a list of flights by number.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find flights by number method.");

            return result;
        }

        /// <summary>
        /// Set status to selected flight.
        /// </summary>
        /// <param name="idFlight">Flight identifier.</param>
        /// <param name="status">Flight status.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer SetStatus(Guid idFlight, string status)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start set flight status method.");

            if (!GetAvailableStatuses().Result.Contains(status) || status == null)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Argument error", "Invalid value of status.");

                _logger.Warning($"Parametr status is invalid (value: {status??"empty"} ).");
                _logger.Debug("Finish set flight status method.");

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

                    _logger.Info($"To flight (id: {idFlight}) has been set new status '{status}'");
                    _logger.Debug("Finish set flight status method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found.");

                _logger.Warning($"Flight (id: {idFlight}) was not found.");
            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();

                _logger.Error($"Exception occurred during the setting new status of flight, id: {idFlight} status: {status}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish set flight status method.");

            return result;
        }

        /// <summary>
        /// Get set of available statuses to selected flight.
        /// </summary>
        /// <param name="flight">Flight identifier</param>
        /// <returns>Service result that contain result(available statuses) success/failure execution and error list of method.</returns>
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
        /// Convert string status to enum flight status variable.
        /// </summary>
        /// <param name="status">Flight status (as string)</param>
        /// <returns>Flight status variable.</returns>
        public FlightStatus Status(string status)
        {
            return GetEnumStatus(status, FlightStatus.Preparing);
        }

        /// <summary>
        /// Update existing flight.
        /// </summary>
        /// <param name="updatedFlight">New flight data.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer UpdateFlight(FlightDto updatedFlight)
        {
            var result = new ServiceAnswer() { Status = AnswerStatus.Success };

            _logger.Debug("Start update flight method.");

            try
            {
                var originalFlight = _flightRepository.FindById(updatedFlight.Id);
                if (originalFlight != null)
                {
                    if (originalFlight.Number == updatedFlight.Number)
                    { }
                    else if (!CheckNumber(result, updatedFlight.Number))
                    {
                        _logger.Debug("Finidh update flight method.");

                        return result;
                    };
              
                    if (!CheckDate(result, updatedFlight.DepartureDate, updatedFlight.ArrivalDate, false))
                    {
                        _logger.Debug("Finidh update flight method.");

                        return result;
                    };
          
                    if (!CheckExistencePilot(result, updatedFlight.AircrewMembers.Select(x => x.Id)))
                    {
                        _logger.Debug("Finidh update flight method.");

                        return result;
                    }

                    var cities = GetCities(result, updatedFlight.From.Id, updatedFlight.To.Id);
                    if (cities.Count() == 0)
                    {
                        _logger.Debug("Finidh update flight method.");

                        return result;
                    };

                    _flightRepository.StartTransaction();

                    SetFlightData(originalFlight, updatedFlight, cities);

                    if (!SetAircrew(result, originalFlight, updatedFlight.AircrewMembers.Select(x => x.Id)))
                    {
                        _flightRepository.RollBack();

                        _logger.Debug("Finidh update flight method.");

                        return result;
                    }

                    SaveOrDisbandAircrew(originalFlight);

                    _flightRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Flight have been updated.\r\n {originalFlight.ToString()}");
                    _logger.Debug("Finidh update flight method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found.");

                _logger.Warning($"Flight (id: {updatedFlight.Id}) was not found.");
            }
            catch (Exception exc)
            {
                _flightRepository.RollBack();

                _logger.Error($"Exception occurred during the update of a flight, id: {updatedFlight.Id}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finidh update flight method.");

            return result;
        }

        /// <summary>
        /// Check flight number. It must be unique and isn't empty.
        /// </summary>
        /// <param name="result">Service answer, need for creation chain of checks.</param>
        /// <param name="Number">Flight number.</param>
        /// <returns>Boolean result of cheking.</returns>
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

                _logger.Warning("Flight number is empty");

                return false;
            }

            var uniqeNumber = _flightRepository.FindByNumber(Number) == null ? true : false;
            if (!uniqeNumber)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Number error", "Flight number is not uniqe.");

                _logger.Warning($"Flight number '{Number}' is not uniqe");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check correct of departure and arrival date. Departure date must be before arrival.
        /// </summary>
        /// <param name="result">Service answer, need for creation chain of checks.</param>
        /// <param name="departureDate">Departure date.</param>
        /// <param name="arrivalDate">Arival date.</param>
        /// <param name="isCheckDepartureAfterCurrent">Is add condition to check departure date. It must be after current.</param>
        /// <returns>Boolean result of cheking.</returns>
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

                _logger.Warning($"Departure or arrival date is not valid (dep. date: {departureDate.Date.ToString("dd-MM-yyyy")}, arriv. date {arrivalDate.Date.ToString("dd-MM-yyyy")})");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Check at not equality and  existing at system required cities. Return found cities. 
        /// </summary>
        /// <param name="result">Service answer, need for creation chain of checks.</param>
        /// <param name="idFromCity">From city identifier.</param>
        /// <param name="idToCity">To city identifier.</param>
        /// <returns>Boolean result of cheking.</returns>
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
                result.Errors.Add("City error", "One of the cities is not found or you selected one same city twice.");

                _logger.Warning($"One of the cities is not found or one same city twice. Cities id:\r\n {idFromCity}\r\n {idToCity}");

                return new List<City>();
            }

            return cities;
        }

        /// <summary>
        /// Check that flight aircrew contain least one pilot.
        /// </summary>
        /// <param name="result">Service answer, need for creation chain of checks.</param>
        /// <param name="members">List of aircrew members identifiers.</param>
        /// <returns>Boolean result of cheking.</returns>
        private bool CheckExistencePilot(ServiceAnswer result, IEnumerable<Guid> members)
        {
            if (result.Status == AnswerStatus.Failure)
            {
                return false;
            }

            try
            {
                var pilots = _aircrewMemberRepository.GetAll()
                    .Where(x => x.Profession.Name == "Pilot")
                    .Where(x => members.Contains(x.Id))
                    .Count();

                if (pilots == 0)
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Pilot error", "In aircrew is no one pilot.");

                    _logger.Warning("Pilot was not found.");

                    return false;
                }
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the checking the pilot in aircrew.\r\n Exception: {exc.ToString()}");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Set new or edit existing aircrew of flight.
        /// </summary>
        /// <param name="result">Service answer, need for creation chain of checks.</param>
        /// <param name="flight">Flight that updating.</param>
        /// <param name="members">List of aircrew members identifiers.</param>
        /// <returns>Boolean result of success setting new aircrew.</returns>
        private bool SetAircrew(ServiceAnswer result, Flight flight, IEnumerable<Guid> members)
        {
            if (result.Status == AnswerStatus.Failure)
            {
                return false;
            }

            var newMembers = GetNewMembers(flight, members);

            try {
                var aircrewMembers = _aircrewMemberRepository.GetAll()
                    .Where(x => x.Status == AircrewMemberStatus.Available)
                    .Where(x => newMembers.Contains(x.Id))
                    .ToList();

                if (newMembers.Count() != aircrewMembers.Count())
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Some of the aircrew member are not found or unawailable.");

                     _logger.Warning("Not all new aircrew was find in system.");

                    return false;
                }

                _flightRepository.AddAircrewMembers(flight, aircrewMembers);
                ManageAircrewByStatus(flight);

            } catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the changing new aircrew.\r\n Exception: {exc.ToString()}");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Set new flight data without aircrew members.
        /// </summary>
        /// <param name="flight">Flight that updating.</param>
        /// <param name="updatedFlight">New flight data.</param>
        /// <param name="cityes">Cities identifiers.</param>
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
        /// Return enum flight status variable  of flight status.
        /// </summary>
        /// <param name="status">Flight status (as string)</param>
        /// <param name="currentStatus">Flight current status</param>
        /// <returns>Enum flight status variable.</returns>
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
        /// Select new aircrew members and delete old that are not required.
        /// </summary>
        /// <param name="flight">light that updating.</param>
        /// <param name="newAircrewMembers">List of new aircrew memberes identifiers.</param>
        /// <returns>List of new aircrew members identifiers.</returns>
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
        /// Set for all flight aircrew members required aircrew member status. 
        /// </summary>
        /// <param name="flight">Flight that updating.</param>
        /// <param name="isDisband">Is remove aircrew from flight with ignoring flight status.</param>
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
        /// Delete users from flight if it needed or if it is needed by app logic (for required status).
        /// </summary>
        /// <param name="flight">Flight that updating.</param>
        /// <param name="isDisbaned">Is remove aircrew from flight with ignoring flight status.</param>
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
        private IServiceLogger _logger;
    }
}
