using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Library.Tests
{
    [TestClass()]
    public class Library1Tests
    {
        Library1 m_library = new Library1();
        Communications m_communications = new Communications();


        public Library1 Library
        {
            get
            {
                if (m_library == null)
                {
                    m_library = new Library1();
                }
                return m_library;
            }
        }


        [TestMethod()]
        public void GetIPHostInfoTest()
        {
            String localName = Library.GetLocalName();
            System.Net.IPHostEntry IPHEntry = m_communications.GetIPHostInfo(localName);

            String expectedName = Dns.GetHostName();
            String actualName = IPHEntry.HostName.ToString();
            Assert.AreEqual(expectedName, actualName);
        }


        [TestMethod()]
        public void GetLocalNameTest()
        {
            String localName = m_library.GetLocalName();
            String expectedName = Dns.GetHostName();
            Assert.AreEqual(expectedName, localName);
        }


        [TestMethod()]
        public void GetIP4AddressTest_CorrectIP()
        {


            // Testing correct path.
            String localName = m_library.GetLocalName();
            System.Net.IPHostEntry IPHEntry = m_communications.GetIPHostInfo(localName);
            IPAddress actualAddress = m_communications.GetIP4Address(IPHEntry);
            String actualAddressString = actualAddress.ToString();

            bool isCorrectlyFormatted = actualAddressString.Contains("192.168.");

            String Message = String.Format("IP4 Address did not contain 192.168.  Actual: {0}", actualAddressString);
            Assert.IsTrue(isCorrectlyFormatted, Message);
        }


        [TestMethod()]
        public void GetIP4AddressTest_MissingIP4Address()
        {
            // Testing the failure when the IP4Address format is missing from the structure.
            String localName = m_library.GetLocalName();
            System.Net.IPHostEntry IPHEntry = m_communications.GetIPHostInfo(localName);

            IPAddress fakeAddress;
            String fakeAddressString = "fe80::7d02:2f4c:6208:edc2%12";
            fakeAddress = IPAddress.Parse(fakeAddressString);

            int length = IPHEntry.AddressList.Length;
            for (int i = 0; i < length; i++)
            {
                IPHEntry.AddressList[i] = fakeAddress;
            }


            IPAddress fakeValue = m_communications.GetIP4Address(IPHEntry);
            String fakeValueString = fakeValue.ToString();

            bool isFakeValue = fakeValueString.Contains(fakeAddressString);
            Assert.IsTrue(isFakeValue);
        }


        [TestMethod()]
        public void TestFilePathExistanceTest()
        {
            bool isANullFalse = m_library.TestFilePathExistance("");
            Assert.IsFalse(isANullFalse);

            bool isNewLineFalse = m_library.TestFilePathExistance("\r\n");
            Assert.IsFalse(isNewLineFalse);

            bool isBadFileFalse = m_library.TestFilePathExistance("foofoo.txt");
            Assert.IsFalse(isBadFileFalse);

            bool isDirectoryTrue = m_library.TestFilePathExistance(@"c:\");
            Assert.IsTrue(isDirectoryTrue);

            bool isRealFileTrue = m_library.TestFilePathExistance("UnitTestProject1.dll");
            Assert.IsTrue(isRealFileTrue);

            bool isFakeFileFalse = m_library.TestFilePathExistance("UnitTestProject_FAKE.dl");
            Assert.IsFalse(isFakeFileFalse);
        }


        [TestMethod()]
        public void MoveMouseOffScreenTest()
        {
            System.Drawing.Point initialPoint = m_library.GetMousePosition();

            m_library.MoveMouseOffScreen();
            //System.Drawing.Point expectedPoint = new System.Drawing.Point(2000, 2000);
            System.Drawing.Point actualPoint = m_library.GetMousePosition();

            Assert.AreNotEqual(initialPoint, actualPoint);

            System.Windows.Forms.Cursor.Position = initialPoint;
        }


        [TestMethod()]
        public void ExecuteCommandTest()
        {
            int result = m_library.ExecuteCommand(@"c:\windows\system32\cmd.exe", "/C dir");
            Assert.AreEqual(0, result);
        }


        [TestMethod()]
        public void SetStateFileTest()
        {
            Library1.State expected = Library1.State.first_enum;
            m_library.SetStateFile(expected);
            System.Threading.Thread.Sleep(1);
            Library1.State actual = m_library.GetStateFile();
            Assert.AreEqual(expected, actual);

            expected = Library1.State.last_enum;
            m_library.SetStateFile(expected);
            System.Threading.Thread.Sleep(1);
            actual = m_library.GetStateFile();
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void StateToStringTest()
        {
            Library1.State expected = Library1.State.first_enum;
            String temp = m_library.StateToString(expected);
            Library1.State actual = m_library.StringToState(temp);
            Assert.AreEqual(expected, actual);

            expected = Library1.State.first_enum - 1;
            String actualString = m_library.StateToString(expected);
            String expectedString = m_library.StateToString(Library1.State.error);
            Assert.AreEqual(expectedString, actualString);

            expected = Library1.State.last_enum + 1;
            actualString = m_library.StateToString(expected);
            expectedString = m_library.StateToString(Library1.State.error);
            Assert.AreEqual(expectedString, actualString);

        }


        [TestMethod()]
        public void StringToStateTest()
        {
            String expectedString = "not_busy";
            Library1.State temp = m_library.StringToState(expectedString);
            String actualString = m_library.StateToString(temp);
            Assert.AreEqual(expectedString, actualString);

            Library1.State actual = m_library.StringToState("foo");
            Library1.State expected = Library1.State.error;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void VerifyProcessIsRunningTest()
        {
            m_library.ExecuteCommand_NoWait(@"c:\temp\MyMediaPlayer.exe", "");
            System.Threading.Thread.Sleep(100);

            bool isRunning = m_library.VerifyProcessIsRunning("MyMediaPlayer");
            Assert.IsTrue(isRunning);

            m_library.KillRunningProcess("MyMediaPlayer");
            System.Threading.Thread.Sleep(100);

            bool isNotRunning = m_library.VerifyProcessIsRunning("MyMediaPlayer");
            Assert.IsFalse(isNotRunning);

            isNotRunning = m_library.KillRunningProcess("MyMediaPlayer");
            Assert.IsFalse(isNotRunning);
        }



        [TestMethod()]
        public void IsVideoSyncClientParserBusyTest()
        {
            m_library.SetStateFile(Library1.State.not_busy);
            System.Threading.Thread.Sleep(10);
            bool isNotBusy = m_library.IsVideoSyncClientParserBusy();
            Assert.IsFalse(isNotBusy);

            m_library.SetStateFile(Library1.State.playing);
            System.Threading.Thread.Sleep(10);
            bool isBusy = m_library.IsVideoSyncClientParserBusy();
            Assert.IsTrue(isBusy);

        }


        [TestMethod()]
        public void LoopUntilVideoSyncClientParserIsNotBusyTest()
        {
            m_library.SetStateFile(Library1.State.not_busy);
            System.Threading.Thread.Sleep(10);
            bool isNotBusy = m_library.LoopUntilVideoSyncClientParserIsNotBusy();
            Assert.IsTrue(isNotBusy);
        }


        [TestMethod()]
        public void SetFilePathTest()
        {
            MediaItem m_mediaItem = new MediaItem(0, "", "");
            String expectedPath = "";
            String actualPath = m_mediaItem.GetFilePath();
            Assert.AreEqual(expectedPath, actualPath);

            expectedPath = @"c:\foo.txt";
            m_mediaItem.SetFilePath(expectedPath);
            actualPath = m_mediaItem.GetFilePath();
            Assert.AreEqual(expectedPath, actualPath);

            String expectedExtension = @".txt";
            String actualExtension = m_mediaItem.GetFileExtension();
            Assert.AreEqual(expectedExtension, actualExtension);

            // Assuming c:\foo.txt does not exist, it should also be not valid as well.
            Assert.IsFalse(m_mediaItem.isFilePathValid);

            MediaItem.Classification expectedClassification = MediaItem.Classification.undef;
            MediaItem.Classification actualClassification = m_mediaItem.GetClassification();
            Assert.AreEqual(expectedClassification, actualClassification);


            String blackScreenImagePath = AppDomain.CurrentDomain.BaseDirectory + @"\blackscreenimage.png";
            m_mediaItem.SetFilePath(blackScreenImagePath);
            actualPath = m_mediaItem.GetFilePath();
            Assert.AreEqual(blackScreenImagePath, actualPath);

            expectedExtension = @".png";
            actualExtension = m_mediaItem.GetFileExtension();
            Assert.AreEqual(expectedExtension, actualExtension);

            // Assuming blackscreenimage.png does exist, it should also be valid as well.
            Assert.IsTrue(m_mediaItem.isFilePathValid);

            expectedClassification = MediaItem.Classification.image;
            actualClassification = m_mediaItem.GetClassification();
            Assert.AreEqual(expectedClassification, actualClassification);
        }


        [TestMethod()]
        public void GetTimeLimitCategoryTest()
        {
            MediaItem m_imageItem = new MediaItem(1, @"c:\temp\foo.jpg", "");
            String actual = m_imageItem.GetTimeLimitCategory();
            String expected = " @Playtime: 10 seconds";
            Assert.AreEqual(expected, actual);

            MediaItem m_videoItem = new MediaItem(2, @"c:\temp\foo.mov", "");
            actual = m_videoItem.GetTimeLimitCategory();
            expected = "";
            Assert.AreEqual(expected, actual);

            MediaItem m_audioItem = new MediaItem(3, @"c:\temp\foo.mp3", "");
            actual = m_audioItem.GetTimeLimitCategory();
            expected = "";
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void SetPlayTimeTest()
        {
            MediaItem m_imageItem = new MediaItem(1, @"c:\temp\foo.jpg", "");
            String expected = "10 seconds";
            m_imageItem.SetPlayTime(expected);
            String actual = m_imageItem.GetPlayTime();
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void VerifyFileNameTest()
        {
            bool doNOTIssueWarning = false;
            String correctFileName = @"c:\temp\foo.jpg";
            bool isCorrect = m_library.VerifyFileName(correctFileName, doNOTIssueWarning);
            Assert.IsTrue(isCorrect);


            String badFileName = @"c:\temp\foo#.jpg";
            bool isBad = m_library.VerifyFileName(badFileName, doNOTIssueWarning);
            Assert.IsFalse(isBad);
        }



        MediaItem m_mediaItem = new MediaItem(1, "", "");
        [TestMethod()]
        public void IsDirectoryTest()
        {
            String knownGoodPath = @"c:\Windows";
            bool isDirectory = m_mediaItem.IsDirectory(knownGoodPath);
            Assert.IsTrue(isDirectory);


            String knownBadPath = @"c:\windowsfoo";
            bool isNotDirectory = m_mediaItem.IsDirectory(knownBadPath);
            Assert.IsFalse(isNotDirectory);


            String knownFile = @"..\debug\Library.dll";
            isNotDirectory = m_mediaItem.IsDirectory(knownFile);
            Assert.IsFalse(isNotDirectory);

        }




        
        [TestMethod()]
        public void SetServerNameTest()
        {
            String nullString = "";
            Assert.IsFalse(m_communications.SetServerName(nullString));


            String expected = "testing";
            Assert.IsTrue(m_communications.SetServerName(expected));
            String actual = m_communications.GetServerName();
            Assert.AreEqual(expected, actual);
        }



        [TestMethod()]
        public void SetPortNumberTest()
        {
            String nullString = "";
            Assert.IsFalse(m_communications.SetPortNumber(nullString));


            String expected = "testing";
            Assert.IsTrue(m_communications.SetPortNumber(expected));
            String actual = m_communications.GetPortNumber();
            Assert.AreEqual(expected, actual);
        }
    }
}
