namespace InterpreterPattern;

public interface IBooleanExpression
{
    bool Interpret();
}

public class ConstantExpression : IBooleanExpression
{
    private readonly bool value;

    public ConstantExpression(bool value)
    {
        this.value = value;
    }

    public bool Interpret()
    {
        return value;
    }
}

public class AndExpression : IBooleanExpression
{
    private readonly IBooleanExpression leftExpression;
    private readonly IBooleanExpression rightExpression;

    public AndExpression(IBooleanExpression leftExpression, IBooleanExpression rightExpression)
    {
        this.leftExpression = leftExpression;
        this.rightExpression = rightExpression;
    }

    public bool Interpret()
    {
        return leftExpression.Interpret() && rightExpression.Interpret();
    }
}

public class ORExpression : IBooleanExpression
{
    private readonly IBooleanExpression leftExpression;
    private readonly IBooleanExpression rightExpression;

    public ORExpression(IBooleanExpression leftExpression,IBooleanExpression rightExpression)
    {
        this.leftExpression = leftExpression;
        this.rightExpression = rightExpression;
    }

    public bool Interpret()
    {
        return leftExpression.Interpret() || rightExpression.Interpret();
    }
}

class Program
{
    static void Main(string[] args)
    {
       var trueExpression = new ConstantExpression(true);
       var falseExpression = new ConstantExpression(false);

       var andExpression = new AndExpression(trueExpression,falseExpression).Interpret();
       var orExpression = new ORExpression(trueExpression,falseExpression).Interpret();

       System.Console.WriteLine($"{andExpression}, {orExpression}");
    }
}
