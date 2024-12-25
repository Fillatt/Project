namespace Figure;

/// <summary>
/// Интерфейс для определения поведения фигуры
/// </summary>
public interface IFigure
{
    /// <summary>
    /// Тип фигуры
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Задача фигуры
    /// </summary>
    string Mission { get; }

    string Message { get; set; }

    /// <summary>
    /// Метод, начинающий выполнять поставленную фигуре задачу
    /// </summary>
    void StartTheMission();

    /// <summary>
    /// Метод, завершающий выполнение поставленной задачи
    /// </summary>
    void StopTheMission();
}
