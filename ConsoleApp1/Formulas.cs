using Serilog.Core;
using static System.Math;
namespace Stage1;

/// <summary>
/// Статический класс для вычисления по формулам
/// </summary>
public static class Formulas
{
    /// <summary>
    /// Статическое свойство для взаимодействия с логгером
    /// </summary>
    public static Logger Logger { get; set; }

    /// <summary>
    /// Формула при oddNumbers[i] = 9
    /// </summary>
    /// <param name="oddNumbers"></param>
    /// <param name="randomValues"></param>
    /// <param name="k"></param>
    /// <param name="i"></param>
    /// <param name="log"></param>
    public static void First(double[] randomValues, double?[,] k, int i)
    {
        Logger.Debug("Formulas.First: Start; Rank: {I}", i);
        for (int j = 0; j < k.GetLength(1); j++)
        {
            k[i, j] = Math.Sin(Math.Sin(Math.Pow((randomValues[j] / (randomValues[j] + 1 / 2)), randomValues[j])));
            Logger.Debug("Formulas.First: Iteration done; k[{I}][{J}]: {Value}", i, j, k[i, j]);
        }
        Logger.Debug("Formulas.First: Done; Rank: {I}", i);
    }

    /// <summary>
    /// Формула при oddNumbers[i] ∊ {5,7,11,15}
    /// </summary>
    /// <param name="oddNumbers"></param>
    /// <param name="randomValues"></param>
    /// <param name="k"></param>
    /// <param name="i"></param>
    /// <param name="log"></param>
    public static void Second(double[] randomValues, double?[,] k, int i)
    {
        Logger.Debug("Formulas.Second: Start; Rank: {I}", i);
        for (int j = 0; j < k.GetLength(1); j++)
        {
            k[i, j] = Math.Pow(0.5 / (Math.Tan(2 * randomValues[j]) + 2 / 3), Math.Pow(randomValues[j], 1 / 9));
            Logger.Debug("Formulas.Second: Iteration done; k[{I}][{J}]: {Value}", i, j, k[i, j]);
        }
        Logger.Debug("Formulas.Second: Done; Rank: {I}", i);
    }

    /// <summary>
    /// Формула для остальных случаев
    /// </summary>
    /// <param name="oddNumbers"></param>
    /// <param name="randomValues"></param>
    /// <param name="k"></param>
    /// <param name="i"></param>
    /// <param name="log"></param>
    public static void Third(double[] randomValues, double?[,] k, int i)
    {
        Logger.Debug("Formulas.Third: Start; Rank: {I}", i);
        for (int j = 0; j < k.GetLength(1); j++)
        {
            k[i, j] = Math.Tan(Math.Pow(Math.Exp((1 - randomValues[j]) / Math.PI) / 3 / 4, 3));
            Logger.Debug("Formulas.Third: Iteration done; k[{I}][{J}]: {Value}", i, j, k[i, j]);
        }
        Logger.Debug("Formulas.Third: Done; Rank: {I}", i);
    }
}

