using System;
using System.Text.Json.Serialization;

namespace SmartTaskApp.Models;

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}

public class TaskItem
{

    private static int _nextId = 1;
    public int Id { get; set; }
    public string? Title { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public DateTime CreatedAt { get; } = DateTime.Now;
    
    public TaskItem() { } //for json serialization

    public TaskItem(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Id = _nextId++;
        Status = TaskStatus.Pending;
        CreatedAt = DateTime.Now;
    }

    public TaskItem(int id, string title, TaskStatus status = TaskStatus.Pending)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Id = id;
        Status = status;
        CreatedAt = DateTime.Now;
        if (id >= _nextId)
         _nextId = id + 1; // Ensure next ID is always greater than the highest existing ID
    }

    public override string ToString()
    {
        return $"{Id}. {Title} [{Status}] created {CreatedAt:t}";// Format: "Title [Status] created HH:mm"
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
