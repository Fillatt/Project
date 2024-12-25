using Serilog;
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

    public override string Mission => "Build a bridge";

    public override void StartTheMission()
    {
        Log.Debug("Rectangle.StartTheMission: Start");
        StringBuilder sb = new();
        sb.AppendLine(StartTheMissionInfo);
        Message = sb.ToString();
        Log.Debug("Rectangle.StartTheMission: Done");
    }
    public override void StopTheMission() 
    {
        Log.Debug("Rectangle.StopTheMission: Start");
        StringBuilder sb = new();
        sb.AppendLine(CompleteTheMissionInfo);
        Message = sb.ToString();
        Log.Debug("Rectangle.StopTheMission: Done");
    }
}
