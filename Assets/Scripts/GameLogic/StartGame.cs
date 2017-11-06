using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZFrameWork;
using System;
using Jhqc.EditorCommon;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Init();
        // Server Net
        //InitJHQCNet();

        // Mouse Manager
        MouseManager.Instance.OnInit();

        // Level Manager
        LevelManager.Instance.OnInit();

        // UI Pool
        UIPoolManager.Instance.OnInit();

        // Load First Scene
        LoadFirstScn();
    }

    private void Init()
    {
        DontDestroyOnLoad(this);
        Application.runInBackground = true;
    }

    /// <summary>
    /// Init Net
    /// </summary>
    private void InitJHQCNet()
    {
        WWWManager.Instance.Init(Defines.ServerAddress, Jhqc.EditorCommon.LogType.None);// 外网
        WWWManager.Instance.TimeOut = 600f;
    }

    /// <summary>
    /// Load First Scene
    /// </summary>
    private void LoadFirstScn()
    {
        // Register Login Scene Module
        ModuleManager.Instance.Register(typeof(StartScn));
    }

    void Update()
    {
        NetMgr.Update();
    }
}
