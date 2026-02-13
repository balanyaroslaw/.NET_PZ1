using Balan.TaskPlanner.DataAccess.Abstractions;
using Balan.TaskPlanner.Domain.Models.Enums;
using Balan.TaskPlanner.Domain.Models;
using Balan.TaskPlanner.Domain.Logic;
using Moq;

namespace Balan.TaskPlanner.Domain.Logic.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CreatePlan_ShouldSortAndFilterCorrectly()
        {
            var tasks = new[]
            {
                new WorkItem
                {
                    Id = Guid.NewGuid(),
                    Title = "C Task",
                    Priority = Priority.Medium,
                    DueDate = new DateTime(2025, 2, 10),
                    IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(),
                    Title = "A Task",
                    Priority = Priority.High,
                    DueDate = new DateTime(2025, 1, 5),
                    IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(),
                    Title = "B Task",
                    Priority = Priority.Low,
                    DueDate = new DateTime(2025, 1, 1),
                    IsCompleted = false
                },
                new WorkItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Completed Task",
                    Priority = Priority.High,
                    DueDate = new DateTime(2025, 2, 1),
                    IsCompleted = true      
                }
            };

            var mockRepo = new Mock<IWorkItemsRepository>();

            mockRepo.Setup(r => r.GetAll()).Returns(tasks);

            var planner = new SimpleTaskPlanner(mockRepo.Object);

            var plan = planner.CreatePlan();


            Assert.DoesNotContain(plan, t => t.IsCompleted);

            Assert.Equal(3, plan.Length);

            Assert.Equal("A Task", plan[0].Title); 
            Assert.Equal("C Task", plan[1].Title); 
            Assert.Equal("B Task", plan[2].Title); 
        }
    }
}