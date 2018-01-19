using System.Collections.Generic;

namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent profession of concrate aircrewmember.
    /// </summary>
    public class Profession : Entity
    {
        public string Name { get; set; }

        public List<AircrewMember> AircrewMember { get; set; }
    }
}
