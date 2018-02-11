using Airline.Web.Models;
using System.Web;
using System.Web.Mvc;
using Airline.AppLogic.Service;
using Microsoft.AspNet.Identity.Owin;
using Airline.Web.App_Start.Identity;
using System;
using Airline.Web.AdditionalExtensions;
using Airline.AppLogic.Dto;
using Airline.Web.Attributes.Filters;

namespace Airline.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        public AccountController(ICityService cityService,
            IProfessionService professionService,
            IAircrewMemberService aircrewMemberService)
        {
            _cityService = cityService;
            _professionService = professionService;
            _aircrewMemberService = aircrewMemberService;
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel signInModel)
        {
            if(ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignIn(signInModel.Email, signInModel.Password, signInModel.IsRememberMe, false);
                if (result.Equals(SignInStatus.Success))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Authorithation error", "Incorrect password or e-mail");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult SignOff()
        {
            _signInManager.AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserInformation(Guid? userId)
        {
            var signInUserId = new Guid(User.Identity.GetIdUser());
            var targetUser = userId == null ? signInUserId : userId.Value;

            if (targetUser != signInUserId)
            {
                if (_userManager.IsInRoleAsync(targetUser, "Administrator").Result)
                {
                    return View("Index", "Home", new { area = "" });
                }
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == targetUser;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(targetUser, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(targetUser);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(targetUser).Result.ToDto();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateFirstName(FirstOrSecondNameModel firstOrSecondNameModel)
        {
            if(ModelState.IsValid)
            {
                var result = _userManager.SetFirstName(firstOrSecondNameModel.Id, firstOrSecondNameModel.FSName);
                if(result.Succeeded)
                {
                    if (firstOrSecondNameModel.Id == new Guid(User.Identity.GetIdUser()))
                    {
                        var user = _userManager.FindByIdAsync(firstOrSecondNameModel.Id).Result;
                        _signInManager.AuthenticationManager.SignOut();
                        _signInManager.SignInAsync(user, false, false);
                    }

                    return RedirectToAction("UserInformation", new { userId = firstOrSecondNameModel.Id });
                }
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == firstOrSecondNameModel.Id;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(firstOrSecondNameModel.Id, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(firstOrSecondNameModel.Id);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(firstOrSecondNameModel.Id).Result.ToDto();

            return View("UserInformation", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateSecondName(FirstOrSecondNameModel firstOrSecondNameModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userManager.SetSecondName(firstOrSecondNameModel.Id, firstOrSecondNameModel.FSName);
                if (result.Succeeded)
                {
                    if (firstOrSecondNameModel.Id == new Guid(User.Identity.GetIdUser()))
                    {
                        var user = _userManager.FindByIdAsync(firstOrSecondNameModel.Id).Result;
                        _signInManager.AuthenticationManager.SignOut();
                        _signInManager.SignInAsync(user, false, false);
                    }

                    return RedirectToAction("UserInformation", new { userId = firstOrSecondNameModel.Id });
                }
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == firstOrSecondNameModel.Id;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(firstOrSecondNameModel.Id, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(firstOrSecondNameModel.Id);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(firstOrSecondNameModel.Id).Result.ToDto();

            return View("UserInformation", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateBirthDate(BirthDateModel birthDateModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userManager.SetBornDate(birthDateModel.Id, birthDateModel.Date);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserInformation", new { userId = birthDateModel.Id });
                }
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == birthDateModel.Id;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(birthDateModel.Id, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(birthDateModel.Id);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(birthDateModel.Id).Result.ToDto();

            return View("UserInformation", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdatePhoneNumber(PhoneNumberModel phoneNumberModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userManager.SetPhoneNumberAsync(phoneNumberModel.Id, phoneNumberModel.PhoneNumber).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("UserInformation", new { userId = phoneNumberModel.Id });
                }
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == phoneNumberModel.Id;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(phoneNumberModel.Id, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(phoneNumberModel.Id);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(phoneNumberModel.Id).Result.ToDto();

            return View("UserInformation", model); ;
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateEmail(EmailModel emailModel)
        {
            if (ModelState.IsValid)
            {

                var result = _userManager.SetEmailAsync(emailModel.Id, emailModel.Email).Result;
                if (result.Succeeded)
                {
                    if (emailModel.Id == new Guid(User.Identity.GetIdUser()))
                    {
                        var user = _userManager.FindByIdAsync(emailModel.Id).Result;
                        _signInManager.AuthenticationManager.SignOut();
                        _signInManager.SignInAsync(user, false, false);
                    }

                    return RedirectToAction("UserInformation", new { userId = emailModel.Id });
                }

                ModelState.AddModelError("Email", "This email is already used");
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == emailModel.Id;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(emailModel.Id, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(emailModel.Id);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(emailModel.Id).Result.ToDto();

            return View("UserInformation", model);
        }

        /// <summary>
        /// Reset password by administrator.
        /// </summary>
        [HttpPost]
        [Authorize]
        public ActionResult UpdatePassword(PasswordModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var token = _userManager.GeneratePasswordResetTokenAsync(passwordModel.Id).Result;
                if (true)
                {
                    var result = _userManager.ResetPasswordAsync(passwordModel.Id, token, passwordModel.NewPassword).Result;
                    if (result.Succeeded)
                    {
                        if (passwordModel.Id == new Guid(User.Identity.GetIdUser()))
                        {
                            var user = _userManager.FindByIdAsync(passwordModel.Id).Result;
                            _signInManager.AuthenticationManager.SignOut();
                            _signInManager.SignInAsync(user, false, false);
                        }

                        return RedirectToAction("UserInformation", new { userId = passwordModel.Id });
                    }
                }
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == passwordModel.Id;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(passwordModel.Id, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(passwordModel.Id);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(passwordModel.Id).Result.ToDto();

            return View("UserInformation", model);
        }

        /// <summary>
        /// Change password by user.
        /// </summary>
        [HttpPost]
        [Authorize]
        public ActionResult ChangePsssword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userManager.ChangePasswordAsync(changePasswordModel.Id, changePasswordModel.OldPassword, changePasswordModel.NewPassword).Result;
                if (result.Succeeded)
                {
                    if (changePasswordModel.Id == new Guid(User.Identity.GetIdUser()))
                    {
                        var user = _userManager.FindByIdAsync(changePasswordModel.Id).Result;
                        _signInManager.AuthenticationManager.SignOut();
                        _signInManager.SignInAsync(user, false, false);
                    }

                    return RedirectToAction("UserInformation", new { userId = changePasswordModel.Id });
                }
            }

            var isChangeYourself = new Guid(User.Identity.GetIdUser()) == changePasswordModel.Id;
            var isAircrewMember = isChangeYourself && User.IsInRole("AircrewMember");

            if (isAircrewMember || _userManager.IsInRoleAsync(changePasswordModel.Id, "AircrewMember").Result)
            {
                var modelAircrewMember = GetAircrewMemberModel(changePasswordModel.Id);

                return View("AircrewMemberInformation", modelAircrewMember);
            }

            var model = _userManager.FindByIdAsync(changePasswordModel.Id).Result.ToDto();

            return View("UserInformation", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangeCurrentLocation(Guid? userId, Guid? cityId)
        {
            if (ModelState.IsValid)
            {
                if (userId != null && cityId != null)
                {
                    var result = _aircrewMemberService.SetCurrentLocation(userId.Value, cityId.Value);
                    if(result.Status == AnswerStatus.Success)
                    {
                        return RedirectToAction("UserInformation", new { userId = userId.Value });
                    }
                }
            }

            var modelAircrewMember = GetAircrewMemberModel(userId.Value);

            return View("AircrewMemberInformation", modelAircrewMember);
        }

        [HttpPost]
        [OnlyAdministratorAccess]
        public ActionResult ChangeProfession(Guid? userId, Guid? professionId, AircrewMemberModel aircrewMemberModel)
        {
            if (ModelState.IsValid)
            {
                if (userId != null && professionId != null)
                {
                    var result = _aircrewMemberService.SetProfession(userId.Value, professionId.Value);
                    if (result.Status == AnswerStatus.Success)
                    {
                        return RedirectToAction("UserInformation", new { userId = userId.Value });
                    }
                }
            }

            var modelAircrewMember = GetAircrewMemberModel(userId.Value);

            return View("AircrewMemberInformation", modelAircrewMember);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangeStatus(Guid? userId, string status)
        {
            if(ModelState.IsValid)
            {
                var result = _aircrewMemberService.SetNewStatus(userId.Value, status);
                if (result.Status == AnswerStatus.Success)
                {
                    return RedirectToAction("UserInformation", new { userId = userId.Value });
                }
            }

            var modelAircrewMember = GetAircrewMemberModel(userId.Value);

            return View("AircrewMemberInformation", modelAircrewMember);
        }

        private AircrewMemberModel GetAircrewMemberModel(Guid userId)
        {
            var cities = _cityService.GetAll().Result;
            var professions = _professionService.GetAll().Result;
            var aircrewMember = _aircrewMemberService.FindById(userId).Result;

            var modelAircrewMember = new AircrewMemberModel()
            {
                User = aircrewMember,
                Cities = cities,
                Professions = professions,
                FlightNumber = aircrewMember.Flight != null ? aircrewMember.Flight.Number : string.Empty
            };

            aircrewMember.Flight = null;

            return modelAircrewMember;
        }

        private AppUserManager _userManager
        {
            get { return HttpContext.GetOwinContext().Get<AppUserManager>(); }
        }

        private AppSignInManager _signInManager
        {
            get { return HttpContext.GetOwinContext().Get<AppSignInManager>(); }
        }

        private ICityService _cityService;
        private IProfessionService _professionService;
        private IAircrewMemberService _aircrewMemberService;
    }
}