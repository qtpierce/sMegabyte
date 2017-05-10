using System;


namespace Library
{
    public class Globals
    {
        private String m_tempPath = "";
        public bool isTempPathSet = false;
        private String m_VLCPath = "";
        public bool isVLCPathSet = false;
        private String m_MyMediaPlayerPath = "";
        public bool isMyMediaPlayerPathSet = false;
        public bool m_usePartialScreenSize = false;

        
        private Library1 m_library = new Library1();
        private XMLDataStructure m_xmlDataStructure = new XMLDataStructure();
        public Communications m_communications = new Communications();



        // Regression test:  VideoSyncClient1Tests.cs::Set_tempPathTest() and all the other specific tests too.
        private bool SetArbitraryFilePath (String newFilePath, ref String globalToSet, ref bool globalIsSet)
        {
            bool didSucceed = false;

            if (m_library.TestFilePathExistance(newFilePath))
            {
                globalToSet = newFilePath;
                globalIsSet = true;
                didSucceed = true;
            }
            else
            {
                globalIsSet = false;
                didSucceed = false;
            }

            return didSucceed;
        }



        // Regression test:  VideoSyncClient1Tests.cs::Set_tempPathTest()
        public bool Set_tempPath(String tempPath)
        {
            return SetArbitraryFilePath(tempPath, ref m_tempPath, ref isTempPathSet);
        }



        // Regression test:  VideoSyncClient1Tests.cs::Set_tempPathTest()
        public String Get_tempPath ()
        {
            return @m_tempPath;
        }



        // Regression test:  VideoSyncClient1Tests.cs::Set_VLCPathTest()
        public bool Set_VLCPath(String VLCPath)
        {
            return SetArbitraryFilePath(VLCPath, ref m_VLCPath, ref isVLCPathSet);
        }



        // Regression test:  VideoSyncClient1Tests.cs::Set_VLCPathTest()
        public String Get_VLCPath()
        {
            return @m_VLCPath;
        }



        // Regression test:  VideoSyncClient1Tests.cs::Set_MyMediaPlayerPathTest()
        public bool Set_MyMediaPlayerPath(String MyMediaPlayerPath)
        {
            return SetArbitraryFilePath(MyMediaPlayerPath, ref m_MyMediaPlayerPath, ref isMyMediaPlayerPathSet);
        }



        // Regression test:  VideoSyncClient1Tests.cs::Set_MyMediaPlayerPathTest()
        public String Get_MyMediaPlayerPath()
        {
            return @m_MyMediaPlayerPath;
        }



        // Regression test:  VideoSyncClient1Tests.cs::SetpathToVariablesFileTest()
        private String m_pathToVariablesFile = "";
        public bool isPathToVariablesFileSet = false;
        public bool SetpathToVariablesFile (String variablesFilePath)
        {
            m_pathToVariablesFile = variablesFilePath;
            isPathToVariablesFileSet = true;
            return isPathToVariablesFileSet;
        }



        // Regression test:  VideoSyncClient1Tests.cs::SetpathToVariablesFileTest()
        public String GetpathToVariablesFile ()
        {
            return m_pathToVariablesFile;
        }



        // Regression test:  VideoSyncClient1Tests.cs::SaveXMLVariablesFileTest()
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
                didSucceed &= m_communications.SetServerName(serverName);
                didSucceed &= m_communications.SetPortNumber(portNumber);
            }

            return didSucceed;
        }



        // Regression test:  VideoSyncClient1Tests.cs::SaveXMLVariablesFileTest()
        public bool SaveXMLVariablesFile ()
        {
            bool didSucceed = false;

            if (isPathToVariablesFileSet)
            {
                m_xmlDataStructure.SaveXMLVariablesFile(@m_pathToVariablesFile, m_tempPath, m_VLCPath, m_MyMediaPlayerPath, m_communications.m_serverName, m_communications.m_portNumber);

            }
            return didSucceed;
        }



        public void AssignVariablesFromXML(String FileName)
        {   // TODO:  write a test for this too.
            if (true)
            {
                SetpathToVariablesFile(FileName);
                LoadXMLVariablesFile();

                m_communications.m_serverName = m_communications.GetServerName();
                m_communications.m_portNumber = m_communications.GetPortNumber();

            }
        }


    }
}
