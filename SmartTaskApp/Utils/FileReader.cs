using System;
using System.Collections.Generic;
using System.IO;
using SmartTaskApp.Models;

namespace SmartTaskApp.Utils
{
    public static class FileReader
    {
        public static List<TaskItem> LoadTasks(string filePath)
        {
            var tasks = new List<TaskItem>();

            if (!File.Exists(filePath))
                return tasks;

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Trim().Split(',');
                if (parts.Length >= 3 &&
                    int.TryParse(parts[0], out int id) &&
                    Enum.TryParse(parts[2], out Models.TaskStatus status))
                {
                    tasks.Add(new TaskItem(id, parts[1].Trim(), status));
                }
            }

            return tasks;
        }

        public static void SaveTasks(IEnumerable<TaskItem> tasks, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            foreach (var task in tasks)
            {
                writer.WriteLine($"{task.Id},{task.Title},{task.Status}");
            }
        }
    }
}
