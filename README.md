# 🚀 Software Architecture Patterns Evolution

### A Journey from Monolith to Service-Oriented Architecture (SOA)

This repository demonstrates the evolution of a **.NET 10 Ordering System** through various architectural patterns. Each folder represents a stage in the application's maturity, solving specific problems while introducing new trade-offs.

---

## 🗺️ The Evolution Path

1. **Monolith** ➡️ 2. **Layered** ➡️ 3. **Modular Monolith** ➡️ 4. **Pipeline** ➡️ 5. **Micro-kernel** ➡️ 6. **Event-Driven** ➡️ 7. **SOA**

---

## 1. Monolith (The Big Ball of Mud)

A single codebase deployed as a single unit.

* **Pros:** Single versioning, easy deployment to client environments, consistent interface.
* **Cons:** Scaling requires redeploying everything; technical debt grows as behaviors intertwine.
* **Best Use-Case:** New projects where component boundaries aren't clear yet.

---

## 2. Layered Architecture (The Lasagna)

Organizes code into horizontal tiers based on technology or function (Data, Business, UI).

* **Pros:** Clear delineation of responsibilities; easy to locate where new code goes.
* **Cons:** Hard to scale individual features; changes often ripple through all layers.
* **Best Use-Case:** Separating concerns within a component to allow for easier mocking and testing.

---

## 3. Modular Monolith

A step toward microservices where pieces are split into components but still deployed together.

* **Pros:** Loose coupling; easier component replacement compared to a standard monolith.
* **Cons:** Inherits the worst of both worlds; requires redeploying everything and complex integration testing.
* **Best Use-Case:** A transition state when moving from a monolith to microservices.

---

## 4. Pipeline Architecture (The Assembly Line)

A linear process where data flows through a series of sequential components.

* **Pros:** Ideal for complex workflows like data ingestion, cleansing, and transformation.
* **Cons:** Processing can be slower as each step waits for the previous one to complete.
* **Best Use-Case:** Complex steps that can be separated and scaled individually.

---

## 5. Micro-kernel (Plug-in Architecture)

A minimal core system augmented by plug-in components for specific tasks.

* **Pros:** Core remains stable; plugins can grow organically and be deployed individually.
* **Cons:** Hard to decide what belongs in the core vs. a plugin; can lead to code duplication.
* **Best Use-Case:** Systems with diverse, quickly evolving functions (e.g., tax calculation for different regions).

---

## 6. Event-Driven Architecture

Asynchronous communication where components publish events to a service bus for others to consume.

* **Pros:** Very loose coupling; components can process data at different speeds; high availability.
* **Cons:** "Eventually consistent" state; difficult to reconstruct current state without replaying event history.
* **Best Use-Case:** Asynchronous systems where dependencies need to be separated to compensate for downtime.

---

## 7. Service-Oriented Architecture (SOA)

An organizational pattern focused on creating reusable "enterprise services" to avoid reinventing the wheel.

* **Pros:** Operational efficiency; removes duplication of features like email distribution across a company.
* **Cons:** Teams lose control over speed and quality; technological stagnation if common services don't evolve.
* **Best Use-Case:** Large organizations with great inter-team communication and a shared-service mindset.

---

## 🏁 Summary Comparison

| Pattern | Deployment | Coupling | Scaling | Focus |
| --- | --- | --- | --- | --- |
| **Monolith** | Single Unit | High | Hard | Simplicity |
| **Layered** | Single Unit | Moderate | Hard | Technical Tiers |
| **Modular** | Single Unit | Low | Hard | Transitioning |
| **Pipeline** | Variable | Moderate | Individual Steps | Linear Workflow |
| **Micro-kernel** | Minimal Core | Very Low | Plugins Only | Extensibility |
| **Event-Driven** | Distributed | Lowest | High | Asynchrony |
| **SOA** | Distributed | Moderate | Enterprise-wide | Reusability |

---

### How to Run

1. Clone the repository.
2. Navigate to any folder (e.g., `01-Spaghetti-Monolith`).
3. Run `dotnet run` inside the project folder.

