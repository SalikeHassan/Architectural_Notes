### Creational Patterns

**Creational patterns focus on object creation mechanisms. They aim to provide flexibility and reuse in how objects are instantiated, ensuring proper object construction while decoupling the creation process.**

---
### Structural Patterns
**Structural patterns deal with the composition of classes or objects. They simplify the relationships between entities, making the system easier to design, maintain, and extend.**

---
### Behavioral Patterns
**Behavioral patterns focus on communication and interaction between objects. They improve the system’s flexibility in carrying out complex behaviors.**

![image](https://github.com/user-attachments/assets/51b1f2ea-325b-4a08-a7e2-142bb991c64a)

| **Category**          | **Pattern**              | **Definition**                                                                                 |
|------------------------|--------------------------|-----------------------------------------------------------------------------------------------|
| **Creational**         | Singleton               | Ensures only one instance of a class is created and provides a global point of access to it.  |
|                        | Factory Method          | Defines an interface for creating objects but lets subclasses decide which class to instantiate. |
|                        | Abstract Factory        | Provides an interface to create families of related objects without specifying their concrete classes. |
|                        | Builder                 | Constructs complex objects step-by-step, allowing for controlled and flexible object creation. |
|                        | Prototype               | Creates new objects by copying an existing object (prototype).                                |
| **Structural**         | Adapter                 | Converts one interface into another to make incompatible interfaces compatible.               |
|                        | Decorator               | Dynamically adds behavior to an object without altering its structure.                        |
|                        | Proxy                   | Controls access to another object, often used for lazy initialization, caching, or security.  |
|                        | Composite               | Treats individual objects and compositions of objects uniformly.                              |
|                        | Bridge                  | Decouples abstraction from its implementation to allow the two to vary independently.         |
|                        | Facade                  | Provides a simplified interface to a complex subsystem.                                       |
| **Behavioral**         | Observer                | Notifies multiple objects about state changes in a subject.                                   |
|                        | Strategy                | Defines a family of interchangeable algorithms and lets the client choose one at runtime.     |
|                        | Command                 | Encapsulates a request as an object, allowing for parameterization, logging, or undo/redo.    |
|                        | Chain of Responsibility | Passes requests along a chain of handlers until one can process the request.                  |
|                        | State                   | Allows an object to alter its behavior based on its internal state.                           |
|                        | Mediator                | Centralizes communication between objects to reduce coupling.                                 |
|                        | Memento                 | Captures and restores an object's state without exposing its internal structure.              |
|                        | Iterator                | Provides a way to access elements of a collection sequentially without exposing its structure. |
|                        | Visitor                 | Adds new operations to a group of objects without modifying their structure.                  |
|                        | Interpreter             | Defines a grammar and interprets sentences in that grammar.                                   |
| **Other/Architectural**| MVC                     | Separates an application into Model, View, and Controller for modularity and separation of concerns. |
|                        | CQRS                    | Segregates read and write operations into separate models to optimize performance.            |
