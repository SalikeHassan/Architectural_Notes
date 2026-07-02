# Domain-Driven Design — Study Notes
*(Based on 3 video sources)*

DDD is a software development approach for designing complex business software, made popular by Eric Evans in his 2003 book. The core idea: **the software you build should represent the business**, and it should be clear from the code how the business functions.

---

## The Problem DDD Solves — The Language Gap

The biggest cause of project failure is rarely technology. It's the **language gap** between the people who understand the business and the people building the software.

As developers, we instinctively translate everything into **CRUD** — Create, Read, Update, Delete. But your business experts don't speak that language.

**Netflix example:**
- A product owner says: *"A viewer should be able to start watching a video"*
- A developer hears: *"UPDATE users SET last_watched = ..."*

All the business meaning is lost the moment we reduce it to a database operation. DDD solves this by closing that language gap.

---

## Strategic Design

The first step — exploring the problem space at a high level, always done as a **group exercise with the business**. The engineering team should never define subdomains alone.

### Subdomains

A **domain** is the subject area you're building for. **Subdomains** are distinct parts of that domain. Working them out is an **iterative process** — you may find a domain is too large and needs breaking down further.

Netflix subdomains:

| Subdomain | Type | Description |
|---|---|---|
| **Video Streaming** | Core | The main competitive advantage |
| **Recommendations** | Supporting | Supports core but not the differentiator |
| **Billing** | Generic | Handles subscriptions and payments |

The three subdomain types:
- **Core** — the reason the business exists; where most investment should go
- **Supporting** — necessary but not the main differentiator
- **Generic** — common functionality that could be off-the-shelf

---

### Ubiquitous Language

Everyone — business and engineering — must use the **same language** within a given context. The terms in the code should match the terms used in conversations with business experts.

This is not just naming convention — it is a discipline. If the domain expert says *"a member earns affinity points"*, the code should have a class called `Member` with a method called `EarnAffinityPoints`. If the code says `UserRecommendationScoreIncrement`, the language gap is already open.

**The same real-world thing, three different identities across Netflix's bounded contexts:**

| Real person | Billing (Generic) | Streaming (Core) | Recommendations (Supporting) |
|---|---|---|---|
| Someone who pays and watches | **Subscriber** | **Viewer** | **Member** |
| Their unique identifier | `SubscriberId` | `ViewerId` | `MemberId` |
| Their primary concern | Plan, payment, billing cycle | Video quality, playback | Watch history, taste profile |
| Their key action | `StartSubscription`, `MakePayment` | `StreamContent` | `WatchContent`, `RateTitle` |

This is completely fine — each bounded context owns its own language. Trying to force one term across all three would mean every context drags in concepts that don't belong to it.

**Why this matters in practice:**

| If you ignore Ubiquitous Language... | What goes wrong |
|---|---|
| A developer calls it `User` everywhere | The domain expert cannot read or review the model |
| A method is named `UpdateUserRecord` | The business meaning (e.g. "cancel subscription") is invisible |
| The same term means different things in two contexts | Bugs appear at integration points; teams talk past each other |
| Code uses `userId` but the business says `subscriberId` | Every conversation needs translation; alignment erodes over time |

**Netflix example — language is consistent end-to-end within each context:**

```
Billing context
  Business says:  "A subscriber upgrades their plan"
  Command:         UpgradePlan          (not UpdateUserPlan or ChangeTier)
  Event:           PlanUpgraded         (not UserUpdated or TierChanged)
  Aggregate:       Subscription         (not UserAccount or BillingRecord)
  Value Object:    PaymentDetails       (not CardInfo or PaymentData)

Recommendations context
  Business says:  "A member watches a title and their taste profile is updated"
  Command:         WatchContent         (not ViewVideo or PlayItem)
  Event:           ContentWatched       (not VideoPlayed or ItemViewed)
  Aggregate:       ViewerProfile        (not UserPreferences or RecommendationModel)
  Value Object:    GenreAffinity        (not CategoryScore or GenreWeight)
```

---

### Bounded Context

A **bounded context** is a logical boundary within which the language is consistent and unambiguous. Each subdomain maps to its own bounded context.

You don't need the whole business to agree on one universal term. You only need agreement **within each context**. A well-defined bounded context will have things **unique to just that domain** — for example, payment details would only appear in Billing.

---

### Context Map

