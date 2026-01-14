using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// 运行时日志模块自测脚本：
/// - 覆盖 UniLogger / Unity Debug 两条链路
/// - 覆盖数组日志拼接（曾有索引 bug）
/// - 覆盖 Pause/Resume/Flush 的基本流程（真机落盘时更有意义）
/// </summary>
public class LoggerSelfTest : MonoBehaviour
{
    [Header("Self Test")]
    public bool RunOnStart = true;
    public bool TriggerException = false;
    public float WaitSecondsAfterLog = 2.2f;

    private IEnumerator Start()
    {
        if (!RunOnStart)
        {
            yield break;
        }

        // 确保初始化（UniLogger 静态构造也会做，但这里更显式）
        LogManager.Instance.Initialize();

        string tag = $"[LoggerSelfTest] {DateTime.Now:HH:mm:ss.fff}";
        Debug.Log($"{tag} begin. persistentDataPath={Application.persistentDataPath}, platform={Application.platform}, isEditor={Application.isEditor}");

        List<string> before = SafeGetLatestLogs();

        // 1) UniLogger 基本日志
        UniLogger.Log(tag, " UniLogger.Log");
        UniLogger.LogWarning(tag + " UniLogger.LogWarning");
        UniLogger.LogError(tag + " UniLogger.LogError");

        // 2) 数组日志：确保走到 UniLogger.WriteToSB 的 Array 分支
        UniLogger.Log(tag, " array=", (object)new object[] { 1, null, "A", true });

        // 3) Unity 原生日志（用于验证 LogUnityListener：真机/非Editor 时会被抓取落盘）
        Debug.Log(tag + " Debug.Log");
        Debug.LogWarning(tag + " Debug.LogWarning");
        Debug.LogError(tag + " Debug.LogError");

        // 4) Pause/Resume 流程（Editor 下 writer=LogDebugWriter 时基本是 no-op；真机落盘时能测句柄释放/恢复）
        LogManager.Instance.PauseWrite();
        UniLogger.Log(tag + " during PauseWrite (should buffer when file writer is active)");
        LogManager.Instance.ResumeWrite();
        UniLogger.Log(tag + " after ResumeWrite (should flush later)");

        // 给写线程一点时间（LogWriter 每轮会 Sleep(1000)）
        yield return new WaitForSeconds(WaitSecondsAfterLog);

        // 主动 Flush（尤其在你想立刻看到文件时）
        try
        {
            LogManager.Instance.FlushWrite();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        List<string> after = SafeGetLatestLogs();

        Debug.Log($"{tag} end. latestLogs before={before.Count}, after={after.Count}");
        if (after.Count > 0)
        {
            Debug.Log($"{tag} sample latest log files:\n- {string.Join("\n- ", after)}");
        }

        if (TriggerException)
        {
            throw new Exception(tag + " TriggerException=true (intentional)");
        }
    }

    private static List<string> SafeGetLatestLogs()
    {
        try
        {
            return LogManager.Instance.GetLatestLogs() ?? new List<string>();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return new List<string>();
        }
    }
}


