using Models;
using Models.Responses;

namespace DataAccess.Repository.IRepository;

public interface INotificationRepository : IRepository<Notification>
{
    void Update(Notification obj);
    IEnumerable<NotificationRespose> GetNotificationSecureDetails(string userId);
}
