using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Repository;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service.Implementation
{
    public class ProfessionService : IProfessionService
    {
        public ProfessionService(IProfessionRepository professionRepository)
        {
            _professionRepository = professionRepository;
        }

        public ServiceAnswer Update(ProfessionDto profession)
        {
            var result = new ServiceAnswer();

            try
            {
                var isUniqueName = IsUniqueName(profession.Name);
                if (isUniqueName)
                {
                    var targetProfession = _professionRepository.FindById(profession.Id);
                    if(targetProfession != null)
                    {
                        _professionRepository.StartTransaction();

                        targetProfession.Name = profession.Name;

                        _professionRepository.Commit();

                        result.Status = AnswerStatus.Success;

                        return result;
                    }

                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Profession id not found.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Updating error", "Profession with this name already exists."); 
            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer Create(string professionName)
        {
            var result = new ServiceAnswer();

            try
            {
                var isUniqueName = IsUniqueName(professionName);
                if (isUniqueName)
                {
                    _professionRepository.StartTransaction();

                    _professionRepository.Create(professionName);

                    _professionRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Creation error", "Profession with this name already added.");
            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();
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
                var deletedProfession = _professionRepository.FindById(id);
                if (deletedProfession != null)
                {
                    _professionRepository.StartTransaction();

                    _professionRepository.Delete(deletedProfession);

                    _professionRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Deletting error", "Profession is found.");
            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<ProfessionDto> FindById(Guid id)
        {
            var result = new ServiceResult<ProfessionDto>();

            try
            {
                var targetProfession = _professionRepository.FindById(id);

                if (targetProfession != null)
                {
                    result.Result = targetProfession.ToDto();
                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Profession with this id not found.");
            }
            catch(Exception exc)
            {
                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<ProfessionDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<ProfessionDto>>();

            try
            {
                var targetProfession = _professionRepository.GetAll()
                    .OrderBy(x => x.Name)
                    .ToList()
                    .Select(x => x.ToDto());

                result.Result = targetProfession;
                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Result = new List<ProfessionDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        private bool IsUniqueName(string name)
        {
            return _professionRepository.GetAll()
                    .FirstOrDefault(x => x.Name.ToUpper().Equals((name.ToUpper()))) == null ? true : false;
        }

        private IProfessionRepository _professionRepository;
    }
}
