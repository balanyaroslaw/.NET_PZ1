using Balan.TaskPlanner.DataAccess;
using Balan.TaskPlanner.DataAccess.Abstractions;
using Balan.TaskPlanner.Domain.Logic;
using Balan.TaskPlanner.Domain.Models;
using Balan.TaskPlanner.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Balan.TaskPlanner
{
    internal static class Program
    {
        private static IWorkItemsRepository _repository;
        private static SimpleTaskPlanner _planner;
        static void Main(string[] args)
        {
            _repository = new FileWorkItemsRepository();
            _planner = new SimpleTaskPlanner(_repository);

            Console.WriteLine("=== Task Planner ===");

            while (true)
            {
                Console.WriteLine("\nОберіть дію: [A]dd, [B]uild plan, [M]ark completed, [R]emove, [Q]uit");
                Console.Write("Дія: ");
                string action = Console.ReadLine()?.Trim().ToUpper();

                switch (action)
                {
                    case "A":
                        AddWorkItem();
                        break;

                    case "B":
                        BuildPlan();
                        break;

                    case "M":
                        MarkCompleted();
                        break;

                    case "R":
                        RemoveWorkItem();
                        break;

                    case "Q":
                        Console.WriteLine("⏹ Вихід з програми...");
                        return;

                    default:
                        Console.WriteLine("Невідома команда. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private static void AddWorkItem()
        {
            Console.Write("Заголовок: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("❌ Заголовок не може бути порожнім!");
                return;
            }

            Console.Write("Опис: ");
            string description = Console.ReadLine();

            Console.Write("Дата виконання (dd.MM.yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
            {
                Console.WriteLine("❌ Невірний формат дати!");
                return;
            }

            Console.Write("Пріоритет (None, Low, Medium, High, Urgent): ");
            if (!Enum.TryParse<Priority>(Console.ReadLine(), true, out Priority priority))
            {
                Console.WriteLine("❌ Невірний пріоритет!");
                return;
            }

            Console.Write("Складність (None, Easy, Medium, Hard, Extreme): ");
            if (!Enum.TryParse<Complexity>(Console.ReadLine(), true, out Complexity complexity))
            {
                Console.WriteLine("❌ Невірна складність!");
                return;
            }

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

            var id = _repository.Add(workItem);
            _repository.SaveChanges();

            Console.WriteLine($"✅ Завдання додано! Id: {id}");
        }


        private static void BuildPlan()
        {
            var all = _repository.GetAll();
            if (all.Length == 0)
            {
                Console.WriteLine("❌ Немає завдань для планування!");
                return;
            }

            Console.WriteLine("\n=== План завдань (відсортований) ===");
            var plan = _planner.CreatePlan();

            foreach (var item in plan)
            {
                Console.WriteLine(item);
            }
        }


        private static void MarkCompleted()
        {
            Console.Write("Введіть Id завдання: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("❌ Невірний формат Id!");
                return;
            }

            var item = _repository.Get(id);
            if (item == null)
            {
                Console.WriteLine("❌ Завдання не знайдено!");
                return;
            }

            item.IsCompleted = true;
            _repository.Update(item);
            _repository.SaveChanges();

            Console.WriteLine("✅ Завдання позначено як виконане!");
        }


        private static void RemoveWorkItem()
        {
            Console.Write("Введіть Id завдання: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("❌ Невірний формат Id!");
                return;
            }

            var item = _repository.Get(id);
            if (item == null)
            {
                Console.WriteLine("❌ Завдання не знайдено!");
                return;
            }

            _repository.Remove(id);
            _repository.SaveChanges();

            Console.WriteLine("✅ Завдання видалено!");
        }
    }
}
