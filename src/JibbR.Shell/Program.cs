using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using StructureMap.Pipeline;

namespace JibbR.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostUrl = System.Configuration.ConfigurationManager.AppSettings["jibbr:host"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["jibbr:username"];
            var password = System.Configuration.ConfigurationManager.AppSettings["jibbr:password"];

            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();

            var container = bootstrapper.Container;

            var rooms = new[]
            {
                "development"
            };

            var robot = container.GetInstance<IRobot>(new ExplicitArguments(new Dictionary<string, object>
            {
                { "host", new Uri(hostUrl) },
            }));

            try
            {
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