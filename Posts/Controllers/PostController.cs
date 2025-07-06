using Microsoft.AspNetCore.Mvc;
using Posts.Services;

namespace Posts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostService _service;

        public PostController(PostService service)
        {
            _service = service;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportAll()
        {
            await _service.FetchAndStoreAllPosts();
            return Ok("Import all post.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllPosts();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetPostById(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
