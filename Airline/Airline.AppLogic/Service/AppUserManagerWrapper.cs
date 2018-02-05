//using Microsoft.AspNet.Identity;
//using System;
//using Airline.AppLogic.Dto;
//using Airline.AppData.Model;

//namespace Airline.AppLogic.Service
//{
//    public class AppUserManagerWrapper : IDisposable
//    {
//        public AppUserManagerWrapper(IUserStore<AppUser, Guid> userStore)
//        {
//            userManager = new AppUserManager(userStore);
//            _isDisposed = false;
//        }

//        public IdentityResult CreateUser(UserDto user, string password, string role)
//        {
//            AppUser targetUser;

//            if(user is AircrewMemberDto)
//            {
//                var acm = user as AircrewMemberDto;

//                targetUser = new AircrewMember()
//                {
//                    Status = AircrewMemberStatus.Available,
//                    ProfessionId  = acm.Profession.Id,
//                    CityId = acm.CurrentLocation.Id,  
//                };   
//            }
//            else
//            {
//                targetUser = new AppUser();
//            }

//            TransformDtoUser(targetUser, user);

//            var createResult = userManager.Create(targetUser, password);
//            if (createResult.Equals(IdentityResult.Success))
//            {
//                return userManager.AddToRole(targetUser.Id, role);
//            }

//            return createResult;
//        }

//        private AppUser TransformDtoUser(AppUser toUser, UserDto fromUser)
//        {
//            toUser.Id = fromUser.Id;
//            toUser.UserName = fromUser.UserName;
//            toUser.FirstName = fromUser.FirstName;
//            toUser.SecondName = fromUser.SecondName;
//            toUser.PhoneNumber = fromUser.PhoneNumber;
//            toUser.Email = fromUser.Email;
//            toUser.BornDate = fromUser.BirthDate;

//            return toUser;
//        }

//        public void Dispose()
//        {
//            if(!_isDisposed)
//            {
//                userManager.Dispose();
//                userManager = null;
//            }

//            _isDisposed = true;
//        }

//        private AppUserManager userManager;
//        private bool _isDisposed;
//    }
//}
