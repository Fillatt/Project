using Serilog;
using System.Net.Http.Json;

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
        Log.Debug("NeuralAPIService.HealthRequestAsync: Start");
        try
        {
            var response = await HttpClient.GetAsync($"{ApiUrl}/health");
            Log.Debug("NeuralAPIService.HealthRequestAsync: Done; Response: {Response}", response);
            return response;
        }
        catch
        {
            Log.Debug("NeuralAPIService.HealthRequestAsync: Fail");
            return null;
        }
    }

    public async Task<NeuralAPIResponse> SendAsync(Uri uri)
    {
        Log.Debug("NeuralAPIService.SendAsync: Start");
        var multipartFormContent = new MultipartFormDataContent();
        var fileStreamContent = new StreamContent(File.OpenRead(uri.LocalPath));
        multipartFormContent.Add(fileStreamContent, name: "image", fileName: Path.GetFileName(uri.LocalPath));
        var response = await HttpClient.PostAsync($"{ApiUrl}/resize_image", multipartFormContent);
        Log.Debug("NeuralAPIService.SendAsync: Done; Response: {Response}", response);
        return await response.Content.ReadFromJsonAsync<NeuralAPIResponse>();
    }
    #endregion
}
