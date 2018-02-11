using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppLogic.Dto;
using Airline.AppData.Repository;
using Airline.AppLogic.Logging;

namespace Airline.AppLogic.Service.Implementation
{
    /// <summary>
    /// Represent implementation of functionality to work with cities.
    /// </summary>
    public class CityService : ICityService
    {
        public CityService(ICityRepository cityRepository,
            IServiceLogger logger)
        {
            _cityRepository = cityRepository;
            _logger = logger;
        }

        /// <summary>
        /// Create new city.
        /// </summary>
        /// <param name="cityName">City name.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Create(string cityName)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start create new city method.");

            try
            {
                var isUniqueName = _cityRepository.GetAll()
                .FirstOrDefault(x => x.Name.ToUpper().Equals(cityName.ToUpper())) == null ? true : false;

                if (isUniqueName)
                {
                    _cityRepository.StartTransaction();

                    _cityRepository.Create(cityName);

                    _cityRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"New city '{cityName}' added.");
                    _logger.Debug("Finish create new city method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Creation error", $"City with name '{cityName}' already added.");

                _logger.Warning($"City name '{cityName}' already used");
            }
            catch (Exception exc)
            {
                _cityRepository.RollBack();

                _logger.Error($"Exception occurred during the addition of a new city '{cityName}'.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish create new city method.");

            return result;
        }

        /// <summary>
        /// Delete city by identifier.
        /// </summary>
        /// <param name="id">City identifier</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Delete(Guid id)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start delete city method.");

            try
            { 
                var cityToDelete = _cityRepository.FindById(id);

                if (cityToDelete != null)
                {
                    _cityRepository.StartTransaction();

                    _cityRepository.Delete(cityToDelete);

                    _cityRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"City '{cityToDelete.Name}' has been deleted.");
                    _logger.Debug("Finish delete city method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Deleting error", "City is not found.");

                _logger.Warning($"City (id: {id}) was not found.");
            }
            catch (Exception exc)
            {
                _cityRepository.RollBack();

                _logger.Error($"Exception occurred during the deletting of a city.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish delete city method.");

            return result;
        }

        /// <summary>
        /// Update city.
        /// </summary>
        /// <param name="city">New city data.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Update(CityDto city)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start city update method.");

            try
            {
                var cityToUpdate = _cityRepository.FindById(city.Id);

                if (cityToUpdate != null)
                {
                    _cityRepository.StartTransaction();

                    cityToUpdate.Name = city.Name;

                    _cityRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"City '{cityToUpdate.Name}' has been chanched to '{city.Name}'");
                    _logger.Debug("Finish city update method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Updating error", "City is not found.");

                _logger.Warning($"City (id: {city.Id}) was not found");
            }
            catch (Exception exc)
            {
                _cityRepository.RollBack();

                _logger.Error($"Exception occurred during the updating of a city.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish city update method.");

            return result;
        }

        /// <summary>
        /// Find city by identifier.
        /// </summary>
        /// <param name="id">City identifier.</param>
        /// <returns>Service result that contain  result(fount city), success/failure execution and error list of method.</returns>
        public ServiceResult<CityDto> FindById(Guid id)
        {
            var result = new ServiceResult<CityDto>();

            _logger.Debug("Start city find by id method.");

            try
            {
                var targetCity = _cityRepository.FindById(id);

                if (targetCity != null)
                {
                    result.Result = targetCity.ToDto();
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"City (id: {id} was found.)");
                    _logger.Debug("Finish city find by id method.");

                    return result;
                }

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding by id error", "City is not found.");

                _logger.Warning($"City (id: {id}) was not found");

            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of a city by id: {id}.\r\n Exception: {exc.ToString()}");

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish city find by id method.");

            return result;
        }

        /// <summary>
        /// Get all cities.
        /// </summary>
        /// <returns>Service result that contain  result(fount cities), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<CityDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<CityDto>>();

            _logger.Debug("Start get all cities method.");

            try
            {
                var cities = _cityRepository.GetAll()
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => x.ToDto());

                result.Status = AnswerStatus.Success;
                result.Result = cities;

                _logger.Info($"Was found {cities.Count()} cities.");

            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting all of cities.\r\n Exception: {exc.ToString()}");

                result.Result = new List<CityDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish get all cities method.");

            return result;
        }

        private ICityRepository _cityRepository;
        private IServiceLogger _logger;
    }
}
