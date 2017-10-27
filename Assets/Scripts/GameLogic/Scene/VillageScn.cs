using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class VillageScn : BaseScene
{
    #region Containers
    private GameObject m_objMainPlayer;
    #endregion
    public VillageScn()
    {
        this.AutoRegister = true;
    }

    protected override void OnReady()
    {
        base.OnReady();

        InitContainer();

        RegisterModule();

        NewPlayer();
    }

    private void InitContainer()
    {

    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }

    private void RegisterModule()
    {
        ModuleManager.Instance.RegisterModule(typeof(VillageModule));
    }

    private void NewPlayer()
    {
        RoleProperty rp = RoleManager.Instance.OnNewPlayer();
        rp.CurrentScene = this;
        AddActor(rp);
    }

}
