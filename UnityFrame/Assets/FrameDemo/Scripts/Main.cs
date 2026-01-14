using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Runtime;
using FrameDemo;
using UnityEngine;
using YooAsset;

public class Main : MonoBehaviour
{
    public bool LogToFile = false;
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    private async void Awake()
    {
        try
        {
            YooConst.PackageSettings[0].playMode = PlayMode;
            LogManager.LogToFile = LogToFile;
            var eventData = EventData.Instance;
            await GameFrame.Runtime.GameFrame.Instance.InitAsync();
            GameFrame.Runtime.GameFrame.Instance.AddFsmComponents(typeof(GameProcess));
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    void Start()
    {
        UniLogger.Log("Hello World");
        UniLogger.LogError("Hello Error");
    }
    
}
