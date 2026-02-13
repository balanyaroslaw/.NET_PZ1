using Balan.TaskPlanner.Domain.Models;
using Balan.TaskPlanner.DataAccess.Abstractions; 
using System;
using System.Linq;

namespace Balan.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public WorkItem[] CreatePlan()
        {
            var items = _repository.GetAll();
            if (items == null || items.Length == 0)
                return Array.Empty<WorkItem>();

            var itemsToRemove = items.Select(i => i.IsCompleted == true);

            return items
                .Where(i => !i.IsCompleted)
                .OrderByDescending(i => i.Priority)  
                .ThenBy(i => i.DueDate)             
                .ThenBy(i => i.Title)               
                .ToArray();
        }
    }
}
