// Before Strategy Pattern

public class PhoneCameraApp
{
    public virtual void TakePhoto()
    {
        Console.WriteLine("Clicked");
    }

    public virtual void EditPhoto()
    {
        System.Console.WriteLine("Edit");
    }

    public virtual void SavePhoto()
    {
        System.Console.WriteLine("Saved");
    }

    public virtual void Share()
    {
         Console.WriteLine("Default option");
    }
}

public class BasicCameraApp : PhoneCameraApp
{
    public override void SharePhoto()
    {
        Console.WriteLine("Email Sharing");
    }
}

public class CameraPlusApp : PhoneCameraApp
{
    public override void SharePhoto()
    {
        System.Console.WriteLine("Email");
        System.Console.WriteLine("Whatsapp");
        System.Console.WriteLine("Instagram");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Basic Camera App:");
        var basicApp = new BasicCameraApp();
        basicApp.TakePhoto();
        basicApp.EditPhoto();
        basicApp.SavePhoto();
        basicApp.SharePhoto();

        Console.WriteLine("\nCamera Plus App:");
        var plusApp = new CameraPlusApp();
        plusApp.TakePhoto();
        plusApp.EditPhoto();
        plusApp.SavePhoto();
        plusApp.SharePhoto();
    }
}

// Strategy Pattern
public interface IShareStrategy
{
    void Share();
}

public class EmailShareStrategy : IShareStrategy
{
    public void Share() => Console.WriteLine("Email");
}

public class InstagramShareStrategy : IShareStrategy
{
    public void Share() => Console.WriteLine("Instagram");
}

public class WhatsAppShareStrategy : IShareStrategy
{
    public void Share() => Console.WriteLine("WhatsApp");
}

public class TwitterShare : IShareStrategy
{
    public void Share() => Console.WriteLine("Twitter");
}

public class TextShare : IShareStrategy
{
    public void Share() => Console.WriteLine("Text");
}

public abstract class PhoneCameraApp
{
    protected IShareStrategy shareStrategy;

    public void SetShareStrategy(IShareStrategy shareStrategy)
    {
        this.shareStrategy = shareStrategy;
    }

    public void SharePhoto()
    {
        shareStrategy.Share();
    }

    public abstract void TakePhoto();
    public abstract void EditPhoto();
    public abstract void SavePhoto();
}

public class BasicCameraApp : PhoneCameraApp
{
    public BasicCameraApp()
    {
        SetShareStrategy(new EmailShareStrategy()); // Default sharing strategy
    }

    public override void TakePhoto() => Console.WriteLine("Basic: Photo taken");
    public override void EditPhoto() => Console.WriteLine("Basic: Photo edited");
    public override void SavePhoto() => Console.WriteLine("Basic: Photo saved");
}

public class CameraPlusApp : PhoneCameraApp
{
    public CameraPlusApp()
    {
        // No default strategy, can be set dynamically
    }

    public override void TakePhoto() => Console.WriteLine("Plus: Photo taken");
    public override void EditPhoto() => Console.WriteLine("Plus: Photo edited");
    public override void SavePhoto() => Console.WriteLine("Plus: Photo saved");
}
