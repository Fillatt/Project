using Serilog;
using System.Threading;
using ConsoleApp1;
using Figure;

int N = 0;
int L = 0;
int sleep = 0;

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
// Конфигурация (чтение N и L)
infoLog.Information("Configuration: Start");
try { Configuration.StartConfiguration(ref N, ref L, ref sleep); }
catch (Exception ex) { errorLog.Error(ex.Message); }
infoLog.Information("Configuration: Done");

// Инициализация oddNumbers
infoLog.Information("oddNumbers: Start init");
int[] oddNumbers = { 5, 7, 9, 11, 13, 15, 17, 19 };
infoLog.Information("oddNumbers: Init done");

while (true)
{
    // Список для хранения расчитанных скоростей
    List<double> speeds = new List<double>();

    for (int i = 0; i < 4; i++)
    {
        // Инициализация randomValues случайными double-числами в диапазоне -12.0 - 15.0
        infoLog.Information("randomValues: Start init");
        double[] randomValues = new double[13];
        randomValues.Init(-12.0, 15.0);
        infoLog.Information("randomValues: Init done");

        // Инициализация двумерного массива k
        infoLog.Information("k: Start init");
        double?[,] k = new double?[8, 13];
        k.Init(randomValues, oddNumbers);
        infoLog.Information("k: Init done");

        // Вычесления элемента, равного сумме:
        // Минимального элемента k[i], где i = (N % 8)
        // Среднего значения элемента k[j], где j = (L % 13)
        infoLog.Information("Sum: Start init");
        double? min = k.MinValueInRow(N % 8);
        double? average = k.AverageValueInColumn(L % 13);
        double? sum = min + average;
        infoLog.Information("Sum: Init done; Value: {Value}", sum);

        // Добавление значения в speeds
        double result;
        if (sum != null) result = Math.Abs((double)sum);
        else result = 0;
        speeds.Add(result);
        infoLog.Information("speeds.Add: Done; Result: {Result}", result);

        // Вывод результата на консоль
        Console.WriteLine($"Sum: {sum:f4}");
    }

    // Создание объектов, представляющих фигуры
    Circle circle = new Circle(speeds.Max());
    speeds.Remove(speeds.Max());
    infoLog.Information("Circle: Created; Value: {@Circle}", circle);
    Triangle triangle = new Triangle(speeds[0]);
    infoLog.Information("Triangle: Created; Value: {@Triangle}", triangle);
    Square square = new Square(speeds[1]);
    infoLog.Information("Square: Created; Value: {@Square}", square);
    Rectangle rectangle = new Rectangle(speeds[2]);
    infoLog.Information("Rectangle: Created; Value: {@Rectangle}", rectangle);

    // Создание списка, хранящего фигуры
    List<IFigure> figures = new List<IFigure>()
    {
        circle,
        triangle,
        square,
        rectangle
    };
    infoLog.Information("Figures list: Created");


    // Выполнение фигурами своих задач
    infoLog.Information("Tasks: Start");
    foreach (IFigure figure in figures)
    {
        figure.DoTheTask();
        Thread.Sleep(sleep);
    }
    infoLog.Information("Tasks: Done");
}

