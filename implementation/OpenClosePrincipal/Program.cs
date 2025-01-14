using OpenClosePrincipal;

namespace OpenClosePrincipal;

public class AreaCalculator
{
    public double CalculateArea(Shape shape)
    {
        if (shape is Rectangle)
        {
            Rectangle rectangle = (Rectangle)shape;
            return rectangle.Width * rectangle.Height;
        }
        else if (shape is Circle)
        {
            Circle circle = (Circle)shape;
            return Math.PI * circle.Radius * circle.Radius;
        }
        throw new ArgumentException("Unknown shape type.");
    }
}

public class Shape {}
public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class Circle : Shape
{
    public double Radius { get; set; }
}

//Open close principal

public class abstract Shape
{
    public abstract double Area();
}

public class Rectangle : Shape
{
    public int Width { get; set; }

    public int Height { get; set; }

    public override double Area()
    {
        return Height * Width;
    }
}

public class AreaCalculator
{
    public double CalculateArea(Shape shape)
    {
        return shape.Area();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
