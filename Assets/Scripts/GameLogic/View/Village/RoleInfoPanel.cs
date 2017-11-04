using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;

public class RoleInfoPanel : BasePanel
{
    #region UI
    [HideInInspector,AutoUGUI]
    public Text Txt_Gold;

    [HideInInspector, AutoUGUI]
    public Text Txt_Gem;

    [HideInInspector, AutoUGUI]
    public Text Txt_Name;

    [HideInInspector, AutoUGUI]
    public Text Txt_Attack;

    [HideInInspector,AutoUGUI]
    public Text Txt_Level;

    [HideInInspector, AutoUGUI]
    public Text Txt_PL;

    [HideInInspector, AutoUGUI]
    public Text Txt_HP;

    [HideInInspector, AutoUGUI]
    public Text Txt_Exp;

    [HideInInspector, AutoUGUI]
    public Text Txt_ReTime;

    [HideInInspector, AutoUGUI]
    public Text Txt_ReAllTime;

    [HideInInspector, AutoUGUI]
    public Text Txt_PL_ReTime;

    [HideInInspector, AutoUGUI]
    public Text Txt_PL_ReAllTime;

    [HideInInspector, AutoUGUI]
    public Button Btn_ChangeName;

    [HideInInspector, AutoUGUI]
    public Button Btn_Close;

    [HideInInspector, AutoUGUI]
    public Slider Bg_Exp;

    #endregion

    #region Container
    private int m_nGold;
    private int m_nGem;
    private int m_nLevel;
    private int m_nPL;
    private int m_nHp;
    private int m_nAttack;
    private int m_nExp;
    private string m_strRoleName;

    private int m_nPLTimer ;
    private int m_nHPTimer;

    #endregion
    public override UIType GetUIType()
    {
        return UIType.SubRoleInfo;
    }
    
    protected override void OnReady()
    {
        base.OnReady();

        GetRoleInfo();
    }

    protected override void Register()
    {
        base.Register();

        RegisterMsg(MsgType.Role_RefreshRoleInfo, RefreshRoleInfo);
        RegisterMsg(MsgType.Role_RefreshTimer, RefreshTimer);
    }

    private void GetRoleInfo()
    {
        Message msg = new Message(MsgType.Role_GetRoleInfo, this);
        msg.Send();
    }
    

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        UpdateEnergyAndToughenShow();
    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }

    protected override void InitContainer()
    {
        m_nPL = 0;
        m_nHp = 0;
        m_nPLTimer = 0;
        m_nHPTimer = 0;
    }

    protected override void InitUI()
    {
        Btn_Close.onClick.AddListener(()=>
        {
            UIManager.Instance.HideSubPanel(UIType.SubRoleInfo);
        });
    }

    private void RefreshRoleInfo(Message _msg)
    {
        m_nHp = (int)_msg["hp"];
        m_nPL = (int)_msg["pl"];
        m_nGold = (int)_msg["gold"];
        m_nGem = (int)_msg["gem"];
        m_nLevel = (int)_msg["lv"];
        m_nAttack = (int)_msg["attack"];
        m_nPLTimer = (int)_msg["plTimer"];
        m_nHPTimer = (int)_msg["hpTimer"];
        m_nExp = (int)_msg["exp"];
        m_strRoleName = _msg["name"].ToString();

        m_nPL = m_nPL > 100 ? 100 : m_nPL;
        m_nHp = m_nHp > 100 ? 100 : m_nHp;

        m_nPL = m_nPL < 0 ? 1 : m_nPL;
        m_nHp = m_nHp < 0 ? 1 : m_nHp;

        Txt_Gem.text = m_nGem.ToString();
        Txt_Gold.text = m_nGold.ToString();
        Txt_HP.text = m_nHp.ToString();
        Txt_Level.text = m_nLevel.ToString();
        Txt_Name.text = m_strRoleName;
        Txt_PL.text = m_nPL.ToString();
        Txt_Attack.text = m_nAttack.ToString();
        Bg_Exp.maxValue = 100;
        Bg_Exp.value = m_nExp / 100;
    }

    private void RefreshTimer(Message _msg)
    {
        m_nPLTimer = (int)_msg["plTimer"];
        m_nHPTimer = (int)_msg["hpTimer"];
    }

    void UpdateEnergyAndToughenShow()
    {
        if (m_nHp >= 100)
        {
            Txt_ReTime.text = "00:00:00";
            Txt_ReAllTime.text = "00:00:00";
        }
        else
        {
            int remainTime = 60 - m_nHPTimer;
            string str = remainTime <= 9 ? "0" + remainTime : remainTime.ToString();
            Txt_ReTime.text = "00:00:" + str;

            //首先总的体力为100 其中一个体力是在最后的00表示 
            int minutes = 99 - m_nHp;
            int hours = minutes / 60;
            minutes = minutes % 60;
            string hoursStr = hours <= 9 ? "0" + hours : hours.ToString();
            string minutesStr = minutes <= 9 ? "0" + minutes : minutes.ToString();
            Txt_ReAllTime.text = hoursStr + ":" + minutesStr + ":" + str;
        }

        if (m_nPL >= 50)
        {
            Txt_PL_ReTime.text = "00:00:00";
            Txt_PL_ReAllTime.text = "00:00:00";
        }
        else
        {
            int remainTime = 60 - m_nPLTimer;
            string str = remainTime <= 9 ? "0" + remainTime : remainTime.ToString();
            Txt_PL_ReTime.text = "00:00:" + str;

            //首先总的历练为50 最后的两个零使用了一个历练
            int minutes = 49 - m_nPL;
            int hours = minutes / 60;
            minutes = minutes % 60;
            string hoursStr = hours <= 9 ? "0" + hours : hours.ToString();
            string minutesStr = minutes <= 9 ? "0" + minutes : minutes.ToString();
            Txt_PL_ReAllTime.text = hoursStr + ":" + minutesStr + ":" + str;
        }
    }

}
