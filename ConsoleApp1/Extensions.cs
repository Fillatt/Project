using Serilog.Core;

namespace Stage1;

/// <summary>
/// Статический класс для расширяющих методов
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Статическое свойство для взаимодействия с логгером
    /// </summary>
    public static Logger Logger { get; set; }

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
        Logger.Debug("MinInRow: Start; Row: {Row}", row);
        double? min = arr[row, 0];
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            if (arr[row, j] != null)
            {
                if (arr[row, j] < min) min = arr[row, j];
            }
        }
        Logger.Debug("MinInRow: Done; Value: {Value}", min);
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
        Logger.Debug("AverageValueInColumn: Start; Column: {Column}", column);
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
        double? numberDouble = (double?) number;
        Logger.Debug("(double?)int: Done; Value: {Value}", numberDouble);
        double? result = sum / numberDouble;
        Logger.Debug("AverageValueInColumn: Done; Value: {Value}", result);
        return result;
    }
}