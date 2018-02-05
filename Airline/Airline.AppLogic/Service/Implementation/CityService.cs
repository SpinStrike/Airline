using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppLogic.Dto;
using Airline.AppData.Repository;

namespace Airline.AppLogic.Service.Implementation
{
    public class CityService : ICityService
    {
        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public ServiceAnswer Create(string cityName)
        {
            var result = new ServiceAnswer();

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

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Creation error", "City with this name already added.");
            }
            catch (Exception exc)
            {
                _cityRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer Delete(Guid id)
        {
            var result = new ServiceAnswer();

            try
            { 
                var cityToDelete = _cityRepository.FindById(id);

                if (cityToDelete != null)
                {
                    _cityRepository.StartTransaction();

                    _cityRepository.Delete(cityToDelete);

                    _cityRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Deleting error", "City is not found.");
            }
            catch (Exception exc)
            {
                _cityRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer Update(CityDto city)
        {
            var result = new ServiceAnswer();

            try
            {
                var cityToUpdate = _cityRepository.FindById(city.Id);

                if (cityToUpdate != null)
                {
                    _cityRepository.StartTransaction();

                    cityToUpdate.Name = city.Name;

                    _cityRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Updating error", "City is not found.");
            }
            catch (Exception exp)
            {
                _cityRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<CityDto> FindById(Guid id)
        {
            var result = new ServiceResult<CityDto>();

            try
            {
                var targetCity = _cityRepository.FindById(id);

                if (targetCity != null)
                {
                    result.Result = targetCity.ToDto();
                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding by id error", "City is not found.");
            }
            catch (Exception exc)
            {
                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<CityDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<CityDto>>();

            try
            {
                var cities = _cityRepository.GetAll()
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => x.ToDto());

                result.Status = AnswerStatus.Success;
                result.Result = cities;

            }
            catch (Exception exc)
            {
                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        private ICityRepository _cityRepository;
    }
}
