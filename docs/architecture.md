# Architecture

## Vue d'ensemble

```
GitHub (code + PR + CI)
        │
        ▼
GitHub Actions ──► Tests / Build Docker / Scan / Push GHCR (main uniquement)
        │
        │ (arrêt du pipeline — pas de déploiement distant)
        ▼
Machine locale
  ├── Terraform  → namespace + PVC SQLite
  ├── Ansible    → déploiement K8s
  └── minikube   → exécution des pods
```

## Composants déployés

| Composant | Rôle |
|-----------|------|
| **Nginx** | Point d'entrée utilisateur (NodePort 30080), reverse proxy vers l'app |
| **Locatic (ASP.NET)** | Application métier, ClusterIP interne, métriques `/metrics` |
| **SQLite + PVC** | Persistance des données sur volume `locatic-sqlite-pvc` monté en `/data` |
| **Prometheus** | Collecte métriques app + disponibilité Nginx |
| **Grafana** | Dashboard de supervision (NodePort 30300) |

## Flux utilisateur

1. L'utilisateur accède à Nginx (seul point d'entrée exposé).
2. Nginx proxifie vers `locatic-app:8080`.
3. L'application lit/écrit SQLite sur le volume persistant.

## Rôles DevOps

- **GitHub Actions** : qualité, build, scan, publication image. Ne touche pas à minikube.
- **Terraform** : prépare le namespace et le stockage persistant.
- **Ansible** : enchaîne vérifications locales et `kubectl apply`.
- **Kubernetes** : exécute Nginx, l'application, le monitoring.

## Choix techniques

- SQLite avec chemin configurable (`ConnectionStrings__DefaultConnection`) pour le montage K8s.
- Health checks `/health` et `/health/ready` pour les probes Kubernetes.
- `prometheus-net` pour exposer les métriques HTTP de l'application.
- Image Docker multi-stage, utilisateur non privilégié `appuser`.
