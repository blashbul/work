# Plateforme Transport

## Présentation générale

Cette plateforme a pour objectif de fournir une **infrastructure unifiée de gestion du transport** pour un groupe multi-sociétés, couvrant l’ensemble des besoins opérationnels connus autour du transport de plis et colis.

Elle est conçue pour :

- gérer **plusieurs modes de transport** (course, messagerie, express, tournées),
- supporter une **organisation multi-tenant et multi-sociétés**,
- offrir une **traçabilité bout-en-bout**,
- permettre l’**industrialisation des flux** tout en conservant des promesses commerciales claires,
- intégrer des **sous-traitants internes et externes**,
- évoluer vers des volumes élevés et des organisations complexes.

La plateforme s’inscrit dans une logique **Domain-Driven Design (DDD)**, avec une séparation claire entre :
- le **transport logique** (ce qui est vendu au client),
- et l’**exécution opérationnelle** (comment cela est réellement réalisé).

---

## Objectifs non fonctionnels

La plateforme vise explicitement les objectifs suivants :

### Scalabilité
- montée en charge progressive,
- gestion de volumes élevés de colis, transports et événements,
- absence de couplage fort entre flux commerciaux et exécution terrain.

### Résilience
- tolérance aux retards d’information (scans tardifs, intégrations asynchrones),
- capacité à fonctionner avec des partenaires externes partiellement intégrés,
- reprise sans perte de traçabilité.

### Traçabilité & auditabilité
- historique immuable des événements,
- explicabilité des décisions de routage et d’affectation,
- audit possible a posteriori sur plusieurs années.

### Évolutivité métier
- ajout de nouveaux modes de transport sans remise en cause du modèle existant,
- intégration progressive de nouvelles règles opérationnelles,
- extensibilité par configuration plutôt que par duplication de code.

### Isolation organisationnelle
- cloisonnement strict entre tenants,
- absence de dépendances implicites entre sociétés,
- gouvernance indépendante des règles métier.

## Périmètre fonctionnel

### Inclus

- Transport (course, messagerie, express, tournées)
- Traçabilité colis
- Organisation multi-tenant
- Organisation multi-sociétés
- Sous-traitance (interne et externe)
- Règles opérationnelles
- Pricing

### Hors périmètre (pour l’instant)

- Transport postal
- Facturation détaillée
- Optimisation avancée (service externe dédié)  ✅

---

## Modes de transport supportés

### Course (transport dédié)

La **course** correspond à un transport ponctuel et dédié.

- Enlèvement en **point A**
- Livraison en **point B**
- Dans le fonctionnement nominal :
  - un **seul coursier**
  - un **trajet direct**
  - aucune rupture de charge

#### Industrialisation de la course

Dans un contexte d’industrialisation, une course peut être :

- **décomposée en plusieurs segments (legs)**,
- avec des **ruptures de charge intermédiaires** (hub, agence),
- tout en conservant :
  - une **promesse commerciale unique**,
  - un **suivi client unifié**.

Du point de vue métier, il s’agit toujours **d’une seule course**, même si son exécution est composite.

---

### Messagerie

La **messagerie** repose sur un modèle mutualisé.

#### Principe général

- Collecte de colis auprès de **plusieurs clients**
- Acheminement vers un **hub**
- **Tri et regroupement** des flux

Les envois sont ensuite :

- distribués via des **tournées locales**,
- ou confiés à des **transporteurs externes** pour les segments longue distance.

La **rupture de charge est structurelle** et fait partie intégrante du modèle.

---

### Express

L’**express** est un mode de transport prioritaire, proche de la messagerie mais assorti de **délais contractuels forts**.

Caractéristiques :

- priorisation des flux,
- circuits plus courts,
- délais garantis.

Exemples :
- J+1 avant 10h
- J+1 avant 13h
- J0 soir

L’express n’est pas nécessairement dédié, mais il est **contractualisé**.

---

### Tournées contractuelles

Les **tournées** correspondent à des passages réguliers et planifiés.

