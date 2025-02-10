using Serilog;
using System.Text;

namespace Figure;

public class Square : AbstractFigure
{
    public Square(double speed) : base(speed) { }

    public override string Type => "Square";

    public override string Mission => "Build a structure";

    public override void StartTheMission()
    {
        Log.Debug("Square.StartTheMission: Start");
        StringBuilder sb = new();
        sb.AppendLine(StartTheMissionInfo);
        Message = sb.ToString();
        Log.Debug("Square.StartTheMission: Done");
    }

    public override void StopTheMission()
    {
        Log.Debug("Square.StopTheMission: Start");
        StringBuilder sb = new();
        sb.AppendLine(CompleteTheMissionInfo);
        Message = sb.ToString();
        Log.Debug("Square.StopTheMission: Done");
    }
}
