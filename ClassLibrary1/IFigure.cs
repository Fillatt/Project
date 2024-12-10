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
    string Task { get; }

    string Message { get; set; }

    /// <summary>
    /// Метод, выполняющий поставленную фигуре задачу
    /// </summary>
    void DoTheTask();
}
