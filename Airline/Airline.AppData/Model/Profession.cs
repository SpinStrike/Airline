using System;

namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent profession of concrate aircrewmember.
    /// </summary>
    public class Profession : Entity
    {
        public string Name { get; set; }

        public Guid AircrewMemberId { get; set; }
        public AircrewMember AircrewMember { get; set; }
    }
}