A **context map** describes the relationships between bounded contexts — which domains communicate with each other, how, and in which direction.

**Netflix example:** The Streaming domain needs to know what video quality to show a viewer. This depends on their subscription plan, which lives in the Billing domain:

```
Streaming Domain ←→ [Anti-Corruption Layer] ←→ Billing Domain
    "viewer"                                      "subscriber"
```

To avoid polluting either domain with concepts that don't belong there, an **Anti-Corruption Layer (ACL)** acts as a translator between the two, keeping each context clean.

---

### Anti-Corruption Layer (ACL)

#### Why we need it

When two bounded contexts need to communicate, the naive approach is to let one context directly reference the other's types. The Streaming domain imports `PlanType` from Billing, calls Billing services directly, and starts using Billing language. The damage compounds quickly:

**Without an ACL — what goes wrong:**

| Problem | Consequence |
|---|---|
| `Streaming` imports `PlanType` from `Billing.Domain` | Streaming is now coupled to Billing's internal model — a Billing refactor breaks Streaming |
| A Streaming handler checks `if (plan == PlanType.Premium)` | Business logic that belongs in Billing leaks into Streaming |
| The Streaming domain refers to a "subscriber" | Ubiquitous Language breaks — the Streaming team now speaks two dialects |
| Billing changes `PlanType.Standard` to `PlanType.Mid` | Every place Streaming references the old name must be updated |

The corruption flows in both directions. Without a hard boundary, the two contexts gradually merge into one muddy, coupled context that nobody fully owns.

**With an ACL — what you preserve:**

The Streaming domain never sees `PlanType`, `SubscriptionReadModel`, or any Billing type. It only ever works with `VideoQuality` — its own concept, in its own language. Billing can rename, restructure, or replace its internals without Streaming noticing.

---

#### How it is implemented — Port + Adapter pattern

The ACL in this codebase uses the **Ports and Adapters** pattern (also called Hexagonal Architecture):

- **Port** — an interface defined *by the consumer* (Streaming domain), expressing what it needs in its own language
- **Adapter** — a class in the *infrastructure layer* that implements the port by translating to/from the other context

```
Streaming.Domain          Streaming.Infrastructure        Billing.Application
─────────────────         ────────────────────────        ───────────────────
ISubscriptionService  ←── BillingSubscriptionAdapter ──→  ISubscriptionReadStore
  (Port — Streaming's        (Adapter — the ACL)             (Billing's read model)
   own interface,             Lives in Infrastructure,
   speaks Streaming           knows both languages,
   language only)             translates between them
```

**Key rule:** The port (`ISubscriptionService`) is owned by and lives inside the Streaming domain. The Streaming domain depends only on its own abstraction — it has no compile-time dependency on Billing at all.

---

#### Two questions answered before looking at the code

**Is the ACL making a call to the Billing database?**

In this learning codebase — no. There is no separate database and no network call. Both Billing and Streaming run in the same console process. The `BillingSubscriptionAdapter` is handed the same in-memory `readStore` object that Billing's own query handler uses. The DI wiring in `Program.cs` makes this visible:

```csharp
// Program.cs — both sides receive the same in-memory object
var readStore         = new InMemorySubscriptionReadStore();   // Billing's read model
var acl               = new BillingSubscriptionAdapter(readStore);  // ACL reads from it directly
var videoQualityQuery = new GetVideoQualityHandler(acl);            // Streaming uses the ACL
```

In a real microservices deployment the mechanism changes, but the ACL's responsibility does not:

| Deployment model | How the ACL gets Billing data |
|---|---|
| Single process (this codebase) | Shared in-memory read store — direct object reference, no network |
| Microservices, synchronous | ACL makes an HTTP call to Billing's REST API, maps the response |
| Microservices, asynchronous | Streaming subscribes to Billing's events on a message bus; ACL reads from Streaming's own local copy of the Billing data it needs |

In all three cases the Streaming domain sees only `VideoQuality`. The ACL absorbs all the integration complexity.

---

**Is the ACL part of the Streaming microservice?**

Yes — entirely. `BillingSubscriptionAdapter` lives in `Streaming.Infrastructure`. It is owned, deployed, and maintained by the Streaming team. Billing has no knowledge of it and no dependency on it.