#### Caractéristiques

- Fréquence définie (quotidienne, hebdomadaire, spécifique)
- Pas nécessairement d’annonce préalable de colis
- La valeur porte sur :
  - la **présence à un point donné**
  - plutôt que sur un envoi unitaire

#### Traçabilité des colis en tournée

Dans le cadre des tournées contractuelles, l’annonce préalable de colis
n’est pas systématique.

Même en l’absence d’annonce préalable :

- chaque colis ramassé ou livré doit être **tracé**,
- via des **étiquettes** :
  - autoportantes,
  - potentiellement **réutilisables**.

Les étiquettes contiennent notamment :

- le point de départ,
- le point de destination,
- le code client,
- un identifiant unique.

Les étiquettes peuvent être :

- générées via **API client**,
- ou fournies à l’avance et routées par règles.

---

## Principes structurants

### Transport logique vs exécution opérationnelle

La plateforme distingue explicitement :

- le **transport logique**
  - ce qui est vendu au client,
  - les engagements,
  - les SLA,
- l’**exécution opérationnelle**
  - legs,
  - hubs,
  - ressources,
  - sous-traitants.

Cette séparation permet :

- la mutualisation,
- la sous-traitance,
- la reconfiguration des flux sans impact contractuel.

Cette séparation est un **principe structurant du système**, et non un détail technique.

Elle garantit que :
- la promesse faite au client reste stable,
- l’exécution peut évoluer dans le temps,
- les choix opérationnels (mutualisation, sous-traitance, re-routage)
  n’impactent pas le contrat tant que les engagements sont respectés.

---

### Options & services additionnels (transport logique)

Lors du passage d’une commande de transport, le client peut sélectionner des
**options** (ou *services additionnels*).

Les options font partie intégrante du **transport logique** :
- elles sont **contractuelles**,
- elles participent à la **promesse client**,
- elles sont indépendantes de la manière dont le transport sera exécuté.

Une option peut avoir :
- un **impact commercial** (pricing),
- un **impact opérationnel** (contraintes d’exécution),
- ou les deux.

#### Typologie des options

On distingue principalement :

##### Options de service (pricing)
Options ayant un impact tarifaire, par exemple :
- assurance **ad valorem**

##### Options opérationnelles (contraintes)
Options imposant des contraintes d’exécution, par exemple :
- enlèvement ou livraison sur **rendez-vous**,
- créneau horaire imposé,
- remise contre **code PIN / OTP**,
- carboglace

Une même option peut cumuler :
- une **surcharge tarifaire**,
- et une **contrainte opérationnelle**.

---

### Traçabilité bout-en-bout

Chaque colis ou pli est suivi par :

- des **événements horodatés**,
- des changements de statut,
- des scans aux points clés,
- des preuves de livraison (POD).

La traçabilité est :

- transverse à tous les modes de transport,
- indépendante du nombre de segments.

---

### Qualité & contrôle (Control Tower)

La plateforme intègre une brique transverse de **Control Tower**, dédiée à la qualité et au pilotage opérationnel.

Elle permet notamment :

- la détection automatique d’anomalies :
  - scans manquants ou incohérents,
  - erreurs de routage,
  - SLA à risque ou dépassés,
  - boucles ou ruptures de flux,
- la corrélation d’événements multi-sources :
  - tracking,
  - opérations,
  - ETA estimées,
- la création de **cas d’anomalies** suivis jusqu’à résolution,
- l’alerte des équipes concernées (opérations, client, sous-traitant).

La Control Tower est conçue comme :
- un outil d’aide à la décision,
- un levier d’amélioration continue,
- un point de vérité sur la qualité de service.

---

### Notifications

La plateforme intègre une brique transverse de **notification**
destinée à informer :

- les **clients** (donneurs d’ordre),
- les **destinataires** (consignees),
- les **équipes opérationnelles**

Principes clés :

