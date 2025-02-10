using Serilog;
using System.Text;

namespace Figure;

public class Triangle : AbstractFigure
{
    public Triangle(double speed) : base(speed) { }

    public override string Type => "Triangle";

    public override string Mission => "Strengthen unstable structures of local mines";

    public override void StartTheMission()
    {
        Log.Debug("Triangle.StartTheMission: Start");
        StringBuilder sb = new();
        sb.AppendLine(StartTheMissionInfo);
        Message = sb.ToString();
        Log.Debug("Triangle.StartTheMission: Done");
    }

    public override void StopTheMission()
    {
        Log.Debug("Triangle.StopTheMission: Start");
        StringBuilder sb = new();
        sb.AppendLine(CompleteTheMissionInfo);
        Message = sb.ToString();
        Log.Debug("Triangle.StopTheMission: Done");
    }
}