This matters because **the consumer owns the translation**. The Streaming team decides what they need from Billing and how to map it into their own language. Billing does not need to change its model or expose a special endpoint to suit Streaming. The ACL is the Streaming team's problem to maintain, and that is correct — they are the ones who understand what `VideoQuality` means.

```
Billing team                       Streaming team
────────────                       ──────────────
Owns: Billing.Domain               Owns: Streaming.Domain
      Billing.Application                 Streaming.Application
      Billing.Infrastructure              Streaming.Infrastructure
                                             └── AntiCorruption/
                                                  └── BillingSubscriptionAdapter  ← theirs
```

Billing publishes its model (events, read stores, APIs). The Streaming team's ACL consumes it and translates. Neither team needs to coordinate on naming, structure, or language.

---

#### The actual code

**Step 1 — The Port (`Streaming.Domain`)**

```csharp
// Streaming.Domain/Services/ISubscriptionService.cs
public interface ISubscriptionService
{
    VideoQuality GetVideoQualityForViewer(Guid subscriptionId);
}
```

This interface speaks pure Streaming language. `VideoQuality` is a Streaming value object. There is no `PlanType`, no `SubscriptionReadModel`, no Billing concept anywhere in sight. The Streaming domain defines what it needs — not what Billing provides.

---

**Step 2 — The Adapter (`Streaming.Infrastructure`)**

```csharp
// Streaming.Infrastructure/AntiCorruption/BillingSubscriptionAdapter.cs
public sealed class BillingSubscriptionAdapter(ISubscriptionReadStore billingReadStore)
    : ISubscriptionService
{
    public VideoQuality GetVideoQualityForViewer(Guid subscriptionId)
    {
        // Step 1: fetch raw Billing data (Billing language)
        var billingData = billingReadStore.GetById(subscriptionId)
            ?? throw new InvalidOperationException(...);

        // Step 2: cancelled subscribers get the lowest quality
        if (billingData.IsCancelled)
            return VideoQuality.HD;

        // Step 3: translate — Billing concept → Streaming concept
        return billingData.Plan switch
        {
            PlanType.Basic    => VideoQuality.HD,       // 720p
            PlanType.Standard => VideoQuality.FullHD,   // 1080p
            PlanType.Premium  => VideoQuality.UltraHD,  // 2160p
            _ => throw new InvalidOperationException(...)
        };
    }
}
```

This class knows both languages. That is the *only* place in the codebase where both `PlanType` (Billing) and `VideoQuality` (Streaming) exist side by side. The translation is contained here and nowhere else.

---

**Step 3 — The Consumer (`Streaming.Application`)**

```csharp
// Streaming.Application/Queries/GetVideoQualityHandler.cs
public sealed class GetVideoQualityHandler(ISubscriptionService subscriptionService)
{
    public VideoQuality Handle(GetVideoQualityQuery query)
        => subscriptionService.GetVideoQualityForViewer(query.SubscriptionId);
}
```

The handler asks `ISubscriptionService` — the port — for a `VideoQuality`. It has no idea that Billing exists, what `PlanType` is, or how the quality was derived. This is intentional. The Streaming application layer is completely insulated.

---

**Step 4 — The Value Object that makes the translation meaningful**

```csharp
// Streaming.Domain/ValueObjects/VideoQuality.cs
public sealed class VideoQuality : ValueObject
{
    public static readonly VideoQuality HD      = new("HD",       720);
    public static readonly VideoQuality FullHD  = new("Full HD",  1080);
    public static readonly VideoQuality UltraHD = new("Ultra HD", 2160);
}
```

`VideoQuality` is a first-class Streaming domain concept — it has a label and a resolution. The ACL translates a Billing *tier* into a Streaming *quality specification*. These are different things expressed in different languages, and the value object makes that concrete.

---

#### Full flow — end to end

```
Streaming.Application          Streaming.Infrastructure        Billing.Application
──────────────────────         ────────────────────────        ────────────────────
GetVideoQualityHandler
  .Handle(query)
        │
        │  calls port (own interface)
        ▼
ISubscriptionService
  .GetVideoQualityForViewer()
        │
        │  resolved at runtime via DI
        ▼
BillingSubscriptionAdapter          ──── fetches ────▶  ISubscriptionReadStore
  receives: subscriptionId                               .GetById(subscriptionId)
  fetches:  SubscriptionReadModel                        returns: SubscriptionReadModel
  reads:    billingData.Plan (PlanType)                  { Plan: PlanType.Premium,
                                                           IsCancelled: false }
        │
        │  translates
        ▼
  PlanType.Premium → VideoQuality.UltraHD
        │
        │  returns Streaming concept
        ▼
GetVideoQualityHandler
  receives: VideoQuality.UltraHD ("Ultra HD (2160p)")
  — Billing never mentioned —
```

