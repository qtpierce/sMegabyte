using Library;
using System.Collections.Generic;
using System.Net;
using System;


namespace VideoSyncServer
{
    public class Data
    {

        private List<Publisher> publishers = new List<Publisher>
        {
            //error here!!
            new Publisher(
                    new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001), 
                    new Communications.EventData("first", "details about event first")
                    )
        };

        public List<Publisher> Publishers
        {

            get { return publishers; }
            set { publishers = value; }
        }

   

        public Dictionary<IPAddress, IPEndPoint> publishersIPEndPoints = new Dictionary<IPAddress, IPEndPoint>();
        public void AddPublisher(IPEndPoint endpoint, bool ignorePriorEntry = false)
        {
            
            if (!publishersIPEndPoints.ContainsKey(endpoint.Address))
            {
                publishersIPEndPoints.Add(endpoint.Address, endpoint);
            }
            else
            {
                if (ignorePriorEntry)
                {
                    RemovePublisher(endpoint.Address);
                    publishersIPEndPoints.Add(endpoint.Address, endpoint);
                }
            }
        }


        public void RemovePublisher(IPAddress address)
        {
            publishersIPEndPoints.Remove(address);
        }




        public Dictionary<IPAddress, IPEndPoint> subscribersIPEndPoints = new Dictionary<IPAddress, IPEndPoint>();
        public void AddSubscriber(IPEndPoint endpoint, bool ignorePriorEntry = false)
        {
            // Why true?  If a subscriber on computer-foo is exited and restarted, it would have a different endpoint.
            // A different endpoint on the same IPAddress means a new subscriber for that computer-foo.  If we don't hash the new endpoint, then
            // the new subscriber never sees an update.
            if (!subscribersIPEndPoints.ContainsKey(endpoint.Address))
            {
                subscribersIPEndPoints.Add(endpoint.Address, endpoint);
            }
            else
            {
                if (ignorePriorEntry)
                {
                    RemoveSubscriber(endpoint.Address);
                    subscribersIPEndPoints.Add(endpoint.Address, endpoint);  // Yes, this will overwrite prior entries if we let it.
                }
            }                   
        }


        public void RemoveSubscriber (IPAddress address)
        {
            subscribersIPEndPoints.Remove(address);
        }
    }
}
