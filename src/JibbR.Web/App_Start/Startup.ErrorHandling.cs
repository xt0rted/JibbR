using System;
using System.Threading.Tasks;

namespace JibbR.Web
{
    public partial class Startup
    {
        private static void SetupErrorHandling()
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                try
                {
                    ReportError(args.Exception);
                }
                catch
                {
                }
                finally
                {
                    args.SetObserved();
                }
            };
        }

        private static void ReportError(Exception exception)
        {
            // ToDo: log the error somewhere
        }
    }
}