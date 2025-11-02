# Modélisation procédurale — Exercices (Unity)

Ce projet contient des scripts C# générant des maillages 3D (mesh) procéduraux dans l’éditeur via `OnDrawGizmos`. Chaque forme expose des paramètres dans l’Inspector et construit un `Mesh` sur un `MeshFilter` associé.

## Contenu

- `Assets/TutorialInfo/Scripts/Modelisation/Plan.cs`  
  Génère une grille de quads 5×5 dans le plan XY.
  - Paramètres:
    - `width`: largeur d’une tuile.
    - `height`: hauteur d’une tuile.
  - Détails:
    - La grille est actuellement fixée à `gx = 5`, `gy = 5`.
    - Chaque quad est composé de 2 triangles. Les triangles sont doublés (ordre direct + inversé) pour rendre la face avant et arrière.

- `Assets/TutorialInfo/Scripts/Modelisation/Cone.cs` (`ConeMeridiens`)  
  Génère un cône ou un tronc de cône autour de l’axe Y.
  - Paramètres:
    - `baseRadius`: rayon à la base (y=0).
    - `height`: hauteur totale.
    - `truncationHeight`: hauteur de troncature (0..height). Si > 0, génère un tronc de cône; sinon cône pointu.
    - `meridian`: nombre de méridiens (≥ 3).
  - Détails:
    - Les côtés sont triangulés par bandes entre deux angles successifs.
    - Les caps (haut/bas) sont des “fans” autour d’un centre.
    - Les triangles sont doublés (faces avant/arrière).

- `Assets/TutorialInfo/Scripts/Modelisation/Sphere.cs` (`SphereParallelesMeridiens`)  
  Génère une sphère à partir de parallèles (latitudes) et méridiens (longitudes).
  - Paramètres:
    - `radius`: rayon.
    - `parallels`: nombre de parallèles (≥ 2). Crée `p-1` anneaux entre les pôles.
    - `meridian`: nombre de méridiens (≥ 3).
  - Détails:
    - Pôles gérés avec un sommet unique chacun (Nord/Sud) relié à l’anneau adjacent par un fan.
    - Entre anneaux, les quads sont décomposés en 2 triangles.
    - Triangles doublés (faces avant/arrière).
    - Normales recalculées via `mesh.RecalculateNormals()`.

- `Assets/TutorialInfo/Scripts/Modelisation/Cylindre.cs`  
  Génère un cylindre (côtés en bandes de quads et deux caps). Voir le script pour les paramètres exacts.

## Utilisation

1. Créer un `GameObject` vide dans la scène.
2. Ajouter le script désiré (`Plan`, `ConeMeridiens`, `SphereParallelesMeridiens`, `Cylindre`, …).
3. S’assurer que le `GameObject` possède un `MeshFilter` et un `MeshRenderer` (ajoutés automatiquement via `RequireComponent`).
4. Assigner un `Material` au `MeshRenderer` pour visualiser la géométrie.
5. Ajuster les paramètres dans l’Inspector. Le maillage se met à jour dans l’éditeur grâce à `OnDrawGizmos`.

## Comptage d’indices (avec doublage)

- 1 triangle logique = 6 indices (2 faces × 3 indices).
- Plan: `12 × gx × gy` indices.
- Cône non tronqué: côtés `m` + base `m` = `12m` indices.
- Cône tronqué / Cylindre: côtés `2m` + 2 caps `2m` = `24m` indices.
- Sphère: `top m` + bandes `2m(p−2)` + `bottom m` = `2m(p−1)` triangles logiques → `12m(p−1)` indices.