- toute notification est **déclenchée par un événement métier**,
- les envois sont **asynchrones**, tolérants aux retards et aux retries,
- les notifications sont **idempotentes** (anti-doublon),
- chaque envoi est **auditable** (quoi, quand, à qui, par quel canal, avec quel statut),
- les préférences, consentements et contraintes horaires sont respectés.

La communication n’impacte jamais le **flux métier principal** :
elle est découplée, observable et résiliente.

Certaines options entraînent des scénarios de notification spécifiques :
- confirmation ou rappel de rendez-vous,
- communication du code PIN,
- échec de remise sécurisé.

---

## Annonce de colis & étiquetage

La plateforme distingue explicitement :

- l’**annonce de colis** (vision logique et contractuelle),
- l’**étiquetage** (support physique et traçabilité opérationnelle).

Ces notions sont transverses à tous les modes de transport.

L’annonce de colis permet également au client de définir :
- des **options de service**,
- et des **contraintes spécifiques** liées à l’envoi.

Ces options sont portées par la commande et restent valables
quel que soit le mode d’exécution réel (direct, mutualisé, sous-traité).

**Exemple d’annonce avec options**

- Livraison sur rendez-vous (créneau imposé)
- Assurance ad valorem avec valeur déclarée

Ces éléments :
- influencent le **pricing**,
- génèrent des **contraintes de planification**,
- déclenchent des **notifications spécifiques** (confirmation RDV, PIN, etc.).

---

### Annonce de colis

L’**annonce de colis** correspond à la déclaration préalable d’un ou plusieurs envois
par un client ou un système tiers.

Elle permet notamment de définir :

- le client émetteur,
- les points de départ et de destination,
- le service demandé (course, messagerie, express, tournée),
- les engagements contractuels associés,
- les règles de pricing applicables.

L’annonce de colis peut être :

- **obligatoire** (course, express, messagerie classique),
- **optionnelle ou absente** (tournées contractuelles).

Une annonce de colis appartient toujours au **transport logique**,
indépendamment de la manière dont l’envoi sera exécuté.

---

### Étiquetage

L’**étiquette** est le support physique ou logique permettant d’assurer la
**traçabilité opérationnelle** d’un colis.

Elle porte notamment :

- un identifiant unique,
- les informations de routage,
- les références client,
- les informations nécessaires aux scans et événements.

Les étiquettes peuvent être :

- générées à la demande via API,
- pré-générées et fournies à l’avance,
- **réutilisables**, notamment dans le cadre de tournées contractuelles.

L’étiquetage est **indépendant de l’annonce préalable** :
un colis peut être tracé même s’il n’a pas été annoncé à l’avance.

---

## Organisation multi-tenant

### Tenant

Un **tenant** représente une entité autonome (groupe, filiale, organisation).

Chaque tenant gère de manière indépendante :

- ses sociétés,
- ses agences,
- ses ressources (véhicules, chauffeurs, coursiers),
- ses clients,
- ses sous-traitants,
- ses règles opérationnelles,
- ses règles de pricing.

L’isolation est :

- logique,
- fonctionnelle,
- organisationnelle.

---

### Utilisateurs multi-sociétés

Un utilisateur peut :

- appartenir à plusieurs sociétés,
- avec des rôles distincts selon le contexte.

Les droits sont définis :

- par tenant,
- par société,
- par rôle,
- par périmètre fonctionnel.

---

## Découpage fonctionnel (Bounded Contexts)

### Tenant Management

- création et gestion des tenants,
- gestion des tenant managers (super-administrateurs),
- paramètres globaux.

---

### Company / Partner Management

- sociétés juridiques internes
- sociétés sous-traitantes
- transporteurs externes (carriers : DPD, UPS, etc.)
- contrats cadres / paramètres de service
- conformité / statut
- paramètres d’intégration (optionnel)

---

### Customer Management

- clients
- contrats
- services souscrits
- SLA
- sites clients
- rôles des sites (ramasse, livraison, mixte)
- contraintes opérationnelles
- points de passage contractuels

---

### Addressing / Locations

