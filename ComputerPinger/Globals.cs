using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;


namespace ComputerPinger
{
    public class Globals
    {
        public enum programState_t { RUN, SHUTDOWN };
        public programState_t programState = programState_t.RUN;

        public int longDelay = 2000;  // The longer this delay, the more stable the ping results.
        public int shortDelay = 10;

        public List<String> PossibleIPAddresses = new List<String>();
                
        public List<String> PossibleSubNets = new List<String>();

        public List<PingAddress> pingAddressList = new List<PingAddress>();

        public Dictionary<Thread, int> threadsDictionary = new Dictionary<Thread, int>();


        public Globals ()
        {

        }


        public bool DEBUG = false;
        public void PrintToLog(String message)
        {
            if(!DEBUG)
            {
                return;
            }
            try
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"C:\temp\computerpingerlog.txt", true))
                {
                    file.WriteLine(message);
                }
            }
            catch (SystemException se)
            {
                // do nothing, it is a debug log that failed to be written to.
            }
        }
     
        
        public bool IsValidURLString(String address)
        {
            Regex r = new Regex("^[a-zA-Z0-9_.:+]+$");
            if (r.IsMatch(address))
            {
                return true;
            }
            return false;
        }
    }
}
