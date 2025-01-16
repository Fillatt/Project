using Serilog;
using System.Text;

namespace Figure;

/// <summary>
/// Класс, представляющий круг
/// </summary>
public class Circle : AbstractFigure
{    
    public Circle(double speed) : base(speed)
    {
        Color = Color.Bright;
    }

    public override string Type => "Circle";

    public override string Mission => "Clear the way of obstacles";

    public override void StartTheMission()
    {
        Log.Debug("Circle.StartTheMission: Start");
        StringBuilder sb = new();
        sb.AppendLine(StartTheMissionInfo);
        Message = sb.ToString();
        StartClearTheWay();
        Log.Debug("Circle.StartTheMission: Done");
    }

    public override void StopTheMission() 
    {
        Log.Debug("Circle.StopTheMission: Start");
        StopClearTheWay();
        StringBuilder sb = new();
        sb.AppendLine(CompleteTheMissionInfo);
        Message = sb.ToString();
        Log.Debug("Circle.StopTheMission: Done");
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
        Log.Debug("Circle.StartClearTheWay: Start; Color: {Color}; Speed: {Speed}", Color, Speed);
        Color = Color.Dark;
        Speed /= 2;
        Log.Debug("Circle.StartClearTheWay: Done; Color: {Color}; Speed: {Speed}", Color, Speed);
    }
    /// <summary>
    /// Метод, завершающий очистку пути
    /// </summary>
    private void StopClearTheWay()
    {
        Log.Debug("Circle.StopClearTheWay: Start; Color: {Color}; Speed: {Speed}", Color, Speed);
        Color = Color.Bright;
        Speed *= 2;
        Log.Debug("Circle.StopClearTheWay: Done; Color: {Color}; Speed: {Speed}", Color, Speed);
    }
}
