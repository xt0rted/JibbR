using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Owin.Hosting;

namespace JibbR.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = System.Configuration.ConfigurationManager.AppSettings["jibbr:host"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["jibbr:username"];
            var password = System.Configuration.ConfigurationManager.AppSettings["jibbr:password"];

            ServicePointManager.DefaultConnectionLimit = 100;

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var robot = container.GetInstance<IRobot>();

            try
            {
                robot.SetupClient(new Uri(host));

                robot.Connect(userName, password);

                var url = new UriBuilder
                {
                    Scheme = "http",
                    Host = "localhost",
                    Port = 1326
                };

                using (WebApplication.Start<Startup>(url.ToString()))
                {
                    Console.WriteLine("Server running on {0}", url);
                    Task.Factory.StartNew(() => SpinWait.SpinUntil(() => Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape)).Wait();
                }

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

                Console.ReadLine();
            }

            Console.WriteLine("Robot disconnected");
        }
    }
}