variable "namespace" {
  description = "Namespace Kubernetes pour Locatic"
  type        = string
  default     = "locatic"
}

variable "pvc_name" {
  description = "Nom du PersistentVolumeClaim SQLite"
  type        = string
  default     = "locatic-sqlite-pvc"
}

variable "storage_size" {
  description = "Taille du volume persistant SQLite"
  type        = string
  default     = "1Gi"
}

variable "storage_class_name" {
  description = "StorageClass Kubernetes (standard sur minikube)"
  type        = string
  default     = "standard"
}

variable "kubeconfig_path" {
  description = "Chemin vers le kubeconfig local"
  type        = string
  default     = "~/.kube/config"
}

variable "kube_context" {
  description = "Contexte Kubernetes (minikube par défaut)"
  type        = string
  default     = "minikube"
}

variable "docker_image" {
  description = "Image Docker de l'application"
  type        = string
  default     = "ghcr.io/owner/locatic:latest"
}

variable "app_replicas" {
  description = "Nombre de replicas de l'application"
  type        = number
  default     = 1
}
