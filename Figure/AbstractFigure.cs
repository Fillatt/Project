namespace Figure;

/// <summary>
/// Абстрактный класс, представляющий фигуру
/// </summary>
public abstract class AbstractFigure : IFigure, IMoving
{
    private string _message;

    public AbstractFigure(double speed) => Speed = speed;

    public double Speed { get; set; }

    public abstract string Type { get; }

    public abstract string Mission { get; }

    public string Message { get; set; }

    public abstract void StartTheMission();

    public abstract void StopTheMission();

    /// <summary>
    /// Метод, выводящий на консоль информацию о начале выполнения задачи
    /// </summary>
    public string StartTheMissionInfo => $"{Mission}: The {Type} has started the mission";

    /// <summary>
    /// Метод, выводящий на консоль информацию о завершении выполнения задачи
    /// </summary>
    public string CompleteTheMissionInfo => $"{Mission}: The {Type} has completed the mission";
}
