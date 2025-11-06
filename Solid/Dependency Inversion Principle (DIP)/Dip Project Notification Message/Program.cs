INotificationSender notif = new EmailSender();
NotificationService service = new NotificationService(notif);
service.NotificationSend();


public interface INotificationSender
{
    void ProcessNotification();
}
public class EmailSender :INotificationSender
{
    public void ProcessNotification()
    {
        System.Console.WriteLine("Email sent: Halo, pesanan kamu sudah dikirim!");
    }
}

public class NotificationService
{
    private readonly INotificationSender _notification;
    public NotificationService(INotificationSender notification)
    {
        _notification = notification;
    }
    public void NotificationSend()
    {
        _notification.ProcessNotification();
        
    }

}
