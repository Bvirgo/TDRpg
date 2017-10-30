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
        
        NewPlayer();
    }

    protected override void InitContainer()
    {
        base.InitContainer();

        DataManager.Instance.ReadInventoryInfo(); 
    }
    
    protected override void Register()
    {
        base.Register();
        ModuleManager.Instance.RegisterModule(typeof(TaskModule));
        RegisterMsg(MsgType.Role_GoTargetPos,SetPlayerTargetPos);
    }

    private void NewPlayer()
    {
        RoleProperty rp = RoleManager.Instance.OnNewPlayer();
        rp.CurrentScene = this;
        AddActor(rp);
    }

    private void SetPlayerTargetPos(Message _msg)
    {
        TaskItem task = _msg["task"] as TaskItem;
        Vector3 vPost = (Vector3)_msg["target"];
        BaseActor ba = RoleManager.Instance.OnGetMainPlayer();
        if (ba != null)
        {
            PlayerMove pm = ba.gameObject.GetComponent<PlayerMove>();
            pm.SetTarget(vPost,()=> 
            {
                Message msg = new Message(MsgType.Role_ArriveTargetPos,this);
                msg["task"] = task;
                msg.Send();
            });
        }
    }

}
