using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class VillageScn : BaseScene
{
    #region Containers
    private GameObject m_objMainPlayer;
    private GameObject m_Spawn;
    #endregion

    protected override void OnReady()
    {
        base.OnReady();

        m_Spawn = GameObject.Find("Spawn");
        
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

        RegisterModule(typeof(TaskModule));
        RegisterModule(typeof(InventoryModule));
        RegisterModule(typeof(TeamModule));
        RegisterMsg(MsgType.Role_GoTargetPos,SetPlayerTargetPos);
    }
    
    private void NewPlayer()
    {
        Vector3 vPos = Vector3.zero;
        Quaternion qRotation = Quaternion.identity;
        if (m_Spawn != null)
        {
            vPos = m_Spawn.transform.position;
            qRotation = m_Spawn.transform.rotation;
        }
        MainActor rp = RoleManager.Instance.OnNewMainPlayer(vPos,qRotation);
        rp.CurrentScene = this;
        rp.m_bIsTeam = false;
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

    protected override void OnRelease()
    {
        base.OnRelease();

        RoleManager.Instance.OnRemoveAllRole();
    }

}
