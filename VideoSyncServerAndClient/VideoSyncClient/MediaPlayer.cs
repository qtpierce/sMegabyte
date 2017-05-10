using System;

namespace VideoSyncClient
{
    public class MediaPlayer
    {
        protected String m_name;
        protected String m_executablePath;
        protected String m_playArguments;
        protected String m_blackScreenPlayArguments;
        protected String m_playTimeArgument;
        protected String m_imageDurationArgument;
        protected String m_mediaCompatible;  // file extenstions known to work correctly with a media player.
        protected String m_mediaNotCompatible;  // file extensions known to cause problems with a media player.
        protected String m_fullScreenArgument;  
        protected String m_partialScreenArgument;  // VLC's window sizing is broken in my version.  So this code is not being finished.
        protected String m_zoomPlaceholder;


        public String GetName ()
        {
            return m_name;
        }


        public void SetExecutablePath (String path)
        {
            m_executablePath = path;
        }


        public String GetExecutablePath()
        {
            return m_executablePath;
        }


        public String GetPlayArguments(String fileToPlay)
        {
            return m_playArguments + fileToPlay;
        }


        public String GetBlackScreenPlayArguments(String fileToPlay)
        {
            return m_blackScreenPlayArguments + fileToPlay;
        }


        public String GetPlayTimeArgument (String playTime)
        {
            return m_playTimeArgument + playTime;
        }


        public String GetImageDurationArgument(String playTime)
        {
            return m_imageDurationArgument + playTime;
        }


        public String GetMediaCompatible ()
        {
            return m_mediaCompatible;
        }


        public String GetMediaNotCompatible()
        {
            return m_mediaNotCompatible;
        }


        public String GetFullScreenArgument()
        {
            return m_fullScreenArgument;
        }


        public String GetPartialScreenArgument()
        {
            return m_partialScreenArgument;
        }



        public String GetZoomPlaceholder ()
        {
            return m_zoomPlaceholder;
        }

    }


    class MyMediaPlayer : MediaPlayer
    {
        public MyMediaPlayer ()
        {
            m_name = "MyMediaPlayer";
            m_executablePath = @"c:\temp\MyMediaPlayer.exe";
            m_fullScreenArgument = "-fullscreen";
            m_playArguments = m_fullScreenArgument + " -file ";
            m_blackScreenPlayArguments = m_playArguments;
            m_playTimeArgument = "-time ";
            m_imageDurationArgument = "-time ";
            m_mediaCompatible = ".bmp .gif .jpg .mp3 .mp4 .png";
            m_mediaNotCompatible = ".m4v .mov .ogg .ogv .wav";
            m_zoomPlaceholder = "ZOOMPLACEHOLDER";
            m_partialScreenArgument = "--zoom=" + m_zoomPlaceholder;
        }
    }


    class VLCPlayer : MediaPlayer
    {
        public VLCPlayer ()
        {
            m_name = "vlc";
            m_executablePath = @"c:\utils\VideoLan\VLC\vlc.exe";
            m_fullScreenArgument = "--fullscreen --no-video-title-show";
            m_playArguments = "--play-and-exit "+ m_fullScreenArgument +" --disable-screensaver --video-on-top --intf dummy --dummy-quiet file:///";
            m_blackScreenPlayArguments = "--image-duration=-1 "+ m_fullScreenArgument +" --no-video-title-show --disable-screensaver --video-on-top --intf dummy --dummy-quiet file:///";
            m_playTimeArgument = "--run-time=";
            m_imageDurationArgument = "--image-duration=";
            m_mediaCompatible = ".jpg .m4v .mov .mp3 .mp4 .ogg .ogv .png .wav";
            m_mediaNotCompatible = ".bmp .gif";
            m_zoomPlaceholder = "ZOOMPLACEHOLDER";
            m_partialScreenArgument = "--video-title-show --video-title-timeout=2147483646 --zoom=" + m_zoomPlaceholder;
        }
    }
}
