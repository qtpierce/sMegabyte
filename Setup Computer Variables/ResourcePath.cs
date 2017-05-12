using System;
using System.IO;


namespace SetupComputerVariables
{
    class ResourcePath
    {
        private String FileName;
        private String FullPath;
        private String Path;
        public enum Type{ DIRECTORY, PROGRAM };
        private Type PathType;
        public bool PathExists;


        public ResourcePath( )
        {
            PathExists = false;
        }


        public String GetPath( )
        {
            return Path;
        }


        public String GetFileName( )
        {
            return FileName;
        }


        public String GetFullPath( )
        {
            return FullPath;
        }


        public bool SetFullPath ( String myFilePath, bool TestThatPathExists = true )
        {
            if ( TestThatPathExists == true )
            {
                PathExists = TestFilePathExistance( myFilePath );
            }
            else
            {
                PathExists = true;
            }
            String myPath = myFilePath;
            String myFileName = "";

            if ( PathType == Type.DIRECTORY )
            {
                // Do nothing, the FilePath ends at a directory.
            }
            else if ( PathType == Type.PROGRAM )
            {
                // Assume the FilePath ends in a programs name and we must strip that name off before setting the path.
                myPath = System.IO.Path.GetDirectoryName( myFilePath );
                myFileName = System.IO.Path.GetFileName( myFilePath );
            }


            if ( PathExists == false )
            {
                Path = "";
                FullPath = "";
            }
            else
            {
                Path = myPath;
                FullPath = myFilePath;
            }
            FileName = myFileName;

            return true;
        }


        public bool TestFilePathExistance( String FilePath )
        {
            bool DoesPathExist = false;

            if ( String.IsNullOrEmpty( FilePath ) )
            {
                return false;
            }

            if ( System.IO.Path.HasExtension( FilePath ) )
            {
                // If it has an extension, then set the Type to program.
                PathType = Type.PROGRAM;
            }

            if ( PathType == Type.DIRECTORY )
            {
                // Test directory existance here.
                // https://msdn.microsoft.com/en-us/library/system.io.directory.exists(v=vs.110).aspx
                // This method does not use any exceptions.
                DoesPathExist = Directory.Exists( FilePath );
            }
            else
            {
                // Test file existance here.
                // https://msdn.microsoft.com/en-us/library/system.io.file.exists(v=vs.110).aspx
                // This method does not use any exceptions.
                DoesPathExist = File.Exists( FilePath );
            }

            return DoesPathExist;
        }
    }
}