- adresses normalisées
- lieux géographiques génériques
- géocodage
- déduplication
- historisation et superseding

---

### Transport & Operations

- transports (course, messagerie, express, tournées)
- hubs
- linehauls (corridors inter-hubs)
- règles de routage
- legs / segments
- tournées locales
- dispatch et affectations (ressources internes / sous-traitées main-d’œuvre)
- délégation à des transporteurs externes (carriers) pour certains legs (black-box)
- handover (remise au carrier), références transporteur, suivi d’exécution
- délégation d’exécution à des transporteurs externes (carriers),
- exécution partielle ou totale en mode *black-box*,
- suivi par événements remontés (statuts, POD, références transporteur),
- absence de dépendance au modèle interne du carrier.

Certaines options sélectionnées sur la commande génèrent des
**contraintes opérationnelles**, par exemple :

- rendez-vous ⇒ fenêtres horaires, temps de service,
- PIN,
- options matérielles ⇒ contraintes de ressources ou de véhicules.

Le BC opérationnel ne décide pas des options :
il **les interprète** pour produire une exécution conforme à la promesse client.

---

### Workforce & Resources (Planning)

- ressources opérationnelles (internes et indépendants)
- planning (shifts), absences, indisponibilités
- compétences, habilitations, contraintes

---

### Tracking

- statuts,
- événements,
- scans,
- preuves de livraison,
- incidents.

---

### Pricing

- grilles tarifaires,
- règles de calcul,
- surcharges,
- différenciation par service et par client.

Le pricing tient compte :
- du service principal,
- des caractéristiques de l’envoi,
- des **options sélectionnées par le client**.

Les options sont traitées comme des composants tarifaires :
- forfait,
- pourcentage (ex: assurance ad valorem),
- minimum / plafond,
- règles conditionnelles selon client, service ou zone.

---

### Optimization Orchestration (dans le monolithe)

Brique d’orchestration côté monolithe, responsable de :

- publier les demandes d’optimisation (courses à insérer, tournées à planifier),
- exposer les données nécessaires sous forme de **read models** ou snapshots,
- recevoir les propositions de l’optimiseur (plans, affectations, séquences),
- appliquer les décisions dans le BC opérationnel (si validées),
- garantir l’idempotence, la traçabilité et la compatibilité multi-versions.

---

### Integration

- API clients,
- API sous-traitants,
- intégration fichiers / EDI,
- webhooks **sortants** (publication d’événements vers des SI tiers),
- webhooks **entrants** (remontées de statuts, POD, références externes),
- évènementiel.
- intégration avec le service d’optimisation via messaging (interop Wolverine),
- contrat de messages versionné (commands/events),
- traitement idempotent et outbox pour fiabilité.

---

### Notifications

Cette brique couvre les **notifications** vers :

- les **clients** (donneurs d’ordre),
- les **destinataires** (consignees),
- les **équipes internes** (ops / control tower),
- certains **partenaires** (sous-traitants, carriers).

Objectifs :

- informer en temps réel sur les étapes clés du transport,
- supporter des **notifications multi-canales** (email, SMS, push, messagerie applicative),
- respecter les **préférences** et **consentements** par tenant / client / destinataire,
- garantir une **auditabilité complète** des communications,
- permettre des scénarios avancés :
  - livraison sécurisée par **code PIN / OTP**,
  - livraison sur **rendez-vous**,
  - notifications SLA / anomalies.

Le BC Notifications **ne pilote pas le transport** :
il orchestre uniquement la diffusion des messages à partir d’événements métier.

Il est conçu comme :

- **asynchrone par défaut**,
- découplé des flux opérationnels,
- compatible avec des partenaires partiellement intégrés.

---

### Control Tower / Anomaly Detection

- détection d’anomalies (traçabilité, routage, SLA, capacité)
- corrélation d’événements multi-sources (Tracking, Operations, ETA)
- création de cas (Anomaly Case) et workflow de résolution
- alerting (ops / client / sous-traitant)
- scoring (sévérité) et tableaux de bord qualité

