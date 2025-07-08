# Event Flow in SmartTaskApp Project

This document explains how events are used and flow throughout the SmartTaskApp project. It focuses on the usage of C# `event` keyword, especially how `TaskAdded`, `TaskRemoved`, and `TaskCompleted` are defined, triggered, and handled across the application.

---

## Overview

In this project, we use **C# events** to decouple the logic of task state changes (like adding, removing, completing tasks) from how we respond to those changes (e.g., logging, UI updates). The observer (subscriber) pattern enables cleaner and more modular code.

---

## Project Structure

```
SmartTaskApp/
├── Models/
│   └── TaskItem.cs
├── Events/
│   └── TaskNotifier.cs
├── Services/
│   └── TaskManager.cs
├── Program.cs
└── README.md
```

---

## Event Definitions (`TaskNotifier.cs`)

```csharp
public event EventHandler<TaskAddedEventArgs>? TaskAdded;
public event EventHandler<TaskRemovedEventArgs>? TaskRemoved;
public event EventHandler<TaskEventArgs>? TaskCompleted;
```

These are the **public events** declared inside the `TaskNotifier` class. External components can subscribe to these events but cannot trigger them.

---

## 🎯 Event Arguments Classes

Each event type uses a specific `EventArgs` subclass to encapsulate data related to the event:

- `TaskAddedEventArgs` – includes the added task and timestamp
- `TaskRemovedEventArgs` – includes the removed task and who removed it
- `TaskEventArgs` – used for completion events

---

## Event Triggers (`TaskNotifier.cs`)

```csharp
public void NotifyAdded(TaskItem task)
{
    TaskAdded?.Invoke(this, new TaskAddedEventArgs(task));
}

public void NotifyRemoved(TaskItem task)
{
    TaskRemoved?.Invoke(this, new TaskRemovedEventArgs(task));
}

public void Notify(TaskItem task)
{
    if (task.Status == TaskStatus.Completed)
    {
        TaskCompleted?.Invoke(this, new TaskEventArgs(task));
    }
}
```

These methods encapsulate the logic to fire events when something meaningful happens. Other parts of the program call these methods rather than raising events directly.

---

##  Event Subscriptions (`Program.cs`)

```csharp
notifier.TaskAdded += (sender, e) =>
{
    Console.WriteLine($"Task added: {e.Task.Title} (at {e.AddedAt:t})");
};

notifier.TaskRemoved += (sender, e) =>
{
    Console.WriteLine($"Task removed: {e.Task.Title} (by {e.RemovedBy})");
};

notifier.TaskCompleted += (sender, e) =>
{
    Console.WriteLine($"Task completed: {e.Task.Title} (Created at {e.Task.CreatedAt:t})");
};
```

These anonymous functions respond to events. Once an event is raised, the corresponding handler runs.

---

## Example: Add Task Flow

1. User chooses "Add task" in menu.
2. A new `TaskItem` is created and added to `TaskManager`.
3. `notifier.NotifyAdded(task)` is called.
4. `TaskAdded` event is fired and handled in the `Program.cs` subscriber.
5. The console prints a confirmation message.

---

## Summary of Event Flow

| Action         | Trigger Method         | Event Raised       | Data Passed             | Handled Where         |
|----------------|------------------------|---------------------|--------------------------|------------------------|
| Add Task       | `NotifyAdded(task)`    | `TaskAdded`         | `TaskAddedEventArgs`     | `Program.cs`           |
| Remove Task    | `NotifyRemoved(task)`  | `TaskRemoved`       | `TaskRemovedEventArgs`   | `Program.cs`           |
| Complete Task  | `Notify(task)`         | `TaskCompleted`     | `TaskEventArgs`          | `Program.cs`           |

---

## Benefits of Using Events Here

- Decouples business logic from UI/feedback
- Allows easier extension (e.g., adding loggers, analytics)
- Follows clean architecture principles


