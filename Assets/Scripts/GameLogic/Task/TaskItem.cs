using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ZFrameWork;

public class TaskItem
{
    public string guid;
    public Task m_task;
    private TaskProgress m_taskPro;

    public TaskProgress state
    {
        get { return m_taskPro; }
        set
        {
            Message msg = new Message(MsgType.Role_TaskStateChanged,this);
            msg["old"] = m_taskPro;
            msg["task"] = this;
            msg.Send();
            m_taskPro = value;
        }
    }

    public TaskItem(Task _task)
    {
        guid = Guid.NewGuid().ToString();
        m_task = _task;
        m_taskPro = TaskProgress.NoStart;
    }

}
