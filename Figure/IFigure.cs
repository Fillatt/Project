namespace Figure;

/// <summary>
/// Интерфейс для определения поведения фигуры
/// </summary>
public interface IFigure
{   
    string Type { get; }
    
    string Mission { get; }

    string Message { get; set; }

    void StartTheMission();
   
    void StopTheMission();
}
