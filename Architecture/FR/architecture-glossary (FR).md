# Glossaire d’architecture (FR)

Ce glossaire définit les sigles, patterns d’architecture, concepts opérationnels
et pratiques de delivery utilisés dans les documents de référence.
Les définitions sont concises et orientées usage réel.

---

## Sigles et acronymes

- **ACID**  
  Atomicité, Cohérence, Isolation, Durabilité. Garantissent la fiabilité des transactions.

- **ABAC (Contrôle d’accès basé sur les attributs)**  
  Modèle d’autorisation basé sur des attributs (utilisateur, ressource, contexte).

- **ADR (Architecture Decision Record)**  
  Document court décrivant une décision d’architecture, son contexte et ses conséquences.

- **API (Interface de Programmation Applicative)**  
  Contrat permettant la communication entre systèmes ou composants.

- **CI/CD (Intégration Continue / Déploiement Continu)**  
  Chaînes automatisées de build, test et déploiement.

- **CQRS (Séparation Commandes / Requêtes)**  
  Pattern séparant les écritures des lectures.

- **CDC (Capture des Changements de Données)**  
  Technique de suivi et propagation des modifications en base.

- **CSAT (Indice de Satisfaction Client)**  
  Indicateur de satisfaction utilisateur.

- **DoD (Definition of Done – Définition de terminé)**  
  Liste de critères indiquant qu’un travail est achevé
  (tests, documentation, supervision, déployabilité…).

- **DSR (Demande de la Personne Concernée)**  
  Demande liée aux données personnelles (RGPD).

- **FK (Clé étrangère)**  
  Contrainte assurant l’intégrité référentielle.

- **IaC (Infrastructure as Code)**  
  Gestion de l’infrastructure par du code versionné.

- **KMS (Service de Gestion des Clés)**  
  Service de gestion des clés cryptographiques.

- **MTTD / MTTR**  
  Délai moyen de détection / de résolution d’un incident.

- **NPS (Net Promoter Score)**  
  Indicateur de satisfaction et de fidélité.

- **OIDC (OpenID Connect)**  
  Couche d’identité basée sur OAuth 2.0.

- **OAuth2**  
  Framework d’autorisation pour l’accès délégué sécurisé.

- **OLTP (Traitement Transactionnel en Ligne)**  
  Systèmes optimisés pour les transactions.

- **PITR (Restauration à un instant donné)**  
  Capacité à restaurer les données à un moment précis.

- **RBAC (Contrôle d’accès basé sur les rôles)**  
  Modèle d’autorisation basé sur les rôles.

- **RPO / RTO**  
  Objectif de point de reprise / Objectif de temps de reprise.

- **SLA / SLO / SLI**  
  Accord / Objectif / Indicateur de niveau de service.

- **SSO (Authentification unique)**  
  Accès à plusieurs systèmes avec une seule authentification.

- **WAF (Pare-feu applicatif Web)**  
  Protection contre les attaques web courantes.

---

## Patterns et concepts d’architecture

- **Pattern Adapter**  
  Traduction d’une interface vers une autre pour intégrer des composants incompatibles.

- **Couche anti-corruption (ACL)**  
  Protection du modèle interne contre des modèles externes.

- **Backpressure (contre-pression)**  
  Ralentissement des producteurs lorsque les consommateurs sont saturés.

- **Pattern Bulkhead (cloisonnement)**  
  Isolation empêchant une panne locale de se propager.

- **Modèle canonique**  
  Modèle de données commun pour simplifier les intégrations.

- **Circuit Breaker (disjoncteur)**  
  Interruption temporaire des appels vers une dépendance défaillante.

- **Architecture événementielle**  
  Communication entre systèmes par événements.

- **Event Sourcing**  
  Reconstruction de l’état à partir d’événements immuables.

- **Pattern Facade**  
  Interface simplifiée vers un sous-système complexe.

- **Feature Flag (toggle)**  
  Activation/désactivation dynamique de fonctionnalités.

- **Dégradation contrôlée**  
  Maintien d’un service partiel en cas de défaillance.

- **Architecture hexagonale (Ports & Adapters)**  
  Séparation entre logique métier et dépendances externes.

- **Architecture en couches**  
  Organisation en couches (présentation, application, domaine, infrastructure).

- **Vue matérialisée**  
  Résultat de requête pré-calculé et stocké.

- **Pattern Policy**  
  Encapsulation de règles métier ou techniques.

- **Read Model**  
  Modèle de lecture optimisé, souvent dénormalisé.

- **Politique de retry**  
  Réexécution contrôlée d’opérations échouées.

- **Sharding**  
  Répartition horizontale des données.

- **Service Stateless**  
  Service sans état entre les requêtes.

- **Pattern Strategy**  
  Sélection dynamique d’un algorithme ou comportement.

---

## Exploitation et fiabilité

- **Runbook**  
  Guide opérationnel décrivant l’exploitation, le diagnostic et la reprise d’un système.

- **SOP (Procédure Opérationnelle Standard)**  
  Procédure documentée pour les tâches d’exploitation courantes.

- **Astreinte (On-call)**  
  Organisation de la réponse aux incidents sur des plages définies.

- **Gestion d’incident**  
  Processus de traitement et d’amélioration suite à incident.

- **Postmortem sans blâme**  
  Analyse d’incident axée sur l’amélioration du système.

- **Budget d’erreur (Error Budget)**  
  Indisponibilité acceptable définie par les SLO.

- **Fallback (repli)**  
  Comportement alternatif en cas d’échec.

- **Load Shedding**  
  Rejet volontaire de requêtes pour préserver la stabilité.

- **Préparation à l’exploitation**  
  État indiquant qu’un système est prêt pour la production.

---

## Delivery et qualité

- **Compatibilité descendante**  
  Capacité à fonctionner avec des versions antérieures.

- **Déploiement Blue/Green**  
  Deux environnements pour bascule rapide.

- **Déploiement Canary**  
  Déploiement progressif sur un sous-ensemble d’utilisateurs.

- **Déploiement Rolling**  
  Mise à jour incrémentale sans interruption globale.

- **Fail Fast**  
  Détection et signalement précoce des erreurs.

- **Hotfix**  
  Correctif urgent appliqué en production.

- **Livraison progressive**  
  Techniques de déploiement réduisant les risques.

- **Évolution de schéma**  
  Modification des schémas sans casser l’existant.

- **Shift Left**  
  Déplacement des contrôles qualité plus tôt dans le cycle de développement.

- **Dette technique**  
  Coût futur lié à des compromis de conception ou de qualité.

---
