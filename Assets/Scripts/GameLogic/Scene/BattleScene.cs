using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;

public class BattleScene : BaseScene
{
    #region Containers
    private GameObject m_objMainPlayer;
    private GameObject m_Spawn;
    #endregion

    #region Init & Register
    protected override void OnReady()
    {
        base.OnReady();

        m_Spawn = GameObject.Find("Spawn");

        //NewPlayer();

        ProtocolBase pb = NetMgr.GetCacheNetMsg("Fight");
        StartBattle(pb as ProtocolBytes);
    }

    protected override void Register()
    {
        base.Register();

        RegisterModule(typeof(SkillModule));

        // Play Background Music
        //PlayBackGroundMuisc();
    }

    private void PlayBackGroundMuisc()
    {
        string strClipPath = Defines.ResMusicPath + "bg-city";
        var mClip = ResManager.Instance.Load<AudioClip>(strClipPath);
        SoundManager.Instance.PlayBackgroundMusic(mClip);
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
        MainActor rp = RoleManager.Instance.OnNewMainPlayer(vPos, qRotation, false);
        if (rp.gameObject.GetComponent<PlayerAttack>() == null)
        {
            rp.gameObject.AddComponent<PlayerAttack>();
        }
        rp.CurrentScene = this;
        AddActor(rp);
    }

    protected override void OnRelease()
    {
        base.OnRelease();

        RoleManager.Instance.OnRemoveAllRole();
    }
    #endregion

    #region Battle Fighting
  
    //开始战斗
    public void StartBattle(ProtocolBytes proto)
    {
        //解析协议
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        if (protoName != "Fight")
            return;
        //坦克总数
        int count = proto.GetInt(start, ref start);

        //每一辆坦克
        for (int i = 0; i < count; i++)
        {
            string id = proto.GetString(start, ref start);
            int team = proto.GetInt(start, ref start);
            int swopID = proto.GetInt(start, ref start);
            GenerateTank(id, team, swopID);
        }
        NetMgr.srvConn.msgDist.AddListener("UpdateUnitInfo", RecvUpdateUnitInfo);
        //NetMgr.srvConn.msgDist.AddListener ("Shooting", RecvShooting);
        //NetMgr.srvConn.msgDist.AddListener ("Hit", RecvHit);
        //NetMgr.srvConn.msgDist.AddListener ("Result", RecvResult);
    }


    //产生坦克
    public void GenerateTank(string id, int team, int swopID)
    {
        //获取出生点
        Transform sp = GameObject.Find("SpawnPoints").transform;
        Transform swopTrans;
        if (team == 1)
        {
            Transform teamSwop = sp.GetChild(0);
            swopTrans = teamSwop.GetChild(swopID - 1);
        }
        else
        {
            Transform teamSwop = sp.GetChild(1);
            swopTrans = teamSwop.GetChild(swopID - 1);
        }
        if (swopTrans == null)
        {
            Debug.LogError("GenerateTank出生点错误！");
            return;
        }

        if (NetMgr.IsMainRole(id))
        {
            RoleManager.Instance.OnNewMainPlayer(swopTrans.position, swopTrans.rotation,false);
        }
        else
        {
            RoleManager.Instance.OnNewNetPlayer(swopTrans.position, swopTrans.rotation, id);
        }
    }


    public void RecvUpdateUnitInfo(ProtocolBase protocol)
    {
        //解析协议
        int start = 0;
        ProtocolBytes proto = (ProtocolBytes)protocol;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start, ref start);
        Vector3 nPos;
        Vector3 nRot;
        nPos.x = proto.GetFloat(start, ref start);
        nPos.y = proto.GetFloat(start, ref start);
        nPos.z = proto.GetFloat(start, ref start);
        nRot.x = proto.GetFloat(start, ref start);
        nRot.y = proto.GetFloat(start, ref start);
        nRot.z = proto.GetFloat(start, ref start);
        float turretY = proto.GetFloat(start, ref start);
        float gunX = proto.GetFloat(start, ref start);
        //处理
        Debug.Log("RecvUpdateUnitInfo " + id);
        BaseActor ba = RoleManager.Instance.OnGetRole(id);
        if (ba == null)
        {
            Debug.Log("RecvUpdateUnitInfo bt == null ");
            return;
        }
        if (ba.ActorType == ActorType.NetRole)
        {
            NetPlayerMove npm = ba.gameObject.GetComponent<NetPlayerMove>();
            npm.NetForecastInfo(nPos, nRot);
        }
    }
    #endregion
}
