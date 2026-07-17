# Monitoring

## Stack

- **Prometheus** : collecte et alertes
- **Grafana** : visualisation (dashboard Locatic)

## Services monitorés

| Service | Job Prometheus | Indicateur |
|---------|----------------|------------|
| Locatic | `locatic-app` | `up`, `http_requests_received_total` |
| Nginx | `nginx` | `up` (health endpoint) |
| Prometheus | `prometheus` | auto-scrape |

## Accès

```bash
# Grafana (NodePort 30300)
minikube service grafana -n locatic --url

# Ou port-forward
kubectl port-forward svc/grafana 3000:3000 -n locatic
kubectl port-forward svc/prometheus 9090:9090 -n locatic
```

Identifiants Grafana par défaut : `admin` / `admin` (à changer en production).

## Dashboard

Dashboard provisionné : **Locatic - Vue d'ensemble** (disponibilité app, Nginx, Prometheus, requêtes HTTP).

## Alertes

Règles dans `k8s/monitoring/prometheus-configmap.yaml` :
- `LocaticAppDown` : application injoignable 1 min
- `NginxDown` : reverse proxy injoignable 1 min

## Vérification des targets

Dans Prometheus → Status → Targets : jobs `locatic-app`, `nginx`, `prometheus` doivent être **UP**.
