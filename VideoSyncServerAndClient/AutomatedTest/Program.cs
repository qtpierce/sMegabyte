using System;



namespace AutomatedTest
{
    class Program
    {

        

        static void Main(string[] args)
        {
            Console.WriteLine("AutomatedTest meant for the VideoSync SW.");
            

            ic_BasicTest basicTest = new ic_BasicTest();
            PerformTest( basicTest.BasicTest() , "basicTest" );
            

            ic_DifferentMediaTest differentMediaTest = new ic_DifferentMediaTest();
            PerformTest( differentMediaTest.DifferentMediaTest() , "DifferentMediaTest" );


            ic_ClientConnections clientConnections = new ic_ClientConnections();
            PerformTest( clientConnections.ClientConnections() , "ClientConnections" );
            

            // Done and exiting.
            Console.ReadLine();
        }


        static private void PerformTest (bool results, String testName)
        {
            if (results)
            {
                Console.WriteLine("-P-  Test {0} PASSED.", testName);
            }
            else
            {
                Console.WriteLine("-E-  Test {0} FAILED.", testName);
            }
        }
    }
}

