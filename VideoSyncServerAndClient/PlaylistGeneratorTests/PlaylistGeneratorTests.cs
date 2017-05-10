using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PlaylistGenerator.Tests
{
    [TestClass()]
    public class PlaylistGeneratorTests
    {
        static String[] args = { "" };
        PlaylistGenerator m_playlistGenerator = new PlaylistGenerator(args);


        [TestMethod()]
        public void AppendItemToPlaylistTest()
        {
            String actual = "";
            String appendFoo = "Foo";
            String expected = appendFoo + Environment.NewLine; 
            m_playlistGenerator.AppendItemToPlaylist(appendFoo, ref actual);
            Assert.AreEqual(expected, actual);


            String appendJPG = "photo.jpg";
            expected += appendJPG + Environment.NewLine;
            m_playlistGenerator.AppendItemToPlaylist(appendJPG, ref actual);
            bool hasPlaytime = actual.Contains("Playtime");
            Assert.IsTrue(hasPlaytime);

            bool has10 = actual.Contains("10");
            Assert.IsTrue(has10);


        }
    }
}