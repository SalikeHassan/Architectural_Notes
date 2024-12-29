<img width="1006" alt="image" src="https://github.com/user-attachments/assets/fbc0042f-e230-4c0a-96d0-6785ed82de99" />


![image](https://github.com/user-attachments/assets/45087d93-906c-489c-907a-fb5e40d9823c)

```csharp
int[] numbers = { 1, 2, 3, 4 };
foreach (var num in numbers) { Console.WriteLine(num); }

public interface IIterator<T>
{
    bool HasNext();
    T Next();
}

public class ReverseIterator<T> : IIterator<T>
{
    private readonly T[] items;
    private int position;

    public ReverseIterator(T[] items)
    {
        this.items = items;
        this.position = items.Length - 1;
    }

    public bool HasNext() => position >= 0;

    public T Next() => items[position--];
}
```
