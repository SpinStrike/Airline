using System.Collections.Generic;

namespace Airline.AppLogic.Service
{
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
