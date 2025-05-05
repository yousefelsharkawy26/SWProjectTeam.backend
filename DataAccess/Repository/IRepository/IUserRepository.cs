using Models;
using Models.Responses;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User obj);
        Task<UserResponse> GetUserData(string Id);
        IEnumerable<UserResponse> SearchUsersByName(string userName);
        IEnumerable<UserResponse> FindUsersByEmailOrUserName(string search);
    }
}
