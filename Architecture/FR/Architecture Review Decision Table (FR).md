# Table de décision pour revue d’architecture (FR)
*Propriété essentielle → décisions typiques → risques si ignorée → indicateurs*

Utilise cette table lors des revues d’architecture, audits ou ateliers de conception.  
Pour chaque propriété, vérifie : **les décisions sont explicites**, **les risques sont acceptés/mitigés**, et **les indicateurs sont mesurables**.

---

## 1. Fiabilité et exactitude
- **Décisions d’architecture typiques**
  - Clés d’idempotence, déduplication, retries sûrs pour erreurs transitoires.
  - Frontières transactionnelles claires ; ACID quand nécessaire ; compensations sinon.
  - Validation aux frontières + contraintes en base (invariants).
- **Risques si ignorée**
  - Effets en double (double facturation), corruption silencieuse, états incohérents.
- **Indicateurs (exemples)**
  - Taux d’erreurs, taux de retries, incidents de réconciliation, tickets de correction de données.

---

## 2. Utilisabilité
- **Décisions d’architecture typiques**
  - Patterns UI cohérents ; parcours guidés ; erreurs explicites et actionnables.
  - Composants accessibles ; navigation clavier ; compatibilité lecteur d’écran.
  - APIs ergonomiques : pagination, filtres, opérations en lot adaptées aux parcours utilisateurs.
- **Risques si ignorée**
  - Erreurs utilisateur, faible adoption, surcharge support, contournements hors système.
- **Indicateurs (exemples)**
  - Temps de réalisation d’une tâche, taux d’erreur utilisateur, tickets support par fonctionnalité, CSAT.

---

## 3. Sécurité
- **Décisions d’architecture typiques**
  - AuthN : OIDC/OAuth2/SSO ; durées de tokens ; politiques de session.
  - AuthZ : RBAC/ABAC ; isolation multi-tenant (filtres, frontières).
  - Secrets (vault/KMS), chiffrement repos/transit, rate limiting/WAF.
- **Risques si ignorée**
  - Fuite de données, escalade de privilèges, non-conformité, indisponibilité.
- **Indicateurs (exemples)**
  - Incidents sécurité, tentatives auth échouées, âge du backlog de vulnérabilités, findings d’audit.

---

## 4. Scalabilité
- **Décisions d’architecture typiques**
  - Services stateless ; scaling horizontal ; load balancing.
  - Traitements asynchrones (queues/workers) + backpressure.
  - Partitionnement des données ; caches et read models.
- **Risques si ignorée**
  - Effondrement des perfs en croissance, refonte coûteuse, instabilité.
- **Indicateurs (exemples)**
  - Débit sous charge, saturation CPU/IO, profondeur de queue, latence p95 vs charge.

---

## 5. Interopérabilité
- **Décisions d’architecture typiques**
  - APIs vs événements vs fichiers ; adaptateurs aux frontières.
  - Règles de versioning ; compatibilité backward ; politique de dépréciation.
  - Sémantiques d’erreur : timeouts, retries, jobs de réconciliation.
- **Risques si ignorée**
  - Couplage fort, breaking changes, intégrations fragiles, pannes en cascade.
- **Indicateurs (exemples)**
  - Incidents d’intégration, fréquence des breaking changes, temps d’intégration d’un nouveau consommateur.

---

## 6. Maintenabilité
- **Décisions d’architecture typiques**
  - Modularisation (modules/BC), couches, règles de dépendances.
  - Conventions (gestion d’erreurs, logs, DI, nommage).
  - ADRs + backlog de dette technique piloté.
- **Risques si ignorée**
  - Changements lents, hausse des bugs, silos de connaissance, peur de toucher au code.
- **Indicateurs (exemples)**
  - Lead time, change failure rate, tendances de complexité, temps d’onboarding.

---

## 7. Performance
- **Décisions d’architecture typiques**
  - Stockage adapté (OLTP/search/cache/time-series).
  - Read models (CQRS/vues matérialisées) si lecture dominante.
  - Index/requêtes optimisés ; I/O async ; stratégie de concurrence.
- **Risques si ignorée**
  - Timeouts, UX dégradée, coûts infra élevés, impossibilité de tenir les SLA.
- **Indicateurs (exemples)**
  - Latence p50/p95/p99, requêtes lentes, hit rate cache, coût par requête.

---

## 8. Flexibilité et adaptabilité
- **Décisions d’architecture typiques**
  - Paramétrage par configuration ; feature flags ; progressive delivery.
  - Patterns policy/strategy ; points d’extension plutôt que forks.
  - Séparation des règles métier et de l’infrastructure.
- **Risques si ignorée**
  - Système “hard-coded”, changements coûteux, releases fragiles.
- **Indicateurs (exemples)**
  - Temps d’ajout d’une règle, nombre de hotfixes, taux d’usage des feature flags.

---

