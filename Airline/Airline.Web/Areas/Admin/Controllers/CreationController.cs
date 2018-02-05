using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Airline.AppLogic.Service;
using Airline.Web.Areas.Admin.Models;
using Airline.AppData.Model;
using Airline.Web.Attributes.Filters;

namespace Airline.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CreationController : Controller
    {
        public CreationController(ICityService cityService, IProfessionService professionService)
        {
            _cityService = cityService;
            _professionService = professionService;
        }

        [HttpGet]
        [OnlyAdministratorAccess]
        public ActionResult AddCity()
        {
            return View();
        }

        [HttpPost]
        [OnlyAdministratorAccess]
        public ActionResult AddCity(CityCreationModel cityCreationModel)
        {
            if (ModelState.IsValid)
            {
                var result = _cityService.Create(cityCreationModel.Name);
                if (result.Status == AnswerStatus.Success)
                {
                    return RedirectToAction("AddCity");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Name", error.Value);
                }
            }

            return View();
        }

        [HttpGet]
        [OnlyAdministratorAccess]
        public ActionResult AddUser()
        {
            var model = GenerateModel();

            return View(model);
        }

        [HttpPost]
        [OnlyAdministratorAccess]
        public ActionResult AddUser(UserDataModel userDataModel)
        {
            if(ModelState.IsValid)
            {
                var newUser = SetUserData(userDataModel);

                var createResult = _appUserManager.CreateAsync(newUser, userDataModel.Password).Result;
                if(createResult.Succeeded)
                {
                    var addToRole = _appUserManager.AddToRoleAsync(newUser.Id, userDataModel.Role).Result;
                    if (addToRole.Succeeded)
                    {
                        return RedirectToAction("AddUser");
                    }
                }
                else
                {
                    foreach(var error in createResult.Errors)
                    {
                        if(error.Contains("Email"))
                        {
                            ModelState.AddModelError("userDataModel.Email", error);
                        }
                    }
                }
            }

            var model = GenerateModel();

            return View(model);
        }

        private UserCreationModel GenerateModel()
        {
            var cities = _cityService.GetAll().Result;
            var professions = _professionService.GetAll().Result;

            var model = new UserCreationModel()
            {
                Cities = cities,
                Professions = professions
            };

            return model;
        }

        private AppUser SetUserData(UserDataModel userDataModel)
        {
            AppUser newUser;
            if (userDataModel.Role.Equals("AircrewMember"))
            {
                newUser = new AircrewMember();
                (newUser as AircrewMember).CityId = userDataModel.CityId.Value;
                (newUser as AircrewMember).ProfessionId = userDataModel.ProfessionId.Value;
            }
            else
            {
                newUser = new AppUser();
            }

            newUser.FirstName = userDataModel.FirstName;
            newUser.SecondName = userDataModel.SecondName;

            var email = userDataModel.Email;
            newUser.UserName = email;
            newUser.Email = email;

            newUser.BornDate = userDataModel.BornDate;
            newUser.PhoneNumber = userDataModel.PhoneNumber;

            return newUser;
        }

        private AppUserManager _appUserManager
        {
            get { return HttpContext.GetOwinContext().Get<AppUserManager>(); }
        }

        private ICityService _cityService;
        private IProfessionService _professionService;
    }
}