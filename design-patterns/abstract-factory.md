![image](https://github.com/user-attachments/assets/9b018b1c-50cd-4dcf-8f07-6ad6ae559509)

![image](https://github.com/user-attachments/assets/f9ab262e-d14c-40c6-85c1-0c962b3d1ef6)

![image](https://github.com/user-attachments/assets/f7983ab0-c64c-4d64-9ec9-5c24201c7961)

```csharp
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

using System;

class Program
{
    static void Main(string[] args)
    {
        // Create Modern Furniture
        IFurnitureFactory modernFactory = new ModernFurnitureFactory();
        IChair modernChair = modernFactory.CreateChair();

        modernChair.SitOn();
        Console.WriteLine();

        // Create Victorian Furniture
        IFurnitureFactory victorianFactory = new VictorianFurnitureFactory();
        IChair victorianChair = victorianFactory.CreateChair();

        victorianChair.SitOn();
        Console.ReadLine();
    }
}
```
