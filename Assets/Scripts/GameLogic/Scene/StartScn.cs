using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

/// <summary>
/// Login Scene
/// </summary>
public class StartScn : BaseScene
{
    #region Init & Register

    protected override void OnReady()
    {
        base.OnReady();

        // Open Login View
        UIManager.Instance.OpenUI(UIType.Start, true);
    }

    protected override void Register()
    {
        base.Register();
        RegisterModule();
    }
    
    /// <summary>
    /// Registe Child Module
    /// </summary>
    private void RegisterModule()
    {
        RegisterModule(typeof(LoginModule));
        ModuleManager.Instance.Register(typeof(WindowModule));
        ModuleManager.Instance.Register(typeof(WaitingModule));
    }
    #endregion
}
