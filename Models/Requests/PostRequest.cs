using Microsoft.AspNetCore.Http;

namespace Models.Requests;
public class PostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public IFormFile Image { get; set; }
}
