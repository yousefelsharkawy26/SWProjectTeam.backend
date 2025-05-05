namespace Models.Responses;
public class PostResponse
{
    public int Id { get; set; }
    public Author Author { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Image { get; set; }
    public DateTime Date { get; set; }
    public int Likes { get; set; }
    public int Comments { get; set; }
    public bool IsLiked { get; set; }
}
