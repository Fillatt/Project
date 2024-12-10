using System.Text;

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
        StringBuilder sb = new();
        sb.AppendLine(StartTheTaskInfo);
        sb.AppendLine(CompleteTheTaskInfo);
        Logger.Debug("Triangle.DoTheTask: Done");
        Message = sb.ToString();
    }
}
