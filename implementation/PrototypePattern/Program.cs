using System;
using System.Collections.Generic;

// Event Base Class
public abstract class DomainEvent
{
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
}

public class PaymentPlanCreatedEvent : DomainEvent { }
public class InstallmentCreatedEvent : DomainEvent { }
public class InstallmentApprovedEvent : DomainEvent { }

// Aggregate Root
public class PaymentPlan
{
    public Guid Id { get; private set; }
    public List<DomainEvent> Events { get; private set; } = new List<DomainEvent>();
    public string Status { get; private set; }

    public PaymentPlan(Guid id)
    {
        Id = id;
    }

    public void ApplyEvent(DomainEvent domainEvent)
    {
        Events.Add(domainEvent);

        // Update state based on the event
        if (domainEvent is PaymentPlanCreatedEvent) Status = "Created";
        if (domainEvent is InstallmentApprovedEvent) Status = "Approved";
        if (domainEvent is InstallmentCreatedEvent) Status = "Installment Created";
    }

    public void AddPaymentPlanStartedEvent()
    {
        if (Status == "Approved")
        {
            Events.Add(new DomainEvent()); // Add new event
            Status = "Started";
        }
        else
        {
            throw new InvalidOperationException("Cannot start payment plan unless approved.");
        }
    }
}

// Client Code
class Program
{
    static void Main(string[] args)
    {
        var paymentPlan = new PaymentPlan(Guid.NewGuid());

        // Apply events manually
        paymentPlan.ApplyEvent(new PaymentPlanCreatedEvent());
        paymentPlan.ApplyEvent(new InstallmentApprovedEvent());

        // Add new event and update status
        paymentPlan.AddPaymentPlanStartedEvent();

        Console.WriteLine($"Payment Plan Status: {paymentPlan.Status}");
        Console.WriteLine($"Event Count: {paymentPlan.Events.Count}");
    }
}

//Prototype Pattern
using System;
using System.Collections.Generic;

// Event Base Class
public abstract class DomainEvent
{
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
}

public class PaymentPlanCreatedEvent : DomainEvent { }
public class InstallmentCreatedEvent : DomainEvent { }
public class InstallmentApprovedEvent : DomainEvent { }
public class PaymentPlanStartedEvent : DomainEvent { }

// Prototype Interface
public interface IPrototype<T>
{
    T Clone();
}

// Aggregate Root with Cloning
public class PaymentPlan : IPrototype<PaymentPlan>
{
    public Guid Id { get; private set; }
    public List<DomainEvent> Events { get; private set; } = new List<DomainEvent>();
    public string Status { get; private set; }

    public PaymentPlan(Guid id)
    {
        Id = id;
    }

    public void ApplyEvent(DomainEvent domainEvent)
    {
        Events.Add(domainEvent);

        // Update state based on the event
        if (domainEvent is PaymentPlanCreatedEvent) Status = "Created";
        if (domainEvent is InstallmentApprovedEvent) Status = "Approved";
        if (domainEvent is InstallmentCreatedEvent) Status = "Installment Created";
    }

    public void AddPaymentPlanStartedEvent()
    {
        if (Status == "Approved")
        {
            ApplyEvent(new PaymentPlanStartedEvent());
            Status = "Started";
        }
        else
        {
            throw new InvalidOperationException("Cannot start payment plan unless approved.");
        }
    }

    public PaymentPlan Clone()
    {
        var clone = (PaymentPlan)this.MemberwiseClone();
        clone.Events = new List<DomainEvent>(this.Events); // Deep copy of events
        return clone;
    }
}

// Client Code
class Program
{
    static void Main(string[] args)
    {
        var paymentPlan = new PaymentPlan(Guid.NewGuid());

        // Apply initial events
        paymentPlan.ApplyEvent(new PaymentPlanCreatedEvent());
        paymentPlan.ApplyEvent(new InstallmentApprovedEvent());

        // Clone the aggregate for isolated operations
        var clonedPaymentPlan = paymentPlan.Clone();

        // Apply a new event to the clone
        clonedPaymentPlan.AddPaymentPlanStartedEvent();

        Console.WriteLine($"Original Payment Plan Status: {paymentPlan.Status}"); // "Approved"
        Console.WriteLine($"Cloned Payment Plan Status: {clonedPaymentPlan.Status}"); // "Started"
        Console.WriteLine($"Original Event Count: {paymentPlan.Events.Count}"); // 2
        Console.WriteLine($"Cloned Event Count: {clonedPaymentPlan.Events.Count}"); // 3
    }
}

