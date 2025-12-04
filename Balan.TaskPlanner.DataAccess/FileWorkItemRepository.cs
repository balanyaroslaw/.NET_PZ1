using Balan.TaskPlanner.DataAccess.Abstractions;
using Balan.TaskPlanner.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace Balan.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FileName = "work-items.json";
        private readonly Dictionary<Guid, WorkItem> _workItems;

        public FileWorkItemsRepository()
        {
            _workItems = new Dictionary<Guid, WorkItem>();

            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        var itemsArray = JsonConvert.DeserializeObject<WorkItem[]>(json);
                        if (itemsArray != null)
                        {
                            _workItems = itemsArray.ToDictionary(w => w.Id, w => w);
                        }
                    }
                    catch (Newtonsoft.Json.JsonException)
                    {
                        _workItems = new Dictionary<Guid, WorkItem>();
                    }
                }
            }
        }

        public Guid Add(WorkItem workItem)
        {
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));

            var copy = workItem.Clone();
            copy.Id = Guid.NewGuid();

            _workItems[copy.Id] = copy;
            return copy.Id;
        }

        public WorkItem Get(Guid id)
        {
            return _workItems.TryGetValue(id, out var item) ? item : null;
        }

        public WorkItem[] GetAll()
        {
            return _workItems.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (workItem == null || !_workItems.ContainsKey(workItem.Id))
                return false;

            _workItems[workItem.Id] = workItem.Clone();
            return true;
        }

        public bool Remove(Guid id)
        {
            return _workItems.Remove(id);
        }

        public void SaveChanges()
        {
            var itemsArray = _workItems.Values.ToArray();
            var json = JsonConvert.SerializeObject(itemsArray, Formatting.Indented);
            File.WriteAllText(FileName, json);
        }
    }
}
