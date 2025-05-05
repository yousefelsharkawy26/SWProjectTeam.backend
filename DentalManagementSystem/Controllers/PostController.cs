using Models;
using Microsoft.AspNetCore.Mvc;
using DentalManagementSystem.Services.Interfaces;
using Models.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DentalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly IPostServices _postServices;

        public PostController(IAuthServices authServices, 
                              IPostServices postServices)
        {
            _authServices = authServices;
            _postServices = postServices;
        }

        [HttpGet]
        public IActionResult GetUserPosts()
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                var posts = _postServices.GetUserPosts(userId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("all"), AllowAnonymous]
        public IActionResult GetAllPosts()
        {
            try
            {
                var posts = _postServices.GetAllPosts();
                    

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostRequest post)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                var image = await _postServices.AddNewPost(userId, post);

                return StatusCode(201, image);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(int postId, Post post)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                await _postServices.UpdatePost(userId, postId, post);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                await _postServices.DeletePost(userId, postId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("add-like")]
        public async Task<IActionResult> AddLike(int postId)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                await _postServices.AddLike(userId, postId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-like")]
        public async Task<IActionResult> DeleteLike(int likeId)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                await _postServices.DeleteLike(userId, likeId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                await _postServices.AddComment(userId, comment.PostId, comment.Text);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-comment")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                await _postServices.DeleteComment(userId, commentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateComment(int commentId, string newText)
        {
            var userId = _authServices.GetClaims(Request).First(x => x.Type == "userId").Value;

            try
            {
                await _postServices.UpdateComment(commentId, userId, newText);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("share-post")]
        public IActionResult Share(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
