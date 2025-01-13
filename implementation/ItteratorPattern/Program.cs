namespace ItteratorPattern;
public interface IIterator<T>
{
    T Current {get;}
    bool MoveNext();
    void Reset();
}

public interface IIterableCollection<T>
{
    IIterator<T> CreateIterator();
}

public class CustomCollection : IIterableCollection<string>
{
    private readonly List<string> items = new List<string>();

    public int Count => items.Count;

    public void Add(string item)
    {
        items.Add(item);
    }

    public IIterator<string> CreateIterator()
    {
        return new CustomIterator(this);
    }

    public string GetAt(int index)
    {
        if(index >=0 && index < Count)
        {
            return items[index];
        }

        throw new IndexOutOfRangeException("Index out of range");
    }
    private class CustomIterator: IIterator<string>
    {
        private readonly CustomCollection collection;
        private int currentIndex = -1;

        public CustomIterator(CustomCollection collection)
        {
            collection = collection;
        }

        public string Current => collection.GetAt(currentIndex);

        public bool MoveNext()
        {
            if(currentIndex +1 < collection.Count)
            {
                return true;
            }

            return false;
        } 

        public void Reset()
        {
            currentIndex = -1;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var collection = new CustomCollection();
        collection.Add("item1");
        collection.Add("item2");
        collection.Add("item3");

        var iterator = collection.CreateIterator();

        while(iterator.MoveNext())
        {
            System.Console.WriteLine(iterator.Current);
        }
    }
}