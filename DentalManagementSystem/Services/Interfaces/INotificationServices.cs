using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services.Interfaces
{
    public interface INotificationServices
    {
        IEnumerable<NotificationRespose> GetUserNotifications(string userId);
        Task<bool> SendNotification(NotificationRequest request, string senderId);
    }
}
