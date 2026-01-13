using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public bool LogToFile = false;

    private void Awake()
    {
        LogManager.LogToFile = LogToFile;
    }

    void Start()
    {
        UniLogger.Log("Hello World");
        UniLogger.LogError("Hello Error");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
