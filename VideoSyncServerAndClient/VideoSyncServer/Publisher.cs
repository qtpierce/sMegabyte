using System.Collections.Generic;
using System.Net;
using Library;


namespace VideoSyncServer
{
    public class Publisher
    {
        
        private IPEndPoint _ipEndPoint = null;
        public IPEndPoint IpEndPoint
        {
            get { return _ipEndPoint; }
            set { _ipEndPoint = value; }
        }

        private List<Communications.EventData> _events = new List<Communications.EventData>();
        public List<Communications.EventData> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public Publisher(IPEndPoint ipEndPoint, Communications.EventData evnt)
        {
            IpEndPoint = ipEndPoint;
            Events.Add(evnt);
        }
    }
}
