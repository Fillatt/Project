namespace Figure;

/// <summary>
/// Класс, представляющий круг
/// </summary>
public class Circle : AbstractFigure
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="speed"></param>
    public Circle(double speed) : base(speed)
    {
        Color = Color.Bright;
    }

    public override string Type => "Circle";

    public override string Task => "Clear the way of obstacles";

    public override void DoTheTask()
    {
        Logger.Debug("Circle.DoTheTask: Start");
        StartTheTaskInfo();
        StartClearTheWay();
        StopClearTheWay();
        CompleteTheTaskInfo();
        Logger.Debug("Circle.DoTheTask: Done");
    }

    /// <summary>
    /// Цвет фигуры
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Метод, начинающий очистку пути
    /// </summary>
    private void StartClearTheWay()
    {
        Logger.Debug("Circle.StartClearTheWay: Start; Color: {Color}; Speed: {Speed}", Color, Speed);
        Color = Color.Dark;
        Speed /= 2;
        Logger.Debug("Circle.StartClearTheWay: Done; Color: {Color}; Speed: {Speed}", Color, Speed);
    }

    /// <summary>
    /// Метод, завершающий очистку пути
    /// </summary>
    private void StopClearTheWay()
    {
        Logger.Debug("Circle.StopClearTheWay: Start; Color: {Color}; Speed: {Speed}", Color, Speed);
        Color = Color.Bright;
        Speed *= 2;
        Logger.Debug("Circle.StopClearTheWay: Done; Color: {Color}; Speed: {Speed}", Color, Speed);
    }
}
