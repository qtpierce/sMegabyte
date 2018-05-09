using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace Library
{
    public class Communications
    {

        public Socket m_clientSocket;
        public String m_serverName;
        public String m_portNumber = "8001";
        public bool isServerNameSet = false;
        public bool isPortNumberSet = false;


        public Communications ()
        {
        }



        public bool ConnectToServer()
        {
            bool returnVal = false;
            if (String.IsNullOrEmpty(m_serverName) || String.IsNullOrEmpty(m_portNumber))
            {
                MessageBox.Show("IP Address and Port Number are required to connect to the Server\n");
            }
            try
            {
                Disconnect();  // Just in case a prior connection was made.

                // Create the socket instance
                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Get the remote IP address
                IPHostEntry ipHostInfo = GetIPHostInfo(m_serverName);
               
                IPAddress ip = GetIP4Address(ipHostInfo);

                int iPortNo = System.Convert.ToInt16(m_portNumber);
                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(ip, iPortNo);
                // Connect to the remote host
                m_clientSocket.Connect(ipEnd);
                if (m_clientSocket.Connected)
                {
                    returnVal = true;
                }
            }
            catch (SocketException se)
            {
                string str;
                str = "\nConnection failed, is the server running?\n" + se.Message;
                MessageBox.Show(str);
            }

            return returnVal;
        }


        public void CloseSocket()
        {
            if (m_clientSocket != null)
            {
                m_clientSocket.Close();
                m_clientSocket = null;
            }
        }



        public void Disconnect()
        {
            if (m_clientSocket != null)
            {
                m_clientSocket.Close();
                m_clientSocket = null;
            }
        }



        public System.Net.IPHostEntry GetIPHostInfo(String serverName)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(serverName);

            return ipHostInfo;
        }



        public IPAddress GetIP4Address(IPHostEntry ipHostInfo)
        {
            foreach (IPAddress oneIPAddress in ipHostInfo.AddressList)
            {
                String oneIPAddress_string = oneIPAddress.ToString();
                if (oneIPAddress_string.Contains("."))
                {
                    return oneIPAddress;
                }
            }
            return ipHostInfo.AddressList[0];
        }


        public int GetPortAsInt ()
        {
            return System.Convert.ToInt32(m_portNumber);
        }

        

        public class EventData
        {

            public String Name;
            public String Details;

            public EventData(String name, String details)
            {
                Name = name;
                Details = details;
            }

            /// <summary>
            /// In order to be serialized, an object must have a parameterless Constructor.
            /// </summary>
            public EventData()
            {
            }
        }




        public EventData DeserializeReceivedEventData(string commMessage)
        {
            //Deserialize the received Event
            EventData eventData = new EventData();
            XmlSerializer xmlSerializer;
            MemoryStream memStream = null;
            
            try
            {
                xmlSerializer = new XmlSerializer(typeof(EventData));
                byte[] bytes = new byte[commMessage.Length];
                System.Text.Encoding.UTF8.GetBytes(commMessage, 0, commMessage.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
                object objectFromXml = xmlSerializer.Deserialize(memStream);
                eventData = (EventData)objectFromXml;
            }
            catch (Exception Ex)
            {
                //throw Ex;
                //MessageBox.Show(Ex.Message);
            }
            finally
            {
                if (memStream != null)
                    memStream.Close();
            }
            return eventData;
        }






        public string SerializeEventData(String name, String details)
        {
            EventData eventData = new EventData(name, details);

            return GetSerializeEventData(eventData);
        }




        public string GetSerializeEventData(Communications.EventData eventData)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer = new XmlSerializer(typeof(Communications.EventData));
                MemoryStream memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);
                System.Xml.Serialization.XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
                xs.Add("", "");

                xmlSerializer.Serialize(stWriter, eventData, xs);
                buffer = Encoding.UTF8.GetString(memStream.GetBuffer());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (stWriter != null)
                    stWriter.Close();
            }
            return buffer;
        }



        public class SocketPacket
        {
            public System.Net.Sockets.Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1];
        }




        public String EOMDelimiter = "EOM_ENDOFMESSAGE";
        public bool m_semaphore = false;
        public String SendMessageToServer(String messageName, String messageContent)
        {
            int delayLimit = 10;
            for(int ii = 0; ii < delayLimit; ii++)
            {
                System.Threading.Thread.Sleep(10);
                if(m_semaphore == false)
                {
                    break;
                }
            }
            m_semaphore = true;
            String messageSent = "FAILED";
            try
            {
                Object objData = GetSerializeEventData(new Communications.EventData(messageName, messageContent)) + EOMDelimiter;

                byte[] byData = System.Text.Encoding.UTF8.GetBytes(objData.ToString());
                if (m_clientSocket != null)
                {
                    m_clientSocket.Send(byData);
                    messageSent = objData.ToString();
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
            m_semaphore = false;
            return messageSent;
        }


        // Regression test:  Library1Tests.cs::SetServerNameTest()
        public bool SetServerName(String serverName)
        {
            if (String.IsNullOrEmpty(serverName))
            {
                isServerNameSet = false;
            }
            else
            {
                m_serverName = serverName;
                isServerNameSet = true;
            }
            return isServerNameSet;
        }



        // Regression test:  Library1Tests.cs::SetServerNameTest()
        public String GetServerName()
        {
            return m_serverName;
        }



        // Regression test:  Library1Tests.cs::SetPortNumberTest()
        public bool SetPortNumber(String portNumber)
        {
            if (String.IsNullOrEmpty(portNumber))
            {
                isPortNumberSet = false;
            }
            else
            {
                m_portNumber = portNumber;
                isPortNumberSet = true;
            }
            return isPortNumberSet;
        }



        // Regression test:  Library1Tests.cs::SetPortNumberTest()
        public String GetPortNumber()
        {
            return m_portNumber;
        }





    }
}
