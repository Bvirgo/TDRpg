using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFrameWork;
using UnityEngine.UI;
public class BattleView : BaseUI {
    #region UI
    [HideInInspector, AutoUGUI]
    public Button Btn_Base;
    [HideInInspector, AutoUGUI]
    public Button Btn_Skill_1;
    [HideInInspector, AutoUGUI]
    public Button Btn_Skill_2;
    [HideInInspector, AutoUGUI]
    public Button Btn_Skill_3;
    [HideInInspector, AutoUGUI]
    public Slider Bar_HeadHp;
    [HideInInspector, AutoUGUI]
    public Image Spr_Head;
    [HideInInspector, AutoUGUI]
    public Text Txt_ComboCount;
    [HideInInspector, AutoUGUI]
    public Transform Combo;
    [HideInInspector, AutoUGUI]
    public Transform UIGameOver;
    [HideInInspector, AutoUGUI]
    public Button Btn_Over;
    #endregion

    #region Containers
    private int m_skillPos;
    #endregion

    #region Init & Reigster
    public override UIType GetUIType()
    {
        return UIType.Battle;
    }

    protected override void OnReady()
    {
        base.OnReady();
    }

    protected override void Register()
    {
        base.Register();

        RegisterMsg(MsgType.Role_UpdateSkillCD,UpdateSkillCD);
    }

    protected override void InitContainer()
    {
        base.InitContainer();
        m_skillPos = 0;
    }

    protected override void InitUI()
    {
        base.InitUI();

        ResetUI();

        Btn_Base.onClick.AddListener(()=> 
        {
            m_skillPos = 0;
            PressSkillBtn();
        });

        Btn_Skill_1.onClick.AddListener(()=> 
        {
            m_skillPos = 1;
            PressSkillBtn();
        });
        Btn_Skill_2.onClick.AddListener(() =>
        {
            m_skillPos = 2;
            PressSkillBtn();
        });
        Btn_Skill_3.onClick.AddListener(() =>
        {
            m_skillPos = 3;
            PressSkillBtn();
        });
    }

    private void ResetUI()
    {
        SetSprFillAmount(Btn_Skill_1,0);
        SetSprFillAmount(Btn_Skill_2, 0);
        SetSprFillAmount(Btn_Skill_3, 0);
    }
    #endregion

    #region MyRegion
    private void PressSkillBtn()
    {
        Message msg = new Message(MsgType.Role_PressSkillBtn, this);
        msg["pos"] = m_skillPos;
        msg.Send();
    }

    private void UpdateSkillCD(Message _msg)
    {
        Button btn = Btn_Base;
        int nPos = (int)_msg["pos"];
        float fPro = (float)_msg["progress"];
        switch (nPos)
        {
            case 1:
                btn = Btn_Skill_1; 
                break;
            case 2:
                btn = Btn_Skill_2;
                break;
            case 3:
                btn = Btn_Skill_3;
                break;
            default:
                break;
        }

        SetSprFillAmount(btn,fPro);
    }

    private void SetSprFillAmount(Button _btn,float _fValue)
    {
        Image spr = _btn.transform.Find("Spr_Fill").GetComponent<Image>();
        spr.fillAmount = _fValue;
    }
    #endregion

}
