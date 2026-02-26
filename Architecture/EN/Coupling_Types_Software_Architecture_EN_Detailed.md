# Coupling Types in Software Architecture (Detailed Edition + Examples)

## Executive Summary
Coupling is the degree of interdependence between modules. Architecture does not eliminate coupling; it **controls** it:
- make it **explicit**
- keep it **localized**
- keep it **stable at boundaries**
- allow evolution without domino effects

This document includes:
- Definitions
- Bad / good examples
- Severity
- Detection signals
- How to manage (refactor patterns)
- Sources

---

# Legend
- **Severity**: 🟡 Low / 🟠 Medium / 🔴 High / 🔥 Critical
- **Scope**: Local (inside one codebase) vs Distributed (across services)

---

# 1️⃣ Structural Coupling (Tight Coupling)

## Definition
A module depends directly on a **concrete implementation**, rather than an abstraction.

## Severity
🔴 High (Local) → becomes 🔥 Critical when it spreads across many components.

## Detection Signals
- `new ConcreteClass()` inside business code
- static singletons / service locators everywhere
- testing requires real DB / filesystem / HTTP
- changes in infra code require changes in domain code

## ❌ Bad Example (hard dependency)
```csharp
public class OrderService
{
    private readonly SqlOrderRepository _repo = new SqlOrderRepository();

    public Task<Order> GetAsync(Guid id) => _repo.GetAsync(id);
}
```

## ✅ Good Example (dependency inversion)
```csharp
public interface IOrderRepository
{
    Task<Order> GetAsync(Guid id);
}

public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;

    public Task<Order> GetAsync(Guid id) => _repo.GetAsync(id);
}
```

## How to Manage
- Dependency Injection
- Dependency Inversion Principle (DIP)
- Hexagonal / Ports & Adapters architecture

## Sources
- https://en.wikipedia.org/wiki/Coupling_(computer_programming)
- https://martinfowler.com/bliki/InversionOfControl.html
- Robert C. Martin — *Clean Architecture*

---

# 2️⃣ Temporal Coupling

## Definition
Correct behavior depends on **calling things in the right order**, but the code doesn’t enforce it.

## Severity
🔴 High (often hidden)

## Detection Signals
- comments like “must call X before Y”
- multiple `Init()/Start()/Configure()` methods
- partial construction and “remember to call…” APIs
- brittle workflows in background jobs / hosted services

## ❌ Bad Example (order-sensitive calls)
```csharp
var order = new Order();
order.CalculateTotal(); // assumes lines exist
order.Validate();       // assumes totals computed
order.Save();           // assumes validated
```

## ✅ Good Example (encapsulated invariant)
```csharp
order.Process(); // internally enforces the right order & invariants
```

## ✅ Good Example (state machine)
```csharp
public enum OrderState { Draft, Validated, Paid, Shipped }

public class Order
{
    public OrderState State { get; private set; } = OrderState.Draft;

    public void Validate()
    {
        if (State != OrderState.Draft) throw new InvalidOperationException();
        State = OrderState.Validated;
    }
}
```

## How to Manage
- Encapsulate workflows into a single method or domain service
- State machines for lifecycle
- DDD aggregates enforcing invariants

## Sources
- https://en.wikipedia.org/wiki/Temporal_coupling

---

# 3️⃣ Shared Data Coupling

## Definition
Multiple modules (or services) read/write the same **mutable database tables** directly.

## Severity
🔥 Critical (Distributed)

## Detection Signals
- one “shared” DB used by many services
- services join tables “owned” by others
- unclear column ownership (“who is allowed to write this?”)
- breaking changes in schema ripple everywhere
- heavy reliance on stored procedures as cross-app API

## ❌ Bad Example (two services writing the same table)
Service A:
```sql
UPDATE Operation SET Status = 3 WHERE Id = @id;
```
Service B:
```sql
UPDATE Operation SET Status = 4 WHERE Id = @id;
```

## ✅ Good Example (single writer, others integrate)
Service A is the **single writer** and exposes:
- API: `POST /operations/{id}/complete`
- Event: `OperationCompletedEvent`

