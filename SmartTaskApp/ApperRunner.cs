using SmartTaskApp.Models;
using SmartTaskApp.Services;
using SmartTaskApp.Events;
using SmartTaskApp.Utils;
using System.Net.Mime;

namespace SmartTaskApp;

public class AppRunner
{
    private readonly TaskManager<TaskItem> taskManager = new();
    private readonly TaskManager<AdvancedTaskItem> advancedTaskManager = new();
    private readonly TaskNotifier notifier = new();

    private readonly string taskFilePath = FileReader.GetAssetPath("tasks.json");//tasks.txt or tasks.csv
    private readonly string logFilePath = FileReader.GetAssetPath("log.txt");
    private readonly string advancedFilePath = FileReader.GetAssetPath("advanced_tasks.json");

    public AppRunner()
    {
        // Subscribe to events
        notifier.TaskCompleted += (sender, e) =>
        {
            string content = $"Task completed: {e.Task.Title} (Created at {e.Task.CreatedAt:t})";
            Console.WriteLine(content);
            Logger.Write(content, logFilePath);
        };

        notifier.TaskAdded += (sender, e) =>
        {
            string content = $"Task added: {e.Task.Title} (at {e.AddedAt:t})";

            Console.WriteLine(content);
            Logger.Write(content, logFilePath);
        };

        notifier.TaskRemoved += (sender, e) =>
        {
            string content = $"Task removed: {e.Task.Title} (by {e.RemovedBy})";
            Console.WriteLine(content);
            Logger.Write(content, logFilePath);
        };
    }

    public void Run()
    {
        LoadInitialTasks();
        PrintAdvancedSummaries();
        ShowMainMenu();
    }
    public void LoadInitialTasks()
    {
        // path read from AppContext.BaseDirectory → bin/Debug/net8.0/, so have to go up 3 levels to reach the Assets folder
        var preloadedTasks = Utils.FileReader.LoadTasks(taskFilePath);

        foreach (var task in preloadedTasks)
        {
            taskManager.Add(task);
        }
    }
    public void PrintAdvancedSummaries()
    {
        var advancedTasks = FileReader.LoadAdvancedTasksFromJson(advancedFilePath);
        foreach (var task in advancedTasks)
            advancedTaskManager.Add(task);

        Console.WriteLine("=== High Priority Tasks ===");
        foreach (var task in advancedTaskManager.Filter(t => t.Priority >= 3))
            Console.WriteLine(task);

        Console.WriteLine("=== Urgent Tasks ===");
        foreach (var task in advancedTaskManager.Filter(t => t.IsUrgent()))
            Console.WriteLine("⚠️ " + task.Title);
    }

    public void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\n========== SmartTask ==========");
            Console.WriteLine("1. Add task");
            Console.WriteLine("2. View all tasks");
            Console.WriteLine("3. View completed tasks");
            Console.WriteLine("4. Mark task as completed");
            Console.WriteLine("5. Delete task");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option (1-6): ");

            string? input = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (input)
                {
                    case "1":
                        Console.Write("Enter task title: ");
                        string? title = Console.ReadLine()?.Trim();
                        if (string.IsNullOrWhiteSpace(title))
                            throw new ArgumentException("Task title cannot be empty.");
                        var task = new TaskItem(title);
                        taskManager.Add(task);
                        FileReader.SaveTasks(taskManager.GetAll(), taskFilePath);
                        notifier.NotifyAdded(task);
                        break;

                    case "2":
                        Console.WriteLine("All tasks:");
                        foreach (var t in taskManager.GetAll())
                            Console.WriteLine($"- {t}");
                        break;

                    case "3":
                        Console.WriteLine("Completed tasks:");
                        foreach (var t in taskManager.GetByStatus(Models.TaskStatus.Completed))
                            Console.WriteLine($"- {t}");
                        break;

                    case "4":
                        Console.Write("Enter the ID of the task to complete: ");
                        if (!int.TryParse(Console.ReadLine(), out int completeId))
                            throw new ArgumentException("Invalid task ID.");
                        var taskToComplete = taskManager.GetAll().FirstOrDefault(t => t.Id == completeId);
                        if (taskToComplete == null)
                            throw new InvalidOperationException("Task not found.");
                        taskToComplete.Status = Models.TaskStatus.Completed;
                        notifier.Notify(taskToComplete);
                        break;

                    case "5":
                        Console.Write("Enter the ID of the task to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int deleteId))
                            throw new ArgumentException("Invalid task ID.");
                        var taskToDelete = taskManager.GetAll().FirstOrDefault(t => t.Id == deleteId);
                        if (taskToDelete == null)
                            throw new InvalidOperationException("Task not found.");
                        taskManager.Remove(taskToDelete.Title!);
                        FileReader.SaveTasks(taskManager.GetAll(), taskFilePath);
                        notifier.NotifyRemoved(taskToDelete);
                        break;

                    case "6":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Please enter a number between 1 and 6.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
