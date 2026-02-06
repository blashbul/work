# Architectural Decisions Map (EN)
*Linking essential system properties to typical architectural decisions*

This document connects each essential property to **common architectural decisions** you can make during design or modernization.  
It can be used as a checklist to justify architecture choices and make trade-offs explicit.

---

## 1. Reliability and Accuracy

- **Decision: Idempotency and de-duplication**  
  Use idempotency keys, deduplicate messages/events, and design commands to be safe to replay.
- **Decision: Transaction boundaries and consistency model**  
  Choose ACID transactions where needed; otherwise apply eventual consistency with explicit compensations.
- **Decision: Input validation and invariants**  
  Enforce invariants at boundaries (API) and at the data layer (constraints) to prevent invalid states.
- **Decision: Error handling strategy**  
  Standardize error categories (transient vs permanent), retries, and safe fallbacks.

---

## 2. Usability

- **Decision: UX patterns and navigation model**  
  Choose consistent layout, navigation, and interaction patterns; reduce steps for common flows.
- **Decision: Accessibility-first UI components**  
  Adopt component libraries with accessibility support; define keyboard and screen-reader requirements.
- **Decision: API ergonomics for clients**  
  Design APIs that match user workflows (batch endpoints, search/filter, pagination, clear error messages).

---

## 3. Security

- **Decision: Authentication mechanism**  
  Choose OIDC/OAuth2 vs SSO vs mutual TLS depending on users, apps, and trust boundaries.
- **Decision: Authorization model**  
  RBAC vs ABAC (claims/policies), plus multi-tenant isolation rules if applicable.
- **Decision: Secrets and key management**  
  Use a vault/KMS, rotate keys, and avoid secrets in source control or config files.
- **Decision: Defense-in-depth controls**  
  WAF/rate limiting, input validation, secure headers, dependency scanning, and least privilege.

---

## 4. Scalability

- **Decision: Stateless vs stateful service design**  
  Prefer stateless services; externalize state (DB/cache/object storage) to scale horizontally.
- **Decision: Data partitioning strategy**  
  Choose partitioning/sharding keys; isolate hot tenants or high-volume datasets.
- **Decision: Asynchronous processing**  
  Offload heavy tasks to queues/workers; use backpressure and retry policies.
- **Decision: Caching strategy**  
  Apply read-through caches, CDN, or computed views to reduce expensive operations.

---

## 5. Interoperability

- **Decision: Integration style**  
  Point-to-point APIs vs event-driven integration vs file-based exchange (depending on constraints).
- **Decision: Contract and versioning policy**  
  Version APIs/events; define backward compatibility rules and deprecation timelines.
- **Decision: Canonical data model vs translation at edges**  
  Decide whether to standardize formats or map per integration boundary.
- **Decision: Error and retry semantics across systems**  
  Define timeout, retry, and reconciliation strategy for external dependencies.

---

## 6. Maintainability

- **Decision: Modular structure and boundaries**  
  Define modules/bounded contexts; keep responsibilities cohesive and dependencies directional.
- **Decision: Coding standards and architectural conventions**  
  Enforce consistent patterns (layering, naming, error handling, logging, DI).
- **Decision: Replaceability of components**  
  Prefer interfaces and adapters at integration points; avoid leaking vendor specifics.
- **Decision: Technical debt management**  
  Create ADRs, maintain “debt register”, and allocate refactoring budget.

---

## 7. Performance

- **Decision: Storage technology choices**  
  OLTP DB vs search engine vs time-series DB vs cache depending on query patterns.
- **Decision: Read model strategy**  
  Use denormalized read models, materialized views, CQRS patterns when reads dominate.
- **Decision: Query/index design**  
  Create covering indexes, partition large tables, optimize critical query paths.
- **Decision: Concurrency and async**  
  Use non-blocking I/O, limit lock contention, apply optimistic concurrency when appropriate.

---

## 8. Flexibility and Adaptability

- **Decision: Configuration vs code**  
  Externalize business parameters (thresholds, routing rules, toggles) into configuration or rule engines.
- **Decision: Extension points and plugin approach**  
  Provide hooks for new behaviors (strategies, policies) without modifying core flows.
- **Decision: Domain modeling approach**  
  Choose richer domain model where business rules matter; keep CRUD-only areas simple.
- **Decision: Feature release strategy**  
  Use feature flags, progressive delivery, and incremental rollouts.

---

## 9. Data Management

- **Decision: Source of truth and ownership**  
  Define which system owns which data; avoid multiple masters unless explicitly reconciled.