---

#### Where each file lives

```
src/
├── Streaming/
│   ├── Streaming.Domain/
│   │   ├── Services/
│   │   │   └── ISubscriptionService.cs        ← Port (interface owned by Streaming)
│   │   └── ValueObjects/
│   │       └── VideoQuality.cs                ← Streaming's own quality concept
│   ├── Streaming.Application/
│   │   └── Queries/
│   │       └── GetVideoQualityHandler.cs      ← Consumer — depends only on the port
│   └── Streaming.Infrastructure/
│       └── AntiCorruption/
│           └── BillingSubscriptionAdapter.cs  ← Adapter — the ACL, the only bilingual class
└── Billing/
    └── Billing.Application/
        └── ISubscriptionReadStore.cs          ← Billing's read model interface (Billing language)
```

The ACL lives in `Streaming.Infrastructure` — the outermost layer. This is deliberate. Infrastructure is allowed to reference external things. The domain and application layers are not.

---

## Tactical Design

Once domains are defined, tactical design zooms into implementation — refining the models inside each bounded context.

---

### Commands and Events

Instead of CRUD operations, we model the system around **intent and reaction**:

- **Commands** — actions or intentions sent to the system. They represent what a user or system *wants to do*. They can be accepted or rejected.
- **Events** — the system's reaction to a command. They represent something that **has already happened** and cannot be undone.

**Netflix example (Billing domain):**

| Command | Event |
|---|---|
| `StartSubscription` | `SubscriptionOpened` |
| `UpgradePlan` | `PlanUpgraded` |
| `MakePayment` | `PaymentProcessed` |
| `CancelSubscription` | `SubscriptionCancelled` |

Notice the language — these are meaningful business terms, not database operations. You can discuss naming these with *anyone* on the team, including non-developers.

---

### Entities

Entities map to real-world counterparts in your domain. Key characteristics:
- Have a **unique ID**
- Are **mutable** — properties can change over time
- Two entities are equal **only if they share the same ID**, even if all other properties differ

**Netflix example:** A `Subscriber` is an entity. Their email may change, but as long as the ID is the same, it's the same subscriber.

---

### Value Objects

A **value object** represents a value in your domain. Key characteristics:
- **No ID** — identity is based entirely on its values
- **Immutable** — cannot be updated; create a new one if the value needs to change (values set only in the constructor, no setters)
- Two value objects are equal if **all their property values are equal**

**Netflix example:** A `Subscriber` might have an `EmailAddress` and a `DateOfBirth` as value objects.

The reason to model something as a value object rather than a plain string: it signals this is an **important part of your domain**, and you can bake validation and business logic into the constructor. An `EmailAddress` object validates format on creation — you never need to validate it anywhere else in the codebase.

**Entity vs Value Object — how to decide:**
- In most domains, an address is just data → value object
- In a real estate app, an address is the core of the property → entity
- General rule: **prefer more value objects than entities** — they are small, immutable, and easier to work with

---

### Aggregates

An **aggregate** is a cluster of entities and value objects that:
- Has **state**
- Reacts to **commands**
- Publishes **events**
- Must always remain **consistent as a whole**

Think of an aggregate as a **small self-contained machine**: you push a command in, it checks its rules, updates its own state, and tells the rest of the system what just happened via events. Nothing outside it can reach inside and corrupt it.

---

#### 1. Has state

State is the **current snapshot** of everything the aggregate knows about itself. It is private — only the aggregate can read or change it.

**Netflix `Subscription` aggregate — its state:**

```
SubscriberName    : "Alice Johnson"
Email             : alice@netflix.com
Plan              : Premium
PaymentDetails    : card ending ••••1234, Alice Johnson, 12/2027
BillingCycle      : 01 Mar 2026 → 31 Mar 2026
IsCancelled       : false
Version           : 4   ← how many events have been applied so far
```

This state is **never stored directly**. Under event sourcing, it is rebuilt by replaying all past events in order — the events are the real source of truth, and the state above is just the current answer to the question *"what does the aggregate look like right now?"*.

