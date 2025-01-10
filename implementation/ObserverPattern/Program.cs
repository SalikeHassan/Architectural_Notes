// +----------------+
// |  WeatherData   |
// +----------------+
// | +Temperature   |
// | +Humidity      |
// +----------------+

public class WeatherData
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }

    public void UpdateWeather(double temp, double humidity)
    {
        Temperature = temp;
        Humidity = humidity;
        // Clients would need to manually check for changes
    }
}

// +----------------+       +----------------+       +----------------+
// |  Subject       |<------|  WeatherData   |       |  Display       |
// +----------------+       +----------------+       +----------------+
// | +Register()    |       | +Temperature   |<------| +Update()      |
// | +Unregister()  |       | +Humidity      |       +----------------+
// | +Notify()      |       +----------------+       |  StatsDisplay  |
// +----------------+                                 +----------------+
//                                                     +----------------+
//                                                     |  ForecastDisplay|
//                                                     +----------------+

public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void UnregisterObserver(IObserver observer);
    void NotifyObservers();
}

public interface IObserver
{
    void Update(double temp, double humidity);
}

public class WeatherData : ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    public double Temperature { get; private set; }
    public double Humidity { get; private set; }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void UnregisterObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(Temperature, Humidity);
        }
    }

    public void SetMeasurements(double temp, double humidity)
    {
        Temperature = temp;
        Humidity = humidity;
        NotifyObservers();
    }
}

public class Display : IObserver
{
    public void Update(double temp, double humidity)
    {
        // Display current conditions
    }
}