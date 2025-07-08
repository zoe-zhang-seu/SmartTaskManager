using SmartTaskApp.Models;
using System;

namespace SmartTaskApp.Events;

public class TaskEventArgs : EventArgs// TaskEventArgs is a custom EventArgs class to pass task information
{
    public TaskItem Task { get; }

    public TaskEventArgs(TaskItem task)//why need pass taskitem as parameter?
                                       // This constructor initializes the Task property with the provided task item
                                       // It allows the event handler to access the task that triggered the event
    {
        Task = task;
    }
}

public class TaskAddedEventArgs : EventArgs
{
    public TaskItem Task { get; }
    public DateTime AddedAt { get; }
     public string AddedBy { get; } = "User";

    public TaskAddedEventArgs(TaskItem task)
    {
        Task = task;
        AddedAt = DateTime.Now;
        AddedBy = "User"; // Default value, can be changed later
    }
}

public class TaskRemovedEventArgs : EventArgs
{
    public TaskItem Task { get; }
    public string RemovedBy { get; } = "User";
     public DateTime AddedAt { get; }
    public TaskRemovedEventArgs(TaskItem task)
    {
        Task = task;
        AddedAt = DateTime.Now;
        RemovedBy = "User"; // Default value, can be changed later
    }
}

public class TaskNotifier
{
    public event EventHandler<TaskEventArgs>? TaskCompleted;
    // The event handler will receive an instance of TaskEventArgs, which contains the task that was completed.
    public event EventHandler<TaskAddedEventArgs>? TaskAdded;
    public event EventHandler<TaskRemovedEventArgs>? TaskRemoved;
    public void Notify(TaskItem task)
    {
        if (task.Status == Models.TaskStatus.Completed)
        {
            TaskCompleted?.Invoke(this, new TaskEventArgs(task));
            //this is the instance of TaskNotifier that raised the event
            // TaskCompleted is the event that is being raised
            //cannot be invoked by others
        }
    }

    public void NotifyAdded(TaskItem task)
    {
        TaskAdded?.Invoke(this, new TaskAddedEventArgs(task));
    }

    public void NotifyRemoved(TaskItem task)
    {
        TaskRemoved?.Invoke(this, new TaskRemovedEventArgs(task));
    }

}