## 9. Gestion des données
- **Décisions d’architecture typiques**
  - Source de vérité et ownership par dataset.
  - Contraintes (FK/unique), validation, stratégie d’évolution de schéma.
  - Cycle de vie : rétention, archivage, suppression/effacement, anonymisation.
- **Risques si ignorée**
  - Données incohérentes, plusieurs “masters”, qualité dégradée, non-conformité.
- **Indicateurs (exemples)**
  - Duplicats, taux de nulls, écarts de réconciliation, violations de rétention.

---

## 10. Maîtrise des coûts
- **Décisions d’architecture typiques**
  - Build vs buy ; services managés si TCO inférieur.
  - Right-sizing/autoscaling ; politiques de rétention logs/stockage.
  - Budget de complexité : éviter le distribué sans justification.
- **Risques si ignorée**
  - Factures incontrôlées, sur-ingénierie, charge d’exploitation trop forte.
- **Indicateurs (exemples)**
  - Coût par transaction/utilisateur, utilisation infra, alertes anomalies de dépenses, heures ops/mois.

---

## 11. Conformité
- **Décisions d’architecture typiques**
  - Privacy by design : minimisation, chiffrement, pseudonymisation.
  - Résidence/rétention ; capture du consentement et propagation.
  - Mapping contrôles ↔ exigences (RGPD/ISO) + production de preuves.
- **Risques si ignorée**
  - Risque légal, échec d’audit, refonte forcée en urgence.
- **Indicateurs (exemples)**
  - Findings d’audit, temps pour produire une preuve, conformité rétention, délai de traitement DSR.

---

## 12. Auditabilité
- **Décisions d’architecture typiques**
  - Logs d’audit structurés, immuables, rétention définie.
  - Correlation IDs et métadonnées standardisées.
  - Historisation (tables d’historique / CDC / event sourcing selon criticité).
- **Risques si ignorée**
  - Pas de traçabilité, litiges non résolvables, gaps conformité.
- **Indicateurs (exemples)**
  - % actions critiques auditées, complétude des logs, couverture de traçage.

---

## 13. Support et documentation
- **Décisions d’architecture typiques**
  - Documentation “as product” : ADRs, runbooks, onboarding, DoD inclut la doc.
  - Ownership ops : astreinte, escalade, procédure incident.
  - Outillage support : écrans admin, dashboards, actions sûres.
- **Risques si ignorée**
  - Connaissance “tribale”, incidents longs, erreurs d’exploitation.
- **Indicateurs (exemples)**
  - MTTD/MTTR, couverture runbooks, temps d’onboarding.

---

## 14. Sauvegarde et reprise
- **Décisions d’architecture typiques**
  - Définir RPO/RTO puis dimensionner la stratégie.
  - PITR, copies multi-régions pour données critiques.
  - Tests de restore automatisés + runbooks DR.
- **Risques si ignorée**
  - Perte de données, indisponibilité prolongée, reprise chaotique.
- **Indicateurs (exemples)**
  - Taux de succès backups, succès tests de restore, RPO/RTO mesurés en exercice.

---

## 15. Résilience
- **Décisions d’architecture typiques**
  - Timeouts, circuit breakers, bulkheads, load shedding.
  - Redondance (multi-AZ), isolation dépendances, dégradation contrôlée.
  - Backpressure et lissage via queues.
- **Risques si ignorée**
  - Pannes en cascade, outage global sur une dépendance, instabilité sous stress.
- **Indicateurs (exemples)**
  - Burn de l’error budget, impact des pannes dépendances, fréquence des incidents en cascade.

---

## 16. Observabilité et exploitabilité
- **Décisions d’architecture typiques**
  - Centralisation logs/métriques/traces ; schéma cohérent + corrélation.
  - SLO/SLI ; alerting sur symptômes ; dashboards ops.
  - Health checks, readiness/liveness, monitoring synthétique.
- **Risques si ignorée**
  - Système “boîte noire”, incidents longs, alertes bruyantes, diagnostic lent.
- **Indicateurs (exemples)**
  - Couverture traces, précision alertes, MTTD/MTTR, adoption dashboards.

---

## 17. Testabilité
- **Décisions d’architecture typiques**
  - Stratégie pyramide : unit/integration/e2e ciblée sur le risque.
  - Tests de contrat API/événements ; environnements de test proches prod.
  - Fixtures déterministes et datasets seedés.
- **Risques si ignorée**
  - Releases fragiles, régressions, delivery lent, refactorisation risquée.
- **Indicateurs (exemples)**
  - Durée pipeline, taux de flaky tests, couverture parcours critiques, change failure rate.

---

## 18. Déployabilité
- **Décisions d’architecture typiques**
  - CI/CD ; rollbacks automatisés ; blue/green/canary.
  - IaC + config versionnée ; artefacts immuables.
  - Migrations DB backward-compatible + feature flags.
- **Risques si ignorée**
  - Déploiements risqués, gels prolongés, reprise lente après release ratée.
- **Indicateurs (exemples)**
  - Fréquence déploiement, change failure rate, temps de rollback, lead time.

---
