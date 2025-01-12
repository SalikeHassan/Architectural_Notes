// Base Shape Class
public abstract class Shape
{
    public abstract void Draw();
}

// Concrete Shape Classes with Color
public class RedRectangle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a Red Rectangle");
    }
}

public class BlueRectangle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a Blue Rectangle");
    }
}

public class RedSquare : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a Red Square");
    }
}

public class BlueSquare : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a Blue Square");
    }
}

// Client
public class Client
{
    public static void Main()
    {
        Shape shape1 = new RedRectangle();
        Shape shape2 = new BlueSquare();

        shape1.Draw();
        shape2.Draw();
    }
}

// Bridge Pattern
public interface IColor
{
    void Apply();
}

public class RedColor: IColor
{
    public void Apply()=> System.Console.WriteLine("Red");
}

public class BlueColor: IColor
{
    public void Apply()=> System.Console.WriteLine("Blue");
}

public abstract class Shape
{
    protected IColor color;
    public Program(IColor color)
    {
        this.color = color;
    }
    public abstract void Draw();
}

public class Rectangle : Shape
{
    public Rectangle(IColor color) : base(color) { }

    public override void Draw() 
    {
        System.Console.WriteLine("Rectangle");
        base.ApplyColor();
    } 
}

public class Sqaure : Shape
{
    public Sqaure(IColor color) : base(color) { }

    public override void Draw() 
    {
        System.Console.WriteLine("Rectangle");
        base.ApplyColor();
    } 
}

public class Client
{
    public static void Main()
    {
        Shape rectangle = new Rectangle(new RedColor());
        rectangle.Draw();

        Shape sqaure = new Sqaure(new BlueColor());
        sqaure.Draw();
    }
}