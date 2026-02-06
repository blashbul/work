# Cartographie des décisions d’architecture (FR)
*Relier les propriétés essentielles d’un SI à des décisions d’architecture typiques*

Ce document relie chaque propriété essentielle à des **décisions d’architecture courantes** lors de la conception ou de la modernisation d’un système.  
Il peut servir de check-list pour justifier les choix et expliciter les compromis.

---

## 1. Fiabilité et exactitude

- **Décision : Idempotence et déduplication**  
  Utiliser des clés d’idempotence, dédupliquer messages/événements, rendre les commandes rejouables sans effet de bord.
- **Décision : Frontières transactionnelles et modèle de cohérence**  
  Choisir ACID lorsque nécessaire ; sinon assumer l’éventual consistency avec compensations explicites.
- **Décision : Validation des entrées et invariants**  
  Valider aux frontières (API) et au niveau stockage (contraintes) pour empêcher les états invalides.
- **Décision : Stratégie de gestion d’erreurs**  
  Standardiser erreurs transitoires vs permanentes, retries, et fallback sécurisés.

---

## 2. Utilisabilité

- **Décision : Patterns UX et modèle de navigation**  
  Définir un layout cohérent, des parcours simples et des standards d’interaction.
- **Décision : Composants UI accessibles**  
  Choisir des composants accessibles, définir exigences clavier/lecteur d’écran.
- **Décision : Ergonomie des APIs côté clients**  
  Concevoir des APIs adaptées aux parcours (batch, recherche/filtre, pagination, erreurs claires).

---

## 3. Sécurité

- **Décision : Mécanisme d’authentification**  
  Choisir OIDC/OAuth2 vs SSO vs mTLS selon les utilisateurs, applications et périmètres de confiance.
- **Décision : Modèle d’autorisation**  
  RBAC vs ABAC (claims/policies), et règles d’isolation multi-tenant si nécessaire.
- **Décision : Gestion des secrets et des clés**  
  Utiliser un vault/KMS, rotation des clés, pas de secrets dans le code ou les fichiers de config.
- **Décision : Défense en profondeur**  
  WAF/rate limiting, validation d’entrée, en-têtes sécurisés, scan des dépendances, moindre privilège.

---

## 4. Scalabilité

- **Décision : Services stateless vs stateful**  
  Privilégier stateless ; externaliser l’état (DB/cache/object storage) pour scaler horizontalement.
- **Décision : Stratégie de partitionnement des données**  
  Choisir des clés de partition/shard ; isoler les “hot tenants” ou les datasets volumineux.
- **Décision : Traitement asynchrone**  
  Déporter les tâches lourdes via queues/workers ; backpressure et politiques de retry.
- **Décision : Stratégie de cache**  
  Cache lecture, CDN, vues calculées pour réduire les opérations coûteuses.

---

## 5. Interopérabilité

- **Décision : Style d’intégration**  
  APIs point-à-point vs event-driven vs échanges fichiers selon contraintes et maturité.
- **Décision : Politique de contrats et versioning**  
  Versionner APIs/événements ; définir compatibilité backward et fenêtres de dépréciation.
- **Décision : Modèle canonique vs traduction aux frontières**  
  Standardiser les formats ou mapper à chaque frontière d’intégration.
- **Décision : Gestion des erreurs inter-systèmes**  
  Définir timeouts, retries, réconciliation et mécanismes de rattrapage.

---

## 6. Maintenabilité

- **Décision : Modularisation et frontières**  
  Définir modules/bounded contexts ; responsabilités cohérentes ; dépendances orientées.
- **Décision : Conventions de code et règles d’architecture**  
  Standardiser patterns (couches, nommage, erreurs, logs, DI).
- **Décision : Remplaçabilité des composants**  
  Utiliser des adaptateurs aux points d’intégration ; éviter de diffuser du vendor-specific.
- **Décision : Gestion de la dette technique**  
  ADRs, registre de dette, budget de refactorisation planifié.

---

## 7. Performance

- **Décision : Choix des technologies de stockage**  
  OLTP vs moteur de recherche vs time-series vs cache selon les patterns de lecture/écriture.
- **Décision : Stratégie de read models**  
  Vues dénormalisées, materialized views, CQRS si les lectures dominent.
- **Décision : Design des requêtes et des index**  
  Index couvrants, partitionnement, optimisation des chemins critiques.
- **Décision : Concurrence et asynchronisme**  
  I/O non bloquantes, réduction de contention, concurrence optimiste si pertinent.

---

## 8. Flexibilité et adaptabilité

- **Décision : Configuration vs code**  
  Externaliser paramètres métier (seuils, règles, toggles) en config ou moteur de règles.
- **Décision : Points d’extension / approche plugin**  
  Stratégies/policies pour ajouter des comportements sans modifier le cœur.
- **Décision : Approche de modélisation**  
  Modèle riche là où il y a des règles métier ; CRUD simple ailleurs.
- **Décision : Stratégie de mise en production**  
  Feature flags, progressive delivery, déploiements incrémentaux.

---

