using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZFrameWork;

public class VillageModule : BaseModule
{
    #region Container

    #endregion

    #region Init & Register
    protected override void OnReady()
    {
        base.OnReady();
        DataManager.Instance.ReadInventoryInfo();
    }

    protected override void Register()
    {
        base.Register();
    }

    protected override void InitContainer()
    {
        base.InitContainer();

    }
    #endregion
}
