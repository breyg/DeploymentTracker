namespace GP.ADQ.DeploymentTracker.Application.Interfaces
{
    public interface IChecklistService
    {
        Task<bool> ToggleChecklistItemAsync(int itemId, bool completed);
    }
}