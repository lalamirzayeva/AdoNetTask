using AdoNetTask.Core.Entities;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdoNetTask.Business.Helpers;

public class JSonStatic
{
    readonly HttpClient _httpClient = new HttpClient();
    public async Task<List<Post>> GetPostsFromApi()
    {
        try
        {
            string url = "https://jsonplaceholder.typicode.com/posts";
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<Post>>(data);

                return posts;
            }
            else
            {
                Console.WriteLine($"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
                return null;
            }
        }
        catch (HttpRequestException h)
        {
            Console.WriteLine($"HTTP request error: {h.Message}");
            return null;
        }
    }
}


