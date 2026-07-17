terraform {
  required_version = ">= 1.5.0"

  required_providers {
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.35"
    }
  }
}

provider "kubernetes" {
  config_path    = var.kubeconfig_path
  config_context = var.kube_context
}

resource "kubernetes_namespace" "locatic" {
  metadata {
    name = var.namespace
    labels = {
      app     = "locatic"
      managed = "terraform"
    }
  }
}

resource "kubernetes_persistent_volume_claim" "sqlite" {
  metadata {
    name      = var.pvc_name
    namespace = kubernetes_namespace.locatic.metadata[0].name
    labels = {
      app     = "locatic"
      purpose = "sqlite-storage"
    }
  }

  spec {
    access_modes = ["ReadWriteOnce"]
    resources {
      requests = {
        storage = var.storage_size
      }
    }
    storage_class_name = var.storage_class_name
  }

  wait_until_bound = false
}