---

#### 2. Reacts to commands

A **command** is an instruction sent to the aggregate — it expresses *intent*. The aggregate's job is to **decide whether to accept or reject it**.

Before accepting a command, the aggregate checks its **invariants** (business rules). If any rule is violated, the command is rejected with an exception — **no event is ever raised, no state ever changes**.

**Netflix example — four commands and how the `Subscription` aggregate reacts:**

| Command | Accepted? | Why / Invariant checked |
|---|---|---|
| `StartSubscription` (valid email, card) | Yes → raises `SubscriptionOpened` | Value objects validate format in constructor before any event is raised |
| `StartSubscription` (email = `"not-an-email"`) | **No** | `EmailAddress` constructor throws — command rejected before touching state |
| `UpgradePlan` (Basic → Premium) | Yes → raises `PlanUpgraded` | New plan is higher than current plan ✓ |
| `UpgradePlan` (Premium → Basic) | **No** | Downgrade rule: `"Plan downgrade is not permitted"` |
| `CancelSubscription` (already cancelled) | **No** | `EnsureNotCancelled()` throws: `"Cannot perform this operation on a cancelled subscription"` |
| `MakePayment` (amount = -5) | **No** | `"Payment amount must be greater than zero"` |

The aggregate is the **only** place these rules live. There is no validation in controllers, handlers, or services — it is all here, in one place, always enforced.

```csharp
// Inside Subscription.UpgradePlan():
EnsureNotCancelled();                          // rule 1
if (Plan == cmd.NewPlan) throw ...;            // rule 2
if (cmd.NewPlan < Plan)  throw ...;            // rule 3
ApplyAndRecord(new PlanUpgradedEvent(...));    // all rules passed → raise event
```

---

#### 3. Publishes events

When a command is **accepted**, the aggregate raises a **domain event** — a record of something that has irreversibly happened, written in past tense.

**Netflix example — commands accepted, events raised:**

| Command accepted | Event raised | What it records |
|---|---|---|
| `StartSubscription` | `SubscriptionOpened` | Name, email, plan, masked card, billing dates |
| `UpgradePlan` | `PlanUpgraded` | Old plan, new plan |
| `MakePayment` | `PaymentProcessed` | Amount, masked card used |
| `CancelSubscription` | `SubscriptionCancelled` | Timestamp only |

Events are **never rejected** — they record something that already happened. They are:
- Appended to the **event store** (the append-only write database)
- Published on the **message bus** so projections can update the read model

```
Command → Aggregate checks rules → Rules pass → Event raised
                                              → State updated (in memory)
                                              → Event stored (append-only)
                                              → Event published to message bus
                                              → Projection updates read model
```

If the command is rejected, none of the above happens. The system is unchanged.

---

#### 4. Must always remain consistent as a whole

**Consistency** means the aggregate can never be put into an invalid or contradictory state. All its parts — entities, value objects, flags — must make sense together at all times.

The aggregate enforces this by being the **only entry point** to its own state. No external code can reach in and change `Plan` or `IsCancelled` directly. Every change goes through a method on the aggregate, which checks rules first.

**Netflix example — what "consistent as a whole" protects against:**

| Broken state (without protection) | How the aggregate prevents it |
|---|---|
| `IsCancelled = true` but then a payment is processed | `EnsureNotCancelled()` is called at the start of every command method |
| Plan is downgraded from Premium to Basic | Downgrade check in `UpgradePlan()` — rejected before any event is raised |
| Email stored as `"hello"` with no `@` | `EmailAddress.Create()` validates format in its constructor — throws before the event is raised |
| Subscription exists with no billing cycle | `BillingCycle` is set inside the `Apply(SubscriptionOpenedEvent)` handler — it is structurally impossible to open a subscription without one |

**Performance note:** More invariant rules = longer update times. There is a trade-off between **consistency and performance**. In some cases a **corrective policy** — a background process that periodically checks and fixes inconsistencies — is a better fit than enforcing everything in real time. Netflix accepts eventual consistency on read data (e.g. which device shows which video quality) but enforces immediate consistency on write data (e.g. you cannot be charged after cancelling).

---

**Netflix example — the `Subscription` aggregate structure:**

