using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;
public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly AppDbContext _db;
    public PostRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Post obj)
    {
        _db.Posts.Update(obj);
    }
}
