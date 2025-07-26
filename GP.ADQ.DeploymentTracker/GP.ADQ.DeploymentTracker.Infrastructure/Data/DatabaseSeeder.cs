using GP.ADQ.DeploymentTracker.Domain.Entities;
using GP.ADQ.DeploymentTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.ADQ.DeploymentTracker.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(DeploymentTrackerContext context)
        {
            if (context.Projects.Any())
                return; // Database already seeded

            // Create sample projects with components
            var paymentProject = new Project("Sistema de Pagos", "APIs y lambdas para procesamiento de pagos", "admin");
            var notificationProject = new Project("Sistema de Notificaciones", "Microservicios para envío de notificaciones", "admin");

            context.Projects.AddRange(paymentProject, notificationProject);
            await context.SaveChangesAsync();

            // Add components to payment project
            var paymentLambda = new Component("payment-processor-lambda", ComponentType.Lambda, paymentProject.Id, "PAY-123");
            paymentLambda.UpdateConfiguration("512MB", "30s", "PAY-123");

            var paymentECS = new Component("payment-api-ecs", ComponentType.ECS, paymentProject.Id, "PAY-124");

            // Add components to notification project
            var notificationLambda = new Component("notification-sender-lambda", ComponentType.Lambda, notificationProject.Id, "NOT-456");
            notificationLambda.UpdateConfiguration("256MB", "60s", "NOT-456");

            context.Components.AddRange(paymentLambda, paymentECS, notificationLambda);
            await context.SaveChangesAsync();

            // Add versions for environments
            paymentLambda.SetVersion(EnvironmentType.Dev, "v1.2.3", DeploymentStatus.Deployed);
            paymentLambda.SetVersion(EnvironmentType.QA, "v1.2.2", DeploymentStatus.Deployed);
            paymentLambda.SetVersion(EnvironmentType.UAT, "v1.2.1", DeploymentStatus.Pending);
            paymentLambda.SetVersion(EnvironmentType.Prod, "v1.2.0", DeploymentStatus.Deployed);

            paymentECS.SetVersion(EnvironmentType.Dev, "v2.1.0", DeploymentStatus.Deployed);
            paymentECS.SetVersion(EnvironmentType.QA, "v2.0.5", DeploymentStatus.Deployed);
            paymentECS.SetVersion(EnvironmentType.UAT, "v2.0.4", DeploymentStatus.InProgress);
            paymentECS.SetVersion(EnvironmentType.Prod, "v2.0.3", DeploymentStatus.Deployed);

            notificationLambda.SetVersion(EnvironmentType.Dev, "v1.1.0", DeploymentStatus.Deployed);
            notificationLambda.SetVersion(EnvironmentType.QA, "v1.0.9", DeploymentStatus.Deployed);
            notificationLambda.SetVersion(EnvironmentType.UAT, "v1.0.8", DeploymentStatus.Failed);
            notificationLambda.SetVersion(EnvironmentType.Prod, "v1.0.7", DeploymentStatus.Deployed);

            // Add checklist items
            paymentLambda.AddChecklistItem("Parameter Store configurado", 1);
            paymentLambda.AddChecklistItem("Environment variables definidas", 2);
            paymentLambda.AddChecklistItem("Memoria asignada (512MB)", 3);
            paymentLambda.AddChecklistItem("Timeout configurado (30s)", 4);
            paymentLambda.AddChecklistItem("Permisos IAM verificados", 5);

            paymentECS.AddChecklistItem("Docker image built", 1);
            paymentECS.AddChecklistItem("Task definition updated", 2);
            paymentECS.AddChecklistItem("Load balancer configured", 3);
            paymentECS.AddChecklistItem("Health checks enabled", 4);

            notificationLambda.AddChecklistItem("SES permissions configured", 1);
            notificationLambda.AddChecklistItem("SNS topics created", 2);
            notificationLambda.AddChecklistItem("DLQ configured", 3);

            await context.SaveChangesAsync();
        }
    }
}
