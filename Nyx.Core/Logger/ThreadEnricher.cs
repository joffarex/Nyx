using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace Nyx.Core.Logger
{
    public class ThreadEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty("TheadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}