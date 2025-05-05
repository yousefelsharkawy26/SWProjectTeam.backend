using Models;
using DataAccess.Repository.IRepository;
using DentalManagementSystem.Services.Interfaces;
using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services;
public class PostServices : IPostServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileServices _fileServices;

    public PostServices(IUnitOfWork unitOfWork, 
                        IFileServices fileServices)
    {
        _unitOfWork = unitOfWork;
        _fileServices = fileServices;
    }
    public async Task<string> AddNewPost(string userId, PostRequest post)
    {
        if (post == null) 
            throw new ArgumentNullException(nameof(post) + "From " + nameof(AddNewPost));

        var image = _fileServices.UploudFile(post.Image, null);

        var newPost = new Post
        {
            Content = post.Content,
            ImageUrl = image,
            Title = post.Title,
            UserId = userId
        };

        await _unitOfWork.Post.Add(newPost);

        await _unitOfWork.SaveAsync();

        return image;
    }
    public async Task UpdatePost(string userId, int postId, Post post)
    {
        var prevPost = await _unitOfWork.Post.Get(u => u.UserId == userId && u.Id == postId);

        if (prevPost == null)
            throw new ArgumentNullException($"{nameof(prevPost)} in UpdatePost");

        prevPost.Title = post.Title;
        prevPost.Content = post.Content;
        prevPost.ImageUrl = post.ImageUrl;
        prevPost.CreatedAt = DateTime.Now;
        _unitOfWork.Post.Update(prevPost);
        await _unitOfWork.SaveAsync();
    }
    
    public IEnumerable<PostResponse> GetAllPosts()
    {
        var posts = _unitOfWork.Post.GetAll(includeProp: "User").ToList();

        if (!posts.Any())
            throw new ArgumentNullException("User didn't have any posts");
        var postsRes = new List<PostResponse>();
        foreach(var post in posts)
        {
            var likes = _unitOfWork.Like.GetAll(u => u.PostId == post.Id).Count();
            postsRes.Add(new PostResponse
            {
                Author = new()
                {
                    Name = post.User.FirstName + " " + post.User.LastName,
                    Avatar = post.User.ImageUrl,
                    Role = post.User.Permission
                },
                Content = post.Content,
                Comments = _unitOfWork.Comment.GetAll(u => u.PostId == post.Id).Count(),
                Likes = likes,
                Date = post.CreatedAt,
                Image = post.ImageUrl,
                IsLiked = likes > 0,
                Title = post.Title,
                Id = post.Id,
            });
        }
        
        return postsRes.OrderByDescending(u => u.Date);
    }
    public IEnumerable<PostResponse> GetUserPosts(string userId)
    {
        var posts = _unitOfWork.Post.GetAll(u => u.UserId == userId, includeProp: "User").ToList();

        if (!posts.Any())
            throw new ArgumentNullException("User didn't have any posts");
        var postsRes = new List<PostResponse>();
        foreach (var post in posts)
        {
            var likes = _unitOfWork.Like.GetAll(u => u.PostId == post.Id).Count();
            postsRes.Add(new PostResponse
            {
                Author = new()
                {
                    Name = post.User.FirstName + " " + post.User.LastName,
                    Avatar = post.User.ImageUrl,
                    Role = post.User.Permission
                },
                Content = post.Content,
                Comments = _unitOfWork.Comment.GetAll(u => u.PostId == post.Id).Count(),
                Likes = likes,
                Date = post.CreatedAt,
                Image = post.ImageUrl,
                IsLiked = likes > 0,
                Title = post.Title,
                Id = post.Id,
            });
        }

        return postsRes.OrderByDescending(u => u.Date);
    }
    public async Task DeletePost(string userId, int postId)
    {
        var post = await _unitOfWork.Post.Get(u =>u.UserId == userId && u.Id == postId);

        if (post == null)
            throw new ArgumentNullException($"{nameof(post)} in DeletePost");

        _unitOfWork.Post.Delete(post);
        await _unitOfWork.SaveAsync();
    }

    public async Task AddComment(string userId, int postId, string text)
    {
        await _unitOfWork.Comment.Add(new()
        {
            Text = text,
            UserId = userId,
            PostId = postId
        });

        await _unitOfWork.SaveAsync();
    }
    public async Task UpdateComment(int commentId, string userId,string newComment)
    {
        var prevComment = await _unitOfWork.Comment.Get(u => u.Id == commentId && u.UserId == userId);
        
        prevComment.Text = newComment;

        _unitOfWork.Comment.Update(prevComment);

        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteComment(string userId, int commentId)
    {
        var comment = await _unitOfWork.Comment.Get(u => u.Id == commentId && u.UserId == userId);

        _unitOfWork.Comment.Delete(comment);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task AddLike(string userId, int postId)
    {
        var newLike = new Like
        {
            UserId = userId,
            PostId = postId
        };

        await _unitOfWork.Like.Add(newLike);

        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteLike(string userId, int likeId)
    {
        var like = await _unitOfWork.Like.Get(u => u.Id == likeId && u.UserId == userId);

        _unitOfWork.Like.Delete(like);
        await _unitOfWork.SaveAsync();
    }
    
    public Task Share(string userId, int postId)
    {
        throw new NotImplementedException();
    }

    
   
}
