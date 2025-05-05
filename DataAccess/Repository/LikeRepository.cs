using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;
public class LikeRepository : Repository<Like>, ILikeRepository
{
    private readonly AppDbContext _db;
    public LikeRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Like obj)
    {
        _db.Likes.Update(obj);
    }
}