Service B reacts without writing the table:
```csharp
public Task Handle(OperationCompletedEvent e)
{
    // Update local read model or trigger its own process
    return Task.CompletedTask;
}
```

## How to Manage
- **Single-writer principle** per aggregate/table/column
- Database per service (when feasible)
- Explicit APIs for writes; events for integration
- Build a **data ownership matrix**

## Sources
- https://martinfowler.com/articles/microservices.html
- Sam Newman — *Building Microservices*

---

# 4️⃣ Schema / Format Coupling

## Definition
Consumers depend on a provider’s **internal payload shape** (fields, nesting, naming).

## Severity
🟠 Medium → 🔴 High when many consumers exist.

## Detection Signals
- dynamic JSON parsing
- shared DTO assemblies between bounded contexts
- consumers reading internal fields (e.g., `internalField.subField`)
- breaking changes after “minor” payload edits

## ❌ Bad Example (dynamic / brittle)
```csharp
dynamic obj = JsonConvert.DeserializeObject(json);
var value = obj.data.internalField.subField; // breaks easily
```

## ✅ Good Example (explicit contract + mapping)
```csharp
public record OrderDto(string Id, string Status);

var dto = JsonConvert.DeserializeObject<OrderDto>(json);
```

## ✅ Good Example (versioned contract)
```http
GET /v2/orders/{id}
```
Provider supports `/v1` and `/v2` during migration.

## How to Manage
- Versioned DTOs / endpoints
- Consumer-driven contracts
- Compatibility tests in CI

## Sources
- https://martinfowler.com/articles/consumerDrivenContracts.html

---

# 5️⃣ Semantic Coupling (Hidden Business Coupling)

## Definition
A module depends on **implicit business meaning** from another module (same words, same statuses, same rules),
but nothing enforces consistency.

## Severity
🔥 Critical (because it is **invisible**)

## Detection Signals
- magic numbers (status codes)
- duplicated validation logic
- duplicated pricing rules / routing rules
- “we interpret this flag as…” in multiple places

## ❌ Bad Example (magic status)
```csharp
if (order.Status == 3) // "3 means delivered"
{
    NotifyCustomer();
}
```

## ✅ Good Example (explicit domain meaning)
```csharp
if (order.IsDelivered())
{
    NotifyCustomer();
}
```

## ✅ Good Example (domain event)
```csharp
public record OrderDeliveredEvent(Guid OrderId, DateTime DeliveredAtUtc);
```

## How to Manage
- Ubiquitous language, explicit naming
- Bounded contexts, clear ownership of meaning
- Domain events over generic “StatusChanged”

## Sources
- https://martinfowler.com/bliki/BoundedContext.html
- Eric Evans — *Domain-Driven Design*

---

# 6️⃣ Control Coupling

## Definition
One module controls another module’s internal behavior through flags, modes, or “configuration-by-boolean”.

## Severity
🟠 Medium

## Detection Signals
- methods with many booleans: `Do(x, true, false, true)`
- behavior changes based on hidden flags
- hard to reason about branches

## ❌ Bad Example (boolean flags)
```csharp
ProcessOrder(order, validate: true, sendEmail: false, recalcPrice: true);
```

## ✅ Good Example (options object)
```csharp
public record ProcessOrderOptions(bool Validate, bool SendEmail, bool RecalcPrice);

ProcessOrder(order, new ProcessOrderOptions(true, false, true));
```

## ✅ Good Example (strategy)
```csharp
public interface IPricingStrategy { Money Calculate(Order o); }
```

## How to Manage
- Replace booleans with option objects
- Strategy / Command patterns
- Split method responsibilities

## Sources
- https://en.wikipedia.org/wiki/Coupling_(computer_programming)

---

# 7️⃣ Logical Coupling (Co-change Coupling)

## Definition
Two modules tend to change together even without direct references.

## Severity
🟠 Medium (but indicates deeper architectural mismatch)

## Detection Signals
- frequent “touch 5 repos to change 1 feature”
- commits that always modify the same files
- release trains where unrelated services must redeploy together

## Example (symptom)
A change in “delivery confirmation” always touches:
- Mobile API
- Tracking Service
- Billing rules
- Notifications

