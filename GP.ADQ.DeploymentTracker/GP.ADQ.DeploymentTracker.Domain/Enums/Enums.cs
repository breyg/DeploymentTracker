using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.ADQ.DeploymentTracker.Domain.Enums
{
    public enum ComponentType
    {
        Lambda = 1,
        ECS = 2
    }

    public enum EnvironmentType
    {
        Dev = 1,
        QA = 2,
        UAT = 3,
        Prod = 4
    }

    public enum DeploymentStatus
    {
        Pending = 1,
        InProgress = 2,
        Deployed = 3,
        Failed = 4
    }
}
