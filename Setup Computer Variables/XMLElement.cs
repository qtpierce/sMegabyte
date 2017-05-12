using System;
using System.Xml;


namespace SetupComputerVariables
{
    class XMLElement
    {
        public XMLElement( )
        {

        }


        public String GetElement( XmlDocument doc, String ElementName )
        {
            XmlNode myXNode = doc.SelectSingleNode( "//" + ElementName );
            String ReturnValue;
            ReturnValue = "";

            try
            {
                ReturnValue = myXNode.InnerText.ToString( );
            }
            catch( Exception e )
            {
                // do nothing.
            }
            return ReturnValue;
        }


        public void SetElement( XmlDocument doc, String ElementName, String ElementValue )
        {
            try
            {
                // Add an element.
                XmlElement newElem = doc.CreateElement( ElementName );
                newElem.InnerText = ElementValue;
                doc.DocumentElement.AppendChild( newElem );
            }
            catch( Exception e )
            {
                // do nothing.
            }
        }
    }
}
