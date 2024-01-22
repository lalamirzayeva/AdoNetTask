namespace AdoNetTask.DataAccess.Contexts;

//public static class AdoNetTaskDbContext
//{
//    static readonly HttpClient _httpClient = new HttpClient();
//    public static async Task<Array> GetFromApi()
//    {
//        try
//        {
//            string url = "https://jsonplaceholder.typicode.com/posts";
//            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
//            var data = await httpResponseMessage.Content.ReadAsStringAsync();
//            //dynamic json = JsonValue.Parse(data);
//            //string[] arr = { json.userId, json.id, json.title, json.body };
//            return arr;
//        }
//        catch (HttpRequestException ex)
//        {
//            Console.WriteLine(ex.Message);
//            return null;
//        }
//    }

//    public static List<> Posts { get; set; } = new List<>();
//}
