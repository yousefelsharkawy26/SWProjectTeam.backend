using Models.Requests;
using Models.Responses;
using DentalManagementSystem.Services.Interfaces;
using Azure.Core;
using DataAccess.Repository.IRepository;
using DentalManagementSystem.SignalRHubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Models;
using System.Threading.Tasks;

namespace DentalManagementSystem.Services;
public class NotificationServices : INotificationServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthServices _authServices;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly UserManager<User> _userManager;
    public NotificationServices(IUnitOfWork unitOfWork,
                                IAuthServices authServices,
                                IHubContext<NotificationHub> hubContext,
                                UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _authServices = authServices;
        _hubContext = hubContext;
        _userManager = userManager;
    }
    public IEnumerable<NotificationRespose> GetUserNotifications(string userId)
    {
        try
        {
            var notifications = _unitOfWork.Notification.GetAll(u => u.UserId == userId, includeProp: "Sender");

            var mapper = GetNotificationResposes(notifications);

            return mapper;
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException($"User does not have any notifications: \n{ex.Message}");
        }
    }

    public async Task<bool> SendNotification(NotificationRequest request, string senderId)
    {
        var notification = MapNotification(request, senderId);

        try
        {
            var user = await _userManager.FindByIdAsync(notification.UserId);
            //Save the notification to the database

            await _hubContext.Clients.All
                        .SendAsync("ReceiveNotification", notification);
            if (user == null)
                notification.UserId = null; // Set UserId to null if user is not found

            await _unitOfWork.Notification.Add(notification);
            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error in {nameof(SendNotification)}: {ex.Message}");
        }
    }

    Notification MapNotification(NotificationRequest request, string senderId)
    {
        return new()
        {
            Title = request.Title,
            Message = request.Message,
            UserId = request.UserId,
            Type = request.Type,
            SenderId = senderId
        };
    }
    IEnumerable<NotificationRespose> GetNotificationResposes(IEnumerable<Notification> notifications)
    {
        var lst = new List<NotificationRespose>();
        NotificationRespose mapper;
        foreach (var notification in notifications)
        {
            mapper = new NotificationRespose()
            {
                CreatedAt = notification.CreatedAt,
                ImageUrl = notification.Sender.ImageUrl,
                IsRead = notification.IsRead,
                Message = notification.Message,
                Name = notification.Sender.FirstName + " " + notification.Sender.LastName,
                Title = notification.Title,
                Type = notification.Type,
            };
            lst.Add(mapper);
        }
        return lst;
    }
}
