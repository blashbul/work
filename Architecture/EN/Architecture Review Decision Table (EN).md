# Architecture Review Decision Table (EN)
*Essential property → typical decisions → risks if ignored → indicators*

Use this table during architecture reviews, audits, or design workshops.  
For each property, confirm: **decisions are explicit**, **risks are accepted/mitigated**, and **indicators are measurable**.

---

## 1. Reliability and Accuracy
- **Typical architectural decisions**
  - Idempotency keys, de-duplication, safe retries for transient faults.
  - Clear transaction boundaries; ACID where required; compensations otherwise.
  - Strong validation at boundaries + database constraints for invariants.
- **Risks if ignored**
  - Duplicate effects (double billing, double shipment creation), silent data corruption, inconsistent states.
- **Indicators (examples)**
  - Error rate, retry rate, reconciliation incidents, data correction tickets, defect leakage.

---

## 2. Usability
- **Typical architectural decisions**
  - Consistent UI patterns; guided flows for common tasks; meaningful error messages.
  - Accessibility-ready components; keyboard navigation; semantic HTML.
  - API ergonomics: pagination, filtering, bulk operations aligned with user workflows.
- **Risks if ignored**
  - User mistakes, poor adoption, support overload, workarounds outside the system.
- **Indicators (examples)**
  - Task completion time, user error rate, support tickets per feature, NPS/CSAT.

---

## 3. Security
- **Typical architectural decisions**
  - AuthN: OIDC/OAuth2/SSO; token lifetimes; device/session policies.
  - AuthZ: RBAC/ABAC; multi-tenant isolation (row-level filters, tenant boundaries).
  - Secrets management (vault/KMS), encryption at rest/in transit, rate limiting and WAF.
- **Risks if ignored**
  - Data breaches, privilege escalation, regulatory exposure, service disruption.
- **Indicators (examples)**
  - Security incidents, failed auth attempts, vuln backlog age, audit findings.

---

## 4. Scalability
- **Typical architectural decisions**
  - Stateless services; horizontal scaling; load balancing.
  - Asynchronous processing (queues/workers) + backpressure.
  - Data partitioning strategy; caching and read models.
- **Risks if ignored**
  - Performance collapse with growth, expensive re-architecture, operational instability.
- **Indicators (examples)**
  - Throughput under load, saturation metrics (CPU, IO), queue depth, p95 latency vs load.

---

## 5. Interoperability
- **Typical architectural decisions**
  - Integration style: APIs vs events vs file exchange; adapters at boundaries.
  - Contract versioning rules; backward compatibility; deprecation policy.
  - Error semantics: retries, timeouts, reconciliation jobs.
- **Risks if ignored**
  - Tight coupling, breaking changes, fragile integrations, “domino failures”.
- **Indicators (examples)**
  - Integration incident rate, breaking changes frequency, time-to-integrate a new consumer.

---

## 6. Maintainability
- **Typical architectural decisions**
  - Modular boundaries (modules/contexts), layering, dependency rules.
  - Architectural conventions (error handling, logging, DI, naming).
  - ADRs for decisions and a managed technical debt backlog.
- **Risks if ignored**
  - Slow changes, fear-driven development, rising defect rate, knowledge silos.
- **Indicators (examples)**
  - Lead time for change, change failure rate, cyclomatic complexity trends, onboarding time.

---

## 7. Performance
- **Typical architectural decisions**
  - Fit-for-purpose storage (OLTP/search/cache/time-series).
  - Read models (CQRS/materialized views) for read-heavy workloads.
  - Indexing and query optimization; async I/O; concurrency control strategy.
- **Risks if ignored**
  - Timeouts, poor UX, high infrastructure cost, inability to meet SLAs.
- **Indicators (examples)**
  - p50/p95/p99 latency, slow query count, cache hit rate, cost per request.

---

## 8. Flexibility and Adaptability
- **Typical architectural decisions**
  - Configuration-driven behavior; feature flags; progressive delivery.
  - Policy/strategy patterns; extension points rather than forks.
  - Clear separation of business rules from infrastructure concerns.
- **Risks if ignored**
  - “Hard-coded” system, expensive change requests, brittle releases.
- **Indicators (examples)**
  - Time-to-add a rule, number of hotfixes, feature flag usage and retirement rate.

---

## 9. Data Management
- **Typical architectural decisions**
  - Define “source of truth” per dataset; ownership and responsibilities.
  - Constraints (FK/unique), validation pipelines, schema evolution strategy.
  - Lifecycle: retention, archiving, deletion/erasure, anonymization.
