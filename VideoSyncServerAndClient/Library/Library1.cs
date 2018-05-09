using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;


namespace Library
{
    public class Library1
    {
        #region Constructors
        public Library1()
        {
            logFile = new Library.LogFile("parserLog.txt");
        }
        #endregion

        #region Attributes
        public Library.LogFile logFile; //= new Library.LogFile("parserLog.txt");
        #endregion

        #region Enum
        public enum PathType { DIRECTORY, PROGRAM };
        #endregion

        #region Static Methods
        #endregion

        
        // Regression test:  Library1Tests.cs::GetLocalNameTest()
        public String GetLocalName()
        {
            return (Dns.GetHostName());
        }




        public bool VerifyFileName(String filepath, bool issueWarningMessage = true)
        {
            bool isValid = true;

            if (filepath.Contains("#"))
            {
                isValid = false;
                if (issueWarningMessage)
                {
                    MessageBox.Show("Warning: that filename contains invalid punctuation.");
                }
            }

            return isValid;
        }



        // Regression test:  Library1Tests.cs::TestFilePathExistanceTest()
        public bool TestFilePathExistance(String FilePath)
        {
            bool DoesPathExist = false;

            if (String.IsNullOrEmpty(FilePath))
            {
                return false;
            }

            if (FilePath.Equals("\r\n") || FilePath.Equals("\r\n "))
            {
                return false;
            }

            PathType thisPathType;
            if (System.IO.Path.HasExtension(FilePath))
            {
                // If it has an extension, then set the Type to program.
                thisPathType = PathType.PROGRAM;
            }
            else
            {
                thisPathType = PathType.DIRECTORY;
            }

            if (thisPathType == PathType.DIRECTORY)
            {
                // Test directory existance here.
                // https://msdn.microsoft.com/en-us/library/system.io.directory.exists(v=vs.110).aspx
                // This method does not use any exceptions.
                DoesPathExist = Directory.Exists(FilePath);
            }
            else
            {
                // Test file existance here.
                // https://msdn.microsoft.com/en-us/library/system.io.file.exists(v=vs.110).aspx
                // This method does not use any exceptions.
                DoesPathExist = File.Exists(FilePath);
            }

            return DoesPathExist;
        }


        // Regression test:  Library1Tests.cs::MoveMouseOffScreenTest()
        public void MoveMouseOffScreen()
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(2000, 0);
        }


        // Regression test:  Library1Tests.cs::MoveMouseOffScreenTest()
        public System.Drawing.Point GetMousePosition()
        {
            return System.Windows.Forms.Cursor.Position;
        }


        // Regression test:  Library1Tests.cs::ExecuteCommandTest()
        public int ExecuteCommand(String command, String arguments, bool WaitForExit = true)
        {
            logFile.WriteToLog("-CMD-  " + command + " " + arguments);
            try
            {

                if (!System.IO.File.Exists(@command))  //  TODO: Create catch for this condition
                {   // The command program does NOT exist.
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append(String.Format ("The program: {0} does not exist.  Please fix the path to it.",command));
                    //sb.Append(String.Format ("{0}{1}{2}", "The program: ", command, " does not exist.  Please fix the path to it."));
                    sb.Append("The program: ");
                    sb.Append(command);
                    sb.Append(" does not exist.  Please fix the path to it.");
                    MessageBox.Show(sb.ToString());
                    logFile.WriteToLog(sb.ToString());
                    return -1;
                }
                var proc = new Process();
                proc.StartInfo.FileName = @command;
                proc.StartInfo.Arguments = " " + arguments;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.LoadUserProfile = false;

                // Speeding up the Process.Start() delays.
                //    Running as administrator does not really resolve the delays.
                //    Draining StandardOut and StandardError caused the threads to stop executing.
                //    sc stop SysMain   http://geekswithblogs.net/akraus1/archive/2015/08/25/166493.aspx   seems to work.  NO, it does NOT.
                proc.Start();
                proc.PriorityClass = ProcessPriorityClass.RealTime;
                proc.PriorityBoostEnabled = true;

                
                if (WaitForExit)
                {
                    proc.WaitForExit();
                }

                int exitCode = 0;
                if (proc.HasExited)
                {
                    exitCode = proc.ExitCode;
                }
                proc.Close();

                return exitCode;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
                //TODO: Good Practice: NO returns inside blocks (including try..catch..finally)
                //exitCode = -1;
            }
            //return exitCode;
            return -1;
        }


        public void ExecuteCommand_NoWait(String command, String arguments)
        {
            ExecuteCommand(command, arguments, false);
        }




