using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;
using System;

public class VillageMainView : BaseUI {
    #region UI
    [HideInInspector, AutoUGUI]
    public Button Btn_Head;

    [HideInInspector, AutoUGUI]
    public Slider Bg_Hp;

    [HideInInspector, AutoUGUI]
    public Slider Bg_PL;

    [HideInInspector, AutoUGUI]
    public Button Btn_AddHp;

    [HideInInspector, AutoUGUI]
    public Button Btn_AddPL;

    [HideInInspector, AutoUGUI]
    public Button Btn_AddGold;

    [HideInInspector, AutoUGUI]
    public Button Btn_AddGem;

    [HideInInspector, AutoUGUI]
    public Button Btn_Setting;

    [HideInInspector, AutoUGUI]
    public Button Btn_Shop;

    [HideInInspector, AutoUGUI]
    public Button Btn_Skill;

    [HideInInspector, AutoUGUI]
    public Button Btn_Qust;

    [HideInInspector, AutoUGUI]
    public Button Btn_Bag;

    [HideInInspector, AutoUGUI]
    public Button Btn_Fight;

    [HideInInspector, AutoUGUI]
    public Text Txt_Level;

    [HideInInspector, AutoUGUI]
    public Text Txt_Hp;

    [HideInInspector, AutoUGUI]
    public Text Txt_PL;

    [HideInInspector, AutoUGUI]
    public Text Txt_Name;

    [HideInInspector, AutoUGUI]
    public Text Txt_Gold;

    [HideInInspector, AutoUGUI]
    public Text Txt_Gem;
    #endregion

    #region Container
    private int m_nGold;
    private int m_nGem;
    private int m_nLevel;
    private int m_nPL;
    private int m_nHp;
    private string m_strRoleName;
    private bool m_bInit;
    #endregion

    #region Init & Register
    public override UIType GetUIType()
    {
        return UIType.Village;
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
    }

    protected override void InitUI()
    {
        Btn_Head.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenSubPanel(UIType.SubRoleInfo, root, true);
        });

        Btn_Bag.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenSubPanel(UIType.SubPackage, root, true);
        });

        Btn_Qust.onClick.AddListener(()=> 
        {
            UIManager.Instance.OpenSubPanel(UIType.SubTask, root, true);
        });

        Btn_Fight.onClick.AddListener(() => 
        {
            UIManager.Instance.OpenSubPanel(UIType.SubTeamPanel,root,true);
        });
    }
    #endregion

    #region Role Inforamtion
    private void RefreshRoleInfo(Message _msg)
    {
        m_nHp = (int)_msg["hp"];
        m_nPL = (int)_msg["pl"];
        m_nGold = (int)_msg["gold"];
        m_nGem = (int)_msg["gem"];
        m_nLevel = (int)_msg["lv"];
        m_strRoleName = _msg["name"].ToString();

        m_nPL = m_nPL > 100 ? 100 : m_nPL;
        m_nHp = m_nHp > 100 ? 100 : m_nHp;

        m_nPL = m_nPL < 0 ? 1 : m_nPL;
        m_nHp = m_nHp < 0 ? 1 : m_nHp;

        Txt_Gem.text = m_nGem.ToString();
        Txt_Gold.text = m_nGold.ToString();
        Txt_Hp.text = m_nHp.ToString();
        Txt_Level.text = m_nLevel.ToString();
        Txt_Name.text = m_strRoleName;
        Txt_PL.text = m_nPL.ToString();

        Text txt_hpInfo = Bg_Hp.transform.Find("Txt_Info").GetComponent<Text>();
        txt_hpInfo.text = m_nHp.ToString() + "/ 100";
        Bg_Hp.value = m_nHp / 100.0f;

        Text txt_plInfo = Bg_PL.transform.Find("Txt_Info").GetComponent<Text>();
        txt_hpInfo.text = m_nPL.ToString() + "/ 100";
        Bg_PL.value = m_nPL / 100.0f;
    }

    private void GetRoleInfo()
    {
        Message msg = new Message(MsgType.Role_GetRoleInfo, this);
        msg.Send();
    }
    #endregion

}
