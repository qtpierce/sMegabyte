using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SubscriberWinForm
{
    class XMLDataStructure
    {


        public void LoadXMLVariablesFile(String xmlFilePath, ref String tempPath, ref String VLCPath, ref String myMediaPlayerPath, ref String serverName, ref String portNumber)
        {
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            // Save the document to a file. White space is
            // preserved (no white space).
            doc.PreserveWhitespace = true;
            doc.Load(xmlFilePath);

            tempPath = GetXML_TempPath(doc);
            VLCPath = GetXML_VLCPath(doc);
            myMediaPlayerPath = GetXML_MyMediaPlayerPath(doc);
            serverName = GetXML_ServerName(doc);
            portNumber = GetXML_PortNumber(doc);
        }



        public void SaveXMLVariablesFile(String xmlFilePath, String tempPath, String VLCPath, String myMediaPlayerPath, String serverName, String portNumber)
        {
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\"?>\n<VideoSyncVariables></VideoSyncVariables>");

            SetXML_TempPath(doc, @tempPath);
            SetXML_VLCPath(doc, @VLCPath);
            SetXML_MyMediaPlayerPath(doc, @myMediaPlayerPath);
            SetXML_ServerName(doc, serverName);
            SetXML_PortNumber(doc, portNumber);

            doc.Save(xmlFilePath);
        }


        XMLElement XEtempPath = new XMLElement();
        private String GetXML_TempPath (XmlDocument doc)
        {
            String tempPath = XEtempPath.GetElement(doc, "m_tempPath");
            return @tempPath;
        }


        private void SetXML_TempPath(XmlDocument doc, String tempPath)
        {
            XEtempPath.SetElement(doc, "m_tempPath", @tempPath);
        }


        XMLElement XEVLCPath = new XMLElement();
        private String GetXML_VLCPath(XmlDocument doc)
        {
            String VLCPath = XEVLCPath.GetElement(doc, "m_VLCPath");
            return @VLCPath;
        }


        private void SetXML_VLCPath(XmlDocument doc, String VLCPath)
        {
            XEVLCPath.SetElement(doc, "m_VLCPath", @VLCPath);
        }


        XMLElement XEMyMediaPlayerPath = new XMLElement();
        private String GetXML_MyMediaPlayerPath(XmlDocument doc)
        {
            String MyMediaPlayerPath = XEMyMediaPlayerPath.GetElement(doc, "m_MyMediaPlayerPath");
            return @MyMediaPlayerPath;
        }


        private void SetXML_MyMediaPlayerPath(XmlDocument doc, String myMediaPlayerPath)
        {
            XEMyMediaPlayerPath.SetElement(doc, "m_MyMediaPlayerPath", @myMediaPlayerPath);
        }


        XMLElement XEServerName = new XMLElement();
        private String GetXML_ServerName(XmlDocument doc)
        {
            String serverName = XEMyMediaPlayerPath.GetElement(doc, "m_serverName");
            return serverName;
        }


        private void SetXML_ServerName(XmlDocument doc, String serverName)
        {
            XEServerName.SetElement(doc, "m_serverName", serverName);
        }


        XMLElement XEPortNumber = new XMLElement();
        private String GetXML_PortNumber(XmlDocument doc)
        {
            String portNumber = XEMyMediaPlayerPath.GetElement(doc, "m_portNumber");
            return portNumber;
        }


        private void SetXML_PortNumber(XmlDocument doc, String portNumber)
        {
            XEPortNumber.SetElement(doc, "m_portNumber", portNumber);
        }
    }
}
