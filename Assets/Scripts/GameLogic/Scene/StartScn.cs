using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

/// <summary>
/// Login Scene
/// </summary>
public class StartScn : BaseScene
{
    #region Base
    public StartScn()
    {
        this.AutoRegister = true;
    }

    protected override void OnReady()
    {
        base.OnReady();

        // Open Login View
        UIManager.Instance.OpenUI(UIType.Start, true);

        RegisterModule();

    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }

    /// <summary>
    /// Registe Child Module
    /// </summary>
    private void RegisterModule()
    {
        ModuleManager.Instance.Register(typeof(LoginModule));
        ModuleManager.Instance.Register(typeof(WindowModule));
        ModuleManager.Instance.Register(typeof(WaitingModule));
    }
    #endregion
}
