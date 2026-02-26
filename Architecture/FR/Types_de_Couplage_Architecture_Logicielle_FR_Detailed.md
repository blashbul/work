# Les Types de Couplage en Architecture Logicielle (Version Détaillée + Exemples)

## Résumé exécutif
Le couplage mesure l’interdépendance entre modules. L’architecture ne vise pas à supprimer le couplage,
mais à le **maîtriser** :
- le rendre **explicite**
- le **localiser**
- stabiliser les **frontières**
- éviter les effets domino

Ce document contient :
- définitions
- mauvais / bons exemples
- sévérité
- signaux de détection
- stratégies de remédiation
- sources

---

# Légende
- **Sévérité** : 🟡 Faible / 🟠 Moyenne / 🔴 Élevée / 🔥 Critique
- **Portée** : Locale (dans un codebase) vs Distribuée (entre services)

---

# 1️⃣ Couplage Structurel

## Définition
Dépendance directe à une **implémentation concrète** plutôt qu’à une abstraction.

## Sévérité
🔴 Élevée (locale) → 🔥 Critique si le pattern est répandu

## Signaux
- `new ConcreteClass()` dans la logique métier
- singletons globaux / service locator
- tests qui nécessitent DB/fichiers/HTTP réels
- modification infra ⇒ modification métier

## ❌ Mauvais exemple
```csharp
public class OrderService
{
    private readonly SqlOrderRepository _repo = new SqlOrderRepository();
}
```

## ✅ Bon exemple
```csharp
public interface IOrderRepository
{
    Task<Order> GetAsync(Guid id);
}

public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
}
```

## Gestion
- Injection de dépendances
- DIP
- Architecture hexagonale (Ports & Adapters)

## Sources
- https://en.wikipedia.org/wiki/Coupling_(computer_programming)
- https://martinfowler.com/bliki/InversionOfControl.html
- Robert C. Martin — *Clean Architecture*

---

# 2️⃣ Couplage Temporel

## Définition
Le comportement correct dépend d’un **ordre d’appel**, mais cet ordre n’est pas garanti par le code.

## Sévérité
🔴 Élevée (souvent invisible)

## Signaux
- “il faut appeler X avant Y”
- API à initialisation en plusieurs étapes
- workflows fragiles dans les batchs / workers / hosted services

## ❌ Mauvais exemple
```csharp
var order = new Order();
order.CalculateTotal(); // suppose que les lignes existent
order.Validate();       // suppose que le total est calculé
order.Save();           // suppose que Validate a été fait
```

## ✅ Bon exemple (encapsulation)
```csharp
order.Process(); // garantit l’ordre et les invariants
```

## ✅ Bon exemple (machine à états)
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

## Gestion
- Encapsulation
- Machine à états
- Agrégats DDD

## Sources
- https://en.wikipedia.org/wiki/Temporal_coupling

---

# 3️⃣ Couplage par Données Partagées

## Définition
Plusieurs modules/services lisent/écrivent les mêmes **tables mutables** directement.

## Sévérité
🔥 Critique (distribué)

## Signaux
- DB “partagée” par beaucoup de services
- jointures inter-services
- ownership des colonnes inconnu
- “petite modif DB” ⇒ cassures partout
- SP utilisées comme API transverses

## ❌ Mauvais exemple (deux writers)
Service A :
```sql
UPDATE Operation SET Status = 3 WHERE Id = @id;
```
Service B :
```sql
UPDATE Operation SET Status = 4 WHERE Id = @id;
```

## ✅ Bon exemple (single writer + intégration)
Un seul service écrit, les autres :
- passent par une API
- ou réagissent à un événement

```csharp
public Task Handle(OperationCompletedEvent e)
{
    // met à jour un read model local, ou déclenche son propre process
    return Task.CompletedTask;
}
```

## Gestion
- Principe **single-writer**
- DB par service (si possible)
- APIs explicites pour les writes
- events pour l’intégration
- matrice d’ownership des données

## Sources
- https://martinfowler.com/articles/microservices.html
- Sam Newman — *Building Microservices*

---

# 4️⃣ Couplage par Schéma / Format

## Définition
Dépendance à la forme interne d’un payload (JSON, DTO, etc.).

## Sévérité
🟠 Moyenne → 🔴 Élevée avec beaucoup de consommateurs

## Signaux
- parsing dynamique
- partage de DTO entre contexts
- lecture de champs internes

## ❌ Mauvais exemple
```csharp
dynamic obj = JsonConvert.DeserializeObject(json);
var value = obj.data.internalField.subField;
```

## ✅ Bon exemple (contrat explicite)
```csharp
public record OrderDto(string Id, string Status);
var dto = JsonConvert.DeserializeObject<OrderDto>(json);
```

## ✅ Bon exemple (versionnement)
```http
GET /v2/orders/{id}
```

## Gestion
- DTO / endpoints versionnés
- Consumer-driven contracts
- tests de compatibilité en CI

## Sources
- https://martinfowler.com/articles/consumerDrivenContracts.html

---

# 5️⃣ Couplage Sémantique

## Définition
Dépendance à une signification métier implicite (statuts, règles), non centralisée.

## Sévérité
🔥 Critique (très invisible)

## Signaux
- codes “magiques”
- règles dupliquées
- interprétations différentes du même événement/champ

## ❌ Mauvais exemple
```csharp
if (order.Status == 3) // “3 = livré”
{
    NotifyCustomer();
}
```

## ✅ Bon exemple (sens explicite)
```csharp
if (order.IsDelivered())
{
    NotifyCustomer();
}
```

## ✅ Bon exemple (événement métier)
```csharp
public record OrderDeliveredEvent(Guid OrderId, DateTime DeliveredAtUtc);
```

