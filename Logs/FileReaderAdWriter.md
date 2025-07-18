### Read

1. path issue

- path read from AppContext.BaseDirectory → bin/Debug/net8.0/
so we have to go up level to reach Asset folder

```csharp
 // path read from AppContext.BaseDirectory → bin/Debug/net8.0/, so have to go up 3 levels to reach the Assets folder
var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Assets", "tasks.txt");
// Get the absolute path to the tasks.txt file
var fullPath = Path.GetFullPath(path);

```

```csharp
foreach (var line in File.ReadLines(filePath))
{
    //make line become object
}

```

#### why not use streamReader

```csharp
foreach (var line in File.ReadLines(filePath))
{

}

//vs

using (var reader = new StreamReader(filePath))
{
    string? line;
    while ((line = reader.ReadLine()) != null)
    {
        // process each line
    }
}

```

Compared to using `StreamReader` directly, `File.ReadLines` is:

-  Simpler and more readable
-  Automatically manages file resources (no need for a using block)
-  Supports lazy loading, which is ideal for large files

### Write

- create a file when it is not exist

```csharp
Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(logPath))!);
```

```csharp
using var writer = new StreamWriter(filePath);

writer.WriteLine($"");
```

#### JSON

When it comes to json

1. emtpy constructor to enable deserialize 

```csharp
 public TaskItem() { } //for json serialization
```

1. taskItem id when add, will

```csharp
if (tasks.Count>0&&tasks.Any(t => t.Id==task.Id) )
{
    task.Id = GetMaxId() + 1;
}
```

1. switch extensiong

```csharp
var ext = Path.GetExtension(filePath).ToLower();
return ext switch
{
    ".txt" or ".csv" => LoadFromTxt(filePath),
    ".json" => LoadFromJson(filePath),
    _ => throw new NotSupportedException($"Unsupported file type: {ext}")
};

```