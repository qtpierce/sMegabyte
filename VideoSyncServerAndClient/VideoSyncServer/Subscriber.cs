using System.Collections.Generic;
using System.Net;
using Library;


namespace VideoSyncServer
{
    public class Subscriber
    {

        public IPEndPoint IpEndPoint;
        public List<Communications.EventData> Events;
    }
}
