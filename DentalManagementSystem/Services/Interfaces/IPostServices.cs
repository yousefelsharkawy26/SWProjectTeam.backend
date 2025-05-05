using Models;
using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services.Interfaces;
public interface IPostServices
{
    Task<string> AddNewPost(string userId, PostRequest post);
    IEnumerable<PostResponse> GetAllPosts();
    IEnumerable<PostResponse> GetUserPosts(string userId);
    Task UpdatePost(string userId, int postId, Post post);
    Task DeletePost(string userId, int postId);
    Task AddLike(string userId, int postId);
    Task DeleteLike(string userId, int likeId);
    Task AddComment(string userId, int postId, string text);
    Task UpdateComment(int commentId, string userId,string newComment);
    Task DeleteComment(string userId, int commentId);
    Task Share(string userId, int postId);
}
