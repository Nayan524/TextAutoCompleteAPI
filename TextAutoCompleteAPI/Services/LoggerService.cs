using System;

public class LoggerService : ILoggerService
{

    private readonly string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "api_logs.txt");

    public LoggerService()
    {
        var directory = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public async Task LogApi(string input, int statusCode, string message = "")
    {
        // Creating the log message
        var logMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - Input: {input}, StatusCode: {statusCode}, Message: {message}\n";

        // Add the message to the log file
        await File.AppendAllTextAsync(logFilePath, logMessage);
    }
}
