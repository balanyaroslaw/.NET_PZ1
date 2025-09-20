using Balan.TaskPlanner.Domain.Models;

namespace Balan.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] items)
        {
            if (items == null)
                return Array.Empty<WorkItem>();

            return items
                .OrderByDescending(i => i.Priority)  
                .ThenBy(i => i.DueDate)               
                .ThenBy(i => i.Title)                 
                .ToArray();
        }
    }
}
