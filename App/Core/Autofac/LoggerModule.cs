using Autofac;
using Microsoft.Extensions.Logging;

namespace Core.Autofac
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var f = new LoggerFactory()
                .AddDebug();
            
            builder.RegisterInstance(f.CreateLogger("default"))
                .As<ILogger>()
                .SingleInstance();
        }
    }
}