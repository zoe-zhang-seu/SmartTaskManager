using SmartTaskApp.Models;
using System.Linq;

namespace SmartTaskApp.Services;

public class TaskManager<T> where T : TaskItem
{
    private List<T> tasks = [];

    public void Add(T task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("title cannot be null or empty", nameof(task.Title));
        tasks.Add(task);
    }

    public void Remove(string title)
    {
        var task = tasks.FirstOrDefault(t => t.Title == title);
        if (task == null)
            throw new InvalidOperationException("cannot find task with the specified title");
        tasks.Remove(task);
    }

    public IEnumerable<T> GetByStatus(Models.TaskStatus status)//why not use list? public List<T>
    // IEnumerable<T> is more flexible and allows for deferred execution, which can be more efficient
    {
        return tasks.Where(t => t.Status == status);
    }

    public IEnumerable<T> GetAll()
    {
        return tasks;
    }

    public T this[int index] => tasks[index];// Indexer to access tasks by index
                                             // to aovid write like manager.GetAll().ToList()[1]; 
    // public T this[int index]
    // {
    //     get { return tasks[index]; }
    // }
    public int Count => tasks.Count; // Property to get the number of tasks
}
