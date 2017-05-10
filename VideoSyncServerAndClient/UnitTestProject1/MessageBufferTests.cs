using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Library.Tests
{
    [TestClass()]
    public class MessageBufferTests
    {
        MessageBuffer m_messageBuffer = new MessageBuffer();

        [TestMethod()]
        public void SetTextTest()
        {
            String expectedText = "set text";
            m_messageBuffer.SetText(expectedText);
            String actualText = m_messageBuffer.GetText();
            Assert.AreEqual(expectedText, actualText);


            expectedText = "reset text";
            m_messageBuffer.SetText(expectedText);
            actualText = m_messageBuffer.GetText();
            Assert.AreEqual(expectedText, actualText);
        }

        [TestMethod()]
        public void AppendTextTest()
        {
            String expectedText = "reset text";
            m_messageBuffer.SetText(expectedText);

            String appendedText = ", appended";
            expectedText += appendedText;
            m_messageBuffer.AppendText(appendedText);
            String actualText = m_messageBuffer.GetText();
            Assert.AreEqual(expectedText, actualText);
        }
    }
}