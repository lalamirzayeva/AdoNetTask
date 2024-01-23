namespace AdoNetTask.Business.Services;

using AdoNetTask.Business.Exceptions;
using AdoNetTask.Business.Helpers;
using AdoNetTask.Core.Entities;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

public class TaskService
{
    public async Task<Post> GetObjectByIdAsync(int id)
    {
        if (id < 0) throw new InvalidIdException("Invalid ID input has been entered.");
        HttpClient client = new HttpClient();
        string url = $"https://jsonplaceholder.typicode.com/posts/{id}";
        HttpResponseMessage httpResponseMessage = await client.GetAsync(url);
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<Post>(data);
            return post;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
            Console.ResetColor();
            return null;
        }
    }
    public async Task AddObjToDbAsync(Post post)
    {
        string connString = @"Server=DESKTOP-EJ9B4A0\SQLEXPRESS;Database=P238Db;Trusted_Connection=true";
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            if (ObjExistsInDb(conn, post.Id))
            {
                throw new AlreadyExistException("Object with this id already exist in DB.");
            }
            string query = $"INSERT INTO Posts VALUES(@userId,@id,@title,@body)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@userId", post.UserId);
                cmd.Parameters.AddWithValue("@id", post.Id);
                cmd.Parameters.AddWithValue("@title", post.Title);
                cmd.Parameters.AddWithValue("@body", post.Body);
                int affectedRow = await cmd.ExecuteNonQueryAsync();
                if (affectedRow > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully added.");
                    Console.ResetColor();
                }
            }
        }
    }

    public bool ObjExistsInDb(SqlConnection conn, int id)
    {
        string checkQuery = $"SELECT COUNT(*) FROM Posts WHERE Id={id}";
        using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
        {
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
    public async Task PostsNotFoundInDbAsync()
    {
        JSonStatic jsonData = new JSonStatic();
        List<Post> posts = await jsonData.GetPostsFromApi();

        string connString = @"Server=DESKTOP-EJ9B4A0\SQLEXPRESS;Database=P238Db;Trusted_Connection=true";
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            string query = $"SELECT id FROM Posts";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                List<int> postsIdInDb = new List<int>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        postsIdInDb.Add(Convert.ToInt32(reader["id"]));
                    }
                }
                var postsNotInDb = posts.Where(apiPost => !postsIdInDb.Contains(apiPost.Id)).ToList();
                foreach (var postNotInDb in postsNotInDb)
                {
                    Console.WriteLine($"UserId: {postNotInDb.UserId},\n " +
                            $"Id: {postNotInDb.Id},\n" +
                            $"Title: {postNotInDb.Title},\n" +
                            $"Body: {postNotInDb.Body}");
                }
            }
        }
    }

    public async Task<int> GetPostCountAsync(int userId)
    {
        if (userId > 10 || userId < 1) throw new InvalidIdException("Please, enter valid User Id.");
        string connString = @"Server=DESKTOP-EJ9B4A0\SQLEXPRESS;Database=P238Db;Trusted_Connection=true";
        int result = -1;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            string query = $"SELECT COUNT(id) FROM Posts WHERE userId=@userId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                result = (int)await cmd.ExecuteScalarAsync();
            }
        }
        return result;
    }

    //public async Task AllPostsInApiAsync()
    //{
    //    JSonStatic jsonData = new JSonStatic();
    //    List<Post> posts = await jsonData.GetPostsFromApi();
    //    foreach (var post in posts)
    //    {
    //        Console.WriteLine($"UserId: {post.UserId},\n " +
    //                          $"Id: {post.Id},\n" +
    //                          $"Title: {post.Title},\n" +
    //                          $"Body: {post.Body}");

    //    }
    //}
}


