using System;

public class Document
{
    public string Content { get; set; }

    public void PrintState()
    {
        Console.WriteLine($"Document Content: {Content}");
    }
}

public class Client
{
    public static void Main(string[] args)
    {
        var document = new Document();

        // Modify the state directly
        document.Content = "Initial Content";
        document.PrintState();

        // Save state manually
        var savedContent = document.Content;

        // Make changes
        document.Content = "Modified Content";
        document.PrintState();

        // Restore state manually
        document.Content = savedContent;
        document.PrintState();
    }
}

// // Momento Pattern

#nullable disable
namespace MomentoPattern;

public class DocumentMomento
{
    public string Content { get; }

    public DocumentMomento(string content)
    {
        Content = content;
    }
}

public class Document
{
    public string Content { get; set; }

    public DocumentMomento SaveState()
    {
        return new DocumentMomento(Content);
    }

    public void RestoreState(DocumentMomento momento)
    {
        Content = momento.Content;
    }

    public void PrintState() => System.Console.WriteLine($"Document Content: {Content}");
}

public class CareTaker
{
    private DocumentMomento momento;
    
    public void SaveState(Document document)
    {
        momento = document.SaveState();
    }

    public void Restore(Document document)
    {
        document.RestoreState(momento);
    }
}

public class Program
{
   public static void Main(string[] args)
    {
        var document = new Document();
        var caretaker = new CareTaker();

        document.Content = "Hello World!";

        caretaker.SaveState(document);
        document.PrintState();
        
        document.Content = "Hello World New";
        document.PrintState();

        caretaker.Restore(document);
        document.PrintState();
    } 
}