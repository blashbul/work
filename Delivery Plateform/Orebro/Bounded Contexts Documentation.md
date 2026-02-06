But : implémenter une plateforme qui couvre les besoins actuels connus du groupe, c'est à dire le transport et le postal.

Pour le transport, prendre en compte les multiples modes :

- La course
La course correspond à un transport ponctuel et dédié :
un client demande l’enlèvement d’un colis en un point A et sa livraison en un point B.

Dans son fonctionnement nominal, la course est effectuée d’un trait :
le même coursier réalise l’enlèvement et assure la livraison finale du pli ou du colis.

Dans un contexte d’industrialisation de la course, l’exécution peut toutefois être décomposée en plusieurs segments, avec des ruptures de charge, afin de mutualiser partiellement les transports, tout en conservant une promesse commerciale de type “course”.


- la messagerie
La messagerie repose sur un modèle mutualisé :
les chauffeurs collectent les colis auprès de plusieurs clients, les acheminent vers un hub, où ils sont triés.

Les envois sont ensuite :

soit distribués via des tournées locales,

soit confiés à des transporteurs externes pour les segments longue distance.

La rupture de charge et le passage par un hub sont structurels dans ce mode de transport.


- L'express
L’express repose sur une organisation proche de la messagerie, mais avec des engagements de délai plus forts.

Les flux sont :

priorisés,

traités dans des circuits plus courts,

assortis de délais garantis (J+1 avant une heure donnée, par exemple).

Il s’agit d’un transport rapide et contractualisé, sans être nécessairement dédié.


- Les tournées
Les tournées correspondent à des passages réguliers planifiés, souvent contractualisés,
et ne nécessitent pas obligatoirement l’annonce préalable de colis.

La logique porte sur la présence à un point donné selon une fréquence définie, plutôt que sur un envoi unitaire.
Mais chaque colis ramassés, doit être tracés. Cela est fait via des etiquettes qui peuvent être autoportante et réutilisable, car elles contiennent 
le point de départ encodé et le point de destination ainsi que le code client ce qui permet de faire l'acheminement.
Soit une etiquette est créé via une api par le client qui va choisir un point de départ parmis ces points de départs, et son point d'arrivée.
L'etiquette est ensuite collée sur le colis qui doit être tracé

## Description de la plateforme

La plateforme est multi-tenant :
chaque tenant gère de manière autonome :
    - ses sociétés
    - ses agences,
    - ses services,
    - ses ressources (véhicules, chauffeurs, coursiers),
    - ses clients,
    - ses sous-traitants,
    - ainsi que ses règles opérationnelles et tarifaires.


La plateforme supporte des utilisateurs multi-sociétés :
un même utilisateur peut être rattaché à plusieurs sociétés, avec des droits, rôles et périmètres définis indépendamment pour chaque tenant.

Context Tenant Management:
Fonctionnalités : 
- Création du tenant, reservée a des tenants managers, qui sont comme des super administrateurts. Cela permet de mettre en oeuvre une nouvelle société