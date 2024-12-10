using System.Text;

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
        StringBuilder sb = new();
        sb.AppendLine(StartTheTaskInfo);
        sb.AppendLine(CompleteTheTaskInfo);
        Logger.Debug("Square.DoTheTask: Done");
        Message = sb.ToString();
    }
}
