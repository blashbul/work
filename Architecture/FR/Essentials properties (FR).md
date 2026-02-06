# Propriétés essentielles d’un système d’information (FR)

Ce document décrit les propriétés essentielles qu’un système d’information doit présenter afin d’être fiable, utilisable, sécurisé et durable.  
Ces propriétés peuvent être utilisées à la fois comme critères d’évaluation et comme guides de conception.

---

## 1. Fiabilité et exactitude

- **Fiabilité** : Le système exécute de manière cohérente les fonctions attendues dans des conditions normales d’utilisation.  
  Les défaillances sont rares, prévisibles et ne laissent pas le système dans un état incohérent.

- **Exactitude** : Les informations produites par le système sont correctes, précises et exemptes d’erreurs.  
  À entrées et contexte identiques, le système produit les mêmes résultats.

---

## 2. Utilisabilité

- **Interface conviviale** : Le système est facile à comprendre et à utiliser, y compris pour des utilisateurs non techniques.  
  Les interactions sont intuitives et limitent les erreurs humaines.

- **Accessibilité** : Le système est accessible à tous les utilisateurs autorisés, y compris aux personnes en situation de handicap.  
  Les exigences d’accessibilité sont prises en compte dès la conception.

---

## 3. Sécurité

- **Confidentialité** : Les informations et services sont accessibles uniquement aux utilisateurs et systèmes autorisés.  
  Les règles de contrôle d’accès sont clairement définies et appliquées.

- **Intégrité** : Les informations et le comportement du système sont protégés contre toute modification non autorisée.  
  Les corruptions de données et changements involontaires sont empêchés ou détectés.

- **Disponibilité** : Les informations et ressources sont accessibles lorsque nécessaire.  
  Le système est protégé contre les abus, attaques et charges excessives.

---

## 4. Scalabilité

- Le système peut supporter l’augmentation du volume de données, du nombre d’utilisateurs et de la charge sans dégradation inacceptable des performances.
- La montée en charge peut se faire sans refonte majeure de l’architecture.
- La croissance n’entraîne pas une complexité ou des coûts disproportionnés.

---

## 5. Interopérabilité

- Le système peut échanger des données et interagir avec d’autres systèmes via des interfaces bien définies.
- Les intégrations ne nécessitent pas de couplage fort.
- Les évolutions des systèmes connectés ont un impact limité.

---

## 6. Maintenabilité

- Le système est conçu pour être compréhensible, modifiable et réparable avec un effort raisonnable.
- Les évolutions peuvent être réalisées de manière incrémentale et sécurisée.
- La dette technique est identifiée et maîtrisée.

---

## 7. Performance

- **Efficacité** : Le système utilise efficacement les ressources (CPU, mémoire, stockage, réseau).  
  La consommation de ressources est proportionnelle à l’usage réel.

- **Temps de réponse** : Le système fournit des réponses dans des délais acceptables pour les utilisateurs et les systèmes dépendants.  
  Les performances restent stables sous charge attendue.

---

## 8. Flexibilité et adaptabilité

- Le système peut s’adapter à l’évolution des besoins métier et de l’environnement.
- Les nouveaux besoins peuvent être intégrés sans déstabiliser l’existant.
- Les choix architecturaux favorisent l’évolution plutôt que la rigidité.

---

## 9. Gestion des données

- **Qualité des données** : Les données sont exactes, complètes, cohérentes et fiables.
- **Stockage des données** : Les données sont stockées de manière à garantir leur intégrité, leur durabilité et leur accessibilité.
- **Traitement des données** : Les données sont traitées de façon fiable et efficace pour produire une information exploitable.
- **Cycle de vie des données** : La création, la modification et la suppression des données sont contrôlées et prévisibles.

---

## 10. Maîtrise des coûts

- Le système apporte une valeur proportionnelle à ses coûts de développement et d’exploitation.
- Les coûts sont prévisibles et adaptés à l’usage et à la montée en charge.
- L’évolution du système ne génère pas de dérives budgétaires incontrôlées.

---

## 11. Conformité

- Le système respecte les lois, réglementations et normes applicables (ex. protection des données).
- Les exigences de conformité sont intégrées à la conception du système.
- La conformité ne dégrade pas excessivement l’utilisabilité ou les performances.

---

## 12. Auditabilité

- Le système conserve des traces des actions, événements et modifications de données.
- Les journaux et pistes d’audit permettent de reconstituer les opérations.
- Les informations d’audit sont accessibles sans perturber l’exploitation.

---

## 13. Support et documentation

- La documentation décrit clairement l’utilisation, le comportement et les limites du système.
- La documentation technique et opérationnelle facilite la maintenance et le support.
- Les utilisateurs et exploitants disposent de moyens de support adaptés.

---

## 14. Sauvegarde et reprise

- Le système intègre des mécanismes fiables de sauvegarde des données.
- Les procédures de reprise sont définies, documentées et testées.
- Les données et services peuvent être restaurés dans des délais acceptables en cas d’incident ou de sinistre.

---

## 15. Résilience

- Le système tolère les défaillances partielles et les situations imprévues.
- Les pannes sont isolées afin d’éviter les effets en cascade.
- En mode dégradé, le système se comporte de manière maîtrisée et récupère de façon prévisible.

---

## 16. Observabilité et exploitabilité

- Le système expose suffisamment d’informations sur son état interne et son comportement.
- Les logs, métriques et traces permettent de détecter et diagnostiquer les incidents en production.
- Les actions d’exploitation sont sûres, maîtrisées et prévisibles.

---

## 17. Testabilité

- Le système permet des tests automatisés et reproductibles.
- Les composants peuvent être testés isolément et dans leur ensemble.
- Les anomalies peuvent être reproduites, analysées et corrigées de manière fiable.

---

## 18. Déployabilité

- Le système peut être déployé, mis à jour et restauré de manière contrôlée et répétable.
- Les processus de déploiement sont largement automatisés.
- Les environnements sont cohérents et reproductibles.

---

Un système qui respecte ces propriétés a de fortes chances d’être fiable, évolutif et pérenne dans le temps.