- **Risks if ignored**
  - Inconsistent reports, duplicated masters, data quality issues, compliance failures.
- **Indicators (examples)**
  - Data quality metrics (duplicates, null rates), reconciliation gaps, retention violations.

---

## 10. Cost-Effectiveness
- **Typical architectural decisions**
  - Build vs buy; managed services where it reduces total cost of ownership.
  - Right-sizing, autoscaling, retention policies for logs/storage.
  - Complexity budget: avoid distributed systems unless justified.
- **Risks if ignored**
  - Runaway cloud bills, over-engineering, high operational burden.
- **Indicators (examples)**
  - Cost per transaction/user, infra utilization, spend anomaly alerts, ops hours per month.

---

## 11. Compliance
- **Typical architectural decisions**
  - Privacy-by-design: minimization, encryption, pseudonymization.
  - Residency, retention, consent capture and propagation.
  - Control mapping to standards (GDPR/ISO) + evidence generation.
- **Risks if ignored**
  - Legal exposure, inability to pass audits, forced rework under pressure.
- **Indicators (examples)**
  - Audit findings, time-to-produce evidence, data retention conformance, DSR (data subject request) lead time.

---

## 12. Auditability
- **Typical architectural decisions**
  - Structured audit logs with immutability and retention policies.
  - Correlation IDs and consistent metadata across services.
  - Change tracking for critical entities (history tables / CDC / event sourcing where relevant).
- **Risks if ignored**
  - No traceability, disputes impossible to resolve, compliance gaps.
- **Indicators (examples)**
  - Percentage of critical actions audited, audit log completeness, trace coverage.

---

## 13. Support and Documentation
- **Typical architectural decisions**
  - “Docs as product”: ADRs, runbooks, onboarding guides; definition of done includes docs.
  - On-call model and escalation paths; incident processes.
  - Support tooling: admin screens, safe remediation commands.
- **Risks if ignored**
  - Tribal knowledge, slow incident response, operator errors.
- **Indicators (examples)**
  - Mean time to detect/resolve (MTTD/MTTR), runbook coverage, onboarding time.

---

## 14. Backup and Recovery
- **Typical architectural decisions**
  - Define RPO/RTO targets and design backups accordingly.
  - PITR (point-in-time restore), cross-region copies for critical data.
  - Automated restore testing and DR runbooks.
- **Risks if ignored**
  - Permanent data loss, long outages, chaotic recovery.
- **Indicators (examples)**
  - Backup success rate, restore test success rate, measured RPO/RTO during drills.

---

## 15. Resilience
- **Typical architectural decisions**
  - Timeouts, circuit breakers, bulkheads, load shedding.
  - Redundancy (multi-AZ), dependency isolation, graceful degradation.
  - Backpressure and queue-based smoothing for spikes.
- **Risks if ignored**
  - Cascading failures, global outages from a single dependency, unstable behavior under stress.
- **Indicators (examples)**
  - Error budget burn, dependency failure impact, rate of cascading incidents.

---

## 16. Observability and Operability
- **Typical architectural decisions**
  - Centralized logs, metrics, traces; consistent schema and correlation IDs.
  - SLO/SLI definition; alerting on symptoms; dashboards for operations.
  - Health checks, readiness/liveness, synthetic monitoring.
- **Risks if ignored**
  - “Black box” system, long outages, noisy alerts, slow diagnosis.
- **Indicators (examples)**
  - Trace coverage, alert precision/recall, MTTD/MTTR, dashboard adoption.

---

## 17. Testability
- **Typical architectural decisions**
  - Test pyramid strategy: unit/integration/e2e targeted to risk.
  - Contract tests for APIs/events; production-like test environments.
  - Deterministic fixtures and seeded datasets.
- **Risks if ignored**
  - Fragile releases, regression storms, slow delivery, fear of refactoring.
- **Indicators (examples)**
  - Test duration, flaky test rate, coverage of critical flows, change failure rate.

---

## 18. Deployability
- **Typical architectural decisions**
  - CI/CD pipelines; automated rollbacks; blue/green/canary strategies.
  - IaC (infrastructure as code) + versioned config; immutable artifacts.
  - Backward-compatible DB migrations and feature flags.
- **Risks if ignored**
  - Risky deployments, long freeze periods, slow recovery from bad releases.
- **Indicators (examples)**
  - Deployment frequency, change failure rate, rollback time, lead time for change.

---
