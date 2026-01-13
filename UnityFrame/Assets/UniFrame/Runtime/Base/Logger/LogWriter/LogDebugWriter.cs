using UnityEngine;

internal class LogDebugWriter : ILogWriter
{
    public void AddError(string message)
    {
        Debug.LogError(message);
    }

    public void AddInfo(string message, string gName = null)
    {
        Debug.Log(message);
    }

    public void Start()
    {
    }

    public void Stop()
    {
    }

    public void Flush()
    {
    }

    public void Pause()
    {
    }

    public void Resume()
    {
    }
}