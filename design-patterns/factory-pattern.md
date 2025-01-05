![image](https://github.com/user-attachments/assets/c37c2890-26b3-47cf-bb8a-c86673cb5b83)

![image](https://github.com/user-attachments/assets/9258bea3-6b89-47aa-b158-7cbb91aa639b)

![image](https://github.com/user-attachments/assets/00055e2a-5f88-4ba9-9c21-ea4e15adcc2d)

![image](https://github.com/user-attachments/assets/b3b9c726-4fec-4bfd-9477-fe871858556b)

![image](https://github.com/user-attachments/assets/209b8f1a-ea53-41e8-966d-0084f8f9ae39)

```csharp
public class Animal
{
    public virtual void Speak() { }
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
}

public class Cat : Animal
{
    public override void Speak() => Console.WriteLine("Meow!");
}

public class Chicken : Animal
{
    public override void Speak() => Console.WriteLine("Cluck!");
}

public class AnimalCreator
{
    public Animal CreateAnimal(string type)
    {
        switch (type.ToLower())
        {
            case "dog":
                return new Dog();
            case "cat":
                return new Cat();
            case "chicken":
                return new Chicken();
            default:
                throw new ArgumentException("Unknown animal type", nameof(type));
        }
    }
}

// Usage
AnimalCreator creator = new AnimalCreator();
Animal myAnimal = creator.CreateAnimal("dog");
myAnimal.Speak(); // Outputs: "Woof!"
```
```csharp
public abstract class Animal
{
    public abstract void Speak();
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
}

public class Cat : Animal
{
    public override void Speak() => Console.WriteLine("Meow!");
}

public class Chicken : Animal
{
    public override void Speak() => Console.WriteLine("Cluck!");
}

public abstract class AnimalFactory
{
    public abstract Animal CreateAnimal();
}

public class DogFactory : AnimalFactory
{
    public override Animal CreateAnimal() => new Dog();
}

public class CatFactory : AnimalFactory
{
    public override Animal CreateAnimal() => new Cat();
}

public class ChickenFactory : AnimalFactory
{
    public override Animal CreateAnimal() => new Chicken();
}

public class AnimalCreator
{
    public Animal CreateAnimal(string type)
    {
        AnimalFactory factory = GetFactory(type);
        return factory.CreateAnimal();
    }

    private AnimalFactory GetFactory(string type)
    {
        switch (type.ToLower())
        {
            case "dog": return new DogFactory();
            case "cat": return new CatFactory();
            case "chicken": return new ChickenFactory();
            default: throw new ArgumentException("Unknown animal type", nameof(type));
        }
    }
}

// Usage
AnimalCreator creator = new AnimalCreator();
Animal myAnimal = creator.CreateAnimal("dog");
myAnimal.Speak(); // Outputs: "Woof!"
```
