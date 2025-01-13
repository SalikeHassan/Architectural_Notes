
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

public class ThinCrustPizzaWithExtraCheese : Pizza
{
    public override string GetDescription()
    {
        return "Thin Crust Pizza with Extra Cheese";
    }

    public override double Cost()
    {
        return 10.50; // Base price
    }
}

public class Client
{
    public void OrderPizza()
    {
        // Thin Crust Pizza with Cheese and Olives
        Pizza myPizza = new ThinCrustPizzaWithExtraCheese();
        Console.WriteLine($"Description: {myPizza.GetDescription()}");
        Console.WriteLine($"Cost: ${myPizza.Cost()}");

        // Thick Crust Pizza with Peppers and Cheese
        Pizza anotherPizza = new ThickCrustPizza();
        Console.WriteLine($"Description: {anotherPizza.GetDescription()}");
        Console.WriteLine($"Cost: ${anotherPizza.Cost()}");
    }
}

// Program Main Method
class Program
{
    static void Main(string[] args)
    {
        var client = new Client();
        client.OrderPizza();

        Console.ReadLine();
    }
}

//Decorator Pattern
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
    private readonly Pizza pizza;
    public Program(Pizza pizza)
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
    public Program(Pizza pizza) : base(pizza){}

    public override string GetDescription()
    {
        return base.GetDescription() + "Cheese";
    }

    public override double Cost()
    {
        return base.Cost() + 2.00;
    }
}

public class Olives : ToppingDecorator
{
    public Program(Pizza pizza) : base(pizza){}

    public override string GetDescription()
    {
        return base.GetDescription() + "Olives";
    }

    public override double Cost()
    {
        return base.Cost() + 3.00;
    }
}

public class Client
{
    public void OrderPizza()
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
    }
}

// Program Main Method
class Program
{
    static void Main(string[] args)
    {
        var client = new Client();
        client.OrderPizza();

        Console.ReadLine();
    }
}