namespace APIClient;

public interface IAPIService
{
    HttpClient HttpClient { get; set; }

    string ApiUrl { get; set; }
}
