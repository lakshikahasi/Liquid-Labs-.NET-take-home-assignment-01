using Posts.Data;
using Posts.Models;

namespace Posts.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;
        private readonly Database _db;

        public PostService(HttpClient httpClient, Database db)
        {
            _httpClient = httpClient;
            _db = db;
        }

        public async Task FetchAndStoreAllPosts()
        {
            try
            {
                var posts = await _httpClient.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
                if (posts != null && posts.Any())
                {
                    await _db.BulkInsertPost(posts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during fetching data from 3rd party API" + ex.Message);
                throw;
            }
        }

        public async Task<List<Post>> GetAllPosts()
        {
            var posts = await _db.GetAllPosts();
            if (posts != null)
            {
                return posts;
            }

            var result = await _httpClient.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
            return result;
        }
        public async Task<Post?> GetPostById(int id)
        {
            var post = await _db.GetPostById(id);
            if (post != null)
            {
                return post;
            }

            var result = await _httpClient.GetFromJsonAsync<Post>("https://jsonplaceholder.typicode.com/posts/{id}");
            return result;
        }
    }
}
