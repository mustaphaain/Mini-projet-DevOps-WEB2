output "namespace" {
  description = "Namespace Kubernetes créé"
  value       = kubernetes_namespace.locatic.metadata[0].name
}

output "pvc_name" {
  description = "Nom du PVC SQLite"
  value       = kubernetes_persistent_volume_claim.sqlite.metadata[0].name
}

output "sqlite_mount_path" {
  description = "Chemin de montage SQLite dans le pod"
  value       = "/data"
}

output "docker_image" {
  description = "Image Docker à déployer"
  value       = var.docker_image
}

output "app_replicas" {
  description = "Nombre de replicas configurés"
  value       = var.app_replicas
}

output "kube_context" {
  description = "Contexte Kubernetes utilisé"
  value       = var.kube_context
}
