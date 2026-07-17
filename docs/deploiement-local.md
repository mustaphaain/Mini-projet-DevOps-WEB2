# Déploiement local

Ordre exact des actions pour déployer une image publiée sur minikube.

## 1. Prérequis

```bash
minikube start
kubectl config use-context minikube
```

## 2. Configurer l'image

Copier et adapter :

```bash
cp infrastructure/terraform/terraform.tfvars.example infrastructure/terraform/terraform.tfvars
```

Remplacer `<owner>` par votre identifiant GitHub dans `docker_image`.

## 3. Terraform — infrastructure de base

```bash
cd infrastructure/terraform
terraform init
terraform plan
terraform apply
terraform output
```

Crée le namespace `locatic` et le PVC `locatic-sqlite-pvc`.

## 4. Rendre l'image disponible dans minikube

Si l'image est sur GHCR (privée ou pour éviter un pull externe) :

```bash
docker pull ghcr.io/<owner>/locatic:latest
minikube image load ghcr.io/<owner>/locatic:latest
```

Ou build local :

```bash
docker build -t ghcr.io/<owner>/locatic:latest .
minikube image load ghcr.io/<owner>/locatic:latest
```

## 5. Ansible — déploiement complet

```bash
cd infrastructure/ansible
ansible-playbook playbooks/deploy.yml
```

Déploie : application, Nginx, Prometheus, Grafana.

## 6. Vérification

```bash
kubectl get all -n locatic
minikube service locatic-nginx -n locatic --url
curl $(minikube service locatic-nginx -n locatic --url)/health
```

Grafana : `kubectl port-forward svc/grafana 3000:3000 -n locatic` → http://localhost:3000 (admin/admin)

Prometheus : `kubectl port-forward svc/prometheus 9090:9090 -n locatic`

## 7. Persistance SQLite

```bash
kubectl delete pod -l app=locatic,tier=backend -n locatic
kubectl wait --for=condition=ready pod -l app=locatic,tier=backend -n locatic
```

Les données doivent subsister grâce au PVC.
