using System;


namespace MyMediaPlayer
{
    public class FileProperties
    {
        public Uri FilePath;
        public bool IsFilePathGiven = false;
        public bool DoesFileExist = false;


        public bool TestFileExistence ()
        {
            if (System.IO.File.Exists(FilePath.LocalPath))
            {
                DoesFileExist = true;
            }
            
            return DoesFileExist;
        }



        // Regression test:  FilePropertiesTests.cs::TextWrapFilePathTest()
        int m_lineBreakPosition = 45;
        public String TextWrapFilePath(String filePath)
        {
            String filePathWithLineBreaks = "";
            
            String secondPiece = filePath;
            while (secondPiece.Length > m_lineBreakPosition)
            {
                String firstPiece = secondPiece.Substring(0, m_lineBreakPosition);
                secondPiece = secondPiece.Substring(m_lineBreakPosition);

                filePathWithLineBreaks += firstPiece +"\n";
            }
            filePathWithLineBreaks += secondPiece;

            return filePathWithLineBreaks;
        }



        public int GetLineBreakPosition ()
        {
            return m_lineBreakPosition;
        }
    }
}
