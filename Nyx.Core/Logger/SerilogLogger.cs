using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Nyx.Core.Logger
{
    public static class SerilogLogger
    {
        public static readonly Serilog.Core.Logger Logger = new LoggerConfiguration().MinimumLevel.Debug().MinimumLevel
            .Override("Microsoft", LogEventLevel.Information)
            .Enrich.With(new ThreadEnricher())
            .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
            .CreateLogger();

        public static ILoggerFactory Factory { get; } = new LoggerFactory().AddSerilog(Logger);
    }
}