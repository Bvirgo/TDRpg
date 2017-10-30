using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZFrameWork;

public class TaskModule : BaseModule
{
    #region Container
    private List<TaskItem> m_pRoleTaskList;
    private Vector3 m_vNpcBoy;
    private Vector3 m_vNpcGirl;
    #endregion

    #region Init & Register
    protected override void OnReady()
    {
        base.OnReady();
        DataManager.Instance.ReadTaskConfig();
        RandomRoleTaskList();
    }

    protected override void Register()
    {
        base.Register();

        RegisterMsg(MsgType.Role_GetTaskInfo,RefreshTaskList);

        RegisterMsg(MsgType.Role_TaskItemClicked,TaskItemClicked);

        RegisterMsg(MsgType.Role_TaskStateChanged,TaskStateChanged);
    }

    protected override void InitContainer()
    {
        base.InitContainer();
        m_pRoleTaskList = new List<TaskItem>();
        m_vNpcBoy = new Vector3(118,0,-4.96f);
        m_vNpcGirl = new Vector3(39, 0, -13f);

    }
    #endregion

    #region Task Information

    /// **************************
    ///	Random Task List For Test 
    /// **************************
    private void RandomRoleTaskList()
    {
        for (int i = 1001; i < 1004; i++)
        {
            Task t = DataManager.Instance.OnGetTaskByID(i);
            TaskItem ti = new TaskItem(t);
            m_pRoleTaskList.Add(ti);
        }
    }

    private void RefreshTaskList(Message _msg)
    {
        Message msg = new Message(MsgType.Role_RefreshTaskUI,this);
        msg["data"] = m_pRoleTaskList;
        msg.Send();
    }

    /// <summary>
    /// Deal Task Find Way
    /// </summary>
    /// <param name="_msg"></param>
    private void TaskItemClicked(Message _msg)
    {
        TaskItem task = _msg["task"] as TaskItem;
        Vector3 vPos = task.m_task.IdNpc == 1 ? m_vNpcBoy : m_vNpcGirl;
        if (task.state == TaskProgress.NoStart 
            || task.state == TaskProgress.Complete)
        {
            Message msg = new Message(MsgType.Role_GoTargetPos, this);
            msg["target"] = vPos;
            msg["task"] = task;
            msg.Send();
        }
    }

    private void TaskStateChanged(Message _msg)
    {
        TaskItem task = _msg["task"] as TaskItem;
        switch (task.state)
        {
            case TaskProgress.NoStart:
                // Null
                break;
            case TaskProgress.Accept:
                // Todo:to battle

                break;
            case TaskProgress.Complete:
                // To qGet Reward
                break;
            case TaskProgress.Reward:
                // Close Task
                break;
            default:
                break;
        }
    }
    #endregion
}
