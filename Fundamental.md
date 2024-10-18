# Monolith vs. Microservices: Fundamental Differences

## Monolith

- **Single Unit Architecture**: Monolithic architecture is designed as a single unified unit, where all components are interconnected and run as one process.
- **Centralized Deployment**: The entire system is deployed together. Any changes or updates require redeploying the whole application.
- **Tightly Coupled**: All modules are tightly connected, which can make it harder to scale individual components.
- **Simpler for Small Applications**: Easier to develop and manage in the initial phases, especially for small teams or projects.
- **Deployment**: One deployment unit.
- **Communication**: Using method calls.
- **Scalable but Less Efficient**: Horizontal and Vertical scaling are possible, but it is less efficient because we need to scale the entire app. In reality, only a few modules may need scalability, but since a monolith has a single deployable unit, we can't scale a particular module.
- **Consistency**: We can achieve immediate consistency.

## Microservices

- **Distributed Architecture**: Microservices architecture divides the application into small, independent services, each with a distinct function.
- **Independent Deployment**: Each service can be deployed independently, allowing faster updates and more frequent releases.
- **Loosely Coupled**: Services are designed to be loosely coupled, facilitating individual scalability and maintainability.
- **Complexity**: Requires handling inter-service communication, versioning, and distributed data management, which adds to the system's overall complexity.
- **Deployment**: Multiple deployment units.
- **Scalable**: Efficient horizontal and vertical scaling.
- **Communication**: Communicating with other services requires implementing protocols, such as HTTPS, which involves serialization and deserialization of data.
- **Consistency**: No immediate consistency. It can be achieved by following approaches like the Saga Pattern.

## Which One is Better?

**Architectural Drivers** help decide which one is better. Below are the constraints:

- **Technical Constraints**: Taken by the engineering team and hard to reverse at a later stage (e.g., programming language, framework).
- **Business Constraints**: Mostly non-negotiable (e.g., deadline, budget).
- **Functional Requirements**: Describes what problems the system solves and how it solves them.
- **Quality Attributes**: Known as -ilities:
  - Scalability
  - Availability
  - Testability
  - Modularity
  - Maintainability

## Fallacies of Distributed Computing

- The network is reliable.
- Latency is zero.
- Bandwidth is infinite.
- The network is secure.
- Topology doesn't change.
- There is one administrator.
- Transport cost is zero.
- The network is homogeneous.

## Horizontal vs. Vertical Scaling

### Horizontal Scaling

- **Definition**: Horizontal scaling, also known as "scaling out," involves adding more instances or nodes to distribute the load. It is commonly used in cloud environments to handle increased traffic.
- **Example in Azure**: Azure Virtual Machine Scale Sets (VMSS) allow you to create and manage a group of load-balanced VMs. As the demand grows, Azure can automatically add more instances to meet the traffic needs.
- **Use Case**: Suitable for applications where workload distribution is possible, such as stateless microservices where instances can be added or removed without impacting the system.

### Vertical Scaling

- **Definition**: Vertical scaling, also known as "scaling up," involves increasing the power (CPU, RAM) of an existing server or instance. It enhances the capacity of a single unit rather than adding more units.
- **Example in Azure**: Azure offers the ability to resize a Virtual Machine to a larger size, adding more CPU or memory resources.
- **Use Case**: Useful for monolithic applications where adding more resources to a single instance is easier than re-architecting the application for distribution.

### Comparison

- **Scalability**: Horizontal scaling is more flexible for handling unexpected spikes, while vertical scaling is limited by the physical capabilities of the machine.
- **Cost**: Horizontal scaling can be more cost-effective in the long run, especially in cloud environments like Azure, where scaling out is automated, whereas vertical scaling may have cost implications if larger VMs are required.
- **Azure Perspective**: Azure services such as Azure App Service or Azure Kubernetes Service (AKS) leverage horizontal scaling to automatically handle varying loads, making it ideal for modern, distributed architectures.

## Saga Pattern and How It Handles Distributed Transactions

### Saga Pattern

- **Definition**: The Saga Pattern is a design pattern used to manage distributed transactions in microservices. It splits a long-running transaction into a series of smaller, individual transactions, each managed by a specific microservice. These transactions are coordinated in such a way that the entire business process either completes successfully or is rolled back if an error occurs.

#### Two Types:

1. **Choreography**: Each service involved in the Saga listens to events and performs its task. If it completes successfully, it triggers the next event. In case of failure, compensating actions are initiated to revert any successful changes made by previous services.
2. **Orchestration**: A centralized coordinator (or orchestrator) directs the execution of all steps in the Saga, managing the sequence and handling compensations if needed.

### Handling Distributed Transactions

- **Problem with Distributed Transactions**: In a distributed microservices environment, achieving ACID properties across services is challenging because different services may use different databases, and locking resources across them would cause scalability and performance issues.
- **How Saga Solves This**: The Saga pattern helps by breaking down a complex transaction into multiple steps that are eventually consistent. If one step fails, compensating transactions are initiated to roll back the changes made by previously completed steps. This helps maintain consistency without the need for distributed locking.

