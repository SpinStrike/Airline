using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Airline.AppLogic.Service;
using Airline.AppLogic.Dto;
using Airline.Web.Models;
using Airline.Web.AdditionalExtensions;

namespace Airline.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController(IFlightService flightService,
            ICityService cityService)
        {
            _flightService = flightService;
            _cityService = cityService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var flights = _flightService.GetAll().Result;
            var cities = _cityService.GetAll().Result;

            var selectedListItemCity = cities.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            var homepageModel = new HomePageModel()
            {
                Flights = flights,
                Cities = selectedListItemCity
            };

            return View(homepageModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetFlightInfo(Guid? flightId)
        {
            var task = await Task.Factory.StartNew<FlightInformationModel>(() =>
            {
                if (flightId != null)
                {
                    var flight = _flightService.FindById(flightId.Value).Result;

                    var result = flight.GetStructetFlightDto();

                    result.AvailableStatuses = _flightService.GetAvailableStatuses(result.Flight).Result
                        .Select(x => new SelectListItem() { Text = x, Value = x });

                    return result;
                }

                return new FlightInformationModel();
            });

            return PartialView("GetFlightInfo", task);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetFilteredFlightList(Guid? fromCityId,
            Guid? toCityId,
            DateTime? departureDate,
            DateTime? arrivalDate)
        {
            var task = await Task.Factory.StartNew<IEnumerable<FlightDto>>(() =>
            {
                return _flightService.GetFilteredList(fromCityId, toCityId, departureDate, arrivalDate).Result; 
            });

            return PartialView("FlightsList", task);
        }

        [HttpPost]
        public async Task<PartialViewResult> FideFlightByNumber(string flightNumber)
        {
            var task = await Task.Factory.StartNew<IEnumerable<FlightDto>>(() =>
            {
                Regex checkNumber = new Regex(@"^[A-Z]{2,3}[0-9]{4,4}$");

                var success = checkNumber.Match(flightNumber).Success;
                if (success)
                {
                    return _flightService.FindByNumber(flightNumber).Result;
                }

                return new  List<FlightDto>();
            });

            return PartialView("FlightsList", task);
        }

        [HttpGet]
        public ActionResult NotAllowAccess()
        {
            return View();
        }

        private IFlightService _flightService;
        private ICityService _cityService;

    }
}