# Architecture Glossary (EN)

This glossary defines acronyms, architectural patterns, operational concepts, and delivery practices
used in the architecture reference documents.  
Definitions are concise, practical, and oriented toward real-world usage.

---

## Acronyms & Abbreviations

- **ACID**  
  Atomicity, Consistency, Isolation, Durability. Properties guaranteeing reliable database transactions.

- **ABAC (Attribute-Based Access Control)**  
  Authorization model based on attributes (user, resource, context).

- **ADR (Architecture Decision Record)**  
  Short document capturing an architectural decision, its context, and consequences.

- **API (Application Programming Interface)**  
  Contract allowing systems or components to communicate.

- **CI/CD (Continuous Integration / Continuous Deployment)**  
  Automated pipelines to build, test, and deploy software changes.

- **CQRS (Command Query Responsibility Segregation)**  
  Pattern separating write operations (commands) from read operations (queries).

- **CDC (Change Data Capture)**  
  Technique for tracking and propagating database changes.

- **CSAT (Customer Satisfaction Score)**  
  Metric measuring user satisfaction.

- **DoD (Definition of Done)**  
  Shared checklist defining when a feature or change is considered complete
  (tests, documentation, monitoring, deployability, etc.).

- **DSR (Data Subject Request)**  
  Request from a user to access, modify, or delete personal data.

- **FK (Foreign Key)**  
  Database constraint enforcing referential integrity.

- **IaC (Infrastructure as Code)**  
  Practice of managing infrastructure through versioned code.

- **KMS (Key Management Service)**  
  Service used to manage cryptographic keys.

- **MTTD / MTTR**  
  Mean Time To Detect / Mean Time To Recover from incidents.

- **NPS (Net Promoter Score)**  
  Metric measuring customer loyalty and satisfaction.

- **OIDC (OpenID Connect)**  
  Identity layer built on OAuth 2.0.

- **OAuth2**  
  Authorization framework enabling secure delegated access.

- **OLTP (Online Transaction Processing)**  
  Systems optimized for transactional workloads.

- **PITR (Point-In-Time Recovery)**  
  Ability to restore data to a specific moment in time.

- **RBAC (Role-Based Access Control)**  
  Authorization model based on roles.

- **RPO / RTO**  
  Recovery Point Objective / Recovery Time Objective.

- **SLA / SLO / SLI**  
  Service Level Agreement / Objective / Indicator.

- **SSO (Single Sign-On)**  
  Authentication allowing access to multiple systems with one login.

- **WAF (Web Application Firewall)**  
  Protection against common web attacks.

---

## Architectural Patterns & Design Concepts

- **Adapter Pattern**  
  Translates one interface into another to allow incompatible components to interact.
  Common at integration boundaries.

- **Anti-Corruption Layer (ACL)**  
  Layer protecting the internal domain model from external models by translating concepts.

- **Backpressure**  
  Mechanism slowing down producers when consumers are overloaded.

- **Bulkhead Pattern**  
  Isolation technique preventing failures in one component from impacting others.

- **Canonical Model**  
  Shared data representation used across integrations.

- **Circuit Breaker**  
  Pattern stopping calls to a failing dependency to prevent cascading failures.

- **Event-Driven Architecture**  
  Architecture where systems communicate through events rather than direct calls.

- **Event Sourcing**  
  Pattern where state is derived from a sequence of immutable events.

- **Facade Pattern**  
  Simplified interface over a complex subsystem.

- **Feature Flag (Feature Toggle)**  
  Runtime switch to enable or disable functionality without redeploying.

- **Graceful Degradation**  
  Ability to continue operating with reduced functionality during failures.

- **Hexagonal Architecture (Ports & Adapters)**  
  Architecture separating business logic from external concerns via ports and adapters.

- **Layered Architecture**  
  Separation of concerns into layers (UI, application, domain, infrastructure).

- **Materialized View**  
  Precomputed query result stored for fast access.

- **Policy Pattern**  
  Encapsulation of business or technical rules, often a specialization of Strategy.

- **Read Model**  
  Optimized data representation for queries, often denormalized.

- **Retry Policy**  
  Controlled re-execution of failed operations based on rules.

- **Sharding**  
  Horizontal partitioning of data across nodes.

- **Stateless Service**  
  Service that does not retain client state between requests.

- **Strategy Pattern**  
  Pattern enabling dynamic selection of algorithms or behaviors.

---

## Operations & Reliability

- **Runbook**  
  Step-by-step operational guide for running, troubleshooting, and recovering a system.

- **SOP (Standard Operating Procedure)**  
  Documented process for routine operational tasks.

- **On-call**  
  Operational model where team members respond to incidents during defined periods.

- **Incident Management**  
  Process for detecting, resolving, and learning from production incidents.

- **Blameless Postmortem**  
  Incident review focused on systemic improvements rather than individual fault.

- **Error Budget**  
  Acceptable amount of unreliability defined by SLOs.

- **Fallback**  
  Alternative behavior used when a primary operation fails.

- **Load Shedding**  
  Intentional dropping of requests to preserve system stability.

- **Operational Readiness**  
  State in which a system is ready to be safely operated in production.

---

## Delivery & Quality Practices

- **Backward Compatibility**  
  Ability to support older clients or data formats after changes.

- **Blue/Green Deployment**  
  Deployment strategy using two environments to allow fast rollback.

- **Canary Deployment**  
  Gradual rollout to a subset of users to reduce risk.

- **Rolling Deployment**  
  Incremental update of instances without full downtime.

- **Fail Fast**  
  Principle of detecting and reporting errors as early as possible.

- **Hotfix**  
  Urgent production fix applied outside the normal release cycle.

- **Progressive Delivery**  
  Controlled rollout techniques combining feature flags and gradual exposure.

- **Schema Evolution**  
  Changing data schemas while preserving compatibility.

- **Shift Left**  
  Moving testing, security, and validation earlier in the development lifecycle.

- **Technical Debt**  
  Future cost caused by deferred quality or design decisions.

---
