using System.Collections.Generic;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Represent service answer object that contain service function execution status and error list.
    /// </summary>
    public class ServiceAnswer
    {
        public AnswerStatus Status { get; set; } = AnswerStatus.Failure;

        public IDictionary<string, string> Errors { get; set; }

        public ServiceAnswer()
        {
            Errors = new Dictionary<string, string>();
        }
    }
}
