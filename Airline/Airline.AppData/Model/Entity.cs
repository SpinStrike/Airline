using System;

namespace Airline.AppData.Model
{
    /// <summary>
    /// Base class for all entities. 
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
