namespace Before;
using System;

public abstract class Shape
{
    public abstract void Draw();
    public abstract string ExportToJson();
}

public class Circle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a Circle");
    }

    public override string ExportToJson()
    {
        return "{ \"type\": \"Circle\" }";
    }
}

public class Rectangle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a Rectangle");
    }

    public override string ExportToJson()
    {
        return "{ \"type\": \"Rectangle\" }";
    }
}

public class Client
{
    public static void Main(string[] args)
    {
        Shape[] shapes = { new Circle(), new Rectangle() };

        foreach (var shape in shapes)
        {
            shape.Draw();
            Console.WriteLine(shape.ExportToJson());
        }
    }
}

namespace VisitorPattern;

public interface IVisitor
{
    void Visit(Circle circle);
    void Visit(Rectangle rectangle);
}

public class DrawVisitor : IVisitor
{
    public void Visit(Circle circle)
    {
        System.Console.WriteLine("Draw circle");
    }

    public void Visit(Rectangle rectangle)
    {
        System.Console.WriteLine("Draw rectangle");
    }
}

public class ExportVisitor : IVisitor
{
     public void Visit(Circle circle)
    {
        System.Console.WriteLine("Export circle");
    }

    public void Visit(Rectangle rectangle)
    {
        System.Console.WriteLine("Export rectangle");
    }
}

public abstract class Shape
{
    public abstract void Accept(IVisitor visitor);
}

public class Circle : Shape
{
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
public class Rectangle : Shape
{
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Shape[] shapes = { new Circle(), new Rectangle() };

        var drawVisitor = new DrawVisitor();
        var exportVisitor = new ExportVisitor();

        foreach(var shape in shapes)
        {
            shape.Accept(drawVisitor);
        }

        foreach(var shape in shapes)
        {
            shape.Accept(exportVisitor);
        }
    }
}