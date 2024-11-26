namespace Figure;

public class Rectangle : AbstractFigure
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="speed"></param>
    public Rectangle(double speed) : base(speed) { }

    public override string Type => "Rectangle";

    public override string Task => "Build a bridge";

    public override void DoTheTask()
    {
        Logger.Debug("Rectangle.DoTheTask: Start");
        StartTheTaskInfo();
        CompleteTheTaskInfo();
        Logger.Debug("Rectangle.DoTheTask: Done");
    }
}
