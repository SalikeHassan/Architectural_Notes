using System;

public class ComponentA
{
    public void ActionA()
    {
        Console.WriteLine("ComponentA performing ActionA");
    }
}

public class ComponentB
{
    public void ActionB()
    {
        Console.WriteLine("ComponentB performing ActionB");
    }
}

public class Client
{
    private readonly ComponentA _componentA = new ComponentA();
    private readonly ComponentB _componentB = new ComponentB();

    public void Execute()
    {
        // Direct communication between components
        _componentA.ActionA();
        _componentB.ActionB();
    }
}

public class Program
{
    public static void Main()
    {
        var client = new Client();
        client.Execute();
    }
}

//Command and Mediator Pattern

public interface ICommand{}
public interface class IMediator
{
    void Register(string commandName, ICommand command);
    void Execute(string commandName);
}

public class CommandA : ICommand
{
    private readonly ComponentA componentA;
    public Program(ComponentA componentA)
    {
        this.componentA  = componentA;
    }
}

public class CommandB : ICommand
{
    private readonly ComponentB componentB;
    public Program(ComponentB componentB)
    {
        this.componentB = componentB;
    }
}

public class ComponentA
{
    public void ActionA()
    {
        Console.WriteLine("ComponentA performing ActionA");
    }
}

public class ComponentB
{
    public void ActionB()
    {
        Console.WriteLine("ComponentB performing ActionB");
    }
}

public class Mediator : IMediator
{
    private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

    public void Register(string commandName, ICommand command)
    {
        commands[commandName] = command;
    }

    public void Execute(string commandName)
    {
        if (_commands.ContainsKey(commandName))
        {
            _commands[commandName].Execute();
        }
        else
        {
            Console.WriteLine($"No command registered for {commandName}");
        }
    }
}

public class Client
{
    private readonly IMediator _mediator;

    public Client(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Execute()
    {
        _mediator.Execute("ActionA");
        _mediator.Execute("ActionB");
    }
}

public class Program
{
    public static void Main()
    {
        // Components
        var componentA = new ComponentA();
        var componentB = new ComponentB();

        // Mediator
        var mediator = new Mediator();

        // Commands
        var commandA = new CommandA(componentA);
        var commandB = new CommandB(componentB);

        // Register commands with mediator
        mediator.Register("ActionA", commandA);
        mediator.Register("ActionB", commandB);

        // Client executes actions through mediator
        var client = new Client(mediator);
        client.Execute();
    }
}