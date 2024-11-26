namespace Figure;

public class Square : AbstractFigure
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="speed"></param>
    public Square(double speed) : base(speed) { }

    public override string Type => "Square";

    public override string Task => "Build a structure";

    public override void DoTheTask()
    {
        Logger.Debug("Square.DoTheTask: Start");
        StartTheTaskInfo();
        CompleteTheTaskInfo();
        Logger.Debug("Square.DoTheTask: Done");
    }
}
