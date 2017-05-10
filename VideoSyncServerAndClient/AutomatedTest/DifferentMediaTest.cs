using System;


namespace AutomatedTest
{
    class ic_DifferentMediaTest : ic_BasicTest
    {
        ic_BasicTest m_BasicTest = new ic_BasicTest();

 
        public bool DifferentMediaTest()
        {
            String files = "\"c:\\temp\\testcase objects\\0) Unreal 2004 patching instructions.PNG\",\"c:\\temp\\testcase objects\\1 kHz.wav\",\"c:\\temp\\testcase objects\\A Cat's Parade.mov\",\"c:\\temp\\testcase objects\\Anitek_-_01_-_Dark_City_feat_Sara_Grey.mp3\",\"c:\\temp\\testcase objects\\Anitek_-_02_-_So_Far.ogg\",\"c:\\temp\\testcase objects\\annoyed-cat.jpg\",\"c:\\temp\\testcase objects\\cat looking into camera.mp4\",\"c:\\temp\\testcase objects\\Cat.gif\",\"c:\\temp\\testcase objects\\Cat.m4v\",\"c:\\temp\\testcase objects\\cerberusQP backdrop.bmp\",\"c:\\temp\\testcase objects\\CountDown.ogv\"";

            return m_BasicTest.BasicTest(files);
        }
    }
}
