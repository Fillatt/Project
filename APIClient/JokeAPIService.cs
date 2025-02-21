using ConsoleApp;
using Serilog;
using System.Net.Http.Json;

namespace APIClient;

public class JokeAPIService : IAPIService
{
    #region Properties
    public string ApiUrl { get; set; }

    public HttpClient HttpClient { get; set; }
    #endregion

    #region Constructors
    public JokeAPIService(ConfigurationService cofiguration)
    {
        ApiUrl = cofiguration.GetJokeApiUrl();
        HttpClient = new HttpClient();
    }
    #endregion

    #region Public Methods
    public async Task<Joke> GetRandomJoke()
    {
        Log.Debug("APIService.GetRandomJoke: Start");

        var response = await HttpClient.GetAsync($"{ApiUrl}/jokes/random");
        Joke joke = await response.Content.ReadFromJsonAsync<Joke>();

        Log.Debug("APIService.GetRandomJoke: Done; Result: {Joke}", joke);

        return joke;
    }

    public async Task<List<Joke>> GetRandom10Jokes()
    {
        Log.Debug("APIService.GetRandom10Jokes: Start");

        var response = await HttpClient.GetAsync($"{ApiUrl}/jokes/ten");
        var jokes = await response.Content.ReadFromJsonAsync<List<Joke>>();

        Log.Debug("APIService.GetRandom10Jokes: Done; Result: {Joke}", jokes);

        return jokes;
    }
    #endregion
}
