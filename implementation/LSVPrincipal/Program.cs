namespace LSVPrincipal;

public class Bird
{
    public virtual void Fly() => System.Console.WriteLine("I can FLY!");
}

public class Penguine : Bird
{
    public override void Fly()
    {
       throw new NotSupportedException("Penguins can't fly");
    }
}

// LSV

public abstract class Bird
{
    public virtual void Move() => System.Console.WriteLine("Bird Moves...");
}

public class Sparrow : Bird
{
    public override void Move()
    {
       Fly();
    }

    private void Fly() => System.Console.WriteLine("I can FLY....");
}

public class Penguine : Bird
{
    public override void Move()
    {
       Waddle();
    }

    private void Waddle() => System.Console.WriteLine("I can Waddle....");
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
