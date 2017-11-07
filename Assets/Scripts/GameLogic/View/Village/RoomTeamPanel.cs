using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;

public class RoomTeamPanel : BasePanel
{
    #region UI
    /// **************************
    ///	Battle Teams 
    /// **************************
    [HideInInspector, AutoUGUI]
    public Transform TeamList;
    [HideInInspector, AutoUGUI]
    public Transform Content;
    [HideInInspector, AutoUGUI]
    public Button CloseBtn;
    [HideInInspector, AutoUGUI]
    public Button Btn_Refrash;
    [HideInInspector, AutoUGUI]
    public Button NewBtn;

    /// **************************
    ///	Win Panel 
    /// **************************
    [HideInInspector, AutoUGUI]
    public Transform WinPanel;
    [HideInInspector, AutoUGUI]
    public Text IDText;
    [HideInInspector, AutoUGUI]
    public Text WinText;
    [HideInInspector, AutoUGUI]
    public Text LostText;

    /// **************************
    ///	Team Panel 
    /// **************************
    [HideInInspector, AutoUGUI]
    public Transform RoomPanel;
    [HideInInspector, AutoUGUI]
    public Button StartFightBtn;
    [HideInInspector, AutoUGUI]
    public Button CloseRoomBtn;
    private List<Transform> m_pRooms = new List<Transform>();
    private Transform RoomPrefab;
    #endregion

    #region Container
    private string m_strMainRoleGUID;
    #endregion

    #region Init & Register

    public override UIType GetUIType()
    {
        return UIType.SubTeamPanel;
    }

    protected override void OnReady()
    {
        base.OnReady();
        GetTeamMsg();
    }

