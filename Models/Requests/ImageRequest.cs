using Microsoft.AspNetCore.Http;

namespace Models.Requests;
class ImageRequest
{
    IFormFile File { get; set; }
}
