using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UniLogger
{
	internal delegate void LoggerCallBackDelegate(bool isInfo, string msg);
	private static string _splitSymbol0;

	private static string _splitSymbol1;

	private static string _splitSymbol2;

	private static string _splitSymbol3;

	private static string _splitSymbol4;

	private static string _splitSymbol5;

	private static string _splitSymbol6;

	private static string _splitSymbol7;

	private static string _splitSymbol8;

	private static StringBuilder _sb;

	/// <summary>
	/// UniLogger 的内部日志分发事件：外部只能订阅/退订（+= / -=），不能直接覆盖赋值，避免误伤主链路。
	/// </summary>
	internal static event LoggerCallBackDelegate LoggerCallBack;

	static UniLogger()
	{
		_splitSymbol0 = "Assert(false)!";
		_splitSymbol1 = "  ";
		_splitSymbol2 = "NULL";
		_splitSymbol3 = "Debug:";
		_splitSymbol4 = "TickCount:[";
		_splitSymbol5 = "]FrameCount:[";
		_splitSymbol6 = "]";
		_splitSymbol7 = "[";
		_splitSymbol8 = ";";
		_sb = new StringBuilder();
		LogManager.Instance.Initialize();
	}

	public static void Assert(bool condition, params object[] args)
	{
		if (!condition)
		{
			DoCallBack(isInfo: false, Combine(_splitSymbol0, args));
		}
	}

	public static void LogTime(string arg)
	{
		DoCallBack(isInfo: true, Combine(arg));
	}

	public static void Log(string arg)
	{
		DoCallBack(isInfo: true, Combine(arg));
	}

	public static void Log(params object[] args)
	{
		DoCallBack(isInfo: true, Combine(args));
	}

	public static void LogFormat(string formatStr, params object[] args)
	{
		DoCallBack(isInfo: true, Combine(string.Format(formatStr, args)));
	}

	public static void LogWarning(string arg)
	{
		DoCallBack(isInfo: true, Combine(arg));
	}

	public static void LogWarning(params object[] args)
	{
		DoCallBack(isInfo: true, Combine(args));
	}

	public static void LogWarningFormat(string formatStr, params object[] args)
	{
		DoCallBack(isInfo: true, Combine(string.Format(formatStr, args)));
	}

	public static void LogError(string arg)
	{
		DoCallBack(isInfo: false, Combine(arg));
	}

	public static void LogError(params object[] args)
	{
		DoCallBack(isInfo: false, Combine(args));
	}

	public static void LogErrorFormat(string formatStr, params object[] args)
	{
		DoCallBack(isInfo: false, Combine(string.Format(formatStr, args)));
	}

	public static void LogException(Exception e)
	{
		DoCallBack(isInfo: false, Combine(e.Message, e.Source, e.StackTrace.Trim(), e.StackTrace));
	}

	[Conditional("ASSERT")]
	public static void DebugAssert(bool condition, params object[] args)
	{
		if (condition)
		{
		}
	}

	[Conditional("ASSERT")]
	public static void DebugLogTime(string arg)
	{
		Debug.Log((object)CombineDebug(_splitSymbol4, Environment.TickCount, _splitSymbol5, Time.frameCount, _splitSymbol6, arg));
	}

	[Conditional("ASSERT")]
	public static void DebugLog(string arg)
	{
		Debug.Log((object)CombineDebug(arg));
	}

	[Conditional("ASSERT")]
	public static void DebugLog(params object[] args)
	{
		Debug.Log((object)CombineDebug(args));
	}

	[Conditional("ASSERT")]
	public static void DebugLogFormat(string formatStr, params object[] args)
	{
		Debug.Log((object)CombineDebug(string.Format(formatStr, args)));
	}

	[Conditional("ASSERT")]
	public static void DebugLogWarning(string arg)
	{
		Debug.LogWarning((object)CombineDebug(arg));
	}

	[Conditional("ASSERT")]
	public static void DebugLogWarning(params object[] args)
	{
		Debug.LogWarning((object)CombineDebug(args));
	}

	[Conditional("ASSERT")]
	public static void DebugLogWarningFormat(string formatStr, params object[] args)
	{
		Debug.LogWarning((object)CombineDebug(string.Format(formatStr, args)));
	}

	[Conditional("ASSERT")]
	public static void DebugLogError(string arg)
	{
		Debug.LogError((object)CombineDebug(arg));
	}

	[Conditional("ASSERT")]
	public static void DebugLogError(params object[] args)
	{
		Debug.LogError((object)CombineDebug(args));
	}

	[Conditional("ASSERT")]
	public static void DebugLogErrorFormat(string formatStr, params object[] args)
	{
		Debug.LogError((object)CombineDebug(string.Format(formatStr, args)));
	}

	[Conditional("ASSERT")]
	public static void DebugLogException(Exception e)
	{
		Debug.LogException(e);
	}

	private static string Combine(params object[] args)
	{
		lock (_sb)
		{
			_sb.Remove(0, _sb.Length);
			WriteTimeStamp(_sb);
			WriteToSB(_sb, args);
			return _sb.ToString();
		}
	}

	private static string Combine(string head, object[] args)
	{
		lock (_sb)
		{
			_sb.Remove(0, _sb.Length);
			WriteTimeStamp(_sb);
			_sb.Append(head);
			WriteToSB(_sb, args);
			return _sb.ToString();
		}
	}

	private static string CombineDebug(params object[] args)
	{
		return Combine(_splitSymbol3, args);
	}

	private static string CombineDebug(string head, object[] args)
	{
		lock (_sb)
		{
			_sb.Remove(0, _sb.Length);
			WriteTimeStamp(_sb);
			_sb.Append(_splitSymbol3);
			_sb.Append(head);
			WriteToSB(_sb, args);
			return _sb.ToString();
		}
	}

	private static void WriteTimeStamp(StringBuilder theSB)
	{
		theSB.Append(_splitSymbol4);
		theSB.Append(TimeUtils.GetSystemTicksMS());
		if (Thread.CurrentThread == null || !Thread.CurrentThread.IsBackground)
		{
			theSB.Append(_splitSymbol5);
			theSB.Append(Time.frameCount);
		}
		theSB.Append(_splitSymbol6);
	}

	private static void WriteToSB(StringBuilder theSB, object[] args)
	{
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == null)
			{
				theSB.Append(_splitSymbol2);
			}
			else if (args[i] is Array)
			{
				object[] array = args[i] as object[];
				theSB.Append(_splitSymbol7);
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] == null)
						{
							theSB.Append(_splitSymbol2);
						}
						else
						{
							theSB.Append(array[j]);
						}
						theSB.Append(_splitSymbol8);
					}
				}
				else
				{
					theSB.Append(_splitSymbol2);
				}
				theSB.Append(_splitSymbol6);
			}
			else
			{
				theSB.Append(args[i]);
			}
			theSB.Append(_splitSymbol1);
		}
	}

	private static void DoCallBack(bool isInfo, string msg)
	{
		// 事件触发做一次本地拷贝，避免多线程下的竞态
		LoggerCallBackDelegate cb = LoggerCallBack;
		cb?.Invoke(isInfo, msg);
	}
}