#### Example in Azure

- **Azure Functions and Service Bus**: You can implement the Saga pattern in Azure using Azure Functions as orchestrators or workers and Azure Service Bus for message passing between services. For example, an order processing workflow could involve different services (payment, inventory, shipping) that communicate through messages managed by Service Bus, with compensations triggered by Azure Functions if needed.

### Use Case

- **E-commerce Order Processing**: In an e-commerce application, the Saga pattern can be used to manage order placement, payment, inventory reservation, and shipping. Each step can be managed by different microservices, and compensating actions can be triggered in case any step fails, ensuring that the system remains in a consistent state.

### Handling Failures in Saga Pattern

#### Compensating Transactions

- When a distributed transaction fails at a certain point, the Saga pattern initiates compensating transactions to undo the work done by previous successful steps. Each service involved in the Saga must implement compensating logic, which essentially reverses its action to maintain consistency.

#### Choreography Approach

- In the choreography approach, each service listens for failure events and triggers its own compensating action. This decentralized model allows each service to react independently to a failure, thus rolling back its changes without the need for a central coordinator.

#### Orchestration Approach

- In the orchestration approach, the orchestrator keeps track of the sequence of steps. If a failure occurs, the orchestrator directs each service to perform compensating actions. This ensures that all the necessary rollback steps are executed in a coordinated manner, maintaining consistency across the distributed services.

#### Timeouts and Retries

- To handle transient issues, Sagas often include retry mechanisms with defined timeouts. If a step fails due to a temporary error, the system can attempt to retry before proceeding with compensations, thereby reducing the likelihood of unnecessary rollbacks.

#### Idempotency

- Services involved in the Saga pattern must be idempotent, meaning that repeated execution of the same action has the same effect as executing it once. This helps in handling retries and ensures that compensations and operations are applied consistently, even in the case of failures.

## Performance and Availability Enhancements in Modern Applications

### Caching

- **Definition**: Caching is the process of storing copies of frequently accessed data in a high-speed storage layer (cache) to reduce access time. It helps improve application performance by reducing the time needed to retrieve data from databases or external services.
- **Example in Azure**: Azure Cache for Redis is a distributed, in-memory cache that can be used to improve performance by storing frequently accessed data in memory, reducing the need for repeated database calls.
- **Use Case**: Caching is used in scenarios such as storing session data, frequently accessed database queries, or API responses to reduce latency.

### Performance Enhancements

- **Load Balancing**: Load balancing helps distribute incoming traffic across multiple servers to ensure that no single server is overwhelmed, improving availability and performance. Azure Load Balancer provides an effective way to distribute load across Azure VMs.
- **Auto-scaling**: Auto-scaling automatically adjusts the number of instances of an application or service based on demand, ensuring optimal resource utilization. Azure Kubernetes Service (AKS) offers auto-scaling features to automatically increase or decrease the number of pods running based on the workload.

### Throughput and Latency

- **Throughput**: Throughput refers to the number of requests that can be processed in a given amount of time. Optimizing throughput involves tuning resources, caching, and parallel processing to handle more requests simultaneously.
- **Latency**: Latency is the time taken for a request to travel from the client to the server and back. Reducing latency involves optimizing network communication, caching, and using Content Delivery Networks (CDNs) such as Azure CDN to bring data closer to users.

### Concurrency

- **Definition**: Concurrency involves handling multiple tasks at the same time, such as handling multiple user requests concurrently. Concurrency helps improve application responsiveness and throughput.
- **Techniques**: In .NET, asynchronous programming with async and await can be used to achieve concurrency. Azure Functions and Azure Logic Apps also provide ways to run multiple processes in parallel to handle concurrent workloads.

### Idempotency

- **Definition**: Idempotency ensures that performing the same operation multiple times results in the same outcome. It is crucial for ensuring reliability in distributed systems, especially in scenarios involving retries.
- **Use Case**: In payment processing, making a payment request multiple times should not result in multiple charges. Implementing idempotency keys can help track and avoid duplicate operations.

### Circuit Breaker Pattern

- **Definition**: The Circuit Breaker pattern is used to detect failures and encapsulate the logic of preventing a failure from constantly recurring during maintenance, temporary external system failure, or unexpected system difficulties.
- **Use Case**: In an application that depends on an external payment gateway, the circuit breaker can prevent the application from making repeated calls to the gateway if it is temporarily down, improving system stability.

### Rate Limiting

- **Definition**: Rate limiting controls the number of requests a user can make to an API in a given time frame, which helps in managing traffic and ensuring fair usage of resources.
- **Example in Azure**: Azure API Management allows you to define rate limits and quotas to manage the number of requests from clients, thus maintaining the stability of backend services.

### Retry Policies and Backoff Strategies

- **Definition**: Retry policies help in handling transient failures by automatically retrying failed operations. Backoff strategies involve waiting for a specified amount of time before retrying, reducing the load on the system.
- **Example in Azure**: Azure SDKs often have built-in retry policies for handling transient errors when connecting to Azure services like Azure Storage or Azure SQL Database.
