using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using LitJson;

public class LoginModule : BaseModule {

    #region Base
    public LoginModule()
    {
        this.AutoRegister = true;
    }

    protected override void OnReady()
    {
        base.OnReady();

        MessageCenter.Instance.AddListener(MsgType.Start_EnterGame, OnLogin);
    }

    protected override void OnRelease()
    {
        base.OnRelease();

        MessageCenter.Instance.RemoveListener(MsgType.Start_EnterGame, OnLogin);
    }
    #endregion

    #region Actions
    private void OnLogin(Message _msg)
    {
        string strUser = _msg["user"].ToString();
        string strPsw = _msg["psw"].ToString();

        //Message msg;
        LogicUtils.Instance.OnShowWaiting(1, "Login...",true);

        //连接服务器
        if (NetMgr.srvConn.status != Connection.Status.Connected)
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.srvConn.proto = new ProtocolBytes();
            if (!NetMgr.srvConn.Connect(host, port))
            {
                LogicUtils.Instance.OnAlert("网络连接错误！");
            }
        }
        //发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Login");
        protocol.AddString(strUser);
        protocol.AddString(strPsw);
        NetMgr.srvConn.Send(protocol, OnLoginBack);
    }

    public void OnLoginBack(ProtocolBase protocol)
    {
        LogicUtils.Instance.OnHideWaiting();
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        string strToken = proto.GetString(start);
        if (ret == 0)
        {
            Debug.Log(string.Format("GET Token:{0}",strToken));
            NetMgr.SetToken(strToken);
            //LevelManager.Instance.ChangeScene(ScnType.BattleScene, UIType.Battle);
            LevelManager.Instance.ChangeScene(ScnType.VillageScene, UIType.Village);
        }
        else
        {
            LogicUtils.Instance.OnAlert("登录失败，请检查用户名密码!");
        }
    }
    #endregion

}
