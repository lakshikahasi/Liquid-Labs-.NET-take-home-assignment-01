using Posts.Models;
using System.Data;
using System.Data.SqlClient;

namespace Posts.Data
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task BulkInsertPost(List<Post> posts)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Body", typeof(string));

            foreach (var post in posts)
            {
                table.Rows.Add(post.Id, post.UserId, post.Title, post.Body);
            }

            using var bulk = new SqlBulkCopy(conn)
            {
                DestinationTableName = "Posts"
            };

            await bulk.WriteToServerAsync(table);
        }

        public async Task<List<Post>> GetAllPosts()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = new SqlCommand("SELECT Id, UserId, Title, Body FROM POSTS", conn);
            using var reader = await query.ExecuteReaderAsync();

            var posts = new List<Post>();
            while (await reader.ReadAsync())
            {
                posts.Add(new Post
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Body = reader.GetString(3)
                });
            }
            return posts;
        }

        public async Task<Post?> GetPostById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = new SqlCommand("SELECT Id, UserId, Title, Body FROM POSTS WHERE Id = @id", conn);
            query.Parameters.AddWithValue("@id", id);

            using var reader = await query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Post
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Body = reader.GetString(3)
                };
            }
            return null;
        }
    }
}
