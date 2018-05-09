using System;
using System.Text.RegularExpressions;
using Library;
using System.Windows.Forms;

namespace VideoSyncClient
{
    public class Parser
    {

        public static Globals m_Globals;
        public Parser(Globals Globals_parameter)
        {
            m_Globals = Globals_parameter;
            logFile = m_Globals.m_library.logFile;
        }


        //private Library1 m_library = new Library1();
        private Library.LogFile logFile;
        
        private VLCPlayer vlcPlayer = new VLCPlayer();
        private MyMediaPlayer myMediaPlayer = new MyMediaPlayer();
        

        private void MediaItemPlay(MediaItem newMediaItem)
        {
            String filepath = newMediaItem.GetFilePath();
            if (newMediaItem.CheckIsLocalCopyAvailable(filepath) == true)
            {
                filepath = newMediaItem.GetLocalCopyFilePath();
            }

            newMediaItem.isFilePathValid = m_Globals.m_library.TestFilePathExistance(@filepath);

            if (!newMediaItem.isFilePathValid)
            {   // If the filepath is not valid, then just return.
                return;
            }

            String escapedFilepath = "\"" + @filepath + "\"";  // Must add escaped quotation marks because this will be turned into a string and concatenated into a command string.

            String MediaPlayerPath = "";
            String playArguments = "";
            String playTime = newMediaItem.GetPlayTime();
            String fullScreenArgument = "";
            String partialScreenArgument = "";
            String zoomPlaceholder = "";

            String fileExtension = newMediaItem.GetFileExtension();
            String VLCCompatibilityList = vlcPlayer.GetMediaCompatible();
            if (VLCCompatibilityList.Contains(fileExtension))
            {
                MediaPlayerPath = vlcPlayer.GetExecutablePath(m_Globals);
                playArguments = vlcPlayer.GetPlayArguments(escapedFilepath);
                playTime = vlcPlayer.GetPlayTimeArgument(playTime) + " " + vlcPlayer.GetImageDurationArgument(playTime);
                fullScreenArgument = vlcPlayer.GetFullScreenArgument();
                partialScreenArgument = vlcPlayer.GetPartialScreenArgument();
                zoomPlaceholder = vlcPlayer.GetZoomPlaceholder();
            }
            else
            {
                MediaPlayerPath = myMediaPlayer.GetExecutablePath(m_Globals);
                playArguments = myMediaPlayer.GetPlayArguments(escapedFilepath);
                playTime = myMediaPlayer.GetPlayTimeArgument(playTime);
                fullScreenArgument = myMediaPlayer.GetFullScreenArgument();
                partialScreenArgument = myMediaPlayer.GetPartialScreenArgument();
                zoomPlaceholder = myMediaPlayer.GetZoomPlaceholder();
            }

            if (newMediaItem.isPlayTimeAssigned)
            {
                playArguments += " " + playTime;
            }

            if (m_Globals.m_usePartialScreenSize)
            {   // 1.  Assume a default scalar of 0.4.  Because we have no easy way to determining video dimensions.
                double scalar = 0.4;
                if (newMediaItem.GetClassification() == MediaItem.Classification.image)
                {   // 2.  Here's the easy way to determining an image's dimensions.
                    // http://stackoverflow.com/questions/6455979/how-to-get-the-image-dimension-from-the-file-name
                    scalar = newMediaItem.CalculatePartialScreenScalar();
                }

                String scalarString = scalar.ToString("N2");
                if (String.IsNullOrEmpty(fullScreenArgument))
                {
                    playArguments += " " + partialScreenArgument;
                }
                else
                {
                    playArguments = Regex.Replace(playArguments, fullScreenArgument, partialScreenArgument);
                }
                playArguments = Regex.Replace(playArguments, zoomPlaceholder, scalarString);
            }
            else
            {
                m_Globals.m_library.MoveMouseOffScreen();
            }

            m_Globals.m_library.ExecuteCommand(MediaPlayerPath, playArguments);
        }
         


