using System;

public class RequestHandler
{
    public void HandleRequest(string request)
    {
        if (request == "Low")
        {
            Console.WriteLine("Low-level handler processed the request.");
        }
        else if (request == "Medium")
        {
            Console.WriteLine("Medium-level handler processed the request.");
        }
        else if (request == "High")
        {
            Console.WriteLine("High-level handler processed the request.");
        }
        else
        {
            Console.WriteLine("Request not handled.");
        }
    }
}

// Client Code
public class Client
{
    public static void Main(string[] args)
    {
        var handler = new RequestHandler();

        // Example requests
        Console.WriteLine("Sending request 'Low':");
        handler.HandleRequest("Low");

        Console.WriteLine("\nSending request 'Medium':");
        handler.HandleRequest("Medium");

        Console.WriteLine("\nSending request 'High':");
        handler.HandleRequest("High");

        Console.WriteLine("\nSending request 'Unknown':");
        handler.HandleRequest("Unknown");
    }
}

//Chain of Responsibility

public abstract class Handler
{
    protected Handler next;
    public void SetNext(Handler handler)
    {
        this.next = handler;
    }

    public abstract void Handle(string request);
}

public class LowLevelHandler : Handler
{
    public override void Handle(string request)
    {
            if(request == "low")
            {
                System.Console.WriteLine("Handled Low Level Handler");
            }

            else if(next is not null)
            {
                next.Handle(request);
            }

            else
            {
                System.Console.WriteLine("Not Handled");
            }
    }
}

public class MidLevelHandler : Handler
{
    public override void Handle(string request)
    {
            if(request == "mid")
            {
                System.Console.WriteLine("Handled Low Level Handler");
            }

            else if(next is not null)
            {
                next.Handle(request);
            }

            else
            {
                System.Console.WriteLine("Not Handled");
            }
    }
}

public class HighLevelHandler : Handler
{
    public override void Handle(string request)
    {
            if(request == "high")
            {
                System.Console.WriteLine("Handled Low Level Handler");
            }

            else if(next is not null)
            {
                next.Handle(request);
            }

            else
            {
                System.Console.WriteLine("Not Handled");
            }
    }
}

public class Client
{
    public static void Main(string[] args)
    {
        // Create handlers
        var lowHandler = new LowLevelHandler();
        var mediumHandler = new MidLevelHandler();
        var highHandler = new HighLevelHandler();

        // Set the chain of responsibility
        lowHandler.SetNext(mediumHandler);
        mediumHandler.SetNext(highHandler);

        // Example requests
        Console.WriteLine("Sending request 'Low':");
        lowHandler.Handle("Low");

        Console.WriteLine("\nSending request 'Medium':");
        lowHandler.Handle("Medium");

        Console.WriteLine("\nSending request 'High':");
        lowHandler.Handle("High");

        Console.WriteLine("\nSending request 'Unknown':");
        lowHandler.Handle("Unknown");
    }
}