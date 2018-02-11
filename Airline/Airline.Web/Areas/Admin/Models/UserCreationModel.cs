using System.Collections.Generic;
using Airline.AppLogic.Dto;

namespace Airline.Web.Areas.Admin.Models
{
    public class UserCreationModel
    {
        public IEnumerable<CityDto> Cities { get; set; } = new List<CityDto>();

        public IEnumerable<ProfessionDto> Professions { get; set; } = new List<ProfessionDto>();

        public UserDataModel UserDataModel { get; set; } = null;
    }
}