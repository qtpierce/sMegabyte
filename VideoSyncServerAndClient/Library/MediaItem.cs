using System;
using System.Windows.Forms;
using System.IO;


namespace Library
{
    public class MediaItem
    {
        private int m_IdNumber = -1;
        private String m_filePath;
        private String m_fileExtension;
        private String m_playtime = "10";
        public String delimiter_Playtime = " @Playtime: ";
        public enum Classification { undef, image, video, audio };
        private String m_localCopyFilePath = "";
        private bool m_IsLocalCopyAvailable = false;
        public bool isFilePathValid = false;
        public bool isPlayTimeAssigned = false;
        private Library1 m_library;

        private String m_tempPath = @"c:\temp\testcase objects\";

        private Classification m_classification = Classification.undef;

        // Regression test:  Library1Tests.cs::SetFilePathTest()
        public MediaItem (int IdNumber, String filePath, String tempPath, Library1 libraryParameter)
        {
            m_library = libraryParameter;
            m_IdNumber = IdNumber;
            SetFilePath(filePath);
            SetTempPath(tempPath);
        }


        // Regression test:  Library1Tests.cs::SetFilePathTest()
        private void SetFileExtension (String filename)
        {
            String lc_filename = filename.ToLower();
            m_fileExtension = System.IO.Path.GetExtension(lc_filename);
            m_fileExtension.Trim();
        }


        // Regression test:  Library1Tests.cs::SetFilePathTest()
        public String GetFileExtension ()
        {
            return m_fileExtension;
        }


        // Regression test:  Library1Tests.cs::SetFilePathTest()
        private Classification SetClassification (String filename)
        {
            String lc_filename = filename.ToLower();
            if (lc_filename.Contains(".jpg") || lc_filename.Contains(".png") || lc_filename.Contains(".bmp") || lc_filename.Contains(".gif"))
            {
                m_classification = Classification.image;
            }

            else if (lc_filename.Contains(".mp4") || lc_filename.Contains(".mov") || lc_filename.Contains(".m4v"))
            {
                m_classification = Classification.video;
            }

            else if (lc_filename.Contains(".mp3") || lc_filename.Contains(".wav") || lc_filename.Contains(".ogg"))
            {
                m_classification = Classification.audio;
            }
            else
            {
                m_classification = Classification.undef;
            }

            return m_classification;
        }


        // Regression test:  Library1Tests.cs::SetFilePathTest()
        public Classification GetClassification ()
        {
            return m_classification;
        }


        // Regression test:  Library1Tests.cs::GetTimeLimitCategoryTest()
        public String GetTimeLimitCategory ()
        {
            String timeLimit = "";
            if (m_classification == Classification.image)
            {
                timeLimit = delimiter_Playtime;
                String lowerCaseFilePath = m_filePath.ToLower();
                if (lowerCaseFilePath.Contains(".gif"))
                {
                    timeLimit += "1 seconds";
                }
                else
                {
                    timeLimit += "10 seconds";
                }
            }

            else if (m_classification == Classification.video)
            {
                timeLimit = "";
            }

            else if (m_classification == Classification.audio)
                timeLimit = "";

            else if (m_classification == Classification.undef)
            {
                timeLimit = "";
            }

            else
            {
                MessageBox.Show("MediaItem.GetTimeLimitCategory has found a new Classification value.  Please extend its if-then chain.");
            }



            return timeLimit;
        }



        // Regression test:  Library1Tests.cs::SetFilePathTest()
        public void SetFilePath(String filePath)
        {
            filePath.Trim();
            m_filePath = filePath;

            if (File.Exists(filePath))
            {
                isFilePathValid = true;
            }

            SetFileExtension(filePath);
            SetClassification(filePath);
        }


        // Regression test:  Library1Tests.cs::SetFilePathTest()
        public String GetFilePath ()
        {
            String filepath = m_filePath;
            return filepath;
        }


        // Regression test:  Library1Tests.cs::SetPlayTimeTest()
        public void SetPlayTime(String playtime)
        {
            m_playtime = playtime;
            isPlayTimeAssigned = true;
        }


        // Regression test:  Library1Tests.cs::SetPlayTimeTest()
        public String GetPlayTime()
        {
            return m_playtime;
        }


        public String GetLocalCopyFilePath ()
        {
            return m_localCopyFilePath;
        }


        public bool GetIsLocalCopyAvailable ()
        {
            return m_IsLocalCopyAvailable;
        }


        public bool CheckIsLocalCopyAvailable (String filepath)
        {
            String filename = Path.GetFileName(filepath);
            String localCopyPath = @m_tempPath + filename;
            if (File.Exists(@localCopyPath))
            {
                m_localCopyFilePath = localCopyPath;
                m_IsLocalCopyAvailable = true;
                isFilePathValid = true;
            }

            return m_IsLocalCopyAvailable;
        }


        public bool CreateLocalCopy (String filepath)
        {
            if (IsDirectory(filepath))
            {
                return false;
            }

            bool isLocalCopyAvailable = CheckIsLocalCopyAvailable(filepath);

            if (!isLocalCopyAvailable)
            {
                Directory.CreateDirectory(@m_tempPath);
                String filename = Path.GetFileName(filepath);
                String localCopyPath = @m_tempPath + filename;
                try
                {
                    if (m_library.TestFilePathExistance(@filepath))
                    {
                        File.Copy(@filepath, @localCopyPath);
                        if(m_library.TestFilePathExistance(localCopyPath))
                        {
                            isFilePathValid = true;
                        }
                    }
                }
                catch (System.IO.FileNotFoundException fnfe)
                {
                    String exception = fnfe.ToString();
                    isLocalCopyAvailable = false;
                    isFilePathValid = false;
                    return false;
                }
                isLocalCopyAvailable = CheckIsLocalCopyAvailable(filepath);
            }

            return isLocalCopyAvailable;
        }


        public void SetTempPath (String tempPath)
        {
            @m_tempPath = @tempPath +@"\";
        }



        // Regression test:  Library1Tests.cs::IsDirectoryTest()
        public bool IsDirectory (String filepath)
        {
            bool isDirectory = false;
            try
            {
                FileAttributes attr = File.GetAttributes(@filepath);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    isDirectory = true;
                }
            }
            catch (FileNotFoundException fnfe)
            {
                isDirectory = false;
            }
            catch (DirectoryNotFoundException dnfe)
            {
                isDirectory = false;
            }
            catch (SystemException se)
            {
                isDirectory = false;
            }
            return isDirectory;
        }



        public double CalculatePartialScreenScalar()
        {
            double scalar = 0.4;
            if (m_classification == MediaItem.Classification.image)
            {   // 2.  Here's the easy way to determining an image's dimensions.
                // http://stackoverflow.com/questions/6455979/how-to-get-the-image-dimension-from-the-file-name
                try
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(m_filePath);
                    double scalarWidth = (double)640 / (double)img.Width;
                    double scalarHeight = (double)480 / (double)img.Height;

                    if (scalarWidth <= scalarHeight)
                    {
                        scalar = scalarWidth;
                    }
                    else
                    {
                        scalar = scalarHeight;
                    }
                }
                catch (SystemException se)
                {
                    String message = se.Message;
                }
            }

            return (scalar);
        }
    }
}
