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


### Write

- create a file when it is not exist

```csharp
Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(logPath))!);
```

```csharp
using var writer = new StreamWriter(filePath);

writer.WriteLine($"");
```
