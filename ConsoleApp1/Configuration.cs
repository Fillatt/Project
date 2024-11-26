using Microsoft.Extensions.Configuration;
using Serilog.Core;
using System.Text.Json;

namespace ConsoleApp1;

/// <summary>
/// Статический класс для взаимодействия с конфигурационным файлом
/// </summary>
public static class Configuration
{
    /// <summary>
    /// Статическое свойство для взаимодействия с логгером
    /// </summary>
    public static Logger Logger { get; set; }

    /// <summary>
    /// Чтение указанных переменных из конфигурационного файла; если файл отсутсвует,
    /// то создает его и инициализирует
    /// </summary>
    /// <param name="N"></param>
    /// <param name="L"></param>
    public static void StartConfiguration(ref int N, ref int L, ref int sleep)
    {
        string filePath = "appsettings.json";
        Logger.Debug("Configuration.StartConfiguration: Start; File Path: {FilePath}", filePath);
        if (!File.Exists(filePath)) InitConfiguration(filePath);
        ReadConfiguration(ref N, ref L, ref sleep, filePath);
        Logger.Debug("Configuration.StartConfiguration: Done; File Path: {FilePath}", filePath);
    }

    /// <summary>
    /// Чтение переменных из указанного конфигурационного файла
    /// </summary>
    /// <param name="N"></param>
    /// <param name="L"></param>
    /// <param name="fileName"></param>
    private static void ReadConfiguration(ref int N, ref int L, ref int sleep, string filePath)
    {
        Logger.Debug("Configuration.ReadConfiguration: Start; File Path: {FilePath}", filePath);
        var configuration = new ConfigurationBuilder().AddJsonFile(filePath).Build();
        if (!string.IsNullOrEmpty(configuration["N"]) && !string.IsNullOrEmpty(configuration["L"]) && !string.IsNullOrEmpty(configuration["sleep"]))
        {
            N = Convert.ToInt32(configuration["N"]);
            Logger.Debug("Convert.ToInt32(string): Done; Value: {Value}", N);
            L = Convert.ToInt32(configuration["L"]);
            Logger.Debug("Convert.ToInt32(string): Done; Value: {Value}", L);
            sleep = Convert.ToInt32(configuration["sleep"]);
            Logger.Debug("Convert.ToInt32(string): Done; Value: {Value}", sleep);
            Logger.Debug("Configuration.ReadConfiguration: Done; File Path: {FilePath}; N: {N}; L: {L}; sleep: {Sleep}", filePath, N, L, sleep);
        }
        else
        {
            throw new Exception($"Configuration.ReadConfiguration: The necessary data is missing; File Path: {filePath}");
        }
    }

    /// <summary>
    /// Создание и инициализация конфигурационного файла необходимыми переменными
    /// </summary>
    /// <param name="fileName"></param>
    private static void InitConfiguration(string filePath)
    {
        Logger.Debug("Configuration.InitConfiguration: Start; File Path: {FilePath}", filePath);
        Variables values = new Variables(9, 11, 10000);
        File.WriteAllText(filePath, JsonSerializer.Serialize(values));
        Logger.Debug("Configuration.InitConfiguration: Done; File Path: {FilePath}; N: {N}; L: {L}; sleep: {Sleep}", filePath, values.N, values.L, values.Sleep);
    }

    /// <summary>
    /// Запись, представляющая объект, содержащий небходимые перменные для сериализации в конфигурационный файл
    /// </summary>
    private record Variables(int N, int L, int Sleep);
}