    protected override void InitUI()
    {
        base.InitUI();
        //按钮事件
        Btn_Refrash.onClick.AddListener(() =>
        {
            Message msg = new Message(MsgType.Team_GetTeamList, this);
            msg.Done = (rsp) => { RecvGetRoomList(rsp.Content as ProtocolBytes); };
            msg.Send();
        });

        NewBtn.onClick.AddListener(() =>
        {
            Message msg = new Message(MsgType.Team_NewTeam, this);
            msg.Done = NewTeam;
            msg.Send();
        });

        CloseBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HideSubPanel(UIType.SubTeamPanel);
        });

        //按钮事件
        CloseRoomBtn.onClick.AddListener(() =>
        {
            Message msg = new Message(MsgType.Team_QuitTeam, this);
            msg.Done = p => { OnCloseTeam((bool)p.Content); };
            msg.Send();
        });

        StartFightBtn.onClick.AddListener(() =>
        {
            Message msg = new Message(MsgType.Team_Fighting, this);
            msg.Done = P => { OnStartBack(P.Content as ProtocolBytes); };
            msg.Send();
        });

        m_pRooms = new List<Transform>();
        for (int i = 0; i < 6; i++)
        {
            string name = "RoomPanel/PlayerPrefab" + i.ToString();
            Transform prefab = transform.Find(name);
            m_pRooms.Add(prefab);
        }

        var go = Resources.Load(UiConfig.SubUIItemPath + "RoomPrefab") as GameObject;
        RoomPrefab = go.transform;
        RoomPanel.gameObject.SetActive(false);
    }

    protected override void InitContainer()
    {
        base.InitContainer();
        m_strMainRoleGUID = RoleManager.Instance.OnGetMainPlayer().guid;
    }

    /// **************************
    ///	Get Teams From Net
    /// **************************
    private void GetTeamMsg()
    {
        Message msg = new Message(MsgType.Team_GetTeamList, this);
        msg.Done = (rsp) => { RecvGetRoomList(rsp.Content as ProtocolBytes); };
        msg.Send();

        msg = new Message(MsgType.Team_GetAchive, this);
        msg.Done = (rsp) => { RecvGetAchieve(rsp.Content as ProtocolBytes); }; ;
        msg.Send();
    }
    #endregion


    #region Team List Panel
    //收到GetAchieve协议:战绩信息
    public void RecvGetAchieve(ProtocolBase protocol)
    {
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int win = proto.GetInt(start, ref start);
        int lost = proto.GetInt(start, ref start);
        //处理
        IDText.text = "指挥官：" + m_strMainRoleGUID;
        WinText.text = win.ToString();
        LostText.text = lost.ToString();
    }

    //收到GetRoomList协议：房间列表
    public void RecvGetRoomList(ProtocolBase protocol)
    {
        //清理
        Utils.RemoveChildren(Content);
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int count = proto.GetInt(start, ref start);
        for (int i = 0; i < count; i++)
        {
            int num = proto.GetInt(start, ref start);
            int status = proto.GetInt(start, ref start);
            GenerateRoomUnit(i, num, status);
        }
    }

    //创建一个房间单元
    //参数 i，房间序号（从0开始）
    //参数num，房间里的玩家数
    //参数status，房间状态，1-准备中 2-战斗中
    public void GenerateRoomUnit(int i, int num, int status)
    {
        //添加房间
        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (i + 1) * 110);
        GameObject o = Instantiate(RoomPrefab.gameObject);
        o.transform.SetParent(Content.transform);
        o.SetActive(true);
        //房间信息
        Transform trans = o.transform;
        Text nameText = trans.Find("nameText").GetComponent<Text>();
        Text countText = trans.Find("CountText").GetComponent<Text>();
        Text statusText = trans.Find("StatusText").GetComponent<Text>();
        nameText.text = "序号：" + (i + 1).ToString();
        countText.text = "人数：" + num.ToString();
        if (status == 1)
        {
            statusText.color = Color.black;
            statusText.text = "状态：准备中";
        }
        else
        {
            statusText.color = Color.red;
            statusText.text = "状态：开战中";
        }
        //按钮事件
        Button btn = trans.Find("JoinButton").GetComponent<Button>();
        btn.name = i.ToString();   //改变按钮的名字，以便给OnJoinBtnClick传参
        btn.onClick.AddListener(() =>
        {
            OnJoinBtnClick(btn.name);
        }
        );
    }

    //加入按钮：加入房间
    public void OnJoinBtnClick(string name)
    {
        Message msg = new Message(MsgType.Team_JoinTeam, this);
        msg["id"] = int.Parse(name);
        msg.Done = JoinTeam;
        msg.Send();
    }

    private void JoinTeam(Message _msg)
    {
        bool bSuc = (bool)_msg.Content;
        if (bSuc) ShowRoomPanel();
        else LogicUtils.Instance.OnAlert("进入房间失败");
    }

    private void NewTeam(Message _msg)
    {
        bool bSuc = (bool)_msg.Content;
        if (bSuc) ShowRoomPanel();
        else LogicUtils.Instance.OnAlert("新建房间失败");
    }
    #endregion


    #region Room Panel

    /// <summary>
    /// Show My Join Room
    /// </summary>
    private void ShowRoomPanel()
    {
        RoomPanel.gameObject.SetActive(true);
        Message msg = new Message(MsgType.Team_GetTeamInfo,this);
        msg.Done = p => { RecvGetRoomInfo(p.Content as ProtocolBytes); };
        msg.Send();
    }

    /// <summary>
    /// Get My Join Room Information
    /// </summary>
    /// <param name="protocol"></param>
    public void RecvGetRoomInfo(ProtocolBase protocol)
    {
        //获取总数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int count = proto.GetInt(start, ref start);
        //每个处理
        int i = 0;
        for (i = 0; i < count; i++)
        {
            string id = proto.GetString(start, ref start);
            int team = proto.GetInt(start, ref start);
            int win = proto.GetInt(start, ref start);
            int fail = proto.GetInt(start, ref start);
            int isOwner = proto.GetInt(start, ref start);
            //信息处理
            Transform trans = m_pRooms[i];
            Text text = trans.Find("Text").GetComponent<Text>();
            string str = "名字：" + id + "\r\n";
            str += "阵营：" + (team == 1 ? "红" : "蓝") + "\r\n";
            str += "胜利：" + win.ToString() + "   ";
            str += "失败：" + fail.ToString() + "\r\n";
            if (id == m_strMainRoleGUID)
                str += "【我自己】";
            if (isOwner == 1)
                str += "【房主】";
            text.text = str;

            if (team == 1)
                trans.GetComponent<Image>().color = Color.red;
            else
                trans.GetComponent<Image>().color = Color.blue;
        }

        for (; i < 6; i++)
        {
            Transform trans = m_pRooms[i];
            Text text = trans.Find("Text").GetComponent<Text>();
            text.text = "【等待玩家】";
            trans.GetComponent<Image>().color = Color.gray;
        }
    }
    
    /// <summary>
    /// Quite Team
    /// </summary>
    /// <param name="protocol"></param>
    public void OnCloseTeam(bool _bSun)
    {
        if (_bSun)
        {
            LogicUtils.Instance.OnAlert("退出成功");
            RoomPanel.gameObject.SetActive(false);
        }
        else
        {
            LogicUtils.Instance.OnAlert("退出失败");
        }
    }
    
    /// <summary>
    /// Start Fighting Call Back
    /// </summary>
    /// <param name="protocol"></param>
    public void OnStartBack(ProtocolBase protocol)
    {
        //获取数值
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        //处理
        if (ret != 0)
        {
            LogicUtils.Instance.OnAlert("开始游戏失败！两队至少都需要一名玩家，只有队长可以开始战斗！");
        }
    }
    #endregion
}
