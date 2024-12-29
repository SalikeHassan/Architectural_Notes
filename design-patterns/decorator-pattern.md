![image](https://github.com/user-attachments/assets/d70db795-8ab2-4ad0-9240-6a0c4262f2ba)

<img width="968" alt="image" src="https://github.com/user-attachments/assets/a3101f76-40eb-4eb0-8ff1-67a790cc922d" />

![image](https://github.com/user-attachments/assets/2652d5b5-9391-4e34-be5f-ec4edab9e86c)

![image](https://github.com/user-attachments/assets/d1c57e33-c7aa-42c1-9639-b9c7c959d7f6)

![image](https://github.com/user-attachments/assets/606c4c8b-bae5-4d06-9461-65ae2a4810c7)

```csharp
public abstract class Pizza
{
    public abstract string GetDescription();
    public abstract double Cost();
}

public class ThinCrustPizza : Pizza
{
    public override string GetDescription()
    {
        return "Thin Crust Pizza";
    }

    public override double Cost()
    {
        return 8.00; // Base price
    }
}

public class ThickCrustPizza : Pizza
{
    public override string GetDescription()
    {
        return "Thick Crust Pizza";
    }

    public override double Cost()
    {
        return 10.00; // Base price
    }
}

public abstract class ToppingDecorator : Pizza
{
    protected Pizza pizza;

    public ToppingDecorator(Pizza pizza)
    {
        this.pizza = pizza;
    }

    public override string GetDescription()
    {
        return pizza.GetDescription();
    }

    public override double Cost()
    {
        return pizza.Cost();
    }
}

public class Cheese : ToppingDecorator
{
    public Cheese(Pizza pizza) : base(pizza) { }

    public override string GetDescription()
    {
        return pizza.GetDescription() + ", Cheese";
    }

    public override double Cost()
    {
        return pizza.Cost() + 1.50; // Cost of Cheese
    }
}

public class Olives : ToppingDecorator
{
    public Olives(Pizza pizza) : base(pizza) { }

    public override string GetDescription()
    {
        return pizza.GetDescription() + ", Olives";
    }

    public override double Cost()
    {
        return pizza.Cost() + 1.00; // Cost of Olives
    }
}

public class Peppers : ToppingDecorator
{
    public Peppers(Pizza pizza) : base(pizza) { }

    public override string GetDescription()
    {
        return pizza.GetDescription() + ", Peppers";
    }

    public override double Cost()
    {
        return pizza.Cost() + 0.75; // Cost of Peppers
    }
}

using System;

class Program
{
    static void Main(string[] args)
    {
        // Create a ThinCrustPizza with Cheese and Olives
        Pizza myPizza = new ThinCrustPizza();
        myPizza = new Cheese(myPizza);
        myPizza = new Olives(myPizza);

        Console.WriteLine($"Description: {myPizza.GetDescription()}");
        Console.WriteLine($"Cost: ${myPizza.Cost()}");

        // Create a ThickCrustPizza with Peppers and Cheese
        Pizza anotherPizza = new ThickCrustPizza();
        anotherPizza = new Peppers(anotherPizza);
        anotherPizza = new Cheese(anotherPizza);

        Console.WriteLine($"Description: {anotherPizza.GetDescription()}");
        Console.WriteLine($"Cost: ${anotherPizza.Cost()}");

        Console.ReadLine();
    }
}

```

<img width="829" alt="image" src="https://github.com/user-attachments/assets/f4369ac6-632d-4e89-b70f-31cdc5a2fd5d" />
