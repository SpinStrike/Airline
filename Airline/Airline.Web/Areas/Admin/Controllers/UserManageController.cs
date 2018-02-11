using System;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity.Owin;
using Airline.AppLogic.Service;
using Airline.AppLogic.Dto;
using Airline.Web.AdditionalExtensions;
using Airline.Web.Attributes.Filters;

namespace Airline.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class UserManageController : Controller
    {
        public UserManageController(IUserSearchService usersearchService,
            IAircrewMemberService aircrewMemberService)
        {
            _usersearchService = usersearchService;
            _aircrewMemberService = aircrewMemberService;
        }

        [HttpGet]
        [OnlyAdministratorAccess]
        public ActionResult UserList()
        {
            var users = _usersearchService.GetAllUsers().Result;

            return View(users);
        }

        [HttpPost]
        public async Task<PartialViewResult> FindBySecondName(string secondName)
        {
            var result = await Task.Factory.StartNew<IEnumerable<UserDto>>(() =>
            {
                Regex checkSecondName = new Regex(@"^[a-zA-Z]{2,50}$");

                var success = checkSecondName.Match(secondName).Success;
                if (success)
                {
                    var targetUsers = _usersearchService.FindeBySecondName(secondName).Result;

                    return targetUsers;
                }

                return new List<UserDto>();
            });

            return PartialView("Users", result);
        }

        [HttpPost]
        public async Task<PartialViewResult> FindByEmail(string email)
        {
            var result = await Task.Factory.StartNew<IEnumerable<UserDto>>(() =>
            {
                Regex checkEmail = new Regex(@"^[a-zA-Z0-9._-]{6,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");

                var success = checkEmail.Match(email).Success;
                if (success)
                {
                    var targetUsers = _usersearchService.FindeByEmail(email).Result;

                    return targetUsers;
                }

                return new List<UserDto>();
            });

            return PartialView("Users", result);
        }

        [HttpPost]
        public async Task<PartialViewResult> FindByRole(string role)
        {
            var result = await Task.Factory.StartNew<IEnumerable<UserDto>>(() =>
            {
                var targetUsers = _usersearchService.FindeByRole(role).Result;

                return targetUsers;
            });

            return PartialView("Users", result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser(Guid? userId)
        {
            var result = await Task.Factory.StartNew<Tuple<string, bool>>(() =>
            {
                if (userId.Value != new Guid(User.Identity.GetIdUser()))
                {
                    var user = _userManager.FindByIdAsync(userId.Value).Result;
                    if(user == null)
                    {
                        return new Tuple<string, bool>("User was not found.", false);
                    }

                    var isAircreMember = _userManager.IsInRoleAsync(user.Id, "AircrewMember").Result;
                    if (isAircreMember)
                    {
                        var targetUserAvailable = _aircrewMemberService.IsAvailableToDelete(userId.Value);
                        if(!targetUserAvailable.Result)
                        {
                            var resultString = new StringBuilder();
                            foreach (var errString in targetUserAvailable.Errors.Values)
                            {
                                resultString.Append($"{errString} ");
                            }

                            return new Tuple<string, bool>(resultString.ToString(), false);
                        }
                    }

                    var deletingResult = _userManager.DeleteAsync(user).Result;
                    if (deletingResult.Succeeded)
                    {
                        return new Tuple<string, bool>("Success. User has been deleted.", true);
                    }

                    return new Tuple<string, bool>("Try again.Some troubles at service.", false);
                }

                return new Tuple<string, bool>("You can't delete yourself.", false);
            });

            var jsonResult = new {  Id = userId.Value, StatusMessage = result.Item1, Sucsses = result.Item2 };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
 
        private AppUserManager _userManager
        {
            get { return HttpContext.GetOwinContext().Get<AppUserManager>(); }
        }

        private IUserSearchService _usersearchService;
        private IAircrewMemberService _aircrewMemberService;
    }
}