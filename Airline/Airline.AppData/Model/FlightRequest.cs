using System.Collections.Generic;
using System.Linq;

namespace Airline.AppData.Model
{
    public class FlightRequest : Entity
    {
        public string Message { get; set; }

        public AdminAnswerStatus Status { get; set; } = AdminAnswerStatus.Undefined;

        public List<SendReceiveBroker> RequestBrokers { get; set; }

        public FlightRequest()
        {
            RequestBrokers = new List<SendReceiveBroker>();
        }

        public AppUser From
        {
            get
            {
                return GetUser(Direction.From).User;
            }
            set
            {
                SetUser(Direction.From, value);
            }
        }

        public AppUser To
        {
            get
            {
                return GetUser(Direction.To).User;
            }
            set
            {
                SetUser(Direction.To, value);
            }
        }

        private SendReceiveBroker GetUser(Direction direction)
        {
            return RequestBrokers.FirstOrDefault(x => x.Direction == direction);
        }

        private void SetUser(Direction direction, AppUser user)
        {
            var targetBroker = GetUser(direction);

            targetBroker.User = user;
            targetBroker.UserId = user.Id;
        }
    }

    public enum AdminAnswerStatus
    {
        Confirm,
        Canceled,
        Undefined
    }
}
