using System;
namespace SmartTaskApp.Models;

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}

public class TaskItem
{
    public string Title { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public DateTime CreatedAt { get; } = DateTime.Now;

    public TaskItem(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }

    public override string ToString()
    {
        return $"{Title} [{Status}] created {CreatedAt:t}";// Format: "Title [Status] created HH:mm"
    }

    public void UpdateStatus(TaskStatus newStatus)
    {
        Status = newStatus;
    }

    public bool IsOverdue(DateTime dueDate)
    {
        return DateTime.Now > dueDate;
    }

    public bool IsCompleted()
    {
        return Status == TaskStatus.Completed;
    }

    public bool IsPending()
    {
        return Status == TaskStatus.Pending;
    }
    public bool IsInProgress()
    {
        return Status == TaskStatus.InProgress;
    }


    public static bool TryParseDateTime(string input, out DateTime result)
    {
        return DateTime.TryParse(input, out result);
    }
}
