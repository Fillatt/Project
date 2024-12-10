using System.Text;

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
        StringBuilder sb = new();
        sb.AppendLine(StartTheTaskInfo);
        sb.AppendLine(CompleteTheTaskInfo);
        Logger.Debug("Rectangle.DoTheTask: Done");
        Message = sb.ToString();
    }
}