## 9. Gestion des données

- **Décision : Source de vérité et ownership**  
  Définir qui “possède” quelles données ; éviter plusieurs masters sans réconciliation.
- **Décision : Contraintes et règles de qualité**  
  Contraintes de schéma, clés uniques, intégrité référentielle, validations.
- **Décision : Gestion du cycle de vie**  
  Rétention, archivage, suppression/effacement, anonymisation/pseudonymisation.
- **Décision : Accès aux données**  
  DB partagée vs accès par APIs vs read replicas selon la tolérance au couplage.

---

## 10. Maîtrise des coûts

- **Décision : Build vs buy**  
  Utiliser des services managés/produits lorsque le coût global est inférieur au sur-mesure.
- **Décision : Right-sizing et autoscaling**  
  Politiques de montée/descente en charge ; éviter le sur-provisionnement.
- **Décision : Budget de complexité**  
  Éviter le distribué si ce n’est pas justifié par la charge, la fiabilité ou l’organisation.
- **Décision : Pilotage des coûts**  
  Tags, budgets/alertes, optimisation rétention logs/stockage, rationalisation.

---

## 11. Conformité

- **Décision : Privacy by design**  
  Minimiser, chiffrer, pseudonymiser, séparer les responsabilités.
- **Décision : Résidence et rétention des données**  
  Choix régions, classes de stockage, fenêtres de rétention.
- **Décision : Consentement et finalité**  
  Stocker le consentement, appliquer les usages autorisés, propager les règles en aval.
- **Décision : Cartographie des contrôles**  
  Relier exigences (RGPD/ISO/contrats) à des contrôles et preuves documentées.

---

## 12. Auditabilité

- **Décision : Stratégie de logs d’audit**  
  Définir quoi tracer (qui/quoi/quand/où), immutabilité, rétention.
- **Décision : Corrélation et traçabilité**  
  Correlation IDs, request IDs, métadonnées d’événements standardisées.
- **Décision : Historisation des changements**  
  Tables d’historique, event sourcing, CDC pour les entités critiques.
- **Décision : Accès aux preuves**  
  Exports/rapports sans accès direct et privilégié à la base.

---

## 13. Support et documentation

- **Décision : Standards de documentation**  
  ADRs, runbooks, guides d’onboarding intégrés au “definition of done”.
- **Décision : Ownership opérationnel**  
  Astreinte, escalade, procédures de gestion d’incident.
- **Décision : Outillage support**  
  Écrans admin, dashboards support, actions de remédiation sûres.

---

## 14. Sauvegarde et reprise

- **Décision : Objectifs de reprise**  
  Définir **RPO/RTO** et dimensionner backups/réplications en conséquence.
- **Décision : Architecture de sauvegarde**  
  Snapshots, incrémentales, point-in-time restore, réplication multi-régions.
- **Décision : Vérification de la restauration**  
  Tests de restore automatisés ; runbooks DR documentés.
- **Décision : Plan de reprise des dépendances**  
  Garantir aussi la reprise des secrets, configs, queues, etc.

---

## 15. Résilience

- **Décision : Isolation des défaillances**  
  Bulkheads, isolement par tenant, circuit breakers, timeouts.
- **Décision : Redondance**  
  Multi-instances, multi-AZ, réseau redondant.
- **Décision : Dégradation contrôlée**  
  Prioriser les flux critiques ; dégrader le non essentiel ; fallbacks.
- **Décision : Protection de capacité**  
  Rate limiting, backpressure, load shedding.

---

## 16. Observabilité et exploitabilité

- **Décision : Stack d’observabilité**  
  Centralisation logs/métriques/traces ; schéma de logs cohérent.
- **Décision : SLO/SLI**  
  Définir des objectifs mesurables ; alerting sur symptômes, pas sur le bruit.
- **Décision : Sécurité opérationnelle**  
  Changements de config sûrs, feature flags, rollback automatisés, runbooks.
- **Décision : Endpoints de santé**  
  Liveness/readiness, checks dépendances, monitoring synthétique.

---

## 17. Testabilité

- **Décision : Stratégie de test par couche**  
  Unit tests pour règles, intégration aux frontières, end-to-end pour parcours clés.
- **Décision : Environnements et données de test**  
  Environnements proches prod ; datasets seedés ; fixtures déterministes.
- **Décision : Tests de contrat**  
  Vérifier automatiquement la compatibilité API/événements producteur-consommateur.
- **Décision : Contrôle des dépendances**  
  Doubles de test quand pertinent ; infra réelle pour les tests d’intégration.

---

## 18. Déployabilité

- **Décision : Stratégie de release**  
  Blue/green, canary, rolling selon criticité et niveau de risque.
- **Décision : Infrastructure as Code**  
  Environnements versionnés ; provisioning reproductible ; builds immutables si possible.
- **Décision : Gestion de configuration**  
  Config séparée du code ; config versionnée ; secrets gérés proprement.
- **Décision : Rollback et compatibilité**  
  Migrations DB backward-compatible ; feature flags ; procédure de rollback rapide.

---
