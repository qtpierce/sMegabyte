using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMediaPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMediaPlayer.Tests
{
    [TestClass()]
    public class FilePropertiesTests
    {

        FileProperties m_fileProperties = new FileProperties();

        [TestMethod()]
        public void TextWrapFilePathTest()
        {
            int lineBreakPosition = m_fileProperties.GetLineBreakPosition();
            String shortTestString = "testing";
            shortTestString = shortTestString.PadRight(lineBreakPosition - 1, '1');
            String actual = m_fileProperties.TextWrapFilePath(shortTestString);
            bool shouldNotContainLineBreak = actual.Contains("\n");
            Assert.IsFalse(shouldNotContainLineBreak);


            String longTestString = shortTestString + "2222";
            actual = m_fileProperties.TextWrapFilePath(longTestString);
            bool shouldContainLineBreak = actual.Contains("\n");
            Assert.IsTrue(shouldContainLineBreak);

        }



        [TestMethod()]
        public void ParseTest()
        {
            var parser = new SimpleCommandLineParser();

            String[] args = { "-help" };
            parser.Parse(args);

            bool shouldContainHelp = false;
            if (parser.Arguments.ContainsKey("help"))
            {
                shouldContainHelp = true;
            }
            Assert.IsTrue(shouldContainHelp);


            String[] realArgs = new String[] { "C:\\temp\\Video_Sync_2\\MediaPlayer\\MyMediaPlayer\\bin\\Debug\\MyMediaPlayer.vshost.exe", "-file", "c:\\\\temp\\\\testcase objects\\\\0) Unreal 2004 patching instructions.PNG", "-time", "1" };
            parser.Parse(realArgs);

            bool shouldContainFile = false;
            if (parser.Arguments.ContainsKey("file"))
            {
                shouldContainFile = true;
            }
            Assert.IsTrue(shouldContainFile);


            bool shouldContainTime = false;
            if (parser.Arguments.ContainsKey("time"))
            {
                shouldContainTime = true;
            }
            Assert.IsTrue(shouldContainTime);


        }
    }
}