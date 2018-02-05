using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Airline.AppLogic.Dto;
using Airline.AppLogic.Service;
using Airline.Web.Areas.FlightControl.Models;
using Airline.Web.Attributes.Filters;

namespace Airline.Web.Areas.FlightControl.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public ManageController(ICityService cityService,
           IAircrewMemberService aircrewMemberService,
           IFlightService flightService)
        {
            _cityService = cityService;
            _aircrewMemberService = aircrewMemberService;
            _flightService = flightService;
        }

        [HttpGet]
        [AccessByRole("AirTrafficController")]
        public ActionResult AddFlight()
        {
            var cities = _cityService.GetAll().Result;

            var model = new FlightCreateUpdateModel()
            {
                Cities = cities
            };

            return View(model);
        }

        [HttpPost]
        [AccessByRole("AirTrafficController")]
        public ActionResult AddFlight(FlightDataModel flightDataModel)
        {
            if (ModelState.IsValid)
            {
                var flight = CollectRawData(flightDataModel);

                var result = _flightService.Crete(flight);
                if (result.Status == AnswerStatus.Success)
                {
                    return RedirectToAction("AddFlight");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        if(error.Key == "Number error")
                        {
                            ModelState.AddModelError("flightDataModel.Number", error.Value);
                        }

                        if (error.Key == "Date error")
                        {
                            ModelState.AddModelError("flight.ArrivalDate", error.Value);
                        }

                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
            }

            var cities = _cityService.GetAll().Result;
            var members = GetMemberCollection(flightDataModel);
            var lodededMembers = _aircrewMemberService.FindByIds(members.Select(x => x.Id)).Result;

            var model = new FlightCreateUpdateModel()
            {
                Cities = cities,
                FlightDataModel = flightDataModel,
                CurrentAircrewMemebers = lodededMembers
            };

            return View("AddFlight", model);
        }

        [HttpPost]
        public async Task<JsonResult> AvailableAircrewMembers(Guid? targetId, Guid? flightId = null)
        {
            var availableMemebrs = await Task.Factory.StartNew(() =>
            {
                var targetUsers = _aircrewMemberService.FindByCurrentPosition(targetId.Value, true).Result;

                return targetUsers;
            });

            var result = new List<object>();

            if (flightId != null)
            {
                var flight = _flightService.FindById(flightId.Value).Result;

                if (flight.From.Id == targetId.Value)
                {
                    foreach (var member in flight.AircrewMembers)
                    {
                        result.Add(new { FullName = $"{member.FirstName} {member.SecondName}", Id = member.Id, Prosession = member.Profession.Name });
                    }
                }
            }

            foreach (var member in availableMemebrs)
            {
                result.Add(new { FullName = $"{member.FirstName} {member.SecondName}", Id = member.Id, Prosession = member.Profession.Name });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<PartialViewResult> SetFlightStatus(Guid? flightId, string status)
        {
            var task = await Task.Factory.StartNew<string>(() =>
            {
                var result = _flightService.SetStatus(flightId.Value, status);

                if (result.Status == AnswerStatus.Success)
                {
                    return "Status is changed.";
                }

                return result.Errors.First().Value;
            });

            return PartialView("ChangedStatusResult", task);
        }

        [HttpPost]
        public async Task<JsonResult> DeletPermanentlyFromList(Guid? flightId)
        {
            var task = await Task.Factory.StartNew<ServiceAnswer>(() =>
            {
                return _flightService.Delete(flightId.Value);
            });

            var formatedResult = new List<object>();
            if (task.Status == AnswerStatus.Success)
            {
                formatedResult.Add(new { Message = "Flight is deleted.", Sucsses = true, Id = flightId.Value });
            }
            else
            {
                formatedResult.Add(new { Message = task.Errors.First(), Sucsses = false, Id = flightId.Value });
            }

            return Json(formatedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OnlyAdministratorAccess]
        public ActionResult UpdateFlight(Guid? flightId)
        {
            var flight = _flightService.FindById(flightId.Value).Result;

            var flightModel = new FlightDataModel()
            {
                Id = flight.Id,
                Number = flight.Number,
                FromCity = flight.From.Id,
                ToCity = flight.To.Id,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                Status = flight.Status.GetDescription()
            };

            var cities = _cityService.GetAll().Result;

            var model = new FlightCreateUpdateModel()
            {
                FlightDataModel = flightModel,
                Cities = cities,
                Statuses = _flightService.GetAvailableStatuses().Result,
                CurrentAircrewMemebers = flight.AircrewMembers
            };

            return View("UpdateFlight", model);
        }

        [HttpPost]
        [OnlyAdministratorAccess]
        public ActionResult UpdateFlight(FlightDataModel flightDataModel)
        {
            if (ModelState.IsValid)
            {
                var flight = CollectRawData(flightDataModel);

                var result = _flightService.UpdateFlight(flight);
                if (result.Status == AnswerStatus.Success)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Key == "Number error")
                        {
                            ModelState.AddModelError("flightDataModel.Number", error.Value);
                        }

                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }             
            }

            var cities = _cityService.GetAll().Result;
            var members = GetMemberCollection(flightDataModel);
            var lodededMembers = _aircrewMemberService.FindByIds(members.Select(x => x.Id)).Result;

            var model = new FlightCreateUpdateModel()
            {
                FlightDataModel = flightDataModel,
                Cities = cities,
                Statuses = _flightService.GetAvailableStatuses().Result,
                CurrentAircrewMemebers = lodededMembers
            };

            return View("UpdateFlight", model);
        }

        private FlightDto CollectRawData(FlightDataModel flightDataModel)
        {
            var flight = new FlightDto()
            {
                Number = flightDataModel.Number,
                From = new CityDto() { Id = flightDataModel.FromCity },
                To = new CityDto() { Id = flightDataModel.ToCity },
                DepartureDate = flightDataModel.DepartureDate,
                ArrivalDate = flightDataModel.ArrivalDate,
                Status = _flightService.Status(flightDataModel.Status)
            };

            if (flightDataModel.Id != null)
            {
                flight.Id = flightDataModel.Id.Value;
            }

            var members = GetMemberCollection(flightDataModel);

            flight.AircrewMembers = members;

            return flight;
        }

        private IEnumerable<AircrewMemberDto> GetMemberCollection(FlightDataModel flightDataModel)
        {
            var members = new List<AircrewMemberDto>();
            members.AddRange(flightDataModel.Pilots.Select(x => new AircrewMemberDto() { Id = x }));
            members.AddRange(flightDataModel.AircraftNavigators.Select(x => new AircrewMemberDto() { Id = x }));
            members.AddRange(flightDataModel.RadioOperators.Select(x => new AircrewMemberDto() { Id = x }));
            members.AddRange(flightDataModel.FlightEngineers.Select(x => new AircrewMemberDto() { Id = x }));
            members.AddRange(flightDataModel.Stewardesses.Select(x => new AircrewMemberDto() { Id = x }));

            return members;
        }

        private ICityService _cityService;
        private IAircrewMemberService _aircrewMemberService;
        private IFlightService _flightService;
    }
}