using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SmartTaskApp.Models;
using System.Text.Json.Serialization;


namespace SmartTaskApp.Utils
{
    public static class FileReader
    {
        public static List<TaskItem> LoadTasks(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".txt" or ".csv" => LoadFromTxt(filePath),
                ".json" => LoadFromJson(filePath),
                _ => throw new NotSupportedException($"Unsupported file type: {ext}")
            };
        }

        public static List<TaskItem> LoadFromTxt(string filePath)
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

        private static List<TaskItem> LoadFromJson(string path)
        {
            if (!File.Exists(path)) return new();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }//important for enum
            };

            var json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<List<TaskItem>>(json,options) ?? new();
        }



        public static void SaveTasks(IEnumerable<TaskItem> tasks, string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            switch (ext)
            {
                case ".json":
                    var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(filePath, json);
                    break;

                case ".csv":
                case ".txt":
                    using (var writer = new StreamWriter(filePath))
                    {
                        foreach (var task in tasks)
                            writer.WriteLine($"{task.Id},{task.Title},{task.Status}");
                    }
                    break;

                default:
                    throw new NotSupportedException($"Unsupported file extension: {ext}");
            }
        }
    }
}
