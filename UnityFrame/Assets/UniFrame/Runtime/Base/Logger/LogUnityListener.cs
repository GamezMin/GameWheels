using System.Text;
using UnityEngine;
internal class LogUnityListener
{
	public static bool IsEnabled = true;

	private static string _strSymbol = ">>";

	private ILogWriter _writer = null;

	private StringBuilder _sb = new StringBuilder();

	public LogUnityListener(ILogWriter writer)
	{
		_writer = writer;
	}

	public void Start()
	{
		Application.logMessageReceived += new Application.LogCallback(LogCallback);
	}

	public void Stop()
	{
		Application.logMessageReceived -= new Application.LogCallback(LogCallback);
	}

	public void LogCallback(string condition, string stackTrace, LogType type)
	{
		_sb.Remove(0, _sb.Length);
		_sb.Append(condition);
		_sb.AppendLine(_strSymbol);
		_sb.Append(stackTrace);
		if ((int)type == 0)
		{
			_writer.AddError(_sb.ToString());
		}
		else if ((int)type == 4)
		{
			_writer.AddError(_sb.ToString());
		}
		else if ((int)type == 1)
		{
			_writer.AddError(_sb.ToString());
		}
		else
		{
			_writer.AddInfo(_sb.ToString());
		}
	}
}