---

## Optimisation & planification (service externe)

L’optimisation (mix courses “au fil de l’eau” + tournées connues à l’avance) est assurée par un **service dédié**,
**hors du monolithe**, qui tourne en continu.

Objectifs :
- planifier les tournées connues à l’avance (J-1 / J-n),
- insérer dynamiquement des courses en journée si cela “rentre” (insertion),
- préserver la stabilité opérationnelle (éviter les replanifications brutales),
- produire des propositions explicables (pourquoi ce chauffeur, pourquoi cet ordre).

Les options et contraintes issues du transport logique sont transmises
au service d’optimisation sous forme de contraintes planifiables :

- fenêtres horaires (rendez-vous),
- durées de service,
- exigences de compétences ou d’équipements,
- contraintes de stabilité ou de priorité.

L’optimiseur ne modifie jamais les options :
il cherche une exécution optimale **sous contraintes**.


### Choix d’architecture

- L’optimiseur est un **service indépendant** (Java) basé sur **Timefold.ai / OptaPlanner**.
- Le monolithe reste **source de vérité** pour :
  - transports logiques,
  - exécution opérationnelle (legs/tours),
  - ressources et planning,
  - tracking, SLA, anomalies.
- L’optimiseur agit comme un **moteur de décision** :
  - il calcule des plans / affectations / insertions,
  - il n’est pas le système d’enregistrement final.

### Mode de fonctionnement

- Le service d’optimisation tourne en continu et réagit à :
  - nouveaux transports/courses à affecter,
  - changements de contraintes (planning, absences, capacité),
  - évènements terrain (retards, incidents, annulations),
  - fenêtres de planification (pré-calcul des tournées à l’avance).

## Vision d’évolution

La plateforme est conçue pour :

- intégrer ultérieurement le **transport postal**,
- supporter de nouveaux modes de transport,
- absorber des volumes élevés,
- évoluer vers des mécanismes d’optimisation avancés,
- faciliter l’interconnexion avec des partenaires externes.

---

## Documents associés

- ADR (Architecture Decision Records)
- Modèle de domaine
- Context Map
- Documentation API
- Guides d’intégration clients et sous-traitants

## Glossaire (extrait)

- **Transport logique**  
  Vision contractuelle et commerciale vendue au client.

- **Exécution opérationnelle**  
  Réalité terrain du transport (legs, hubs, ressources, sous-traitants).

- **Leg / Segment**  
  Unité élémentaire d’exécution opérationnelle d’un transport.

- **Hub**  
  Point de rupture de charge, de tri ou de regroupement.

- **Carrier**  
  Transporteur externe exécutant un ou plusieurs segments.

- **POD (Proof of Delivery)**  
  Preuve attestant la livraison effective d’un envoi.

- **Option / service additionnel**  
  Choix contractuel effectué par le client lors de la commande,
  pouvant impacter le pricing et/ou l’exécution opérationnelle.

## Contrats de messages (interop monolithe ↔ optimiseur)

L’intégration repose sur un échange de messages **compatibles Wolverine** côté .NET
et consommables côté Java.

Principes :
- messages versionnés (v1, v2, …),
- sérialisation stable (JSON ou Avro/Protobuf si adopté),
- corrélation (CorrelationId / CausationId),
- idempotence (MessageId / DedupKey),
- publication fiable via outbox.

### Types de messages

- **Commands** (monolithe → optimiseur) :
  - demander une planification à l’avance,
  - demander une insertion de course,
  - demander une re-planification ciblée (incident, absence).

- **Proposals / Results** (optimiseur → monolithe) :
  - proposition de DayPlan (séquence d’arrêts),
  - proposition d’affectations,
  - explication / score (optionnel).

- **Events** (monolithe → optimiseur) :
  - transport créé/annulé/modifié,
  - ressource disponible/indisponible,
  - avancement terrain (retard, scan critique),
  - contraintes mises à jour.
