using Serilog.Core;

namespace ConsoleApp1;

/// <summary>
/// Статический класс для расширяющих методов
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Статическое свойство для взаимодействия с логгером
    /// </summary>
    public static Logger Logger { get; set; }

    public static Logger LoggerError {  get; set; }

    /// <summary>
    /// Расширяющий метод для Random; генерирует случайные double-числа в указанном диапазоне
    /// </summary>
    /// <param name="random"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static double NextDouble(this Random random, double minValue, double maxValue)
    {
        Logger.Debug("Extensions.NextDouble: Start; MinValue: {MinValue}; MaxValue: {MaxValue}", minValue, maxValue);
        double result = random.NextDouble() * (maxValue - minValue) + minValue;
        Logger.Debug("Extensions.NextDouble: Done; Value: {Value}", result);
        return result;
    }

    /// <summary>
    /// Расширяющий метод для двумерного массива типа double; возвращает минимальный элемент
    /// указанной строки массива
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public static double? MinValueInRow(this double?[,] arr, int row)
    {
        Logger.Debug("Extensions.MinInRow: Start; Row: {Row}", row);
        double? min = arr[row, 0];
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[row, j] != null)
            {
                if (arr[row, j] < min) min = arr[row, j];
            }
        }
        Logger.Debug("Extensions.MinInRow: Done; Value: {Value}", min);
        return min;
    }

    /// <summary>
    /// Расширяющий метод для двумерного массива типа double; возвращает средний элемент
    /// указанного столбца массива
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public static double? AverageValueInColumn(this double?[,] arr, int column)
    {
        Logger.Debug("Extensions.AverageValueInColumn: Start; Column: {Column}", column);
        int number = 0;
        double? sum = 0;
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            if (arr[i, column] != null)
            {
                sum += arr[i, column];
                number++;
            }
        }
        double? numberDouble = (double?)number;
        Logger.Debug("(double?)int: Done; Value: {Value}", numberDouble);
        double? result = sum / numberDouble;
        Logger.Debug("Extensions.AverageValueInColumn: Done; Value: {Value}", result);
        return result;
    }

    /// <summary>
    /// Расширяющий метод для массива типа double; инициализирует массив случайными
    /// double-числами в указанном диапазоне
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    public static void Init(this double[] arr, double minValue, double maxValue)
    {
        Logger.Debug("Extensions.Init: Start; MinValue: {MinValue}; MaxValue: {MaxValue}", minValue, maxValue);
        for (int i = 0; i < arr.Length; i++) arr[i] = new Random().NextDouble(minValue, maxValue);
        Logger.Debug("Extensions.Init: Done");
    }

    /// <summary>
    /// Расширяющий метод для двумерного массива типа double; инициализирует массив
    /// вычесленными по формулам значениями
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="randomValues"></param>
    /// <param name="oddNumbers"></param>
    public static void Init(this double?[,] arr, double[] randomValues, int[] oddNumbers)
    {
        Logger.Debug("Extensions.Init: Start");
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            try
            {
                switch (oddNumbers[i])
                {
                    case 9:
                        Formulas.First(randomValues, arr, i);
                        continue;
                    case 5 or 7 or 11 or 15:
                        Formulas.Second(randomValues, arr, i);
                        continue;
                    default:
                        Formulas.Third(randomValues, arr, i);
                        continue;
                }
            }
            catch(DivideByZeroException ex)
            {
                LoggerError.Error("{TargetSite}: {Message}; StackTrace: {StackTrace}", ex.TargetSite, ex.Message, ex.StackTrace);
            }
        }
        Logger.Debug("Extensions.Init: Done");
    }
}