        private void MediaItemCopy (MediaItem newMediaItem)
        {
            String filepath = newMediaItem.GetFilePath();
            
            newMediaItem.CreateLocalCopy(filepath);
        }


        private static String m_playListDetails = "";

        public void ProcessEvents(String name, String details)
        {
            String tempPath = m_Globals.Get_tempPath();
            logFile.WriteToLog("\r\n\r\n-I-  name: "+ name +"details (playlist): \r\n"+ details);
            details.Trim();

            MediaPlayer blackScreenPlayer = myMediaPlayer;

            if (name.Contains("playlist"))
            {
                m_playListDetails = details;
                m_Globals.m_library.SetStateFile(Library1.State.copying);
            }

            if (name.Equals("server_play"))
            {
                details = m_playListDetails;
                m_Globals.m_library.SetStateFile(Library1.State.playing);
                if (!m_Globals.m_usePartialScreenSize)
                {
                    int intentionalDelayToKeepExecutionsInOrder = 1000;
                    m_Globals.m_library.ExecuteCommand_NoWait(blackScreenPlayer.GetExecutablePath(m_Globals), blackScreenPlayer.GetBlackScreenPlayArguments(blackScreenPlayer.GetBlackScreenImagePath()));
                    System.Threading.Thread.Sleep(intentionalDelayToKeepExecutionsInOrder);
                }
            }

            if (name.Contains("playlist") && String.IsNullOrEmpty(m_playListDetails))
            {
                MessageBox.Show("The playlist variable was empty.  Please send a playlist first.");
                return;
            }

            int CountMediaItems = 0;
            string[] lines = Regex.Split(details, "[\r\n]+");
            foreach (String mediaItem in lines)
            {
                if(m_Globals.m_ShouldRun == false)
                {
                    continue;
                }
                if (String.IsNullOrEmpty(mediaItem))
                {
                    // Empty strings happen at the last \n of the details.
                    // But I added .Trim() which should have removed that trailing whitespace.
                }
                else
                {
                    MediaItem newMediaItem = new MediaItem(CountMediaItems, "", tempPath, m_Globals.m_library);
                    CountMediaItems++;
                        
                    String[] mediaItemPieces = Regex.Split(mediaItem, newMediaItem.delimiter_Playtime);
                    mediaItemPieces[0].Trim();
                    newMediaItem.SetFilePath(@mediaItemPieces[0]);

                    String playTime = "";
                    if (mediaItemPieces.Length > 1)
                    {
                        playTime = Regex.Replace(mediaItemPieces[1], " seconds", "");

                        int resultFromTryParse;
                        if (!Int32.TryParse(playTime, out resultFromTryParse))
                        {
                            playTime = "10";
                        }
                    }
                    playTime.Trim();

                    if (String.IsNullOrEmpty(playTime))
                    {
                            
                    }
                    else
                    {
                        newMediaItem.SetPlayTime(playTime);
                    }


                    if (name.Contains("playlist"))
                    {
                        MediaItemCopy(newMediaItem);
                    }
                    else if (name.Equals("server_play") || name.Contains("testSettings"))
                    {
                        MediaItemPlay(newMediaItem);
                    }

                }
            }

            // Done with the loop.
            m_Globals.m_library.SetStateFile(Library1.State.not_busy);

            if (name.Contains("playlist"))
            {
                SendFileCopyDoneMessageToServer();
            }
            else if (name.Equals("server_play") || name.Contains("testSettings"))
            {
                SendPlaylistDoneMessageToServer();
            }
            
        }



        public void SendFileCopyDoneMessageToServer()
        {
            SendMessageToServer("client_file_copy_done", "null");
        }


        public void SendPlaylistDoneMessageToServer()
        {
            SendMessageToServer("client_playlist_done", "null");
        }



        void SendMessageToServer(String messageName, String messageContent)
        {
            String messageSent = "";
            messageSent = m_Globals.m_communications.SendMessageToServer(messageName, messageContent);
            logFile.WriteToLog("-I-  Sending message: " + messageSent);
        }
    }
}
