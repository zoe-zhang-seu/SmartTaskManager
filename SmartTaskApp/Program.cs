using SmartTaskApp.Models;
using SmartTaskApp.Services;
using SmartTaskApp.Events;

Console.OutputEncoding = System.Text.Encoding.UTF8; // Support emojis

var taskManager = new TaskManager<TaskItem>();
var notifier = new TaskNotifier();

// Subscribe to event: Task Completed
notifier.TaskCompleted += (sender, e) =>
{
    Console.WriteLine($"Task completed: {e.Task.Title} (Created at {e.Task.CreatedAt:t})");
};

// Subscribe to event: Task Added
notifier.TaskAdded += (sender, e) =>
{
    Console.WriteLine($"Task added: {e.Task.Title} (at {e.AddedAt:t})");
};

notifier.TaskRemoved += (sender, e) =>
{
    Console.WriteLine($"Task removed: {e.Task.Title} (by {e.RemovedBy})");
};

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
                string? title = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(title))
                    throw new ArgumentException("Task title cannot be empty.");

                var task = new TaskItem(title);
                taskManager.Add(task);
                notifier.NotifyAdded(task);
                break;

            case "2":
                Console.WriteLine("All tasks:");
                foreach (var t in taskManager.GetAll())
                    Console.WriteLine($"- {t}");
                break;

            case "3":
                Console.WriteLine("Completed tasks:");
                foreach (var t in taskManager.GetByStatus(SmartTaskApp.Models.TaskStatus.Completed))
                    Console.WriteLine($"- {t}");
                break;

            case "4":
                Console.Write("Enter the title of the task to complete: ");
                string? completeTitle = Console.ReadLine();
                var taskToComplete = taskManager.GetAll().FirstOrDefault(t => t.Title == completeTitle);
                if (taskToComplete == null)
                    throw new InvalidOperationException("Task not found.");
                taskToComplete.Status = SmartTaskApp.Models.TaskStatus.Completed;
                notifier.Notify(taskToComplete);
                break;

            case "5":
                Console.Write("Enter the title of the task to delete: ");
                string? deleteTitle = Console.ReadLine();
                var taskToDelete = taskManager.GetAll().FirstOrDefault(t => t.Title == deleteTitle);
                if (taskToDelete == null)
                    throw new InvalidOperationException("Task not found.");
                taskManager.Remove(deleteTitle!);
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
        Console.WriteLine(" Error: " + ex.Message);
    }
}
