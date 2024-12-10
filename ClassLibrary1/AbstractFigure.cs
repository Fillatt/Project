using Serilog.Core;

namespace Figure;

/// <summary>
/// Абстрактный класс, представляющий фигуру
/// </summary>
public abstract class AbstractFigure : IFigure, IMoving
{
    private string _message;

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

    public string Message { get; set; }

    public abstract void DoTheTask();

    /// <summary>
    /// Метод, выводящий на консоль информацию о начале выполнения задачи
    /// </summary>
    protected string StartTheTaskInfo => $"{Task}: The {Type} has started the task";

    /// <summary>
    /// Метод, выводящий на консоль информацию о завершении выполнения задачи
    /// </summary>
    protected string CompleteTheTaskInfo => $"{Task}: The {Type} has completed the task";
}
