using System;
using System.Configuration;
using System.Net;

using Microsoft.Owin.Hosting;

namespace JibbR.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = ConfigurationManager.AppSettings["jibbr:host"];
            var userName = ConfigurationManager.AppSettings["jibbr:username"];
            var password = ConfigurationManager.AppSettings["jibbr:password"];

            ServicePointManager.DefaultConnectionLimit = 100;

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var robot = container.GetInstance<IRobot>();

            try
            {
                robot.SetupClient(host);

                robot.Connect(userName, password);

                var url = new UriBuilder
                {
                    Scheme = "http",
                    Host = "localhost",
                    Port = 1326
                };

                using (WebApplication.Start(url.ToString(), new Startup(container).Configuration))
                {
                    Console.WriteLine("Server running on {0}", url);
                    Console.ReadLine();
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