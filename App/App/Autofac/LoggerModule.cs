using Autofac;
using Microsoft.Extensions.Logging;

namespace App.Autofac
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