using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System.Text.Json;

namespace ConsoleApp;

/// <summary>
/// Класс для взаимодействия с конфигурационным файлом
/// </summary>
public class Configuration
{
    #region Properties
    /// <summary>
    /// Путь конфигурационного файла
    /// </summary>
    public string FilePath { get; }
    #endregion

    #region Constructors
    public Configuration(string filePath)
    {
        FilePath = filePath;
    }
    #endregion

    #region Public Methods
    public int GetN() => Convert.ToInt32(ReadFromConfiguration("N"));
   
    public int GetL() => Convert.ToInt32(ReadFromConfiguration("L"));

    public int GetSleep() => Convert.ToInt32(ReadFromConfiguration("Sleep"));

    public string GetConnectionString() => ReadFromConfiguration("ConnectionString");
    #endregion

    #region Private Methods
    /// <summary>
    /// Чтение из конфигурационного файла
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    private string ReadFromConfiguration(string element)
    {
        Log.Debug("Configuration.ReadFromConfiguration: Start; Is Read: {Element}; FilePath: {FilePath}", element, FilePath);
        if (!File.Exists(FilePath)) InitConfiguration();
        var configuration = new ConfigurationBuilder().AddJsonFile(FilePath).Build();
        if (string.IsNullOrEmpty(configuration[element]))
        {
            Log.Error("Configuration.ReadConfiguration: The necessary data for {Element} is missing; File Path: {FilePath}", element, FilePath);
            InitConfiguration();
            configuration = new ConfigurationBuilder().AddJsonFile(FilePath).Build();
        }
        Log.Debug("Configuration.ReadFromConfiguration: Done; Is Read: {Element}; FilePath: {FilePath}", element, FilePath);
        return configuration[element];
    }

    /// <summary>
    /// Создание и инициализация конфигурационного файла необходимыми переменными
    /// </summary>
    /// <param name="fileName"></param>
    private void InitConfiguration()
    {
        Log.Debug("Configuration.InitConfiguration: Start; File Path: {FilePath}", FilePath);
        int n = 9;
        int l = 11;
        int sleep = 3000;
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Account;Trusted_Connection=True;";
        ConfigVariables values = new(n, l, sleep, connectionString);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(values));
        Log.Debug("Configuration.InitConfiguration: Done; File Path: {FilePath}; N: {N}; L: {L}; sleep: {Sleep}; ConnectionString: {ConnectionString}", FilePath, values.N, values.L, values.Sleep, values.ConectionString);
    }
    #endregion

    #region Records
    /// <summary>
    /// Запись, представляющая объект, содержащий небходимые перменные для сериализации в конфигурационный файл
    /// </summary>
    private record ConfigVariables(int N, int L, int Sleep, string ConectionString);
    #endregion
}