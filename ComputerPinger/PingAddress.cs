using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPinger
{
    public class PingAddress
    {
        private String m_Address;
        public enum t_PingResult { Untested, Testing, KnownGood, KnownBad, Problem };
        public t_PingResult m_PingResult { get; set; }

        public enum t_PingState { Idle, Testing, Problem };
        public t_PingState m_PingState { get; set; }

        public bool m_Chosen { get; set; }

        public enum t_AddressType { Unset, IsURL, IsIPAddress };
        public t_AddressType m_AddressType { get; set; }


        private int m_GoodPingCount;
        private int m_BadPingCount;

        public bool m_foundnOnce;
        public int m_FrequencyModulo;


        public PingAddress() : this("", t_AddressType.Unset, 1)
        {
        }


        public PingAddress(String newAddress, t_AddressType newAddressType, int frequencyModulo)
        {
            m_Address = newAddress;
            m_PingResult = t_PingResult.Untested;
            m_PingState = t_PingState.Idle;
            m_Chosen = false;
            m_AddressType = newAddressType;
            GoodPingCountReset();
            m_FrequencyModulo = frequencyModulo;
            m_foundnOnce = false;
        }


        public void SetAddress(String newAddress)
        {
            m_Address = newAddress;
        }


        public String GetAddress ()
        {
            return m_Address;
        }


        public int GetGoodPingCount()
        {
            return m_GoodPingCount;
        }


        public void GoodPingCountAddition()
        {
            m_GoodPingCount++;
            m_foundnOnce = true;
        }


        public int GetBadPingCount()
        {
            return m_BadPingCount;
        }

        public void BadPingCountAddition()
        {
            m_BadPingCount++;
        }


        public void GoodPingCountReset()
        {
            m_GoodPingCount = 0;
            m_BadPingCount = 0;
            m_foundnOnce = false;
        }
    }
}
