using System;
using System.Diagnostics;
using System.IO;

public static class Logger
{
    private static readonly string LogFilePath = Path.Combine(FileSystem.AppDataDirectory, "app_log.txt");

    public static void Log(string message)
    {
        try
        {
            var logEntry = $"{DateTime.Now}: {message}\n";
            File.AppendAllText(LogFilePath, logEntry);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Logging failed: {ex.Message}");
        }
    }

    public static string GetLogs()
    {
        try
        {
            return File.Exists(LogFilePath) ? File.ReadAllText(LogFilePath) : "No logs available";
        }
        catch
        {
            return "Could not read logs";
        }
    }
} 