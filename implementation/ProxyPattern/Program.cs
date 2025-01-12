// Expensive Resource Class
public class ExpensiveResource
{
    public ExpensiveResource()
    {
        Console.WriteLine("ExpensiveResource initialized.");
        // Simulating expensive operation
        System.Threading.Thread.Sleep(2000);
    }

    public void PerformOperation()
    {
        Console.WriteLine("Performing an operation on ExpensiveResource.");
    }
}

// Client
public class Client
{
    public static void Main()
    {
        Console.WriteLine("Client requesting ExpensiveResource...");
        ExpensiveResource resource = new ExpensiveResource();
        resource.PerformOperation();
    }
}

//Proxy Pattern
// Resource Interface
// Resource Interface
public interface IResource
{
    void PerformOperation();
}

// Real Subject (Actual Expensive Resource)
public class ExpensiveResource : IResource
{
    public ExpensiveResource()
    {
        Console.WriteLine("ExpensiveResource initialized.");
        // Simulating expensive operation
        System.Threading.Thread.Sleep(2000);
    }

    public void PerformOperation()
    {
        Console.WriteLine("Performing an operation on ExpensiveResource.");
    }
}

// Proxy Class
public class ExpensiveResourceProxy : IResource
{
    private ExpensiveResource _resource; // Lazily initialized

    public void PerformOperation()
    {
        if (_resource == null)
        {
            Console.WriteLine("Initializing ExpensiveResource via Proxy...");
            _resource = new ExpensiveResource();
        }
        _resource.PerformOperation();
    }
}

// Client
public class Client
{
    public static void Main()
    {
        Console.WriteLine("Client requesting resource via Proxy...");
        IResource resource = new ExpensiveResourceProxy(); // Proxy created
        resource.PerformOperation(); // First call initializes the real object

        Console.WriteLine("Client performing another operation...");
        resource.PerformOperation(); // Reuses the existing object
    }
}