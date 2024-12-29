**The Observer pattern exemplifies the principle of loose coupling, where objects interact with each other without becoming too dependent on one another, enhancing flexibility**

**The Observer Pattern allows one object (the Subject) to notify multiple other objects (Observers) about changes in its state, so they can react to those changes automatically.**

**A better analogy would be a “news subscription” or “YouTube notifications.” Subscribers (observers) are informed whenever new content (state change) is available, but they don’t need to actively check for updates.**

![image](https://github.com/user-attachments/assets/db916b4d-891c-4993-9ea6-e2fe8c3c3a5f)

![image](https://github.com/user-attachments/assets/8044b2ff-7ab6-4f2b-9689-a6a662571f35)

![image](https://github.com/user-attachments/assets/833364e8-1ca1-4cfb-9280-bc1a1555ad0e)

```csharp
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

public interface IObserver
{
    void Update(float temperature, float windSpeed, float pressure);
}
using System.Collections.Generic;

public class WeatherStation : ISubject
{
    private List<IObserver> observers;
    private float temperature;
    private float windSpeed;
    private float pressure;

    public WeatherStation()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature, windSpeed, pressure);
        }
    }

    public void SetMeasurements(float temperature, float windSpeed, float pressure)
    {
        this.temperature = temperature;
        this.windSpeed = windSpeed;
        this.pressure = pressure;
        NotifyObservers();
    }
}

public class UserInterface : IObserver
{
    private WeatherStation weatherStation;

    public UserInterface(WeatherStation weatherStation)
    {
        this.weatherStation = weatherStation;
        weatherStation.RegisterObserver(this);
    }

    public void Update(float temperature, float windSpeed, float pressure)
    {
        Display(temperature, windSpeed, pressure);
    }

    public void Display(float temperature, float windSpeed, float pressure)
    {
        Console.WriteLine($"User Interface Display: Temp={temperature}, WindSpeed={windSpeed}, Pressure={pressure}");
    }
}

public class Logger : IObserver
{
    private WeatherStation weatherStation;

    public Logger(WeatherStation weatherStation)
    {
        this.weatherStation = weatherStation;
        weatherStation.RegisterObserver(this);
    }

    public void Update(float temperature, float windSpeed, float pressure)
    {
        Log(temperature, windSpeed, pressure);
    }

    public void Log(float temperature, float windSpeed, float pressure)
    {
        Console.WriteLine($"Logger: Temp={temperature}, WindSpeed={windSpeed}, Pressure={pressure}");
    }
}

using System;

class Program
{
    static void Main(string[] args)
    {
        WeatherStation weatherStation = new WeatherStation();

        UserInterface ui = new UserInterface(weatherStation);
        Logger logger = new Logger(weatherStation);
        AlertSystem alert = new AlertSystem(weatherStation);

        weatherStation.SetMeasurements(30, 15, 1013);
        weatherStation.SetMeasurements(40, 10, 1005);

        Console.ReadLine();
    }
}
```
