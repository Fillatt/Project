using System.Net.Http.Json;
using Serilog;
using DataBase;
using ConsoleApp;

namespace APIClient;

public class APIService
{
    #region Private Fields
    private string _apiUrl;
    private DbService _dbService;
    private HttpClient _httpClient;
    #endregion

    #region Constructors
    public APIService(Configuration cofiguration, DbService dbService)
    {
        _apiUrl = cofiguration.GetApiUrl();
        _httpClient = new HttpClient();
        _dbService = dbService;
    }
    #endregion

    #region Public Methods
    public async Task<Joke> GetRandomJoke()
    {
        Log.Debug("APIService.GetRandomJoke: Start");

        var response = await _httpClient.GetAsync($"{_apiUrl}/jokes/random");
        Joke joke = await response.Content.ReadFromJsonAsync<Joke>();

        await _dbService.Add(new ApiRequestResult
        {
            Type = joke.Type,
            Setup = joke.Setup,
            Punchline = joke.Punchline,
            JokeId = joke.Id
        });

        Log.Debug("APIService.GetRandomJoke: Done; Result: {Joke}", joke);
        Log.Information("Got a joke from API: {Joke}", joke);

        return joke;
    }

    public async Task<List<Joke>> GetRandom10Jokes()
    {
        Log.Debug("APIService.GetRandom10Jokes: Start");

        var response = await _httpClient.GetAsync($"{_apiUrl}/jokes/ten");
        var jokes = await response.Content.ReadFromJsonAsync<List<Joke>>();

        foreach (Joke joke in jokes)
        {
            await _dbService.Add(new ApiRequestResult
            {
                Type = joke.Type,
                Setup = joke.Setup,
                Punchline = joke.Punchline,
                JokeId = joke.Id
            });
        }

        Log.Debug("APIService.GetRandom10Jokes: Done; Result: {Joke}", jokes);
        Log.Information("Got a 10 jokes from API: {Joke}", jokes);

        return jokes;
    }
    #endregion
}
