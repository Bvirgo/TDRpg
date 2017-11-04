using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;

public class TaskPanel : BasePanel
{
    #region UI 
    [HideInInspector, AutoUGUI]
    public Transform TaskInfo;
    [HideInInspector, AutoUGUI]
    public Button Btn_Close;
    [HideInInspector, AutoUGUI("TaskInfo/TaskList/Grid")]
    public Transform tfTaskGrid;
    [HideInInspector, AutoUGUI]
    public Transform UIMonsterTalk;
    [HideInInspector, AutoUGUI]
    public Image Spr_NPC;
    [HideInInspector, AutoUGUI]
    public Text Txt_Talk;
    [HideInInspector, AutoUGUI]
    public Button Btn_Accept;
    #endregion

    #region Container
    private GameObject m_objTaskItemPrefab;
    #endregion

    #region Init & Reigster
    public override UIType GetUIType()
    {
        return UIType.SubTask;
    }

    public override void OnShow()
    {
        base.OnShow();

        GetTaskList();

        ResetUI();
    }

    protected override void Register()
    {
        base.Register();

        RegisterMsg(MsgType.Role_RefreshTaskUI,RefreshTaskList);

        RegisterMsg(MsgType.Role_ArriveTargetPos, ShowNpcTalkPanel);
    }

    protected override void InitContainer()
    {
        base.InitContainer();
        m_objTaskItemPrefab = Resources.Load(UiConfig.SubUIItemPath + "TaskItem") as GameObject;
    } 

    protected override void InitUI()
    {
        base.InitUI();
        Btn_Close.onClick.AddListener(() =>
        {
            UIManager.Instance.HideSubPanel(UIType.SubTask);
        });
    }

    private void GetTaskList()
    {
        Message msg = new Message(MsgType.Role_GetTaskInfo, this);
        msg.Send();
    }

    private void ResetUI()
    {
        UIMonsterTalk.gameObject.SetActive(false);
        TaskInfo.gameObject.SetActive(true);
    }
    #endregion

    #region InterAct
    private void RefreshTaskList(Message _msg)
    {
        Utils.RemoveChildren(tfTaskGrid);
        var pTaskList = _msg["data"] as List<TaskItem>;
        for (int i = 0; i < pTaskList.Count; i++)
        {
            var task = pTaskList[i];
            Transform tfTask = GameObject.Instantiate(m_objTaskItemPrefab).transform;
            Button btn = tfTask.Find("Btn_Go").GetComponent<Button>();
            bool bShowBtn = task.state == TaskProgress.Reward ? false : true;
            btn.gameObject.SetActive(bShowBtn);
            btn.onClick.AddListener(()=> 
            {
                Message msg = new Message(MsgType.Role_TaskItemClicked,this);
                msg["task"] = task;
                msg.Send();
            });
            Utils.SetBtnName(btn, GetTaskBtnShowName(task.state));
            Image sprTaksType = tfTask.Find("SprTaskType").GetComponent<Image>();
            UIIconDefines.GetGoodsIcon(GetTaskSprName(task.m_task.TaskType), (spr) => 
            {
                sprTaksType.sprite = spr;
            });

            Image sprTask = tfTask.Find("Bg/SprTask").GetComponent<Image>();
            UIIconDefines.GetGoodsIcon(task.m_task.Icon, (spr) =>
            {
                sprTask.sprite = spr;
            });

            Text txtTaskName = tfTask.Find("TxtTaskName").GetComponent<Text>();
            txtTaskName.text = task.m_task.Name;
            Text txtTaskDes = tfTask.Find("TxtTaskDes").GetComponent<Text>();
            txtTaskDes.text = task.m_task.Des;
            Text txtGem = tfTask.Find("Reward1/TxtGem").GetComponent<Text>();
            txtGem.text = task.m_task.Diamond.ToString();
            Text txtGold = tfTask.Find("Reward2/TxtGold").GetComponent<Text>();
            txtGold.text = task.m_task.Coin.ToString();
            tfTask.SetParent(tfTaskGrid);
        }
        Utils.ResetRectTransform(tfTaskGrid, -1, pTaskList.Count * 88);
    }

    private void ShowNpcTalkPanel(Message _msg)
    {
        UIManager.Instance.OpenSubPanel(UIType.SubTask);
        UIMonsterTalk.gameObject.SetActive(true);
        TaskInfo.gameObject.SetActive(false);

        TaskItem task = _msg["task"] as TaskItem;
        string strTips = task.m_task.Des;
        string strBtnName = "接受";
        if (task.state == TaskProgress.Complete)
        {
            strTips = "勇士，我就知道你肯定能行的！\n小小意思，不成敬意！";
            strBtnName = "收下";
        }
        Txt_Talk.text = strTips;
        Utils.SetBtnName(Btn_Accept,strBtnName);
        Btn_Accept.onClick.RemoveAllListeners();
        Btn_Accept.onClick.AddListener(()=> 
        {
            task.state = task.state == TaskProgress.NoStart ? TaskProgress.Accept : TaskProgress.Reward;
            UIMonsterTalk.gameObject.SetActive(false);
        });
    }
    
    private string GetTaskBtnShowName(TaskProgress _tp)
    {
        string strName = "领取";
        switch (_tp)
        {
            case TaskProgress.Accept:
                strName = "出发";
                break;
            case TaskProgress.Complete:
                strName = "奖励";
                break;
            default:
                break;
        }
        return strName;
    }

    private string GetTaskSprName(TaskType _ty)
    {
        string strName = "pic_";
        switch (_ty)
        {
            case TaskType.Main:
                strName += "主线";
                break;
            case TaskType.Reward:
                strName += "奖赏";
                break;
            case TaskType.Daily:
                strName += "日常";
                break;
            default:
                break;
        }
        return strName;
    }
    #endregion

}
