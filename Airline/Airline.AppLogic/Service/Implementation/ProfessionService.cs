using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Repository;
using Airline.AppLogic.Dto;
using Airline.AppLogic.Logging;

namespace Airline.AppLogic.Service.Implementation
{
    /// <summary>
    /// Implement functionality to work with professios.
    /// </summary>
    public class ProfessionService : IProfessionService
    {
        public ProfessionService(IProfessionRepository professionRepository,
            IServiceLogger logger)
        {
            _professionRepository = professionRepository;
            _logger = logger;
        }

        /// <summary>
        /// Update profession.
        /// </summary>
        /// <param name="profession">New profession data.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Update(ProfessionDto profession)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start update profession method.");

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

                        _logger.Info($"Updated profession (id: {profession.Id}).");
                        _logger.Debug("Finish update profession method.");

                        return result;
                    }

                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Profession was not found.");

                    _logger.Warning($"Profession (id: {profession.Id}) was not found.");
                    _logger.Debug("Finish update profession method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Updating error", $"Profession with name '{profession.Name}' already exists.");

                _logger.Warning($"Profession name '{profession.Name}' alredy used.");
            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();

                _logger.Error($"Exception occurred during the updating  profession, id: {profession.Id}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish update profession method.");

            return result;
        }

        /// <summary>
        /// Create profession.
        /// </summary>
        /// <param name="professionName">Profession name.</param>
        /// <returns>Service answer that contain: success/failure execution and error list of method.</returns>
        public ServiceAnswer Create(string professionName)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start create profession method.");

            try
            {
                var isUniqueName = IsUniqueName(professionName);
                if (isUniqueName)
                {
                    _professionRepository.StartTransaction();

                    _professionRepository.Create(professionName);

                    _professionRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"New profession {professionName} added.");
                    _logger.Debug("Finish create profession method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Creation error", "Profession with this name already added.");

                _logger.Warning($"Profession name {professionName} alredy used.");
            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();

                _logger.Error($"Exception occurred during the creating  profession.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish create profession method.");

            return result;
        }

        /// <summary>
        /// Deletr profession.
        /// </summary>
        /// <param name="id">Profession identifier.</param>
        /// <returns>Service answer that contain: success/failure execution and error list of method.</returns>
        public ServiceAnswer Delete(Guid id)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start delete profession method.");

            try
            {
                var deletedProfession = _professionRepository.FindById(id);
                if (deletedProfession != null)
                {
                    _professionRepository.StartTransaction();

                    _professionRepository.Delete(deletedProfession);

                    _professionRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Profession (id: {id}) has been deleted.");
                    _logger.Debug("Finish delete profession method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Deletting error", "Profession was not found.");

                _logger.Warning($"Profession (id: {id}) was not found.");
            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();

                _logger.Error($"Exception occurred during the deleting  profession, id: {id}.\r\nException: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish delete profession method.");

            return result;
        }

        /// <summary>
        /// Find profesion by identifier.
        /// </summary>
        /// <param name="id">Profession identifier.</param>
        /// <returns>Service answer that contain: result (found profession), success/failure execution and error list of method.</returns>
        public ServiceResult<ProfessionDto> FindById(Guid id)
        {
            var result = new ServiceResult<ProfessionDto>();

            _logger.Debug("Start finde profession by id method.");

            try
            {
                var targetProfession = _professionRepository.FindById(id);

                if (targetProfession != null)
                {
                    result.Result = targetProfession.ToDto();
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Profession (id: {id}) was found.");
                    _logger.Debug("Finish finde profession by id method.");

                    return result;
                }

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Profession with this id not found.");

                _logger.Warning($"Profession (id: {id}) was not found.");
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the finding  profession, id: {id}.\r\n Exception: {exc.ToString()}");

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish finde profession by id method.");

            return result;
        }

        /// <summary>
        /// Get set of all professions.
        /// </summary>
        /// <returns>Service answer that contain: result (found professions), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<ProfessionDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<ProfessionDto>>();

            _logger.Debug("Start get all profession method.");

            try
            {
                var targetProfession = _professionRepository.GetAll()
                    .OrderBy(x => x.Name)
                    .ToList()
                    .Select(x => x.ToDto());

                result.Result = targetProfession;
                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {targetProfession.Count()} professions.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting all professions.\r\n Exception: {exc.ToString()}");

                result.Result = new List<ProfessionDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish get all profession method.");

            return result;
        }

        /// <summary>
        /// Check that profession has unique name.
        /// </summary>
        /// <param name="name">Profession name.</param>
        /// <returns>Boolean answer unique or not.</returns>
        private bool IsUniqueName(string name)
        {
            return _professionRepository.GetAll()
                    .FirstOrDefault(x => x.Name.ToUpper().Equals((name.ToUpper()))) == null ? true : false;
        }

        private IProfessionRepository _professionRepository;
        private IServiceLogger _logger;
    }
}
