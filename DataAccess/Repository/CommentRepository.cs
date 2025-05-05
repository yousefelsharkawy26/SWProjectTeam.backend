using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;
public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private readonly AppDbContext _db;
    public CommentRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Comment obj)
    {
        _db.Comments.Update(obj);
    }
}
