using Models;
using Models.Responses;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repository.IRepository;

namespace Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }
        public async Task<UserResponse> GetUserData(string Id)
        {
            var query = from user in _db.Users
                        where user.Id == Id
                        select new UserResponse() 
                        {
                            Id = user.Id,
                            Name = user.FirstName + " " + user.LastName,
                            Email = user.Email,
                            ImageUrl = user.ImageUrl,
                            Permission = user.Permission,
                            Phone = user.PhoneNumber,
                            Bio = user.Bio,
                        };

            return await query.FirstOrDefaultAsync();
        }
        public IEnumerable<UserResponse> SearchUsersByName(string userName)
        {
            var query = from user in _db.Users
                        where (user.FirstName+ " " + user.LastName).Contains(userName)
                        select new UserResponse()
                        {
                            Id = user.Id,
                            Name = user.FirstName + " " + user.LastName,
                            Email = user.Email,
                            ImageUrl = user.ImageUrl,
                            Permission = user.Permission,
                            Phone = user.PhoneNumber,
                            Bio = user.Bio,
                        };

            return query;
        }
        public IEnumerable<UserResponse> FindUsersByEmailOrUserName(string search)
        {
            var query = from user in _db.Users
                        where user.Email.Contains(search) || user.UserName.Contains(search)
                        select new UserResponse()
                        {
                            Id = user.Id,
                            Name = user.FirstName + " " + user.LastName,
                            Email = user.Email,
                            Phone = user.PhoneNumber,
                            ImageUrl = user.ImageUrl != null ? user.ImageUrl : "avatar.png",
                            Permission = user.Permission,
                            Bio = user.Bio
                        };

            return query;
        }
    }
}