        // Regression test:  Library1Tests.cs::VerifyProcessIsRunningTest()
        public bool VerifyProcessIsRunning(String processName)
        {
            bool isExpectedProcessRunning = false;

            try
            {
                Process[] localByName = Process.GetProcessesByName(processName);
                foreach (Process oneProcess in localByName)
                {
                    if (oneProcess.ProcessName.Equals(processName))
                    {
                        isExpectedProcessRunning = true;
                    }
                }
            }
            catch (System.Exception se)
            {
                Console.WriteLine(se.ToString());
            }

            return isExpectedProcessRunning;
        }


        // Regression test:  Library1Tests.cs::VerifyProcessIsRunningTest()
        public bool KillRunningProcess(String processName)
        {
            bool isExpectedProcessRunning = false;

            try
            {
                Process[] localByName = Process.GetProcessesByName(processName);
                foreach (Process oneProcess in localByName)
                {
                    if (oneProcess.ProcessName.Equals(processName))
                    {
                        oneProcess.Kill();
                        isExpectedProcessRunning = true;
                    }
                }
            }
            catch (System.Exception se)
            {
                Console.WriteLine(se.ToString());
            }

            return isExpectedProcessRunning;
        }


        public enum State { first_enum=0, not_busy, busy, copying, playing, error, last_enum };
        public String[] StateString = new String[] {"first_enum", "not_busy", "busy", "copying", "playing", "error", "last_enum" };
        // Regression test:  Library1Tests.cs::StateToStringTest()
        public String StateToString (State currentState)
        {
            if(currentState < State.first_enum || currentState > State.last_enum)
            {
                return "error";
            }
            return StateString[(int)currentState];
        }


        private String m_StateFile = @"c:\temp\parserState.txt";
        // Regression test:  Library1Tests.cs::SetStateFileTest()
        public void SetStateFile (State currentState)
        {
            System.IO.File.WriteAllText(m_StateFile, StateToString(currentState));
        }


        // Regression test:  Library1Tests.cs::StringToStateTest()
        public State StringToState (String currentState)
        {
            for (State iState = State.first_enum; iState <= State.last_enum; iState++)
            {
                String iStateString = StateToString(iState);
                if (currentState.Equals(iStateString))
                {
                    return iState;
                }
            }

            // Else it is an error state.
            return State.error;
        }


        // Regression test:  Library1Tests.cs::SetStateFileTest()
        public State GetStateFile ()
        {
            String currentState = System.IO.File.ReadAllText(m_StateFile);
            return StringToState(currentState);
        }


        // Regression test:  Library1Tests.cs::IsVideoSyncClientParserBusyTest()
        public bool IsVideoSyncClientParserBusy ()
        {
            bool isBusy = true;
            if (GetStateFile() == State.not_busy)
            {
                isBusy = false;
            }

            return isBusy;
        }


        // Regression test:  Library1Tests.cs::LoopUntilVideoSyncClientParserIsNotBusyTest()
        public bool LoopUntilVideoSyncClientParserIsNotBusy (int resolution = 1000)
        {
            while (IsVideoSyncClientParserBusy() == true)
            {
                System.Threading.Thread.Sleep(resolution);
            }
            return true;
        }



        public void KillVideoSyncProcesses()
        {
            KillRunningProcess("vlc");
            KillRunningProcess("MyMediaPlayer");
            KillRunningProcess("VideoSyncClient");
            KillRunningProcess("PlaylistGenerator");
            KillRunningProcess("VideoSyncServer");
        }
    }


    public class MessageBuffer
    {
        public MessageBuffer()
        {
            m_text = "";
        }


        private String m_text;

        public String GetText ()
        {
            return m_text;
        }

        public void SetText (String text)
        {
            m_text = text;
        }

        public void AppendText (String text)
        {
            text = text.Replace("\0", string.Empty);
            m_text = String.Concat(m_text, text);

        }

    }



    public class LogFile
    {
        protected String m_filename = "";
        private String m_filepath = "";

        public LogFile(String filename = "log.txt", String filepath = @"c:\temp\")
        {
            SetFilePath(filepath);
            SetFileName(filename);
        }


        public void WriteToLog(String text)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@m_filepath+'\\'+m_filename, true))
            {
                file.WriteLine(text);
            }
        }


        public void SetFileName (String filename)
        {
            m_filename = filename;
        }


        public void SetFilePath(String filepath)
        {
            m_filepath = filepath;
        }
    }
}

