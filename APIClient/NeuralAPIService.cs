namespace APIClient;

public class NeuralAPIService : IAPIService
{
    #region Properties
    public string ApiUrl { get; set; } = String.Empty;

    public HttpClient HttpClient { get; set; }
    #endregion

    #region Constructors
    public NeuralAPIService()
    {
        HttpClient = new HttpClient();
    }
    #endregion

    #region Public Methods
    public async Task<HttpResponseMessage?> HealthRequestAsync()
    {
        try { return await HttpClient.GetAsync($"{ApiUrl}/health"); }
        catch { return null; }
    }

    public async Task<HttpResponseMessage> SendAsync(Uri uri)
    {
        var multipartFormContent = new MultipartFormDataContent();
        var fileStreamContent = new StreamContent(File.OpenRead(uri.LocalPath));
        multipartFormContent.Add(fileStreamContent);
        return await HttpClient.PostAsync($"{ApiUrl}/resize_image", multipartFormContent);
    }
    #endregion
}
