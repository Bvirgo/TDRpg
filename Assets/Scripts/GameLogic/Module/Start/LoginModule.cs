using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using LitJson;
using ProtoBuf;

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
            //NetMgr.srvConn.proto = new ProtocolPB();
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
            Debug.Log(string.Format("GET Token:{0}", strToken));
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

    private void TestJsonPro(ProtocolBase protocol)
    {
        ProtocolJson proto = (ProtocolJson)protocol;
        string protoName = proto.GetName();
        RspMsg rsp = proto.GetContent<RspMsg>();
        TMsg tg = JsonMapper.ToObject<TMsg>(rsp.strJsData);
        Debug.LogWarning(string.Format("Json:{0},{1} || TMsg:{2}", rsp.rspType, rsp.strTips, tg.name));
        return;
    }

}
/// <summary>
/// 消息对象
/// </summary>
[ProtoContract]
public class ChatMsg
{
    [ProtoMember(1)]
    public string sender;//发送者
    [ProtoMember(2)]
    public string msg;//消息
    [ProtoMember(3)]
    public List<string> data
    {
        get;
        set;
    }
    [ProtoMember(4)]
    public object content;
}

public class RspMsg
{
    public byte rspType;
    public string strTips;
    public string strJsData;
}

public enum RspType : byte
{
    None,
    NetError,
    LogicError,
    DataError
}
public class TMsg
{
    public TMsg()
    {
        name = "Sub TMSG";
        nID = 10;
    }
    public string name;//发送者
    public int nID;//消息
}