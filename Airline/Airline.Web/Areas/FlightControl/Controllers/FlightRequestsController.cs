using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Airline.Web.AdditionalExtensions;
using Airline.Web.Areas.FlightControl.Models;
using Airline.Web.Attributes.Filters;
using Airline.AppLogic.Service;

namespace Airline.Web.Areas.FlightControl.Controllers
{
    [Authorize]
    public class FlightRequestsController : Controller
    {
        public FlightRequestsController(IFlightRequestService flightRequestService)
        {
            _flightRequestService = flightRequestService;
        }

        /// <summary>
        /// Output list of flight requests (answers) sent to user.
        /// </summary>
        [HttpGet]
        [AccessByRole("Dispatcher")]
        public ActionResult Messages()
        {
            var requests = GenereteRequestList();

            return View(requests);
        }

        /// <summary>
        /// Send flight request to administrator.
        /// </summary>
        [HttpPost]
        [AccessByRole("Dispatcher")]
        public ActionResult SendRequest(FlightRequestModel flightRequestModel)
        {
            if (ModelState.IsValid)
            {
                var userFrom = _userManager.FindByEmailAsync(flightRequestModel.From).Result;
                var userTo = _userManager.FindByEmailAsync(flightRequestModel.To).Result;

                if(userTo == null)
                {
                    ModelState.AddModelError("To", "Administrator not found.");
                }
                else if (!_userManager.IsInRoleAsync(userFrom.Id, "Dispatcher").Result)
                {
                    ModelState.AddModelError("From", "Sender is not Dispatcher");
                }
                else if (!_userManager.IsInRoleAsync(userTo.Id, "Administrator").Result)
                {
                    ModelState.AddModelError("To", "Recipient is not Administrator");
                }
                else
                {
                    var result = _flightRequestService.Create(userFrom.Email, userTo.Email, flightRequestModel.Message);
                    if (result.Status == AnswerStatus.Success)
                    {
                        return RedirectToAction("Messages");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }
                }

            }

            var requests = GenereteRequestList();

            return View("Messages", requests);
        }

        /// <summary>
        /// Load flight request from dispatcher to administrator. 
        /// </summary>
        [HttpPost]
        public async Task<PartialViewResult> LoadRequestToChangeStatus(Guid? id)
        {
            var task = await Task.Factory.StartNew<FlightRequestModel>(() =>
            {
                var request = _flightRequestService.FindById(id.Value).Result;

                var result = new FlightRequestModel()
                {
                    Id = request.Id,
                    From = request.From.Email,
                    To = request.To.Email,
                    Message = request.Message,
                    Date = request.SendTime,
                    Status = request.Status.ToString()
            };

                return result;
            });

            return PartialView("AdminRequestView", task);
        }

        /// <summary>
        /// Load request answer to dispatcher from administrator.
        /// </summary>
        [HttpPost]
        public async Task<PartialViewResult> LoadRequestAnswer(Guid? id)
        {
            var task = await Task.Factory.StartNew<FlightRequestModel>(() =>
            {
                var request = _flightRequestService.FindById(id.Value).Result;

                var result = new FlightRequestModel()
                {
                    Id = request.Id,
                    From = request.From.Email,
                    To = request.To.Email,
                    Message = request.Message,
                    Date = request.SendTime,
                    Status = request.Status.ToString()
                };

                return result;
            });

            return PartialView("AdminAnswer", task);
        }

        /// <summary>
        /// Send administrator answer to dispatcher flight request.
        /// </summary>
        [HttpPost]
        public async Task<JsonResult> AdminFlightAnswer(Guid? requestId, bool isCompleted)
        {
            var task = await Task.Factory.StartNew<string>(() =>
            {
                return _flightRequestService.SetAnswerToRequest(requestId.Value, isCompleted).Errors.FirstOrDefault().Value;
            });

            var result = new List<object>();

            result.Add(new { Id = requestId, Error = task });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete flight request (answer) from loaded list.
        /// </summary>
        [HttpPost]
        public async Task<JsonResult> DeleteRequest(Guid? requestId)
        {
            var task = await Task.Factory.StartNew<string>(() =>
            {
                return _flightRequestService.Delete(requestId.Value).Errors.FirstOrDefault().Value;
            });

            var result = new List<object>();

            result.Add(new { Id = requestId, Error = task });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Render form to write a flight request message.
        /// </summary>
        [HttpPost]
        [AccessByRole("Dispatcher")]
        public PartialViewResult GenerateRequestForm()
        {
            return PartialView("WriteRequest", new FlightRequestModel());
        }

        private IEnumerable<FlightRequestListElementModel> GenereteRequestList()
        {
            var targetId = new Guid(User.Identity.GetIdUser());

            var requests = _flightRequestService.GetUserFlightRequests(targetId).Result
                .Select(x => new FlightRequestListElementModel()
                {
                    Id = x.Id,
                    From = x.From.Email,
                });

            return requests;
        }

        private AppUserManager _userManager
        {
            get { return HttpContext.GetOwinContext().Get<AppUserManager>(); }
        }

        private IFlightRequestService _flightRequestService;
    }
}