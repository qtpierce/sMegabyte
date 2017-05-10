using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;


namespace MyMediaPlayer
{
    public partial class MainWindow : Window
    {
        private FileProperties m_fileProperties = new FileProperties();
        private PlayActions m_playActions = new PlayActions();
       

        public MainWindow()
        {
            InitializeComponent();

            ProcessArguments();

            if (m_playActions.UsePartialScreenSize)
            {
                ResetPlayerToPartialSize();
            }

            CallMediaPlayer();
        }


        
        private void CallMediaPlayer()
        {
            if (m_fileProperties.IsFilePathGiven && m_fileProperties.DoesFileExist)
            {
                MediaPlayer.Source = m_fileProperties.FilePath;
                MediaPlayer.Play();
            }
            else
            {
                Application.Current.Shutdown(0);
            }
        }



        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (m_playActions.ExitWhenFinished)
            {
                if (m_playActions.IsRunTimeGiven)
                {
                    Stall(m_playActions.RunTime * 1000);
                }
                Application.Current.Shutdown(0);
            }
        }



        private void ProcessArguments()
        {
            string[] args = Environment.GetCommandLineArgs();  // This is the keyword for getting command line arguments.
            var parser = new SimpleCommandLineParser();
            parser.Parse(args);


            String FilePathString;

            if (parser.Arguments.ContainsKey("file"))
            {
                FilePathString = parser.Arguments["file"][0];
                m_fileProperties.FilePath = new Uri(FilePathString);
                m_fileProperties.IsFilePathGiven = true;
                m_fileProperties.TestFileExistence();
            }


            if (parser.Arguments.ContainsKey("time"))
            {
                String RunTimeString = parser.Arguments["time"][0];
                m_playActions.RunTime = Convert.ToInt32(RunTimeString);
                m_playActions.IsRunTimeGiven = true;
                m_playActions.ExitWhenFinished = true;
            }


            if (parser.Arguments.ContainsKey("fullscreen"))
            {
                // Do nothing.
                // The MainWindow.xaml has code that defaults the mediaelement to fullscreen, so there's no need for
                // a command line argument.  But I do want to document the argument because I use it in the calling
                // program as a placeholder.
            }



            foreach (string aKey in parser.Arguments.Keys)
            {
                if (aKey.Contains("zoom"))
                {
                    m_playActions.UsePartialScreenSize = true;
                }
            }
        }



        private void Stall(int TimeDuration)
        {   // Use threads to sleep so that UI processing may happen concurrently.
            Thread t = new Thread(() => Thread.Sleep(TimeDuration));  // Sleep uses milliseconds.
            t.Start();
            t.Join();
        }



        private void ResetPlayerToPartialSize()
        {
            String filePathString = m_fileProperties.FilePath.ToString();
            String filePathWithLineBreaks = m_fileProperties.TextWrapFilePath(filePathString);
            label_Title.Content = filePathWithLineBreaks;
            label_Title.Background = new SolidColorBrush(Colors.Black);
            label_Title.Background.Opacity = 0.5;

            WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            this.WindowState = WindowState.Normal;
        }
    }
}



// Notes.
// https://code.msdn.microsoft.com/windowsdesktop/How-to-Use-Media-Element-b8ce7e92/view/SourceCode#content
// 1.  This plays *.mp3, *.mp4, *.jpg, *.png, *.bmp, 
// 2.  It does NOT play *.ogg, *.ogv, nor *.mov.

// TODO:
// 1.  Add ogg support:   http://osconqsc2015.azurewebsites.net/qsccontent/FFmpeg/FFmpeg.html  NO!  I used VLC to handle ogg support instead.

