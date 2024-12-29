![image](https://github.com/user-attachments/assets/c37c2890-26b3-47cf-bb8a-c86673cb5b83)

![image](https://github.com/user-attachments/assets/00055e2a-5f88-4ba9-9c21-ea4e15adcc2d)

![image](https://github.com/user-attachments/assets/be690170-fcfc-47d4-be26-eb1926d65fdd)

```csharp
public abstract class Zone
{
    public string DisplayName { get; set; }
    public int Offset { get; set; }

    public abstract string GetDisplayName();
    public abstract int GetOffset();
}

public class ZoneUSEastern : Zone
{
    public ZoneUSEastern()
    {
        DisplayName = "US Eastern Zone";
        Offset = -5;
    }

    public override string GetDisplayName()
    {
        return DisplayName;
    }

    public override int GetOffset()
    {
        return Offset;
    }
}

public class ZoneUSCentral : Zone
{
    public ZoneUSCentral()
    {
        DisplayName = "US Central Zone";
        Offset = -6;
    }

    public override string GetDisplayName()
    {
        return DisplayName;
    }

    public override int GetOffset()
    {
        return Offset;
    }
}

public class ZoneUSMountain : Zone
{
    public ZoneUSMountain()
    {
        DisplayName = "US Mountain Zone";
        Offset = -7;
    }

    public override string GetDisplayName()
    {
        return DisplayName;
    }

    public override int GetOffset()
    {
        return Offset;
    }
}

public class ZoneUSPacific : Zone
{
    public ZoneUSPacific()
    {
        DisplayName = "US Pacific Zone";
        Offset = -8;
    }

    public override string GetDisplayName()
    {
        return DisplayName;
    }

    public override int GetOffset()
    {
        return Offset;
    }
}

public class ZoneFactory
{
    public static Zone CreateZone(string zoneId)
    {
        switch (zoneId)
        {
            case "USEastern":
                return new ZoneUSEastern();
            case "USCentral":
                return new ZoneUSCentral();
            case "USMountain":
                return new ZoneUSMountain();
            case "USPacific":
                return new ZoneUSPacific();
            default:
                throw new ArgumentException($"Zone ID '{zoneId}' is not recognized.");
        }
    }
}
```
