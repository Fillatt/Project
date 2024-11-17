using Stage1;
using Serilog;

int N = 0;
int L = 0;

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

// Настройка логгеров для статических классов
Configuration.Logger = debugLog;
Formulas.Logger = debugLog;
Extensions.Logger = debugLog;

// Конфигурация (чтение N и L)
infoLog.Information("Configuration: Start");
try { Configuration.StartConfiguration(ref N, ref L); }
catch (Exception ex) { errorLog.Error(ex.Message); }
infoLog.Information("Configuration: Done");

// Инициализация oddNumbers
infoLog.Information("oddNumbers: Start init");
int[] oddNumbers = { 5, 7, 9, 11, 13, 15, 17, 19 };
infoLog.Information("oddNumbers: Init done");

// Инициализация randomValues случайными double-числами в диапазоне
infoLog.Information("randomValues: Start init");
double[] randomValues = new double[13];
for (int i = 0; i < randomValues.Length; i++) randomValues[i] = new Random().NextDouble(-12.0, 15.0);
infoLog.Information("randomValues: Init done");

// Инициализация двумерного массива k
infoLog.Information("k: Start init");
double?[,] k = new double?[8, 13];
for (int i = 0; i < k.GetLength(0); i++)
{
    try
    {
        switch (oddNumbers[i])
        {
            case 9:
                Formulas.First(randomValues, k, i);
                continue;
            case 5 or 7 or 11 or 15:
                Formulas.Second(randomValues, k, i);
                continue;
            default:
                Formulas.Third(randomValues, k, i);
                continue;
        }
    }
    catch(DivideByZeroException ex)
    {
        errorLog.Error("{TargetSite}: {Message}; StackTrace: {StackTrace}", ex.TargetSite, ex.Message, ex.StackTrace);
    }
}
infoLog.Information("k: Init done");

// Вычесления элемента, равного сумме:
// Минимального элемента k[i], где i = (N % 8)
// Среднего значения элемента k[j], где j = (L % 13)
infoLog.Information("Sum: Start init");
double? min = k.MinValueInRow(N % 8);
double? average = k.AverageValueInColumn(L % 13);
double? sum = min + average;
infoLog.Information("Sum: Init done; Value: {Value}", sum);

// Вывод результата на консоль
Console.WriteLine($"Sum: {sum:f4}");