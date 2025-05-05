using Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using Models.Responses;
using DataAccess.Repository.IRepository;

namespace Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _db;
        public NotificationRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<NotificationRespose> GetNotificationSecureDetails(string userId)
        {
            var query = from notification in _db.Notifications
                           join user in _db.Users on notification.UserId equals user.Id
                           where notification.UserId == userId
                           select new NotificationRespose()
                           {
                               Name = user.FirstName + " " + user.LastName,
                               ImageUrl = user.ImageUrl,
                               IsRead = notification.IsRead,
                               Message = notification.Message,
                               Title = notification.Title,
                               Type = notification.Type,
                               CreatedAt = notification.CreatedAt,
                           };

            return query;
        }

        public void Update(Notification obj)
        {
            _db.Update(obj);
        }
    }
}