→ likely the domain boundary is wrong or ownership is unclear.

## How to Manage
- Git co-change analysis
- Move code to align with domain boundaries
- Reduce shared concepts, split responsibilities

## Sources
- https://martinfowler.com/articles/evodb.html
- Adam Tornhill — *Your Code as a Crime Scene*

---

# 8️⃣ Event Coupling (Event-Driven Coupling)

Event-driven architecture often **reduces visible coupling** (no direct calls),
but can increase **implicit coupling** if contracts and semantics are weak.

## Severity
🔴 High → 🔥 Critical when events become de-facto public APIs with many consumers.

## 8.1 Event Schema Coupling
### Definition
Consumers depend on event payload structure.

### ❌ Bad Example (generic, ambiguous payload)
```json
{ "type": "Order", "status": 3, "flagA": true }
```

### ✅ Good Example (explicit event contract)
```csharp
public record OrderDeliveredEvent(Guid OrderId, DateTime DeliveredAtUtc);
```

### Mitigation
- Explicit event types
- Versioning (e.g., `OrderDelivered.v1`, `OrderDelivered.v2`)

---

## 8.2 Temporal Event Coupling
### Definition
Consumers assume events arrive in a specific order.

### ❌ Bad Example
Consumer expects `OrderValidated` then `OrderDelivered` always.
If one is delayed/lost → inconsistent state.

### ✅ Good Example
- use idempotency
- rebuildable projections (event stream / log)
- include sequence/version info where appropriate

---

## 8.3 Cascade Coupling (Event Chains)
### Definition
One event triggers a chain reaction across services, forming an implicit workflow.

### ❌ Bad Example (domino chain)
`OrderCreated → PricingCalculated → InvoiceCreated → AccountingExported → NotificationSent`

### ✅ Good Example
- Orchestrate explicitly with a **Process Manager / Saga**
- Keep events for facts; use commands for intent

---

## 8.4 Infrastructure Coupling
### Definition
Coupling to broker guarantees (ordering, exactly-once), routing rules, partitions, etc.

### ❌ Bad Example
Business correctness depends on “the broker will deliver exactly once” (rarely true in practice).

### ✅ Good Example
- at-least-once delivery + idempotent handlers
- outbox pattern for atomic publish

---

## Mitigation Checklist (Event Coupling)
- **Idempotency keys** per handler
- **Outbox pattern** (DB transaction + publish)
- **Event versioning** strategy
- **Contract tests** (CDC)
- Limit number of consumers of “core” events
- Prefer explicit domain events over “StatusChanged”

## Sources
- https://martinfowler.com/articles/201701-event-driven.html
- https://microservices.io/patterns/data/saga.html
- https://microservices.io/patterns/data/event-sourcing.html

---

# Quick Severity Matrix
| Type | Typical Severity | Hidden? | Most common failure mode |
|------|------------------|---------|---------------------------|
| Structural | 🔴 | No | Hard to test / swap implementations |
| Temporal | 🔴 | Yes | Order-dependent bugs |
| Shared data | 🔥 | Yes | Ownership chaos, ripple changes |
| Schema/format | 🟠→🔴 | Yes | Silent breaking changes |
| Semantic | 🔥 | Very | Business rules drift |
| Control | 🟠 | No | Complexity explosion |
| Logical | 🟠 | Yes | Constant co-deploy |
| Event | 🔴→🔥 | Yes | Contract drift & workflow spaghetti |

---

# Architectural Maturity Model
| Maturity | What “coupling” looks like | Typical move |
|----------|----------------------------|--------------|
| CRUD | structural | introduce abstractions + tests |
| Legacy DB-centric | shared data | define ownership + APIs |
| Naive microservices | schema/event | version contracts + CDC |
| DDD mature | explicit contracts + events | govern boundaries & evolution |

---

# Practical Audit Checklist (fast)
- Where do you have **multiple writers** to the same table/column?
- Where do you use **magic statuses**?
- Which events have **10+ consumers**?
- Which workflows rely on **event ordering**?
- Which changes always touch the same modules? (logical coupling)

---