```
Subscription (Aggregate Root — the single entry point)
├── SubscriberName      → plain string (part of state)
├── Email               → EmailAddress (Value Object — validates format)
├── Plan                → PlanType enum (Basic / Standard / Premium)
├── PaymentDetails      → Value Object (masks card number, validates expiry)
├── BillingCycle        → BillingCycleDates Value Object (start/end of billing period)
└── IsCancelled         → bool (guards all further commands)
```

---

#### Aggregate Root

The entity at the root of the aggregate is the **aggregate root**. It is the **single point of entry** — all external interactions go through it, and it is solely responsible for enforcing every invariant.

In the `Subscription` aggregate, `Subscription` itself is the aggregate root. Its constructor is **private** — the only way to create one is through `Subscription.Start(cmd)`, which ensures a subscription is always created in a valid state.

```csharp
// You cannot do: new Subscription()        ← private constructor blocks this
// You must do:   Subscription.Start(cmd)   ← the only valid entry point
```

This means it is **structurally impossible** for a `Subscription` to exist without going through the rules.

---

#### Netflix Domain → Subdomain → Aggregate Tree

How aggregates sit within Netflix's full domain structure:

```
Netflix (Domain)
│
├── Video Streaming [Core Domain]  ← most investment; richest model; Netflix's reason for existing
│   └── Bounded Context: Streaming
│       │   (Ubiquitous Language: person is a "Viewer", action is "streaming a title")
│       │
│       ├── Aggregate: StreamingSession  ◄── Aggregate Root
│       │   │   Represents one active viewing session for one viewer on one device.
│       │   │   This is where Netflix's most critical invariants live —
│       │   │   e.g. a Basic plan viewer cannot open a second concurrent stream.
│       │   │
│       │   ├── State (rebuilt by replaying events)
│       │   │   ├── SessionId           : Guid
│       │   │   ├── ViewerId            : Guid
│       │   │   ├── TitleId             : Guid
│       │   │   ├── DeviceId            : string
│       │   │   ├── VideoQuality        : VideoQuality        [Value Object — Basic/Standard/Premium]
│       │   │   ├── PlaybackPosition    : PlaybackPosition    [Value Object — timestamp + percentage]
│       │   │   ├── Status              : SessionStatus enum  (Active / Paused / Ended)
│       │   │   └── Version             : int  (optimistic concurrency)
│       │   │
│       │   ├── Value Objects
│       │   │   ├── VideoQuality        (Resolution, MaxBitrate — derived from plan via ACL)
│       │   │   └── PlaybackPosition    (ElapsedSeconds, PercentageComplete 0–100)
│       │   │
│       │   ├── Commands (intent — can be accepted or rejected)
│       │   │   ├── StartStream   → invariants: viewer has active subscription (checked via ACL);
│       │   │   │                              concurrent stream count < plan limit
│       │   │   │                              (Basic=1, Standard=2, Premium=4)
│       │   │   ├── PauseStream   → invariant: session must be Active
│       │   │   ├── ResumeStream  → invariant: session must be Paused
│       │   │   └── EndStream     → invariant: session must not already be Ended
│       │   │
│       │   └── Events (facts — immutable, append-only)
│       │       ├── StreamStarted       (SessionId, ViewerId, TitleId, DeviceId, VideoQuality)
│       │       ├── StreamPaused        (SessionId, PlaybackPosition, PausedAt)
│       │       ├── StreamResumed       (SessionId, ResumedAt)
│       │       └── StreamEnded         (SessionId, PlaybackPosition, EndedAt)
│       │
│       └── Anti-Corruption Layer (boundary with Billing)
│           └── ISubscriptionService (port) ←── implemented by BillingSubscriptionAdapter
│               └── translates: PlanType (Billing language) → VideoQuality (Streaming language)
│               └── called by: StartStream command to check plan before allowing the session
│
├── Recommendations [Supporting Domain]
│   └── Bounded Context: Recommendations
│       └── Aggregate: ViewerProfile  ◄── Aggregate Root
│           │   (Ubiquitous Language: person is a "Member", action is "watching a title")
│           │
│           ├── State (rebuilt by replaying events)
│           │   ├── MemberId            : Guid
│           │   ├── DisplayName         : string
│           │   ├── WatchHistory        : WatchRecord[]       [Value Object collection]
│           │   ├── GenreAffinities     : GenreAffinity[]     [Value Object collection]
│           │   ├── ContentRatings      : ContentRating[]     [Value Object collection]
│           │   └── Version             : int  (optimistic concurrency)
│           │
│           ├── Value Objects
│           │   ├── WatchRecord         (TitleId, WatchedAt, PercentageComplete)
│           │   ├── GenreAffinity       (Genre, AffinityScore 0.0–1.0)
│           │   └── ContentRating       (TitleId, Stars 1–5, RatedAt)
│           │
│           ├── Commands (intent — can be accepted or rejected)
│           │   ├── CreateProfile       → invariant: MemberId must be unique
│           │   ├── WatchContent        → invariant: PercentageComplete must be 0–100
│           │   ├── RateTitle           → invariant: Stars must be 1–5; title must be in WatchHistory
│           │   └── UpdateGenreAffinity → invariant: AffinityScore must be 0.0–1.0
│           │
│           └── Events (facts — immutable, append-only)
│               ├── ProfileCreated      (MemberId, DisplayName)
│               ├── ContentWatched      (TitleId, Genre, WatchedAt, PercentageComplete)
│               ├── TitleRated          (TitleId, Stars, RatedAt)
│               └── GenreAffinityUpdated (Genre, OldScore, NewScore)
│
└── Billing [Generic Domain]
    └── Bounded Context: Billing
        └── Aggregate: Subscription  ◄── Aggregate Root (single entry point)
            │
            ├── State (private — never stored directly, rebuilt by replaying events)
            │   ├── SubscriberName      : string
            │   ├── Email               : EmailAddress        [Value Object]
            │   ├── Plan                : PlanType enum       (Basic / Standard / Premium)
            │   ├── PaymentDetails      : PaymentDetails      [Value Object]
            │   ├── BillingCycle        : BillingCycleDates   [Value Object]
            │   ├── IsCancelled         : bool
            │   └── Version             : int  (optimistic concurrency)
            │
            ├── Commands (intent — can be accepted or rejected)
            │   ├── StartSubscription
            │   ├── UpgradePlan
            │   ├── MakePayment
            │   └── CancelSubscription
            │
            └── Events (facts — immutable, append-only)
                ├── SubscriptionOpened
                ├── PlanUpgraded
                ├── PaymentProcessed
                └── SubscriptionCancelled
```

