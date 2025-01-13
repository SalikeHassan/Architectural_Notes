using System;

// Product Interfaces
public interface IChair
{
    void SitOn();
}

// Modern Chair
public class ModernChair : IChair
{
    public void SitOn()
    {
        Console.WriteLine("Sitting on a modern chair.");
    }
}

// Victorian Chair
public class VictorianChair : IChair
{
    public void SitOn()
    {
        Console.WriteLine("Sitting on a Victorian chair.");
    }
}

// Client Service (Before Abstract Factory)
public class ClientService
{
    private IChair _chair;

    public ClientService(string chairType)
    {
        if (chairType == "Modern")
        {
            _chair = new ModernChair();
        }
        else if (chairType == "Victorian")
        {
            _chair = new VictorianChair();
        }
        else
        {
            throw new ArgumentException("Invalid chair type");
        }
    }

    public void UseChair()
    {
        _chair.SitOn();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Client decides chair type
        ClientService modernService = new ClientService("Modern");
        modernService.UseChair();

        ClientService victorianService = new ClientService("Victorian");
        victorianService.UseChair();
    }
}

//Abstract Factory Pattern
using System;

// Abstract Product A
public interface IChair
{
    void SitOn();
}

// Modern Furniture Products
public class ModernChair : IChair
{
    public void SitOn()
    {
        Console.WriteLine("Sitting on a modern chair.");
    }
}

// Victorian Furniture Products
public class VictorianChair : IChair
{
    public void SitOn()
    {
        Console.WriteLine("Sitting on a Victorian chair.");
    }
}

// Abstract Factory Interface
public interface IFurnitureFactory
{
    IChair CreateChair();
}

// Modern Furniture Factory
public class ModernFurnitureFactory : IFurnitureFactory
{
    public IChair CreateChair()
    {
        return new ModernChair();
    }
}

// Victorian Furniture Factory
public class VictorianFurnitureFactory : IFurnitureFactory
{
    public IChair CreateChair()
    {
        return new VictorianChair();
    }
}

// Client Service (After Abstract Factory)
public class ClientService
{
    private readonly IChair _chair;

    public ClientService(IFurnitureFactory factory)
    {
        // Factory creates the chair
        _chair = factory.CreateChair();
    }

    public void UseChair()
    {
        _chair.SitOn();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create Modern Furniture
        IFurnitureFactory modernFactory = new ModernFurnitureFactory();
        ClientService modernService = new ClientService(modernFactory);
        modernService.UseChair();

        Console.WriteLine();

        // Create Victorian Furniture
        IFurnitureFactory victorianFactory = new VictorianFurnitureFactory();
        ClientService victorianService = new ClientService(victorianFactory);
        victorianService.UseChair();

        Console.ReadLine();
    }
}