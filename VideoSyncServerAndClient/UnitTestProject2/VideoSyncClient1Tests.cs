using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using Library;


namespace VideoSyncClient.Tests
{
    [TestClass()]
    public class VideoSyncClient1Tests
    {
        [TestMethod()]
        public void SetElementTest()
        {
            XmlDocument doc = new XmlDocument();
            XMLElement XEtest = new XMLElement();
            doc.LoadXml("<?xml version=\"1.0\"?>\n<testingXML></testingXML>");

            String ElementName = "testing";
            String expected = "foo testing";

            XEtest.SetElement(doc, ElementName, expected);
            String actual = XEtest.GetElement(doc, ElementName);
            Assert.AreEqual(expected, actual);
        }



        Globals m_globals = new Globals();
        Communications m_communications = new Communications();


        [TestMethod()]
        public void Set_tempPathTest()
        {
            // Test that the default value is "".
            String expectedPath = "";
            String actualPath = m_globals.Get_tempPath();
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolFalse = m_globals.isTempPathSet;
            Assert.IsFalse(isIsSetBoolFalse);


            // Assuming c:\temp does exist, test a known good path.
            expectedPath = @"c:\temp";
            bool didSetWork = m_globals.Set_tempPath(expectedPath);
            actualPath = m_globals.Get_tempPath();
            Assert.IsTrue(didSetWork);
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolTrue = m_globals.isTempPathSet;
            Assert.IsTrue(isIsSetBoolTrue);


            // Assuming c:\tempfoo does NOT exist, test a known bad path.
            String fakePath = @"c:\tempfoo";
            bool didSetReject = m_globals.Set_tempPath(fakePath);
            actualPath = m_globals.Get_tempPath();
            Assert.IsFalse(didSetReject);
            Assert.AreEqual(expectedPath, actualPath);  // Because of a bad path, this should be an old value set 12 lines prior.
            isIsSetBoolFalse = m_globals.isTempPathSet;
            Assert.IsFalse(isIsSetBoolFalse);
        }


        [TestMethod()]
        public void Set_VLCPathTest()
        {
            // Test that the default value is "".
            String expectedPath = "";
            String actualPath = m_globals.Get_VLCPath();
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolFalse = m_globals.isVLCPathSet;
            Assert.IsFalse(isIsSetBoolFalse);

            // Assuming c:\temp does exist, test a known good path.
            expectedPath = @"c:\temp";
            bool didSetWork = m_globals.Set_VLCPath(expectedPath);
            actualPath = m_globals.Get_VLCPath();
            Assert.IsTrue(didSetWork);
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolTrue = m_globals.isVLCPathSet;
            Assert.IsTrue(isIsSetBoolTrue);


            // Assuming c:\tempfoo does NOT exist, test a known bad path.
            String fakePath = @"c:\tempfoo";
            bool didSetReject = m_globals.Set_VLCPath(fakePath);
            actualPath = m_globals.Get_VLCPath();
            Assert.IsFalse(didSetReject);
            Assert.AreEqual(expectedPath, actualPath);  // Because of a bad path, this should be an old value set 12 lines prior.
            isIsSetBoolFalse = m_globals.isVLCPathSet;
            Assert.IsFalse(isIsSetBoolFalse);
        }


        [TestMethod()]
        public void Set_MyMediaPlayerPathTest()
        {
            // Test that the default value is "".
            String expectedPath = "";
            String actualPath = m_globals.Get_MyMediaPlayerPath();
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolFalse = m_globals.isMyMediaPlayerPathSet;
            Assert.IsFalse(isIsSetBoolFalse);


            // Assuming c:\temp does exist, test a known good path.
            expectedPath = @"c:\temp";
            bool didSetWork = m_globals.Set_MyMediaPlayerPath(expectedPath);
            actualPath = m_globals.Get_MyMediaPlayerPath();
            Assert.IsTrue(didSetWork);
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolTrue = m_globals.isMyMediaPlayerPathSet;
            Assert.IsTrue(isIsSetBoolTrue);


            // Assuming c:\tempfoo does NOT exist, test a known bad path.
            String fakePath = @"c:\tempfoo";
            bool didSetReject = m_globals.Set_MyMediaPlayerPath(fakePath);
            actualPath = m_globals.Get_MyMediaPlayerPath();
            Assert.IsFalse(didSetReject);
            Assert.AreEqual(expectedPath, actualPath);  // Because of a bad path, this should be an old value set 12 lines prior.
            isIsSetBoolFalse = m_globals.isMyMediaPlayerPathSet;
            Assert.IsFalse(isIsSetBoolFalse);
        }


        [TestMethod()]
        public void SetpathToVariablesFileTest()
        {
            // Test that the default value is "".
            String expectedPath = "";
            String actualPath = m_globals.GetpathToVariablesFile();
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolFalse = m_globals.isPathToVariablesFileSet;
            Assert.IsFalse(isIsSetBoolFalse);


            // Assuming c:\temp does exist, test a known good path.
            expectedPath = @"c:\temp";
            bool didSetWork = m_globals.SetpathToVariablesFile(expectedPath);
            actualPath = m_globals.GetpathToVariablesFile();
            Assert.IsTrue(didSetWork);
            Assert.AreEqual(expectedPath, actualPath);
            bool isIsSetBoolTrue = m_globals.isPathToVariablesFileSet;
            Assert.IsTrue(isIsSetBoolTrue);


            // Assuming c:\tempfoo does NOT exist, test a known bad path.
            String fakePath = @"c:\tempfoo";
            didSetWork = m_globals.SetpathToVariablesFile(fakePath);
            actualPath = m_globals.GetpathToVariablesFile();
            Assert.IsTrue(didSetWork);
            Assert.AreEqual(fakePath, actualPath);
            isIsSetBoolTrue = m_globals.isPathToVariablesFileSet;
            Assert.IsTrue(isIsSetBoolTrue);
        }


        [TestMethod()]
        public void SaveXMLVariablesFileTest()
        {
            String expected = @"c:\temp";
            String pathToVariablesFile = @"c:\temp\testingsavefile.xml";
            m_globals.SetpathToVariablesFile(pathToVariablesFile);
            m_communications.SetPortNumber(expected);
            m_communications.SetServerName(expected);
            m_globals.Set_MyMediaPlayerPath(expected);
            m_globals.Set_tempPath(expected);
            m_globals.Set_VLCPath(expected);

            m_globals.SaveXMLVariablesFile();

            m_communications.SetPortNumber(null);
            m_communications.SetServerName(null);
            m_globals.Set_MyMediaPlayerPath(null);
            m_globals.Set_tempPath(null);
            m_globals.Set_VLCPath(null);

            m_globals.LoadXMLVariablesFile();
            String actual = m_globals.GetpathToVariablesFile();
            Assert.AreEqual(pathToVariablesFile, actual);

            actual = m_communications.GetPortNumber();
            Assert.AreEqual(expected, actual);

            actual = m_communications.GetServerName();
            Assert.AreEqual(expected, actual);

            actual = m_globals.Get_MyMediaPlayerPath();
            Assert.AreEqual(expected, actual);

            actual = m_globals.Get_tempPath();
            Assert.AreEqual(expected, actual);

            actual = m_globals.Get_VLCPath();
            Assert.AreEqual(expected, actual);
        }


    }
}


