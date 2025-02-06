using ConsoleApp;
using DataBase;
using Serilog;
using System.Net.Http.Json;

namespace APIClient;

public class JokeAPIService : IAPIService
{
    #region Private Fields
    private DbService _dbService;
    #endregion

    #region Properties
    public string ApiUrl { get; set; }

    public HttpClient HttpClient { get; set; }
    #endregion

    #region Constructors
    public JokeAPIService(Configuration cofiguration, DbService dbService)
    {
        ApiUrl = cofiguration.GetJokeApiUrl();
        HttpClient = new HttpClient();
        _dbService = dbService;
    }
    #endregion

    #region Public Methods
    public async Task<Joke> GetRandomJoke()
    {
        Log.Debug("APIService.GetRandomJoke: Start");

        var response = await HttpClient.GetAsync($"{ApiUrl}/jokes/random");
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

        var response = await HttpClient.GetAsync($"{ApiUrl}/jokes/ten");
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
