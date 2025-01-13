// Web User class
public class WebUser
{
    private string _name;

    public WebUser(string name)
    {
        _name = name;
    }

    public void Notify(string channelName, string videoTitle)
    {
        Console.WriteLine($"[Web] {_name} has been notified about a new video: '{videoTitle}' on channel '{channelName}'.");
    }
}

// Mobile User class
public class MobileUser
{
    private string _name;

    public MobileUser(string name)
    {
        _name = name;
    }

    public void Notify(string channelName, string videoTitle)
    {
        Console.WriteLine($"[Mobile] {_name} has been notified about a new video: '{videoTitle}' on channel '{channelName}'.");
    }
}

// YouTube Channel class
public class YouTubeChannel
{
    private string _channelName;
    private List<WebUser> _webUsers = new List<WebUser>();
    private List<MobileUser> _mobileUsers = new List<MobileUser>();

    public YouTubeChannel(string channelName)
    {
        _channelName = channelName;
    }

    public void UploadVideo(string videoTitle)
    {
        Console.WriteLine($"Channel '{_channelName}' uploaded a new video: '{videoTitle}'.");

        foreach (var user in _webUsers)
        {
            user.Notify(_channelName, videoTitle);
        }

        foreach (var user in _mobileUsers)
        {
            user.Notify(_channelName, videoTitle);
        }
    }
}

// Usage
class Program
{
    static void Main(string[] args)
    {
        var channel = new YouTubeChannel("TechExplained");

        channel.UploadVideo("Observer Pattern Explained");

        channel.UploadVideo("Design Patterns in C#");
    }
}

//Abstract Factory Pattern

public interface IObserver
{
    void Update(string channelName, string videoTitle);
}

public class WebUser : IObserver
{
    private string _name;

    public WebUser(string name)
    {
        _name = name;
    }

    public void Update(string channelName, string videoTitle)
    {
        Console.WriteLine($"[Web] {_name} has been notified about a new video: '{videoTitle}' on channel '{channelName}'.");
    }
}

// Concrete Observer (Mobile User)
public class MobileUser : IObserver
{
    private string _name;

    public MobileUser(string name)
    {
        _name = name;
    }

    public void Update(string channelName, string videoTitle)
    {
        Console.WriteLine($"[Mobile] {_name} has been notified about a new video: '{videoTitle}' on channel '{channelName}'.");
    }
}

// Subject interface
public interface ISubject
{
    void Subscribe(IObserver observer);
    void Unsubscribe(IObserver observer);
    void NotifySubscribers(string videoTitle);
}

// Concrete Subject (YouTubeChannel)
public class YouTubeChannel : ISubject
{
    private string _channelName;
    private List<IObserver> _observers = new List<IObserver>();

    public YouTubeChannel(string channelName)
    {
        _channelName = channelName;
    }

    public void Subscribe(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifySubscribers(string videoTitle)
    {
        foreach (var observer in _observers)
        {
            observer.Update(_channelName, videoTitle);
        }
    }

    public void UploadVideo(string videoTitle)
    {
        Console.WriteLine($"Channel '{_channelName}' uploaded a new video: '{videoTitle}'.");
        NotifySubscribers(videoTitle);
    }
}

// Usage
class Program
{
    static void Main(string[] args)
    {
        var channel = new YouTubeChannel("TechExplained");

        var webUser1 = new WebUser("Alice");
        var mobileUser1 = new MobileUser("Bob");

        channel.Subscribe(webUser1);
        channel.Subscribe(mobileUser1);

        channel.UploadVideo("Observer Pattern Explained");

        channel.Unsubscribe(webUser1);

        channel.UploadVideo("Design Patterns in C#");
    }
}