using Figure;
using Serilog;
using Serilog.Core;

namespace ConsoleApp;

/// <summary>
/// Статический класс для вычислительных операций
/// </summary>
public static class Calculations
{
    #region Public Methods
    /// <summary>
    /// Инициализация списка, хранящего фигуры
    /// </summary>
    /// <param name="speeds"></param>
    /// <returns></returns>
    public static List<IFigure> FiguresInit(List<double> speeds)
    {
        Circle circle = new Circle(speeds.Max());
        speeds.Remove(speeds.Max());
        Log.Information("Circle: Created; Value: {@Circle}", circle);
        Triangle triangle = new Triangle(speeds[0]);
        Log.Information("Triangle: Created; Value: {@Triangle}", triangle);
        Square square = new Square(speeds[1]);
        Log.Information("Square: Created; Value: {@Square}", square);
        Rectangle rectangle = new Rectangle(speeds[2]);
        Log.Information("Rectangle: Created; Value: {@Rectangle}", rectangle);

        var figures = new List<IFigure>()
        {
            circle,
            triangle,
            square,
            rectangle
        };
        Log.Information("Figures list: Created");
        return figures;
    }    

    /// <summary>
    /// Инициализация oddNumbers
    /// </summary>
    /// <param name="oddNumbers"></param>
    public static int[] OddNumbersInit()
    {
        Log.Information("oddNumbers: Start init");
        int[] oddNumbers = [5, 7, 9, 11, 13, 15, 17, 19];
        Log.Information("oddNumbers: Init done");
        return oddNumbers;
    }

    /// <summary>
    /// Вычисление сумм
    /// </summary>
    /// <param name="oddNumbers"></param>
    /// <param name="N"></param>
    /// <param name="L"></param>
    /// <returns></returns>
    public static List<double> CountAmounts(int[] oddNumbers, int N, int L)
    {
        List<double> amounts = [];
        for (int i = 0; i < 4; i++)
        {
            bool isNull = true;
            while (isNull)
            {
                double[] randomValues = RandomValuesInit(13);
                double?[,] k = new double?[8, 13];
                k = KInit(8, 13, randomValues, oddNumbers);
                double? sum = CountSum(k, N, L);
                if (sum != null)
                {
                    isNull = false;
                    amounts.Add((double)sum);
                }
            }                       
        }
        return amounts;
    }

    /// <summary>
    /// Инициализация скоростей фигур
    /// </summary>
    /// <param name="amounts"></param>
    /// <returns></returns>
    public static List<double> SpeedsInit(List<double> amounts)
    {
        List<double> speeds = [];
        foreach (var amount in amounts) AddSpeed(ref speeds, amount);
        return speeds;
    }

    /// <summary>
    /// Инициализация randomValues
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    #endregion

    #region Private Methods
    private static double[] RandomValuesInit(int size)
    {
        Log.Information("randomValues: Start init");
        double[] randomValues = new double[size];
        randomValues.Init(-12.0, 15.0);
        Log.Information("randomValues: Init done");
        return randomValues;
    }

    /// <summary>
    /// Инициализация двумерного массива k
    /// </summary>
    /// <param name="size1"></param>
    /// <param name="size2"></param>
    /// <param name="randomValues"></param>
    /// <param name="oddNumbers"></param>
    /// <returns></returns>
    private static double?[,] KInit(int size1, int size2, double[] randomValues, int[] oddNumbers)
    {
        Log.Information("k: Start init");
        double?[,] k = new double?[size1, size2];
        k.Init(randomValues, oddNumbers);
        Log.Information("k: Init done");
        return k;
    }

    /// <summary>
    /// Вычисление суммы
    /// </summary>
    /// <param name="k"></param>
    /// <param name="N"></param>
    /// <param name="L"></param>
    /// <returns></returns>
    private static double? CountSum(double?[,] k, int N, int L)
    {
        Log.Information("Sum: Start init");
        double? min = k.MinValueInRow(N % 8);
        double? average = k.AverageValueInColumn(L % 13);
        double? sum = min + average;
        Log.Information("Sum: Init done; Value: {Value}", sum);
        return sum;
    }

    /// <summary>
    /// Добавление значения в speeds
    /// </summary>
    /// <param name="speeds"></param>
    /// <param name="sum"></param>
    private static void AddSpeed(ref List<double> speeds, double? sum)
    {
        double result;
        if (sum != null) result = Math.Abs((double)sum);
        else result = 0;
        speeds.Add(result);
        Log.Information("speeds.Add: Done; Result: {Result}", result);
    }
    #endregion
}
