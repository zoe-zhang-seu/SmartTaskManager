using SmartTaskApp.Models;

public class AdvancedTaskItem : TaskItem
{
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }

    public AdvancedTaskItem() { } // for deserialization

    public AdvancedTaskItem(string title, int priority = 1, DateTime? dueDate = null)
        : base(title)
    {
        Priority = priority;
        DueDate = dueDate;
    }

    public override string ToString()
    {
        var due = DueDate.HasValue ? DueDate.Value.ToShortDateString() : "No due";
        return $"{base.ToString()} [Priority: {Priority}, Due: {due}]";
    }

    public bool IsUrgent() =>
        DueDate.HasValue && DateTime.Now > DueDate.Value.AddDays(-1);

}
