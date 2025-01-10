public interface ICameraApp
{
    void OpenCamera();
}

public class BasicApp : ICameraApp
{
    public void OpenCamera() => System.Console.WriteLine("Basic Camera");
}

public class PlusApp : ICameraApp
{
    public void OpenCamera() => System.Console.WriteLine("Plus Camera");
}

public class ProApp : ICameraApp
{
    public void OpenCamera() => System.Console.WriteLine("Pro Camera");
}

public class ClientSerice
{
    private readonly ICameraApp cameraApp
    
    public ClientSerice(string appType)
    {
        switch(appType)
        {
            case "Basic":
            return new BasicApp();
            
            case "Plus":
            return new PlusApp();

            case "Pro":
            return new ProApp();

            default:
            throw new ArgumentException();
        }
    }

    public void PerformCameraOperation()
    {
        cameraApp.OpenCamera();
    }
}

//Factory pattern
public interface ICameraApp
{
    void OpenCamera();
}

public class BasicApp : ICameraApp
{
    public void OpenCamera()
    {
        Console.WriteLine("Basic camera features enabled.");
    }
}

public class PlusApp : ICameraApp
{
    public void OpenCamera()
    {
        Console.WriteLine("Advanced camera features enabled.");
    }
}

public class ProApp : ICameraApp
{
    public void OpenCamera()
    {
        Console.WriteLine("Professional camera features enabled.");
    }
}

public static class CameraFactory
{
    public static ICameraApp CreateCameraApp(string appType)
    {
        return appType switch
        {
                "Basic" => new BasicApp(),
                "Plus" => new PlusApp(),
                "Pro" => new ProApp(),
                _=> throw new ArgumentException()
        };
    }
}

public class ClientSerice
{
    private readonly ICameraApp cameraApp
    
    public ClientSerice(string appType)
    {
        cameraApp = CameraFactory.CreateCameraApp(appType);
    }

    public void PerformCameraOperation()
    {
        cameraApp.OpenCamera();
    }
}