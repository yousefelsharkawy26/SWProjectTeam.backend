using Models;

namespace DataAccess.Repository.IRepository;
public interface ICommentRepository : IRepository<Comment>
{
    void Update(Comment obj);
}
