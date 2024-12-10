using ConsoleApp1;
using Figure;
using Serilog;

int N = 0;
int L = 0;
int sleep = 0;

int[] oddNumbers = new int[8];

// Логгер для уровня Information
var infoLog = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("InformationLog.json")
    .CreateLogger();

// Логгер для уровня Debug
var debugLog = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("DebugLog.json")
    .CreateLogger();

// Логгер для уровня Error
var errorLog = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.Console()
    .WriteTo.File("ErrorLog.json")
    .CreateLogger();

// Настройка логгеров для классов
Configuration.Logger = debugLog;
Formulas.Logger = debugLog;
Extensions.Logger = debugLog;
Extensions.LoggerError = errorLog;
AbstractFigure.Logger = debugLog;
Calculations.InfoLog = infoLog;
Calculations.ErrorLog = errorLog;

Calculations.StartConfiguration(ref N, ref L, ref sleep);
oddNumbers = Calculations.OddNumbersInit();

while (true)
{
    List<double> speeds = [];
    List<double?> amounts = Calculations.CountAmounts(oddNumbers, N, L);
    foreach (double? amount in amounts) Console.WriteLine(amount);
    speeds = Calculations.SpeedsInit(amounts);
    List<IFigure> figures = [];
    figures = Calculations.FiguresInit(speeds);
    foreach (IFigure figure in figures)
    {
        figure.DoTheTask();
        Console.WriteLine(figure.Message);
        Thread.Sleep(sleep);
    }
}
