using Serilog;
using Serilog.Events;

namespace Manager;

public static class AppLogger
{
  public static void Init()
  {
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
            .WriteTo.File("logs/verbose.txt"))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
            .WriteTo.File("logs/debug.txt"))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
            .WriteTo.File("logs/info.txt"))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
            .WriteTo.File("logs/warning.txt"))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
            .WriteTo.File("logs/error.txt"))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
            .WriteTo.File("logs/fatal.txt"))
        .CreateLogger();
  }

  public static void Shutdown()
  {
    Log.CloseAndFlush();
  }
}