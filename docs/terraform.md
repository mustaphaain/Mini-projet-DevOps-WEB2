# Terraform

## Ressources gérées

| Ressource | Description |
|-----------|-------------|
| `kubernetes_namespace.locatic` | Namespace isolé pour l'application |
| `kubernetes_persistent_volume_claim.sqlite` | Volume persistant pour SQLite |

## Variables

Voir `variables.tf`. Fichier d'exemple : `terraform.tfvars.example`.

| Variable | Défaut | Usage |
|----------|--------|-------|
| `namespace` | `locatic` | Namespace K8s |
| `pvc_name` | `locatic-sqlite-pvc` | Nom du PVC |
| `storage_size` | `1Gi` | Taille du volume |
| `kube_context` | `minikube` | Contexte kubectl |
| `docker_image` | — | Image pour Ansible (output) |

## Outputs (pour Ansible)

```bash
terraform output namespace
terraform output pvc_name
terraform output docker_image
```

## Commandes

```bash
terraform init
terraform fmt
terraform validate
terraform plan
terraform apply
terraform output -json
```

## État

- L'état (`*.tfstate`) est **ignoré par Git** (`.gitignore`).
- Utiliser un backend local par défaut ; ne pas committer l'état.

## Prérequis

- Cluster minikube démarré
- Provider Kubernetes configuré via `~/.kube/config`
