public interface IDuck
{
    void Fly();
    void Quack();
}

public class MallardDuck : IDuck
{
    public void Fly()=>System.Console.WriteLine("Duck Fly");
    public void Quack() => System.Console.WriteLine("Quack");
}

public class Drone
{
    public void Beep() => System.Console.WriteLine("Beep beep!");
    public void SpinRotors() => System.Console.WriteLine("Spin");
    public void TakeOff() => System.Console.WriteLine("TakeOff");
}

public class Client
{
    public static void Main()
    {
        Console.WriteLine("=== Non-Adapter Implementation ===");
        IDuck duck = new MallardDuck();
        duck.Quack();
        duck.Fly();

        IDuck drone = new Drone(); //Compile time error
    }
}

// Adapter using interface
public interface IDuck
{
    void Fly();
    void Quack();
}

public class MallardDuck : IDuck
{
    public void Fly()=>System.Console.WriteLine("Duck Fly");
    public void Quack() => System.Console.WriteLine("Quack");
}

public class Drone
{
    public void Beep() => System.Console.WriteLine("Beep beep!");
    public void SpinRotors() => System.Console.WriteLine("Spin");
    public void TakeOff() => System.Console.WriteLine("TakeOff");
}

public class DroneAdapter : IDuck
{
    private readonly Drone drone;
    public Program(Drone drone)
    {
        this.drone = drone;
    }
    public void Quack()
    {
        drone.Beep();
    }

    public void Fly()
    {
        drone.SpinRotors();
        drone.TakeOff();
    }
}

public class Client
{
    public static void Main()
    {
        Console.WriteLine("\n=== Adapter Implementation (Interface-Based) ===");
        Drone drone = new Drone();
        IDuck droneAdapter = new DroneAdapter(drone);
        droneAdapter.Quack();
    }
}

// Adapter pattern using composition
