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

    public string GetAccountConnectionString() => ReadFromConfiguration("AccountConnection");

    public string GetApiRequestResultConnectionString() => ReadFromConfiguration("ApiRequestResultConnection");
   
    public string GetApiUrl() => ReadFromConfiguration("ApiUrl");
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

        string accountConnection = @"Server=(localdb)\mssqllocaldb;Database=Accounts;Trusted_Connection=True;";
        string apiRequestResultConnection = @"Server=(localdb)\mssqllocaldb;Database=ApiRequestsResults;Trusted_Connection=True;";
       
        string apiUrl = "https://official-joke-api.appspot.com";

        ConfigVariables values = new(n, l, sleep, accountConnection, apiRequestResultConnection, apiUrl);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(values));
        Log.Debug("Configuration.InitConfiguration: Done; File Path: {FilePath}; " +
            "N: {N}; L: {L}; sleep: {Sleep}; " +
            "AccountConnection: {AccountConnection}; " +
            "ApiRequestResultConnection: {ApiRequestResultConnection}; " +
            "ApiUrl: {ApiUrl}", FilePath, values.N, values.L, values.Sleep, values.AccountConnection, values.ApiRequestResultConnection, values.ApiUrl);
    }
    #endregion

    #region Records
    public record ConfigVariables(int N, int L, int Sleep, string AccountConnection, string ApiRequestResultConnection, string ApiUrl);

    #endregion
}