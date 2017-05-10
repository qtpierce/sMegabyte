using System;
using Library;

namespace SubscriberWinForm
{
    class Globals
    {
        private String m_tempPath = "";
        public bool isTempPathSet = false;
        private String m_VLCPath = "";
        public bool isVLCPathSet = false;
        private String m_MyMediaPlayerPath = "";
        public bool isMyMediaPlayerPathSet = false;
        private String m_serverName = "";
        public bool isServerNameSet = false;
        private String m_portNumber = "8000";
        public bool isPortNumberSet = false;

        
        private Library1 m_library = new Library1();
        private XMLDataStructure m_xmlDataStructure = new XMLDataStructure();


        private bool SetArbitraryFilePath (String newFilePath, ref String globalToSet, ref bool globalIsSet)
        {
            bool didSucceed = false;

            if (m_library.TestFilePathExistance(newFilePath))
            {
                globalToSet = newFilePath;
                globalIsSet = true;
                didSucceed = true;
            }

            return didSucceed;
        }


        public bool Set_tempPath(String tempPath)
        {
            return SetArbitraryFilePath(tempPath, ref m_tempPath, ref isTempPathSet);
        }


        public String Get_tempPath ()
        {
            return @m_tempPath;
        }


        public bool Set_VLCPath(String VLCPath)
        {
            return SetArbitraryFilePath(VLCPath, ref m_VLCPath, ref isVLCPathSet);
        }


        public String Get_VLCPath()
        {
            return @m_VLCPath;
        }


        public bool Set_MyMediaPlayerPath(String MyMediaPlayerPath)
        {
            return SetArbitraryFilePath(MyMediaPlayerPath, ref m_MyMediaPlayerPath, ref isMyMediaPlayerPathSet);
        }


        public String Get_MyMediaPlayerPath()
        {
            return @m_MyMediaPlayerPath;
        }



        private String m_pathToVariablesFile;
        public bool isPathToVariablesFileSet = false;
        public bool SetpathToVariablesFile (String variablesFilePath)
        {
            m_pathToVariablesFile = variablesFilePath;
            isPathToVariablesFileSet = true;
            return isPathToVariablesFileSet;
        }


        public String GetpathToVariablesFile ()
        {
            return m_pathToVariablesFile;
        }


        public bool LoadXMLVariablesFile()
        {
            bool didSucceed = false;

            if (isPathToVariablesFileSet)
            {
                String tempPath = "";
                String VLCPath = "";
                String MyMediaPlayerPath = "";
                String serverName = "";
                String portNumber = "";
                m_xmlDataStructure.LoadXMLVariablesFile(@m_pathToVariablesFile, ref tempPath, ref VLCPath, ref MyMediaPlayerPath, ref serverName, ref portNumber);

                didSucceed = Set_tempPath(tempPath);
                didSucceed &= Set_VLCPath(VLCPath);
                didSucceed &= Set_MyMediaPlayerPath(MyMediaPlayerPath);
                didSucceed &= SetServerName(serverName);
                didSucceed &= SetPortNumber(portNumber);
            }

            return didSucceed;
        }


        public bool SaveXMLVariablesFile ()
        {
            bool didSucceed = false;

            if (isPathToVariablesFileSet)
            {
                m_xmlDataStructure.SaveXMLVariablesFile(@m_pathToVariablesFile, m_tempPath, m_VLCPath, m_MyMediaPlayerPath, m_serverName, m_portNumber);

            }
            return didSucceed;
        }


        public bool SetServerName (String serverName)
        {
            if (String.IsNullOrEmpty(serverName))
            {
                isServerNameSet = false;
                return false;
            }
            else
            {
                m_serverName = serverName;
                isServerNameSet = true;
                return true;
            }
        }


        public String GetServerName ()
        {
            return m_serverName;
        }


        public bool SetPortNumber(String portNumber)
        {
            if (String.IsNullOrEmpty(portNumber))
            {
                isPortNumberSet = false;
                return false;
            }
            else
            {
                m_portNumber = portNumber;
                isPortNumberSet = true;
                return true;
            }
        }


        public String GetPortNumber()
        {
            return m_portNumber;
        }
    }
}
