using ConsoleApp;
using Figure;
using Serilog;
using Serilog.Events;

ConfigurationService configuration = new("appsettings.json");

int N = configuration.GetN();
int L = configuration.GetL();
int sleep = configuration.GetSleep();

int[] oddNumbers = new int[8];

// Настройка логгера
Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Debug()
                   .WriteTo.Console()
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info.log"))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug.log"))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning.log"))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error.log"))
                   .CreateLogger();

oddNumbers = Calculations.OddNumbersInit();

while (true)
{
    List<double> speeds = [];
    List<double> amounts = Calculations.CountAmounts(oddNumbers, N, L);
    foreach (double amount in amounts) Console.WriteLine(amount);
    speeds = Calculations.SpeedsInit(amounts);
    List<IFigure> figures = [];
    figures = Calculations.FiguresInit(speeds);
    foreach (IFigure figure in figures)
    {
        figure.StartTheMission();
        Console.WriteLine(figure.Message);
        Thread.Sleep(sleep);
        figure.StopTheMission();
        Console.WriteLine(figure.Message);
    }
}
