using Balan.TaskPlanner.Domain.Models.Enums;

namespace Balan.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            string dueDateFormatted = DueDate.ToString("dd.MM.yyyy");

            string priorityFormatted = Priority.ToString().ToLower();

            return $"{Title}: due {dueDateFormatted}, {priorityFormatted} priority";
        }
    }
}
