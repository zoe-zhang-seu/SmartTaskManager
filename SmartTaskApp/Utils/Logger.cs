public static class Logger
{
    private static readonly string LogFilePath = "Assets/logs.txt";

    public static void Write(string message,string? path = null)
    {
        string logPath = path ?? LogFilePath;
        var logLine = $"{message}";
        
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(logPath))!);

        using var writer = new StreamWriter(logPath, append: true);
        writer.WriteLine(logLine);
    }
}