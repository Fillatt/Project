namespace Figure;

public class Triangle : AbstractFigure
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="speed"></param>
    public Triangle(double speed) : base(speed) { }

    public override string Type => "Triangle";

    public override string Task => "Strengthen unstable structures of local mines";

    public override void DoTheTask()
    {
        Logger.Debug("Triangle.DoTheTask: Start");
        StartTheTaskInfo();
        CompleteTheTaskInfo();
        Logger.Debug("Triangle.DoTheTask: Done");
    }
}
