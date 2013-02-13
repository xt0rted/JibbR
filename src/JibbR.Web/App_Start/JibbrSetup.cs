using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

using JibbR.Annotations;

[assembly: WebActivator.PostApplicationStartMethod(typeof(JibbR.Web.App_Start.JibbrSetup), "Start")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(JibbR.Web.App_Start.JibbrSetup), "Stop")]

namespace JibbR.Web.App_Start
{
    public static class JibbrSetup
    {
        private static readonly TimeSpan Pulse = TimeSpan.FromMinutes(13);

        public static IRobot Robot;

        private static Task _robotTask;
        private static Timer _heartBeat;

        [UsedImplicitly]
        public static void Start()
        {
            var host = ConfigurationManager.AppSettings["jibbr:host"];
            var userName = ConfigurationManager.AppSettings["jibbr:username"];
            var password = ConfigurationManager.AppSettings["jibbr:password"];

            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();

            Robot = bootstrapper.CreateRobot();

            _robotTask =
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Robot.SetupClient(new Uri(host));

                        Robot.Connect(userName, password);

                        // jabbr sets you idle after 15+ minutes so we need to run faster than that
                        _heartBeat = new Timer(_ => Robot.HeartBeat(), null, Pulse, Pulse);
                    }
                    catch
                    {
                        if (_heartBeat != null)
                        {
                            _heartBeat.Dispose();
                        }

                        Robot.Disconnect();
                    }
                });
        }

        [UsedImplicitly]
        public static void Stop()
        {
            _heartBeat.DisposeSafely();

            if (Robot != null)
            {
                Robot.Disconnect();
            }

            _robotTask.DisposeSafely();
        }
    }
}