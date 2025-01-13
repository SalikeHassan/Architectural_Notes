// Individual To-Do Item
public class ToDoItem
{
    private string _name;
    private bool _isComplete;

    public ToDoItem(string name)
    {
        _name = name;
        _isComplete = false;
    }

    public void MarkComplete()
    {
        _isComplete = true;
    }

    public bool IsComplete()
    {
        return _isComplete;
    }

    public void Display()
    {
        Console.WriteLine($"To-Do: {_name}, Completed: {_isComplete}");
    }
}

// Checklist for Group of To-Do Items
public class Checklist
{
    private string _name;
    private List<ToDoItem> _items = new List<ToDoItem>();

    public Checklist(string name)
    {
        _name = name;
    }

    public void AddItem(ToDoItem item)
    {
        _items.Add(item);
    }

    public bool IsComplete()
    {
        foreach (var item in _items)
        {
            if (!item.IsComplete())
                return false;
        }
        return true;
    }

    public void Display()
    {
        Console.WriteLine($"Checklist: {_name}");
        foreach (var item in _items)
        {
            item.Display();
        }
    }
}

// Client
public class Client
{
    public static void Main()
    {
        // Create To-Do Items
        ToDoItem item1 = new ToDoItem("Buy groceries");
        ToDoItem item2 = new ToDoItem("Pay bills");

        // Create Checklist and Add Items
        Checklist checklist = new Checklist("Daily Tasks");
        checklist.AddItem(item1);
        checklist.AddItem(item2);

        // Mark an item complete
        item1.MarkComplete();

        // Display checklist and completion status
        checklist.Display();
        Console.WriteLine($"Checklist Complete: {checklist.IsComplete()}");
    }
}

//Composite Pattern
public interface IComponent
{
    void IsComplete();
    void Display();
}

public class ToDoItem: IComponent
{
    private readonly string name;
    private book isComplete;
    public Program(string name)
    {
        this.name= name;
    }

    public void MarkComplete() => this.isComplete = true;
   

    public bool IsComplete() => return isComplete;

    public void Display() => System.Console.WriteLine($"{name} : {isComplete}");
}

public class Checklist: IComponent
{
    private readonly string name;

    private List<IComponent> todoItems = new ();
    private book isComplete;
    public Program(string name)
    {
        this.name= name;
    }

    public void AddComponent(IComponent todo) => todoItems.Add(todo);

    public bool IsComplete() 
    {
        foreach (var component in _components)
        {
            if (!component.IsComplete())
                return false;
        }
        return true;
    }
    public void Display() => System.Console.WriteLine($"{name} : {isComplete}");
}

public class Client
{
    public static void Main()
    {
        // Create To-Do Items
        IComponent item1 = new ToDoItem("Buy groceries");
        IComponent item2 = new ToDoItem("Pay bills");

        // Create Checklist and Add Items
        IComponent checklist = new Checklist("Daily Tasks");
        ((Checklist)checklist).AddComponent(item1);
        ((Checklist)checklist).AddComponent(item2);

        // Nested Checklist
        IComponent subChecklist = new Checklist("Weekend Tasks");
        ((Checklist)subChecklist).AddComponent(new ToDoItem("Clean house"));
        ((Checklist)checklist).AddComponent(subChecklist);

        // Mark some items complete
        ((ToDoItem)item1).MarkComplete();

        // Display checklist and completion status
        checklist.Display();
        Console.WriteLine($"Checklist Complete: {checklist.IsComplete()}");
    }
}