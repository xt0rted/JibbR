using System;
using System.Threading;
using System.Threading.Tasks;

namespace JibbR.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = System.Configuration.ConfigurationManager.AppSettings["jibbr:host"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["jibbr:username"];
            var password = System.Configuration.ConfigurationManager.AppSettings["jibbr:password"];

            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();

            var rooms = new[]
            {
                "development"
            };

            var robot = bootstrapper.CreateRobot();

            try
            {
                robot.SetupClient(new Uri(host));

                robot.Connect(userName, password);

                robot.JoinRooms(rooms);

                Task.Factory.StartNew(() =>
                {
                    SpinWait.SpinUntil(() =>
                    {
                        return Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape;
                    });
                }).Wait();

                robot.Disconnect();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("error while running");

                while (ex != null)
                {
                    Console.Error.WriteLine(ex.ToString());
                    ex = ex.InnerException;
                }
            }

            Console.WriteLine("Robot disconnected");
        }
    }
}