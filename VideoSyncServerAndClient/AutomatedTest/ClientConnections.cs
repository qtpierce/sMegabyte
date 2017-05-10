using System;


namespace AutomatedTest
{
    class ic_ClientConnections : ic_BasicTest
    {
        ic_BasicTest m_BasicTest = new ic_BasicTest();
        

        public bool ClientConnections()
        {
            String files = "\"c:\\temp\\testcase objects\\171162270.png\",\"c:\\temp\\testcase objects\\1090235720.png\""; 

            if (LaunchServer() == false)
            {
                return false;
            }
            System.Threading.Thread.Sleep(100);


            // Test VideoSyncClient connect+disconnect+reconnect.
            if (LaunchClient() == false)
            {
                return false;
            }
            System.Threading.Thread.Sleep(1000);

            m_library.KillRunningProcess("VideoSyncClient");
            System.Threading.Thread.Sleep(100);


            if (LaunchClient() == false)
            {
                return false;
            }
            System.Threading.Thread.Sleep(1000);

            if (m_library.VerifyProcessIsRunning("VideoSyncClient") == true)
            {
                ;
            }
            else
            {
                Console.WriteLine("-E-  Test ClientConnections::VideoSyncClient connect+disconnect+reconnect failed.");
                return false;
            }




            // Test PlaylistGenerator connect+disconnect+reconnect.
            if (LaunchPlaylistGenerator(files) == false)
            {
                return false;
            }
            System.Threading.Thread.Sleep(1000);

            if (LaunchPlaylistGenerator(files) == false)
            {
                return false;
            }
            System.Threading.Thread.Sleep(1000);

            if (m_library.VerifyProcessIsRunning("vlc") == true)
            {
                ;
            }
            else
            {
                Console.WriteLine("-E-  Test ClientConnections::PlaylistGenerator connect+disconnect+reconnect failed.");
                return false;
            }

            LoopAndKill();

            return true;
        }
    }
}