using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
public class TeamModule : BaseModule
{

    protected override void OnReady()
    {
        base.OnReady();
    }

    protected override void Register()
    {
        base.Register();

        RegisterMsg(MsgType.Team_GetTeamList, LoadTeamList);
        RegisterMsg(MsgType.Team_GetAchive, LoadArchive);
        RegisterMsg(MsgType.Team_GetTeamInfo, LoadTeamInfo);

        RegisterMsg(MsgType.Team_QuitTeam, QuitTeam);
        RegisterMsg(MsgType.Team_JoinTeam, JoinTeam);
        RegisterMsg(MsgType.Team_NewTeam, NewTeam);
        RegisterMsg(MsgType.Team_Fighting,StartFight);
        RgisterNetMsg();
    }

    private void RgisterNetMsg()
    {
        // TeamListPanel监听
        NetMgr.AddListener(MSG_TEAM.GetAchieve, RecvGetAchieve);
        NetMgr.AddListener(MSG_TEAM.GetTeamList, RecvGetRoomList);

        // RoomPanel监听
        NetMgr.AddListener(MSG_TEAM.GetRoomInfo, RecvGetRoomInfo);
        NetMgr.AddListener(MSG_TEAM.Fight, RecvFight);
    }

    #region Local Message
    Action<Message> msgCb_TeamList;
    Action<Message> msgCb_TeamInfo;
    Action<Message> msgCb_Archive;
    Action<Message> msgCb_Fighting;
    private void LoadTeamList(Message _msg)
    {
        // 查新所有队伍信息
        msgCb_TeamList = _msg.Done;
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString(MSG_TEAM.GetTeamList);
        NetMgr.Send(protocol);
    }

    private void LoadTeamInfo(Message _msg)
    {
        msgCb_TeamInfo = _msg.Done;

        //发送查询
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString(MSG_TEAM.GetRoomInfo);
        NetMgr.srvConn.Send(protocol);
    }

    private void LoadArchive(Message _msg)
    {
        msgCb_Archive = _msg.Done;
        // 获取当前玩家战绩
        ProtocolBytes protocol = new ProtocolBytes();
        protocol = new ProtocolBytes();
        protocol.AddString(MSG_TEAM.GetAchieve);
        NetMgr.Send(protocol);
    }

    //收到GetAchieve协议:战绩信息
    public void RecvGetAchieve(ProtocolBase protocol)
    {
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        Message msg = new Message();
        msg.Content = proto;
        msgCb_Archive(msg);
    }

    //收到GetRoomList协议：房间列表
    public void RecvGetRoomList(ProtocolBase protocol)
    {
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        Message msg = new Message();
        msg.Content = proto;
        msgCb_TeamList(msg);
    }

    /// <summary>
    /// Get My Join Room Information
    /// </summary>
    /// <param name="protocol"></param>
    public void RecvGetRoomInfo(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        Message msg = new Message();
        msg.Content = proto;
        if (msgCb_TeamInfo !=null)
        {
            msgCb_TeamInfo(msg);
        }
    }
    
    /// <summary>
    /// Join Team
    /// </summary>
    /// <param name="_msg"></param>
    private void JoinTeam(Message _msg)
    {
        int nId = (int)_msg["id"];
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString(MSG_TEAM.EnterRoom);
        protocol.AddInt(nId);
        NetMgr.SendWithListenOnce(protocol, (p)=> 
        {
            //解析参数
            ProtocolBytes proto = (ProtocolBytes)p;
            int start = 0;
            string protoName = proto.GetString(start, ref start);
            int ret = proto.GetInt(start, ref start);

            bool bSuc = 0 == ret;
            Message msg = new Message();
            msg.Content = bSuc;
            _msg.Done(msg);
        });
    }

    private void QuitTeam(Message _msg)
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString(MSG_TEAM.LeaveRoom);
        NetMgr.SendWithListenOnce(protocol, (p) => 
        {
            //获取数值
            ProtocolBytes proto = (ProtocolBytes)protocol;
            int start = 0;
            string protoName = proto.GetString(start, ref start);
            int ret = proto.GetInt(start, ref start);
            bool bSuc = ret == 0;
            Message msg = new Message();
            msg.Content = bSuc;
            _msg.Done(msg);
        });
    }
    
    private void NewTeam(Message _msg)
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString(MSG_TEAM.CreateRoom);
        NetMgr.SendWithListenOnce(protocol, (p)=> 
        {
            //解析参数
            ProtocolBytes proto = (ProtocolBytes)protocol;
            int start = 0;
            string protoName = proto.GetString(start, ref start);
            int ret = proto.GetInt(start, ref start);
            bool bSuc = 0 == ret;
            Message msg = new Message();
            msg.Content = bSuc;
            _msg.Done(msg);
        });
    }
    
    private void StartFight(Message _msg)
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString(MSG_TEAM.StartFight);
        NetMgr.SendWithListenOnce(protocol, (p)=> 
        {
            Message msg = new Message();
            msg.Content = p;
            _msg.Done(msg);
        });
    }

    #endregion
   
    /// <summary>
    /// Net Player Receive BroadCast
    /// </summary>
    /// <param name="protocol"></param>
    public void RecvFight(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        NetMgr.CacheNetMsg(protocol.GetName(), protocol);
        LevelManager.Instance.ChangeScene(ScnType.BattleScene, UIType.Battle);
    }
}
