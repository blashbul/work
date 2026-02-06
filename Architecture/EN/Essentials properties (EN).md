# Essential Properties of an Information System (EN)

This document describes the essential properties an information system must exhibit in order to be reliable, usable, secure, and sustainable.  
These properties can be used both as evaluation criteria and as design guidelines.

---

## 1. Reliability and Accuracy

- **Reliability**: The system consistently performs its intended functions under expected operating conditions.  
  Failures are rare, predictable, and do not leave the system in an inconsistent state.

- **Accuracy**: The information produced by the system is correct, precise, and free from errors.  
  Given the same inputs and context, the system produces the same outputs.

---

## 2. Usability

- **User-Friendly Interface**: The system is easy to navigate and use, even for non-technical users.  
  Interactions are intuitive and minimize user errors.

- **Accessibility**: The system is accessible to all authorized users, including users with disabilities.  
  Accessibility requirements are considered from the design phase.

---

## 3. Security

- **Confidentiality**: Information is accessible only to authorized users and systems.  
  Access control rules are clearly defined and enforced.

- **Integrity**: Information and system behavior are protected from unauthorized modification.  
  Data corruption and unintended changes are prevented or detected.

- **Availability**: Information and resources remain available when needed.  
  The system is protected against misuse, attacks, and excessive load.

---

## 4. Scalability

- The system can handle increasing amounts of data, users, and workload without unacceptable performance degradation.
- Scaling can be achieved without major architectural redesign.
- Growth does not disproportionately increase complexity or cost.

---

## 5. Interoperability

- The system can work seamlessly with other systems through well-defined interfaces.
- Data exchange and integration do not require tight coupling.
- Changes in connected systems have limited impact.

---

## 6. Maintainability

- The system is designed so that it can be easily understood, updated, and repaired.
- Changes can be applied incrementally with limited risk.
- Technical debt is identified and kept under control.

---

## 7. Performance

- **Efficiency**: The system uses resources (CPU, memory, storage, network) efficiently.  
  Resource usage is proportional to actual demand.

- **Response Time**: The system provides timely responses to user queries and transactions.  
  Performance remains stable under expected load.

---

## 8. Flexibility and Adaptability

- The system can adapt to evolving business needs and environments.
- New requirements can be addressed without destabilizing existing functionality.
- Architectural choices favor evolution over rigidity.

---

## 9. Data Management

- **Data Quality**: Data is accurate, complete, consistent, and trustworthy.
- **Data Storage**: Data is stored in a way that ensures integrity, durability, and efficient retrieval.
- **Data Processing**: Data is processed accurately and efficiently to generate reliable information.
- **Data Lifecycle**: The creation, modification, and deletion of data are controlled and predictable.

---

## 10. Cost-Effectiveness

- The system provides good value relative to its development and operational costs.
- Costs are predictable and proportional to system usage and scale.
- The system can evolve without uncontrolled cost increases.

---

## 11. Compliance

- The system adheres to relevant laws, regulations, and standards (e.g. data protection regulations).
- Compliance requirements are integrated into system design.
- Compliance does not excessively hinder usability or performance.

---

## 12. Auditability

- The system records relevant actions, events, and data changes.
- Logs and audit trails allow operations to be traced and verified.
- Audit information can be accessed without disrupting operations.

---

## 13. Support and Documentation

- Documentation explains system usage, behavior, and limitations clearly.
- Technical and operational documentation supports maintenance and troubleshooting.
- Users and operators have access to appropriate support mechanisms.

---

## 14. Backup and Recovery

- The system includes reliable data backup mechanisms.
- Recovery procedures are defined, documented, and tested.
- Data and services can be restored within acceptable timeframes after failures or disasters.

---

## 15. Resilience

- The system tolerates partial failures and unexpected conditions.
- Failures are isolated to prevent cascading effects.
- When operating under degraded conditions, the system fails gracefully and recovers predictably.

---

## 16. Observability and Operability

- The system exposes sufficient information about its internal state and behavior.
- Logs, metrics, and traces allow issues to be detected and diagnosed in production.
- Operational actions are safe, controlled, and predictable.

---

## 17. Testability

- The system supports automated and repeatable testing.
- Components can be tested in isolation and as a whole.
- Defects can be reproduced, diagnosed, and fixed reliably.

---

## 18. Deployability

- The system can be deployed, updated, and rolled back in a controlled and repeatable manner.
- Deployment processes are automated as much as possible.
- Environments are consistent and reproducible.

---

A system that satisfies these properties is more likely to be reliable, evolvable, and sustainable over time.
