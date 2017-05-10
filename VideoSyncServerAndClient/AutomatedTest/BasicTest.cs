using System;
using Library;


namespace AutomatedTest
{
    internal class ic_BasicTest
    {
        public static Library1 m_library = new Library1();

        protected String m_files = "\"c:\\temp\\testcase objects\\171162270.png\",\"c:\\temp\\testcase objects\\1090235720.png\"";
        

        private void SetFiles(String files)
        {
            if (String.IsNullOrEmpty(files))
            {
                ;
            }
            else
            {
                m_files = files;
            }
        }




        protected bool LaunchServer()
        {
            String program = "../../../VideoSyncServer/bin/Debug/VideoSyncServer.exe";
            String arguments = "-autoconnect -autoplay";
            m_library.ExecuteCommand_NoWait(program, arguments);

            return ReportProcessIsRunning("VideoSyncServer");
        }




        protected bool ReportProcessIsRunning (String processName, String comment = ", so testing needs to end")
        {
            if (m_library.VerifyProcessIsRunning(processName) == false)
            {
                Console.WriteLine("-I-  {0} is NOT running{1}.", processName, comment);
                return false;
            }
            else
            {
                Console.WriteLine("-I-  {0} is running as expected.", processName);
            }

            return true;
        }




        protected bool ReportProcessIsNotRunning (String processName)
        {
            return ReportProcessIsRunning(processName, ", which is EXPECTED");
        }




        protected bool LaunchClient()
        {
            String program = "../../../VideoSyncClient/bin/Debug/VideoSyncClient.exe";
            String arguments = "-autoconnect";
            m_library.ExecuteCommand_NoWait(program, arguments);

            return ReportProcessIsRunning("VideoSyncClient");
        }




        protected bool LaunchPlaylistGenerator(String files)
        {
            String program = "../../../PlaylistGenerator/bin/Debug/PlaylistGenerator.exe";
            String arguments = "-autoconnect -exit";
            if (String.IsNullOrEmpty(files))
            {
                ;
            }
            else
            {
                arguments = "-autoconnect -exit -file " + files;
            }
            m_library.ExecuteCommand_NoWait(program, arguments);

            bool runningResult = ReportProcessIsRunning("PlaylistGenerator");

            System.Threading.Thread.Sleep(2000);
            bool finishedResult = (ReportProcessIsNotRunning("PlaylistGenerator") == false);

            return (true);
            return (runningResult & finishedResult);
        }


        protected void LoopAndKill ()
        {
            System.Threading.Thread.Sleep(100);

            m_library.LoopUntilVideoSyncClientParserIsNotBusy();
            m_library.KillVideoSyncProcesses();
        }


        public bool BasicTest(String files = "")
        {
            SetFiles(files);

            System.Threading.Thread.Sleep(100);
            if (LaunchServer() == false)
            {
                return false;
            }
            System.Threading.Thread.Sleep(100);

            if (LaunchClient() == false)
            {
                return false;
            }
            System.Threading.Thread.Sleep(1000);

            if (LaunchPlaylistGenerator(m_files) == false)
            {
                return false;
            }

            LoopAndKill();

            return true;
        }

    }
}