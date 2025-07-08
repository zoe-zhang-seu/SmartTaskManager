## why use T type in taskItem

#  Why `Filter(Func<T, bool> predicate)` Works

We define a method in a generic class like this:

```csharp
public IEnumerable<T> Filter(Func<T, bool> predicate)
{
    return tasks.Where(predicate);
}
```
####  `IEnumerable<T>`

- This is the **return type**.
- It means "a list of items of type `T` that can be looped over".
- Example: `List<int>` and `T[]` are `IEnumerable<T>`.

###  `Func<T, bool> predicate`

- This is a **parameter**.
- `Func<T, bool>` means:
  - A function that takes one input of type `T`
  - And returns a `bool` (`true` or `false`)
- This function is called a **predicate**.

```csharp
Func<int, bool> isEven = x => x % 2 == 0;

Console.WriteLine(isEven(4)); // true
Console.WriteLine(isEven(5)); // false
```

so we can use it in `AppRunner`

```csharp
advancedTaskManager.Filter(t => t.Priority >= 3)
```

without filter

```csharp
advancedTaskManager.GetAll().Where(t => t.Priority >= 3)

```