**Reading the tree:**
- **Core Domain** → where the business competes; highest investment; owned internally
- **Supporting Domain** → necessary but not the differentiator; can be built or outsourced
- **Generic Domain** → commodity functionality; ideal candidate for off-the-shelf solutions
- Each domain maps to one **Bounded Context** — a linguistic and logical boundary
- Aggregates live **inside** bounded contexts; they are the write-side consistency units
- The Anti-Corruption Layer at the boundary keeps each context's language clean

**Why the learning codebase wired Streaming as query-only:**

The `Netflix.DDD` codebase intentionally simplified Streaming to a query-only consumer (no write side, no aggregate) so the focus could stay on learning Event Sourcing and CQRS through the Billing domain first. But that simplification creates a conceptual gap — the Core Domain ends up looking like the least sophisticated part of the system, which is the opposite of what DDD prescribes.

In a production system `StreamingSession` would be the most actively developed aggregate in the codebase. The concurrent stream limit invariant alone (Basic=1, Standard=2, Premium=4) is a real business rule Netflix enforces, and it belongs here — checked at `StartStream`, enforced inside the aggregate, not in a controller or service layer.

---

### Repositories

The **persistence layer** for aggregates. Repositories handle storing and retrieving aggregates from the database. One repository per aggregate.

---

### Services

Contain **business logic that doesn't fit inside a single aggregate** or spans multiple aggregates.

---

## Event Sourcing

Event sourcing is a **data persistence pattern** that pairs naturally with DDD, though it is independent of it.

Instead of storing the *current state* of your system, you store the **sequence of events (changes) that led to the current state** — similar to how Git stores changes to files over time.

**Netflix Billing example:**

Instead of storing: `subscriber_plan = Premium, balance = £0`

You store:
```
1. SubscriptionOpened   → plan: Basic
2. PlanUpgraded         → plan: Standard
3. PaymentProcessed     → amount: £10.99
4. PlanUpgraded         → plan: Premium
5. PaymentProcessed     → amount: £15.99
```

To get the current state, you **replay** events from the beginning. This is an append-only log — **no updates, no deletes**.

