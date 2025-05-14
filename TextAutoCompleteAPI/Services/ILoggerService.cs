using System;
using System.IO;
using System.Threading.Tasks;

public interface ILoggerService
{
    Task LogApi(string input, int statusCode, string message = "");
}
