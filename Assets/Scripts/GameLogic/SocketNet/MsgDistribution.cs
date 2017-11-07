using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//消息分发
public class MsgDistribution
{
    //每帧处理消息的数量
    public int num = 15;
    //消息列表
    public List<ProtocolBase> msgList = new List<ProtocolBase>();
    //委托类型
    public Action<ProtocolBase> cb;
    //事件监听表
    private Dictionary<string, Action<ProtocolBase>> eventDict = new Dictionary<string, Action<ProtocolBase>>();
    private Dictionary<string, Action<ProtocolBase>> onceDict = new Dictionary<string, Action<ProtocolBase>>();

    //Update
    public void Update()
    {
        for (int i = 0; i < num; i++)
        {
            if (msgList.Count > 0)
            {
                DispatchMsgEvent(msgList[0]);
                lock (msgList)
                    msgList.RemoveAt(0);
            }
            else
            {
                break;
            }
        }
    }

    //消息分发
    public void DispatchMsgEvent(ProtocolBase protocol)
    {
        string name = protocol.GetName();
        Debug.Log("分发处理消息 " + name);
        if (eventDict.ContainsKey(name))
        {
            var cb = eventDict[name];
            if (cb != null)
            {
                cb(protocol);
            }
        }
        if (onceDict.ContainsKey(name))
        {
            var cb = onceDict[name];
            if (cb != null)
            {
                cb(protocol);
            }
            onceDict[name] = null;
            onceDict.Remove(name);
        }
    }

    //添加事件监听 
    public void AddListener(string name, Action<ProtocolBase> cb)
    {
        if (eventDict.ContainsKey(name))
            eventDict[name] += cb;
        else
            eventDict[name] = cb;
    }

    //添加单次监听事件
    public void AddOnceListener(string name, Action<ProtocolBase> cb)
    {
        if (onceDict.ContainsKey(name))
            onceDict[name] += cb;
        else
            onceDict[name] = cb;
    }

    //删除监听事件
    public void DelListener(string name, Action<ProtocolBase> cb)
    {
        if (eventDict.ContainsKey(name))
        {
            eventDict[name] -= cb;
            if (eventDict[name] == null)
                eventDict.Remove(name);
        }
    }

    //删除单次监听事件
    public void DelOnceListener(string name, Action<ProtocolBase> cb)
    {
        if (onceDict.ContainsKey(name))
        {
            onceDict[name] -= cb;
            if (onceDict[name] == null)
                onceDict.Remove(name);
        }
    }
}