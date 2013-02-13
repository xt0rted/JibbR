using JibbR.Annotations;

using Nancy;
using Nancy.Conventions;

namespace JibbR.Web
{
    [UsedImplicitly]
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddFile("/robots.txt", "/robots.txt")
                );
        }
    }
}