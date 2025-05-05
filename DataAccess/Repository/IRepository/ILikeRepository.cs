using Models;

namespace DataAccess.Repository.IRepository;
public interface ILikeRepository : IRepository<Like>
{
    void Update(Like obj);
}
