using System.Collections.Generic;
using System.Threading;
internal class LogWriter : ILogWriter
{
	private Dictionary<string, LogGroup> _groups = new Dictionary<string, LogGroup>();

	private LogGroup _null_groups = null;

	private LogGroup _error_groups = null;

	private List<LogGroup> _tmp_groups = new List<LogGroup>();

	private Thread _thread = null;

	private bool _isRunning = false;

	private AutoResetEvent _event = new AutoResetEvent(initialState: false);

	private object _locker = new object();

	private string _logDir = string.Empty;

	public LogWriter(string logDir)
	{
		_logDir = logDir;
	}

	public void Start()
	{
		_thread = new Thread(DoWriteFile);
		_thread.IsBackground = true;
		_thread.Start();
	}

	public void Stop()
	{
		_isRunning = false;
		_event.Set();
		if (_thread != null)
		{
			_thread.Join(1000);
			_thread = null;
		}
	}

	public void Pause()
	{
		List<LogGroup> list = new List<LogGroup>();
		lock (_locker)
		{
			if (_error_groups != null)
			{
				list.Add(_error_groups);
			}
			if (_null_groups != null)
			{
				list.Add(_null_groups);
			}
			Dictionary<string, LogGroup>.Enumerator enumerator = _groups.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					list.Add(enumerator.Current.Value);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].FreeHandler();
		}
	}

	public void Resume()
	{
		List<LogGroup> list = new List<LogGroup>();
		lock (_locker)
		{
			if (_error_groups != null)
			{
				list.Add(_error_groups);
			}
			if (_null_groups != null)
			{
				list.Add(_null_groups);
			}
			Dictionary<string, LogGroup>.Enumerator enumerator = _groups.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					list.Add(enumerator.Current.Value);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].AllocHandler();
		}
	}

	public virtual void AddInfo(string message, string gName = null)
	{
		LogGroup logGroup = GetGroup(gName);
		logGroup.Add(message);
		_event.Set();
	}

	public virtual void AddError(string message)
	{
		if (_error_groups == null)
		{
			_error_groups = new LogGroup("Error", GetFilePath("Error"));
		}
		_error_groups.Add(message);
		_event.Set();
	}

	public void Flush()
	{
		for (int i = 0; i < _tmp_groups.Count; i++)
		{
			_tmp_groups[i].ToFile();
		}
	}

	private LogGroup GetGroup(string gName)
	{
		if (string.IsNullOrEmpty(gName))
		{
			if (_null_groups == null)
			{
				_null_groups = new LogGroup("Default", GetFilePath("Default"));
			}
			return _null_groups;
		}
		_groups.TryGetValue(gName, out var value);
		if (value == null)
		{
			value = new LogGroup(gName, GetFilePath(gName));
			lock (_locker)
			{
				_groups[gName] = value;
			}
		}
		return value;
	}

	private void DoWriteFile()
	{
		_isRunning = true;
		while (_isRunning)
		{
			_event.WaitOne();
			_tmp_groups.Clear();
			lock (_locker)
			{
				if (_error_groups != null)
				{
					_tmp_groups.Add(_error_groups);
				}
				if (_null_groups != null)
				{
					_tmp_groups.Add(_null_groups);
				}
				Dictionary<string, LogGroup>.Enumerator enumerator = _groups.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						_tmp_groups.Add(enumerator.Current.Value);
					}
				}
				finally
				{
					enumerator.Dispose();
				}
			}
			for (int i = 0; i < _tmp_groups.Count; i++)
			{
				_tmp_groups[i].ToFile();
			}
			Thread.Sleep(1000);
		}
	}

	private string GetFilePath(string gName)
	{
		return string.Format("{0}/{1}.txt", _logDir, string.IsNullOrEmpty(gName) ? "Default" : gName);
	}
}
