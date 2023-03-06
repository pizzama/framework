
namespace PFramework
{
    public interface IObserver
    {
        void handleEvent(ObserverEvent evt);
    }
    public class ObserverEvent
    {
        private string _eventId;
        public string EventId
        {
            get => _eventId;
            set
            {
                _eventId = value;
            }
        }
        private object _eventObject;
        public object EventObject
        {
            get => _eventObject;
            set
            {
                _eventObject = value;
            }
        }
        private object _sender;
        public object Sender
        {
            get => _sender;
            set
            {
                _sender = value;
            }
        }
        public ObserverEvent(string eventId, object eventObject, object sender = null)
        {
            _eventId = eventId;
            _eventObject = eventObject;
            _sender = sender;
        }
    }

}
