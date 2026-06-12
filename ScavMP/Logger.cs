using BepInEx.Logging;
using LiteEntitySystem;

namespace ScavMP;

internal class Logger : ILogger
{
    ManualLogSource _logSource;

    public Logger(ManualLogSource logSource)
    {
        _logSource = logSource;
    }

    public void Log(string msg)
    {
        _logSource.LogInfo(msg);
    }

    public void LogWarning(string msg)
    {
        _logSource.LogWarning(msg);
    }

    public void LogError(string msg)
    {
        _logSource.LogError(msg);
    }
}
