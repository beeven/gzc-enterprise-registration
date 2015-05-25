using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;

//#if !DNXCORE50
//using Serilog;
//using Serilog.Sinks.IOFile;
//using Serilog.Formatting.Raw;
//using Serilog.Sinks.RollingFile;
//using Serilog.Formatting.Json;
//#endif


namespace EnterpriseRegistration.Receipt.Logging
{
    public class Logger:EnterpriseRegistration.Receipt.Logging.ILogger
    {

        private readonly Microsoft.Framework.Logging.ILogger _logger;

        public Logger()
        {
            var factory = new Microsoft.Framework.Logging.LoggerFactory();

            _logger = factory.CreateLogger("EnterpriseRegistration");
//#if !DNXCORE50
//            factory.AddSerilog(new Serilog.LoggerConfiguration()
//                .Enrich.WithMachineName()
//                .Enrich.WithProcessId()
//                .Enrich.WithThreadId()
//                .MinimumLevel.Debug()
//                .WriteTo.RollingFile("file-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level}:{EventId} [{SourceContext}] {Message}{NewLine}{Exception}")
//                .WriteTo.Sink(new RollingFileSink("file-{Date}.json", new JsonFormatter(), null, null))
//                .WriteTo.Sink(new FileSink("dump.txt", new RawFormatter(), null)));
//#endif
            factory.AddConsole();
        }
        public void Log(String content)
        {
            _logger.LogInformation(content);
        }
    }
}
