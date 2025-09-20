using Balan.TaskPlanner.Domain.Logic;
using Balan.TaskPlanner.Domain.Models.Enums;
using Balan.TaskPlanner.Domain.Models;

namespace Balan.TaskPlanner
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var items = new List<WorkItem>();

            Console.WriteLine("=== Task Planner ===");
            Console.WriteLine("Вводьте завдання. Для виходу натиснiть [Esc] або залиште Заголовок порожнiм.\n");

            while (true)
            {
                Console.Write("Заголовок: ");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("\n⏹ Вихiд з введення завдань...");
                        break;
                    }
                }

                string title = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(title))
                    break;

                Console.Write("Опис: ");
                string description = Console.ReadLine();

                Console.Write("Дата виконання (dd.MM.yyyy): ");
                DateTime dueDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Прiорiтет (None, Low, Medium, High, Urgent): ");
                Priority priority = Enum.Parse<Priority>(Console.ReadLine(), ignoreCase: true);

                Console.Write("Складнiсть (None, Easy, Medium, Hard, Extreme): ");
                Complexity complexity = Enum.Parse<Complexity>(Console.ReadLine(), ignoreCase: true);

                var workItem = new WorkItem
                {
                    CreationDate = DateTime.Now,
                    DueDate = dueDate,
                    Priority = priority,
                    Complexity = complexity,
                    Title = title,
                    Description = description,
                    IsCompleted = false
                };

                items.Add(workItem);

                Console.WriteLine("✅ Завдання додано!\n");
            }

            Console.WriteLine("\n=== План завдань (вiдсортований) ===");

            var planner = new SimpleTaskPlanner();
            var plan = planner.CreatePlan(items.ToArray());

            foreach (var item in plan)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\nГотово!");
        }
    }
}
