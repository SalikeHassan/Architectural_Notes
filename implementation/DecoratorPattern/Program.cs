// +-------------+
// |    Pizza    |
// +-------------+
// | +GetDescription() |
// | +Cost()            |
// +-------------+
//      ^
//      |
// +-------------+   +--------------+
// | ThinCrustPizza | | ThickCrustPizza |
// +-------------+   +--------------+
// | +hasCheese     | | +hasCheese      |
// | +hasOlives     | | +hasPeppers     |
// | +AddCheese()   | | +AddCheese()    |
// | +AddOlives()   | | +AddPeppers()   |
// +-------------+   +--------------+

public abstract class Pizza
{
    public abstract string GetDescription();
    public abstract double Cost();
}

public class ThinCrustPizza : Pizza
{
    private bool hasCheese = false;
    private bool hasOlives = false;

    public override string GetDescription()
    {
        string desc = "Thin Crust Pizza";
        if (hasCheese) desc += ", Cheese";
        if (hasOlives) desc += ", Olives";
        return desc;
    }

    public override double Cost()
    {
        double cost = 8.00; // Base price
        if (hasCheese) cost += 1.50;
        if (hasOlives) cost += 1.00;
        return cost;
    }

    public void AddCheese() => hasCheese = true;
    public void AddOlives() => hasOlives = true;
}

public abstract class Pizza
{
    public abstract string GetDescription();
    public abstract double cost();
}

public class ThinCrustPizza : Pizza
{
    public override string GetDescription()
    {
        return "Thin crust pizza";
    }
}
