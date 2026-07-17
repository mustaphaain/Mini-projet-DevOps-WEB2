# Kubernetes

## Namespace

`locatic` (créé par Terraform)

## Application

| Ressource | Fichier | Détail |
|-----------|---------|--------|
| ConfigMap | `k8s/app/configmap.yaml` | Variables ASP.NET, chemin SQLite `/data/locatic.db` |
| Deployment | `k8s/app/deployment.yaml` | 1 replica, probes `/health`, volume PVC |
| Service | `k8s/app/service.yaml` | ClusterIP `:8080` (interne) |

## Nginx (reverse proxy)

| Ressource | Fichier | Détail |
|-----------|---------|--------|
| ConfigMap | `k8s/nginx/configmap.yaml` | Proxy vers `locatic-app:8080` |
| Deployment | `k8s/nginx/deployment.yaml` | nginx:1.27-alpine |
| Service | `k8s/nginx/service.yaml` | NodePort **30080** — point d'entrée |

## Stockage SQLite

- PVC : `locatic-sqlite-pvc` (Terraform)
- Montage pod : `/data`
- Connection string : `Data Source=/data/locatic.db`

## Paramètres modifiables

- Image : `deployment.yaml` ou `kubectl set image`
- Replicas : `spec.replicas`
- Ressources CPU/mémoire : `resources` dans les Deployments
- NodePort Nginx : `k8s/nginx/service.yaml`

## Exposition

L'application **n'est pas** exposée directement. Seul Nginx est en NodePort.
