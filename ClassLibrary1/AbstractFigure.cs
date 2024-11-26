using Serilog.Core;

namespace Figure;

/// <summary>
/// Абстрактный класс, представляющий фигуру
/// </summary>
public abstract class AbstractFigure : IFigure, IMoving
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="speed"></param>
    public AbstractFigure(double speed) => Speed = speed;

    /// <summary>
    /// Логгер
    /// </summary>
    public static Logger Logger { get; set; }

    public double Speed { get; set; }

    public abstract string Type { get; }

    public abstract string Task { get; }

    public abstract void DoTheTask();

    /// <summary>
    /// Метод, выводящий на консоль информацию о начале выполнения задачи
    /// </summary>
    protected void StartTheTaskInfo() => Console.WriteLine($"{Task}: The {Type} has started the task");

    /// <summary>
    /// Метод, выводящий на консоль информацию о завершении выполнения задачи
    /// </summary>
    protected void CompleteTheTaskInfo() => Console.WriteLine($"{Task}: The {Type} has completed the task");
}
