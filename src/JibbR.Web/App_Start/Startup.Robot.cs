using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

using JibbR.Annotations;

using StructureMap;

[assembly: WebActivator.PostApplicationStartMethod(typeof(JibbR.Web.Startup), "StartRobot")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(JibbR.Web.Startup), "StopRobot")]

namespace JibbR.Web
{
    public partial class Startup
    {
        private static readonly TimeSpan Pulse = TimeSpan.FromMinutes(13);

        private static IRobot _robot;

        private static Task _robotTask;
        private static Timer _heartBeat;

        [UsedImplicitly]
        public static void StartRobot()
        {
            var host = ConfigurationManager.AppSettings["jibbr:host"];
            var userName = ConfigurationManager.AppSettings["jibbr:username"];
            var password = ConfigurationManager.AppSettings["jibbr:password"];

            _robot = CreateRobot();

            _robotTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    _robot.SetupClient(new Uri(host));

                    _robot.Connect(userName, password);

                    // jabbr sets you idle after 15+ minutes so we need to run faster than that
                    _heartBeat = new Timer(_ => _robot.HeartBeat(), null, Pulse, Pulse);
                }
                catch
                {
                    _heartBeat.DisposeSafely();

                    _robot.Disconnect();
                }
            });
        }

        [UsedImplicitly]
        public static void StopRobot()
        {
            _heartBeat.DisposeSafely();

            if (_robot != null)
            {
                _robot.Disconnect();
            }

            _robotTask.DisposeSafely();
        }

        private static IRobot CreateRobot()
        {
            var container = ObjectFactory.Container;
            return container.GetInstance<IRobot>();
        }
    }
}