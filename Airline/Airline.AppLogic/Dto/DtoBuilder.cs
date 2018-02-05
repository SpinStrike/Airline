using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Model;

namespace Airline.AppLogic.Dto
{
    public static class DtoBuilder
    {
        public static CityDto ToDto(this City city)
        {
            return new CityDto()
            {
                Id = city.Id,
                Name = city.Name
            };
        }

        public static ProfessionDto ToDto(this Profession profession)
        {
            return new ProfessionDto()
            {
                Id = profession.Id,
                Name = profession.Name
            };
        }

        public static UserDto ToDto(this AppUser user, bool isLoadRole = false)
        {
            return new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                BornDate = user.BornDate,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = isLoadRole ? user.Roles.First().Name : null
            };
        }

        public static AircrewMemberDto ToDto(this AircrewMember aircrewMember)
        {
            return new AircrewMemberDto()
            {
                Id = aircrewMember.Id,
                UserName = aircrewMember.Email,
                FirstName = aircrewMember.FirstName,
                SecondName = aircrewMember.SecondName,
                BornDate = aircrewMember.BornDate,
                Status = aircrewMember.Status,
                Email = aircrewMember.Email,
                PhoneNumber = aircrewMember.PhoneNumber,
                Profession = aircrewMember.CurrentLocation != null ? aircrewMember.Profession.ToDto() : null,
                CurrentLocation = aircrewMember.CurrentLocation.ToDto(),
                Flight = aircrewMember.Flight != null ? aircrewMember.Flight.ToDto(false) : null     
            };
        }

        public static FlightDto ToDto(this Flight aircrewMember, bool isLoadMembers = true)
        {
            var loadedCities = aircrewMember.Points.Count();

            var flightDto = new FlightDto()
            {
                Id = aircrewMember.Id,
                Number = aircrewMember.Number,
                Name = loadedCities == 2 ? aircrewMember.Name : null,
                From = loadedCities == 2 ? aircrewMember.From.ToDto() : null,
                To = loadedCities == 2 ? aircrewMember.To.ToDto() : null,
                DepartureDate = aircrewMember.DepartureDate,
                ArrivalDate = aircrewMember.ArrivalDate,
                Status = aircrewMember.Status
            };

            flightDto.AircrewMembers = isLoadMembers ? 
                aircrewMember.Aircrew.Select(x => x.ToDto()) : 
                new List<AircrewMemberDto>();

            return flightDto;
        }

        public static FlightRequestDto ToDto(this FlightRequest flightRequest)
        {
            var request = new FlightRequestDto()
            {
                Id = flightRequest.Id,
                Message = flightRequest.Message,
                SendTime = flightRequest.SendTime,
                IsReaded = flightRequest.IsReaded,
                Status = flightRequest.Status
            };

            request.From = new UserDto()
            {
                Id = flightRequest.From.Id,
                FirstName = flightRequest.From.FirstName,
                SecondName = flightRequest.From.SecondName,
                Email = flightRequest.From.Email
            };

            request.To = new UserDto()
            {
                Id = flightRequest.To.Id,
                FirstName = flightRequest.To.FirstName,
                SecondName = flightRequest.To.SecondName,
                Email = flightRequest.To.Email
            };

            return request;
        }
    }
}
