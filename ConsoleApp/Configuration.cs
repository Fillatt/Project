using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;

namespace ConsoleApp;

/// <summary>
/// Класс для взаимодействия с конфигурационным файлом
/// </summary>
public partial class Configuration
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
   
    public string GetJokeApiUrl() => ReadFromConfiguration("JokeAPIUrl");

    public string GetNeuralApiUrl() => ReadFromConfiguration("NeuralAPIUrl");
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
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=DataBase;Trusted_Connection=True;";
        string jokeApiUrl = "https://official-joke-api.appspot.com";
        string neuralApiUrl = "http://localhost:8000";

        ConfigVariables values = new(n, l, sleep, connectionString, jokeApiUrl, neuralApiUrl);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(values));
        Log.Debug("Configuration.InitConfiguration: Done; File Path: {FilePath}; " +
            "N: {N}; L: {L}; sleep: {Sleep}; " +
            "ConnectionString: {AccountConnection}; " +
            "JokeAPIUrl: {JokeAPIUrl}" +
            "NeuralApiUrl: {NeuralApiUrl}", 
            FilePath, values.N, values.L, values.Sleep, values.ConnectionString, values.JokeAPIUrl, values.NeuralApiUrl);
    }
    #endregion

    #region Records
    public record ConfigVariables(int N, int L, int Sleep, string ConnectionString, string JokeAPIUrl, string NeuralApiUrl);

    #endregion
}