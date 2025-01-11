using System;

public class Car
{
    public string Type { get; private set; }
    public bool HasFrontDoor { get; private set; }
    public bool HasBackDoor { get; private set; }
    public bool HasMoonRoof { get; private set; }

    public Car(string type, bool hasFrontDoor, bool hasBackDoor, bool hasMoonRoof)
    {
        Type = type;
        HasFrontDoor = hasFrontDoor;
        HasBackDoor = hasBackDoor;
        HasMoonRoof = hasMoonRoof;
    }

    public void DisplayFeatures()
    {
        Console.WriteLine($"Car Type: {Type}");
        Console.WriteLine($"Features: {(HasFrontDoor ? "Front Door, " : "")}" +
                          $"{(HasBackDoor ? "Back Door, " : "")}" +
                          $"{(HasMoonRoof ? "Moon Roof" : "No Moon Roof")}");
    }
}

// Client Class
class Program
{
    static void Main(string[] args)
    {
        // Create a Sedan with all features
        Car sedan = new Car("Sedan", true, true, true);
        sedan.DisplayFeatures();

        // Create an SUV without Moon Roof
        Car suv = new Car("SUV", true, true, false);
        suv.DisplayFeatures();

        Console.ReadLine();
    }
}

//Builder Pattern
public class Car
{
    public string Type { get; private set; }
    public bool HasFrontDoor { get; private set; }
    public bool HasBackDoor { get; private set; }
    public bool HasMoonRoof { get; private set; }

    public Car(string type, bool hasFrontDoor, bool hasBackDoor, bool hasMoonRoof)
    {
        Type = type;
        HasFrontDoor = hasFrontDoor;
        HasBackDoor = hasBackDoor;
        HasMoonRoof = hasMoonRoof;
    }

    public void DisplayFeatures()
    {
        Console.WriteLine($"Car Type: {Type}");
        Console.WriteLine($"Features: {(HasFrontDoor ? "Front Door, " : "")}" +
                          $"{(HasBackDoor ? "Back Door, " : "")}" +
                          $"{(HasMoonRoof ? "Moon Roof" : "No Moon Roof")}");
    }
}

public interface ICarBuilder
{
    bool AddFrontDoor();
    bool AddBackDoor();

    bool AddMoonRoof();

    Car Build();
}


public class SedanBuilder : ICarBuilder
{
    private bool hasFrontDoor;
    private bool hasBackDoor;
    private bool hasMoonRoof;

    public ICarBuilder AddFrontDoor()
    {
        hasFrontDoor = true;
        return this;
    }

    public ICarBuilder AddBackDoor()
    {
        hasBackDoor = true;
        return this;
    }

    public ICarBuilder AddMoonRoof()
    {
        hasMoonRoof = false;
        return this;
    }

    public Car Build()
    {
        return new Car("Sedan", hasFrontDoor, hasBackDoor, hasMoonRoof);
    }
}

public class SuvBuilder : ICarBuilder
{
    private bool hasFrontDoor;
    private bool hasBackDoor;
    private bool hasMoonRoof;

    public ICarBuilder AddFrontDoor()
    {
        hasFrontDoor = true;
        return this;
    }

    public ICarBuilder AddBackDoor()
    {
        hasBackDoor = true;
        return this;
    }

    public ICarBuilder AddMoonRoof()
    {
        hasMoonRoof = true;
        return this;
    }

    public Car Build()
    {
        return new Car("Sedan", hasFrontDoor, hasBackDoor, hasMoonRoof);
    }
}

public class CarAssemblyManager
{
    private readonly ICarBuilder builder;
    public Program(ICarBuilder builder)
    {
        builder = builder;
    }

    public Car Construct()
    {
        Car car = builder.AddBackDoor()
                    .AddBackDoor()
                    .AddMoonRoof()
                    .Build();
        
        return car;
    }
}

// Usage in Main Method
class Program
{
    static void Main(string[] args)
    {
        // Using the Builder Pattern with renamed "CarAssemblyManager"
        CarAssemblyManager manager = new CarAssemblyManager();

        // Build a Sedan
        ICarBuilder sedanBuilder = new SedanBuilder();
        Car sedan = manager.Construct(sedanBuilder);
        sedan.DisplayFeatures();

        // Build an SUV without Moon Roof
        ICarBuilder suvBuilder = new SUVBuilder();
        suvBuilder.AddFrontDoor()
                  .AddBackDoor();
        Car suv = suvBuilder.Build();
        suv.DisplayFeatures();

        Console.ReadLine();
    }
}