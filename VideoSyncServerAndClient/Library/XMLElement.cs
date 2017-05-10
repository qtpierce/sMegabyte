using System;
using System.Xml;


namespace Library
{
    public class XMLElement
    {
        public XMLElement()
        {

        }


        // Regression test:  VideoSyncClient1Tests.cs::SetElementTest()
        public String GetElement(XmlDocument doc, String ElementName)
        {
            XmlNode myXNode = doc.SelectSingleNode("//" + ElementName);
            String ReturnValue;
            ReturnValue = "";

            try
            {
                ReturnValue = myXNode.InnerText.ToString();
            }
            catch (Exception e)
            {
                String exception = e.ToString();
            }
            return ReturnValue;
        }


        // Regression test:  VideoSyncClient1Tests.cs::SetElementTest()
        public bool SetElement(XmlDocument doc, String ElementName, String ElementValue)
        {
            try
            {
                // Add an element.
                XmlElement newElem = doc.CreateElement(ElementName);
                newElem.InnerText = ElementValue;
                doc.DocumentElement.AppendChild(newElem);
                return true;
            }
            catch (Exception e)
            {
                String exception = e.ToString();
                return false;
            }
        }
    }
}
