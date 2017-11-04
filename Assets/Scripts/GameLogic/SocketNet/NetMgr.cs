using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//网络管理
public class NetMgr
{
    public static Connection srvConn = new Connection();
    public static Dictionary<string, ProtocolBase> netMsgKey_netPro = new Dictionary<string, ProtocolBase>();
    //public static Connection platformConn = new Connection();
    public static void Update()
    {
        srvConn.Update();
        //platformConn.Update();
    }

    //心跳
    public static ProtocolBase GetHeatBeatProtocol()
    {
        //具体的发送内容根据服务端设定改动
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("HeatBeat");
        return protocol;
    }

    public static void SetToken(string _strToken)
    {
        srvConn.m_strToken = _strToken;
    }

    public static bool IsMainRole(string _strToken)
    {
        return !string.IsNullOrEmpty(srvConn.m_strToken) && srvConn.m_strToken.Equals(_strToken);
    }

    public static void CacheNetMsg(string _strName,ProtocolBase _pb)
    {
        netMsgKey_netPro.AddOrReplace(_strName,_pb);
    }

    public static ProtocolBase GetCacheNetMsg(string _strName)
    {
        ProtocolBase pb = new ProtocolBase();
        if (netMsgKey_netPro.ContainsKey(_strName))
        {
           pb= netMsgKey_netPro[_strName];
        }
        return pb;
    }
}