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

    protected override void OnLoad()
    {
        base.OnLoad();

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
        ModuleManager.Instance.RegisterModule(typeof(LoginModule));
        ModuleManager.Instance.RegisterModule(typeof(WindowModule));
        ModuleManager.Instance.RegisterModule(typeof(WaitingModule));
    }
    #endregion
}
