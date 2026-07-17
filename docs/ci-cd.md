# CI/CD

## Workflow Git

- Branche `main` protégée (push direct interdit, merge via Pull Request).
- Chaque modification passe par une PR avec checks CI obligatoires.

## Pipeline GitHub Actions (`.github/workflows/ci.yml`)

| Job | Déclencheur | Actions |
|-----|-------------|---------|
| **test** | PR + push `main` | restore, build, tests xUnit |
| **docker-build** | après test | build image Docker (cache GHA) |
| **security-scan** | après build | Trivy (image) + Gitleaks (secrets repo) |
| **publish** | push `main` uniquement | push vers `ghcr.io/<owner>/locatic` |

## Publication de l'image

Sur merge vers `main`, l'image est taguée :
- `ghcr.io/<owner>/locatic:<sha>`
- `ghcr.io/<owner>/locatic:latest`

Configurer le package GHCR en **public** ou fournir un `imagePullSecret` pour minikube.

## Limites du pipeline

Le pipeline **ne déploie pas** sur minikube (runners GitHub sans accès au cluster local). Le déploiement est déclenché manuellement via Terraform puis Ansible sur votre machine.

## Protection de `main` (à configurer sur GitHub)

1. Settings → Branches → Add rule sur `main`
2. Require a pull request before merging
3. Require status checks : `Tests et qualité`, `Build image Docker`, `Scan sécurité`

## Secrets GitHub

- `GITHUB_TOKEN` (fourni automatiquement) : publication GHCR
- Aucun secret en clair dans le dépôt