**Why this matters for Netflix:**
- If a product manager asks *"how many subscribers upgraded from Standard to Premium in the last 6 months?"* — you already have all the data. Replay the events and answer the question.
- Compare that to a traditional database approach: you'd need to first add a tracking field, wait months for data to accumulate, then answer.

---

## CQRS — Command Query Responsibility Segregation

CQRS is a design pattern that separates **reading** from **writing** at the application level.

It is built on the **CQS principle (Command Query Separation)**: every method should either:
- **Change state** and return nothing (a *command*)
- **Return data** and not change state (a *query*)
- Never both at the same time

CQRS applies this at the architecture level:

```
User
 ├── Wants to change something → Command → Write API
 └── Wants to read something   → Query  → Read API
```

### Why Two Databases?

A single normalised database is a compromise — neither great for reading nor writing. CQRS solves this:

| | Write Database | Read Database |
|---|---|---|
| **Structure** | Normalised (consistent writes) | Denormalised (fast reads) |
| **Operations** | Commands only | Queries only |
| **Netflix example** | Subscription changes, payment records | "What plan is this viewer on?", "What content can they access?" |

Synchronisation between the two happens via a **message queue** — when something is written, an event is published to the queue, and the read database(s) update asynchronously.

### Eventual Consistency

Because read databases update asynchronously, they may briefly be out of sync. This is called **eventual consistency** — the system *will* be consistent, it just takes a moment.

**Netflix example:** When a subscriber upgrades from Standard to Premium, there may be a brief window where one device still shows HD while another has already switched to Ultra HD. This is acceptable.

**When eventual consistency is NOT acceptable:** anything life-critical, health-related, or involving public safety infrastructure.

### CAP Theorem

Out of these three properties, a distributed system can only guarantee two:

| Letter | Stands For | Meaning |
|---|---|---|
| **C** | Consistency | Every read gets the most recent write |
| **A** | Availability | Every request gets a response |
| **P** | Partition Tolerance | System works even if nodes go down |

Since network partitions are impossible to fully prevent, the real choice is:
- **CP** — consistent but may have downtime when a node fails
- **AP** — always available but may serve stale data

**Netflix chooses AP** — it's more important that the platform stays up than that every device sees the exact same subscription state at the exact same millisecond.

### Scaling with CQRS

Because read databases are append-only and read-only, you can **duplicate them freely**:

```
Write DB → Message Queue → Read DB 1
                        → Read DB 2
                        → Read DB 3 (scale as needed)
```

Write and read sides scale **independently** — important for Netflix, where content is browsed far more often than subscriptions change.

---

## How DDD + Event Sourcing + CQRS Work Together

```
UI
 │
 ├── Command → Write API (DDD models: aggregates, commands, events)
 │                  ↓
 │             Event Store (append-only, event sourcing)
 │                  ↓
 │             Message Queue
 │                  ↓
 │             Read Database (denormalised projections)
 │
 └── Query  → Read API → Read Database
```

**Netflix example flow — subscriber upgrades to Premium:**
1. UI sends `UpgradePlan` command to Write API
2. Write API's `Subscription` aggregate validates the command (invariants checked by aggregate root)
3. `PlanUpgraded` event is published and stored in the event store
4. Event is sent via message queue to the read database
5. Read database updates the viewer's accessible content and video quality
6. Streaming domain queries the read API to confirm what quality to serve

---

## Summary

```
Domain (Netflix)
├── Subdomain: Video Streaming  [Core]       → Bounded Context: Streaming
├── Subdomain: Recommendations  [Supporting] → Bounded Context: Recommendations
└── Subdomain: Billing          [Generic]    → Bounded Context: Billing
         ↕ Context Map + Anti-Corruption Layer

Inside each Bounded Context:
└── Aggregate (e.g. Subscription)
    ├── Aggregate Root — Subscriber (enforces invariants)
    ├── Entities — SubscriptionPlan
    └── Value Objects — PaymentDetails, BillingCycleDates
    └── Commands → Events → State

Persistence & Architecture:
├── Event Sourcing  → store events, not state; replay to rebuild
├── CQRS            → separate write API and read API
└── Eventual Consistency → read side catches up via message queue
```

> **Important distinction:** DDD is not a technical concept — it is about people, communication, and shared understanding. Event Sourcing and CQRS are technical patterns. They work well together but are independent of each other.
