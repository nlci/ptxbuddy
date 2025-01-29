using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class GeminiApiHelper
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> CallGeminiApi(string apiKey, string endpoint, string requestData)
    {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var content = new StringContent(requestData, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error calling API: {response.ReasonPhrase}");
        }

        return await response.Content.ReadAsStringAsync();
    }
}
