using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LogManager
{
	public static bool LogToFile { get; set; } = false;

	private static bool _initialized;

	private static LogManager _instance;

	private LogUnityListener _unityListener = null;

	private ILogWriter _writer = null;

	private int _logDirCount = 10;

	private int _commitLogDirCount = 5;

	public static LogManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LogManager();
			}
			if (!_initialized && _instance != null)
			{
				_instance.Initialize();
			}
			return _instance;
		}
	}

	/// <summary>
	/// 主动 Flush 当前 writer（真机落盘时有意义；Editor 下是 no-op）。
	/// </summary>
	public void FlushWrite()
	{
		_writer?.Flush();
	}

	public void Initialize()
	{
		if (_initialized)
		{
			Debug.Log((object)"LogManager has been initialized");
			return;
		}
		string logDir = null;
		if (Application.platform != RuntimePlatform.WebGLPlayer)
		{
			try
			{
				logDir = GetLogOutDir();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				return;
			}
		}
		_initialized = true;
		if (!LogToFile || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			_writer = new LogDebugWriter();
		}
		else
		{
			_writer = new LogWriter(logDir);
			_unityListener = new LogUnityListener(_writer);
			_unityListener.Start();
		}

		// 绑定 UniLogger -> ILogWriter 桥接
		UniLogger.LoggerCallBack += OnUniLoggerMessage;

		_writer.Start();
		if (Application.platform != RuntimePlatform.WebGLPlayer)
		{
			registerSystemEvent();
			deleteOverLogDirCountDirs();
		}
	}

	public void UnInitialize()
	{
		UniLogger.LoggerCallBack -= OnUniLoggerMessage;

		if (_unityListener != null)
		{
			_unityListener.Stop();
			_unityListener = null;
		}
		if (_writer != null)
		{
			_writer.Stop();
			_writer = null;
		}
	}

	private void OnUniLoggerMessage(bool isInfo, string msg)
	{
		if (_writer == null) 
			return;
		if (isInfo)
			_writer.AddInfo(msg);
		else
			_writer.AddError(msg);
	}

	public void ManualLog(string condition, LogType type)
	{
		if (_unityListener != null)
		{
			_unityListener.LogCallback(condition, "", type);
		}
	}

	public List<string> GetLatestLogs()
	{
		List<string> list = new List<string>();
		List<string> logDirList = getLogDirList();
		int num = ((_commitLogDirCount < logDirList.Count) ? (logDirList.Count - _commitLogDirCount) : 0);
		for (int i = num; i < logDirList.Count; i++)
		{
			string[] files = Directory.GetFiles(logDirList[i], "*.txt");
			if (files != null)
			{
				list.AddRange(files);
			}
		}
		list.AddRange(getOtherLogFileList());
		return list;
	}

	public void PauseWrite()
	{
		if (_writer != null)
		{
			_writer.Pause();
		}
	}

	public void ResumeWrite()
	{
		if (_writer != null)
		{
			_writer.Resume();
		}
	}

	private void registerSystemEvent()
	{
		AppDomain currentDomain = AppDomain.CurrentDomain;
		if (currentDomain != null)
		{
			currentDomain.UnhandledException += uncaughtException;
			currentDomain.DomainUnload += onDomainUnload;
			currentDomain.ProcessExit += onProcessExit;
		}
	}

	private void uncaughtException(object sender, UnhandledExceptionEventArgs e)
	{
		if (e != null && e.ExceptionObject is Exception ex)
		{
			Debug.LogException(ex);
		}
		_writer.Flush();
	}

	private void onDomainUnload(object sender, EventArgs e)
	{
		_writer.Flush();
	}

	private void onProcessExit(object sender, EventArgs e)
	{
		_writer.Flush();
	}

	private void deleteOverLogDirCountDirs()
	{
		try
		{
			List<string> logDirList = getLogDirList();
			if (logDirList.Count > _logDirCount)
			{
				for (int i = 0; i < logDirList.Count - _logDirCount; i++)
				{
					FileUtils.DeleteDirectory(logDirList[i]);
				}
			}
		}
		catch (Exception e)
		{
			UniLogger.LogException(e);
		}
	}

	private List<string> getLogDirList()
	{
		string writePath = GetLogWriterPath();
		List<string> list = new List<string>();
		if (Directory.Exists(writePath))
		{
			list.AddRange(Directory.GetDirectories(writePath));
			list.Sort(compare);
		}
		return list;
	}

	private List<string> getOtherLogFileList()
	{
		string writePath = GetLogWriterPath();
		List<string> list = new List<string>();
		if (Directory.Exists(writePath))
		{
			list.AddRange(Directory.GetFiles(writePath, "*.txt", SearchOption.TopDirectoryOnly));
		}
		return list;
	}

	private int compare(string arg1, string arg2)
	{
		return arg1.CompareTo(arg2);
	}

	private string GetLogWriterPath()
	{
		string writePath = Path.Combine(Application.persistentDataPath, "Logs");
		return writePath;
	}
	private string GetLogOutDir()
	{
		string writePath = GetLogWriterPath();
		Debug.Log((object)("LogFilePath:" + writePath));
		if (!Directory.Exists(writePath))
		{
			Directory.CreateDirectory(writePath);
		}
		string text = writePath + "/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss (ffff)");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}
}
