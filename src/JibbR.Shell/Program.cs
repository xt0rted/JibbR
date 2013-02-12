using System;
using System.Threading;
using System.Threading.Tasks;

namespace JibbR.Shell
{
    class Program
    {
        private static readonly TimeSpan Pulse = TimeSpan.FromMinutes(13);

        static void Main(string[] args)
        {
            var host = System.Configuration.ConfigurationManager.AppSettings["jibbr:host"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["jibbr:username"];
            var password = System.Configuration.ConfigurationManager.AppSettings["jibbr:password"];

            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();

            var robot = bootstrapper.CreateRobot();

            Timer heartBeat = null;
            try
            {
                robot.SetupClient(new Uri(host));

                robot.Connect(userName, password);

                // jabbr sets you idle after 15+ minutes so we need to run faster than that
                heartBeat = new Timer(_ => robot.HeartBeat(), null, Pulse, Pulse);

                Task.Factory.StartNew(() =>
                {
                    SpinWait.SpinUntil(() =>
                    {
                        return Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape;
                    });
                }).Wait();

                heartBeat.Dispose();
                robot.Disconnect();
            }
            catch (Exception ex)
            {
                if (heartBeat != null)
                {
                    heartBeat.Dispose();
                }

                Console.Error.WriteLine("error while running");

                while (ex != null)
                {
                    Console.Error.WriteLine(ex.ToString());
                    ex = ex.InnerException;
                }

                Console.ReadLine();
            }

            Console.WriteLine("Robot disconnected");
        }
    }
}