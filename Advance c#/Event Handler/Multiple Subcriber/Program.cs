public class NewsAgency
{
    string _news;
    public string News
    {
        get => _news;
        set => _news = value;
    }
    public event EventHandler NewsPublished;

    public void SetNews(string news)
    {
        News = news;

        OnNewsPublished();
    }

    protected virtual void OnNewsPublished()
    {
        NewsPublished?.Invoke(this, EventArgs.Empty);
    }


}

public class Sms
{
    public void SendNotifier(object sender, EventArgs e)
    {
        if (sender is NewsAgency send)
        System.Console.WriteLine($"SMS notif : {send.News}");
    }
}
public class Email
{
    public void SendNotifier(object sender, EventArgs e)
    {
        if (sender is NewsAgency send)
        System.Console.WriteLine($"Email notif : {send.News}");
    }
}
public class Log
{
    public void SendNotifier(object sender, EventArgs e)
    {
        if (sender is NewsAgency send)
        System.Console.WriteLine($"Log notif : {send.News}");
    }
}

public class Program
{
    public static void Main()
    {
        var news = new NewsAgency();
        var sms = new Sms();
        var email = new Email();
        var log = new Log();

        news.NewsPublished += sms.SendNotifier;
        news.NewsPublished += email.SendNotifier;
        news.NewsPublished += log.SendNotifier;

        news.SetNews("Hari ini the best day ever");
    }
}

//sebenarnya bisa gk dibikin class sms, email dan log
//tinggal subcribe methodnya

/*
        newsAgency.NewsPublished += SmsNotifier;
        newsAgency.NewsPublished += EmailNotifier;
        newsAgency.NewsPublished += LogNotifier;
        jadinya kayak gini
        tapi bikin methodnya harus gini:
        private static void EmailNotifier
        harus static void juga kalau gk return
*/