- **Decision: Data constraints and governance rules**  
  Use schema constraints, unique keys, referential integrity, and validation pipelines.
- **Decision: Lifecycle management**  
  Choose retention periods, archiving, deletion/erasure processes, and data anonymization.
- **Decision: Data access patterns**  
  Decide between shared database, API-only access, or read replicas depending on coupling tolerance.

---

## 10. Cost-Effectiveness

- **Decision: Build vs buy**  
  Choose managed services or commercial products when operational cost is lower than maintaining custom code.
- **Decision: Right-sizing and autoscaling**  
  Define scaling policies; avoid over-provisioning; use scheduled scaling where relevant.
- **Decision: Architecture complexity budget**  
  Avoid distributed architecture unless justified by scale, reliability, or org needs.
- **Decision: Observability and cost controls**  
  Tag resources, monitor spend, set budgets and alerts, optimize storage/retention.

---

## 11. Compliance

- **Decision: Privacy-by-design patterns**  
  Minimize data, encrypt sensitive fields, pseudonymize identifiers, separate duties.
- **Decision: Data residency and retention policy**  
  Choose regions, storage classes, and retention windows aligned to legal constraints.
- **Decision: Consent and purpose enforcement**  
  Store consent, enforce allowed uses, and ensure downstream propagation rules.
- **Decision: Evidence and control mapping**  
  Map controls to standards (e.g., GDPR articles, ISO 27001 controls) via documented policies.

---

## 12. Auditability

- **Decision: Audit log strategy**  
  Decide what to log (who/what/when/where), log immutability, and retention policy.
- **Decision: Correlation and traceability**  
  Use correlation IDs, request IDs, and consistent event metadata across systems.
- **Decision: Change tracking**  
  Use data change history tables, event sourcing, or CDC for critical entities.
- **Decision: Access to audit evidence**  
  Provide reporting/export mechanisms without privileged DB access.

---

## 13. Support and Documentation

- **Decision: Documentation standards**  
  Define architecture docs (ADRs), runbooks, and onboarding guides as part of “definition of done”.
- **Decision: Operational ownership model**  
  Assign on-call responsibilities, escalation paths, and incident response procedures.
- **Decision: Support tooling**  
  Provide admin screens, support dashboards, and safe remediation actions.

---

## 14. Backup and Recovery

- **Decision: Recovery objectives**  
  Define **RPO/RTO** targets and design backups/replication accordingly.
- **Decision: Backup architecture**  
  Choose snapshots, incremental backups, point-in-time restore, cross-region replication.
- **Decision: Restore verification**  
  Automate restore tests; document disaster recovery runbooks.
- **Decision: Dependency recovery plan**  
  Ensure external dependencies (secrets, configs, queues) are also recoverable.

---

## 15. Resilience

- **Decision: Failure isolation**  
  Bulkheads, per-tenant isolation, circuit breakers, and dependency timeouts.
- **Decision: Redundancy**  
  Multi-instance services, multi-AZ deployments, and redundant networking.
- **Decision: Graceful degradation**  
  Prioritize critical flows; degrade non-essential features; implement fallbacks.
- **Decision: Capacity protection**  
  Rate limiting, queue backpressure, and load shedding.

---

## 16. Observability and Operability

- **Decision: Observability stack**  
  Centralized logging, metrics, tracing, and dashboards; consistent log schema.
- **Decision: SLO/SLI definition**  
  Define measurable service objectives; alert on symptoms, not noise.
- **Decision: Operational safety**  
  Safe configuration changes, feature flags, automated rollbacks, and runbook-driven ops.
- **Decision: Health endpoints**  
  Liveness/readiness checks, dependency checks, and synthetic monitoring.

---

## 17. Testability

- **Decision: Testing strategy by layer**  
  Unit tests for rules, integration tests for boundaries, end-to-end tests for key journeys.
- **Decision: Test environments and data**  
  Production-like environments; seeded datasets; deterministic fixtures.
- **Decision: Contract testing**  
  API/event contracts verified automatically between producers and consumers.
- **Decision: Dependency control**  
  Use test doubles where appropriate; prefer real infrastructure for integration tests.

---

## 18. Deployability

- **Decision: Release strategy**  
  Blue/green, canary, rolling deployments based on risk profile and system criticality.
- **Decision: Infrastructure as Code**  
  Version environments; reproducible provisioning; immutable builds where possible.
- **Decision: Configuration management**  
  Separate config from code; version config; manage secrets securely.
- **Decision: Rollback and rollback safety**  
  Backward-compatible DB migrations; feature flags; fast rollback procedures.

---
