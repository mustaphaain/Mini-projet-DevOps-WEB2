# Mini-projet DevOps WEB2 — Locatic

Chaîne DevOps complète pour l'application **Locatic** (location de voitures ASP.NET Core MVC), issue du projet POO.

## Objectif

- Code source applicatif + conteneurisation Docker
- Pipeline CI/CD GitHub Actions (tests, build, scan, publication GHCR sur `main`)
- Infrastructure locale Terraform (namespace + PVC SQLite)
- Orchestration Ansible du déploiement minikube
- Stack Kubernetes : Nginx (reverse proxy) → application → SQLite (volume persistant)
- Monitoring Prometheus + Grafana

## Prérequis locaux

- [.NET SDK 9](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [minikube](https://minikube.sigs.k8s.io/docs/start/) + [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [Terraform](https://www.terraform.io/) >= 1.5
- [Ansible](https://docs.ansible.com/)

## Structure du dépôt

```
src/Locatic/              Application ASP.NET Core MVC (projet POO)
tests/Locatic.Tests/      Tests automatisés
.github/workflows/        Pipeline CI/CD
infrastructure/
  terraform/              Namespace + PVC SQLite
  ansible/                Playbook de déploiement local
k8s/                      Manifests Kubernetes (app, nginx, monitoring)
docs/                     Documentation détaillée
Dockerfile                Image de l'application
```

## Démarrage rapide (application seule)

```bash
dotnet restore Locatic.sln
dotnet test Locatic.sln
dotnet run --project src/Locatic/Locatic.csproj
```

Endpoint de santé : `GET /health`

## Déploiement local (résumé)

1. Merger sur `main` → l'image est publiée sur `ghcr.io/<owner>/locatic`
2. `minikube start`
3. `cd infrastructure/terraform && terraform init && terraform apply`
4. `cd ../ansible && ansible-playbook playbooks/deploy.yml`
5. Accéder via Nginx : `minikube service locatic-nginx -n locatic --url`

Voir [docs/deploiement-local.md](docs/deploiement-local.md) pour le détail.

## Documentation

| Document | Contenu |
|----------|---------|
| [architecture.md](docs/architecture.md) | Schéma et rôles des composants |
| [ci-cd.md](docs/ci-cd.md) | Pull Requests, pipeline GitHub Actions |
| [deploiement-local.md](docs/deploiement-local.md) | Procédure complète minikube |
| [terraform.md](docs/terraform.md) | Infrastructure Terraform |
| [ansible.md](docs/ansible.md) | Playbook Ansible |
| [kubernetes.md](docs/kubernetes.md) | Ressources K8s |
| [monitoring.md](docs/monitoring.md) | Prometheus et Grafana |
| [exploitation.md](docs/exploitation.md) | Vérifications et diagnostic |

## Lien avec le projet POO

Ce dépôt reprend l'application **Locatic** (marques, modèles, voitures, clients, réservations) avec SQLite et Entity Framework Core, en y ajoutant les adaptations DevOps (health checks, métriques Prometheus, chemin SQLite configurable).

## Secrets

Ne jamais committer : `.env`, `terraform.tfvars`, `*.tfstate`, tokens. Utiliser les secrets GitHub pour la CI et `.env.example` comme modèle local.



## Démonstration Pull Request
Cette modification sert à démontrer le workflow GitHub avec Pull Request.