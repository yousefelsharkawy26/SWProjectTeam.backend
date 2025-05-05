using Models;

namespace DataAccess.Repository.IRepository;
public interface IPostRepository : IRepository<Post>
{
    void Update(Post obj);
}
