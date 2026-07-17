# Ansible

## Rôle

Orchestrer le déploiement local entre Terraform et Kubernetes.

## Playbook principal

`infrastructure/ansible/playbooks/deploy.yml`

### Étapes

1. Vérifier `minikube status`
2. Vérifier `kubectl`
3. Lire `terraform output -json`
4. Appliquer les manifests `k8s/` (app, nginx, monitoring)
5. Mettre à jour l'image du Deployment
6. Attendre les rollouts
7. Afficher l'URL du service Nginx

## Variables

Définies dans `group_vars/all.yml`, surchargeables par environnement :

| Variable | Source |
|----------|--------|
| `LOCATIC_NAMESPACE` | env ou Terraform |
| `LOCATIC_IMAGE` | env ou Terraform |
| `LOCATIC_REPLICAS` | env ou Terraform |
| `LOCATIC_PVC` | env ou Terraform |

## Exécution

```bash
cd infrastructure/ansible
ansible-playbook playbooks/deploy.yml
```

Simulation (limitée selon modules) :

```bash
ansible-playbook playbooks/deploy.yml --check
```

## Dépendance Terraform

Terraform doit être appliqué **avant** Ansible pour créer le namespace et le PVC.
