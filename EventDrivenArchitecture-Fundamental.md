# Core Components of Event-Driven Architecture (EDA)

The core components of an EDA are:

- **Producers**
- **Consumers**
- **Brokers**

### Producers
Producers are upstream services in your backend, such as:
- A database (for data change events)
- A content management system (e.g., when a new post is uploaded)
- A third-party API

They create events and send them to the event broker.

### Brokers
The broker stores these messages until a consumer is ready to ingest them. Each event might be read by a single consumer or multiple consumers. Once all the consumers have read the event, the broker might delete it (depending on the configuration).

## Events

An event is a type of message that signals a change in state or an action within the system. Examples include:
- A user on Amazon placing an order
- A content creator on YouTube uploading a video

**Note**: Although often used interchangeably, an event and a message are different. An event is a type of message that denotes an action or state update. All events are messages, but not all messages can be considered events. For instance, a message could also be an acknowledgment, a logging metric, or an error message.

Events are usually represented in a key/value format where:
- **Key**: Identifies the event, such as an order ID or unique identifier
- **Value**: Stores the event details

In Event-Driven Architectures, events are immutable. Once an event is published to the system, it typically isn’t changed. Instead, a new event with updated data is published.

## Event Patterns

The exact state you include in the event depends on your system’s architecture. Common patterns include:

- **Event Notification Pattern**: The event is a short message that indicates a state change. Downstream consumers must query upstream producers to get the relevant data.
- **Event-Carried State Transfer Pattern**: The event contains all the state that has changed, so downstream consumers can obtain all relevant information directly from the event message.

## Event Producers

Event producers (or sources) are the parts of the system responsible for sending events to the broker.

### Role of Producers

- **Event Creation**: Take actions/updates from users or other backend services and create events according to a predefined schema.
- **Serialization**: Serialize the event using a format like JSON, Avro, or Protobuf.
- **Transmission**: Send the serialized event to the event broker.

Events are typically transmitted in batches to avoid network congestion. Producers aggregate messages until:
- A specified time duration has passed
- A configured threshold of accumulated messages is reached

## Event Broker

The event broker is an immutable log that stores events. In many systems, the broker can be partitioned across multiple machines to ensure fault tolerance and scalability.

### Types of Event Brokers

#### Event Stream
- Stores a continuous flow of events, allowing multiple backend services (consumers) to ingest each event.
- Consumers can re-read past events to "go back in time," which is useful if a consumer crashes and needs to rebuild its state.

In an event stream, messages can be configured to be deleted when:
- A **TTL (time to live)** expires
- Certain conditions are met (e.g., all consumers have read the message or based on a retention period)

**Examples**: Kafka, AWS Kinesis

#### Event Queue
- Traditionally, a single consumer reads each event from the queue, though multiple consumers can process different events individually.
- Once a message is consumed (acknowledged by the consumer), it is removed from the queue.

**Examples**: AWS SQS, RabbitMQ

## Event Consumers

Consumers are the backend services that ingest messages from the event broker. Each message can be processed by a single consumer or multiple consumers.

### Example Use Case: YouTube Video Upload
When a video upload event is produced, multiple consumers might process it:
- A **subtitle service** for generating subtitles
- A **transcoding service** for converting the video to different screen sizes
- A **copyright service** to ensure it’s not pirated

### Consumer Offset Tracking

- **Event Stream (e.g., Kafka)**: Each consumer has an **offset** that tracks which messages it has already processed. Kafka tracks this offset but allows consumers to change it to replay past messages if needed.
- **Event Queue (e.g., RabbitMQ)**: Consumers and the queue use **acknowledgment (ack) messages** to confirm that an event has been ingested successfully.

# Key Benefits of Event-Driven Architectures (EDA)

Here are some reasons why you’d want to use an event-driven architecture.

## Loose Coupling

Tightly coupled backend services can lead to cascading failures and require careful handling of retry storms or thundering herd issues. In tightly coupled systems, scaling one component without adjusting another can overwhelm services.

With event-driven architectures, the **broker acts as a buffer** and decouples producers and consumers. Each part of the system can scale independently. For example, if consumer services are more compute-intensive, they can be autoscaled faster than the producer (and vice-versa).

## Scalability

Event brokers are built to be highly scalable.

- **Kafka**: Events are grouped into topics (similar to folders), and each topic can have as many partitions as needed. As throughput increases, nodes can be added to the Kafka cluster. For instance, LinkedIn scaled Kafka to over 7 trillion messages per day with 7 million partitions (as of 2019).
- **Event Queues**: Services like **AWS SQS** can handle thousands of messages per second per queue.

## Extensibility

In an event-streaming architecture (e.g., using Kafka), each event can be consumed by multiple consumers without modifying producers or other consumer services. Adding new consumers is seamless.

# Relevant Technologies

## Event Serialization

Event messages need to be serialized for transmission across networks. Popular formats include:

- **JSON**: A text-based format where data is structured like a JavaScript object (curly braces, keys, values).
- **Protobuf**: A binary format developed by Google, where data is structured based on predefined schemas for compact and efficient serialization.
- **Avro**: A compact binary format that supports schema evolution, useful if messages need to change over time.

## Messaging Platforms

Messaging platforms can be categorized as **event brokers** or **event streams**. Event brokers are ideal for single-consumer setups, while event streams support multiple consumers.

### Event Streams

- **Kafka**: An open-source stream-processing platform by LinkedIn, designed for high-throughput, fault tolerance, and scalability.
- **Pulsar**: Developed by Apache, a distributed pub-sub messaging platform with a flexible model and intuitive API.
- **AWS Kinesis (Data Stream)**: AWS's scalable, durable real-time data streaming service, capable of capturing gigabytes of data per second.

### Event Brokers

- **RabbitMQ**: An open-source message broker implementing AMQP, known for its plugin architecture and versatile routing.
- **ActiveMQ**: An Apache open-source broker written in Java, offering JMS, REST, and WebSocket interfaces.
- **AWS SQS**: Amazon's distributed message queuing service, integrated with other AWS services to facilitate decoupling and scaling.

- **Design Patterns with EDAs**
  - Event Notification
  - Event-Carried State Transfer
  - CQRS and more
- **Challenges with EDAs**
  - Event Ordering/Sequencing
  - Data Consistency
  - Idempotency and more
 
# Kafka Offset

For more information on Kafka offset management, refer to the [Confluent Kafka Consumer Documentation - Offset Management](https://docs.confluent.io/platform/current/clients/consumer.html?utm_source=blog.quastor.org&utm_medium=newsletter&utm_campaign=a-technical-deep-dive-on-event-driven-architectures#offset-management).

# RabbitMQ Ack

For more information on RabbitMQ acknowledgments, refer to the [RabbitMQ Documentation - Confirms and Acknowledgments](https://www.rabbitmq.com/docs/confirms?utm_source=blog.quastor.org&utm_medium=newsletter&utm_campaign=a-technical-deep-dive-on-event-driven-architectures).
