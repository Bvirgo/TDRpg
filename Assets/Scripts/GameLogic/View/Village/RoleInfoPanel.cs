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
    private string m_strRoleName;

    #endregion
    public override UIType GetUIType()
    {
        return UIType.SubRoleInfo;
    }

    protected override void OnStart()
    {
        base.OnStart();

        InitUI();

        RegisterMsg();

        GetRoleInfo();

    }

    private void GetRoleInfo()
    {
        Message msg = new Message(MsgType.Role_GetRoleInfo, this);
        msg.Send();
    }

    public override void OnShow()
    {
        base.OnShow();

    }

    public override void OnHide()
    {
        base.OnHide();

    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

    }

    protected override void OnRelease()
    {
        base.OnRelease();

        UnRegisterMsg();
    }

    private void RegisterMsg()
    {

        MessageCenter.Instance.AddListener(MsgType.Role_RefreshRoleInfo, RefreshRoleInfo);
    }

    private void UnRegisterMsg()
    {

        MessageCenter.Instance.RemoveListener(MsgType.Role_RefreshRoleInfo, RefreshRoleInfo);
    }

    private void InitUI()
    {
        Btn_Close.onClick.AddListener(()=>
        {
            UIManager.Instance.HideSubPanle(parent, UIType.SubRoleInfo);
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
    }

}