## Gestion
- Ubiquitous Language
- Bounded Context
- événements métier explicites (éviter “StatusChanged”)

## Sources
- https://martinfowler.com/bliki/BoundedContext.html
- Eric Evans — *Domain-Driven Design*

---

# 6️⃣ Couplage par Contrôle

## Définition
Un module contrôle l’autre via des flags/modes qui modifient la logique interne.

## Sévérité
🟠 Moyenne

## Signaux
- méthodes à 4+ booléens
- branches difficiles à comprendre
- “mode” caché

## ❌ Mauvais exemple
```csharp
ProcessOrder(order, validate: true, sendEmail: false, recalcPrice: true);
```

## ✅ Bon exemple (options)
```csharp
public record ProcessOrderOptions(bool Validate, bool SendEmail, bool RecalcPrice);
ProcessOrder(order, new ProcessOrderOptions(true, false, true));
```

## ✅ Bon exemple (strategy)
```csharp
public interface IPricingStrategy { Money Calculate(Order o); }
```

## Gestion
- remplacer booléens par objet d’options
- Strategy / Command
- séparer les responsabilités

## Sources
- https://en.wikipedia.org/wiki/Coupling_(computer_programming)

---

# 7️⃣ Couplage Logique (Co-change)

## Définition
Modules qui changent souvent ensemble même sans dépendance directe.

## Sévérité
🟠 Moyenne (signal d’un mauvais découpage)

## Signaux
- “pour une feature, il faut toucher 5 repos”
- déploiements synchronisés
- mêmes fichiers modifiés ensemble

## Exemple (symptôme)
Le “delivery confirmed” touche toujours :
- Mobile API
- Tracking
- Billing
- Notifications
→ probable problème de frontière ou ownership

## Gestion
- analyse des co-changements Git
- réaligner le code sur les frontières métier
- réduire les concepts partagés

## Sources
- https://martinfowler.com/articles/evodb.html
- Adam Tornhill — *Your Code as a Crime Scene*

---

# 8️⃣ Couplage Événementiel

L’event-driven réduit le couplage **visible** (pas d’appel direct),
mais peut augmenter le couplage **implicite** si les contrats et la sémantique sont faibles.

## Sévérité
🔴 Élevée → 🔥 Critique si les événements deviennent des APIs publiques avec plein de consommateurs

---

## 8.1 Couplage par Schéma d’Événement

### ❌ Mauvais exemple (générique, ambigu)
```json
{ "type": "Order", "status": 3, "flagA": true }
```

### ✅ Bon exemple (contrat explicite)
```csharp
public record OrderDeliveredEvent(Guid OrderId, DateTime DeliveredAtUtc);
```

### Gestion
- types explicites
- versionnement `OrderDelivered.v1`, `OrderDelivered.v2`

---

## 8.2 Couplage Temporel Événementiel

### ❌ Mauvais exemple
Le consommateur suppose “Validated arrive toujours avant Delivered”.
Si l’un est retardé/perdu → incohérence.

### ✅ Bon exemple
- idempotence
- projections reconstruisibles
- séquences/versions si nécessaire

---

## 8.3 Couplage en Cascade (Chaînes d’événements)

### ❌ Mauvais exemple (dominos)
`OrderCreated → PricingCalculated → InvoiceCreated → AccountingExported → NotificationSent`

### ✅ Bon exemple
- orchestration explicite (Process Manager / Saga)
- événements = faits, commandes = intentions

---

## 8.4 Couplage d’Infrastructure

### ❌ Mauvais exemple
La correction métier dépend de “exactly once” du broker.

### ✅ Bon exemple
- at-least-once + handlers idempotents
- outbox pattern (transaction DB + publish)

---

## Checklist de remédiation (Event Coupling)
- clés d’idempotence
- outbox pattern
- stratégie de versionnement
- tests de contrat (CDC)
- limiter le nombre de consommateurs des événements “core”
- éviter “StatusChanged” au profit d’événements métier explicites

## Sources
- https://martinfowler.com/articles/201701-event-driven.html
- https://microservices.io/patterns/data/saga.html
- https://microservices.io/patterns/data/event-sourcing.html

---

# Matrice de sévérité (rapide)
| Type | Sévérité typique | Invisible ? | Échec fréquent |
|------|------------------|------------|----------------|
| Structurel | 🔴 | Non | tests impossibles / verrou infra |
| Temporel | 🔴 | Oui | bugs d’ordre d’appel |
| Données partagées | 🔥 | Oui | ownership chaos / ripple changes |
| Schéma | 🟠→🔴 | Oui | breaking changes silencieuses |
| Sémantique | 🔥 | Très | dérive des règles métier |
| Contrôle | 🟠 | Non | explosion de complexité |
| Logique | 🟠 | Oui | co-déploiements permanents |
| Événementiel | 🔴→🔥 | Oui | drift de contrats + workflow spaghetti |

---

# Modèle de maturité
| Maturité | À quoi ressemble le couplage | Mouvement typique |
|----------|------------------------------|-------------------|
| CRUD | structurel | abstractions + tests |
| Legacy DB-centric | données partagées | ownership + APIs |
| Microservices naïfs | schéma / events | versionnement + CDC |
| DDD mature | contrats explicites | gouvernance des frontières |

---

# Checklist d’audit (rapide)
- Où as-tu plusieurs writers sur la même table/colonne ?
- Où utilises-tu des statuts “magiques” ?
- Quels événements ont 10+ consommateurs ?
- Quels workflows supposent un ordering d’événements ?
- Quels changements touchent toujours les mêmes modules ?
